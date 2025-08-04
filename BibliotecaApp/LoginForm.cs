using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using System;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp
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

                    string query = @"SELECT Nome, SenhaHash, SenhaSalt FROM usuarios 
    WHERE Email = @email AND TipoUsuario = 'Bibliotecário(a)'";

                    using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@email", email);

                        using (SqlCeDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hashSalvo = reader["SenhaHash"].ToString();
                                string saltSalvo = reader["SenhaSalt"].ToString();
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

        #region update empréstimos
        private async Task AtualizarStatusEmprestimosAsync()
        {
            using (var progressForm = new frmProgresso())
            {
                progressForm.Show();

                await Task.Run(() =>
                {
                    AtualizarEmprestimos(progressForm);
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

                    // Contar empréstimos ativos
                    string countQuery = "SELECT COUNT(*) FROM Emprestimo WHERE Status <> 'Devolvido'";
                    var countCommand = new SqlCeCommand(countQuery, connection);
                    int totalEmprestimos = (int)countCommand.ExecuteScalar();

                    if (totalEmprestimos == 0)
                    {
                        progressForm.AtualizarProgresso(100, "Nenhum empréstimo para atualizar");
                        return;
                    }

                    // Obter empréstimos ativos
                    string selectQuery = @"SELECT Id, DataDevolucao, DataProrrogacao, DataRealDevolucao 
                                 FROM Emprestimo
                                 WHERE Status <> 'Devolvido'";
                    var selectCommand = new SqlCeCommand(selectQuery, connection);
                    var reader = selectCommand.ExecuteReader();

                    int processados = 0;

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        DateTime dataDevolucao = reader.GetDateTime(1);
                        DateTime? dataProrrogacao = reader.IsDBNull(2) ? null : (DateTime?)reader.GetDateTime(2);
                        DateTime? dataRealDevolucao = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3);

                        string novoStatus = CalcularStatus(dataDevolucao, dataProrrogacao, dataRealDevolucao);

                        // Atualizar o status
                        string updateQuery = "UPDATE Emprestimo SET Status = @Status WHERE Id = @Id";
                        var updateCommand = new SqlCeCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@Status", novoStatus);
                        updateCommand.Parameters.AddWithValue("@Id", id);
                        updateCommand.ExecuteNonQuery();

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