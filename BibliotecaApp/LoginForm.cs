using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        //Variável para checagem de aba
        public static bool cancelar = false;
        
        private void Btn_Login_Click(object sender, EventArgs e)
        {
            if (Txt_Email.Text =="admin" && Txt_Senha.Text =="123")
            {
               cancelar = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
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
