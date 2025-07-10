using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BibliotecaApp
{
    public partial class EmprestimoForm : Form
    {
        #region Propriedades

        public List<Usuarios> Usuarios { get; set; }
        public List<Livro> Livros { get; set; }
        public List<Emprestimo> Emprestimos { get; set; }

        private List<Livro> _cacheLivros = new List<Livro>();

        #endregion

        #region Construtores

        public EmprestimoForm(List<Usuarios> usuarios, List<Livro> livros)
        {
            InitializeComponent();

            Usuarios = usuarios;
            Livros = livros;
            Emprestimos = new List<Emprestimo>();

            // Configurações de AutoComplete para usuário e livro
            txtNomeUsuario.AutoCompleteCustomSource = new AutoCompleteStringCollection();
            txtNomeUsuario.AutoCompleteCustomSource.AddRange(Usuarios.Select(u => u.Nome).ToArray());

            txtLivro.AutoCompleteCustomSource = new AutoCompleteStringCollection();
            txtLivro.AutoCompleteCustomSource.AddRange(Livros.Select(l => l.Nome).ToArray());

            txtLivro.KeyDown += txtLivro_KeyDown;
            lstLivros.Click += lstLivros_Click;
            lstLivros.KeyDown += lstLivros_KeyDown;
        }

        public EmprestimoForm() // Construtor alternativo para design ou testes
        {
            InitializeComponent();
            Usuarios = new List<Usuarios>();
            Livros = new List<Livro>();
            Emprestimos = new List<Emprestimo>();

            txtLivro.KeyDown += txtLivro_KeyDown;
            lstLivros.Click += lstLivros_Click;
            lstLivros.KeyDown += lstLivros_KeyDown;
        }

        #endregion

        #region Evento de carregamento do formulário

        private void EmprestimoForm_Load(object sender, EventArgs e)
        {
            dtpDataEmprestimo.Value = DateTime.Today;
            dtpDataDevolucao.Value = DateTime.Today.AddDays(7);

            txtBarcode.KeyDown += txtBarcode_KeyDown;

            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;
        }

        #endregion

        #region Emprestar Livro

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

            // Atualiza a disponibilidade
            livro.Quantidade--;
            livro.Disponibilidade = livro.Quantidade > 0;

            MessageBox.Show("Empréstimo registrado com sucesso!");
        }

        #endregion

        #region Seleção e sugestão de usuários

        private void txtNomeUsuario_TextChanged(object sender, EventArgs e)
        {
            string texto = txtNomeUsuario.Text.ToLower();
            var sugestoes = Usuarios.Where(u => u.Nome.ToLower().Contains(texto)).ToList();

            lstSugestoesUsuario.Items.Clear();

            foreach (var usuario in sugestoes)
            {
                lstSugestoesUsuario.Items.Add(usuario.Nome);
            }

            lstSugestoesUsuario.Visible = sugestoes.Any();
        }

        private void lstSugestoesUsuario_Click(object sender, EventArgs e)
        {
            if (lstSugestoesUsuario.SelectedItem != null)
            {
                txtNomeUsuario.Text = lstSugestoesUsuario.SelectedItem.ToString();
                lstSugestoesUsuario.Visible = false;

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
                        dtpDataDevolucao.Enabled = true;
                    }
                }
            }
        }

        #endregion

        #region AutoComplete para livros

        private void txtLivro_KeyDown(object sender, KeyEventArgs e)
        {
            if (lstLivros.Visible)
            {
                if (e.KeyCode == Keys.Down && lstLivros.SelectedIndex < lstLivros.Items.Count - 1)
                {
                    e.Handled = true;
                    lstLivros.SelectedIndex++;
                }
                else if (e.KeyCode == Keys.Up && lstLivros.SelectedIndex > 0)
                {
                    e.Handled = true;
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

        private void lstLivros_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstLivros.SelectedIndex >= 0)
            {
                e.Handled = true;
                SelecionarLivro(lstLivros.SelectedIndex);
            }
        }

        private void SelecionarLivro(int index)
        {
            var livro = _cacheLivros[index];

            txtLivro.Text = livro.Nome;
            lstLivros.Visible = false;

            txtBarcode.Enabled = true;
            txtBarcode.Text = livro.CodigoDeBarras;
            txtBarcode.Enabled = false;
        }

        #endregion

        #region Data de devolução personalizada

        private void chkDevolucaoPersonalizada_CheckedChanged(object sender, EventArgs e)
        {
            dtpDataDevolucao.Enabled = chkDevolucaoPersonalizada.Checked;

            if (!chkDevolucaoPersonalizada.Checked)
                dtpDataDevolucao.Value = DateTime.Today.AddDays(7);
        }

        #endregion

        #region Navegação com ENTER

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }

        #endregion

        #region Scanner de código de barras

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
                        txtLivro.Text = titulo;
                        txtBarcode.Enabled = true;
                        txtBarcode.Text = codigo;
                        txtBarcode.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Livro não encontrado!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar o livro: " + ex.Message);
            }
        }



        #endregion

        private void cbBibliotecaria_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbBibliotecaria.DrawMode = DrawMode.OwnerDrawFixed;
            cbBibliotecaria.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBibliotecaria.ItemHeight = 35; // define a altura dos itens
        }
    }
}
