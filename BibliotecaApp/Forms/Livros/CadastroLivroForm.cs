using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Livros
{
    public partial class CadastroLivroForm : Form
    {
        #region Propriedades e Campos
        public event EventHandler LivroAtualizado;

        private List<string> generosPadronizados = new List<string>
        {
            "Poesia", "Literatura de Cordel", "Biografia", "Autobiografia", "Diálogo",
            "Hábito", "Psicologia", "Cultura Afro-brasileira", "História", "Teatro",
            "Educação", "Romance", "Ficção", "Fantasia", "Mitologia", "Literatura Infantil",
            "Adolescentes", "Infantojuvenil", "Suspense", "Lenda", "Folclore", "Novela",
            "Fábula", "Narrativa", "Afetividade", "Letramento", "Filosofia",
            "Política", "Culinária", "Crônica", "Conto", "Didático", "Literatura",
        };

        private bool generoSelecionadoDaLista = false; // Flag para controlar quando gênero foi selecionado da lista
        private Timer focoTimer;
        private Timer validationTimer;

        #endregion

        #region Inicialização do Formulário

        public CadastroLivroForm()
        {
            focoTimer = new Timer();
            focoTimer.Interval = 50; // 50 milissegundos
            focoTimer.Tick += FocoTimer_Tick;

            validationTimer = new Timer();
            validationTimer.Interval = 100; // 100ms delay for validation
            validationTimer.Tick += ValidationTimer_Tick;

            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void CadastroLivroForm_Load(object sender, EventArgs e)
        {
            ConfigurarEventos();
            ConfigurarNavegacao();
        }

        private void ConfigurarEventos()
        {
            txtGenero.TextChanged += txtGenero_TextChanged;
            txtGenero.KeyDown += txtGenero_KeyDown;
            txtGenero.Leave += txtGenero_Leave;
            lstSugestoesGenero.Click += lstSugestoesGenero_Click;
            lstSugestoesGenero.KeyDown += lstSugestoesGenero_KeyDown;
            lstSugestoesGenero.Leave += lstSugestoesGenero_Leave;
        }

        private void ConfigurarNavegacao()
        {
            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;
        }

        #endregion

        #region Eventos de Navegação

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }

        #endregion

        #region Eventos dos Botões

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "Tem certeza de que deseja limpar tudo?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                LimparFormulario();
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            if (!ValidarQuantidade(out int quantidade))
                return;

            if (!ValidarCodigoBarras(out string codigoBarras))
                return;

            CadastrarLivro(quantidade, codigoBarras);
        }

        #endregion

        #region Sistema de Sugestão de Gêneros

        private void txtGenero_TextChanged(object sender, EventArgs e)
        {
            generoSelecionadoDaLista = false;

            string texto = txtGenero.Text.Trim();

            if (string.IsNullOrEmpty(texto))
            {
                lstSugestoesGenero.Visible = false;
                return;
            }

            var sugestoes = generosPadronizados
                .Where(g => g.StartsWith(texto, StringComparison.OrdinalIgnoreCase) ||
                           CalcularDistanciaLevenshtein(g.ToLower(), texto.ToLower()) <= 2)
                .OrderBy(g => g.StartsWith(texto, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                .ThenBy(g => CalcularDistanciaLevenshtein(g.ToLower(), texto.ToLower()))
                .ThenBy(g => g)
                .ToList();

            if (sugestoes.Any())
            {
                lstSugestoesGenero.DataSource = sugestoes;
                lstSugestoesGenero.Visible = true;
                PosicionarListBox();
                if (lstSugestoesGenero.Items.Count > 0)
                    lstSugestoesGenero.SelectedIndex = 0;
            }
            else
            {
                lstSugestoesGenero.Visible = false;
            }
        }

        private void PosicionarListBox()
        {
            lstSugestoesGenero.Location = new System.Drawing.Point(txtGenero.Left, txtGenero.Bottom);
            lstSugestoesGenero.Width = txtGenero.Width;
            lstSugestoesGenero.BringToFront();
        }

        private void txtGenero_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesGenero.Visible || lstSugestoesGenero.Items.Count == 0)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    this.SelectNextControl((Control)sender, true, true, true, true);
                }
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Down:
                    e.SuppressKeyPress = true;
                    lstSugestoesGenero.Focus();
                    if (lstSugestoesGenero.Items.Count > 0)
                        lstSugestoesGenero.SelectedIndex = 0;
                    break;

                case Keys.Enter:
                case Keys.Tab:
                    e.SuppressKeyPress = true;
                    if (lstSugestoesGenero.Items.Count > 0)
                    {
                        SelecionarPrimeiroGenero();
                    }
                    break;

                case Keys.Escape:
                    e.SuppressKeyPress = true;
                    lstSugestoesGenero.Visible = false;
                    break;
            }
        }

        private void txtGenero_Leave(object sender, EventArgs e)
        {
            validationTimer.Stop();
            validationTimer.Start();
        }

        private void lstSugestoesGenero_Click(object sender, EventArgs e)
        {
            if (lstSugestoesGenero.SelectedItem != null)
            {
                string generoSelecionado = lstSugestoesGenero.SelectedItem.ToString();
                generoSelecionadoDaLista = true;
                txtGenero.Text = generoSelecionado;
                lstSugestoesGenero.Visible = false;
                txtGenero.Focus();
                txtGenero.SelectionStart = txtGenero.Text.Length;
            }
        }

        private void lstSugestoesGenero_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (lstSugestoesGenero.SelectedItem != null)
                    {
                        e.SuppressKeyPress = true;
                        string generoSelecionado = lstSugestoesGenero.SelectedItem.ToString();
                        generoSelecionadoDaLista = true;
                        txtGenero.Text = generoSelecionado;
                        lstSugestoesGenero.Visible = false;
                        txtGenero.Focus();
                        txtGenero.SelectionStart = txtGenero.Text.Length;
                        ProximoCampo(txtGenero);
                    }
                    break;

                case Keys.Escape:
                    e.SuppressKeyPress = true;
                    lstSugestoesGenero.Visible = false;
                    txtGenero.Focus();
                    break;
            }
        }

        private void lstSugestoesGenero_Leave(object sender, EventArgs e)
        {
            lstSugestoesGenero.Visible = false;
        }

        #endregion

        #region Métodos de Validação

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtAutor.Text) ||
                string.IsNullOrWhiteSpace(txtGenero.Text) ||
                string.IsNullOrWhiteSpace(txtQuantidade.Text) ||
                string.IsNullOrWhiteSpace(mtxCodigoBarras.Text.Replace(" ", "")))
            {
                MessageBox.Show("Por favor, preencha todos os campos antes de cadastrar.",
                                "Campos obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidarQuantidade(out int quantidade)
        {
            if (!int.TryParse(txtQuantidade.Text, out quantidade))
            {
                MessageBox.Show("Por favor, insira apenas números no campo 'Quantidade'.",
                                "Erro de formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidarCodigoBarras(out string codigoBarras)
        {
            codigoBarras = ObterCodigoDeBarrasFormatado();

            if (codigoBarras.Length != 13)
            {
                MessageBox.Show("O código de barras deve conter exatamente 13 dígitos.",
                                "Código inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void ValidarGeneroAoSair()
        {
            if (generoSelecionadoDaLista)
            {
                generoSelecionadoDaLista = false;
                lstSugestoesGenero.Visible = false;
                return;
            }

            string entrada = txtGenero.Text.Trim();

            if (string.IsNullOrEmpty(entrada))
            {
                lstSugestoesGenero.Visible = false;
                return;
            }

            if (!GeneroExisteOuSimilar(entrada))
            {
                if (ConfirmarNovoGenero(entrada))
                {
                    AdicionarNovoGenero(entrada);
                }
                else
                {
                    txtGenero.Text = "";
                }
            }

            lstSugestoesGenero.Visible = false;
        }

        #endregion

        #region Métodos de Banco de Dados

        private void CadastrarLivro(int quantidade, string codigoBarras)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();

                    using (SqlCeCommand comando = conexao.CreateCommand())
                    {
                        comando.CommandText = @"INSERT INTO Livros 
                            (Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade)
                            VALUES
                            (@Nome, @Autor, @Genero, @Quantidade, @CodigoBarras, @Disponibilidade)";

                        comando.Parameters.AddWithValue("@Nome", txtNome.Text);
                        comando.Parameters.AddWithValue("@Autor", txtAutor.Text);
                        comando.Parameters.AddWithValue("@Genero", txtGenero.Text);
                        comando.Parameters.AddWithValue("@Quantidade", quantidade);
                        comando.Parameters.AddWithValue("@CodigoBarras", codigoBarras);
                        comando.Parameters.AddWithValue("@Disponibilidade", 1);

                        comando.ExecuteNonQuery();
                    }

                    MessageBox.Show("Livro salvo com sucesso!",
                                    "Cadastro realizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LivroAtualizado?.Invoke(this, EventArgs.Empty);

                    // Dispara evento global para atualizar todos os forms
                    BibliotecaApp.Utils.EventosGlobais.OnLivroCadastradoOuAlterado();

                    LimparFormulario();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao cadastrar livro: {ex.Message}",
                                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Métodos Utilitários

        private void LimparFormulario()
        {
            txtNome.Text = "";
            txtAutor.Text = "";
            txtGenero.Text = "";
            txtQuantidade.Text = "";
            mtxCodigoBarras.Text = "";
            lstSugestoesGenero.Visible = false;
            txtNome.Focus();
        }

        private string ObterCodigoDeBarrasFormatado()
        {
            return new string(mtxCodigoBarras.Text.Where(char.IsDigit).ToArray());
        }

        private void SelecionarPrimeiroGenero()
        {
            if (lstSugestoesGenero.Items.Count > 0)
            {
                generoSelecionadoDaLista = true;
                txtGenero.Text = lstSugestoesGenero.Items[0].ToString();
                lstSugestoesGenero.Visible = false;
                txtGenero.SelectionStart = txtGenero.Text.Length;
                this.SelectNextControl(txtGenero, true, true, true, true);
            }
        }

        private void ProximoCampo(object sender)
        {
            this.SelectNextControl((Control)sender, true, true, true, true);
        }

        private bool GeneroExiste(string entrada)
        {
            return generosPadronizados
                .Any(g => string.Equals(g.Trim(), entrada.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private bool GeneroExisteOuSimilar(string entrada)
        {
            // First check for exact match
            if (GeneroExiste(entrada))
                return true;

            // Then check for very similar matches (distance <= 1)
            return generosPadronizados
                .Any(g => CalcularDistanciaLevenshtein(g.ToLower(), entrada.ToLower()) <= 1);
        }

        private int CalcularDistanciaLevenshtein(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1)) return s2?.Length ?? 0;
            if (string.IsNullOrEmpty(s2)) return s1.Length;

            int[,] matriz = new int[s1.Length + 1, s2.Length + 1];

            for (int i = 0; i <= s1.Length; i++)
                matriz[i, 0] = i;

            for (int j = 0; j <= s2.Length; j++)
                matriz[0, j] = j;

            for (int i = 1; i <= s1.Length; i++)
            {
                for (int j = 1; j <= s2.Length; j++)
                {
                    int custo = s1[i - 1] == s2[j - 1] ? 0 : 1;
                    matriz[i, j] = Math.Min(
                        Math.Min(matriz[i - 1, j] + 1, matriz[i, j - 1] + 1),
                        matriz[i - 1, j - 1] + custo);
                }
            }

            return matriz[s1.Length, s2.Length];
        }

        private bool ConfirmarNovoGenero(string genero)
        {
            DialogResult resultado = MessageBox.Show(
                $"O gênero '{genero}' não existe na lista padrão.\nDeseja adicioná-lo como um novo gênero?",
                "Novo Gênero",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            return resultado == DialogResult.Yes;
        }

        private void AdicionarNovoGenero(string genero)
        {
            if (!string.IsNullOrWhiteSpace(genero) && !GeneroExiste(genero))
            {
                generosPadronizados.Add(genero.Trim());
                generosPadronizados.Sort(); // Keep the list sorted
                MessageBox.Show($"Gênero '{genero}' adicionado com sucesso!",
                               "Gênero Adicionado",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
        }

        private void FocoTimer_Tick(object sender, EventArgs e)
        {
            focoTimer.Stop(); // para o timer
            this.SelectNextControl(txtGenero, true, true, true, true); // move o foco para o próximo campo
        }

        private void ValidationTimer_Tick(object sender, EventArgs e)
        {
            validationTimer.Stop();
            ValidarGeneroAoSair();
        }

        #endregion

        
    }
}
