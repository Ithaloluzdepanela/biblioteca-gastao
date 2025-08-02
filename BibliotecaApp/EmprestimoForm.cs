using BibliotecaApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
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

        public EmprestimoForm(List<Usuarios> usuarios, List<Livro> livros)
        {
            InitializeComponent();

            Usuarios = usuarios;
            Livros = livros;
            Emprestimos = new List<Emprestimo>();

            // Configurações de AutoComplete para usuário e livro
         

        

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


            CarregarUsuariosDoBanco();
            CarregarLivrosDoBanco();

            CarregarBibliotecarias();

            //Autocompleta a bibliotecária logada
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

        #region Evento de carregamento do formulário

        private void EmprestimoForm_Load(object sender, EventArgs e)
        {
            dtpDataEmprestimo.Value = DateTime.Today;
            dtpDataDevolucao.Value = DateTime.Today.AddDays(7);

           

            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;

        }

        

        #endregion

        #region Emprestar Livro

       
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
            var usuario = Usuarios.FirstOrDefault(u => u.Nome.Equals(nomeUsuario, StringComparison.OrdinalIgnoreCase));
            var livro = Livros.FirstOrDefault(l => l.Nome.Equals(nomeLivro, StringComparison.OrdinalIgnoreCase));
            var responsavel = Usuarios.FirstOrDefault(u => u.Nome.Equals(nomeBibliotecaria, StringComparison.OrdinalIgnoreCase));

            // Verifica se algum dos dados não foi encontrado
            if (usuario == null || livro == null || responsavel == null)
            {
                MessageBox.Show("Usuário, livro ou responsável não encontrado.");
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
                WHERE Alocador = @usuarioId AND Status = 'Emprestado'";

                    using (var cmdVerifica = new SqlCeCommand(sqlVerificaEmprestimo, conexao))
                    {
                        cmdVerifica.Parameters.AddWithValue("@usuarioId", usuario.Id);
                        int emprestimosAtivos = (int)cmdVerifica.ExecuteScalar();

                        // Se o aluno já tiver empréstimo ativo, bloqueia o novo
                        if (emprestimosAtivos > 0)
                        {
                            MessageBox.Show("Este aluno já possui um empréstimo ativo.");
                            return;
                        }
                    }
                }
            }

            // Verifica se o livro está disponível
            if (!livro.Disponibilidade || livro.Quantidade <= 0)
            {
                MessageBox.Show("Livro indisponível para empréstimo.");
                return;
            }

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // Inserção do novo empréstimo
                    string sqlInserir = @"
                INSERT INTO Emprestimo (Alocador, Livro, Responsavel, DataEmprestimo, DataDevolucao, DataProrrogacao, DataRealDevolucao, Status)
                VALUES (@alocador, @livro, @responsavel, @dataEmprestimo, @dataDevolucao, NULL, NULL, 'Emprestado')";

                    using (var cmdInsert = new SqlCeCommand(sqlInserir, conexao))
                    {
                        cmdInsert.Parameters.AddWithValue("@alocador", usuario.Id);
                        cmdInsert.Parameters.AddWithValue("@livro", livro.Id);
                        cmdInsert.Parameters.AddWithValue("@responsavel", responsavel.Id);
                        cmdInsert.Parameters.AddWithValue("@dataEmprestimo", DateTime.Now);
                        cmdInsert.Parameters.AddWithValue("@dataDevolucao", usuario.TipoUsuario == "Professor" ? DateTime.Now : dtpDataDevolucao.Value);

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

                    MessageBox.Show("Empréstimo registrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao registrar empréstimo:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Usuários

        private void txtNomeUsuario_TextChanged(object sender, EventArgs e)
        {
            string texto = txtNomeUsuario.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(texto))
            {
                lstSugestoesUsuario.Visible = false;
                return;
            }

            var sugestoes = Usuarios.Where(u => u.Nome.ToLower().Contains(texto)).ToList();

            lstSugestoesUsuario.Items.Clear();
            foreach (var usuario in sugestoes)
                lstSugestoesUsuario.Items.Add(usuario.Nome);

            lstSugestoesUsuario.Visible = sugestoes.Any();
        }

        private void lstSugestoesUsuario_Click(object sender, EventArgs e)
        {
            if (lstSugestoesUsuario.SelectedIndex >= 0)
                SelecionarUsuario(lstSugestoesUsuario.SelectedIndex);
        }
        private void btnBuscarUsuario_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtNomeUsuario.Text))
            {
                lstSugestoesUsuario.Visible = false;
            }

            string filtro = txtNomeUsuario.Text.Trim();
            if (string.IsNullOrEmpty(filtro))
                return;

            _cacheUsuarios.Clear();
            lstSugestoesUsuario.Items.Clear();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT Id, Nome, TipoUsuario FROM Usuarios WHERE Nome LIKE @nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", "%" + filtro + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var u = new Usuarios
                                {
                                    Id = reader.GetInt32(0),
                                    Nome = reader.GetString(1),
                                    TipoUsuario = reader.GetString(2)
                                };
                                _cacheUsuarios.Add(u);
                                lstSugestoesUsuario.Items.Add(u.Nome);
                            }
                        }
                    }
                }

                lstSugestoesUsuario.Visible = _cacheUsuarios.Any();
                if (lstSugestoesUsuario.Visible)
                    lstSugestoesUsuario.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na busca de usuários:\n" + ex.Message, "Erro",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                chkDevolucaoPersonalizada.Enabled =true;
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


        #endregion

        #region Livros

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
                    string sql = "SELECT Id, Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade FROM Livros WHERE Nome LIKE @nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", "%" + filtro + "%");

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
                                lstLivros.Items.Add(livro.Nome);
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
            string texto = txtLivro.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(texto))
            {
                lstLivros.Visible = false;
                return;
            }

            var sugestoes = _cacheLivros.Where(l => l.Nome.ToLower().Contains(texto)).ToList();

            lstLivros.Items.Clear();
            foreach (var livro in sugestoes)
                lstLivros.Items.Add(livro.Nome);

            lstLivros.Visible = sugestoes.Any();
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
        
        

        private void txtBarcode_Leave(object sender, EventArgs e)
        {
            BuscarEPreencherLivroPorCodigo();
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
                                MessageBox.Show("Livro não encontrado. Escaneei novamente");
                                txtBarcode.Focus();
                                txtBarcode.Text = "";
                            }
                        }
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        


        private void lstSugestoesUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        

        //carrega as bibliotecárias do banco de dados para o ComboBox
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
                    MessageBox.Show("Nenhuma bibliotecária encontrada.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar bibliotecárias:\n" + ex.Message);
            }
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
                MessageBox.Show("Erro ao carregar usuários: " + ex.Message);
            }
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
                MessageBox.Show("Erro ao carregar livros: " + ex.Message);
            }
        }


        private void txtLivro_Load(object sender, EventArgs e)
        {

        }

        private void txtBarcode_Load(object sender, EventArgs e)
        {

        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dtpDataEmprestimo_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
    }

