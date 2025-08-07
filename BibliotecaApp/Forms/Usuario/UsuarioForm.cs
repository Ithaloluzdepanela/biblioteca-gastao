using BibliotecaApp;
using BibliotecaApp.Froms.Usuario;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Usuario
{


    public partial class UsuarioForm : Form
    {
        public UsuarioForm()
        {
            InitializeComponent();
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           AbrirCadastroUsuario();

        }

        private void AbrirCadastroUsuario()
        {
            // Remove controles anteriores, se necessário
            panelConteudo.Controls.Clear();

            CadUsuario cadastro = new CadUsuario();
            cadastro.TopLevel = false; // Permite adicionar como controle
            cadastro.FormBorderStyle = FormBorderStyle.None;
            cadastro.Dock = DockStyle.None; // Para centralizar manualmente

            // Centraliza manualmente
            cadastro.StartPosition = FormStartPosition.Manual;
            cadastro.Location = new Point(
                (panelConteudo.Width - cadastro.Width) / 2,
                (panelConteudo.Height - cadastro.Height) / 2
            );

            panelConteudo.Controls.Add(cadastro);
            cadastro.Show();
            cadastro.BringToFront();
        }
        private void AbrirEditarUsuario()
        {
            // Remove controles anteriores
            panelConteudo.Controls.Clear();

            EditarUsuarioForm editarUsuario = new EditarUsuarioForm();
            editarUsuario.TopLevel = false;
            editarUsuario.FormBorderStyle = FormBorderStyle.None;
            editarUsuario.Dock = DockStyle.None;
            editarUsuario.StartPosition = FormStartPosition.Manual;

            // Adiciona ao painel e mostra primeiro (pra ele calcular tamanho)
            panelConteudo.Controls.Add(editarUsuario);
            editarUsuario.Show();

            // Agora que já foi mostrado, dá pra centralizar corretamente
            editarUsuario.Location = new Point(
                (panelConteudo.Width - editarUsuario.Width) / 2,
                (panelConteudo.Height - editarUsuario.Height) / 2
            );

            editarUsuario.BringToFront();
        }

        private void panelConteudo_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            AbrirEditarUsuario();
        }
    }
}
