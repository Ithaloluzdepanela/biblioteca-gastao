using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace BibliotecaApp
{
    public partial class LoginForm : Form
    {
        // Variável para controle externo de login
        public static bool cancelar = false;

        public LoginForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;


        }
        

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            // Navegação entre campos com Enter
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
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
        private void BtnEntrar_Click(object sender, EventArgs e)
        {

            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text;

            // login como administrador (provisório)
            if (email == "admin@admin.com" && senha == "1234")
            {
                // Login como administrador
                cancelar = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;

            }

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Campos obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (string.IsNullOrEmpty(email)) txtEmail.Focus();
                else txtSenha.Focus();
                return;
            }

            try
            {
                using (SqlCeConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    string query = @"SELECT * FROM usuarios 
                             WHERE email = @email AND senha = @senha AND tipousuario = 'Bibliotecário(a)'";

                    using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@email", email);
                        comando.Parameters.AddWithValue("@senha", senha);

                        using (SqlCeDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Login válido
                                cancelar = true;
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }



                            else
                            {
                                MessageBox.Show("Acesso negado. Verifique seu e-mail e senha.", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtEmail.Clear();
                                txtSenha.Clear();
                                txtEmail.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na autenticação: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion

        #region Mostrar/Ocultar Senha


        #endregion



        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}