using System;
using System.Drawing;
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
            cbMostrarSenha.Text = "Mostrar senha"; // texto inicial do checkbox
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

        #region Login

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string usuario = txtEmail.Text.Trim();
            string senha = txtSenha.Text;

            // Validação de campos vazios
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Campos obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (string.IsNullOrEmpty(usuario)) txtEmail.Focus();
                else txtSenha.Focus();
                return;
            }

            // Autenticação simulada
            if (usuario == "admin" && senha == "123")
            {
                cancelar = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Acesso negado", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Clear();
                txtSenha.Clear();
                txtEmail.Focus();
            }
        }

        #endregion

        #region Mostrar/Ocultar Senha

        private void cbMostrarSenha_CheckedChanged(object sender, EventArgs e)
        {
            bool mostrar = cbMostrarSenha.Checked;
            txtSenha.UseSystemPasswordChar = !mostrar;
            cbMostrarSenha.Text = mostrar ? "Ocultar senha" : "Mostrar senha";
        }

        #endregion

    }
}
