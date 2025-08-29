using BibliotecaApp.Models;
using BibliotecaApp.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Livros
{
    public partial class ReservaForm : Form
    {
        // caches e listas
        private List<Usuarios> Usuarios = new List<Usuarios>();
        private List<Livro> Livros = new List<Livro>();
        private List<Usuarios> _cacheUsuarios = new List<Usuarios>();
        private List<Livro> _cacheLivros = new List<Livro>();

        // controle de hover para listbox
        private int hoveredIndex = -1;

        public ReservaForm()
        {
            InitializeComponent();

            // eventos principais
            this.Load += ReservaForm_Load;

            // liga handlers adicionais (alguns já estão no Designer; ligar aqui evita faltar)
            txtNomeUsuario.TextChanged += txtNomeUsuario_TextChanged;
            txtNomeUsuario.KeyDown += txtNomeUsuario_KeyDown;
            txtNomeUsuario.Leave += txtNomeUsuario_Leave;

            txtLivro.TextChanged += txtLivro_TextChanged;
            txtLivro.KeyDown += txtLivro_KeyDown;
            txtLivro.Leave += txtLivro_Leave;

            txtBarcode.Leave += txtBarcode_Leave;

            btnReservar.Click += btnReservar_Click;
            btnCancelar.Click += btnCancelar_Click; // tratamento do botão cancelar

            // estilizar listboxes como no EmprestimoForm
            EstilizarListBoxSugestao(lstSugestoesUsuario);
            EstilizarListBoxSugestao(lstSugestoesLivros);

            // assegura que handlers do designer estão conectados corretamente
            lstSugestoesUsuario.Click -= lstSugestoesUsuario_Click;
            lstSugestoesUsuario.Click += lstSugestoesUsuario_Click;
            lstSugestoesUsuario.SelectedIndexChanged -= lstSugestoesUsuario_SelectedIndexChanged;
            lstSugestoesUsuario.SelectedIndexChanged += lstSugestoesUsuario_SelectedIndexChanged;
            lstSugestoesUsuario.KeyDown -= lstSugestoesUsuario_KeyDown;
            lstSugestoesUsuario.KeyDown += lstSugestoesUsuario_KeyDown;

            lstSugestoesLivros.Click -= lstSugestoesLivros_Click;
            lstSugestoesLivros.Click += lstSugestoesLivros_Click;
            lstSugestoesLivros.SelectedIndexChanged -= lstSugestoesLivros_SelectedIndexChanged;
            lstSugestoesLivros.SelectedIndexChanged += lstSugestoesLivros_SelectedIndexChanged;
            lstSugestoesLivros.KeyDown -= lstSugestoesLivros_KeyDown;
            lstSugestoesLivros.KeyDown += lstSugestoesLivros_KeyDown;
        }

        private void ReservaForm_Load(object sender, EventArgs e)
        {
            // garante alinhamento/ancoragem como solicitado
            try
            {
                this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            }
            catch { /* se o designer nomear diferente, ignore */ }

            CarregarDadosIniciais();
        }

        /// <summary>
        /// Preenche a reserva com informações vindas do formulário de empréstimo.
        /// Chame antes de ShowDialog.
        /// </summary>
        public void PreFillFromEmprestimo(Usuarios usuario, Livro livro, Usuarios bibliotecaria, string codigoBarras, DateTime sugestaoDataDevolucao)
        {
            // assegura dados carregados
            if (Usuarios.Count == 0 || Livros.Count == 0)
                CarregarDadosIniciais();

            if (usuario != null)
            {
                txtNomeUsuario.Text = usuario.Nome;
            }

            if (livro != null)
            {
                txtLivro.Text = livro.Nome;
                txtBarcode.Text = !string.IsNullOrWhiteSpace(codigoBarras) ? codigoBarras : (livro.CodigoDeBarras ?? "");
                lstSugestoesLivros.Visible = false;
            }

            if (bibliotecaria != null && !string.IsNullOrWhiteSpace(bibliotecaria.Nome))
            {
                var idx = cbBibliotecaria.Items.IndexOf(bibliotecaria.Nome);
                if (idx >= 0) cbBibliotecaria.SelectedIndex = idx;
            }

            dtpDataReserva.Value = DateTime.Today;
        }

        #region Carregamento Inicial

        private void CarregarDadosIniciais()
        {
            CarregarUsuariosDoBanco();
            CarregarLivrosIndisponiveisDoBanco();
            CarregarBibliotecarias();

            // caches para buscas rápidas
            _cacheUsuarios = new List<Usuarios>(Usuarios);
            _cacheLivros = new List<Livro>(Livros);
        }

        private void CarregarUsuariosDoBanco()
        {
            Usuarios.Clear();
            _cacheUsuarios.Clear();
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT Id, Nome, TipoUsuario, Email, Turma FROM usuarios ORDER BY Nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var usuario = new Usuarios
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                TipoUsuario = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Turma = reader.IsDBNull(4) ? "" : reader.GetString(4)
                            };
                            Usuarios.Add(usuario);
                            _cacheUsuarios.Add(usuario);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar usuários: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarLivrosIndisponiveisDoBanco()
        {
            Livros.Clear();
            _cacheLivros.Clear();
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT Id, Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade
                                   FROM Livros
                                   WHERE Disponibilidade = 0 OR Quantidade = 0
                                   ORDER BY Nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var livro = new Livro(
                                reader.IsDBNull(1) ? "" : reader.GetString(1),
                                reader.IsDBNull(2) ? "" : reader.GetString(2),
                                reader.IsDBNull(3) ? "" : reader.GetString(3),
                                reader.IsDBNull(6) ? false : reader.GetBoolean(6),
                                reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                reader.IsDBNull(5) ? "" : reader.GetString(5)
                            );
                            if (!reader.IsDBNull(0))
                                typeof(Livro).GetProperty("Id").SetValue(livro, reader.GetInt32(0));
                            Livros.Add(livro);
                            _cacheLivros.Add(livro);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar livros: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarBibliotecarias()
        {
            cbBibliotecaria.Items.Clear();
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT Nome FROM usuarios WHERE TipoUsuario LIKE @tipo ORDER BY Nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@tipo", "%Bibliotec%");
                        using (var r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                if (!r.IsDBNull(0))
                                {
                                    var nome = r.GetString(0);
                                    if (!cbBibliotecaria.Items.Contains(nome))
                                        cbBibliotecaria.Items.Add(nome);
                                }
                            }
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(Sessao.NomeBibliotecariaLogada))
                {
                    var idx = cbBibliotecaria.Items.IndexOf(Sessao.NomeBibliotecariaLogada);
                    if (idx >= 0) cbBibliotecaria.SelectedIndex = idx;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar bibliotecárias: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Reserva / Cancelar

        private void btnReservar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var usuario = ObterUsuarioSelecionado();
            var livro = ObterLivroSelecionado();
            var bibliotecaria = ObterBibliotecariaSelecionada();

            if (!ValidarSelecoes(usuario, livro, bibliotecaria)) return;

            if (!ValidarDisponibilidade(livro)) return;

            if (VerificarReservaAtiva(usuario.Id, livro.Id)) return;

            DateTime dataReserva = dtpDataReserva.Value;

            bool ok = RegistrarReserva(usuario, livro, bibliotecaria, dataReserva);
            if (ok)
            {
                MessageBox.Show("Reserva registrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // retorna OK para o EmprestimoForm
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // RegistrarReserva já mostrou o erro interno; apenas mantenha o formulário aberto
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Apenas fecha e retorna Cancel para o EmprestimoForm
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNomeUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtLivro.Text) ||
                cbBibliotecaria.SelectedIndex == -1)
            {
                MessageBox.Show("Preencha todos os campos antes de reservar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private Usuarios ObterUsuarioSelecionado()
        {
            return _cacheUsuarios.FirstOrDefault(u => u.Nome.Equals(txtNomeUsuario.Text, StringComparison.OrdinalIgnoreCase));
        }

        private Livro ObterLivroSelecionado()
        {
            return _cacheLivros.FirstOrDefault(l => l.Nome.Equals(txtLivro.Text, StringComparison.OrdinalIgnoreCase));
        }

        private Usuarios ObterBibliotecariaSelecionada()
        {
            if (cbBibliotecaria.SelectedItem == null) return null;
            return Usuarios.FirstOrDefault(u => u.Nome.Equals(cbBibliotecaria.SelectedItem.ToString(), StringComparison.OrdinalIgnoreCase));
        }

        private bool ValidarSelecoes(Usuarios usuario, Livro livro, Usuarios bibliotecaria)
        {
            if (usuario == null || livro == null || bibliotecaria == null)
            {
                MessageBox.Show("Usuário, livro ou bibliotecária não encontrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        #endregion

        #region Banco: registrar / utilitários

        /// <summary>
        /// Registra a reserva e retorna true se tudo correu bem.
        /// Não exibe MessageBox de sucesso — isso fica no chamador.
        /// </summary>
        private bool RegistrarReserva(Usuarios usuario, Livro livro, Usuarios bibliotecaria, DateTime dataReserva)
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    var dataDisponibilidade = ObterDataProximaDisponibilidade(livro.Id);
                    var dataLimiteRetirada = dataDisponibilidade.AddDays(7);

                    string sql = @"
                        INSERT INTO Reservas 
                            (UsuarioId, LivroId, BibliotecariaId, DataReserva, DataDisponibilidade, DataLimiteRetirada, Status)
                        VALUES
                            (@usuarioId, @livroId, @bibliotecariaId, @DataReserva, @DataDisponibilidade, @DataLimiteRetirada, 'Pendente')";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@usuarioId", usuario.Id);
                        cmd.Parameters.AddWithValue("@livroId", livro.Id);
                        cmd.Parameters.AddWithValue("@bibliotecariaId", bibliotecaria.Id);
                        cmd.Parameters.AddWithValue("@DataReserva", dataReserva);
                        cmd.Parameters.AddWithValue("@DataDisponibilidade", dataDisponibilidade);
                        cmd.Parameters.AddWithValue("@DataLimiteRetirada", dataLimiteRetirada);
                        cmd.ExecuteNonQuery();
                    }

                    // opcional: enviar email de confirmação (se existir EmailService)
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(usuario.Email) && ValidarEmailStatic(usuario.Email))
                        {
                            string assunto = "Reserva criada - Biblioteca";
                            string corpo = $"Olá {usuario.Nome}, sua reserva do livro '{livro.Nome}' foi registrada.";
                            BibliotecaApp.Services.EmailService.Enviar(usuario.Email, assunto, corpo);
                        }
                    }
                    catch
                    {
                        // não importa se o e-mail falhar — a reserva já foi salva
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao registrar reserva: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private DateTime ObterDataProximaDisponibilidade(int livroId)
        {
            using (var conexao = EmprestimoForm.Conexao.ObterConexao())
            {
                conexao.Open();
                string sql = @"
                    SELECT MIN(
                        CASE 
                            WHEN DataProrrogacao IS NOT NULL THEN DataProrrogacao
                            ELSE DataDevolucao
                        END
                    )
                    FROM Emprestimo
                    WHERE Livro = @livroId 
                      AND Status IN ('Ativo', 'Atrasado')";

                using (var cmd = new SqlCeCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@livroId", livroId);
                    var resultado = cmd.ExecuteScalar();
                    if (resultado != null && resultado != DBNull.Value)
                        return (DateTime)resultado;
                }
            }
            return DateTime.Today;
        }

        private bool VerificarReservaAtiva(int usuarioId, int livroId)
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT COUNT(*) FROM Reservas
                                   WHERE UsuarioId = @usuarioId AND LivroId = @livroId
                                   AND Status IN ('Pendente','Disponível')";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                        cmd.Parameters.AddWithValue("@livroId", livroId);
                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("O usuário já possui reserva ativa para este livro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar reservas: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private bool ValidarDisponibilidade(Livro livro)
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();

                    // pega quantidade total
                    string sqlTotal = @"SELECT Quantidade FROM Livros WHERE Id = @id";
                    int quantidadeTotal = 0;
                    using (var cmd = new SqlCeCommand(sqlTotal, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", livro.Id);
                        var res = cmd.ExecuteScalar();
                        quantidadeTotal = (res != null && res != DBNull.Value) ? Convert.ToInt32(res) : 0;
                    }

                    // emprestimos ativos
                    sqlTotal = @"SELECT COUNT(*) FROM Emprestimo WHERE Livro = @livroId AND Status IN ('Ativo','Atrasado')";
                    int emprestimosAtivos = 0;
                    using (var cmd = new SqlCeCommand(sqlTotal, conexao))
                    {
                        cmd.Parameters.AddWithValue("@livroId", livro.Id);
                        emprestimosAtivos = (int)cmd.ExecuteScalar();
                    }

                    // reservas ativas
                    sqlTotal = @"SELECT COUNT(*) FROM Reservas WHERE LivroId = @livroId AND Status IN ('Pendente','Disponível')";
                    int reservasAtivas = 0;
                    using (var cmd = new SqlCeCommand(sqlTotal, conexao))
                    {
                        cmd.Parameters.AddWithValue("@livroId", livro.Id);
                        reservasAtivas = (int)cmd.ExecuteScalar();
                    }

                    if (emprestimosAtivos == 0)
                    {
                        MessageBox.Show("Não há exemplares emprestados no momento.\nUse o formulário de empréstimo para retirar o livro agora.", "Sem Empréstimos Ativos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }

                    if (reservasAtivas >= emprestimosAtivos)
                    {
                        MessageBox.Show("Todas as cópias emprestadas já têm reserva ativa — aguarde liberação.", "Reservas Esgotadas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar disponibilidade: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region Autocomplete / ListBox handlers

        private void txtNomeUsuario_TextChanged(object sender, EventArgs e)
        {
            string nomeBusca = txtNomeUsuario.Text.Trim();
            lstSugestoesUsuario.Items.Clear();
            lstSugestoesUsuario.Visible = false;

            if (string.IsNullOrWhiteSpace(nomeBusca)) return;

            foreach (var u in _cacheUsuarios.Where(x => x.Nome.StartsWith(nomeBusca, StringComparison.OrdinalIgnoreCase)))
            {
                lstSugestoesUsuario.Items.Add($"{u.Nome} - {u.Turma}");
            }

            lstSugestoesUsuario.Visible = lstSugestoesUsuario.Items.Count > 0;
        }

        private void txtNomeUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesUsuario.Visible) return;
            if (e.KeyCode == Keys.Down && lstSugestoesUsuario.SelectedIndex < lstSugestoesUsuario.Items.Count - 1)
            {
                e.Handled = true; lstSugestoesUsuario.SelectedIndex++;
            }
            else if (e.KeyCode == Keys.Up && lstSugestoesUsuario.SelectedIndex > 0)
            {
                e.Handled = true; lstSugestoesUsuario.SelectedIndex--;
            }
            else if (e.KeyCode == Keys.Enter && lstSugestoesUsuario.SelectedIndex >= 0)
            {
                e.Handled = true; SelecionarUsuarioDaLista();
            }
        }

        private void txtNomeUsuario_Leave(object sender, EventArgs e)
        {
            if (!lstSugestoesUsuario.Focused) lstSugestoesUsuario.Visible = false;
        }

        private void lstSugestoesUsuario_Click(object sender, EventArgs e) => SelecionarUsuarioDaLista();
        private void lstSugestoesUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { SelecionarUsuarioDaLista(); e.Handled = true; }
        }
        private void lstSugestoesUsuario_Leave(object sender, EventArgs e) => lstSugestoesUsuario.Visible = false;

        // Esse handler é referenciado no Designer — garante compatibilidade
        private void lstSugestoesUsuario_SelectedIndexChanged(object sender, EventArgs e) => SelecionarUsuarioDaLista();

        private void SelecionarUsuarioDaLista()
        {
            if (lstSugestoesUsuario.SelectedIndex >= 0)
            {
                var text = lstSugestoesUsuario.SelectedItem.ToString();
                var nome = text.Split(new[] { " - " }, StringSplitOptions.None)[0];
                txtNomeUsuario.Text = nome;
                lstSugestoesUsuario.Visible = false;
                txtLivro.Focus();
            }
        }

        // livros
        private void txtLivro_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtLivro.Text.Trim();
            lstSugestoesLivros.Items.Clear();
            lstSugestoesLivros.Visible = false;

            if (string.IsNullOrWhiteSpace(filtro)) return;

            foreach (var l in _cacheLivros.Where(x => x.Nome.StartsWith(filtro, StringComparison.OrdinalIgnoreCase)))
            {
                lstSugestoesLivros.Items.Add($"{l.Nome} - {l.Autor}");
            }
            lstSugestoesLivros.Visible = lstSugestoesLivros.Items.Count > 0;
        }

        private void txtLivro_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesLivros.Visible) return;
            if (e.KeyCode == Keys.Down && lstSugestoesLivros.SelectedIndex < lstSugestoesLivros.Items.Count - 1)
            {
                e.Handled = true; lstSugestoesLivros.SelectedIndex++;
            }
            else if (e.KeyCode == Keys.Up && lstSugestoesLivros.SelectedIndex > 0)
            {
                e.Handled = true; lstSugestoesLivros.SelectedIndex--;
            }
            else if (e.KeyCode == Keys.Enter && lstSugestoesLivros.SelectedIndex >= 0)
            {
                e.Handled = true; SelecionarLivroDaLista();
            }
        }

        private void txtLivro_Leave(object sender, EventArgs e)
        {
            if (!lstSugestoesLivros.Focused) lstSugestoesLivros.Visible = false;
        }

        private void lstSugestoesLivros_Click(object sender, EventArgs e) => SelecionarLivroDaLista();
        private void lstSugestoesLivros_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { SelecionarLivroDaLista(); e.Handled = true; }
        }
        private void lstSugestoesLivros_Leave(object sender, EventArgs e) => lstSugestoesLivros.Visible = false;

        // Esse handler é referenciado no Designer — garante compatibilidade
        private void lstSugestoesLivros_SelectedIndexChanged(object sender, EventArgs e) => SelecionarLivroDaLista();

        private void SelecionarLivroDaLista()
        {
            if (lstSugestoesLivros.SelectedIndex >= 0)
            {
                var text = lstSugestoesLivros.SelectedItem.ToString();
                var nome = text.Split(new[] { " - " }, StringSplitOptions.None)[0];
                txtLivro.Text = nome;
                lstSugestoesLivros.Visible = false;
                txtBarcode.Focus();
            }
        }

        private void txtBarcode_Leave(object sender, EventArgs e)
        {
            // opcional: buscar informações do livro por barcode (se desejar)
            var codigo = txtBarcode.Text.Trim();
            if (string.IsNullOrEmpty(codigo)) return;

            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT Nome FROM Livros WHERE CodigoBarras = @codigo";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@codigo", codigo);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtLivro.Text = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            }
                            else
                            {
                                MessageBox.Show("Livro não encontrado. Escaneie novamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtBarcode.Focus();
                                txtBarcode.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar o livro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ListBox styling (copiado do EmprestimoForm)

        private void EstilizarListBoxSugestao(ListBox listBox)
        {
            listBox.DrawMode = DrawMode.OwnerDrawFixed;
            listBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            listBox.ItemHeight = 40;

            listBox.BackColor = Color.White;
            listBox.ForeColor = Color.FromArgb(30, 61, 88);
            listBox.BorderStyle = BorderStyle.FixedSingle;
            listBox.IntegralHeight = false;

            // (re)atribui para evitar handlers duplicados múltiplos ao reconstruir
            listBox.DrawItem -= ListBoxSugestao_DrawItem;
            listBox.DrawItem += ListBoxSugestao_DrawItem;

            listBox.MouseMove -= ListBoxSugestao_MouseMove;
            listBox.MouseMove += ListBoxSugestao_MouseMove;

            listBox.MouseLeave -= ListBoxSugestao_MouseLeave;
            listBox.MouseLeave += ListBoxSugestao_MouseLeave;
        }

        private void ListBoxSugestao_DrawItem(object sender, DrawItemEventArgs e)
        {
            var listBox = sender as ListBox;
            if (e.Index < 0) return;

            bool hovered = (e.Index == hoveredIndex);

            Color backColor = hovered ? Color.FromArgb(235, 235, 235) : Color.White;
            Color textColor = Color.FromArgb(60, 60, 60);

            using (SolidBrush b = new SolidBrush(backColor))
                e.Graphics.FillRectangle(b, e.Bounds);

            string text = listBox.Items[e.Index].ToString();
            Font font = listBox.Font;
            Rectangle textRect = new Rectangle(e.Bounds.Left + 12, e.Bounds.Top, e.Bounds.Width - 24, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, text, font, textRect, textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            if (e.Index < listBox.Items.Count - 1)
            {
                using (Pen p = new Pen(Color.FromArgb(220, 220, 220)))
                    e.Graphics.DrawLine(p, e.Bounds.Left + 8, e.Bounds.Bottom - 1, e.Bounds.Right - 8, e.Bounds.Bottom - 1);
            }
        }

        private void ListBoxSugestao_MouseMove(object sender, MouseEventArgs e)
        {
            var listBox = sender as ListBox;
            int index = listBox.IndexFromPoint(e.Location);
            if (index != hoveredIndex)
            {
                hoveredIndex = index;
                listBox.Invalidate();
            }
        }

        private void ListBoxSugestao_MouseLeave(object sender, EventArgs e)
        {
            hoveredIndex = -1;
            (sender as ListBox).Invalidate();
        }

        #endregion

        #region Métodos estáticos auxiliares solicitados pelo build

        /// <summary>
        /// Envia e-mail informando que a reserva está disponível — versão "forte" que recebe modelos.
        /// dataLimiteRetirada é opcional; se não informado, será dataDisponibilidade + 7 dias.
        /// </summary>
        public static void EnviarEmailDisponibilidadeStatic(Usuarios usuario, Livro livro, DateTime dataDisponibilidade, DateTime? dataLimiteRetirada = null)
        {
            try
            {
                if (usuario == null || string.IsNullOrWhiteSpace(usuario.Email))
                    return;

                if (!ValidarEmailStatic(usuario.Email))
                    return;

                DateTime dataLimite = dataLimiteRetirada ?? dataDisponibilidade.AddDays(7);

                string assunto = $"📚 Reserva disponível: {(livro != null ? livro.Nome : "seu livro")}";
                string corpo = $@"
<html>
<body style='font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
        <h2 style='color: #2c3e50;'>Olá, {System.Net.WebUtility.HtmlEncode(usuario.Nome)} 👋</h2>
        <p>Sua reserva do livro <strong>{(livro != null ? System.Net.WebUtility.HtmlEncode(livro.Nome) : "—")}</strong> já está disponível.</p>
        <p><strong>📅 Data de disponibilidade:</strong> {dataDisponibilidade:dd/MM/yyyy}</p>
        <p><strong>📅 Data limite para retirada:</strong> {dataLimite:dd/MM/yyyy}</p>
        <p>Por favor, retire o exemplar dentro do prazo para não perder a reserva.</p>
        <hr />
        <p style='font-size: 14px; color: #888;'>Este é um e-mail automático enviado pela Biblioteca Monteiro Lobato.</p>
    </div>
</body>
</html>";

                // Usa EmailService existente (ajuste se seu EmailService precisar de parâmetros diferentes)
                BibliotecaApp.Services.EmailService.Enviar(usuario.Email, assunto, corpo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Falha ao enviar email de disponibilidade: " + ex.Message);
            }
        }

        /// <summary>
        /// Sobrecarga conveniente: aceita dados em strings (email, nomeUsuario, nomeLivro)
        /// e converte em objetos internos antes de chamar a versão principal.
        /// </summary>
        public static void EnviarEmailDisponibilidadeStatic(string email, string nomeUsuario, string nomeLivro, DateTime dataDisponibilidade, DateTime? dataLimiteRetirada = null)
        {
            var usuario = new Usuarios { Nome = nomeUsuario ?? "", Email = email ?? "" };
            var livro = new Livro { Nome = nomeLivro ?? "" };
            EnviarEmailDisponibilidadeStatic(usuario, livro, dataDisponibilidade, dataLimiteRetirada);
        }

        /// <summary>
        /// Valida se um e-mail é válido (forma estática, usada em várias partes do projeto).
        /// </summary>
        public static bool ValidarEmailStatic(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
