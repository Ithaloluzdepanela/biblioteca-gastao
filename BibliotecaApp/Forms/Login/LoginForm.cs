using BibliotecaApp.Forms.Livros;
using BibliotecaApp.Forms.Utils;
using BibliotecaApp.Models;
using BibliotecaApp.Services;
using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Login
{
    public partial class LoginForm : Form
    {
        #region Propriedades
        // Variável para controle externo de login
        public static bool cancelar = false;
        #endregion

        #region Construtor
        public LoginForm()
        {
            InitializeComponent();
            txtEmail.KeyDown += txtEmail_KeyDown;
            txtSenha.KeyDown += txtSenha_KeyDown;
        }
        #endregion

        #region Eventos de Saída
        private void picExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void picExit_MouseEnter(object sender, EventArgs e)
        {
            picExit.BackColor = Color.Gainsboro;
        }

        private void picExit_MouseLeave(object sender, EventArgs e)
        {
            picExit.BackColor = Color.Transparent;
        }
        #endregion

        #region Classe Conexao
        // Classe estática para conectar ao banco .sdf
        public static class Conexao
        {
            public static string CaminhoBanco => Application.StartupPath + @"\bibliotecaDB\bibliotecaDB.sdf";
            public static string Conectar => $"Data Source={CaminhoBanco}; Password=123";

            public static SqlCeConnection ObterConexao()
            {
                return new SqlCeConnection(Conectar);
            }
        }
        #endregion

        #region Métodos de Login
        private async void BtnEntrar_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text;

            // login como administrador (provisório)
            if (email == "admin" && senha == "1234")
            {
                // Login como administrador
                cancelar = true;
                await AtualizarStatusEmprestimosAsync();
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Campos obrigatórios",
                              MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (string.IsNullOrEmpty(email)) txtEmail.Focus();
                else txtSenha.Focus();
                return;
            }

            try
            {
                using (SqlCeConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    string query = @"SELECT Nome, Senha_hash, Senha_salt FROM usuarios 
                    WHERE Email = @email AND TipoUsuario = 'Bibliotecário(a)'";

                    using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@email", email);

                        using (SqlCeDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hashSalvo = reader["Senha_hash"].ToString();
                                string saltSalvo = reader["Senha_salt"].ToString();
                                string nomeUsuario = reader["Nome"].ToString();

                                bool senhaCorreta = CriptografiaSenha.VerificarSenha(senha, hashSalvo, saltSalvo);

                                if (senhaCorreta)
                                {
                                    Sessao.NomeBibliotecariaLogada = nomeUsuario;

                                    await AtualizarStatusEmprestimosAsync();

                                    cancelar = true;
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show("Senha incorreta.", "Erro de autenticação",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtSenha.Focus();
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("E-mail não encontrado ou usuário sem permissão.", "Erro de autenticação",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtEmail.Focus();
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na autenticação: " + ex.Message, "Erro",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEntrar_MouseLeave(object sender, EventArgs e)
        {
            BtnEntrar.BackColor = Color.FromArgb(9, 74, 158);
            BtnEntrar.Refresh();
        }

        private void BtnEntrar_MouseEnter(object sender, EventArgs e)
        {
            BtnEntrar.BackColor = Color.FromArgb(33, 145, 245);
            BtnEntrar.Refresh();
        }
        #endregion

        #region Métodos de Atualização
        private async Task AtualizarStatusEmprestimosAsync()
        {
            using (var progressForm = new frmProgresso())
            {
                progressForm.Show();

                await Task.Run(() =>
                {
                    AtualizarEmprestimos(progressForm);
                    AtualizarReservas(progressForm);
                });
            }
        }

        private void AtualizarEmprestimos(frmProgresso progressForm)
        {
            try
            {
                using (var connection = Conexao.ObterConexao())
                {
                    connection.Open();

                    string countQuery = "SELECT COUNT(*) FROM Emprestimo WHERE Status <> 'Devolvido'";
                    var countCommand = new SqlCeCommand(countQuery, connection);
                    int totalEmprestimos = (int)countCommand.ExecuteScalar();

                    if (totalEmprestimos == 0)
                    {
                        progressForm.AtualizarProgresso(100, "Nenhum empréstimo para atualizar");
                        return;
                    }

                    string selectQuery = @"
                    SELECT 
                        e.Id,
                        e.DataDevolucao,
                        e.DataProrrogacao,
                        e.DataRealDevolucao,
                        e.NotificadoLembrete,
                        e.NotificadoAtraso,
                        u.Email,
                        u.Nome,
                        l.Nome
                    FROM Emprestimo e
                    INNER JOIN Usuarios u ON e.Alocador = u.Id
                    INNER JOIN Livros l ON e.Livro = l.Id
                    WHERE e.Status <> 'Devolvido'";

                    var selectCommand = new SqlCeCommand(selectQuery, connection);
                    var reader = selectCommand.ExecuteReader();

                    int processados = 0;

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        DateTime dataDevolucao = reader.GetDateTime(1);
                        DateTime? dataProrrogacao = reader.IsDBNull(2) ? null : (DateTime?)reader.GetDateTime(2);
                        DateTime? dataRealDevolucao = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3);

                        bool notificadoLembrete = !reader.IsDBNull(4) && reader.GetBoolean(4);
                        bool notificadoAtraso = !reader.IsDBNull(5) && reader.GetBoolean(5);

                        string emailUsuario = reader.GetString(6);
                        string nomeUsuario = reader.GetString(7);
                        string tituloLivro = reader.GetString(8);

                        string novoStatus = CalcularStatus(dataDevolucao, dataProrrogacao, dataRealDevolucao);

                        string updateStatusQuery = "UPDATE Emprestimo SET Status = @Status WHERE Id = @Id";
                        var updateStatusCommand = new SqlCeCommand(updateStatusQuery, connection);
                        updateStatusCommand.Parameters.AddWithValue("@Status", novoStatus);
                        updateStatusCommand.Parameters.AddWithValue("@Id", id);
                        updateStatusCommand.ExecuteNonQuery();

                        DateTime dataReferencia = dataProrrogacao ?? dataDevolucao;
                        TimeSpan diferenca = dataReferencia.Date - DateTime.Now.Date;

                        if (diferenca.Days == 3 && !notificadoLembrete)
                        {
                            EnviarEmailLembrete(emailUsuario, nomeUsuario, tituloLivro, dataReferencia);

                            var updateFlagCmd = new SqlCeCommand("UPDATE Emprestimo SET NotificadoLembrete = 1 WHERE Id = @Id", connection);
                            updateFlagCmd.Parameters.AddWithValue("@Id", id);
                            updateFlagCmd.ExecuteNonQuery();
                        }

                        if (diferenca.Days < 0 && !notificadoAtraso)
                        {
                            EnviarEmailAtraso(emailUsuario, nomeUsuario, tituloLivro, dataReferencia);

                            var updateFlagCmd = new SqlCeCommand("UPDATE Emprestimo SET NotificadoAtraso = 1 WHERE Id = @Id", connection);
                            updateFlagCmd.Parameters.AddWithValue("@Id", id);
                            updateFlagCmd.ExecuteNonQuery();
                        }

                        processados++;
                        int progresso = (int)((double)processados / totalEmprestimos * 100);
                        progressForm.AtualizarProgresso(progresso, $"Atualizando empréstimo {processados} de {totalEmprestimos}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar empréstimos: {ex.Message}", "Erro",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarReservas(frmProgresso progressForm)
        {
            try
            {
                using (var connection = Conexao.ObterConexao())
                {
                    connection.Open();

                    DateTime hoje = DateTime.Now.Date;

                    // Buscar TODAS as reservas que não estão finalizadas para processar no C#
                    string sqlBuscarReservas = @"
                SELECT r.Id, r.Status, r.DataDisponibilidade, r.DataLimiteRetirada, 
                       r.UsuarioId, r.LivroId,
                       u.Nome, u.Email, l.Nome, l.Autor
                FROM Reservas r
                INNER JOIN Usuarios u ON r.UsuarioId = u.Id
                INNER JOIN Livros l ON r.LivroId = l.Id
                WHERE r.Status IN ('Pendente', 'Disponível')";

                    var reservasParaAtualizar = new List<dynamic>();
                    var reservasParaEmail = new List<dynamic>();
                    var livrosParaLiberar = new List<int>();

                    // Primeiro, buscar todas as reservas
                    using (var cmd = new SqlCeCommand(sqlBuscarReservas, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reserva = new
                            {
                                Id = reader.GetInt32(0),
                                Status = reader.GetString(1),
                                DataDisponibilidade = reader.GetDateTime(2),
                                DataLimiteRetirada = reader.GetDateTime(3),
                                UsuarioId = reader.GetInt32(4),
                                LivroId = reader.GetInt32(5),
                                NomeUsuario = reader.GetString(6),
                                EmailUsuario = reader.IsDBNull(7) ? null : reader.GetString(7),
                                NomeLivro = reader.GetString(8),
                                AutorLivro = reader.GetString(9)
                            };

                            // Lógica de atualização baseada nas datas
                            if (reserva.Status == "Pendente" && reserva.DataDisponibilidade.Date <= hoje)
                            {
                                // Pendente → Disponível
                                reservasParaAtualizar.Add(new { Id = reserva.Id, NovoStatus = "Disponível" });

                                // Se ficou disponível hoje, enviar email
                                if (reserva.DataDisponibilidade.Date == hoje)
                                {
                                    reservasParaEmail.Add(reserva);
                                }
                            }
                            else if (reserva.Status == "Disponível" && reserva.DataLimiteRetirada.Date < hoje)
                            {
                                // Disponível → Expirada
                                reservasParaAtualizar.Add(new { Id = reserva.Id, NovoStatus = "Expirada" });
                                livrosParaLiberar.Add(reserva.LivroId);
                            }
                        }
                    }

                    progressForm.AtualizarProgresso(25, "Processando reservas...");

                    // Atualizar status das reservas
                    foreach (var reserva in reservasParaAtualizar)
                    {
                        string sqlUpdate = "UPDATE Reservas SET Status = @Status WHERE Id = @Id";
                        using (var cmdUpdate = new SqlCeCommand(sqlUpdate, connection))
                        {
                            cmdUpdate.Parameters.AddWithValue("@Status", reserva.NovoStatus);
                            cmdUpdate.Parameters.AddWithValue("@Id", reserva.Id);
                            cmdUpdate.ExecuteNonQuery();
                        }
                    }

                    progressForm.AtualizarProgresso(50, "Enviando emails de disponibilidade...");

                    // Enviar emails para reservas que ficaram disponíveis hoje
                    int emailsEnviados = 0;
                    int emailsFalharam = 0;

                    foreach (var reserva in reservasParaEmail)
                    {
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(reserva.EmailUsuario) &&
                                BibliotecaApp.Forms.Livros.ReservaForm.ValidarEmailStatic(reserva.EmailUsuario))
                            {
                                var usuario = new Usuarios { Nome = reserva.NomeUsuario, Email = reserva.EmailUsuario };
                                var livro = new Livro { Nome = reserva.NomeLivro, Autor = reserva.AutorLivro };

                                BibliotecaApp.Forms.Livros.ReservaForm.EnviarEmailDisponibilidadeStatic(
                                    usuario, livro, reserva.DataDisponibilidade, reserva.DataLimiteRetirada);

                                emailsEnviados++;
                                System.Diagnostics.Debug.WriteLine($"Email enviado para: {reserva.EmailUsuario}");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"Email inválido para usuário: {reserva.NomeUsuario}");
                            }
                        }
                        catch (Exception emailEx)
                        {
                            emailsFalharam++;
                            System.Diagnostics.Debug.WriteLine($"Erro ao processar email: {emailEx.Message}");
                        }
                    }

                    progressForm.AtualizarProgresso(75, "Liberando livros expirados...");

                    // Liberar livros cujas reservas expiraram
                    foreach (var livroId in livrosParaLiberar.Distinct())
                    {
                        string sqlLiberar = "UPDATE Livros SET Disponibilidade = 1 WHERE Id = @LivroId";
                        using (var cmdLiberar = new SqlCeCommand(sqlLiberar, connection))
                        {
                            cmdLiberar.Parameters.AddWithValue("@LivroId", livroId);
                            cmdLiberar.ExecuteNonQuery();
                        }
                    }

                    progressForm.AtualizarProgresso(100, "Atualização de reservas concluída!");

                    // Log dos resultados
                    if (emailsEnviados > 0 || emailsFalharam > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Processamento de emails - Enviados: {emailsEnviados}, Falhas: {emailsFalharam}");
                    }

                    System.Diagnostics.Debug.WriteLine($"Reservas atualizadas: {reservasParaAtualizar.Count}");
                    System.Diagnostics.Debug.WriteLine($"Livros liberados: {livrosParaLiberar.Distinct().Count()}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar reservas: {ex.Message}", "Erro",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Erro detalhado: {ex}");
            }
        }

        private string CalcularStatus(DateTime dataDevolucao, DateTime? dataProrrogacao, DateTime? dataRealDevolucao)
        {
            DateTime dataReferencia = dataProrrogacao ?? dataDevolucao;

            if (dataRealDevolucao.HasValue)
            {
                return "Devolvido";
            }
            else if (DateTime.Now > dataReferencia)
            {
                return "Atrasado";
            }
            else
            {
                return "Ativo";
            }
        }
        #endregion

        #region Métodos de Email
        private void EnviarEmailLembrete(string email, string nome, string tituloLivro, DateTime dataDevolucao)
        {
            string assunto = "📚 Lembrete: Devolução de livro se aproxima!";

            string corpo = $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;'>
                    <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
                        <h2 style='color: #2c3e50;'>Olá, {nome} 👋</h2>
                        <p>Este é um lembrete de que o livro <strong>{tituloLivro}</strong> precisa ser devolvido em breve.</p>
                        <p style='font-size: 16px;'><strong>📅 Data limite de devolução:</strong> {dataDevolucao:dd/MM/yyyy}</p>
                        <p>Por favor, devolva o livro no prazo para evitar bloqueios no sistema e problemas com a secretaria.</p>
                        <hr />
                        <p style='font-size: 14px; color: #888;'>Este é um e-mail automático enviado pela Biblioteca Monteiro Lobato.</p>
                    </div>
                </body>
                </html>";

            BibliotecaApp.Services.EmailService.Enviar(email, assunto, corpo);
        }

        private void EnviarEmailAtraso(string email, string nome, string tituloLivro, DateTime dataDevolucao)
        {
            string assunto = "⚠️ Atraso na devolução de livro";

            string corpo = $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333; background-color: #fffbe6; padding: 20px;'>
                    <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #e1a500; border-radius: 8px; padding: 20px;'>
                        <h2 style='color: #d35400;'>Atenção, {nome} ⚠️</h2>
                        <p>O prazo para devolução do livro <strong>{tituloLivro}</strong> venceu em <strong>{dataDevolucao:dd/MM/yyyy}</strong>.</p>
                        <p>Devido a isso, você <strong>não poderá retirar documentos na secretaria</strong> até regularizar a situação.</p>
                        <p>Pedimos que devolva o material o quanto antes ou entre em contato com a biblioteca.</p>
                        <hr />
                        <p style='font-size: 14px; color: #888;'>Este é um e-mail automático enviado pela Biblioteca Monteiro Lobato.</p>
                    </div>
                </body>
                </html>";

            BibliotecaApp.Services.EmailService.Enviar(email, assunto, corpo);
        }
        #endregion

        #region Eventos de Teclado
        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtSenha.Focus();
            }
        }

        private void txtSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnEntrar.PerformClick();
            }
        }
        #endregion

        #region Eventos de Interface
        private void gradientPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Implementação do paint do gradientPanel
        }

        private void gradientPanel1_Paint_1(object sender, PaintEventArgs e)
        {
            // Implementação alternativa do paint do gradientPanel
        }
        #endregion

        private void lblEsqueceuSenha_Click(object sender, EventArgs e)
        {
            EsqueceuSenhaForm popup = new EsqueceuSenhaForm();
            popup.ShowDialog(); // Abre como modal
        }
    }
}