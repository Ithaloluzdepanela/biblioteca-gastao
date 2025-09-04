using BibliotecaApp.Forms.Utils;
using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BibliotecaApp.Utils;

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

        //Fecharo form se o login for cancelado externamente
        private void LoginForm_Load(object sender, EventArgs e)
        {
            AppPaths.EnsureFolders();
            if (cancelar == true) { this.Close(); }

            
        }
        #endregion

        #region Eventos de Saída
        private void picExit_Click(object sender, EventArgs e)
        {
            const string msg = "Tem certeza de que quer fechar a Aplicação?";
            const string box = "Confirmação de Encerramento";
            var confirma = MessageBox.Show(msg, box, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirma == DialogResult.Yes)
            {
                Application.Exit();
            }
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

                    //Retirar comentarios para ativar o envio semanal do relatorio!!!


                    //---- Envio Semanal Relatorio ----


                    if (!ControleSemanal.JaEnviadoEstaSemana())
                    {
                        //                        try
                        //                        {
                        //                            progressForm.AtualizarProgresso(90, "Gerando relatório semanal...");

                        //                            using (var conexao = Conexao.ObterConexao())
                        //                            {
                        //                                conexao.Open();
                        //                                string pdfPath = GerarRelatorioAtrasados(conexao);

                        //                                progressForm.AtualizarProgresso(95, "Enviando relatório para secretaria...");

                        //                                string assunto = $"📑 Relatório semanal de alunos não aptos - {DateTime.Now:dd/MM/yyyy}";
                        //                                string corpo = $@"
                        //<html>
                        //<body style='font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;'>
                        //    <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
                        //        <h2 style='color: #2c3e50;'>📚 Biblioteca Monteiro Lobato</h2>
                        //        <p>Prezada secretaria,</p>
                        //        <p>Segue em anexo o relatório semanal de alunos que <strong>não estão aptos</strong> a retirar documentos,
                        //           devido a <span style='color:#d35400; font-weight:bold;'>empréstimos em atraso</span>.</p>
                        //        <p style='font-size: 16px;'><strong>📅 Data do relatório:</strong> {DateTime.Now:dd/MM/yyyy}</p>
                        //        <p>O PDF anexo contém a lista de alunos e suas respectivas turmas.</p>
                        //        <p style='margin-top:20px;'>Atenciosamente,<br/><strong>Sistema da Biblioteca</strong></p>
                        //        <hr />
                        //        <p style='font-size: 13px; color: #888;'>Este é um e-mail automático. Não responda a esta mensagem.</p>
                        //    </div>
                        //</body>
                        //</html>";

                        //                                EmailService.Enviar(
                        //                                    "secretaria.79448@gmail.com", 
                        //                                    assunto,
                        //                                    corpo,
                        //                                    pdfPath
                        //                                );

                        //                                // Registra envio no TXT
                        //                                ControleSemanal.RegistrarEnvio();

                        //                                progressForm.AtualizarProgresso(100, "Relatório semanal enviado com sucesso!");
                        //                            }
                        //                        }
                        //                        catch
                        //                        {
                        //                            progressForm.AtualizarProgresso(100, "Falha ao enviar relatório.");
                        //                        }
                    }
                });
            }
        }


        // ---- PDF Relatorio Gerador ----
        private string GerarRelatorioAtrasados(SqlCeConnection conexao)
        {
            string caminho = Path.Combine(Application.StartupPath, $"Relatorio_Atrasados_{DateTime.Now:yyyyMMdd}.pdf");

            using (var fs = new FileStream(caminho, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Título
                var titulo = new iTextSharp.text.Paragraph(
                    "Relatório de Alunos NÃO Aptos a Retirar Documentos\n\n",
                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY)
                )
                { Alignment = iTextSharp.text.Element.ALIGN_CENTER };
                doc.Add(titulo);

                var data = new iTextSharp.text.Paragraph(
                    $"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}\n\n",
                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.ITALIC, new iTextSharp.text.BaseColor(128, 128, 128))
                )
                { Alignment = iTextSharp.text.Element.ALIGN_RIGHT };
                doc.Add(data);

                // Tabela (Nome, Turma)
                PdfPTable tabela = new PdfPTable(2) { WidthPercentage = 100 };
                tabela.SetWidths(new float[] { 3, 2 });

                string[] headers = { "Nome", "Turma" };
                foreach (var h in headers)
                {
                    var cell = new PdfPCell(new Phrase(h, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.WHITE)))
                    {
                        BackgroundColor = new BaseColor(30, 61, 88),
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                    tabela.AddCell(cell);
                }

                string sql = @"SELECT Nome, Turma FROM Usuarios u
                       WHERE EXISTS (
                           SELECT 1 FROM Emprestimo e
                           WHERE e.Alocador = u.Id AND e.Status = 'Atrasado'
                       )";

                using (var cmd = new SqlCeCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tabela.AddCell(reader["Nome"].ToString());
                        tabela.AddCell(reader["Turma"].ToString());
                    }
                }

                doc.Add(tabela);
                doc.Close();
            }

            return caminho;
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

                        string emailUsuario = reader.IsDBNull(6) ? null : reader.GetString(6);
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
                            if (!string.IsNullOrWhiteSpace(emailUsuario))
                            {
                                EnviarEmailLembrete(emailUsuario, nomeUsuario, tituloLivro, dataReferencia);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"Usuário '{nomeUsuario}' sem e-mail cadastrado para lembrete de empréstimo.");
                            }

                            var updateFlagCmd = new SqlCeCommand("UPDATE Emprestimo SET NotificadoLembrete = 1 WHERE Id = @Id", connection);
                            updateFlagCmd.Parameters.AddWithValue("@Id", id);
                            updateFlagCmd.ExecuteNonQuery();
                        }

                        if (diferenca.Days < 0 && !notificadoAtraso)
                        {
                            if (!string.IsNullOrWhiteSpace(emailUsuario))
                            {
                                EnviarEmailAtraso(emailUsuario, nomeUsuario, tituloLivro, dataReferencia);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"Usuário '{nomeUsuario}' sem e-mail cadastrado para notificação de atraso.");
                            }

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

                            if (reserva.Status == "Pendente" && reserva.DataDisponibilidade.Date <= hoje)
                            {
                                reservasParaAtualizar.Add(new { Id = reserva.Id, NovoStatus = "Disponível" });

                                if (reserva.DataDisponibilidade.Date == hoje)
                                {
                                    reservasParaEmail.Add(reserva);
                                }
                            }
                            else if (reserva.Status == "Disponível" && reserva.DataLimiteRetirada.Date < hoje)
                            {
                                reservasParaAtualizar.Add(new { Id = reserva.Id, NovoStatus = "Expirada" });
                                livrosParaLiberar.Add(reserva.LivroId);
                            }
                        }
                    }

                    progressForm.AtualizarProgresso(25, "Processando reservas...");

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
                                System.Diagnostics.Debug.WriteLine($"Usuário '{reserva.NomeUsuario}' sem e-mail cadastrado ou e-mail inválido para reserva.");
                            }
                        }
                        catch (Exception emailEx)
                        {
                            emailsFalharam++;
                            System.Diagnostics.Debug.WriteLine($"Erro ao processar email: {emailEx.Message}");
                        }
                    }

                    progressForm.AtualizarProgresso(75, "Liberando livros expirados...");

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


        #region ControleSemanal (TXT)
        public static class ControleSemanal
        {
            private static readonly string txtPath = Path.Combine(AppPaths.AppDataFolder, "EnvioRelatorioSemanal.txt");


            public static bool JaEnviadoEstaSemana()
            {
                if (!File.Exists(txtPath)) return false;

                string conteudo = File.ReadAllText(txtPath);
                if (DateTime.TryParse(conteudo, out DateTime ultimaData))
                {
                    var diff = (DateTime.Now - ultimaData).TotalDays;
                    return diff < 7; // menos de 7 dias = já enviou
                }
                return false;
            }

            public static void RegistrarEnvio()
            {
                File.WriteAllText(txtPath, DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }
        #endregion

        private void lblEsqueceuSenha_Click(object sender, EventArgs e)
        {
            this.Hide(); // Esconde o formulário atual

            using (EsqueceuSenhaForm popup = new EsqueceuSenhaForm())
            {
                popup.ShowDialog(); // Abre como modal
            }

            this.Show(); // Reexibe o formulário anterior após o fechamento do modal
        }

        

        private void lblVersion_Click(object sender, EventArgs e)
        {
            this.Hide();

            using (AboutForm popup = new AboutForm())
            {
                popup.ShowDialog(); // Abre como modal
            }

            this.Show();
        }

        private void lblVersion_MouseEnter(object sender, EventArgs e)
        {
            lblVersion.ForeColor = Color.SkyBlue;
        }

        private void lblVersion_MouseLeave(object sender, EventArgs e)
        {lblVersion.ForeColor = Color.White;

        }
    }
}