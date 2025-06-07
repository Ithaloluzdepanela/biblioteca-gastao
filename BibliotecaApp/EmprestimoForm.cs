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
    public partial class EmprestimoForm : Form
    {
        public List<Usuarios> Usuarios { get; set; }
        public List<Livro> Livros { get; set; }
        public List<Emprestimo> Emprestimos { get; set; }

        public EmprestimoForm(List<Usuarios> usuarios, List<Livro> livros)
        {
            // Configuração de AutoComplete
            AutoCompleteStringCollection nomes = new AutoCompleteStringCollection();
            nomes.AddRange(Usuarios.Select(u => u.Nome).ToArray());
            txtNomeUsuario.AutoCompleteCustomSource = nomes;

            AutoCompleteStringCollection nomesLivros = new AutoCompleteStringCollection();
            nomesLivros.AddRange(Livros.Select(l => l.Nome).ToArray());
            txtLivro.AutoCompleteCustomSource = nomesLivros;
            Usuarios = usuarios;
            Livros = livros;
            Emprestimos = new List<Emprestimo>();  // Definir corretamente a lista de empréstimos
        }

        private void EmprestimoForm_Load(object sender, EventArgs e)
        {
            dtpDataEmprestimo.Value = DateTime.Today;
            dtpDataDevolucao.Value = DateTime.Today.AddDays(7);



            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;
        }

        public EmprestimoForm()
        {
            InitializeComponent();
            Usuarios = new List<Usuarios>();
            Livros = new List<Livro>();
            Emprestimos = new List<Emprestimo>();
        }

        private void txtNomeUsuario_TextChanged(object sender, EventArgs e)
        {
            string texto = txtNomeUsuario.Text.ToLower();
            var sugestoes = Usuarios.Where(u => u.Nome.ToLower().Contains(texto)).ToList();

            lstSugestoesUsuario.Items.Clear();  // Limpa os itens antes de adicionar novos

            // Adiciona as sugestões ao ListBox
            foreach (var usuario in sugestoes)
            {
                lstSugestoesUsuario.Items.Add(usuario.Nome);
            }

            // Exibe ou esconde a lista de sugestões
            lstSugestoesUsuario.Visible = sugestoes.Any();
        }

        private void lstSugestoesUsuario_Click(object sender, EventArgs e)
        {
            if (lstSugestoesUsuario.SelectedItem != null)
            {
                txtNomeUsuario.Text = lstSugestoesUsuario.SelectedItem.ToString();
                lstSugestoesUsuario.Visible = false;  // esconde a lista de sugestões ao selecionar


                var usuario = Usuarios.FirstOrDefault(u => u.Nome == txtNomeUsuario.Text);
                if (usuario != null)
                {
                    if (usuario.TipoUsuario == "Professor")
                    {
                        dtpDataDevolucao.Value = DateTime.Today;
                        dtpDataDevolucao.Enabled = false;                               
                        
                    }
                    else
                    {
                        dtpDataDevolucao.Value = DateTime.Today.AddDays(7);
                        
                    }
                }
            }
        }



        private void btnEmprestar_Click(object sender, EventArgs e)
        {
            var usuario = Usuarios.FirstOrDefault(u => u.Nome.Equals(txtNomeUsuario.Text, StringComparison.OrdinalIgnoreCase));
            var livro = Livros.FirstOrDefault(l => l.Nome.Equals(txtLivro.Text, StringComparison.OrdinalIgnoreCase));

            if (usuario == null || livro == null)
            {
                MessageBox.Show("Usuário ou livro não encontrado.");
                return;
            }

            if (!livro.Disponibilidade || livro.Quantidade <= 0)
            {
                MessageBox.Show("Livro indisponível.");
                return;
            }

            Emprestimo novoEmprestimo = new Emprestimo
            {
                Id = Emprestimos.Count + 1,
                UsuarioId = usuario.Id,
                LivroId = livro.Id,
                BibliotecariaResponsavel = cbBibliotecaria.SelectedItem?.ToString(),
                DataEmprestimo = DateTime.Today,
                DataDevolucao = (usuario.TipoUsuario == "Professor") ? (DateTime?)null : dtpDataDevolucao.Value,
                DataRealDevolucao = null,
                Status = "Emprestado"
            };

            Emprestimos.Add(novoEmprestimo);

            // Atualiza a quantidade do livro
            livro.Quantidade--;
            livro.Disponibilidade = livro.Quantidade > 0;

            MessageBox.Show("Empréstimo registrado com sucesso!");
        }

        private void roundedTextBox1_Load(object sender, EventArgs e)
        {

        }

       

        private void biblio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lstSugestoesUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }

        private void dtpDataEmprestimo_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtNomeUsuario_Load(object sender, EventArgs e)
        {

        }

        private void chkDevolucaoPersonalizada_CheckedChanged(object sender, EventArgs e)
        {

            // Habilita o DateTimePicker para seleção de data personalizada
            dtpDataDevolucao.Enabled = chkDevolucaoPersonalizada.Checked; 

            if (!chkDevolucaoPersonalizada.Checked)
            {
                // Se a opção não estiver marcada, define a data de devolução para 7 dias após o empréstimo
                dtpDataDevolucao.Value = DateTime.Today.AddDays(7);
            }
        }
    }
}
