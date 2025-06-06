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
    public partial class LivrosForm : Form
    {
        public LivrosForm()
        {
            InitializeComponent();
        }

        private void Pic_Cadastrar_Click(object sender, EventArgs e)
        {
            CadastroLivroForm popup = new CadastroLivroForm();
            Location = popup.Location;
            popup.ShowDialog();
        }
    }
}
