using BibliotecaApp.Forms.Usuario;
using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel; // MaskedTextProvider
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace BibliotecaApp
{
    public partial class AlterarCadLivroForm : Form
    {
        #region Estado / Campos
        private int livroId;
        private IButtonControl _acceptBackup, _cancelBackup;

        // Flags de controle do autocomplete
        private bool _suppressGeneroSuggest = false;   // evita loop ao setar Text programaticamente
        private bool _isClickingSugestoes = false;     // zona segura ao clicar na lista
        #endregion

        #region Evento público
        public event EventHandler LivroAtualizado;
        #endregion

        #region Construtor / Init
        public AlterarCadLivroForm()
        {
            InitializeComponent();

            // Prioridade de teclado no Form (ganha do AcceptButton)
            this.KeyPreview = true;

            // Estado inicial da lista de sugestões
            if (lstSugestoesGenero != null)
            {
                lstSugestoesGenero.Visible = false;
                lstSugestoesGenero.TabStop = false;
            }

            // Autocomplete Gênero: wiring
            txtGenero.Enter += txtGenero_Enter;                 // foca => reavalia
            txtGenero.MouseDown += txtGenero_MouseDown;         // clique => reavalia
            txtGenero.TextChanged += txtGenero_TextChanged;     // digitação => reavalia
            txtGenero.KeyUp += txtGenero_KeyUp;                 // reforço de refresh p/ RoundedTextBox
            txtGenero.KeyDown += txtGenero_KeyDown;             // Enter/Tab/Down/Esc
            txtGenero.Leave += txtGenero_Leave;                 // esconder quando sair (com proteção)

            lstSugestoesGenero.MouseDown += lstSugestoesGenero_MouseDown; // marca clique seguro
            lstSugestoesGenero.MouseUp += lstSugestoesGenero_MouseUp;     // libera marca
            lstSugestoesGenero.Click += lstSugestoesGenero_Click;         // clique confirma
            lstSugestoesGenero.KeyDown += lstSugestoesGenero_KeyDown;     // Enter/Tab/Esc
            lstSugestoesGenero.Leave += lstSugestoesGenero_Leave;
        }
        #endregion

        #region Teclado Global
        // Enter/Tab/Down/Escape priorizam a lista quando visível
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (lstSugestoesGenero != null && lstSugestoesGenero.Visible)
            {
                if (keyData == Keys.Enter || keyData == Keys.Tab)
                {
                    if (ConfirmarSugestaoGenero())
                        return true; // consome a tecla
                }
                else if (keyData == Keys.Down)
                {
                    if (!lstSugestoesGenero.Focused)
                    {
                        lstSugestoesGenero.Focus();
                        if (lstSugestoesGenero.Items.Count > 0 && lstSugestoesGenero.SelectedIndex < 0)
                            lstSugestoesGenero.SelectedIndex = 0;
                        return true;
                    }
                }
                else if (keyData == Keys.Escape)
                {
                    EsconderSugestoes();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Carregar Livro
        public void PreencherLivro(Livro livro)
        {
            // Evita disparar sugestões por alteração programática
            _suppressGeneroSuggest = true;

            livroId = livro.Id;
            txtNome.Text = livro.Nome;
            txtAutor.Text = livro.Autor;
            txtGenero.Text = livro.Genero;
            txtQuantidade.Text = livro.Quantidade.ToString();

            // Mostra no UI exatamente o que veio do BD (com/sem máscara)
            mtxCodigoBarras.Text = livro.CodigoDeBarras;

            EsconderSugestoes();

            _suppressGeneroSuggest = false;
        }
        #endregion

        #region Persistência
        private string ObterSenha(string titulo, string mensagem)
        {
            using (var f = new PasswordForm())
            {
                f.Titulo = titulo;
                f.Mensagem = mensagem;
                return f.ShowDialog() == DialogResult.OK ? f.SenhaDigitada : null;
            }
        }

        // === NOVO: Captura com MÁSCARA, mesmo sem TextMaskFormat, usando MaskedTextProvider ===
        private string ObterCodigoDeBarrasFormatado()
        {
            return new string(mtxCodigoBarras.Text.Where(char.IsDigit).ToArray());
        }

 

        private bool VerificarSenhaBibliotecaria(string senha)
        {
            string nome = Sessao.NomeBibliotecariaLogada;
            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Nenhum bibliotecário está logado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            try
            {
                using (var cx = Conexao.ObterConexao())
                {
                    cx.Open();
                    using (var cmd = new SqlCeCommand(
                        @"SELECT Senha_hash, Senha_salt FROM usuarios 
                          WHERE Nome = @n AND TipoUsuario LIKE '%Bibliotec%'", cx))
                    {
                        cmd.Parameters.AddWithValue("@n", nome);
                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                return CriptografiaSenha.VerificarSenha(
                                    senha, r["Senha_hash"].ToString(), r["Senha_salt"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar senha: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string autor = txtAutor.Text.Trim();
            string genero = txtGenero.Text.Trim();
            int quantidade = int.Parse(txtQuantidade.Text);

            // >>> Salvar COM máscara, sempre que possível
            string codigoBarras = ObterCodigoDeBarrasFormatado();

            using (var conn = Conexao.ObterConexao())
            {
                conn.Open();
                string nA = "", aA = "", gA = "", cbA = ""; int qA = 0; bool mudou = false;

                using (var cmdL = new SqlCeCommand(
                    "SELECT Nome, Autor, Genero, Quantidade, CodigoBarras FROM Livros WHERE Id=@id", conn))
                {
                    cmdL.Parameters.AddWithValue("@id", livroId);
                    using (var rd = cmdL.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            nA = rd.GetString(0); aA = rd.GetString(1); gA = rd.GetString(2);
                            qA = rd.GetInt32(3); cbA = rd.GetString(4);
                            mudou = nome != nA || autor != aA || genero != gA || quantidade != qA || codigoBarras != cbA;
                        }
                    }
                }

                if (!mudou) { MessageBox.Show("Nenhuma alteração foi feita."); return; }

                var msg = MontarMensagemConfirmacaoLivro(nA, aA, gA, qA, cbA, nome, autor, genero, quantidade, codigoBarras);
                if (MessageBox.Show(msg, "Confirmar Alterações", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                using (var cmd = new SqlCeCommand(
                    @"UPDATE Livros 
                      SET Nome=@n, Autor=@a, Genero=@g, Quantidade=@q, CodigoBarras=@c 
                      WHERE Id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@n", nome);
                    cmd.Parameters.AddWithValue("@a", autor);
                    cmd.Parameters.AddWithValue("@g", genero);
                    cmd.Parameters.AddWithValue("@q", quantidade);
                    cmd.Parameters.AddWithValue("@c", codigoBarras);
                    cmd.Parameters.AddWithValue("@id", livroId);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Livro atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LivroAtualizado?.Invoke(this, EventArgs.Empty);
                        EventosGlobais.OnLivroCadastradoOuAlterado();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Nenhuma alteração foi feita.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // >>> Somente UMA senha
            var senha = ObterSenha("Confirmação de Senha", "Digite sua senha para confirmar a exclusão:");
            if (string.IsNullOrEmpty(senha)) { MessageBox.Show("Operação cancelada."); return; }
            if (!VerificarSenhaBibliotecaria(senha)) { MessageBox.Show("Senha incorreta."); return; }

            if (MessageBox.Show("Tem certeza que deseja excluir este livro?", "Confirmação",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                using (var conn = Conexao.ObterConexao())
                {
                    conn.Open();
                    using (var cmd = new SqlCeCommand("DELETE FROM Livros WHERE Id=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", livroId);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Livro excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LivroAtualizado?.Invoke(this, EventArgs.Empty);
                            EventosGlobais.OnLivroCadastradoOuAlterado();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Nenhum livro foi excluído.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir o livro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => this.Close();
        #endregion

        #region Mensagem de Confirmação
        private string MontarMensagemConfirmacaoLivro(string nA, string aA, string gA, int qA, string cbA,
                                                      string nN, string aN, string gN, int qN, string cbN)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Confirme as alterações a serem salvas:\n");
            if (nA != nN) sb.AppendLine($"Nome: {nA} → {nN}");
            if (aA != aN) sb.AppendLine($"Autor: {aA} → {aN}");
            if (gA != gN) sb.AppendLine($"Gênero: {gA} → {gN}");
            if (qA != qN) sb.AppendLine($"Quantidade: {qA} → {qN}");
            if (cbA != cbN) sb.AppendLine($"Código de Barras: {cbA} → {cbN}");
            sb.AppendLine("\nDeseja salvar estas alterações?");
            return sb.ToString();
        }
        #endregion

        #region Autocomplete de Gênero
        private readonly List<string> generosPadronizados = new List<string>
        {
            "Poesia","Literatura de Cordel","Biografia","Autobiografia","Diálogo","Hábito","Psicologia",
            "Cultura Afro-brasileira","História","Teatro","Educação","Romance","Ficção","Fantasia",
            "Mitologia","Literatura Infantil","Adolescentes","Infantojuvenil","Suspense","Lenda",
            "Folclore","Novela","Fábula","Narrativa","Afetividade","Letramento","Filosofia",
            "Política","Culinária","Crônica","Conto","Didático","Literatura",
        };

        // Eventos de abertura
        private void txtGenero_Enter(object sender, EventArgs e) { AtualizarSugestoesGenero(); }
        private void txtGenero_MouseDown(object sender, MouseEventArgs e) { AtualizarSugestoesGenero(); }

        // Em RoundedTextBox, TextChanged pode não propagar como no TextBox comum; KeyUp garante refresh.
        private void txtGenero_TextChanged(object sender, EventArgs e)
        {
            if (_suppressGeneroSuggest) return;
            if (!txtGenero.ContainsFocus) return;   // RoundedTextBox: usa ContainsFocus
            AtualizarSugestoesGenero();
        }

        private void txtGenero_KeyUp(object sender, KeyEventArgs e)
        {
            if (_suppressGeneroSuggest) return;
            if (!txtGenero.ContainsFocus) return;

            // Ignora teclas de navegação; o resto dispara refresh
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down ||
                e.KeyCode == Keys.Left || e.KeyCode == Keys.Right ||
                e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab ||
                e.KeyCode == Keys.Escape)
                return;

            AtualizarSugestoesGenero();
        }

        private void AtualizarSugestoesGenero()
        {
            if (_suppressGeneroSuggest) return;

            string texto = (txtGenero.Text ?? string.Empty).Trim();

            // Vazio => esconde; senão filtra por prefixo
            List<string> sug = string.IsNullOrEmpty(texto)
                ? new List<string>()
                : generosPadronizados
                    .Where(g => g.StartsWith(texto, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(g => g)
                    .ToList();

            // Sempre desamarrar antes de reatribuir
            lstSugestoesGenero.DataSource = null;

            if (sug.Count > 0)
            {
                lstSugestoesGenero.DataSource = sug;
                lstSugestoesGenero.Location = new Point(txtGenero.Left, txtGenero.Bottom);
                lstSugestoesGenero.Width = txtGenero.Width;
                lstSugestoesGenero.BringToFront();
                lstSugestoesGenero.Visible = true;

                lstSugestoesGenero.SelectedIndex = 0;

                MutarAcceptCancelEnquantoSugestao(true);
            }
            else
            {
                EsconderSugestoes();
            }
        }

        private void txtGenero_KeyDown(object sender, KeyEventArgs e)
        {
            // Lista fechada: Enter/Tab só navegam
            if (!lstSugestoesGenero.Visible || lstSugestoesGenero.Items.Count == 0)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    e.SuppressKeyPress = true;
                    this.SelectNextControl((Control)sender, true, true, true, true);
                }
                return;
            }

            if (e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
                lstSugestoesGenero.Focus();
                if (lstSugestoesGenero.Items.Count > 0 && lstSugestoesGenero.SelectedIndex < 0)
                    lstSugestoesGenero.SelectedIndex = 0;
            }
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;
                ConfirmarSugestaoGenero();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                EsconderSugestoes();
            }
        }

        private void txtGenero_Leave(object sender, EventArgs e)
        {
            // Se ainda está dentro do RoundedTextBox (filhos), não esconda
            if (txtGenero.ContainsFocus) return;

            // Se o mouse está clicando na lista, não esconda aqui — deixe a lista receber o foco
            if (_isClickingSugestoes) return;

            if (!lstSugestoesGenero.Focused)
                EsconderSugestoes();
        }

        private void lstSugestoesGenero_MouseDown(object sender, MouseEventArgs e) { _isClickingSugestoes = true; }
        private void lstSugestoesGenero_MouseUp(object sender, MouseEventArgs e) { _isClickingSugestoes = false; }

        private void lstSugestoesGenero_Click(object sender, EventArgs e)
        {
            if (lstSugestoesGenero.SelectedIndex >= 0)
                SelecionarGenero(lstSugestoesGenero.SelectedIndex);
        }

        private void lstSugestoesGenero_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;
                ConfirmarSugestaoGenero();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                EsconderSugestoes();
                txtGenero.Focus();
            }
        }

        private void lstSugestoesGenero_Leave(object sender, EventArgs e)
        {
            EsconderSugestoes();
        }

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

            // Suprime TextChanged enquanto atualiza o texto programaticamente
            _suppressGeneroSuggest = true;
            txtGenero.Text = lstSugestoesGenero.Items[index].ToString();
            _suppressGeneroSuggest = false;

            EsconderSugestoes();

            // Enter/Tab seguem o tab order
            this.SelectNextControl(txtGenero, true, true, true, true);
        }
        #endregion

        #region Infra
        private void EsconderSugestoes()
        {
            lstSugestoesGenero.Visible = false;
            lstSugestoesGenero.DataSource = null;
            MutarAcceptCancelEnquantoSugestao(false);
        }

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
