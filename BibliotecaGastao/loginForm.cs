using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using gastao_Biblioteca;



namespace BibliotecaGastao
{
    public partial class loginForm : Form
    {
        mainForm main;
        loginForm login;

        public loginForm()
        {
            InitializeComponent();
        }



        private void Btn_Entrar_Click(object sender, EventArgs e)
        {
            if (Txt_Usuario.Text == "admin" && Txt_Senha.Text == "1234")
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuário ou senha inválidos!");
            }
        }

        private void Txt_Senha_Leave(object sender, EventArgs e)
        {
            if (Txt_Senha.Text == "")
            {
              Txt_Senha.UseSystemPasswordChar = false;
            }
        }

    }
    
}
