using BibliotecaApp.Models;
using BibliotecaApp.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BibliotecaApp.Forms.Livros
{
    public partial class EmprestimoForm : Form
    {
        #region Propriedades
        public List<Usuarios> Usuarios { get; set; }
        public List<Livro> Livros { get; set; }
        public List<Emprestimo> Emprestimos { get; set; }
        public bool AbertoPelaReserva { get; set; } = false;
        private bool _carregandoLivroAutomaticamente = false;
        private List<Livro> _cacheLivros = new List<Livro>();
        private List<Usuarios> _cacheUsuarios = new List<Usuarios>();


        #endregion

        #region Classe Conexao
        // Classe estática para conectar ao banco .sdf
        public static class Conexao
        {
            public static string CaminhoBanco => Application.StartupPath + @"\bibliotecaDB\bibliotecaDB.sdf";
            public static string Conectar => $"Data Source={CaminhoBanco}; Password=123";

            public static SqlCeConnection ObterConexao()
            {
                return new SqlCeConnection(Conectar);
            }
        }
        #endregion

        #region Construtores


        public EmprestimoForm()
        {
            InitializeComponent();


            EstilizarListBoxSugestao(lstSugestoesUsuario);
            EstilizarListBoxSugestao(lstLivros);

            // ... para outros ListBox de sugestão
            Usuarios = new List<Usuarios>();
            Livros = new List<Livro>();
            Emprestimos = new List<Emprestimo>();

            CarregarUsuariosDoBanco();
            CarregarLivrosDoBanco();
            CarregarBibliotecarias();

            if (!string.IsNullOrWhiteSpace(Sessao.NomeBibliotecariaLogada))
            {
                int idx = cbBibliotecaria.Items.IndexOf(Sessao.NomeBibliotecariaLogada);
                if (idx >= 0)
                    cbBibliotecaria.SelectedIndex = idx;
            }

            txtNomeUsuario.TextChanged += txtNomeUsuario_TextChanged;
            txtNomeUsuario.Leave += txtNomeUsuario_Leave;
            lstSugestoesUsuario.Leave += lstSugestoesUsuario_Leave;
            txtBarcode.Leave += txtBarcode_Leave;

            txtLivro.TextChanged += txtLivro_TextChanged;
            txtLivro.Leave += txtLivro_Leave;
            lstLivros.Leave += lstLivros_Leave;
            txtLivro.KeyDown += txtLivro_KeyDown;
            lstLivros.Click += lstLivros_Click;
            lstLivros.KeyDown += lstLivros_KeyDown;
        }

        #endregion

        #region Eventos do Formulário


        private void EmprestimoForm_Load(object sender, EventArgs e)
        {
            dtpDataEmprestimo.Value = DateTime.Today;
            dtpDataDevolucao.Value = DateTime.Today.AddDays(7);

            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;

        }

        private void label2_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void lstSugestoesUsuario_SelectedIndexChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void txtLivro_Load(object sender, EventArgs e) { }
        private void txtBarcode_Load(object sender, EventArgs e) { }
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e) { }
        private void dtpDataEmprestimo_ValueChanged(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void cbBibliotecaria_SelectedIndexChanged(object sender, EventArgs e)


        {
            cbBibliotecaria.DrawMode = DrawMode.OwnerDrawFixed;
            cbBibliotecaria.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBibliotecaria.ItemHeight = 35; // define a altura dos itens
        }
        #endregion

        #region Métodos de Empréstimo
        private void btnEmprestar_Click(object sender, EventArgs e)
        {
            // Obtendo o nome do usuário, livro e responsável (bibliotecário)
            string nomeUsuario = txtNomeUsuario.Text.Trim();
            string nomeLivro = txtLivro.Text.Trim();
            string nomeBibliotecaria = cbBibliotecaria.SelectedItem?.ToString();

            // Valida se todos os campos foram preenchidos
            if (string.IsNullOrEmpty(nomeUsuario) || string.IsNullOrEmpty(nomeLivro) || string.IsNullOrEmpty(nomeBibliotecaria))
            {
                MessageBox.Show("Preencha todos os campos antes de emprestar.");
                return;
            }

            // Buscando o usuário, livro e bibliotecário selecionados
            Usuarios usuario = null;

            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sqlUsuario = "SELECT TOP 1 * FROM usuarios WHERE Nome = @nome";

                using (var cmd = new SqlCeCommand(sqlUsuario, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", nomeUsuario);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuarios
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                TipoUsuario = reader["TipoUsuario"].ToString(),
                                Email = reader["Email"].ToString()?.Trim()
                            };
                        }
                    }
                }
            }
            var livro = Livros.FirstOrDefault(l => l.Nome.Equals(nomeLivro, StringComparison.OrdinalIgnoreCase));
            var responsavel = Usuarios.FirstOrDefault(u => u.Nome.Equals(nomeBibliotecaria, StringComparison.OrdinalIgnoreCase));

            // Verifica se algum dos dados não foi encontrado
            if (usuario == null || livro == null || responsavel == null)
            {
                MessageBox.Show("Usuário, livro ou responsável não encontrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verifica se o aluno já tem empréstimo ativo
            if (usuario.TipoUsuario == "Aluno(a)")
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    string sqlVerificaEmprestimo = @"
                SELECT COUNT(*) 
                FROM Emprestimo 
                WHERE Alocador = @usuarioId AND Status = 'Ativo'";

                    using (var cmdVerifica = new SqlCeCommand(sqlVerificaEmprestimo, conexao))
                    {
                        cmdVerifica.Parameters.AddWithValue("@usuarioId", usuario.Id);
                        int emprestimosAtivos = (int)cmdVerifica.ExecuteScalar();

                        // Se o aluno já tiver empréstimo ativo, bloqueia o novo
                        if (emprestimosAtivos > 0)
                        {
                            MessageBox.Show("Este aluno já possui um empréstimo ativo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }

            // Verifica se o livro está disponível
            if (!livro.Disponibilidade || livro.Quantidade <= 0)
            {
                DialogResult resposta = MessageBox.Show(
                    $"O livro \"{livro.Nome}\" está indisponível para empréstimo.\n\nDeseja abrir o formulário de reserva?",
                    "Livro Indisponível",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resposta == DialogResult.Yes)
                {
                    var form = new ReservaForm();
                    form.txtLivro.Text = livro.Nome;
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.ShowDialog();
                }
                return;
            }

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // Inserção do novo empréstimo
                    string sqlInserir = @"
    INSERT INTO Emprestimo 
        (Alocador, Livro, Responsavel, DataEmprestimo, DataDevolucao, DataProrrogacao, DataRealDevolucao, Status, CodigoBarras)
    VALUES 
        (@alocador, @livro, @responsavel, @dataEmprestimo, @dataDevolucao, NULL, NULL, 'Ativo', @codigoBarras)";

                    using (var cmdInsert = new SqlCeCommand(sqlInserir, conexao))
                    {
                        cmdInsert.Parameters.AddWithValue("@alocador", usuario.Id);
                        cmdInsert.Parameters.AddWithValue("@livro", livro.Id);
                        cmdInsert.Parameters.AddWithValue("@responsavel", responsavel.Id);
                        cmdInsert.Parameters.AddWithValue("@dataEmprestimo", DateTime.Now);

                        var dataDevolucaoParaInserir = usuario.TipoUsuario == "Professor" ? DateTime.Now : dtpDataDevolucao.Value;
                        cmdInsert.Parameters.AddWithValue("@dataDevolucao", dataDevolucaoParaInserir);

                        // Prioriza texto do scanner; se vazio usa o código do livro; se ainda vazio grava string vazia ""
                        string codigoBarras = !string.IsNullOrWhiteSpace(txtBarcode.Text)
                            ? txtBarcode.Text.Trim()
                            : (string.IsNullOrWhiteSpace(livro.CodigoDeBarras) ? "" : livro.CodigoDeBarras);

                        // grava sempre uma string (vazia quando não existir)
                        cmdInsert.Parameters.AddWithValue("@codigoBarras", codigoBarras);

                        cmdInsert.ExecuteNonQuery();
                    }



                    // Atualiza a quantidade e disponibilidade do livro
                    livro.Quantidade--;
                    livro.Disponibilidade = livro.Quantidade > 0;

                    string sqlAtualizaLivro = "UPDATE Livros SET Quantidade = @quantidade, Disponibilidade = @disponivel WHERE Id = @id";

                    using (var cmdLivro = new SqlCeCommand(sqlAtualizaLivro, conexao))
                    {
                        cmdLivro.Parameters.AddWithValue("@quantidade", livro.Quantidade);
                        cmdLivro.Parameters.AddWithValue("@disponivel", livro.Disponibilidade);
                        cmdLivro.Parameters.AddWithValue("@id", livro.Id);
                        cmdLivro.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Empréstimo registrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();

                // Envio de e-mail apenas se o e-mail for válido
                string email = usuario.Email?.Trim();
                if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
                {
                    string assunto = "✅ Empréstimo Confirmado - Biblioteca Monteiro Lobato";
                    string corpo = $@"
<html>
<body style='font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
        <h2 style='color: #2c3e50;'>Olá, {usuario.Nome} 👋</h2>
        <p>Seu empréstimo foi registrado com sucesso! Aqui estão os detalhes:</p>
        <p><strong>📖 Livro:</strong> {livro.Nome}</p>
        <p><strong>📅 Data do Empréstimo:</strong> {DateTime.Now:dd/MM/yyyy}</p>
        <p><strong>📆 Data de Devolução:</strong> {dtpDataDevolucao.Value:dd/MM/yyyy}</p>
        <p style='margin-top: 20px;'>Por favor, devolva o livro no prazo para evitar bloqueios no sistema e restrições na secretaria.</p>
        <hr />
        <p style='font-size: 14px; color: #888;'>Este é um e-mail automático enviado pela Biblioteca Monteiro Lobato.</p>
    </div>
</body>
</html>";
                    EmailService.Enviar(email, assunto, corpo);
                }
                else
                {
                    MessageBox.Show("Empréstimo registrado, porém o usuário não possui e-mail cadastrado ou válido. Nenhum e-mail foi enviado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao registrar empréstimo:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Métodos de Usuário
        private void txtNomeUsuario_TextChanged(object sender, EventArgs e)
        {
            string nomeBusca = txtNomeUsuario.Text.Trim();

            lstSugestoesUsuario.Items.Clear();
            
            lstSugestoesUsuario.Visible = false;
            _cacheUsuarios.Clear();

            if (string.IsNullOrWhiteSpace(nomeBusca))
                return;

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT * FROM usuarios WHERE Nome LIKE @nome ORDER BY Nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", nomeBusca + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var usuario = new Usuarios
                                {
                                    Id = (int)reader["Id"],
                                    Nome = reader["Nome"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    CPF = reader["CPF"].ToString(),
                                    DataNascimento = reader["DataNascimento"] != DBNull.Value ? Convert.ToDateTime(reader["DataNascimento"]) : DateTime.MinValue,
                                    Telefone = reader["Telefone"].ToString(),
                                    Turma = reader["Turma"].ToString(),
                                    TipoUsuario = reader["TipoUsuario"].ToString()
                                };
                                _cacheUsuarios.Add(usuario);
                                lstSugestoesUsuario.Items.Add($"{usuario.Nome} - {usuario.Turma}");
                            }
                        }
                    }
                }
                lstSugestoesUsuario.Visible = lstSugestoesUsuario.Items.Count > 0;
                lstSugestoesUsuario.Enabled = lstSugestoesUsuario.Items.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na busca: " + ex.Message);
            }
        }

        private void lstSugestoesUsuario_Click(object sender, EventArgs e)
        {
            if (lstSugestoesUsuario.SelectedIndex >= 0)
                SelecionarUsuario(lstSugestoesUsuario.SelectedIndex);
        }

       

        private void txtNomeUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesUsuario.Visible) return;

            if (e.KeyCode == Keys.Down && lstSugestoesUsuario.SelectedIndex < lstSugestoesUsuario.Items.Count - 1)
            {
                e.Handled = true;
                lstSugestoesUsuario.SelectedIndex++;
            }
            else if (e.KeyCode == Keys.Up && lstSugestoesUsuario.SelectedIndex > 0)
            {
                e.Handled = true;
                lstSugestoesUsuario.SelectedIndex--;
            }
            else if (e.KeyCode == Keys.Enter && lstSugestoesUsuario.SelectedIndex >= 0)
            {
                e.Handled = true;
                SelecionarUsuario(lstSugestoesUsuario.SelectedIndex);
            }
        }

        private void lstSugestoesUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstSugestoesUsuario.SelectedIndex >= 0)
            {
                e.Handled = true;
                SelecionarUsuario(lstSugestoesUsuario.SelectedIndex);
            }
        }

        private void SelecionarUsuario(int index)
        {
            var usuario = _cacheUsuarios[index];
            txtNomeUsuario.Text = usuario.Nome;
            lstSugestoesUsuario.Visible = false;

            if (usuario.TipoUsuario == "Professor(a)")
            {
                chkDevolucaoPersonalizada.Checked = true;
                chkDevolucaoPersonalizada.Enabled = false;
                dtpDataDevolucao.Enabled = true;
            }
            else
            {
                chkDevolucaoPersonalizada.Checked = false;
                chkDevolucaoPersonalizada.Enabled = true;
                dtpDataDevolucao.Enabled = false;
                dtpDataDevolucao.Value = DateTime.Today.AddDays(7); // 7 dias por padrão
            }
        }

        private void txtNomeUsuario_Leave(object sender, EventArgs e)
        {
            // Se o foco sair do textbox e da listbox
            if (!lstSugestoesUsuario.Focused)
                lstSugestoesUsuario.Visible = false;
        }

        private void lstSugestoesUsuario_Leave(object sender, EventArgs e)
        {
            lstSugestoesUsuario.Visible = false;
        }

        private void CarregarUsuariosDoBanco()
        {
            Usuarios.Clear();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT Id, Nome, TipoUsuario FROM Usuarios";

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
                MessageBox.Show("Erro ao carregar usuários: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Métodos de Livros
        private void txtLivro_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstLivros.Visible) return;

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

        private void btnBuscarLivro_Click(object sender, EventArgs e)
        {
            string filtro = txtLivro.Text.Trim();
            if (string.IsNullOrEmpty(filtro))
                return;

            _cacheLivros.Clear();
            lstLivros.Items.Clear();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT Id, Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade 
                           FROM Livros 
                           WHERE Nome LIKE @nome
                           ORDER BY Nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        // Busca só nomes que começam com o filtro
                        cmd.Parameters.AddWithValue("@nome", filtro + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Livro livro = new Livro
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

                                // Adiciona nome e gênero na lista
                                lstLivros.Items.Add($"{livro.Nome} - {livro.Autor}");
                            }
                        }
                    }
                }

                lstLivros.Visible = lstLivros.Items.Count > 0;
                if (lstLivros.Visible)
                    lstLivros.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar livros:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtBarcode.Enabled = true;
            txtBarcode.Text = livro.CodigoDeBarras;
            txtBarcode.Enabled = false;

            lstLivros.Visible = false;
        }


        private void txtLivro_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtLivro.Text.Trim();

            lstLivros.Items.Clear();
            lstLivros.Visible = false;
            _cacheLivros.Clear();

            if (string.IsNullOrWhiteSpace(filtro))
                return;

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT Id, Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade 
                           FROM Livros 
                           WHERE Nome LIKE @nome
                           ORDER BY Nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", filtro + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Livro livro = new Livro
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
                                lstLivros.Items.Add($"{livro.Nome} - {livro.Autor}");
                            }
                        }
                    }
                }
                lstLivros.Visible = lstLivros.Items.Count > 0;
                if (lstLivros.Visible)
                    lstLivros.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar livros:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtLivro_Leave(object sender, EventArgs e)
        {
            if (!lstLivros.Focused)
                lstLivros.Visible = false;
        }

        private void lstLivros_Leave(object sender, EventArgs e)
        {
            lstLivros.Visible = false;
        }

        private void CarregarLivrosDoBanco()
        {
            Livros.Clear();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT Id, Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade FROM Livros";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Livro livro = new Livro(
                                reader.GetString(1), // Nome
                                reader.GetString(2), // Autor
                                reader.GetString(3), // Genero
                                reader.GetBoolean(6), // Disponibilidade
                                reader.GetInt32(4),   // Quantidade
                                reader.GetString(5)   // CodigoBarras
                            );

                            // Setando o ID (tornar public set temporariamente ou criar outro construtor com ID)
                            typeof(Livro).GetProperty("Id").SetValue(livro, reader.GetInt32(0));
                            Livros.Add(livro);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar livros: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Métodos de Código de Barras
        private void txtBarcode_Leave(object sender, EventArgs e)
        {// Não verifica se foi aberto pela reserva
            if (AbertoPelaReserva) return;

            // Só busca se o campo estiver preenchido
            if (!string.IsNullOrEmpty(txtBarcode.Text))
            {
                BuscarEPreencherLivroPorCodigo();
            }
        }

        private void BuscarEPreencherLivroPorCodigo()
        {
            string codigo = txtBarcode.Text.Trim();
            if (string.IsNullOrEmpty(codigo)) return;

            try
            {
                using (var conexao = Conexao.ObterConexao())
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
                                txtLivro.Text = reader.GetString(0);
                            }
                            else
                            {
                                MessageBox.Show("Livro não encontrado. Escaneei novamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtBarcode.Focus();
                                txtBarcode.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar o livro: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Métodos de Bibliotecárias
        private void CarregarBibliotecarias()
        {
            cbBibliotecaria.Items.Clear();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    string sql = "SELECT Nome FROM Usuarios WHERE TipoUsuario = 'Bibliotecário(a)'";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nome = reader.GetString(0);
                            cbBibliotecaria.Items.Add(nome);
                        }
                    }
                }

                if (cbBibliotecaria.Items.Count == 0)
                {
                    MessageBox.Show("Nenhuma bibliotecária encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar bibliotecárias:\n" + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Métodos de Navegação e Data
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }

        private void chkDevolucaoPersonalizada_CheckedChanged(object sender, EventArgs e)
        {
            dtpDataDevolucao.Enabled = chkDevolucaoPersonalizada.Checked;

            if (!chkDevolucaoPersonalizada.Checked)
                dtpDataDevolucao.Value = DateTime.Today.AddDays(7);
        }

        public void LimparCampos()
        {
            txtNomeUsuario.Text = "";
            txtLivro.Text = "";
            txtBarcode.Text = "";
            cbBibliotecaria.Text = "";
            dtpDataDevolucao.Value = DateTime.Today.AddDays(7);
            chkDevolucaoPersonalizada.Checked = false;

        }
        #endregion

        #region estilizacao listbox
        private int hoveredIndex = -1;

        private void EstilizarListBoxSugestao(ListBox listBox)
        {
            listBox.DrawMode = DrawMode.OwnerDrawFixed;
            listBox.Font = new Font("Segoe UI", 12, FontStyle.Regular); 
            listBox.ItemHeight = 40; 

            listBox.BackColor = Color.White;
            listBox.ForeColor = Color.FromArgb(30, 61, 88);
            listBox.BorderStyle = BorderStyle.FixedSingle;
            listBox.IntegralHeight = false;

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

            // Tons de cinza
            Color backColor = hovered
                ? Color.FromArgb(235, 235, 235) // cinza claro no hover
                : Color.White;                  // fundo branco

            Color textColor = Color.FromArgb(60, 60, 60); // cinza escuro

            using (SolidBrush b = new SolidBrush(backColor))
                e.Graphics.FillRectangle(b, e.Bounds);

            string text = listBox.Items[e.Index].ToString();
            Font font = listBox.Font;

            Rectangle textRect = new Rectangle(e.Bounds.Left + 12, e.Bounds.Top, e.Bounds.Width - 24, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, text, font, textRect, textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            // Linha divisória entre itens (cinza bem suave)
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

        
    }
}