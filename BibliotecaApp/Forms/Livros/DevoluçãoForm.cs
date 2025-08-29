using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Livros
{
    public partial class DevoluçãoForm : Form
    {
        public DevoluçãoForm()
        {
            InitializeComponent();
        }
        // Método que limpa todos os campos do formulário
        private void LimparTabela()
        {
            txtNome.Text = "";
            mtxCodigoBarras.Text = "";
            txtNome.Focus(); // Foco volta para o primeiro campo
        }
        
        #region Evento: Botão "Limpar"

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            // Confirma antes de limpar os campos
            DialogResult resultado = MessageBox.Show(
                "Tem certeza de que deseja limpar tudo?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                LimparTabela(); // Se sim, limpa todos os campos
            }
        }


        #endregion

        private void btnConfirmarDevolucao_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
