using BibliotecaApp.Forms.Inicio;
using BibliotecaApp.Forms.Usuario;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BibliotecaApp.Forms.Livros
{
    /// <summary>
    /// Formulário principal para gerenciamento de livros da biblioteca
    /// </summary>
    public partial class LivrosForm : Form
    {
        #region Construtor e Inicialização

        /// <summary>
        /// Inicializa uma nova instância do formulário LivrosForm
        /// </summary>
        public LivrosForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Classes Auxiliares

        /// <summary>
        /// Classe estática para gerenciar conexões com o banco de dados
        /// </summary>
        public static class Conexao
        {
            /// <summary>
            /// Caminho para o arquivo do banco de dados
            /// </summary>
            public static string CaminhoBanco => Application.StartupPath + @"\bibliotecaDB\bibliotecaDB.sdf";

            /// <summary>
            /// String de conexão com o banco de dados
            /// </summary>
            public static string Conectar => $"Data Source={CaminhoBanco}; Password=123";

            /// <summary>
            /// Obtém uma nova conexão com o banco de dados
            /// </summary>
            /// <returns>Conexão SqlCeConnection configurada</returns>
            public static SqlCeConnection ObterConexao()
            {
                return new SqlCeConnection(Conectar);
            }
        }

        #endregion

        #region Métodos Utilitários

        /// <summary>
        /// Remove acentos e converte texto para minúsculas para facilitar buscas
        /// </summary>
        /// <param name="texto">Texto a ser normalizado</param>
        /// <returns>Texto normalizado sem acentos e em minúsculas</returns>
        private string NormalizarTexto(string texto)
        {
            string semAcento = new string(
                texto.Normalize(NormalizationForm.FormD)
                     .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                     .ToArray()
            );
            return semAcento.ToLower();
        }

        /// <summary>
        /// Escapa caracteres especiais para uso em consultas LIKE
        /// </summary>
        /// <param name="value">Valor a ser escapado</param>
        /// <returns>Valor com caracteres especiais escapados</returns>
        private string EscapeLikeValue(string value)
        {
            return value.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("\\", "[\\]");
        }

        #endregion

        #region Eventos do Formulário

        /// <summary>
        /// Evento executado ao carregar o formulário
        /// </summary>
        private void LivrosForm_Load(object sender, EventArgs e)
        {
            CarregarLivros();
            ConfigurarGridLivros();
        }

        #endregion

        #region Navegação entre Formulários

        /// <summary>
        /// Abre o formulário de cadastro de livros
        /// </summary>
        private void Pic_Cadastrar_Click(object sender, EventArgs e)
        {
            CadastroLivroForm popup = new CadastroLivroForm();
            Location = popup.Location;
            popup.ShowDialog();
        }

        /// <summary>
        /// Abre o formulário de devolução de livros
        /// </summary>
        private void btnDevolução_Click(object sender, EventArgs e)
        {
            DevoluçãoForm poup = new DevoluçãoForm();
            Location = poup.Location;
            poup.ShowDialog();
        }

        /// <summary>
        /// Abre o formulário de empréstimo de livros
        /// </summary>
        private void picEmprestimo_Click(object sender, EventArgs e)
        {
            EmprestimoForm popup = new EmprestimoForm();
            Location = popup.Location;
            popup.ShowDialog();
        }

        /// <summary>
        /// Abre o formulário de reservas de livros
        /// </summary>
        private void picReserva_Click(object sender, EventArgs e)
        {
            ReservaForm popup = new ReservaForm();
            Location = popup.Location;
            popup.ShowDialog();
        }

        #endregion

        #region Funcionalidades de Busca e Filtros

        /// <summary>
        /// Executa busca de livros com filtros dinâmicos
        /// </summary>
        private void btnProcurar_Click(object sender, EventArgs e)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();

                    // Define campo da pesquisa: nome, autor ou gênero
                    string campo = "nome"; // padrão
                    if (cbFiltro.SelectedItem != null)
                    {
                        string selecionado = cbFiltro.SelectedItem.ToString();
                        if (selecionado == "Autor") campo = "autor";
                        else if (selecionado == "Gênero") campo = "genero";
                    }

                    // Começa com consulta base
                    string query = "SELECT * FROM livros WHERE 1=1";

                    // Aplica filtro de texto se houver
                    if (!string.IsNullOrWhiteSpace(txtNome.Text))
                    {
                        query += $" AND {campo} LIKE @termo";
                    }

                    // Aplica filtro de disponibilidade
                    if (cbDisponibilidade.SelectedItem != null)
                    {
                        string status = cbDisponibilidade.SelectedItem.ToString();
                        if (status == "Disponíveis")
                            query += " AND disponibilidade = '1'";
                        else if (status == "Indisponíveis")
                            query += " AND disponibilidade = '0'";
                    }

                    // Ordena por nome
                    query += " ORDER BY nome ASC";

                    using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                    {
                        if (!string.IsNullOrWhiteSpace(txtNome.Text))
                        {
                            comando.Parameters.AddWithValue("@termo", "%" + txtNome.Text.Trim() + "%");
                        }

                        SqlCeDataAdapter adaptador = new SqlCeDataAdapter(comando);
                        DataTable tabela = new DataTable();
                        adaptador.Fill(tabela);

                        //Gerar Itens da coluna
                        dgvLivros.AutoGenerateColumns = true;
                        dgvLivros.DataSource = tabela;

                        lblTotal.Text = $"Total de livros encontrados: {tabela.Rows.Count}";

                        //Ocultar a coluna Disponibilidade
                        if (dgvLivros.Columns.Contains("disponibilidade"))
                        {
                            dgvLivros.Columns["disponibilidade"].Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    dgvLivros.DataSource = null;
                    MessageBox.Show("Erro ao procurar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Carrega lista de livros com filtros opcionais
        /// </summary>
        /// <param name="nomeFiltro">Filtro por nome do livro</param>
        /// <param name="generoFiltro">Filtro por gênero</param>
        /// <param name="disponibilidadeFiltro">Filtro por disponibilidade</param>
        private void CarregarLivros(string nomeFiltro = "", string generoFiltro = "Todos", string disponibilidadeFiltro = "Todos")
        {
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    string sql = @"
                SELECT
                    Id,
                    Nome,
                    Autor,
                    Genero,
                    Quantidade,
                    CodigoBarras,
                    CASE
                        WHEN Quantidade = 0 THEN 'Indisponível'
                        WHEN Disponibilidade = 1 THEN 'Disponível'
                        ELSE 'Reservado'
                    END AS Status
                FROM Livros
                WHERE 1 = 1";

                    if (!string.IsNullOrWhiteSpace(nomeFiltro))
                        sql += " AND Nome LIKE @nome ESCAPE '\\'";

                    if (!string.Equals(generoFiltro, "Todos", StringComparison.OrdinalIgnoreCase))
                        sql += " AND Genero LIKE @genero";

                    if (!string.Equals(disponibilidadeFiltro, "Todos", StringComparison.OrdinalIgnoreCase))
                    {
                        switch (disponibilidadeFiltro)
                        {
                            case "Disponível":
                                sql += " AND Disponibilidade = 1 AND Quantidade > 0";
                                break;
                            case "Indisponível":
                                sql += " AND Quantidade = 0";
                                break;
                            case "Reservado":
                                sql += " AND Disponibilidade = 0 AND Quantidade > 0";
                                break;
                        }
                    }

                    sql += " ORDER BY Nome ASC";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        if (!string.IsNullOrWhiteSpace(nomeFiltro))
                        {
                            var seguro = EscapeLikeValue(nomeFiltro.Trim());
                            cmd.Parameters.AddWithValue("@nome", seguro + "%");
                        }

                        if (!string.Equals(generoFiltro, "Todos", StringComparison.OrdinalIgnoreCase))
                            cmd.Parameters.AddWithValue("@genero", "%" + generoFiltro + "%");

                        var tabela = new DataTable();
                        using (var adapter = new SqlCeDataAdapter(cmd))
                        {
                            adapter.Fill(tabela);
                        }

                        dgvLivros.DataSource = tabela;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar livros: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Configuração e Estilização do DataGridView

        /// <summary>
        /// Configura aparência e colunas do DataGridView de livros
        /// </summary>
        private void ConfigurarGridLivros()
        {
            dgvLivros.SuspendLayout();

            dgvLivros.AutoGenerateColumns = false;
            dgvLivros.Columns.Clear();
            dgvLivros.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewTextBoxColumn AddTextCol(string dataProp, string header, float fillWeight, DataGridViewContentAlignment align, int minWidth = 60)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = dataProp,
                    Name = dataProp,
                    HeaderText = header,
                    ReadOnly = true,
                    FillWeight = fillWeight,
                    MinimumWidth = minWidth,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = align, WrapMode = DataGridViewTriState.False }
                };
                dgvLivros.Columns.Add(col);
                return col;
            }

            AddTextCol("Id", "ID", 50, DataGridViewContentAlignment.MiddleCenter, 40);
            AddTextCol("Nome", "Nome do Livro", 180, DataGridViewContentAlignment.MiddleLeft, 120);
            AddTextCol("Autor", "Autor", 160, DataGridViewContentAlignment.MiddleLeft, 100);
            AddTextCol("Genero", "Gênero", 140, DataGridViewContentAlignment.MiddleLeft, 100);
            AddTextCol("Quantidade", "Qtd", 80, DataGridViewContentAlignment.MiddleCenter, 60);
            AddTextCol("CodigoBarras", "Código de Barras", 160, DataGridViewContentAlignment.MiddleLeft, 120);

            // Botão Editar
            var btnEditar = new DataGridViewButtonColumn
            {
                Name = "Editar",
                HeaderText = "",
                Text = "Editar",
                UseColumnTextForButtonValue = true,
                Width = 80,
                FillWeight = 60,
                FlatStyle = FlatStyle.Flat
            };
            dgvLivros.Columns.Add(btnEditar);

            dgvLivros.BackgroundColor = Color.White;
            dgvLivros.BorderStyle = BorderStyle.None;
            dgvLivros.GridColor = Color.FromArgb(235, 239, 244);
            dgvLivros.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLivros.RowHeadersVisible = false;
            dgvLivros.ReadOnly = true;
            dgvLivros.MultiSelect = false;
            dgvLivros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLivros.AllowUserToAddRows = false;
            dgvLivros.AllowUserToDeleteRows = false;
            dgvLivros.AllowUserToResizeRows = false;

            dgvLivros.DefaultCellStyle.BackColor = Color.White;
            dgvLivros.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
            dgvLivros.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            dgvLivros.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgvLivros.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvLivros.RowTemplate.Height = 40;
            dgvLivros.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

            dgvLivros.EnableHeadersVisualStyles = false;
            dgvLivros.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvLivros.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
            dgvLivros.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLivros.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            dgvLivros.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvLivros.ColumnHeadersHeight = 44;
            dgvLivros.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvLivros.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn col in dgvLivros.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
                col.Resizable = DataGridViewTriState.False;
            }

            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                dgvLivros,
                new object[] { true }
            );

            dgvLivros.ResumeLayout();
        }

        /// <summary>
        /// Formata células baseado na disponibilidade do livro
        /// </summary>
        private void Lista_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvLivros.Columns[e.ColumnIndex].Name == "disponibilidade" && e.Value != null)
            {
                string valor = e.Value.ToString();

                if (valor == "0")
                {
                    // Destaca a linha toda se indisponível
                    dgvLivros.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    dgvLivros.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
                    dgvLivros.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(dgvLivros.Font, FontStyle.Italic);
                }
                else if (valor == "1")
                {
                    // Estilo para disponível
                    dgvLivros.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    dgvLivros.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// Desenha botões personalizados no DataGridView
        /// </summary>
        private void dgvLivros_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLivros.Columns[e.ColumnIndex].Name == "Editar")
            {
                e.PaintBackground(e.CellBounds, true);

                // Cores do tema
                Color corFundo = Color.FromArgb(30, 61, 88);
                Color corTexto = Color.White;

                // Desenha botão arredondado
                int borderRadius = 8;
                Rectangle rect = new Rectangle(e.CellBounds.X + 6, e.CellBounds.Y + 6,
                                               e.CellBounds.Width - 12, e.CellBounds.Height - 12);

                using (SolidBrush brush = new SolidBrush(corFundo))
                using (Pen pen = new Pen(corFundo, 1))
                {
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    path.AddArc(rect.X, rect.Y, borderRadius, borderRadius, 180, 90);
                    path.AddArc(rect.Right - borderRadius, rect.Y, borderRadius, borderRadius, 270, 90);
                    path.AddArc(rect.Right - borderRadius, rect.Bottom - borderRadius, borderRadius, borderRadius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - borderRadius, borderRadius, borderRadius, 90, 90);
                    path.CloseFigure();

                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                }

                // Texto centralizado
                TextRenderer.DrawText(e.Graphics, "Editar",
                    new Font("Segoe UI Semibold", 9F),
                    rect,
                    corTexto,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
            }
        }

        #endregion

        #region Eventos do DataGridView

        /// <summary>
        /// Manipula cliques em células do DataGridView
        /// </summary>
        private void dgvLivros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLivros.Columns[e.ColumnIndex].Name == "Editar")
            {
                var row = dgvLivros.Rows[e.RowIndex];
                var nomeLivro = row.Cells["Nome"].Value?.ToString();

                var confirm = MessageBox.Show($"Deseja editar o livro \"{nomeLivro}\"?", "Editar Livro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    var livroEdit = new AlterarCadLivroForm();

                    livroEdit.PreencherLivro(new Livro
                    {
                        Id = Convert.ToInt32(row.Cells["Id"].Value),
                        Nome = row.Cells["Nome"].Value?.ToString(),
                        Autor = row.Cells["Autor"].Value?.ToString(),
                        Genero = row.Cells["Genero"].Value?.ToString(),
                        Quantidade = Convert.ToInt32(row.Cells["Quantidade"].Value),
                        CodigoDeBarras = row.Cells["CodigoBarras"].Value?.ToString()
                    });

                    // Conecta o evento para atualizar o grid após edição
                    livroEdit.LivroAtualizado += (s, args) => CarregarLivros();

                    livroEdit.MdiParent = this.MdiParent;
                    livroEdit.Dock = DockStyle.Fill;
                    livroEdit.FormClosed += (s, args) => { livroEdit.Dispose(); };
                    livroEdit.Show();
                }
            }
        }

        #endregion

        #region Métodos de Criação de Tabelas (Desabilitados)

        /// <summary>
        /// Método para criar tabela de livros (desabilitado - tabela já existe)
        /// </summary>
        private void btnCriarTablea_Click(object sender, EventArgs e)
        {
            // Essa região está comentada porque a tabela já foi criada
            // Caso precise criar novamente, descomente e execute

            /*
            SqlCeConnection conexao = Conexao.ObterConexao();

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText =
        @"CREATE TABLE Livros (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Nome NVARCHAR(80) NOT NULL,
                    Autor NVARCHAR(80) NOT NULL,
                    Genero NVARCHAR(30) NOT NULL,
                    Quantidade INT NOT NULL DEFAULT 0,
                    CodigoBarras NVARCHAR(13) NOT NULL UNIQUE,
                    Disponibilidade BIT NOT NULL DEFAULT 1
                );";

                comando.ExecuteNonQuery();
                lblTeste.Text = "Tabela criada com sucesso!";
            }
            catch (Exception ex)
            {
                lblTeste.Text = $"Erro: {ex.Message}";
            }
            finally
            {
                conexao.Close();
            }
            */
        }

        #endregion

        #region Métodos Comentados para Referência

        /*
        /// <summary>
        /// Método para criar tabela de empréstimos (desabilitado)
        /// </summary>
        private void btnCriarEmprestimo_Click(object sender, EventArgs e)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();

                    string sql = @"
            CREATE TABLE Emprestimo (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                Alocador INT NOT NULL,
                Livro INT NOT NULL,
                Responsavel INT NOT NULL,
                DataEmprestimo DATETIME NOT NULL,
                DataDevolucao DATETIME NOT NULL,
                DataProrrogacao DATETIME NULL,
                DataRealDevolucao DATETIME NULL,
                Status NVARCHAR(15) NOT NULL
            );";

                    SqlCeCommand comando = new SqlCeCommand(sql, conexao);
                    comando.ExecuteNonQuery();

                    MessageBox.Show("Tabela 'Emprestimo' criada com sucesso!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao criar tabela: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Método para atualizar estrutura da tabela usuários (desabilitado)
        /// </summary>
        private void AtualizarEstruturaUsuarios()
        {
            using (var conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();

                    // Verifica e adiciona senha_hash se não existir
                    using (var checkCmd = new SqlCeCommand(
                        "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'usuarios' AND COLUMN_NAME = 'Senha_hash'", conexao))
                    {
                        var exists = checkCmd.ExecuteScalar();
                        if (exists == null)
                        {
                            var cmdHash = new SqlCeCommand("ALTER TABLE usuarios ADD Senha_hash NVARCHAR(64) NULL;", conexao);
                            cmdHash.ExecuteNonQuery();
                        }
                    }

                    // Verifica e adiciona senha_salt se não existir
                    using (var checkCmd = new SqlCeCommand(
                        "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'usuarios' AND COLUMN_NAME = 'Senha_salt'", conexao))
                    {
                        var exists = checkCmd.ExecuteScalar();
                        if (exists == null)
                        {
                            var cmdSalt = new SqlCeCommand("ALTER TABLE usuarios ADD Senha_salt NVARCHAR(32) NULL;", conexao);
                            cmdSalt.ExecuteNonQuery();
                        }
                    }

                    // Verifica e remove a coluna senha se existir
                    using (var checkCmd = new SqlCeCommand(
                        "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'usuarios' AND COLUMN_NAME = 'Senha'", conexao))
                    {
                        var exists = checkCmd.ExecuteScalar();
                        if (exists != null)
                        {
                            var dropCmd = new SqlCeCommand("ALTER TABLE usuarios DROP COLUMN Senha;", conexao);
                            dropCmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Estrutura da tabela 'usuarios' atualizada com sucesso!", "Sucesso",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao atualizar tabela 'usuarios': " + ex.Message, "Erro",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conexao.Close();
                }
            }
        }
        */

        #endregion

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
