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
    public partial class frmProgresso : Form
    {
        private Timer closeTimer;

        public frmProgresso()
        {
            InitializeComponent();

            // Configura o timer para fechar automaticamente
            closeTimer = new Timer();
            closeTimer.Interval = 3000; // 1.5 segundos após concluir
            closeTimer.Tick += (s, e) => { this.Close(); };
        }

        public void AtualizarProgresso(int valor, string mensagem)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() =>
                {
                    progressBar1.Value = valor;
                    lblStatus.Text = mensagem;

                    // Se completou 100%, inicia o timer para fechar
                    if (valor == 100) closeTimer.Start();
                }));
            }
            else
            {
                progressBar1.Value = valor;
                lblStatus.Text = mensagem;

                // Se completou 100%, inicia o timer para fechar
                if (valor == 100) closeTimer.Start();
            }
        }

        private void frmProgresso_Load(object sender, EventArgs e)
        {

        }
    }
}
