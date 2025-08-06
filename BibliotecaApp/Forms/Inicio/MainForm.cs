using BibliotecaApp.Forms.Livros;
using BibliotecaApp.Forms.Login;
using BibliotecaApp.Forms.Relatorio;
using BibliotecaApp.Forms.Usuario;
using BibliotecaApp.Properties;
using System;
using ToggleSwitch;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace BibliotecaApp.Forms.Inicio
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

            btnInicio.Enabled = false;
            btnLivro.Enabled = true;
            btnRel.Enabled = true;
            btnUsuario.Enabled = true;
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

            btnInicio.Enabled = false;

            inicio.Show();
        }
        private void Inicio_FormClosed(object sender, FormClosedEventArgs e)
        {
            inicio = null;
        }

        //Form usuário
        private void btnUsuario_Click(object sender, EventArgs e)
        {
            btnUsuario.Enabled = false;
            btnLivro.Enabled = true;
            btnRel.Enabled = true;
            btnInicio.Enabled = true;

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
        private async void btnLivro_Click(object sender, EventArgs e)
        {
            btnLivro.Enabled = false; // Desabilita o botão
            btnUsuario.Enabled = true;
            btnRel.Enabled = true;
            btnInicio.Enabled = true;

            if (livros == null)
            {
                btnUsuario.Enabled = true;
                btnRel.Enabled = true;
                btnInicio.Enabled = true;

                livros = new LivrosForm();
                livros.FormClosed += Livros_FormClosed;
                livros.MdiParent = this;
                livros.Dock = DockStyle.Fill;
                livros.Show();
            }
            else
            {
                livros.BringToFront(); // Opcional: traz o form para frente se já estiver aberto
            }

            livroTransition.Start();

            await Task.Delay(500);

            btnLivro.Enabled = true; // Reabilita o botão
        }

        private void Livros_FormClosed(object sender, FormClosedEventArgs e)
        {
            livros = null;
        }

        //Form rel
        private void btnRel_Click(object sender, EventArgs e)
        {
            btnRel.Enabled = false;
            btnLivro.Enabled = true;
            btnInicio.Enabled = true;
            btnUsuario.Enabled = true;

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
            const string msg = "Tem certeza de que quer finalizar a sessão?";
            const string box = "Confirmação de logout";
            var confirma = MessageBox.Show(msg, box, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirma == DialogResult.Yes)
            {
                this.Close();
            }
        }

        #endregion

        #region Interações/Funcionalidades do Form

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

        //Load para fechar o Login
        private void MainForm_Load(object sender, EventArgs e)
        {


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

        //Transição de expansão do livro
        bool livroExpand = false;
        private void livroTransition_Tick(object sender, EventArgs e)
        {

            if (livroExpand == false)
            {
                livroContainer.Height += 10;
                if (livroContainer.Height >= 300)
                {
                    livroTransition.Stop();
                    livroExpand = true;
                }
            }
            else
            {
                livroContainer.Height -= 10;
                if (livroContainer.Height <= 60)
                {
                    livroTransition.Stop();
                    livroExpand = false;
                }
            }
        }

        #endregion
    }
}