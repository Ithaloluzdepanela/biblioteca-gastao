using BibliotecaApp.Utils;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static Usuarios;

namespace BibliotecaApp.Forms.Livros
{
    public partial class DevoluçãoForm : Form
    {
        #region Construtor

        public DevoluçãoForm()
        {
            InitializeComponent();
            
        }

        #endregion

        #region Eventos do Formulário

        private void DevoluçãoForm_Load(object sender, EventArgs e)
        {
            InicializarFormulario();
            VerificarAtrasos(); // Atualiza status de atrasos antes de buscar
            BuscarEmprestimos(); // Exibe todos os empréstimos atualizados no grid
        }

        private void btnBuscarEmprestimo_Click(object sender, EventArgs e)
        {
            BuscarEmprestimos();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            ConfirmarELimparCampos();
        }

        private void btnConfirmarDevolucao_Click(object sender, EventArgs e)
        {
            DevolverEmprestimo();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            BuscarEmprestimos();
        }

        private void dgvEmprestimos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvEmprestimos.Columns[e.ColumnIndex].Name != "Status" || e.Value == null)
                return;

            string status = e.Value.ToString().Trim();
            Color cor = ObterCorStatus(status);
            FontStyle estilo = ObterEstiloStatus(status);

            e.CellStyle.ForeColor = cor;
            e.CellStyle.Font = new Font(e.CellStyle.Font, estilo);
        }

        private void dgvEmprestimos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ProcessarCliqueBotaoFicha(e);
        }

        private void dgvEmprestimos_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DesenharBotaoFicha(e);
        }

        #endregion

        #region Inicialização

        private void InicializarFormulario()
        {
            BuscarEmprestimos();
            ConfigurarGridEmprestimos();
            cbFiltroEmprestimo.SelectedIndex = 0;
            VerificarAtrasos();
        }

        #endregion

        #region Métodos de Interface

        private void ConfirmarELimparCampos()
        {
            DialogResult resultado = MessageBox.Show(
                "Tem certeza de que deseja limpar tudo?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                LimparCampos();
            }
        }

        private void LimparCampos()
        {
            txtNome.Text = "";
            mtxCodigoBarras.Text = "";
            txtNome.Focus();
        }

        private Color ObterCorStatus(string status)
        {
            switch (status)
            {
                case "Ativo":
                    return Color.Green;
                case "Atrasado":
                    return Color.Red;
                case "Devolvido":
                    return Color.DimGray;
                default:
                    return Color.Black;
            }
        }

        private FontStyle ObterEstiloStatus(string status)
        {
            switch (status)
            {
                case "Ativo":
                case "Atrasado":
                    return FontStyle.Bold;
                case "Devolvido":
                default:
                    return FontStyle.Regular;
            }
        }

        #endregion

        #region Busca e Consulta de Dados

        private void BuscarEmprestimos()
        {
            var filtros = ObterFiltrosBusca();

            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                conexao.Open();
                string query = ConstruirQueryBusca(filtros);

                using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                {
                    AdicionarParametrosBusca(comando, filtros);

                    SqlCeDataAdapter adaptador = new SqlCeDataAdapter(comando);
                    DataTable tabela = new DataTable();
                    adaptador.Fill(tabela);

                    dgvEmprestimos.DataSource = tabela;
                }
            }
        }

        private FiltrosBusca ObterFiltrosBusca()
        {
            return new FiltrosBusca
            {
                NomeLivro = txtNome.Text.Trim(),
                CodigoBarras = mtxCodigoBarras.Text.Trim(),
                StatusFiltro = cbFiltroEmprestimo.SelectedItem?.ToString()
            };
        }

        private string ConstruirQueryBusca(FiltrosBusca filtros)
        {
            string queryBase = @"
                SELECT 
                    e.Id AS [ID do Empréstimo],
                    uAlocador.Nome AS [Alocador],
                    uResponsavel.Nome AS [Responsável],
                    e.Alocador AS [IdResponsavel],
                    l.Nome AS [Livro],
                    l.CodigoBarras AS [Código De Barras],
                    e.DataEmprestimo AS [Data do Empréstimo],
                    e.DataDevolucao AS [Data de Devolução],
                    e.Status AS [Status]
                FROM Emprestimo e
                JOIN Usuarios uAlocador ON e.Alocador = uAlocador.Id
                JOIN Usuarios uResponsavel ON e.Responsavel = uResponsavel.Id
                JOIN Livros l ON e.Livro = l.Id
                WHERE l.Nome LIKE @NomeLivro";

            if (filtros.FiltrarCodigoBarras)
                queryBase += " AND l.CodigoBarras LIKE @CodigoBarras";

            if (filtros.FiltrarStatus)
                queryBase += " AND e.Status = @Status";

            queryBase += @"
                ORDER BY 
                    CASE e.Status
                        WHEN 'Atrasado' THEN 1
                        WHEN 'Ativo' THEN 2
                        WHEN 'Devolvido' THEN 3
                        ELSE 4
                    END";

            return queryBase;
        }

        private void AdicionarParametrosBusca(SqlCeCommand comando, FiltrosBusca filtros)
        {
            comando.Parameters.AddWithValue("@NomeLivro", "%" + filtros.NomeLivro + "%");

            if (filtros.FiltrarCodigoBarras)
                comando.Parameters.AddWithValue("@CodigoBarras", "%" + filtros.CodigoBarras + "%");

            if (filtros.FiltrarStatus)
                comando.Parameters.AddWithValue("@Status", filtros.StatusFiltro);
        }

        #endregion

        #region Configuração do DataGridView

        private void ConfigurarGridEmprestimos()
        {
            dgvEmprestimos.SuspendLayout();

            ConfigurarColunasGrid();
            ConfigurarEstiloGrid();
            ConfigurarEventosGrid();
            AdicionarBotaoFicha();

            dgvEmprestimos.ResumeLayout();
        }

        private void ConfigurarColunasGrid()
        {
            dgvEmprestimos.AutoGenerateColumns = false;
            dgvEmprestimos.Columns.Clear();
            dgvEmprestimos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            var colunas = ObterDefinicoesColunas();
            foreach (var coluna in colunas)
            {
                AdicionarColuna(coluna);
            }

            dgvEmprestimos.Columns["IdResponsavel"].Visible = false;
        }

        private DefinicaoColuna[] ObterDefinicoesColunas()
        {
            return new[]
            {
                new DefinicaoColuna("ID do Empréstimo", "ID", 50, DataGridViewContentAlignment.MiddleCenter, 40),
                new DefinicaoColuna("Alocador", "Alocador", 160, DataGridViewContentAlignment.MiddleLeft, 100),
                new DefinicaoColuna("Responsável", "Responsável", 160, DataGridViewContentAlignment.MiddleLeft, 100),
                new DefinicaoColuna("Livro", "Nome do Livro", 180, DataGridViewContentAlignment.MiddleLeft, 120),
                new DefinicaoColuna("Código De Barras", "Código de Barras", 160, DataGridViewContentAlignment.MiddleLeft, 120),
                new DefinicaoColuna("Data do Empréstimo", "Data de Empréstimo", 150, DataGridViewContentAlignment.MiddleCenter, 110),
                new DefinicaoColuna("Data de Devolução", "Data de Devolução", 140, DataGridViewContentAlignment.MiddleCenter, 100),
                new DefinicaoColuna("Status", "Status", 100, DataGridViewContentAlignment.MiddleCenter, 80),
                new DefinicaoColuna("IdResponsavel", "IdResponsavel", 50, DataGridViewContentAlignment.MiddleCenter, 40)
            };
        }

        private void AdicionarColuna(DefinicaoColuna def)
        {
            var col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = def.DataProperty,
                Name = def.DataProperty,
                HeaderText = def.Header,
                ReadOnly = true,
                FillWeight = def.FillWeight,
                MinimumWidth = def.MinWidth,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = def.Alignment,
                    WrapMode = DataGridViewTriState.False,
                    SelectionBackColor = Color.FromArgb(16, 87, 174),
                    SelectionForeColor = Color.White
                }
            };
            dgvEmprestimos.Columns.Add(col);
        }

        private void ConfigurarEstiloGrid()
        {
            ConfigurarEstilosBasicos();
            ConfigurarEstiloCelulas();
            ConfigurarEstiloCabecalho();
            ConfigurarColunas();
            HabilitarDoubleBuffering();
        }

        private void ConfigurarEstilosBasicos()
        {
            dgvEmprestimos.BackgroundColor = Color.White;
            dgvEmprestimos.BorderStyle = BorderStyle.None;
            dgvEmprestimos.GridColor = Color.FromArgb(235, 239, 244);
            dgvEmprestimos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvEmprestimos.RowHeadersVisible = false;
            dgvEmprestimos.ReadOnly = true;
            dgvEmprestimos.MultiSelect = false;
            dgvEmprestimos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmprestimos.AllowUserToAddRows = false;
            dgvEmprestimos.AllowUserToDeleteRows = false;
            dgvEmprestimos.AllowUserToResizeRows = false;
            dgvEmprestimos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ConfigurarEstiloCelulas()
        {
            dgvEmprestimos.DefaultCellStyle.BackColor = Color.White;
            dgvEmprestimos.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
            dgvEmprestimos.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            dgvEmprestimos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(16, 87, 174);
            dgvEmprestimos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvEmprestimos.RowTemplate.Height = 40;
            dgvEmprestimos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        }

        private void ConfigurarEstiloCabecalho()
        {
            dgvEmprestimos.EnableHeadersVisualStyles = false;
            dgvEmprestimos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvEmprestimos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
            dgvEmprestimos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEmprestimos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            dgvEmprestimos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvEmprestimos.ColumnHeadersHeight = 44;
            dgvEmprestimos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        }

        private void ConfigurarColunas()
        {
            foreach (DataGridViewColumn col in dgvEmprestimos.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
                col.Resizable = DataGridViewTriState.False;
            }
        }

        private void HabilitarDoubleBuffering()
        {
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                dgvEmprestimos,
                new object[] { true }
            );
        }

        private void ConfigurarEventosGrid()
        {
            dgvEmprestimos.DataBindingComplete += (s, e) =>
            {
                foreach (DataGridViewRow row in dgvEmprestimos.Rows)
                {
                    AplicarCorStatus(row);
                }
            };
        }

        private void AplicarCorStatus(DataGridViewRow row)
        {
            var status = row.Cells["Status"].Value?.ToString()?.Trim();
            Color foreColor = ObterCorForegroundStatus(status);
            Color selectionColor = ObterCorSelectionStatus(status);

            row.Cells["Status"].Style.ForeColor = foreColor;
            row.Cells["Status"].Style.SelectionForeColor = selectionColor;
        }

        private Color ObterCorForegroundStatus(string status)
        {
            switch (status)
            {
                case "Atrasado":
                    return Color.Red;
                case "Ativo":
                    return Color.Green;
                case "Finalizado":
                    return Color.DimGray;
                default:
                    return Color.Black;
            }
        }

        private Color ObterCorSelectionStatus(string status)
        {
            switch (status)
            {
                case "Atrasado":
                    return Color.Red;
                case "Ativo":
                    return Color.Green;
                case "Finalizado":
                    return Color.DimGray;
                default:
                    return Color.Black;
            }
        }

        #endregion

        #region Botão Ficha do Aluno

        private void AdicionarBotaoFicha()
        {
            var btnFicha = new DataGridViewButtonColumn
            {
                Name = "Ficha",
                HeaderText = "Ficha",
                Text = "Abrir",
                UseColumnTextForButtonValue = true,
                Width = 80,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(16, 87, 174),
                    SelectionBackColor = Color.FromArgb(16, 87, 174),
                    SelectionForeColor = Color.White
                }
            };

            dgvEmprestimos.Columns.Add(btnFicha);
        }

        private void ProcessarCliqueBotaoFicha(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvEmprestimos.Columns[e.ColumnIndex].Name != "Ficha")
                return;

            var idObj = dgvEmprestimos.Rows[e.RowIndex].Cells["IdResponsavel"].Value;
            if (idObj != null && int.TryParse(idObj.ToString(), out int idUsuario))
            {
                AbrirFichaAluno(idUsuario);
            }
        }

        private void AbrirFichaAluno(int idUsuario)
        {
            var aluno = BuscarAlunoPorId(idUsuario);

            if (aluno != null)
            {
                var fichaForm = new FichaAlunoForm();
                fichaForm.PreencherAluno(aluno);
                fichaForm.MdiParent = this.MdiParent;
                fichaForm.Dock = DockStyle.Fill;
                fichaForm.Show();
            }
            else
            {
                MessageBox.Show("Aluno não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DesenharBotaoFicha(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvEmprestimos.Columns[e.ColumnIndex].Name != "Ficha")
                return;

            e.PaintBackground(e.CellBounds, true);

            Color corFundo = Color.FromArgb(30, 61, 88);
            Color corTexto = Color.White;
            int borderRadius = 8;

            Rectangle rect = new Rectangle(
                e.CellBounds.X + 6,
                e.CellBounds.Y + 6,
                e.CellBounds.Width - 12,
                e.CellBounds.Height - 12);

            DesenharBotaoArredondado(e.Graphics, rect, corFundo, corTexto, borderRadius);
            e.Handled = true;
        }

        private void DesenharBotaoArredondado(Graphics graphics, Rectangle rect, Color corFundo, Color corTexto, int borderRadius)
        {
            using (SolidBrush brush = new SolidBrush(corFundo))
            using (Pen pen = new Pen(corFundo, 1))
            {
                var path = CriarCaminhoArredondado(rect, borderRadius);
                graphics.FillPath(brush, path);
                graphics.DrawPath(pen, path);
            }

            TextRenderer.DrawText(graphics, "Ficha",
                new Font("Segoe UI Semibold", 9F),
                rect,
                corTexto,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private System.Drawing.Drawing2D.GraphicsPath CriarCaminhoArredondado(Rectangle rect, int borderRadius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, borderRadius, borderRadius, 180, 90);
            path.AddArc(rect.Right - borderRadius, rect.Y, borderRadius, borderRadius, 270, 90);
            path.AddArc(rect.Right - borderRadius, rect.Bottom - borderRadius, borderRadius, borderRadius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - borderRadius, borderRadius, borderRadius, 90, 90);
            path.CloseFigure();
            return path;
        }

        #endregion

        #region Operações de Devolução

        private void DevolverEmprestimo()
        {
            if (!ValidarSelecaoEmprestimo())
                return;

            var emprestimoInfo = ObterInformacoesEmprestimoSelecionado();

            if (emprestimoInfo.Status == "Devolvido")
            {
                MessageBox.Show("Este empréstimo já foi devolvido.");
                return;
            }

            // Obter dados adicionais da linha selecionada
            var row = dgvEmprestimos.SelectedRows[0];
            string livro = row.Cells["Livro"].Value?.ToString();
            string alocador = row.Cells["Alocador"].Value?.ToString();
           
            string dataEmprestimo = row.Cells["Data do Empréstimo"].Value != null
                ? Convert.ToDateTime(row.Cells["Data do Empréstimo"].Value).ToString("dd/MM/yyyy")
                : "";
 

            string msg = $"Confirma a devolução deste empréstimo?\n\n" +
                         $"Livro: {livro}\n" +
                         $"Alocador: {alocador}\n" +
                         $"Data do Empréstimo: {dataEmprestimo}\n";
                         

            var confirm = MessageBox.Show(msg, "Confirmação de Devolução", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            ProcessarDevolucaoNoBanco(emprestimoInfo.Id);
            MessageBox.Show("Livro devolvido com sucesso.");
            BuscarEmprestimos();
        }

        private bool ValidarSelecaoEmprestimo()
        {
            if (dgvEmprestimos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um empréstimo para devolver.");
                return false;
            }
            return true;
        }

        private EmprestimoInfo ObterInformacoesEmprestimoSelecionado()
        {
            var row = dgvEmprestimos.SelectedRows[0];
            return new EmprestimoInfo
            {
                Id = Convert.ToInt32(row.Cells["ID do Empréstimo"].Value),
                Status = row.Cells["Status"].Value?.ToString()
            };
        }

        private void ProcessarDevolucaoNoBanco(int idEmprestimo)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                conexao.Open();
                using (var transacao = conexao.BeginTransaction())
                {
                    try
                    {
                        int idLivro = ObterIdLivroDoEmprestimo(conexao, idEmprestimo, transacao);
                        AtualizarStatusEmprestimo(conexao, idEmprestimo, transacao);
                        AtualizarDisponibilidadeLivro(conexao, idLivro, transacao);

                        transacao.Commit();
                    }
                    catch
                    {
                        transacao.Rollback();
                        throw;
                    }
                }
            }
        }

        private int ObterIdLivroDoEmprestimo(SqlCeConnection conexao, int idEmprestimo, SqlCeTransaction transacao)
        {
            string query = "SELECT Livro FROM Emprestimo WHERE Id = @Id";
            using (SqlCeCommand cmd = new SqlCeCommand(query, conexao, transacao))
            {
                cmd.Parameters.AddWithValue("@Id", idEmprestimo);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private void AtualizarStatusEmprestimo(SqlCeConnection conexao, int idEmprestimo, SqlCeTransaction transacao)
        {
            string query = @"
                UPDATE Emprestimo 
                SET Status = @Status, DataRealDevolucao = @DataDevolucao 
                WHERE Id = @Id";

            using (SqlCeCommand cmd = new SqlCeCommand(query, conexao, transacao))
            {
                cmd.Parameters.AddWithValue("@Status", "Devolvido");
                cmd.Parameters.AddWithValue("@DataDevolucao", DateTime.Now);
                cmd.Parameters.AddWithValue("@Id", idEmprestimo);
                cmd.ExecuteNonQuery();
            }
        }

        private void AtualizarDisponibilidadeLivro(SqlCeConnection conexao, int idLivro, SqlCeTransaction transacao)
        {
            string query = @"
                UPDATE Livros 
                SET Quantidade = Quantidade + 1, Disponibilidade = 1
                WHERE Id = @IdLivro";

            using (SqlCeCommand cmd = new SqlCeCommand(query, conexao, transacao))
            {
                cmd.Parameters.AddWithValue("@IdLivro", idLivro);
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Verificação de Atrasos

        private void VerificarAtrasos()
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string query = @"
                    UPDATE Emprestimo
                    SET Status = 'Atrasado'
                    WHERE Status = 'Ativo'
                    AND (
                        (DataProrrogacao IS NOT NULL AND DataProrrogacao < @Hoje)
                        OR
                        (DataProrrogacao IS NULL AND DataDevolucao < @Hoje)
                    )";

                using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                {
                    comando.Parameters.AddWithValue("@Hoje", DateTime.Now);
                    int afetados = comando.ExecuteNonQuery();

                    if (afetados > 0)
                    {
                        MessageBox.Show($"{afetados} empréstimo(s) foram marcados como atrasados.",
                                        "Verificação de Atrasos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        #endregion

        #region Consulta de Alunos

        public Aluno BuscarAlunoPorId(int id)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string query = @"
                    SELECT Nome, Email, Turma, Telefone, Cpf, DataNascimento 
                    FROM Usuarios WHERE Id = @Id";

                using (SqlCeCommand cmd = new SqlCeCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlCeDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Aluno
                            {
                                Nome = reader["Nome"].ToString(),
                                Email = reader["Email"].ToString(),
                                Turma = reader["Turma"].ToString(),
                                Telefone = reader["Telefone"].ToString(),
                                CPF = reader["Cpf"].ToString(),
                                DataNascimento = reader["DataNascimento"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["DataNascimento"])
                                    : DateTime.MinValue
                            };
                        }
                    }
                }
            }

            return null;
        }

        #endregion

        private void DevoluçãoForm_Activated(object sender, EventArgs e)
        {
           BuscarEmprestimos();
            VerificarAtrasos();
        }
    }

    #region Classes Auxiliares

    public class FiltrosBusca
    {
        public string NomeLivro { get; set; } = "";
        public string CodigoBarras { get; set; } = "";
        public string StatusFiltro { get; set; } = "";

        public bool FiltrarCodigoBarras => !string.IsNullOrEmpty(CodigoBarras);
        public bool FiltrarStatus => StatusFiltro != "Todos" && !string.IsNullOrEmpty(StatusFiltro);
    }

    public class DefinicaoColuna
    {
        public string DataProperty { get; }
        public string Header { get; }
        public float FillWeight { get; }
        public DataGridViewContentAlignment Alignment { get; }
        public int MinWidth { get; }

        public DefinicaoColuna(string dataProperty, string header, float fillWeight,
                              DataGridViewContentAlignment alignment, int minWidth)
        {
            DataProperty = dataProperty;
            Header = header;
            FillWeight = fillWeight;
            Alignment = alignment;
            MinWidth = minWidth;
        }
    }

    public class EmprestimoInfo
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }


    #endregion
}
