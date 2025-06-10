using BibliotecaApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace BibliotecaApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            mdiProp();
            btnIn();
        }
        #region Componentes de inicialização
        //Funções da API para movimentar a aba
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        //Nome dos Forms
        InicioForm inicio;
        UsuarioForm usuario;
        LivrosForm livros;
        RelForm rel;

        //Proporção dos Forms
        private void mdiProp()
        {
            this.SetBevel(false);
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.FromArgb(232, 234, 237);
        }
        
        //Variável criada para inicialização do Form de inicio no MDI
        private void btnInicio_Click(object sender, EventArgs e)
        {
            btnIn();
        }
        #endregion

        #region Expansão do menu
        bool menuExpand = false;
        private void picMenu_Click(object sender, EventArgs e)
        {
            menuTransition.Start();
        }

        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (menuExpand == false)
            {
                menu.Width += 10;
                incioContainer.Width = menu.Width;
                livroContainer.Width = menu.Width;
                relContainer.Width = menu.Width;
                sairContainer.Width = menu.Width;
                usuarioContainer.Width = menu.Width;
                if (menu.Width >= 200)
                {
                    menuTransition.Stop();
                    menuExpand = true;
                }
            }
            else
            {
                menu.Width -= 10;
                incioContainer.Width = menu.Width;
                livroContainer.Width = menu.Width;
                relContainer.Width = menu.Width;
                sairContainer.Width = menu.Width;
                usuarioContainer.Width = menu.Width;
                if (menu.Width <= 40)
                {
                    menuTransition.Stop();
                    menuExpand = false;
                }
            }
        }
        #endregion

        #region Botões
        //Form Inicio
        public void btnIn()
        {
                inicio = new InicioForm();
                inicio.FormClosed += Inicio_FormClosed;
                inicio.MdiParent = this;
                inicio.Dock = DockStyle.Fill;
                inicio.Show();
         }
        private void Inicio_FormClosed(object sender, FormClosedEventArgs e)
        {
            inicio = null;
        }
        
        //Form usuário
        private void btnUsuario_Click(object sender, EventArgs e)
        {
            usuario = new UsuarioForm();
            usuario.FormClosed += Usuario_FormClosed;
            usuario.MdiParent = this;
            usuario.Dock = DockStyle.Fill;
            usuario.Show();
        }

        private void Usuario_FormClosed(object sender, FormClosedEventArgs e)
        {
            usuario = null;
        }
        
        //Form Livros
        private void btnLivro_Click(object sender, EventArgs e)
        {
            livros = new LivrosForm();
            livros.FormClosed += Livros_FormClosed;
            livros.MdiParent = this;
            livros.Dock = DockStyle.Fill;
            livros.Show();
        }

        private void Livros_FormClosed(object sender, FormClosedEventArgs e)
        {
            livros = null;
        }

        //Form rel
        private void btnRel_Click(object sender, EventArgs e)
        {
            rel = new RelForm();
            rel.FormClosed += Relatorios_FormClosed;
            rel.MdiParent = this;
            rel.Dock = DockStyle.Fill;
            rel.Show();
        }

        private void Relatorios_FormClosed(object sender, FormClosedEventArgs e)
        {
            rel = null;
        }

        //Botão de sair
        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();

        }
        #endregion

        #region Control box
        private void picExit_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }
        bool janela = true;

        //Funcionalidade dos botões
        private void picMax_Click(object sender, EventArgs e)
        {
            if (janela == false)
            {
                this.WindowState = FormWindowState.Maximized;
                picMax.BackgroundImage = Resources.icons8_verificar_todos_os_20;
                janela = true;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                picMax.BackgroundImage = Resources.icons8_quadrado_arredondado_20;
                janela = false;
            }
        }
        private void picMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //Animação de fundo
        private void picExit_MouseEnter(object sender, EventArgs e)
        {
            picExit.BackColor = Color.Gainsboro;
        }

        private void picExit_MouseLeave(object sender, EventArgs e)
        {
            picExit.BackColor = Color.Transparent;
        }

        private void picMax_MouseEnter(object sender, EventArgs e)
        {
            picMax.BackColor = Color.Gainsboro;
        }

        private void picMax_MouseLeave(object sender, EventArgs e)
        {
            picMax.BackColor = Color.Transparent;
        }

        private void picMin_MouseEnter(object sender, EventArgs e)
        {
            picMin.BackColor = Color.Gainsboro;
        }

        private void picMin_MouseLeave(object sender, EventArgs e)
        {
            picMin.BackColor = Color.Transparent;
        }


        #endregion

        #region Interações do Form
        //Load para fechar o Login
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoginForm f = new LoginForm();
            f.ShowDialog();
            if (LoginForm.cancelar == false)
            {
                Application.Exit();
            }

            //Inicializar com tela cheia
            if (LoginForm.cancelar == true)
            {
                this.WindowState = FormWindowState.Maximized;
            }

        }

        //Locomoção do painel
        private void panelControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                picMax.BackgroundImage = Resources.icons8_verificar_todos_os_20;
                janela = true;
            }
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        //Correção para o icone da control box
        private void panelControl_MouseEnter(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                picMax.BackgroundImage = Resources.icons8_quadrado_arredondado_20;
                janela = false;
            }
        }
        #endregion

    }
}