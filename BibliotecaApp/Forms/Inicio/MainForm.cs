using BibliotecaApp.Forms.Livros;
using BibliotecaApp.Forms.Login;
using BibliotecaApp.Forms.Relatorio;
using BibliotecaApp.Forms.Usuario;
using BibliotecaApp.Froms.Usuario;
using BibliotecaApp.Models;
using BibliotecaApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToggleSwitch;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
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

        private void tamanho()
        {
            this.Width = 1440; this.Height = 800;
        }
        private Size tamanhoOriginal;
        private Point localOriginal;
        private bool maximizado = false;
        

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
        EmprestimoForm emprestimo;
        EmprestimoRapidoForm emprestimoRap;
        CadastroLivroForm cadastroLivro;
        DevoluçãoForm devolução;
        CadUsuario usuarioCad; 
        EditarUsuarioForm usuarioEdit;

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
            btnLivros.Enabled = true;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;
        }

        //Função de maximizar/restaurar o Form
        private void AlternarMaximizado()
        {
            
            if (!maximizado)
            {
                tamanhoOriginal = this.Size;
                localOriginal = this.Location;

                Rectangle areaTrabalho = Screen.FromHandle(this.Handle).WorkingArea; // exclui barra de tarefas
                this.Location = areaTrabalho.Location;
                this.Size = areaTrabalho.Size;

                maximizado = true;
            }
            else
            {
                this.Size = tamanhoOriginal;
                this.Location = localOriginal;

                maximizado = false;
            }
        }

        private void PanelControl_MouseDown(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
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

        #region Form do Usuários

        //Form usuário
        private async void btnUsuario_Click(object sender, EventArgs e)
        {

            btnUsuario.Enabled = false; // Desabilita o botão
            btnLivro.Enabled = false;
           
            if (livroContainer.Height > 60)
            {
                livroTransition.Start();
                await Task.Delay(610);
            }

            userTransition.Start();

            await Task.Delay(400);

            btnUsuario.Enabled = true; // Reabilita o botão
            btnLivro.Enabled = true;

        }

        //Botão de cadastro do usuário
        private void btnUser_Click(object sender, EventArgs e)
        {
            btnUser.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

            usuario = new UsuarioForm();
            usuario.FormClosed += Usuario_FormClosed;
            usuario.MdiParent = this;
            usuario.Dock = DockStyle.Fill;
            usuario.Show();
        }

        //Botão de cadastro do usuário
        private void btnUserCad_Click(object sender, EventArgs e)
        {
            btnUserCad.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserEdit.Enabled = true;

            usuarioCad = new CadUsuario();
            usuarioCad.FormClosed += UsuarioCad_FormClosed;
            usuarioCad.MdiParent = this;
            usuarioCad.Dock = DockStyle.Fill;
            usuarioCad.Show();
        }

        private void UsuarioCad_FormClosed(object sender, FormClosedEventArgs e)
        {
            usuarioCad = null;
        }

        //Botão de edição do usuário
        private void btnUserEdit_Click(object sender, EventArgs e)
        {
            btnUserEdit.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;

            usuarioEdit = new EditarUsuarioForm();
            usuarioEdit.FormClosed += UsuarioEdit_FormClosed;
            usuarioEdit.MdiParent = this;
            usuarioEdit.Dock = DockStyle.Fill;
            usuarioEdit.Show();
        }

        private void UsuarioEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            usuarioEdit = null;
        }

        //Botão Usuário(Biblioteca)

        #endregion

        private void Usuario_FormClosed(object sender, FormClosedEventArgs e)
        {
            usuario = null;
        }

        #region Form dos Livros
        //Botão Expansão do Livro
        private async void btnLivro_Click(object sender, EventArgs e)
        {
            btnLivro.Enabled = false; // Desabilita o botão
            btnUsuario.Enabled = false;
            
            if (userContainer.Height > 60)
            {
                userTransition.Start();
                await Task.Delay(400);
                
            }

            livroTransition.Start();

            await Task.Delay(500);

            btnLivro.Enabled = true; // Reabilita o botão
            btnUsuario.Enabled = true;
            
        }

        //Botão Livro(Bilbioteca)
        private void btnLivros_Click(object sender, EventArgs e)
        {
            btnLivros.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

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

        //Botão de empréstimo do livro
        private void btnEmprestimo_Click(object sender, EventArgs e)
        {
            btnEmprestimo.Enabled = false;
            btnRel.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

            emprestimo = new EmprestimoForm();
            emprestimo.FormClosed += Emprestimo_FormClosed;
            emprestimo.MdiParent = this;
            emprestimo.Dock = DockStyle.Fill;
            emprestimo.Show();
        }

        private void Emprestimo_FormClosed(object sender, FormClosedEventArgs e)
        {
            emprestimo = null;
        }

        //Botão Empréstimo Rápido
        private void btnEmprestimoRap_Click(object sender, EventArgs e)
        {
            btnEmprestimoRap.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

            emprestimoRap = new EmprestimoRapidoForm();
            emprestimoRap.FormClosed += EmprestimoRap_FormClosed;
            emprestimoRap.MdiParent = this;
            emprestimoRap.Dock = DockStyle.Fill;
            emprestimoRap.Show();

        }

        //Botão de empréstimo rápido
        private void EmprestimoRap_FormClosed(object sender, FormClosedEventArgs e)
        {
            emprestimo = null;
        }
        private void btnLivroCad_Click(object sender, EventArgs e)
        {
            btnLivroCad.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

            cadastroLivro = new CadastroLivroForm();
            cadastroLivro.FormClosed += CadastroLivro_FormClosed;
            cadastroLivro.MdiParent = this;
            cadastroLivro.Dock = DockStyle.Fill;
            cadastroLivro.Show();

        }

        private void CadastroLivro_FormClosed(object sender, FormClosedEventArgs e)
        {
            cadastroLivro = null;
        }

        //Botão Devolução
        private void btnDev_Click(object sender, EventArgs e)
        {
            btnDev.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

            devolução = new DevoluçãoForm();
            devolução.FormClosed += Devolução_FormClosed;
            devolução.MdiParent = this;
            devolução.Dock = DockStyle.Fill;
            devolução.Show();
        }

        private void Devolução_FormClosed(object sender, FormClosedEventArgs e)
        {
            devolução = null;
        }


        #endregion

        //Form rel
        private void btnRel_Click(object sender, EventArgs e)
        {
            btnRel.Enabled = false;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

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
                LoginForm.cancelar = false;
                this.Close();
            }
        }

        #endregion

        #region Interações/Funcionalidades do Form

        #region Control box
        private void picExit_Click(object sender, EventArgs e)
        {
            const string msg = "Tem certeza de que quer fechar a Aplicação?";
            const string box = "Confirmação de Encerramento";
            var confirma = MessageBox.Show(msg, box, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirma == DialogResult.Yes)
            {
                Application.Exit();
            }
            

        }

        //Funcionalidade dos botões
        private void picMax_Click(object sender, EventArgs e)
        {
            AlternarMaximizado();

            if (maximizado == false)
            {
               picMax.BackgroundImage = Resources.icons8_quadrado_arredondado_20;
            }
            else
            {
               picMax.BackgroundImage = Resources.icons8_verificar_todos_os_20;
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
                AlternarMaximizado();
            }
            userLbl.Text = Sessao.NomeBibliotecariaLogada;
        }

        //Locomoção do painel
        private void panelControl_MouseDown(object sender, MouseEventArgs e)
        {
            maximizado = false;
            picMax.BackgroundImage = Resources.icons8_quadrado_arredondado_20;
            tamanho();
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        //Transição de expansão do livro e usuário
        bool userExpand = false;
        bool livroExpand = false;
        private void userTransition_Tick(object sender, EventArgs e)
        {
            if (userExpand == false)
            {
                userContainer.Height += 10;
                if (userContainer.Height >= 240)
                {
                    userTransition.Stop();
                    userExpand = true;
                }
            }
            else
            {
                userContainer.Height -= 10;
                if (userContainer.Height <= 60)
                {
                    userTransition.Stop();
                    userExpand = false;
                }
            }
        }
        private void livroTransition_Tick(object sender, EventArgs e)
        {

            if (livroExpand == false)
            {
                livroContainer.Height += 10;
                if (livroContainer.Height >= 360)
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