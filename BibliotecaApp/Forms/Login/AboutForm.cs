using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Login
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void picExit_Click(object sender, EventArgs e)
        {
            Form loginForm = Application.OpenForms["LoginForm"];

            if (loginForm != null)
            {
                this.Hide();
                loginForm.Show();
                loginForm.BringToFront();
            }
            else
            {
                this.Hide();
                LoginForm novoLogin = new LoginForm();
                novoLogin.Show();
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

        private void BtnOk_Click(object sender, EventArgs e)
        {
            Form loginForm = Application.OpenForms["LoginForm"];

            if (loginForm != null)
            {
                this.Hide();
                loginForm.Show();
                loginForm.BringToFront();
            }
            else
            {
                this.Hide();
                LoginForm novoLogin = new LoginForm();
                novoLogin.Show();
            }
        }
    }
}
