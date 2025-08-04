using BibliotecaApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Windows.Forms;

namespace BibliotecaApp
{
    public partial class ReservaForm : Form
    {
        #region Propriedades
        public List<Usuarios> Usuarios { get; set; }
        public List<Livro> Livros { get; set; }
        private List<Livro> _cacheLivros = new List<Livro>();
        private List<Usuarios> _cacheUsuarios = new List<Usuarios>();

        // Controle para evitar duplicação de verificações
        private int _livroAtualSendoVerificado = -1;
        #endregion

        #region Construtores
        public ReservaForm()
        {
            InitializeComponent();
            CarregarDadosIniciais();
            ConfigurarControles();
        }

        private void CarregarDadosIniciais()
        {
            Usuarios = new List<Usuarios>();
            Livros = new List<Livro>();

            CarregarUsuariosDoBanco();
            CarregarLivrosIndisponiveisDoBanco();
            CarregarBibliotecarias();

            dtpDataReserva.Value = DateTime.Today;
        }

        private void ConfigurarControles()
        {
            txtNomeUsuario.TextChanged += txtNomeUsuario_TextChanged;
            txtLivro.TextChanged += txtLivro_TextChanged;
            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;
        }
        #endregion

        #region Métodos de Carregamento
        private void CarregarUsuariosDoBanco()
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT Id, Nome, TipoUsuario FROM usuarios";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuarios.Add(new Usuarios
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                TipoUsuario = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar usuários: " + ex.Message,
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarLivrosIndisponiveisDoBanco()
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT Id, Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade 
                          FROM Livros 
                          WHERE Disponibilidade = 0 AND Quantidade = 0";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var livro = new Livro(
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetBoolean(6),
                                reader.GetInt32(4),
                                reader.GetString(5));

                            typeof(Livro).GetProperty("Id").SetValue(livro, reader.GetInt32(0));
                            Livros.Add(livro);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar livros indisponíveis: " + ex.Message,
                              "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string sql = "SELECT Nome FROM usuarios WHERE TipoUsuario = 'Bibliotecário(a)'";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbBibliotecaria.Items.Add(reader.GetString(0));
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(Sessao.NomeBibliotecariaLogada))
                {
                    cbBibliotecaria.SelectedIndex = cbBibliotecaria.Items.IndexOf(Sessao.NomeBibliotecariaLogada);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar bibliotecárias: " + ex.Message,
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Métodos de Reserva (Atualizados)
        private void btnReservar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var usuario = ObterUsuarioSelecionado();
            var livro = ObterLivroSelecionado();
            var bibliotecaria = ObterBibliotecariaSelecionada();

            if (!ValidarSelecoes(usuario, livro, bibliotecaria)) return;
            if (!ValidarDisponibilidade(livro)) return;
            if (VerificarReservaAtiva(usuario.Id, livro.Id)) return;

            // Verificação final para garantir que o livro ainda está indisponível
            if (!LivroAindaIndisponivel(livro.Id))
            {
                MessageBox.Show("O status do livro foi alterado recentemente e agora está disponível.\n" +
                               "Por favor, verifique a disponibilidade antes de continuar.",
                              "Status Alterado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RegistrarReserva(usuario, livro, bibliotecaria);
        }

        private bool LivroAindaIndisponivel(int livroId)
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT Disponibilidade, Quantidade 
                          FROM Livros 
                          WHERE Id = @id";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", livroId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool disponivel = reader.GetBoolean(0);
                                int quantidade = reader.GetInt32(1);
                                return !disponivel && quantidade == 0;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Se houver erro, assume que não está mais indisponível
                return false;
            }
            return false;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNomeUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtLivro.Text) ||
                cbBibliotecaria.SelectedIndex == -1)
            {
                MessageBox.Show("Preencha todos os campos antes de reservar.",
                              "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private Usuarios ObterUsuarioSelecionado()
        {
            return _cacheUsuarios.FirstOrDefault(u =>
                u.Nome.Equals(txtNomeUsuario.Text, StringComparison.OrdinalIgnoreCase));
        }

        private Livro ObterLivroSelecionado()
        {
            return _cacheLivros.FirstOrDefault(l =>
                l.Nome.Equals(txtLivro.Text, StringComparison.OrdinalIgnoreCase));
        }

        private Usuarios ObterBibliotecariaSelecionada()
        {
            return Usuarios.FirstOrDefault(u =>
                u.Nome.Equals(cbBibliotecaria.SelectedItem.ToString(), StringComparison.OrdinalIgnoreCase));
        }

        private bool ValidarSelecoes(Usuarios usuario, Livro livro, Usuarios bibliotecaria)
        {
            if (usuario == null || livro == null || bibliotecaria == null)
            {
                MessageBox.Show("Usuário, livro ou bibliotecária não encontrado.",
                               "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidarDisponibilidade(Livro livro)
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();

                    // 1. Verifica quantidade total do livro
                    string sql = @"SELECT Quantidade FROM Livros WHERE Id = @id";
                    int quantidadeTotal;

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", livro.Id);
                        quantidadeTotal = (int)cmd.ExecuteScalar();
                    }

                    // 2. Conta empréstimos ativos (Ativo + Atrasado)
                    sql = @"SELECT COUNT(*) FROM Emprestimo 
                   WHERE Livro = @livroId AND Status IN ('Ativo', 'Atrasado')";
                    int emprestimosAtivos;

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@livroId", livro.Id);
                        emprestimosAtivos = (int)cmd.ExecuteScalar();
                    }

                    // 3. Conta reservas pendentes (Pendente + Disponível)
                    sql = @"SELECT COUNT(*) FROM Reservas 
                   WHERE LivroId = @livroId AND Status IN ('Pendente', 'Disponível')";
                    int reservasAtivas;

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@livroId", livro.Id);
                        reservasAtivas = (int)cmd.ExecuteScalar();
                    }

                    // Cálculo final
                    int disponivelParaReserva = quantidadeTotal - emprestimosAtivos - reservasAtivas;

                    if (disponivelParaReserva > 0)
                    {
                        MessageBox.Show($"Existem {disponivelParaReserva} exemplar(es) disponível(eis) para empréstimo imediato.\n\n" +
                                      "Por favor, utilize o formulário de empréstimo diretamente.",
                                      "Exemplares Disponíveis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }

                    if (reservasAtivas >= quantidadeTotal)
                    {
                        MessageBox.Show($"Todos os {quantidadeTotal} exemplares já estão:\n" +
                                      $"- Emprestados: {emprestimosAtivos}\n" +
                                      $"- Com reserva ativa: {reservasAtivas}\n\n" +
                                      "Aguarde até que algum exemplar seja devolvido.",
                                      "Reservas Esgotadas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao verificar disponibilidade: {ex.Message}",
                              "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void RegistrarReserva(Usuarios usuario, Livro livro, Usuarios bibliotecaria)
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"INSERT INTO Reservas 
                                 (UsuarioId, LivroId, BibliotecariaId, DataReserva, Status) 
                                 VALUES (@usuarioId, @livroId, @bibliotecariaId, @dataReserva, 'Pendente')";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@usuarioId", usuario.Id);
                        cmd.Parameters.AddWithValue("@livroId", livro.Id);
                        cmd.Parameters.AddWithValue("@bibliotecariaId", bibliotecaria.Id);
                        cmd.Parameters.AddWithValue("@dataReserva", DateTime.Today);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Reserva registrada com sucesso!",
                                  "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao registrar reserva:\n" + ex.Message,
                              "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Métodos de Atualização (Para chamar no Login)
        public static void AtualizarReservas(frmProgresso progressForm, int totalOperacoes, ref int progressoAtual)
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();

                    // 1. Atualizar reservas quando o livro ficar disponível
                    progressForm.AtualizarProgresso(progressoAtual++, "Verificando reservas...");
                    string updateDisponivel = @"UPDATE Reservas 
                                             SET Status = 'Disponível', 
                                                 DataDisponibilidade = GETDATE(),
                                                 DataLimiteRetirada = DATEADD(day, 3, GETDATE())
                                             WHERE Status = 'Pendente' 
                                             AND LivroId IN (SELECT Id FROM Livros WHERE Disponibilidade = 1)";
                    new SqlCeCommand(updateDisponivel, conexao).ExecuteNonQuery();

                    // 2. Expirar reservas não retiradas em 3 dias
                    progressForm.AtualizarProgresso(progressoAtual++, "Verificando prazos...");
                    string updateExpiradas = @"UPDATE Reservas 
                                             SET Status = 'Expirada'
                                             WHERE Status = 'Disponível' 
                                             AND DataLimiteRetirada < GETDATE()";
                    new SqlCeCommand(updateExpiradas, conexao).ExecuteNonQuery();

                    // 3. Liberar livros de reservas expiradas
                    progressForm.AtualizarProgresso(progressoAtual++, "Liberando livros...");
                    string liberarLivros = @"UPDATE Livros 
                                           SET Disponibilidade = 1
                                           WHERE Id IN (
                                               SELECT LivroId FROM Reservas 
                                               WHERE Status = 'Expirada'
                                           )";
                    new SqlCeCommand(liberarLivros, conexao).ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                progressForm.Invoke((MethodInvoker)(() =>
                {
                    MessageBox.Show($"Erro ao atualizar reservas: {ex.Message}",
                                  "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
        }
        #endregion

        #region Métodos Auxiliares
        private bool VerificarReservaAtiva(int usuarioId, int livroId)
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT COUNT(*) 
                          FROM Reservas 
                          WHERE UsuarioId = @usuarioId 
                          AND LivroId = @livroId 
                          AND Status IN ('Pendente', 'Disponível')";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                        cmd.Parameters.AddWithValue("@livroId", livroId);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show($"O usuario selecionado já tem uma reserva ativa para este livro.\n" +
                                          "Não é possível fazer múltiplas reservas para o mesmo livro.",
                                          "Reserva Existente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao verificar reservas: {ex.Message}",
                              "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true; // Em caso de erro, impede a reserva por segurança
            }
            return false;
        }

        private void LimparCampos()
        {
            txtNomeUsuario.Text = "";
            txtLivro.Text = "";
            txtBarcode.Text = "";
            dtpDataReserva.Value = DateTime.Today;
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }
        #endregion

        #region Métodos de Interface
        private void txtNomeUsuario_TextChanged(object sender, EventArgs e)
        {
            string texto = txtNomeUsuario.Text.Trim().ToLower();
            lstSugestoesUsuario.Visible = !string.IsNullOrWhiteSpace(texto);

            if (lstSugestoesUsuario.Visible)
            {
                lstSugestoesUsuario.Items.Clear();
                lstSugestoesUsuario.Items.AddRange(
                    Usuarios.Where(u => u.Nome.ToLower().Contains(texto))
                           .Select(u => u.Nome)
                           .ToArray());
            }
        }

        private void txtLivro_TextChanged(object sender, EventArgs e)
        {
            string texto = txtLivro.Text.Trim().ToLower();
            lstSugestoesLivros.Visible = !string.IsNullOrWhiteSpace(texto);

            if (lstSugestoesLivros.Visible)
            {
                lstSugestoesLivros.Items.Clear();
                lstSugestoesLivros.Items.AddRange(
                    Livros.Where(l => l.Nome.ToLower().Contains(texto))
                          .Select(l => l.Nome)
                          .ToArray());
            }
        }

        private void lstSugestoesUsuario_Click(object sender, EventArgs e)
        {
            if (lstSugestoesUsuario.SelectedIndex >= 0)
            {
                txtNomeUsuario.Text = lstSugestoesUsuario.SelectedItem.ToString();
                lstSugestoesUsuario.Visible = false;
            }
        }

        private void lstSugestoesLivros_Click(object sender, EventArgs e)
        {
            if (lstSugestoesLivros.SelectedIndex >= 0)
            {
                var livroSelecionado = _cacheLivros[lstSugestoesLivros.SelectedIndex];
                txtLivro.Text = livroSelecionado.Nome;
                txtBarcode.Text = livroSelecionado.CodigoDeBarras;
                lstSugestoesLivros.Visible = false;

                VerificarDisponibilidadeLivro(livroSelecionado.Id);
            }
        }

        private void VerificarDisponibilidadeLivro(int livroId)
        {
            if (livroId == _livroAtualSendoVerificado) return;
            _livroAtualSendoVerificado = livroId;

            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT Disponibilidade, Quantidade FROM Livros WHERE Id = @id";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", livroId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool disponivel = reader.GetBoolean(0);
                                int quantidade = reader.GetInt32(1);

                                if (disponivel && quantidade > 0)
                                {
                                    DialogResult resposta = MessageBox.Show(
                                        "Este livro está disponível para empréstimo imediato.\n\nDeseja abrir o formulário de empréstimo agora?",
                                        "Livro Disponível",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question);

                                    if (resposta == DialogResult.Yes)
                                    {
                                       
                                        var form = new EmprestimoForm
                                        {
                                            
                                            StartPosition = FormStartPosition.CenterScreen

                                        };

                                        form.ShowDialog();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar disponibilidade: " + ex.Message,
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _livroAtualSendoVerificado = -1;
            }
        }

        

        private void txtBarcode_Leave(object sender, EventArgs e)
        {
            string codigo = txtBarcode.Text.Trim();
            if (string.IsNullOrEmpty(codigo)) return;

            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT Id, Nome FROM Livros WHERE CodigoBarras = @codigo";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@codigo", codigo);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtLivro.Text = reader.GetString(1);
                                VerificarDisponibilidadeLivro(reader.GetInt32(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar livro: " + ex.Message,
                              "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscarLivro_Click(object sender, EventArgs e)
        {
            string filtro = txtLivro.Text.Trim();
            if (string.IsNullOrEmpty(filtro)) return;

            _cacheLivros.Clear();
            lstSugestoesLivros.Items.Clear();

            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT Id, Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade 
                         FROM Livros 
                         WHERE Nome LIKE @nome";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", "%" + filtro + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var livro = new Livro
                                {
                                    Id = reader.GetInt32(0),
                                    Nome = reader.GetString(1),
                                    Autor = reader.GetString(2),
                                    Genero = reader.GetString(3),
                                    Quantidade = reader.GetInt32(4),
                                    CodigoDeBarras = reader.GetString(5),
                                    Disponibilidade = reader.GetBoolean(6)
                                };
                                _cacheLivros.Add(livro);
                                lstSugestoesLivros.Items.Add(livro.Nome);
                            }
                        }
                    }
                }

                lstSugestoesLivros.Visible = lstSugestoesLivros.Items.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar livros: " + ex.Message,
                              "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void btnBuscarUsuario_Click(object sender, EventArgs e)
        {
            string filtro = txtNomeUsuario.Text.Trim();

            // Se o campo estiver vazio, limpa a lista de sugestões
            if (string.IsNullOrEmpty(filtro))
            {
                lstSugestoesUsuario.Items.Clear();
                lstSugestoesUsuario.Visible = false;
                return;
            }

            _cacheUsuarios.Clear();
            lstSugestoesUsuario.Items.Clear();

            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();

                    // Busca usuários pelo nome (case insensitive)
                    string sql = @"SELECT Id, Nome, TipoUsuario 
                          FROM usuarios 
                          WHERE Nome LIKE @nome
                          ORDER BY Nome";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", "%" + filtro + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var usuario = new Usuarios
                                {
                                    Id = reader.GetInt32(0),
                                    Nome = reader.GetString(1),
                                    TipoUsuario = reader.GetString(2)
                                };

                                _cacheUsuarios.Add(usuario);
                                lstSugestoesUsuario.Items.Add(usuario.Nome);
                            }
                        }
                    }
                }

                // Exibe a lista de sugestões se houver resultados
                lstSugestoesUsuario.Visible = lstSugestoesUsuario.Items.Count > 0;

                // Posiciona a lista de sugestões abaixo do campo de texto
                if (lstSugestoesUsuario.Visible)
                {
                    lstSugestoesUsuario.Top = txtNomeUsuario.Bottom;
                    lstSugestoesUsuario.Left = txtNomeUsuario.Left;
                    lstSugestoesUsuario.Width = txtNomeUsuario.Width;
                    lstSugestoesUsuario.BringToFront();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao buscar usuários:\n{ex.Message}",
                              "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtNomeUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesUsuario.Visible) return;

            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (lstSugestoesUsuario.SelectedIndex < lstSugestoesUsuario.Items.Count - 1)
                        lstSugestoesUsuario.SelectedIndex++;
                    e.Handled = true;
                    break;

                case Keys.Up:
                    if (lstSugestoesUsuario.SelectedIndex > 0)
                        lstSugestoesUsuario.SelectedIndex--;
                    e.Handled = true;
                    break;

                case Keys.Enter:
                    if (lstSugestoesUsuario.SelectedIndex >= 0)
                        SelecionarUsuarioDaLista();
                    e.Handled = true;
                    break;
            }
        }

        private void lstSugestoesUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelecionarUsuarioDaLista();
                e.Handled = true;
            }
        }

        private void SelecionarUsuarioDaLista()
        {
            if (lstSugestoesUsuario.SelectedIndex >= 0)
            {
                txtNomeUsuario.Text = lstSugestoesUsuario.SelectedItem.ToString();
                lstSugestoesUsuario.Visible = false;
                txtLivro.Focus(); // Move o foco para o próximo campo
            }
        }

        private void lstSugestoesUsuario_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lstSugestoesLivros_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}