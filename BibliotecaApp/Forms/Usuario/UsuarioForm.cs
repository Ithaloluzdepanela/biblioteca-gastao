using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Usuario
{
    public partial class UsuarioForm : Form
    {
        public UsuarioForm()
        {
            InitializeComponent();
            this.Load += UsuarioForm_Load;

        }

        #region Classe Conexao

        // Classe estática para conectar ao banco .sdf
        public static class Conexao
        {
            public static string CaminhoBanco
            {
                get
                {
                    var raiz = Application.StartupPath;
                    return Path.Combine(raiz, "bibliotecaDB", "bibliotecaDB.sdf");
                }
            }

            public static string Conectar => $"Data Source={CaminhoBanco}; Password=123";

            public static SqlCeConnection ObterConexao()
            {
                if (!File.Exists(CaminhoBanco))
                {
                    throw new FileNotFoundException("Arquivo .sdf não encontrado no caminho: " + CaminhoBanco);
                }
                return new SqlCeConnection(Conectar);
            }
        }

        #endregion

        private void UsuarioForm_Load(object sender, EventArgs e)
        {
            var caminho = Conexao.CaminhoBanco;
            if (!File.Exists(caminho))
            {
                MessageBox.Show("Arquivo .sdf NÃO encontrado em: " + caminho +
                                "\nDefina o arquivo como 'Copy to Output Directory'.",
                                "Banco não encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Teste rápido de conexão e existência de tabela/dados
            try
            {
                using (var c = Conexao.ObterConexao())
                {
                    c.Open();
                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM usuarios", c))
                    {
                        var count = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        // Opcional para depurar:
                        // MessageBox.Show("Registros em 'usuarios': " + count);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha ao abrir o banco .sdf ou acessar a tabela 'usuarios': " + ex.Message,
                                "Erro de conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Itens e seleção padrão
            cmbTipoUsuario.Items.Clear();
            cmbTipoUsuario.Items.AddRange(new object[] { "Todos", "Aluno(a)", "Professor(a)", "Bibliotecário(a)", "Outros" });
            cmbTipoUsuario.SelectedItem = "Todos";
            if (cmbTipoUsuario.SelectedIndex < 0 && cmbTipoUsuario.Items.Count > 0)
                cmbTipoUsuario.SelectedIndex = 0;

            // Estilo e colunas do grid antes de carregar
            ConfigurarGrid();
            CarregarUsuarios();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text?.Trim() ?? string.Empty;
            string tipo = cmbTipoUsuario.SelectedItem?.ToString() ?? "Todos";
            CarregarUsuarios(nome, tipo);
        }

        private void CarregarUsuarios(string nomeFiltro = "", string tipoFiltro = "Todos")
        {
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    string sql = @"
                SELECT
                    id            AS Id,
                    nome          AS Nome,
                    email         AS Email,
                    tipousuario   AS TipoUsuario,
                    cpf           AS CPF,
                    telefone      AS Telefone,
                    turma         AS Turma,
                    datanascimento AS DataNascimento
                FROM usuarios
                WHERE 1=1";

                    if (!string.IsNullOrWhiteSpace(nomeFiltro))
                        sql += " AND nome LIKE @nome";

                    if (!string.Equals(tipoFiltro, "Todos", StringComparison.OrdinalIgnoreCase))
                        sql += " AND tipousuario LIKE @tipo"; // LIKE para tolerar variações

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        if (!string.IsNullOrWhiteSpace(nomeFiltro))
                            cmd.Parameters.AddWithValue("@nome", "%" + nomeFiltro + "%");

                        if (!string.Equals(tipoFiltro, "Todos", StringComparison.OrdinalIgnoreCase))
                            cmd.Parameters.AddWithValue("@tipo", "%" + tipoFiltro + "%");

                        var tabela = new DataTable();
                        using (var adapter = new SqlCeDataAdapter(cmd))
                        {
                            adapter.Fill(tabela);
                        }

                        dgvUsuarios.DataSource = tabela;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar usuários: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void ConfigurarGrid()
        {
            dgvUsuarios.SuspendLayout();

            dgvUsuarios.AutoGenerateColumns = false;
            dgvUsuarios.Columns.Clear();

            DataGridViewTextBoxColumn AddTextCol(string dataProp, string header, float fillWeight, DataGridViewContentAlignment align, int minWidth = 60)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = dataProp, // casa com os alias do SELECT
                    Name = dataProp,             // usado em Columns["..."]
                    HeaderText = header,
                    ReadOnly = true,
                    FillWeight = fillWeight,
                    MinimumWidth = minWidth,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = align, WrapMode = DataGridViewTriState.False }
                };
                dgvUsuarios.Columns.Add(col);
                return col;
            }

            AddTextCol("Id", "ID", 50, DataGridViewContentAlignment.MiddleCenter, 40);
            AddTextCol("Nome", "Nome", 180, DataGridViewContentAlignment.MiddleLeft, 120);
            AddTextCol("Email", "E-mail", 200, DataGridViewContentAlignment.MiddleLeft, 140);
            AddTextCol("TipoUsuario", "Tipo", 110, DataGridViewContentAlignment.MiddleCenter, 90);
            AddTextCol("CPF", "CPF", 110, DataGridViewContentAlignment.MiddleCenter, 90);
            AddTextCol("Telefone", "Telefone", 120, DataGridViewContentAlignment.MiddleCenter, 90);
            AddTextCol("Turma", "Turma", 100, DataGridViewContentAlignment.MiddleCenter, 80);
            var nasc = AddTextCol("DataNascimento", "Nascimento", 130, DataGridViewContentAlignment.MiddleCenter, 110);
            nasc.DefaultCellStyle.Format = "dd/MM/yyyy";
            nasc.DefaultCellStyle.NullValue = "";

            // Aparência
            dgvUsuarios.BackgroundColor = Color.White;
            dgvUsuarios.BorderStyle = BorderStyle.None;
            dgvUsuarios.GridColor = Color.FromArgb(235, 239, 244);
            dgvUsuarios.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToResizeRows = false;

            dgvUsuarios.DefaultCellStyle.BackColor = Color.White;
            dgvUsuarios.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
            dgvUsuarios.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvUsuarios.RowTemplate.Height = 40;
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

            // Cabeçalho
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvUsuarios.ColumnHeadersHeight = 44;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Largura
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Sort
            foreach (DataGridViewColumn col in dgvUsuarios.Columns)
                col.SortMode = DataGridViewColumnSortMode.Automatic;

            // Suavizar rolagem
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                dgvUsuarios,
                new object[] { true }
            );

            dgvUsuarios.ResumeLayout();
        }

        private void txtNome_Load(object sender, EventArgs e)
        {

        }
    }
}
