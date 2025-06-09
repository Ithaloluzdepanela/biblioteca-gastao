using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp
{
    public partial class LoginForm : Form
    {
        LoginForm login;
        public LoginForm()
        {
            InitializeComponent();
        }
        // teste de commit
        public static bool cancelar = false;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Btn_Login_Click(object sender, EventArgs e)
        {
            if (Txt_Email.Text =="Admin" && Txt_Senha.Text =="1234")
            {
               cancelar = true;
               Close();
            }
            else
            {
                MessageBox.Show("Acesso negado");
                Txt_Email.Text = "";
                Txt_Senha.Text = "";
                Txt_Email.Focus();
            }

        }
        #region Exit

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
    }
}
