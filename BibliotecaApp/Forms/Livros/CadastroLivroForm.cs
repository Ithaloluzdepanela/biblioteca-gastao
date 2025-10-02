using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Livros
{
    public partial class CadastroLivroForm : Form
    {
        #region Propriedades e Campos
        public event EventHandler LivroAtualizado;

        private readonly List<string> generosPadronizados = new List<string>
        {
            "Poesia", "Literatura de Cordel", "Biografia", "Autobiografia", "Diálogo",
            "Hábito", "Psicologia", "Cultura Afro-brasileira", "História", "Teatro",
            "Educação", "Romance", "Ficção", "Fantasia", "Mitologia", "Literatura Infantil",
            "Adolescentes", "Infantojuvenil", "Suspense", "Lenda", "Folclore", "Novela",
            "Fábula", "Narrativa", "Afetividade", "Letramento", "Filosofia",
            "Política", "Culinária", "Crônica", "Conto", "Didático", "Literatura",
        };

        private bool generoSelecionadoDaLista = false;
        private Timer focoTimer;
        private Timer validationTimer;

        // Backup para “mutar” Accept/Cancel enquanto a lista está aberta
        private IButtonControl _acceptBackup, _cancelBackup;
        #endregion

        #region Inicialização do Formulário
        public CadastroLivroForm()
        {
            focoTimer = new Timer { Interval = 50 };
            focoTimer.Tick += FocoTimer_Tick;

            validationTimer = new Timer { Interval = 100 };
            validationTimer.Tick += ValidationTimer_Tick;

            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            ConfigurarEventos();
            ConfigurarNavegacao();

            if (lstSugestoesGenero != null) lstSugestoesGenero.TabStop = false;
        }

        private void CadastroLivroForm_Load(object sender, EventArgs e)
        {
            // Mantido por compatibilidade, sem lógica adicional
        }

        private void ConfigurarEventos()
        {
            // Sugestões de gênero
            txtGenero.TextChanged += txtGenero_TextChanged;
            txtGenero.KeyDown += txtGenero_KeyDown;
            txtGenero.Leave += txtGenero_Leave;

            lstSugestoesGenero.Click += lstSugestoesGenero_Click;
            lstSugestoesGenero.KeyDown += lstSugestoesGenero_KeyDown;
            lstSugestoesGenero.Leave += lstSugestoesGenero_Leave;

            // Botões (se não estiverem no designer)
            // btnCadastrar.Click += btnCadastrar_Click;
            // btnLimpar.Click += btnLimpar_Click;
        }

        private void ConfigurarNavegacao()
        {
            // Captura teclas no form antes dos filhos (ganha do AcceptButton)
            this.KeyPreview = true;
        }
        #endregion

        #region Overrides de Teclado (prioridade para a lista)
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (lstSugestoesGenero != null && lstSugestoesGenero.Visible)
            {
                if (keyData == Keys.Enter)
                {
                    if (ConfirmarSugestaoGenero())
                        return true;
                }
                else if (keyData == Keys.Down)
                {
                    if (!lstSugestoesGenero.Focused)
                    {
                        validationTimer.Stop();                  // <- evita validação indevida
                        lstSugestoesGenero.Focus();
                        if (lstSugestoesGenero.Items.Count > 0 && lstSugestoesGenero.SelectedIndex < 0)
                            lstSugestoesGenero.SelectedIndex = 0;
                        return true;
                    }
                }
                else if (keyData == Keys.Tab)
                {
                    // Tab confirma a sugestão e segue fluxo
                    if (ConfirmarSugestaoGenero())
                        return true;
                }
                else if (keyData == Keys.Escape)
                {
                    lstSugestoesGenero.Visible = false;
                    MutarAcceptCancelEnquantoSugestao(false);
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Eventos dos Botões
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                "Tem certeza de que deseja limpar tudo?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
                LimparFormulario();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;
            if (!ValidarQuantidade(out int quantidade)) return;
            if (!ValidarCodigoBarras(out string codigoBarras)) return;

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
                lstSugestoesGenero.DataSource = null;
                MutarAcceptCancelEnquantoSugestao(false);
                return;
            }

            // Ranking: prefixo primeiro; depois Levenshtein <= 2
            var lower = texto.ToLower();
            var sugestoes = generosPadronizados
                .Where(g => g.StartsWith(texto, StringComparison.OrdinalIgnoreCase)
                         || CalcularDistanciaLevenshtein(g.ToLower(), lower) <= 2)
                .OrderBy(g => g.StartsWith(texto, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                .ThenBy(g => CalcularDistanciaLevenshtein(g.ToLower(), lower))
                .ThenBy(g => g)
                .ToList();

            if (sugestoes.Any())
            {
                lstSugestoesGenero.DataSource = sugestoes;
                lstSugestoesGenero.Visible = true;

                // posiciona e traz pra frente
                lstSugestoesGenero.Location = new Point(txtGenero.Left, txtGenero.Bottom);
                lstSugestoesGenero.Width = txtGenero.Width;
                lstSugestoesGenero.BringToFront();

                // seleciona o primeiro por padrão
                if (lstSugestoesGenero.Items.Count > 0)
                    lstSugestoesGenero.SelectedIndex = 0;

                // Muta Accept/Cancel enquanto a lista estiver aberta
                MutarAcceptCancelEnquantoSugestao(true);
            }
            else
            {
                lstSugestoesGenero.Visible = false;
                lstSugestoesGenero.DataSource = null;
                MutarAcceptCancelEnquantoSugestao(false);
            }
        }

        private void txtGenero_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesGenero.Visible || lstSugestoesGenero.Items.Count == 0)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
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
                    validationTimer.Stop();                      // <- para não validar ao focar a lista
                    lstSugestoesGenero.Focus();
                    if (lstSugestoesGenero.Items.Count > 0 && lstSugestoesGenero.SelectedIndex < 0)
                        lstSugestoesGenero.SelectedIndex = 0;
                    break;

                case Keys.Enter:
                case Keys.Tab:                                   // <- Tab confirma e avança
                    e.SuppressKeyPress = true;
                    ConfirmarSugestaoGenero();
                    break;

                case Keys.Escape:
                    e.SuppressKeyPress = true;
                    lstSugestoesGenero.Visible = false;
                    MutarAcceptCancelEnquantoSugestao(false);
                    break;
            }
        }

        private void txtGenero_Leave(object sender, EventArgs e)
        {
            // Se a lista está aberta (ou iremos para ela), não validar agora
            if (lstSugestoesGenero.Visible)
                return;

            validationTimer.Stop();
            validationTimer.Start();
        }

        private void lstSugestoesGenero_Click(object sender, EventArgs e)
        {
            if (lstSugestoesGenero.SelectedItem != null)
            {
                SelecionarGenero(lstSugestoesGenero.SelectedIndex);
            }
        }

        private void lstSugestoesGenero_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Tab:                                   // <- Tab também confirma na lista
                    e.SuppressKeyPress = true;
                    ConfirmarSugestaoGenero();
                    break;

                case Keys.Escape:
                    e.SuppressKeyPress = true;
                    lstSugestoesGenero.Visible = false;
                    MutarAcceptCancelEnquantoSugestao(false);
                    txtGenero.Focus();
                    break;
            }
        }

        private void lstSugestoesGenero_Leave(object sender, EventArgs e)
        {
            lstSugestoesGenero.Visible = false;
            MutarAcceptCancelEnquantoSugestao(false);
        }
        #endregion

        #region Validações
        private void ValidationTimer_Tick(object sender, EventArgs e)
        {
            validationTimer.Stop();

            // Se a lista estiver visível, adia a validação (evita popup indevido)
            if (lstSugestoesGenero.Visible)
                return;

            ValidarGeneroAoSair();
        }

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
            // Se veio da lista, não precisa validar
            if (generoSelecionadoDaLista)
            {
                generoSelecionadoDaLista = false;
                lstSugestoesGenero.Visible = false;
                MutarAcceptCancelEnquantoSugestao(false);
                return;
            }

            string entrada = txtGenero.Text.Trim();

            // Se vazio, nada a validar
            if (string.IsNullOrEmpty(entrada))
            {
                lstSugestoesGenero.Visible = false;
                MutarAcceptCancelEnquantoSugestao(false);
                return;
            }

            // Não validar se a lista está aberta (proteção extra)
            if (lstSugestoesGenero.Visible)
                return;

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
            MutarAcceptCancelEnquantoSugestao(false);
        }
        #endregion

        #region Banco de Dados
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

                    // Broadcast para outras telas
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

        #region Utilitários
        private void LimparFormulario()
        {
            txtNome.Text = "";
            txtAutor.Text = "";
            txtGenero.Text = "";
            txtQuantidade.Text = "";
            mtxCodigoBarras.Text = "";
            lstSugestoesGenero.Visible = false;
            lstSugestoesGenero.DataSource = null;
            MutarAcceptCancelEnquantoSugestao(false);
            txtNome.Focus();
        }

        private string ObterCodigoDeBarrasFormatado()
        {
            return new string(mtxCodigoBarras.Text.Where(char.IsDigit).ToArray());
        }

        private void FocoTimer_Tick(object sender, EventArgs e)
        {
            focoTimer.Stop();
            this.SelectNextControl(txtGenero, true, true, true, true);
        }

        private bool GeneroExiste(string entrada)
        {
            return generosPadronizados
                .Any(g => string.Equals(g.Trim(), entrada.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private bool GeneroExisteOuSimilar(string entrada)
        {
            if (GeneroExiste(entrada)) return true;
            var lower = entrada.ToLower();
            return generosPadronizados.Any(g => CalcularDistanciaLevenshtein(g.ToLower(), lower) <= 1);
        }

        private int CalcularDistanciaLevenshtein(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1)) return s2?.Length ?? 0;
            if (string.IsNullOrEmpty(s2)) return s1.Length;

            int[,] matriz = new int[s1.Length + 1, s2.Length + 1];
            for (int i = 0; i <= s1.Length; i++) matriz[i, 0] = i;
            for (int j = 0; j <= s2.Length; j++) matriz[0, j] = j;

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
            var resultado = MessageBox.Show(
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
                generosPadronizados.Sort();
                MessageBox.Show($"Gênero '{genero}' adicionado com sucesso!",
                               "Gênero Adicionado",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
        }
        #endregion

        #region Fluxo de Seleção (confirma/avança)
        private bool ConfirmarSugestaoGenero()
        {
            if (!lstSugestoesGenero.Visible || lstSugestoesGenero.Items.Count == 0)
                return false;

            int idx = lstSugestoesGenero.SelectedIndex;
            if (idx < 0) idx = 0;
            SelecionarGenero(idx);
            return true;
        }

        private void SelecionarGenero(int index)
        {
            if (index < 0 || index >= lstSugestoesGenero.Items.Count)
                return;

            string genero = lstSugestoesGenero.Items[index].ToString();
            generoSelecionadoDaLista = true;
            txtGenero.Text = genero;

            lstSugestoesGenero.Visible = false;
            lstSugestoesGenero.DataSource = null;
            MutarAcceptCancelEnquantoSugestao(false);

            // Enter/Tab seguem o tab order
            this.SelectNextControl(txtGenero, true, true, true, true);
        }
        #endregion

        #region Infra de Accept/Cancel “mute”
        private void MutarAcceptCancelEnquantoSugestao(bool mutar)
        {
            if (mutar)
            {
                if (_acceptBackup == null) _acceptBackup = this.AcceptButton;
                if (_cancelBackup == null) _cancelBackup = this.CancelButton;
                this.AcceptButton = null;
                this.CancelButton = null;
            }
            else
            {
                this.AcceptButton = _acceptBackup;
                this.CancelButton = _cancelBackup;
            }
        }
        #endregion
    }
}
