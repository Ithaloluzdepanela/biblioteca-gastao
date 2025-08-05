using BibliotecaApp.Forms.Utils;
using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Login
{
    public partial class LoginForm : Form
    {
        // Variável para controle externo de login
        public static bool cancelar = false;

        public LoginForm()
        {
            InitializeComponent();
            txtEmail.KeyDown += txtEmail_KeyDown;
            txtSenha.KeyDown += txtSenha_KeyDown;


        }




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

        #region Login
        private async void BtnEntrar_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text;

            // login como administrador (provisório)
            if (email == "admin@admin.com" && senha == "1234")
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

        #region Mostrar/Ocultar Senha


        #endregion

        #region update Empréstimos
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
                SELECT e.Id, e.DataDevolucao, e.DataProrrogacao, e.DataRealDevolucao,
                       e.EmailLembreteEnviado, e.EmailAtrasoEnviado,
                       u.Email, u.Nome,
                       l.Titulo
                FROM Emprestimo e
                INNER JOIN Usuarios u ON e.UsuarioId = u.Id
                INNER JOIN Livros l ON e.LivroId = l.Id
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

                        bool emailLembreteEnviado = !reader.IsDBNull(4) && reader.GetBoolean(4);
                        bool emailAtrasoEnviado = !reader.IsDBNull(5) && reader.GetBoolean(5);

                        string emailUsuario = reader.GetString(6);
                        string nomeUsuario = reader.GetString(7);
                        string tituloLivro = reader.GetString(8);

                        string novoStatus = CalcularStatus(dataDevolucao, dataProrrogacao, dataRealDevolucao);

                        // Atualiza o status no banco
                        string updateStatusQuery = "UPDATE Emprestimo SET Status = @Status WHERE Id = @Id";
                        var updateStatusCommand = new SqlCeCommand(updateStatusQuery, connection);
                        updateStatusCommand.Parameters.AddWithValue("@Status", novoStatus);
                        updateStatusCommand.Parameters.AddWithValue("@Id", id);
                        updateStatusCommand.ExecuteNonQuery();

                        DateTime dataReferencia = dataProrrogacao ?? dataDevolucao;
                        TimeSpan diferenca = dataReferencia.Date - DateTime.Now.Date;

                        // Enviar email lembrete se faltarem 3 dias e ainda não enviou
                        if (diferenca.Days == 3 && !emailLembreteEnviado)
                        {
                            EnviarEmailLembrete(emailUsuario, nomeUsuario, tituloLivro, dataReferencia);

                            // Atualiza flag no banco
                            var updateFlagCmd = new SqlCeCommand("UPDATE Emprestimo SET EmailLembreteEnviado = 1 WHERE Id = @Id", connection);
                            updateFlagCmd.Parameters.AddWithValue("@Id", id);
                            updateFlagCmd.ExecuteNonQuery();
                        }

                        // Enviar email atraso se passou da data e não enviou ainda
                        if (diferenca.Days < 0 && !emailAtrasoEnviado)
                        {
                            EnviarEmailAtraso(emailUsuario, nomeUsuario, tituloLivro, dataReferencia);

                            // Atualiza flag no banco
                            var updateFlagCmd = new SqlCeCommand("UPDATE Emprestimo SET EmailAtrasoEnviado = 1 WHERE Id = @Id", connection);
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

        private void EnviarEmailLembrete(string email, string nome, string tituloLivro, DateTime dataDevolucao)
        {
            string assunto = "📚 Lembrete: Devolução de livro se aproxima!";

            string corpo = $@"
    <html>
    <body style='font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;'>
        <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
            <h2 style='color: #2c3e50;'>Olá, {nome} 👋</h2>
            <p>Este é um lembrete de que o livro <strong>\{tituloLivro}\</strong> precisa ser devolvido em breve.</p>
                    <p style = 'font-size: 16px;'><strong>📅 Data limite de devolução:</ strong > {dataDevolucao: dd / MM / yyyy}</ p >
        
                    < p > Por favor, devolva o livro no prazo para evitar bloqueios no sistema e problemas com a secretaria.</ p >
        
                    < hr />
        
                    < p style = 'font-size: 14px; color: #888;' > Este é um e - mail automático enviado pela Biblioteca Monteiro Lobato.</ p >
        
                </ div >
        
            </ body >
        
            </ html > ";

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
            <p>O prazo para devolução do livro <strong>\{tituloLivro}\</strong> venceu em <strong>{dataDevolucao:dd/MM/yyyy}</strong>.</p>
                    < p > Devido a isso, você < strong > não poderá retirar documentos na secretaria </ strong > até regularizar a situação.</ p >
        
                    < p > Pedimos que devolva o material o quanto antes ou entre em contato com a biblioteca.</ p >
        
                    < hr />
        
                    < p style = 'font-size: 14px; color: #888;' > Este é um e - mail automático enviado pela Biblioteca Monteiro Lobato.</ p >
        
                </ div >
        
            </ body >
        
            </ html > ";

            BibliotecaApp.Services.EmailService.Enviar(email, assunto, corpo);
        }


        private void AtualizarReservas(frmProgresso progressForm)
        {
            try
            {
                using (var connection = Conexao.ObterConexao())
                {
                    connection.Open();

                    // Buscar reservas com status 'Disponível' (usuário foi avisado e tem prazo para retirar)
                    string selectQuery = @"
                SELECT Id, DataLimiteRetirada
                FROM Reservas
                WHERE Status = 'Disponível'";

                    using (var selectCommand = new SqlCeCommand(selectQuery, connection))
                    using (var reader = selectCommand.ExecuteReader())
                    {
                        var reservasParaExpirar = new List<(int Id, DateTime? DataLimiteRetirada)>();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            DateTime? dataLimite = reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1);
                            reservasParaExpirar.Add((id, dataLimite));
                        }

                        int total = reservasParaExpirar.Count;
                        int processadas = 0;

                        foreach (var reserva in reservasParaExpirar)
                        {
                            // Se já passou da DataLimiteRetirada, expira a reserva
                            if (reserva.DataLimiteRetirada.HasValue && DateTime.Now > reserva.DataLimiteRetirada.Value)
                            {
                                using (var updateCmd = new SqlCeCommand("UPDATE Reservas SET Status = 'Expirada' WHERE Id = @Id", connection))
                                {
                                    updateCmd.Parameters.AddWithValue("@Id", reserva.Id);
                                    updateCmd.ExecuteNonQuery();
                                }
                            }

                            processadas++;
                            int progresso = (int)((double)processadas / total * 100);
                            progressForm.AtualizarProgresso(progresso, $"Verificando reservas ({processadas}/{total})");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar reservas: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void gradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}