using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        private List<Livro> _cacheLivros = new List<Livro>();


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

            txtBarcode.KeyDown += txtBarcode_KeyDown;

            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;
        }

        public EmprestimoForm()
        {
            InitializeComponent();
            Usuarios = new List<Usuarios>();
            Livros = new List<Livro>();
            Emprestimos = new List<Emprestimo>();

            
            txtLivro.KeyDown += txtLivro_KeyDown;
            lstLivros.Click += lstLivros_Click;
            lstLivros.KeyDown += lstLivros_KeyDown;
        }

        private void lstLivros_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyCode == Keys.Enter && lstLivros.SelectedIndex >= 0)
            {
                e.Handled = true;  // evita o som padrão de “ding”
                SelecionarLivro(lstLivros.SelectedIndex);
            }
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


        private void txtLivro_KeyDown(object sender, KeyEventArgs e)
        {
            if (lstLivros.Visible)
            {
                if (e.KeyCode == Keys.Down)
                {
                    e.Handled = true;
                    if (lstLivros.SelectedIndex < lstLivros.Items.Count - 1)
                        lstLivros.SelectedIndex++;
                }
                else if (e.KeyCode == Keys.Up)
                {
                    e.Handled = true;
                    if (lstLivros.SelectedIndex > 0)
                        lstLivros.SelectedIndex--;
                }
                else if (e.KeyCode == Keys.Enter && lstLivros.SelectedIndex >= 0)
                {
                    e.Handled = true;
                    SelecionarLivro(lstLivros.SelectedIndex);
                }
            }
        }

        private void lstLivros_Click(object sender, EventArgs e)
        {
            if (lstLivros.SelectedIndex >= 0)
                SelecionarLivro(lstLivros.SelectedIndex);
        }

        private void SelecionarLivro(int index)
        {
            var livro = _cacheLivros[index];

            // Preenche txtLivro e esconde autocomplete
            txtLivro.Text = livro.Nome;
            lstLivros.Visible = false;

            // Preenche txtBarcode
            txtBarcode.Enabled = true;
            txtBarcode.Text = livro.CodigoDeBarras;
            txtBarcode.Enabled = false;
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

        private void txtBarcode_Load(object sender, EventArgs e)
        {

        }


        //bar code scanner


        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string codigo = txtBarcode.Text.Trim();
                if (!string.IsNullOrEmpty(codigo))
                {
                    
                    BuscarLivroPorCodigo(codigo);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }


        private void BuscarLivroPorCodigo(string codigo)
        {
            try
            {
                string connStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=Biblioteca;Integrated Security=True";
                string sql = "SELECT Titulo FROM Livros WHERE CodigoBarra = @codigo";

                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@codigo", codigo);
                    conn.Open();

                    object resultado = cmd.ExecuteScalar();
                    if (resultado != null)
                    {
                        string titulo = resultado.ToString();

                        // 1) Preenche txtLivro com o título
                        txtLivro.Text = titulo;

                        // 2) Atualiza txtBarcode (normalmente ele já contém, mas só pra garantir)
                        txtBarcode.Enabled = true;
                        txtBarcode.Text = codigo;
                        txtBarcode.Enabled = false;

                        // se você estiver usando lstResultados e quiser selecionar o item:
                        // for (int i = 0; i < lstResultados.Items.Count; i++)
                        // {
                        //     if ((lstResultados.Items[i] as Livro)?.CodigoBarra == codigo)
                        //     {
                        //         lstResultados.SelectedIndex = i;
                        //         break;
                        //     }
                        // }
                    }
                    else
                    {
                        MessageBox.Show("Livro não encontrado!");
                        // limpa campos ou faz outro tratamento...
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar o livro: " + ex.Message);
            }
        }


       

        private void txtLivro_Load(object sender, EventArgs e)
        {

        }

        
    }
}
