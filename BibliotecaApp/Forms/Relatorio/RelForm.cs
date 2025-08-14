using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace BibliotecaApp.Forms.Relatorio
{

    public partial class RelForm : Form
    {

        #region Classe de Conexão com o Banco de Dados

        // Centraliza as configurações de conexão com o banco
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
        public RelForm()
        {
            InitializeComponent();
            this.Load += RelForm_Load;
        }


        private void RelForm_Load(object sender, EventArgs e)
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





            // Configurações iniciais do DataGridView
            ConfigurarGrid();

        }

        private void dgvHistorico_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarLog();
        }

        private void CarregarLog()
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                // Consulta para empréstimos
                string sqlEmprestimo = @"
                    SELECT 
                        e.Id,
                        u.Nome AS NomeU,
                        l.Nome AS NomeL,
                        'Empréstimo' AS Acao,
                        e.DataEmprestimo AS DataAcao,
                        b.Nome AS Bibliotecaria
                    FROM Emprestimo e
                    LEFT JOIN Usuarios u ON e.Alocador = u.Id
                    LEFT JOIN Livros l ON e.Livro = l.Id
                    LEFT JOIN Usuarios b ON e.Responsavel = b.Id
                    WHERE 1=1";

                // Consulta para reservas
                string sqlReserva = @"
                    SELECT 
                        r.Id,
                        u.Nome AS NomeU,
                        l.Nome AS NomeL,
                        'Reserva' AS Acao,
                        r.DataReserva AS DataAcao,
                        b.Nome AS Bibliotecaria
                    FROM Reservas r
                    LEFT JOIN Usuarios u ON r.UsuarioId = u.Id
                    LEFT JOIN Livros l ON r.LivroId = l.Id
                    LEFT JOIN Usuarios b ON r.BibliotecariaId = b.Id
                    WHERE 1=1";

                // Filtros dinâmicos
                string filtros = "";
                if (!string.IsNullOrWhiteSpace(txtUsuario.Text))
                    filtros += " AND u.Nome LIKE @usuario";
                if (!string.IsNullOrWhiteSpace(txtLivro.Text))
                    filtros += " AND l.Nome LIKE @livro";
                if (cmbAcao.SelectedIndex == 1) // Empréstimos
                    filtros += " AND 'Empréstimo' = @acao";
                else if (cmbAcao.SelectedIndex == 2) // Reservas
                    filtros += " AND 'Reserva' = @acao";
                if (!string.IsNullOrWhiteSpace(txtBibliotecaria.Text))
                    filtros += " AND b.Nome LIKE @bibliotecaria";
                filtros += " AND DataAcao >= @inicio AND DataAcao <= @fim";

                // Aplica filtros nas duas consultas
                sqlEmprestimo += filtros;
                sqlReserva += filtros;

                // Junta as consultas
                string sqlFinal = $@"
                    {sqlEmprestimo}
                    UNION ALL
                    {sqlReserva}
                    ORDER BY DataAcao DESC";

                using (var cmd = new SqlCeCommand(sqlFinal, conexao))
                {
                    if (!string.IsNullOrWhiteSpace(txtUsuario.Text))
                        cmd.Parameters.AddWithValue("@usuario", "%" + txtUsuario.Text.Trim() + "%");
                    if (!string.IsNullOrWhiteSpace(txtLivro.Text))
                        cmd.Parameters.AddWithValue("@livro", "%" + txtLivro.Text.Trim() + "%");
                    if (cmbAcao.SelectedIndex == 1)
                        cmd.Parameters.AddWithValue("@acao", "Empréstimo");
                    else if (cmbAcao.SelectedIndex == 2)
                        cmd.Parameters.AddWithValue("@acao", "Reserva");
                    if (!string.IsNullOrWhiteSpace(txtBibliotecaria.Text))
                        cmd.Parameters.AddWithValue("@bibliotecaria", "%" + txtBibliotecaria.Text.Trim() + "%");
                    cmd.Parameters.AddWithValue("@inicio", dtpInicio.Value.Date);
                    cmd.Parameters.AddWithValue("@fim", dtpFim.Value.Date.AddDays(1).AddSeconds(-1));

                    var tabela = new DataTable();
                    using (var adapter = new SqlCeDataAdapter(cmd))
                    {
                        adapter.Fill(tabela);
                    }
                    dgvHistorico.DataSource = tabela;
                }
            }
        }
        private void ConfigurarGrid()
        {
            dgvHistorico.SuspendLayout();

            dgvHistorico.AutoGenerateColumns = false;
            dgvHistorico.Columns.Clear();

            // Alinha o conteúdo padrão à esquerda
            dgvHistorico.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewTextBoxColumn AddTextCol(string dataProp, string header, float fillWeight, DataGridViewContentAlignment align, int minWidth = 60, string format = null)
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
                if (!string.IsNullOrEmpty(format))
                    col.DefaultCellStyle.Format = format;
                dgvHistorico.Columns.Add(col);
                return col;
            }

            AddTextCol("NomeU", "Nome do Usuário", 180, DataGridViewContentAlignment.MiddleLeft, 120);
            AddTextCol("NomeL", "Nome do Livro", 180, DataGridViewContentAlignment.MiddleLeft, 120);
            AddTextCol("Acao", "Ação", 110, DataGridViewContentAlignment.MiddleLeft, 90);
            AddTextCol("Bibliotecaria", "Bibliotecária", 140, DataGridViewContentAlignment.MiddleLeft, 110);
            AddTextCol("DataAcao", "Data da Ação", 130, DataGridViewContentAlignment.MiddleLeft, 110, "dd/MM/yyyy HH:mm");

            // Aparência
            dgvHistorico.BackgroundColor = Color.White;
            dgvHistorico.BorderStyle = BorderStyle.None;
            dgvHistorico.GridColor = Color.FromArgb(235, 239, 244);
            dgvHistorico.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHistorico.RowHeadersVisible = false;
            dgvHistorico.ReadOnly = true;
            dgvHistorico.MultiSelect = false;
            dgvHistorico.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorico.AllowUserToAddRows = false;
            dgvHistorico.AllowUserToDeleteRows = false;
            dgvHistorico.AllowUserToResizeRows = false;

            dgvHistorico.DefaultCellStyle.BackColor = Color.White;
            dgvHistorico.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
            dgvHistorico.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            dgvHistorico.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgvHistorico.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvHistorico.RowTemplate.Height = 40;
            dgvHistorico.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

            // Cabeçalho alinhado à esquerda
            dgvHistorico.EnableHeadersVisualStyles = false;
            dgvHistorico.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvHistorico.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
            dgvHistorico.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHistorico.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            dgvHistorico.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvHistorico.ColumnHeadersHeight = 44;
            dgvHistorico.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvHistorico.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn col in dgvHistorico.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
                col.Resizable = DataGridViewTriState.False;
            }

            // Suavizar rolagem
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null,
                dgvHistorico,
                new object[] { true }
            );

            dgvHistorico.ResumeLayout();
        }
    }
}
