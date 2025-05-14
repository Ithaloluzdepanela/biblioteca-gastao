using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BibliotecaGastao;

namespace gastao_Biblioteca
{
    public partial class mainForm : Form
    {
        inicioForm inicio;
        empresForm empres;
        userForm user;
        findForm find;
        relForm relF;

        public mainForm()
        {
            InitializeComponent();
            mdiProp();
            inicio = new inicioForm();
            inicio.FormClosed += Inicio_FormClosed;
            inicio.MdiParent = this;
            inicio.Dock = DockStyle.Fill;
            inicio.Show();
            picTheme.BackgroundImage = BibliotecaGastao.Properties.Resources.icons8_símbolo_da_lua_16;
            theme = false;
        }

        //Imersão das telas dentro da aplicação
        private void mdiProp()
        {
            this.SetBevel(false);
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.FromArgb(232, 234, 237);
        }

        //Variantes Usadas
        bool bookExpand = false,
             menuExpand = false,
             theme = false;

        //Transição dos Livros
        private void book_Click(object sender, EventArgs e)
        {
            bookTransition.Start();
        }

        private void bookTransition_Tick(object sender, EventArgs e)
        {
            if (bookExpand == false)
            {
                bookContainer.Height += 10;
                if (menuExpand == false)
                {
                    menuTransition.Start();
                }
                if (bookContainer.Height >= 168)
                {
                    bookTransition.Stop();
                    bookExpand = true;
                }
            }
            else
            {
                bookContainer.Height -= 10;
                if (bookContainer.Height <= 56)
                {
                    bookTransition.Stop();
                    bookExpand = false;
                }

            }
        }

        //Transição de menu
        private void btnMenu_Click(object sender, EventArgs e)
        {
            menuTransition.Start();
        }
        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (menuExpand == false)
            {
                menu.Width += 10;
                if (menu.Width >= 200)
                {
                    menuTransition.Stop();
                    menuExpand = true;
                    pnHome.Width = menu.Width;
                    pnLogout.Width = menu.Width;
                    pnRel.Width = menu.Width;
                    pnUsers.Width = menu.Width;
                    bookContainer.Width = menu.Width;
                }

            }
            else
            {
                if (bookExpand == true)
                {
                    bookTransition.Start();
                }
                if (menuExpand == true)
                    menu.Width -= 10; if (menu.Width <= 38)
                {
                    menuTransition.Stop();
                    pnHome.Width = menu.Width;
                    pnLogout.Width = menu.Width;
                    pnRel.Width = menu.Width;
                    menuExpand = false;
                    pnUsers.Width = menu.Width;
                    bookContainer.Width = menu.Width;
                }

            }
        }

        //Mudança de tema
        private void picTheme_Click(object sender, EventArgs e)
        {
            if (theme == false)
            {
                picTheme.BackgroundImage = BibliotecaGastao.Properties.Resources.icons8_sol_16;
                if (inicio != null) { inicio.BackColor = ColorTranslator.FromHtml("#1a1a1a"); }
                if (user != null) { user.BackColor = ColorTranslator.FromHtml("#1a1a1a"); }
                if (empres != null) { empres.BackColor = ColorTranslator.FromHtml("#1a1a1a"); }
                if (find != null) { find.BackColor = ColorTranslator.FromHtml("#1a1a1a"); }
                if (relF != null) { relF.BackColor = ColorTranslator.FromHtml("#1a1a1a"); ; }
                theme = true;
            }
            else
            {
                picTheme.BackgroundImage = BibliotecaGastao.Properties.Resources.icons8_símbolo_da_lua_16;
                if (inicio != null) { inicio.BackColor = ColorTranslator.FromHtml("#f0f0f0"); }
                if (user != null) { user.BackColor = ColorTranslator.FromHtml("#f0f0f0"); }
                if (empres != null) { empres.BackColor = ColorTranslator.FromHtml("#f0f0f0"); }
                if (find != null) { find.BackColor = ColorTranslator.FromHtml("#f0f0f0"); }
                if (relF != null) { relF.BackColor = ColorTranslator.FromHtml("#f0f0f0"); }
                theme = false;
            }
        }
        //Botão para sair(não finalizado)
        private void logout_Click(object sender, EventArgs e)
        {

            Application.Exit();
        }

        //Mudança de painel
        //Painel incio
        private void home_Click(object sender, EventArgs e)
        {
            if (inicio == null)
            {
                inicio = new inicioForm();
                inicio.FormClosed += Inicio_FormClosed;
                inicio.MdiParent = this;
                inicio.Dock = DockStyle.Fill;
                inicio.Show();
                picTheme.BackgroundImage = BibliotecaGastao.Properties.Resources.icons8_símbolo_da_lua_16;
                theme = false;

            }
            else
            {
                inicio.Activate();
            }
        }
        private void Inicio_FormClosed(object sender, FormClosedEventArgs e)
        {
            inicio = null;
        }

        //Painel Usuários
        private void users_Click(object sender, EventArgs e)
        {
            if (user == null)
            {
                user = new userForm();
                user.FormClosed += User_FormClosed;
                user.MdiParent = this;
                user.Dock = DockStyle.Fill;
                user.Show();
                picTheme.BackgroundImage = BibliotecaGastao.Properties.Resources.icons8_símbolo_da_lua_16;
                theme = false;

            }
            else
            {
                user.Activate();
            }
        }

        private void User_FormClosed(object sender, FormClosedEventArgs e)
        {
            user = null;
        }

        //Painel de livros(emprestimo e pesquisa)
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (find == null)
            {
                find = new findForm();
                find.FormClosed += Find_FormClosed;
                find.MdiParent = this;
                find.Dock = DockStyle.Fill;
                find.Show();
                picTheme.BackgroundImage = BibliotecaGastao.Properties.Resources.icons8_símbolo_da_lua_16;
                theme = false;

            }
            else
            {
                find.Activate();
            }
        }

        private void Find_FormClosed(object sender, FormClosedEventArgs e)
        {
            find = null;
        }



        private void btnEmpres_Click(object sender, EventArgs e)
        {
            if (empres == null)
            {
                empres = new empresForm();
                empres.FormClosed += Empres_FormClosed;
                empres.MdiParent = this;
                empres.Dock = DockStyle.Fill;
                empres.Show();
                picTheme.BackgroundImage = BibliotecaGastao.Properties.Resources.icons8_símbolo_da_lua_16;
                theme = false;

            }
            else
            {
                empres.Activate();
            }
        }

        private void Empres_FormClosed(object sender, FormClosedEventArgs e)
        {
            empres = null;
        }


        //Painel Relatórios
        private void btnRel_Click(object sender, EventArgs e)
        {
            if (relF == null)
            {
                relF = new relForm();
                relF.FormClosed += RelF_FormClosed;
                relF.MdiParent = this;
                relF.Dock = DockStyle.Fill;
                relF.Show();
                picTheme.BackgroundImage = BibliotecaGastao.Properties.Resources.icons8_símbolo_da_lua_16;
                theme = false;

            }
            else
            {
                relF.Activate();

            }
        }

        private void RelF_FormClosed(object sender, FormClosedEventArgs e)
        {
            relF = null;
        }
    }
}
