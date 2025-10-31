using BibliotecaApp.Utils;
using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Livros
{

    public partial class LivrosForm : Form
    {
        #region Construtor e Inicialização

        // Inicializa uma nova instância do formulário LivrosForm
        public LivrosForm()
        {
            InitializeComponent();
            btnProcurar.PerformClick();

            // Assina o evento global para atualizar a lista automaticamente
            BibliotecaApp.Utils.EventosGlobais.LivroCadastradoOuAlterado += (s, e) => CarregarLivros();
        }

        #endregion

        #region Métodos Utilitários

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
            AddTextCol("Quantidade", "Quantidade", 160, DataGridViewContentAlignment.MiddleLeft, 100);
            AddTextCol("CodigoBarras", "Código de Barras", 160, DataGridViewContentAlignment.MiddleLeft, 120);
          /*  AddTextCol("Disponibilidade", "Disponivel", 160, DataGridViewContentAlignment.MiddleLeft, 120);*/ /*Verificar Mudança de Disponibilidade do livro*/

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
            cbDisponibilidade.SelectedIndex = 0;
            cbFiltro.SelectedIndex = 0;
        }

        #endregion

        #region Funcionalidades de Busca e Filtros

        // Executa busca de livros com filtros dinâmicos
        public void btnProcurar_Click(object sender, EventArgs e)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();

                    string campo = "nome"; // padrão
                    if (cbFiltro.SelectedItem != null)
                    {
                        string selecionado = cbFiltro.SelectedItem.ToString();
                        if (selecionado == "Autor") campo = "autor";
                        else if (selecionado == "Gênero") campo = "genero";
                        else if (selecionado == "Nome") campo = "nome";
                    }

                    string query = "SELECT * FROM livros WHERE 1=1";

                    // 🔍 Filtro por nome, autor ou gênero
                    if (!string.IsNullOrWhiteSpace(txtNome.Text))
                    {
                        query += $" AND {campo} LIKE @termo";
                    }

                    // 🔍 Filtro por código de barras (busca por prefixo)
                    if (!string.IsNullOrWhiteSpace(ObterCodigoDeBarrasFormatado()))
                    {
                        query += " AND CodigoBarras LIKE @codigo";
                    }

                    // 🔍 Filtro por disponibilidade
                    if (cbDisponibilidade.SelectedItem != null)
                    {
                        string status = cbDisponibilidade.SelectedItem.ToString();
                        if (status == "Disponíveis")
                            query += " AND disponibilidade = '1'";
                        else if (status == "Indisponíveis")
                            query += " AND disponibilidade = '0'";
                    }

                    query += " ORDER BY nome ASC";

                    using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                    {
                        if (!string.IsNullOrWhiteSpace(txtNome.Text))
                        {
                            comando.Parameters.AddWithValue("@termo", "%" + txtNome.Text.Trim() + "%");
                        }

                        if (!string.IsNullOrWhiteSpace(ObterCodigoDeBarrasFormatado()))
                        {
                            // 👇 Busca apenas códigos que comecem com os dígitos digitados
                            comando.Parameters.AddWithValue("@codigo", ObterCodigoDeBarrasFormatado() + "%");
                        }

                        SqlCeDataAdapter adaptador = new SqlCeDataAdapter(comando);
                        DataTable tabela = new DataTable();
                        adaptador.Fill(tabela);

                        dgvLivros.AutoGenerateColumns = true;
                        dgvLivros.DataSource = tabela;

                        lblTotal.Text = $"Total de livros encontrados: {tabela.Rows.Count}";

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


        private string ObterCodigoDeBarrasFormatado()
        {
            return new string(mtxCodigoBarras.Text.Where(char.IsDigit).ToArray());
        }



        // Carrega lista de livros com filtros opcionais
        // Torna o método público para permitir atualização externa
        public void CarregarLivros(string nomeFiltro = "", string generoFiltro = "Todos", string disponibilidadeFiltro = "Todos")
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



        // Formata células baseado na disponibilidade do livro
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


        // Desenha botões personalizados no DataGridView

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

        // Manipula cliques em células do DataGridView
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
                        CodigoDeBarras = row.Cells["CodigoBarras"].Value?.ToString(),

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

        private void mtxCodigoBarras_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Enter)
            {
                btnProcurar.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
