using BibliotecaApp.Forms.Livros;
using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BibliotecaApp.Forms.Inicio
{
    public partial class InicioForm : Form
    {
        // controls dinâmicos
        private FlowLayoutPanel flowCards;
        private DataGridView dgvTopAtrasos;
        private Button btnAtualizar;
        private Button btnEmprestimoRapido;
        private Label lblStatusSmall;

        // referência à janela de Empréstimo Rápido (se já aberta)
        private EmprestimoRapidoForm emprestimoRap = null;

        // guarda estados originais dos botões do MainForm para restaurar ao fechar o MDI child
        private Dictionary<string, bool> mainButtonsOriginalState = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        public InicioForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += InicioForm_KeyDown;
        }

        #region Conexao (padrão do app)
        public static class Conexao
        {
            public static string CaminhoBanco => Application.StartupPath + @"\bibliotecaDB\bibliotecaDB.sdf";
            public static string Conectar => $"Data Source={CaminhoBanco}; Password=123";
            public static SqlCeConnection ObterConexao() => new SqlCeConnection(Conectar);
        }
        #endregion

        private void InicioForm_Load(object sender, EventArgs e)
        {
            AppPaths.EnsureFolders();

            // relógio e saudação
            timerRelogio.Interval = 100; // Atualiza a cada 100ms para uma animação mais suave
            timerRelogio.Tick += timerRelogio_Tick;
            timerRelogio.Start();
            AtualizarRelogio();

            lblOla.Text = $"Olá, {Sessao.NomeBibliotecariaLogada}!";

            // Ajuste o tamanho do painel superior para acomodar o relógio maior
            panelTop.Height = 100;

            // construir dashboard
            ConstruirDashboardUI();
            Task.Run(() => CarregarEstatisticasAsync());
        }

        private void ConstruirDashboardUI()
        {
            // remover anterior se necessário
            if (flowCards != null)
            {
                panel1.Controls.Remove(flowCards);
                flowCards.Dispose();
            }

            flowCards = new FlowLayoutPanel
            {
                Name = "flowCards",
                Location = new Point(30, 60),
                Size = new Size(panel1.Width - 60, 140),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent
            };

            panel1.Controls.Add(flowCards);
            flowCards.BringToFront();

            var cardsInfo = new[]
            {
                new { Key = "Usuarios", Title = "Usuários", Sub = "Total de usuários", Color = Color.FromArgb(30,61,88) },
                new { Key = "Livros", Title = "Livros", Sub = "Total de livros", Color = Color.FromArgb(9,74,158) },
                new { Key = "EmpAtivos", Title = "Empréstimos Ativos", Sub = "Empréstimos não devolvidos", Color = Color.FromArgb(34,139,34) },
                new { Key = "Atrasados", Title = "Atrasados", Sub = "Empréstimos em atraso", Color = Color.FromArgb(178,34,34) },
                new { Key = "Reservas", Title = "Reservas Pendentes", Sub = "Reservas aguardando", Color = Color.FromArgb(233,149,25) },
                new { Key = "RapidosHoje", Title = "Rápidos (hoje)", Sub = "Empréstimos rápidos hoje", Color = Color.FromArgb(92,92,205) }
            };

            foreach (var c in cardsInfo)
                flowCards.Controls.Add(CriarCard(c.Key, c.Title, c.Sub, c.Color));

            // botão ATUALIZAR
            btnAtualizar = new Button
            {
                Text = "ATUALIZAR",
                AutoSize = true,
                BackColor = Color.FromArgb(30, 61, 88),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Location = new Point(30, flowCards.Bottom + 12),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.Click += (s, e) => Task.Run(() => CarregarEstatisticasAsync());
            panel1.Controls.Add(btnAtualizar);

            // botão EMPRESTIMO RÁPIDO (atalho)
            btnEmprestimoRapido = new Button
            {
                Text = "EMPRÉSTIMO RÁPIDO",
                AutoSize = true,
                BackColor = Color.FromArgb(9, 74, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Location = new Point(btnAtualizar.Right + 10, flowCards.Bottom + 12),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            btnEmprestimoRapido.FlatAppearance.BorderSize = 0;
            btnEmprestimoRapido.Click += BtnEmprestimoRapido_Click;
            panel1.Controls.Add(btnEmprestimoRapido);

            lblStatusSmall = new Label
            {
                Text = "",
                AutoSize = true,
                Location = new Point(btnEmprestimoRapido.Right + 12, flowCards.Bottom + 18),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9F)
            };
            panel1.Controls.Add(lblStatusSmall);


            // TabControl moderno e minimalista
            TabControl tabEstatisticas = new TabControl
            {
                Name = "tabEstatisticas",
                Location = new Point(30, btnAtualizar.Bottom + 18),
                Size = new Size(panel1.Width - 60, Math.Max(200, panel1.Height - (btnAtualizar.Bottom + 40))),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Appearance = TabAppearance.Normal,
                ItemSize = new Size(120, 32),
                SizeMode = TabSizeMode.Fixed,
                DrawMode = TabDrawMode.OwnerDrawFixed
            };

            // Estilização moderna das abas
            tabEstatisticas.DrawItem += (sender, e) =>
            {
                var tabControl = (TabControl)sender;
                var tabPage = tabControl.TabPages[e.Index];
                var rect = e.Bounds;
                var isSelected = tabControl.SelectedIndex == e.Index;

                // Cores modernas
                var backColor = isSelected ? Color.White : Color.FromArgb(240, 240, 240);
                var textColor = isSelected ? Color.FromArgb(30, 61, 88) : Color.FromArgb(120, 120, 120);
                var borderColor = Color.FromArgb(220, 220, 220);

                using (var brush = new SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }

                // Borda sutil
                using (var pen = new Pen(borderColor))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }

                // Texto estilizado
                TextRenderer.DrawText(e.Graphics, tabPage.Text,
                    new Font("Segoe UI", 9, isSelected ? FontStyle.Bold : FontStyle.Regular),
                    rect, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            // Aba 1: Principais Devedores - Estilizada
            TabPage tabDevedores = new TabPage("Devedores")
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(0) // remove 1px padding que podia parecer sombra
            };

            // Aba 2: Estatísticas de Empréstimos - Estilizada
            TabPage tabEstatisticasEmprestimos = new TabPage("Empréstimos")
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(0)
            };

            // Aba 3: Livros Populares - Estilizada
            TabPage tabLivrosPopulares = new TabPage("Livros Populares")
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(0)
            };

            tabEstatisticas.TabPages.Add(tabDevedores);
            tabEstatisticas.TabPages.Add(tabEstatisticasEmprestimos);
            tabEstatisticas.TabPages.Add(tabLivrosPopulares);

            panel1.Controls.Add(tabEstatisticas);

            // DataGridView para devedores - com estilo aprimorado
            DataGridView dgvDevedores = new DataGridView
            {
                Name = "dgvDevedores",
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                RowTemplate = { Height = 40 },
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 44,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                AutoGenerateColumns = false,
                GridColor = Color.White, // deixar clean
                CellBorderStyle = DataGridViewCellBorderStyle.None, // tira linhas internas
                Margin = new Padding(0)
            };

            // Aplicar estilo personalizado
            AplicarEstiloDataGridView(dgvDevedores);

            // Adicionar efeito de destaque para a linha selecionada
            dgvDevedores.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgvDevedores.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 42, 60);

            // Configurar colunas para devedores
            var colPosicao = new DataGridViewTextBoxColumn
            {
                Name = "Posicao",
                HeaderText = "#",
                DataPropertyName = "Posicao", // <-- corrigido: agora preenche a posição
                Width = 40,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10f, FontStyle.Bold) }
            };

            var colNomeDevedor = new DataGridViewTextBoxColumn
            {
                Name = "Nome",
                HeaderText = "Nome",
                DataPropertyName = "Nome",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            };

            var colTurmaDevedor = new DataGridViewTextBoxColumn
            {
                Name = "Turma",
                HeaderText = "Turma",
                DataPropertyName = "Turma",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            };

            var colAtrasos = new DataGridViewTextBoxColumn
            {
                Name = "Atrasos",
                HeaderText = "Atrasos",
                DataPropertyName = "Atrasos",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold) }
            };

            var colDiasAtrasoMedio = new DataGridViewTextBoxColumn
            {
                Name = "DiasAtrasoMedio",
                HeaderText = "Dias de Atraso (Médio)",
                DataPropertyName = "DiasAtrasoMedio",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            dgvDevedores.Columns.AddRange(new DataGridViewColumn[] { colPosicao, colNomeDevedor, colTurmaDevedor, colAtrasos, colDiasAtrasoMedio });

            AplicarEstiloDataGridView(dgvDevedores);
            tabDevedores.Controls.Add(dgvDevedores);

            // DataGridView para estatísticas de empréstimos
            DataGridView dgvEstatisticasEmprestimos = new DataGridView
            {
                Name = "dgvEstatisticasEmprestimos",
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                RowTemplate = { Height = 40 },
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 44,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                AutoGenerateColumns = false,
                GridColor = Color.FromArgb(240, 240, 240),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };

            AplicarEstiloDataGridView(dgvEstatisticasEmprestimos);

            dgvEstatisticasEmprestimos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgvEstatisticasEmprestimos.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 42, 60);

            // Configurar colunas para estatísticas
            var colCategoria = new DataGridViewTextBoxColumn
            {
                Name = "Categoria",
                HeaderText = "Categoria",
                DataPropertyName = "Categoria",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 61, 88)
                }
            };

            var colValor = new DataGridViewTextBoxColumn
            {
                Name = "Valor",
                HeaderText = "Valor",
                DataPropertyName = "Valor",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(9, 74, 158)
                }
            };

            var colDetalhes = new DataGridViewTextBoxColumn
            {
                Name = "Detalhes",
                HeaderText = "Detalhes",
                DataPropertyName = "Detalhes",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    ForeColor = Color.Gray,
                    Font = new Font("Segoe UI", 9f)
                }
            };

            dgvEstatisticasEmprestimos.Columns.Add(colCategoria);
            dgvEstatisticasEmprestimos.Columns.Add(colValor);
            dgvEstatisticasEmprestimos.Columns.Add(colDetalhes);

            AplicarEstiloDataGridView(dgvEstatisticasEmprestimos);
            tabEstatisticasEmprestimos.Controls.Add(dgvEstatisticasEmprestimos);

            // DataGridView para livros populares
            DataGridView dgvLivrosPopulares = new DataGridView
            {
                Name = "dgvLivrosPopulares",
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                RowTemplate = { Height = 36 },
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 40,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                AutoGenerateColumns = false, // Isso evita colunas duplicadas
                CellBorderStyle = DataGridViewCellBorderStyle.None,
                GridColor = Color.White,
                Margin = new Padding(0)
            };

            AplicarEstiloDataGridView(dgvLivrosPopulares);
            dgvLivrosPopulares.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgvLivrosPopulares.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 42, 60);

            // Configurar colunas para livros populares
            var colPosicaoLivro = new DataGridViewTextBoxColumn
            {
                Name = "Posicao",
                HeaderText = "#",
                DataPropertyName = "Posicao",
                Width = 40,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(100, 100, 100)
                }
            };

            var colTituloLivro = new DataGridViewTextBoxColumn
            {
                Name = "Titulo",
                HeaderText = "Título",
                DataPropertyName = "Titulo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 200,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 0, 0, 0)
                }
            };

            var colEmprestimos = new DataGridViewTextBoxColumn
            {
                Name = "Emprestimos",
                HeaderText = "Empréstimos",
                DataPropertyName = "Emprestimos",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 61, 88)
                }
            };

            var colDisponibilidade = new DataGridViewTextBoxColumn
            {
                Name = "Disponibilidade",
                HeaderText = "Status",
                DataPropertyName = "Disponibilidade",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9f)
                }
            };

            dgvLivrosPopulares.Columns.AddRange(new DataGridViewColumn[] {
    colPosicaoLivro, colTituloLivro, colEmprestimos, colDisponibilidade
});

            AplicarEstiloDataGridView(dgvLivrosPopulares);
            tabLivrosPopulares.Controls.Add(dgvLivrosPopulares);

        }

        private Panel CriarCard(string key, string title, string subtitle, Color headerColor)
        {
            var card = new Panel
            {
                Width = 220,
                Height = 120,
                Margin = new Padding(8),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(0),
            };

            // borda sutil
            card.Paint += (s, e) =>
            {
                using (var p = new Pen(Color.FromArgb(235, 239, 244)))
                    e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
            };

            var header = new Panel { BackColor = headerColor, Height = 44, Dock = DockStyle.Top };
            card.Controls.Add(header);

            var lblTitle = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                Location = new Point(12, 8),
                AutoSize = true
            };
            header.Controls.Add(lblTitle);

            var lblValue = new Label
            {
                Name = "val_" + key,
                Text = "0",
                ForeColor = Color.FromArgb(20, 42, 60),
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                Location = new Point(12, header.Bottom + 6),
                AutoSize = false,
                Size = new Size(card.Width - 24, 44)
            };
            card.Controls.Add(lblValue);

            var lblSub = new Label
            {
                Text = subtitle,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(12, lblValue.Bottom + 2),
                AutoSize = true
            };
            card.Controls.Add(lblSub);

            card.Tag = key;
            return card;
        }

        private void AplicarEstiloDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.GridColor = Color.White; // clean

            // Estilo das células
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 240, 240);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 61, 88);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.DefaultCellStyle.Padding = new Padding(8, 6, 8, 6);

            // Linhas alternadas
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);

            // Cabeçalhos das colunas
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Removido handler de CellPainting que desenhava linhas em condições estranhas

            // Double buffering para performance
            try
            {
                typeof(DataGridView).InvokeMember("DoubleBuffered",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                    null, dgv, new object[] { true });
            }
            catch { /* ignore em ambientes onde não é possível */ }
        }

        private void AtualizarRelogio()
        {
            DateTime agora = DateTime.Now;
            string diaSemana = agora.ToString("dddd");
            string data = agora.ToString("dd 'de' MMMM 'de' yyyy");
            string hora = agora.ToString("HH:mm:ss");

            lblRelogio.Text = $"{diaSemana}, {data} - {hora}";
            lblRelogio.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblRelogio.ForeColor = Color.FromArgb(30, 61, 88);
        }

        private void timerRelogio_Tick(object sender, EventArgs e) => AtualizarRelogio();

        #region Carregamento de dados

        private async Task CarregarEstatisticasAsync()
        {
            try
            {
                SetStatus("Carregando estatísticas...");
                var stats = await Task.Run(() => ObterEstatisticas());

                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    BeginInvoke(new Action(() => AtualizarCards(stats)));
                }

                // Carregar dados para as abas
                var topDevedores = await Task.Run(() => ObterTopDevedores(10));
                var estatisticasEmprestimos = await Task.Run(() => ObterEstatisticasEmprestimos());
                var livrosPopulares = await Task.Run(() => ObterLivrosPopulares(10));

                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    BeginInvoke(new Action(() =>
                    {
                        // Encontrar os DataGridViews pelo TabControl
                        TabControl tabControl = panel1.Controls.Find("tabEstatisticas", true).FirstOrDefault() as TabControl;
                        if (tabControl != null && tabControl.TabPages.Count >= 3)
                        {
                            DataGridView dgvDevedores = tabControl.TabPages[0].Controls.Find("dgvDevedores", true).FirstOrDefault() as DataGridView;
                            DataGridView dgvEstatisticas = tabControl.TabPages[1].Controls.Find("dgvEstatisticasEmprestimos", true).FirstOrDefault() as DataGridView;
                            DataGridView dgvLivros = tabControl.TabPages[2].Controls.Find("dgvLivrosPopulares", true).FirstOrDefault() as DataGridView;

                            if (dgvDevedores != null)
                            {
                                dgvDevedores.DataSource = null; // garantir refresh
                                dgvDevedores.DataSource = topDevedores;
                                dgvDevedores.ClearSelection();
                                dgvDevedores.Refresh();
                            }
                            if (dgvEstatisticas != null)
                            {
                                dgvEstatisticas.DataSource = null;
                                dgvEstatisticas.DataSource = estatisticasEmprestimos;
                                dgvEstatisticas.ClearSelection();
                                dgvEstatisticas.Refresh();
                            }
                            if (dgvLivros != null)
                            {
                                dgvLivros.DataSource = null;
                                dgvLivros.DataSource = livrosPopulares;
                                dgvLivros.ClearSelection();
                                dgvLivros.Refresh();
                            }
                        }

                        SetStatus($"Última atualização: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    }));
                }
            }
            catch (Exception ex)
            {
                BeginInvoke(new Action(() => SetStatus($"Erro ao carregar: {ex.Message}")));
            }
        }

        private List<DevedorInfo> ObterTopDevedores(int topN)
        {
            var lista = new List<DevedorInfo>();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // Consulta simplificada e mais resiliente
                    string sql = $@"
                SELECT TOP (@topN)
       u.Nome,
       u.Turma,
       COUNT(*) AS QtdAtrasos,
       AVG(DATEDIFF(day, e.DataDevolucao, e.DataRealDevolucao)) AS DiasAtrasoMedio
FROM Emprestimos e
INNER JOIN Usuarios u ON e.Alocador = u.Id
WHERE e.Status = 'Atrasado'
GROUP BY u.Nome, u.Turma
ORDER BY QtdAtrasos DESC, DiasAtrasoMedio DESC";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        int posicao = 1;
                        while (reader.Read())
                        {
                            var nome = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            var turma = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            int qtdAtrasos = 0;
                            double diasAtrasoMedio = 0;

                            try { qtdAtrasos = reader.GetInt32(2); } catch { }
                            try { diasAtrasoMedio = reader.IsDBNull(3) ? 0 : Convert.ToDouble(reader.GetValue(3)); } catch { }

                            lista.Add(new DevedorInfo
                            {
                                Posicao = posicao++,
                                Nome = nome,
                                Turma = turma,
                                Atrasos = qtdAtrasos,
                                DiasAtrasoMedio = Math.Round(diasAtrasoMedio, 1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log para auxiliar debugging - cria um arquivo simples no diretório de logs
                try
                {
                    var logDir = Path.Combine(Application.StartupPath, "logs");
                    Directory.CreateDirectory(logDir);
                    File.AppendAllText(Path.Combine(logDir, "inicio_obter_devedores.log"), DateTime.Now + " - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine);
                }
                catch { }
            }

            return lista;
        }

        private List<EstatisticaEmprestimo> ObterEstatisticasEmprestimos()
        {
            var lista = new List<EstatisticaEmprestimo>();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // Empréstimos totais
                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo", conexao))
                    {
                        int total = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        lista.Add(new EstatisticaEmprestimo
                        {
                            Categoria = "Empréstimos Totais",
                            Valor = total,
                            Detalhes = "Desde o início do sistema"
                        });
                    }

                    // Empréstimos ativos
                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status <> 'Devolvido'", conexao))
                    {
                        int ativos = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        lista.Add(new EstatisticaEmprestimo
                        {
                            Categoria = "Empréstimos Ativos",
                            Valor = ativos,
                            Detalhes = "Aguardando devolução"
                        });
                    }

                    // Empréstimos atrasados
                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status = 'Atrasado'", conexao))
                    {
                        int atrasados = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        lista.Add(new EstatisticaEmprestimo
                        {
                            Categoria = "Empréstimos Atrasados",
                            Valor = atrasados,
                            Detalhes = "Fora do prazo de devolução"
                        });
                    }

                    // Taxa de devolução no prazo
                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status = 'Devolvido' AND DataDevolucaoReal <= DataDevolucaoPrevista", conexao))
                    {
                        int noPrazo = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        int totalDevolvidos = 0;

                        using (var cmd2 = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status = 'Devolvido'", conexao))
                        {
                            totalDevolvidos = Convert.ToInt32(cmd2.ExecuteScalar() ?? 0);
                        }

                        double taxa = totalDevolvidos > 0 ? (noPrazo * 100.0 / totalDevolvidos) : 0;

                        lista.Add(new EstatisticaEmprestimo
                        {
                            Categoria = "Devolução no Prazo",
                            Valor = Math.Round(taxa, 1),
                            Detalhes = $"{noPrazo} de {totalDevolvidos} empréstimos devolvidos"
                        });
                    }
                }
            }
            catch { /*silent*/ }

            return lista;
        }

        private List<LivroPopular> ObterLivrosPopulares(int topN)
        {
            var lista = new List<LivroPopular>();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    string sql = $@"
                SELECT TOP (@topN)
       l.Nome,
       l.Autor,
       COUNT(e.Id) AS TotalEmprestimos,
       l.Quantidade
FROM Livros l
LEFT JOIN Emprestimos e ON l.Id = e.Livro
GROUP BY l.Id, l.Nome, l.Autor, l.Quantidade
ORDER BY TotalEmprestimos DESC, l.Nome";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        int posicao = 1;
                        while (reader.Read())
                        {
                            var nome = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            var autor = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            int emprestimos = 0;
                            int quantidade = 0;

                            try { emprestimos = reader.GetInt32(2); } catch { }
                            try { quantidade = reader.GetInt32(3); } catch { }

                            string disponibilidade = quantidade > 0 ? "Disponível" : "Indisponível";
                            if (quantidade > 0 && quantidade < 5) disponibilidade = "Poucas unidades";

                            lista.Add(new LivroPopular
                            {
                                Posicao = posicao++,
                                Titulo = nome,
                                Autor = autor,
                                Emprestimos = emprestimos,
                                Disponibilidade = disponibilidade
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var logDir = Path.Combine(Application.StartupPath, "logs");
                    Directory.CreateDirectory(logDir);
                    File.AppendAllText(Path.Combine(logDir, "inicio_obter_livros.log"), DateTime.Now + " - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine);
                }
                catch { }
            }

            return lista;
        }

        public class DevedorInfo
        {
            public int Posicao { get; set; }
            public string Nome { get; set; }
            public string Turma { get; set; }
            public int Atrasos { get; set; }
            public double DiasAtrasoMedio { get; set; }
        }

        public class EstatisticaEmprestimo
        {
            public string Categoria { get; set; }
            public double Valor { get; set; }
            public string Detalhes { get; set; }
        }

        public class LivroPopular
        {
            public int Posicao { get; set; }
            public string Titulo { get; set; }
            public string Autor { get; set; }
            public int Emprestimos { get; set; }
            public string Disponibilidade { get; set; }
        }

        private void SetStatus(string texto)
        {
            if (lblStatusSmall == null) return;
            try { BeginInvoke(new Action(() => lblStatusSmall.Text = texto)); } catch { }
        }

        private Dictionary<string, int> ObterEstatisticas()
        {
            var dict = new Dictionary<string, int>
            {
                ["Usuarios"] = 0,
                ["Livros"] = 0,
                ["EmpAtivos"] = 0,
                ["Atrasados"] = 0,
                ["Reservas"] = 0,
                ["RapidosHoje"] = 0
            };

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Usuarios", conexao))
                        dict["Usuarios"] = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Livros", conexao))
                        dict["Livros"] = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status <> 'Devolvido'", conexao))
                        dict["EmpAtivos"] = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status = 'Atrasado'", conexao))
                        dict["Atrasados"] = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Reservas WHERE Status = 'Pendente'", conexao))
                        dict["Reservas"] = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                    using (var cmd = new SqlCeCommand("SELECT Id, DataHoraEmprestimo FROM EmprestimoRapido WHERE Status = 'Ativo'", conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        int cont = 0;
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(1))
                            {
                                var dt = reader.GetDateTime(1);
                                if (dt.Date == DateTime.Now.Date) cont++;
                            }
                        }
                        dict["RapidosHoje"] = cont;
                    }
                }
            }
            catch { /*silent*/ }

            return dict;
        }

        private void AtualizarCards(Dictionary<string, int> stats)
        {
            if (stats == null) return;
            foreach (Control c in flowCards.Controls)
            {
                if (c is Panel card && card.Tag is string key)
                {
                    var valLabel = card.Controls.Find("val_" + key, true).FirstOrDefault() as Label;
                    if (valLabel != null)
                    {
                        int v = stats.ContainsKey(key) ? stats[key] : 0;
                        valLabel.Text = v.ToString("N0");
                    }
                }
            }

            // Forçar uma atualização visual do DataGridView
        }

        #endregion

        #region DataGrid formatting

        private void DgvTopAtrasos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                var grid = sender as DataGridView;
                if (grid.Columns[e.ColumnIndex].Name == "Qtd")
                {
                    if (e.Value == null || e.Value == DBNull.Value) return;
                    if (int.TryParse(e.Value.ToString(), out int qtd))
                    {
                        if (qtd > 0)
                        {
                            e.CellStyle.ForeColor = Color.FromArgb(178, 34, 34);
                            e.CellStyle.Font = new Font(grid.DefaultCellStyle.Font, FontStyle.Bold);
                        }
                        else
                        {
                            e.CellStyle.ForeColor = Color.FromArgb(20, 42, 60);
                            e.CellStyle.Font = grid.DefaultCellStyle.Font;
                        }
                    }
                }
                else
                {
                    e.CellStyle.Font = (grid.DefaultCellStyle?.Font ?? new Font("Segoe UI", 10f));
                    e.CellStyle.ForeColor = Color.FromArgb(20, 42, 60);
                }
            }
            catch { /* non-blocking */ }
        }

        #endregion

        #region EmprestimoRapido open as MDI child (com toggle de botões do MainForm)

        private void BtnEmprestimoRapido_Click(object sender, EventArgs e)
        {
            AbrirEmprestimoRapido();
        }

        private void AbrirEmprestimoRapido()
        {
            try
            {
                // Se já existe emprestimoRap aberto, traz para frente
                if (emprestimoRap != null && !emprestimoRap.IsDisposed)
                {
                    emprestimoRap.BringToFront();
                    return;
                }

                // Obter o MainForm (MDI container)
                Form mainForm = this.MdiParent;
                if (mainForm == null)
                {
                    MessageBox.Show("Não foi possível identificar a janela principal (MainForm).", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Salvar estados originais dos botões do MainForm
                string[] btnNames = new string[]
                {
            "btnEmprestimoRap","btnRel","btnEmprestimo","btnLivros","btnInicio","btnDev",
            "btnLivroCad","btnUser","btnUserCad","btnUserEdit"
                };

                mainButtonsOriginalState.Clear();
                foreach (var name in btnNames)
                {
                    var ctrl = FindControlOnForm(mainForm, name);
                    if (ctrl != null)
                    {
                        mainButtonsOriginalState[name] = ctrl.Enabled;
                    }
                }

                // Aplicar comportamento desejado nos botões
                SetButtonEnabledOnForm(mainForm, "btnEmprestimoRap", false);
                foreach (var name in btnNames.Where(n => !string.Equals(n, "btnEmprestimoRap", StringComparison.OrdinalIgnoreCase)))
                    SetButtonEnabledOnForm(mainForm, name, true);

                // Criar e abrir EmprestimoRapido como MDI child
                emprestimoRap = new EmprestimoRapidoForm();
                emprestimoRap.FormClosed += EmprestimoRap_FormClosed;
                emprestimoRap.MdiParent = mainForm;
                emprestimoRap.Dock = DockStyle.Fill;
                emprestimoRap.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir Empréstimo Rápido: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EmprestimoRap_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // restaurar estados originais dos botões
                Form main = this.FindForm();
                if (main == null) main = this.MdiParent;
                if (main == null) return;

                foreach (var kv in mainButtonsOriginalState)
                {
                    SetButtonEnabledOnForm(main, kv.Key, kv.Value);
                }

                // limpar referencia
                emprestimoRap = null;

                // atualizar estatísticas após fechar
                Task.Run(() => CarregarEstatisticasAsync());
            }
            catch { /*silent*/ }
        }

        // helper: encontra Control (Button) no form via reflection (campos) ou Controls.Find recursivo
        private Control FindControlOnForm(Form form, string controlName)
        {
            if (form == null || string.IsNullOrWhiteSpace(controlName)) return null;

            // 1) tenta Controls.Find (caso controle seja público/componente do designer)
            var found = form.Controls.Find(controlName, true);
            if (found != null && found.Length > 0) return found[0];

            // 2) tenta reflection em fields (private/protected)
            var t = form.GetType();
            var field = t.GetField(controlName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field != null)
            {
                var val = field.GetValue(form);
                if (val is Control c) return c;
            }

            // 3) tenta property
            var prop = t.GetProperty(controlName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (prop != null)
            {
                var val = prop.GetValue(form);
                if (val is Control c2) return c2;
            }

            return null;
        }

        // helper: seta Enabled em um controle do form (se existir)
        private void SetButtonEnabledOnForm(Form form, string controlName, bool enabled)
        {
            var ctrl = FindControlOnForm(form, controlName);
            if (ctrl != null)
            {
                try { ctrl.Enabled = enabled; }
                catch { /*ignore*/ }
            }
        }

        // atalho Ctrl+R para abrir EmprestimoRapido; Ctrl+U para atualizar
        private void InicioForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.R)
            {
                e.SuppressKeyPress = true;
                AbrirEmprestimoRapido();
            }
            if (e.Control && e.KeyCode == Keys.U)
            {
                e.SuppressKeyPress = true;
                Task.Run(() => CarregarEstatisticasAsync());
            }
        }

        #endregion

        private void lblResultado_Click(object sender, EventArgs e)
        {

        }

    }
}
