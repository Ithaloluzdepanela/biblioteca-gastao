using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using BibliotecaApp.Utils;

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
            BuscarEmprestimos();
            ConfigurarGridEmprestimos();
            cbFiltroEmprestimo.SelectedIndex = 0;
            VerificarAtrasos();
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

        private void dgvEmprestimos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            FormatarCelulaStatus(e);
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
                LimparTabela();
            }
        }

        private void LimparTabela()
        {
            txtNome.Text = "";
            mtxCodigoBarras.Text = "";
            txtNome.Focus();
        }

        private void FormatarCelulaStatus(DataGridViewCellFormattingEventArgs e)
        {
            if (dgvEmprestimos.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString().Trim();

                switch (status)
                {
                    case "Ativo":
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                        break;

                    case "Atrasado":
                        e.CellStyle.ForeColor = Color.Red;
                        e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                        break;

                    case "Devolvido":
                        e.CellStyle.ForeColor = Color.DimGray;
                        e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Regular);
                        break;
                }
            }
        }

        #endregion

        #region Métodos de Busca e Dados

        private void BuscarEmprestimos()
        {
            string nomeLivro = txtNome.Text.Trim();
            string codigoBarras = mtxCodigoBarras.Text.Trim();
            string statusFiltro = cbFiltroEmprestimo.SelectedItem?.ToString();

            bool filtrarCodigo = !string.IsNullOrEmpty(codigoBarras);
            bool filtrarStatus = statusFiltro != "Todos" && !string.IsNullOrEmpty(statusFiltro);

            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string query = ConstruirQueryBusca(filtrarCodigo, filtrarStatus);

                using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                {
                    comando.Parameters.AddWithValue("@NomeLivro", "%" + nomeLivro + "%");

                    if (filtrarCodigo)
                        comando.Parameters.AddWithValue("@CodigoBarras", "%" + codigoBarras + "%");

                    if (filtrarStatus)
                        comando.Parameters.AddWithValue("@Status", statusFiltro);

                    SqlCeDataAdapter adaptador = new SqlCeDataAdapter(comando);
                    DataTable tabela = new DataTable();
                    adaptador.Fill(tabela);

                    dgvEmprestimos.DataSource = tabela;
                }
            }
        }



        private string ConstruirQueryBusca(bool filtrarCodigoBarras, bool filtrarStatus)
        {
            string query = @"
        SELECT 
            e.Id AS [ID do Empréstimo],
            uAlocador.Nome AS [Alocador],
            uResponsavel.Nome AS [Responsável],
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

            if (filtrarCodigoBarras)
                query += " AND l.CodigoBarras LIKE @CodigoBarras";

            if (filtrarStatus)
                query += " AND e.Status = @Status";

            query += @"
        ORDER BY 
            CASE e.Status
                WHEN 'Atrasado' THEN 1
                WHEN 'Ativo' THEN 2
                WHEN 'Devolvido' THEN 3
                ELSE 4
            END";

            return query;
        }


        #endregion

        #region Configuração do Grid

        private void ConfigurarGridEmprestimos()
        {
            dgvEmprestimos.SuspendLayout();

            ConfigurarColunasGrid();
            ConfigurarEstiloGrid();
            ConfigurarEventosGrid();

            dgvEmprestimos.ResumeLayout();
        }

        private void ConfigurarColunasGrid()
        {
            dgvEmprestimos.AutoGenerateColumns = false;
            dgvEmprestimos.Columns.Clear();
            dgvEmprestimos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            AdicionarColuna("ID do Empréstimo", "ID", 50, DataGridViewContentAlignment.MiddleCenter, 40);
            AdicionarColuna("Alocador", "Alocador", 160, DataGridViewContentAlignment.MiddleLeft, 100);
            AdicionarColuna("Responsável", "Responsável", 160, DataGridViewContentAlignment.MiddleLeft, 100);
            AdicionarColuna("Livro", "Nome do Livro", 180, DataGridViewContentAlignment.MiddleLeft, 120);
            AdicionarColuna("Código De Barras", "Código de Barras", 160, DataGridViewContentAlignment.MiddleLeft, 120);
            AdicionarColuna("Data do Empréstimo", "Data de Empréstimo", 150, DataGridViewContentAlignment.MiddleCenter, 110);
            AdicionarColuna("Data de Devolução", "Data de Devolução", 140, DataGridViewContentAlignment.MiddleCenter, 100);
            AdicionarColuna("Status", "Status", 100, DataGridViewContentAlignment.MiddleCenter, 80);
        }
        private void AdicionarColuna(string dataProp, string header, float fillWeight, DataGridViewContentAlignment align, int minWidth)
        {
            var col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = dataProp,
                Name = dataProp,
                HeaderText = header,
                ReadOnly = true,
                FillWeight = fillWeight,
                MinimumWidth = minWidth,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = align,
                    WrapMode = DataGridViewTriState.False,
                    SelectionBackColor = Color.FromArgb(16, 87, 174), // azul escuro
                    SelectionForeColor = Color.White                 // texto branco
                }
            };
            dgvEmprestimos.Columns.Add(col);
        }



        private void ConfigurarEstiloGrid()
        {
            // Configurações básicas
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

            ConfigurarEstiloCelulas();
            ConfigurarEstiloCabecalho();
            ConfigurarColunas();
            HabilitarDoubleBuffering();
        }

        private void ConfigurarEstiloCelulas()
        {
            dgvEmprestimos.DefaultCellStyle.BackColor = Color.White;
            dgvEmprestimos.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
            dgvEmprestimos.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            dgvEmprestimos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(16, 87, 174); // azul escuro
            dgvEmprestimos.DefaultCellStyle.SelectionForeColor = Color.White;                // texto branco
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

            switch (status)
            {
                case "Atrasado":
                    row.Cells["Status"].Style.ForeColor = Color.Red;
                    row.Cells["Status"].Style.SelectionForeColor = Color.Red;
                    break;
                case "Ativo":
                    row.Cells["Status"].Style.ForeColor = Color.Green;
                    row.Cells["Status"].Style.SelectionForeColor = Color.Green;
                    break;
                case "Finalizado":
                    row.Cells["Status"].Style.ForeColor = Color.DimGray;
                    row.Cells["Status"].Style.SelectionForeColor = Color.DimGray;
                    break;
            }
        }

        #endregion

        #region Métodos de Devolução

        private void DevolverEmprestimo()
        {
            if (!ValidarSelecaoEmprestimo()) return;

            int idEmprestimo = ObterIdEmprestimoSelecionado();
            string statusAtual = ObterStatusEmprestimoSelecionado();

            if (statusAtual == "Devolvido")
            {
                MessageBox.Show("Este empréstimo já foi devolvido.");
                return;
            }

            ProcessarDevolucaoNoBanco(idEmprestimo);
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

        private int ObterIdEmprestimoSelecionado()
        {
            return Convert.ToInt32(dgvEmprestimos.SelectedRows[0].Cells["ID do Empréstimo"].Value);
        }

        private string ObterStatusEmprestimoSelecionado()
        {
            return dgvEmprestimos.SelectedRows[0].Cells["Status"].Value?.ToString();
        }

        private void ProcessarDevolucaoNoBanco(int idEmprestimo)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                int idLivro = ObterIdLivroDoEmprestimo(conexao, idEmprestimo);
                AtualizarStatusEmprestimo(conexao, idEmprestimo);
                AtualizarDisponibilidadeLivro(conexao, idLivro);
            }
        }

        private int ObterIdLivroDoEmprestimo(SqlCeConnection conexao, int idEmprestimo)
        {
            string queryLivro = "SELECT Livro FROM Emprestimo WHERE Id = @Id";
            using (SqlCeCommand cmdLivro = new SqlCeCommand(queryLivro, conexao))
            {
                cmdLivro.Parameters.AddWithValue("@Id", idEmprestimo);
                object result = cmdLivro.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private void AtualizarStatusEmprestimo(SqlCeConnection conexao, int idEmprestimo)
        {
            string queryEmprestimo = @"
                UPDATE Emprestimo 
                SET Status = @Status, DataRealDevolucao = @DataDevolucao 
                WHERE Id = @Id";

            using (SqlCeCommand cmdEmprestimo = new SqlCeCommand(queryEmprestimo, conexao))
            {
                cmdEmprestimo.Parameters.AddWithValue("@Status", "Devolvido");
                cmdEmprestimo.Parameters.AddWithValue("@DataDevolucao", DateTime.Now);
                cmdEmprestimo.Parameters.AddWithValue("@Id", idEmprestimo);
                cmdEmprestimo.ExecuteNonQuery();
            }
        }

        private void AtualizarDisponibilidadeLivro(SqlCeConnection conexao, int idLivro)
        {
            string queryLivroUpdate = @"
                UPDATE Livros 
                SET Quantidade = Quantidade + 1,
                Disponibilidade = 1
                WHERE Id = @IdLivro";

            using (SqlCeCommand cmdUpdateLivro = new SqlCeCommand(queryLivroUpdate, conexao))
            {
                cmdUpdateLivro.Parameters.AddWithValue("@IdLivro", idLivro);
                cmdUpdateLivro.ExecuteNonQuery();
            }
        }

        private void VerificarAtrasos()
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string query = @"UPDATE Emprestimo
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


    }
}
