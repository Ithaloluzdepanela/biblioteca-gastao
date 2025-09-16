// Substitua completamente o conteúdo do seu InicioForm.cs por este arquivo.
// Presume-se que seu Designer já tem: panelTop, panel1, lblRelogio, lblOla, timerRelogio, etc.

using BibliotecaApp.Forms.Livros;
using BibliotecaApp.Forms.Relatorio;
using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Inicio
{
    public partial class InicioForm : Form
    {
        // Adicione este campo para armazenar o top usuário
        private (string Nome, int Qtd) topUsuarioMaisEmprestimos = ("-", 0);

        // controls dinâmicos
        private FlowLayoutPanel flowCards;
        private Panel topPanelInside;         // container para cards
        private Panel actionsPanel;           // painel no canto superior direito com botão de Empréstimo Rápido
        private Button btnEmprestimoRapido;   // botão principal no topo-direito, bem visível (maior)
        private Label lblStatusSmall;
        private ToolTip formToolTip;

        // auto-refresh timer
        private System.Windows.Forms.Timer timerAutoRefresh;

        private EmprestimoRapidoForm emprestimoRap = null;
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

            // relógio
            timerRelogio.Interval = 1000;
            timerRelogio.Tick += timerRelogio_Tick;
            timerRelogio.Start();
            AtualizarRelogio();

            // tooltip
            formToolTip = new ToolTip { AutoPopDelay = 6000, InitialDelay = 300, ReshowDelay = 150, IsBalloon = false };

            // auto-refresh:
            timerAutoRefresh = new System.Windows.Forms.Timer();
            timerAutoRefresh.Interval = 20000; // 
            timerAutoRefresh.Tick += (s, ev) => { _ = CarregarEstatisticasAsync(); };
            timerAutoRefresh.Start();

            // construir UI dinâmica (cards, tabs, botão de empréstimo rápido no topo direito)
            ConstruirDashboardUI();

            // primeira carga
            _ = CarregarEstatisticasAsync();
        }

        private void ConstruirDashboardUI()
        {
            // remover controles dinâmicos antigos do panel1 (se houver)
            foreach (Control c in panel1.Controls.OfType<Control>().ToArray())
            {
                if (!(c is TabControl && c.Name == "tabEstatisticas"))
                {
                    panel1.Controls.Remove(c);
                    c.Dispose();
                }
            }

            // === TOP PANEL INSIDE (cards) ===
            if (topPanelInside != null) { panel1.Controls.Remove(topPanelInside); topPanelInside.Dispose(); }
            topPanelInside = new Panel
            {
                Name = "topPanelInside",
                Dock = DockStyle.Top,
                Height = 160,
                BackColor = Color.White
            };
            panel1.Controls.Add(topPanelInside);

            flowCards = new FlowLayoutPanel
            {
                Name = "flowCards",
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(12),
                BackColor = Color.White
            };
            topPanelInside.Controls.Add(flowCards);

            var cardsInfo = new[] 
            {
                new { Key = "Usuarios", Title = "Usuários", Sub = "Total de usuários", Color = Color.FromArgb(30,61,88) },
                new { Key = "Livros", Title = "Livros", Sub = "Total de livros", Color = Color.FromArgb(9,74,158) },
                new { Key = "EmpAtivos", Title = "Empréstimos Ativos", Sub = "Empréstimos não devolvidos", Color = Color.FromArgb(34,139,34) },
                new { Key = "Atrasados", Title = "Atrasados", Sub = "Empréstimos em atraso", Color = Color.FromArgb(178,34,34) },
                new { Key = "RapidosHoje", Title = "Rápidos (hoje)", Sub = "Empréstimos rápidos hoje", Color = Color.FromArgb(92,92,205) },
                new { Key = "MediaMes", Title = "Média/Mês", Sub = "Média de empréstimos deste mês", Color = Color.FromArgb(255, 140, 0) },
                new { Key = "TopUsuario", Title = "Top Usuário 🏆", Sub = "Quem mais emprestou", Color = Color.FromArgb(0, 123, 167) }
            };

            // Calcula o tamanho ideal para os cards
            int cardWidth = (topPanelInside.Width - 24) / cardsInfo.Length - 16; // 16 = margem
            int cardHeight = 110;

            foreach (var c in cardsInfo) flowCards.Controls.Add(CriarCard(c.Key, c.Title, c.Sub, c.Color, cardWidth, cardHeight));

            // === ACTIONS PANEL (topo direito) com botão visível e maior ===
            if (actionsPanel != null) { panelTop.Controls.Remove(actionsPanel); actionsPanel.Dispose(); }
            actionsPanel = new Panel
            {
                Name = "actionsPanel",
                Size = new Size(260, 64),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(panelTop.Width - 280, 12),
                BackColor = Color.Transparent
            };
            panelTop.Controls.Add(actionsPanel);
            actionsPanel.BringToFront();

            var btnFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                AutoSize = false,
                Padding = new Padding(8),
                Margin = new Padding(0)
            };
            actionsPanel.Controls.Add(btnFlow);

            // Botão de Empréstimo Rápido (maior, topo direito, com leve arredondamento)
            btnEmprestimoRapido = new Button
            {
                Text = "Empréstimo Rápido",
                AutoSize = false,
                Size = new Size(220, 44), // Aumentado
                BackColor = Color.FromArgb(9, 74, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                AccessibleName = "Empréstimo Rápido",
                Cursor = Cursors.Hand
            };
            btnEmprestimoRapido.FlatAppearance.BorderSize = 0;
            btnEmprestimoRapido.Click += BtnEmprestimoRapido_Click;

            // aplicar cantos arredondados (suave)
            btnEmprestimoRapido.Paint += (s, e) =>
            {
                var btn = s as Button;
                if (btn == null) return;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new Rectangle(0, 0, btn.Width, btn.Height), 8))
                using (var brush = new SolidBrush(btn.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }
                // desenhar texto manualmente para garantir centralização
                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, new Rectangle(0, 0, btn.Width, btn.Height), btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            // esconder border padrão para evitar sobreposição
            btnEmprestimoRapido.FlatAppearance.BorderSize = 0;
            btnFlow.Controls.Add(btnEmprestimoRapido);
            formToolTip.SetToolTip(btnEmprestimoRapido, "Abrir Empréstimo Rápido (atalho: Ctrl+R)");

            // reajusta posição quando redimensionar o panelTop
            panelTop.Resize += (s, e) =>
            {
                actionsPanel.Location = new Point(Math.Max(12, panelTop.Width - actionsPanel.Width - 20), actionsPanel.Location.Y);
                CenterClock(); // reposicionar relógio para centro
            };

            // lblStatusSmall (texto de status) – posicionado logo abaixo dos cards
            if (lblStatusSmall != null) { panel1.Controls.Remove(lblStatusSmall); lblStatusSmall.Dispose(); }
            lblStatusSmall = new Label
            {
                Name = "lblStatusSmall",
                Text = "",
                AutoSize = true,
                Location = new Point(18, topPanelInside.Bottom + 6),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9F),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            panel1.Controls.Add(lblStatusSmall);

            // === TabControl moderno ===
            TabControl tabEstatisticas = new TabControl
            {
                Name = "tabEstatisticas",
                Dock = DockStyle.Fill,
                Appearance = TabAppearance.Normal,
                ItemSize = new Size(120, 32),
                SizeMode = TabSizeMode.Fixed,
                DrawMode = TabDrawMode.OwnerDrawFixed,
                Font = new Font("Segoe UI", 9F)
            };
            tabEstatisticas.DrawItem += (sender, e) =>
            {
                var tabControl = (TabControl)sender;
                var tabPage = tabControl.TabPages[e.Index];
                var rect = e.Bounds;
                var isSelected = tabControl.SelectedIndex == e.Index;

                var backColor = isSelected ? Color.White : Color.FromArgb(250, 250, 250);
                var textColor = isSelected ? Color.FromArgb(30, 61, 88) : Color.FromArgb(110, 110, 110);
                var borderColor = Color.FromArgb(230, 230, 230);

                using (var brush = new SolidBrush(backColor)) e.Graphics.FillRectangle(brush, rect);
                using (var pen = new Pen(borderColor)) e.Graphics.DrawRectangle(pen, rect);

                TextRenderer.DrawText(e.Graphics, tabPage.Text,
                    new Font("Segoe UI", 9, isSelected ? FontStyle.Bold : FontStyle.Regular),
                    rect, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            var tabDevedores = new TabPage("Devedores") { BackColor = Color.White, Padding = new Padding(12) };
            var tabEstEmp = new TabPage("Empréstimos") { BackColor = Color.White, Padding = new Padding(12) };
            var tabLivros = new TabPage("Livros Populares") { BackColor = Color.White, Padding = new Padding(12) };

            tabEstatisticas.TabPages.Add(tabDevedores);
            tabEstatisticas.TabPages.Add(tabEstEmp);
            tabEstatisticas.TabPages.Add(tabLivros);

            // DataGrids com margin top aumentado para dar 'respiração'
            int topMarginDataGrid = 36; // já aumentado conforme seu pedido
            var dgvDevedores = CriarDataGridBasico("dgvDevedores");
            dgvDevedores.Margin = new Padding(12, topMarginDataGrid, 12, 12);
            dgvDevedores.Columns.AddRange(new DataGridViewColumn[] {
                new DataGridViewTextBoxColumn { Name = "Nome", HeaderText = "Nome", DataPropertyName = "Nome", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill },
                new DataGridViewTextBoxColumn { Name = "Turma", HeaderText = "Turma", DataPropertyName = "Turma", Width = 120 },
                new DataGridViewTextBoxColumn { Name = "Atrasos", HeaderText = "Atrasos", DataPropertyName = "Atrasos", Width = 80 },
                new DataGridViewTextBoxColumn { Name = "DiasAtrasoMedio", HeaderText = "Dias de Atraso (Médio)", DataPropertyName = "DiasAtrasoMedio", Width = 120 }
            });
          
            tabDevedores.Controls.Add(dgvDevedores);

            var dgvEstatEmp = CriarDataGridBasico("dgvEstatisticasEmprestimos");
            dgvEstatEmp.Margin = new Padding(12, topMarginDataGrid, 12, 12);
            dgvEstatEmp.Columns.AddRange(new DataGridViewColumn[] {
                new DataGridViewTextBoxColumn { Name = "Categoria", HeaderText = "Categoria", DataPropertyName = "Categoria", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill },
                new DataGridViewTextBoxColumn { Name = "Valor", HeaderText = "Valor", DataPropertyName = "Valor", Width = 100 },
                new DataGridViewTextBoxColumn { Name = "Detalhes", HeaderText = "Detalhes", DataPropertyName = "Detalhes", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill }
            });
          
            tabEstEmp.Controls.Add(dgvEstatEmp);

           

            // === Livros Populares: ajustar colunas para evitar truncamento do título ===
            var dgvLivrosPop = CriarDataGridBasico("dgvLivrosPopulares");
            dgvLivrosPop.Margin = new Padding(12, topMarginDataGrid, 12, 12);
            


            // Fazemos colunas com sizing misto:
            dgvLivrosPop.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // gerenciamos larguras manualmente
            var colRanking = new DataGridViewTextBoxColumn { Name = "Ranking", HeaderText = "Ranking", DataPropertyName = "Posicao", Width = 100 };
            var colTitulo = new DataGridViewTextBoxColumn { Name = "Titulo", HeaderText = "Título", DataPropertyName = "Titulo", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 260 };
            var colAutor = new DataGridViewTextBoxColumn { Name = "Autor", HeaderText = "Autor", DataPropertyName = "Autor", Width = 200 };
            var colEmp = new DataGridViewTextBoxColumn { Name = "Emprestimos", HeaderText = "Empréstimos", DataPropertyName = "Emprestimos", Width = 100 };
            var colDisp = new DataGridViewTextBoxColumn { Name = "Disponibilidade", HeaderText = "Status", DataPropertyName = "Disponibilidade", Width = 220 };

            dgvLivrosPop.CellFormatting += (s, e) =>
            {
                var grid = (DataGridView)s;
                if (grid.Columns[e.ColumnIndex].Name == "Ranking" && e.Value != null)
                {
                    int pos;
                    if (int.TryParse(e.Value.ToString(), out pos))
                    {
                        if (pos == 1)
                        {
                            e.CellStyle.ForeColor = Color.Gold; // dourado
                            e.CellStyle.Font = new Font(grid.Font, FontStyle.Bold); 
                            e.Value = "🏆 #1"; // troféu e #1
                        }
                        else
                        {
                            e.Value = $"#{pos}";
                            e.CellStyle.ForeColor = Color.Black;
                            e.CellStyle.Font = grid.Font;
                        }
                        e.FormattingApplied = true;
                    }
                }
            };

            // adiciona na ordem
            dgvLivrosPop.Columns.AddRange(new DataGridViewColumn[] { colRanking,colTitulo, colAutor, colEmp, colDisp });

            // Depois que o controle estiver em tela, definimos colTitulo como Fill para aproveitar espaço restante.
            // (Aplicamos FillWeight para priorizar bastante espaço ao título)
            colTitulo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colTitulo.FillWeight = 150; // maior proporção de espaço
            colAutor.FillWeight = 40;
            colEmp.FillWeight = 20;
            colDisp.FillWeight = 30;

            // Ajustes visuais
            colRanking.DefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleCenter;
            colTitulo.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            colTitulo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colAutor.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colEmp.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDisp.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


           

            tabLivros.Controls.Add(dgvLivrosPop);

            panel1.Controls.Add(tabEstatisticas);
            tabEstatisticas.BringToFront();

            // status inicial
            SetStatus("Pronto.");

            // aumentar e centralizar visual das labels do header: saudação e relógio
            try
            {
                lblOla.Font = new Font("Segoe UI", 20F, FontStyle.Bold);   // aumentada
                lblOla.ForeColor = Color.FromArgb(30, 61, 88);

                lblRelogio.Font = new Font("Segoe UI", 16F, FontStyle.Regular); // já aumentado anteriormente
                lblRelogio.ForeColor = Color.FromArgb(60, 60, 60);

                // garantir que o relógio seja centralizado no panelTop
                CenterClock();
            }
            catch { /* ignore se labels não existirem no designer */ }
        }

        // Helper para desenhar retângulo arredondado
        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var gp = new GraphicsPath();
            int d = radius * 2;
            gp.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            gp.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
            gp.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            gp.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        private Panel CriarCard(string key, string title, string subtitle, Color headerColor, int cardWidth, int cardHeight)
        {
            var card = new Panel
            {
                Width = 210,
                Height = 110,
                Margin = new Padding(8),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(0),
                Tag = key
            };
            card.Paint += (s, e) =>
            {
                using (var p = new Pen(Color.FromArgb(235, 239, 244)))
                    e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
            };
            var header = new Panel { BackColor = headerColor, Height = 36, Dock = DockStyle.Top };
            card.Controls.Add(header);

            var lblTitle = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                Location = new Point(10, 6),
                AutoSize = true
            };
            header.Controls.Add(lblTitle);

            var lblValue = new Label
            {
                Name = "val_" + key,
                Text = "0",
                ForeColor = Color.FromArgb(20, 42, 60),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                Location = new Point(12, header.Bottom + 6),
                AutoSize = false,
                Size = new Size(card.Width - 24, 36)
            };
            card.Controls.Add(lblValue);

            var lblSub = new Label
            {
                Text = subtitle,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(12, lblValue.Bottom + 2),
                AutoSize = true
            };
            card.Controls.Add(lblSub);

            return card;
        }

        private DataGridView CriarDataGridBasico(string name)
        {
            var dgv = new DataGridView
            {
                Name = name,
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
                AutoGenerateColumns = false,
                GridColor = Color.FromArgb(245, 245, 245),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                Margin = new Padding(12)
            };

            AplicarEstiloDataGridView(dgv);
            dgv.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#E7EEF7");
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 42, 60);
          

            dgv.CellMouseEnter += DataGrid_CellMouseEnter;
            dgv.CellMouseLeave += DataGrid_CellMouseLeave;

            return dgv;
        }

        private void AplicarEstiloDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.GridColor = Color.FromArgb(245, 245, 245);

            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgv.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#E7EEF7");
            dgv.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#123A5D");
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.DefaultCellStyle.Padding = new Padding(8, 6, 8, 6);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(70, 70, 70);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            dgv.ColumnHeadersHeight = 36;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            
       
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgv.ColumnHeadersDefaultCellStyle.BackColor;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgv.ColumnHeadersDefaultCellStyle.ForeColor;

            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;

            try
            {
                typeof(DataGridView).InvokeMember("DoubleBuffered",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                    null, dgv, new object[] { true });
            }
            catch { }

            dgv.CellFormatting += (s, e) =>
            {
                try
                {
                    var grid = (DataGridView)s;
                    if (grid.Columns[e.ColumnIndex].Name == "Disponibilidade")
                    {
                        string v = e.Value?.ToString() ?? "";
                        if (v == "Disponível")
                        {
                            e.CellStyle.BackColor = Color.FromArgb(220, 245, 225);
                            e.CellStyle.ForeColor = Color.FromArgb(20, 120, 40);
                            e.Value = "●  " + v;
                        }
                        else if (v == "Poucas unidades")
                        {
                            e.CellStyle.BackColor = Color.FromArgb(255, 250, 220);
                            e.CellStyle.ForeColor = Color.FromArgb(150, 110, 20);
                            e.Value = "●  " + v;
                        }
                        else
                        {
                            e.CellStyle.BackColor = Color.FromArgb(255, 235, 235);
                            e.CellStyle.ForeColor = Color.FromArgb(160, 30, 30);
                            e.Value = "●  " + v;
                        }
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
                catch { }
            };
        }

        private void DataGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                var dgv = sender as DataGridView;
                if (dgv == null) return;
                var hoverColor = Color.FromArgb(245, 248, 251);
                dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = hoverColor;
            }
            catch { }
        }

        private void DataGrid_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                var dgv = sender as DataGridView;
                if (dgv == null) return;
                var row = dgv.Rows[e.RowIndex];
                row.DefaultCellStyle.BackColor = (e.RowIndex % 2 == 0) ? Color.White : Color.FromArgb(250, 250, 250);
            }
            catch { }
        }

        private void EnsureEmptyOverlay(DataGridView dgv, string message)
        {
            var existing = panel1.Controls.Find("emptyOverlay_" + dgv.Name, true).FirstOrDefault() as Label;
            if (dgv.Rows.Count == 0)
            {
                if (existing == null)
                {
                    var lbl = new Label
                    {
                        Name = "emptyOverlay_" + dgv.Name,
                        Text = message,
                        AutoSize = false,
                        Width = dgv.Width,
                        Height = dgv.Height,
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = Color.Gray,
                        BackColor = Color.Transparent,
                        Font = new Font("Segoe UI", 10F, FontStyle.Regular)
                    };
                    lbl.Location = dgv.PointToScreen(Point.Empty);
                    lbl.Left = dgv.Left;
                    lbl.Top = dgv.Top;
                    dgv.Parent?.Controls.Add(lbl);
                    lbl.BringToFront();
                }
                else
                {
                    existing.Text = message;
                    existing.Visible = true;
                }
            }
            else
            {
                if (existing != null) existing.Visible = false;
            }
        }

        private void AtualizarRelogio()
        {
            DateTime agora = DateTime.Now;
            string saudacao = ObterSaudacao(agora);
            try
            {
                string nomeCompleto = Sessao.NomeBibliotecariaLogada ?? "";
                string primeiroNome = nomeCompleto.Split(' ').FirstOrDefault() ?? "";
                lblOla.Text = $"{saudacao}, {primeiroNome}!";
                lblOla.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
                lblOla.ForeColor = Color.FromArgb(30, 61, 88);
            }
            catch { }

            string diaSemana = agora.ToString("dddd");
            string data = agora.ToString("dd 'de' MMMM 'de' yyyy");
            string hora = agora.ToString("HH:mm:ss");

            lblRelogio.Text = $"{diaSemana}, {data} - {hora}";
            lblRelogio.Font = new Font("Segoe UI", 16F, FontStyle.Regular);
            lblRelogio.ForeColor = Color.FromArgb(60, 60, 60);

            // centralizar relógio após atualizar o texto
            CenterClock();
        }

        // centraliza lblRelogio horizontalmente dentro do panelTop (se existir)
        private void CenterClock()
        {
            try
            {
                if (lblRelogio == null || panelTop == null) return;
                // força medida atualizada
                lblRelogio.AutoSize = true;
                lblRelogio.Refresh();
                int centerX = Math.Max(0, (panelTop.ClientSize.Width - lblRelogio.Width) / 2);
                // respeitar margem superior (aprox. mesma Y atual)
                int y = lblRelogio.Location.Y;
                lblRelogio.Location = new Point(centerX, y);
            }
            catch { }
        }

        private string ObterSaudacao(DateTime now)
        {
            int hora = now.Hour;
            if (hora >= 5 && hora < 12) return "Bom dia";
            if (hora >= 12 && hora < 18) return "Boa tarde";
            return "Boa noite";
        }

        private void timerRelogio_Tick(object sender, EventArgs e) => AtualizarRelogio();

        #region Carregamento de dados
        private async Task CarregarEstatisticasAsync()
        {
            try
            {
                SetStatus("Carregando estatísticas...");
                var stats = await Task.Run(() => ObterEstatisticas());

                // NOVO: buscar top usuário (não-professor)
                var topUser = await Task.Run(() => ObterTopUsuarioMaisEmprestimos());
                topUsuarioMaisEmprestimos = topUser;

                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    BeginInvoke(new Action(() =>
                    {
                        AtualizarCards(stats);

                        // Atualiza o card do Top Usuário
                        var card = flowCards.Controls.OfType<Panel>().FirstOrDefault(p => (string)p.Tag == "TopUsuario");
                        if (card != null)
                        {
                            var lbl = card.Controls.Find("val_TopUsuario", true).FirstOrDefault() as Label;
                            if (lbl != null)
                            {
                                if (!string.IsNullOrWhiteSpace(topUsuarioMaisEmprestimos.Nome))
                                    lbl.Text = $"{topUsuarioMaisEmprestimos.Nome}\n({topUsuarioMaisEmprestimos.Qtd} empréstimos)";
                                else
                                    lbl.Text = "-";
                            }
                        }
                    }));
                }

                var topDevedores = await Task.Run(() => ObterTopDevedores(10));
                var estatisticasEmprestimos = await Task.Run(() => ObterEstatisticasEmprestimos());
                var livrosPopulares = await Task.Run(() => ObterLivrosPopulares(10));

                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    BeginInvoke(new Action(() =>
                    {
                        TabControl tabControl = panel1.Controls.Find("tabEstatisticas", true).FirstOrDefault() as TabControl;
                        if (tabControl != null && tabControl.TabPages.Count >= 3)
                        {
                            DataGridView dgvDevedores = tabControl.TabPages[0].Controls.Find("dgvDevedores", true).FirstOrDefault() as DataGridView;
                            DataGridView dgvEstatisticas = tabControl.TabPages[1].Controls.Find("dgvEstatisticasEmprestimos", true).FirstOrDefault() as DataGridView;
                            DataGridView dgvLivros = tabControl.TabPages[2].Controls.Find("dgvLivrosPopulares", true).FirstOrDefault() as DataGridView;

                            if (dgvDevedores != null)
                            {
                                dgvDevedores.DataSource = null;
                                dgvDevedores.DataSource = topDevedores;
                                dgvDevedores.ClearSelection();
                                dgvDevedores.Refresh();
                                EnsureEmptyOverlay(dgvDevedores, "Nenhum devedor no momento.");
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
                                EnsureEmptyOverlay(dgvLivros, "Nenhum livro encontrado por enquanto");
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

        /// <summary>
        /// Retorna o usuário (exceto professores) que mais fez empréstimos.
        /// </summary>
        private (string Nome, int Qtd) ObterTopUsuarioMaisEmprestimos()
        {
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    // Inclui professores, exclui empréstimos rápidos
                    string sql = @"
SELECT TOP 1 u.Nome, COUNT(e.Id) AS Qtd
FROM Emprestimo e
INNER JOIN Usuarios u ON e.Alocador = u.Id
LEFT JOIN EmprestimoRapido er ON e.Id = er.EmprestimoId  -- ajuste a relação
WHERE er.EmprestimoId IS NULL  -- garante que NÃO é empréstimo rápido
GROUP BY u.Nome
ORDER BY Qtd DESC, u.Nome";
            using (var cmd = new SqlCeCommand(sql, conexao))
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string nome = reader.IsDBNull(0) ? "-" : reader.GetString(0);
                    int qtd = reader.IsDBNull(1) ? 0 : Convert.ToInt32(reader.GetValue(1));
                    return (nome, qtd);
                }
            }
        }
    }
    catch (Exception ex) { try { var logDir = Path.Combine(Application.StartupPath, "logs"); Directory.CreateDirectory(logDir); File.AppendAllText(Path.Combine(logDir, "inicio_obter_top_usuario.log"), DateTime.Now + " - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine); } catch { } }
    return ("-", 0);
}

        #region Métodos de obtenção (mantidos do seu código original)
        private List<DevedorInfo> ObterTopDevedores(int topN)
        {
            var lista = new List<DevedorInfo>();
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = $@"
SELECT TOP {topN}
    u.Nome,
    u.Turma,
    COUNT(*) AS QtdAtrasos,
    AVG(CAST(DATEDIFF(day, e.DataDevolucao, COALESCE(e.DataRealDevolucao, GETDATE())) AS FLOAT)) AS DiasAtrasoMedio
FROM Emprestimo e
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
                            try { qtdAtrasos = reader.IsDBNull(2) ? 0 : Convert.ToInt32(reader.GetValue(2)); } catch { }
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
                try { var logDir = Path.Combine(Application.StartupPath, "logs"); Directory.CreateDirectory(logDir); File.AppendAllText(Path.Combine(logDir, "inicio_obter_devedores.log"), DateTime.Now + " - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine); } catch { }
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
                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo", conexao))
                        lista.Add(new EstatisticaEmprestimo { Categoria = "Empréstimos Totais", Valor = Convert.ToInt32(cmd.ExecuteScalar() ?? 0), Detalhes = "Desde o início do sistema" });

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status <> 'Devolvido'", conexao))
                        lista.Add(new EstatisticaEmprestimo { Categoria = "Empréstimos Ativos", Valor = Convert.ToInt32(cmd.ExecuteScalar() ?? 0), Detalhes = "Aguardando devolução" });

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status = 'Atrasado'", conexao))
                        lista.Add(new EstatisticaEmprestimo { Categoria = "Empréstimos Atrasados", Valor = Convert.ToInt32(cmd.ExecuteScalar() ?? 0), Detalhes = "Fora do prazo de devolução" });

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status = 'Devolvido' AND DataRealDevolucao <= DataDevolucao", conexao))
                    {
                        int noPrazo = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        int totalDevolvidos = 0;
                        using (var cmd2 = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status = 'Devolvido'", conexao))
                        {
                            totalDevolvidos = Convert.ToInt32(cmd2.ExecuteScalar() ?? 0);
                        }
                        double taxa = totalDevolvidos > 0 ? (noPrazo * 100.0 / totalDevolvidos) : 0;
                        lista.Add(new EstatisticaEmprestimo { Categoria = "Devolução no Prazo", Valor = Math.Round(taxa, 1), Detalhes = $"{noPrazo} de {totalDevolvidos} empréstimos devolvidos" });
                    }
                }
            }
            catch (Exception ex)
            {
                try { var logDir = Path.Combine(Application.StartupPath, "logs"); Directory.CreateDirectory(logDir); File.AppendAllText(Path.Combine(logDir, "inicio_obter_estatisticas.log"), DateTime.Now + " - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine); } catch { }
            }
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
SELECT TOP {topN}
    l.Nome,
    l.Autor,
    COUNT(e.Id) AS TotalEmprestimos,
    l.Quantidade
FROM Livros l
LEFT JOIN Emprestimo e ON l.Id = e.Livro
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
                            try { emprestimos = reader.IsDBNull(2) ? 0 : Convert.ToInt32(reader.GetValue(2)); } catch { }
                            try { quantidade = reader.IsDBNull(3) ? 0 : Convert.ToInt32(reader.GetValue(3)); } catch { }
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
                try { var logDir = Path.Combine(Application.StartupPath, "logs"); Directory.CreateDirectory(logDir); File.AppendAllText(Path.Combine(logDir, "inicio_obter_livros.log"), DateTime.Now + " - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine); } catch { }
            }
            return lista;
        }

        public class DevedorInfo { public int Posicao { get; set; } public string Nome { get; set; } public string Turma { get; set; } public int Atrasos { get; set; } public double DiasAtrasoMedio { get; set; } }
        public class EstatisticaEmprestimo { public string Categoria { get; set; } public double Valor { get; set; } public string Detalhes { get; set; } }
        public class LivroPopular { public int Posicao { get; set; } public string Titulo { get; set; } public string Autor { get; set; } public int Emprestimos { get; set; } public string Disponibilidade { get; set; } }

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
                ["RapidosHoje"] = 0,
                ["MediaMes"] = 0 // novo campo
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

                    // Média de empréstimos do mês atual
                    var primeiroDia = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var ultimoDia = primeiroDia.AddMonths(1).AddDays(-1);
                    using (var cmd = new SqlCeCommand(
                        "SELECT COUNT(*) FROM Emprestimo WHERE DataEmprestimo >= @inicio AND DataEmprestimo <= @fim", conexao))
                    {
                        cmd.Parameters.AddWithValue("@inicio", primeiroDia);
                        cmd.Parameters.AddWithValue("@fim", ultimoDia);
                        int totalMes = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        int dias = DateTime.Now.Day;
                        dict["MediaMes"] = dias > 0 ? (int)Math.Round(totalMes / (double)dias) : 0;
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
        }

        #endregion

        #region EmprestimoRapido open as MDI child (com toggle de botões do MainForm)
        private void BtnEmprestimoRapido_Click(object sender, EventArgs e)
        {
            AbrirEmprestimoRapido();
        }

        private async void AbrirEmprestimoRapido()
        {
            try
            {
                MainForm mainForm = this.MdiParent as MainForm;
                if (mainForm == null)
                {
                    MessageBox.Show("Não foi possível identificar a janela principal (MainForm).",
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 🔥 chama a animação e clique
                mainForm.btnLivro_Click(null, EventArgs.Empty);
                mainForm.btnEmprestimoRap_Click(null, EventArgs.Empty);

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir Empréstimo Rápido: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void EmprestimoRap_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Form main = this.FindForm();
                if (main == null) main = this.MdiParent;
                if (main == null) return;

                foreach (var kv in mainButtonsOriginalState) SetButtonEnabledOnForm(main, kv.Key, kv.Value);

                emprestimoRap = null;
                _ = CarregarEstatisticasAsync();
            }
            catch { /*silent*/ }
        }

        private Control FindControlOnForm(Form form, string controlName)
        {
            if (form == null || string.IsNullOrWhiteSpace(controlName)) return null;
            var found = form.Controls.Find(controlName, true);
            if (found != null && found.Length > 0) return found[0];
            var t = form.GetType();
            var field = t.GetField(controlName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field != null)
            {
                var val = field.GetValue(form);
                if (val is Control c) return c;
            }
            var prop = t.GetProperty(controlName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (prop != null)
            {
                var val = prop.GetValue(form);
                if (val is Control c2) return c2;
            }
            return null;
        }

        private void SetButtonEnabledOnForm(Form form, string controlName, bool enabled)
        {
            var ctrl = FindControlOnForm(form, controlName);
            if (ctrl != null) try { ctrl.Enabled = enabled; } catch { }
        }

        private void InicioForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.R)
            {
                e.SuppressKeyPress = true;
                AbrirEmprestimoRapido();
            }
        }
        #endregion
        #endregion

        private void lblResultado_Click(object sender, EventArgs e) { }

        
    }
}
