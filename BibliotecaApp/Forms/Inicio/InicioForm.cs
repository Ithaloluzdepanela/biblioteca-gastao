using BibliotecaApp.Forms.Livros;
using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
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
            timerRelogio.Interval = 1000;
            timerRelogio.Tick += timerRelogio_Tick;
            timerRelogio.Start();
            AtualizarRelogio();

            lblOla.Text = $"Olá, {Sessao.NomeBibliotecariaLogada}!";

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

            // DataGrid estilizado (top atrasos)
            dgvTopAtrasos = new DataGridView
            {
                Location = new Point(30, btnAtualizar.Bottom + 18),
                Size = new Size(panel1.Width - 60, panel1.Height - (btnAtualizar.Bottom + 40)),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
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
                RowHeadersVisible = false
            };

            // definir colunas
            dgvTopAtrasos.Columns.Clear();
            var colNome = new DataGridViewTextBoxColumn
            {
                Name = "Nome",
                HeaderText = "Nome",
                DataPropertyName = "Nome",
                ReadOnly = true,
                MinimumWidth = 200,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            };
            dgvTopAtrasos.Columns.Add(colNome);

            var colTurma = new DataGridViewTextBoxColumn
            {
                Name = "Turma",
                HeaderText = "Turma",
                DataPropertyName = "Turma",
                ReadOnly = true,
                MinimumWidth = 120,
                Width = 140,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            };
            dgvTopAtrasos.Columns.Add(colTurma);

            var colQtd = new DataGridViewTextBoxColumn
            {
                Name = "Qtd",
                HeaderText = "Atrasos",
                DataPropertyName = "Qtd",
                ReadOnly = true,
                MinimumWidth = 80,
                Width = 80,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };
            dgvTopAtrasos.Columns.Add(colQtd);

            // estilo visual (igual EmprestimoRapidoForm)
            dgvTopAtrasos.GridColor = Color.FromArgb(235, 239, 244);
            dgvTopAtrasos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dgvTopAtrasos.DefaultCellStyle.BackColor = Color.White;
            dgvTopAtrasos.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
            dgvTopAtrasos.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            dgvTopAtrasos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgvTopAtrasos.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvTopAtrasos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

            dgvTopAtrasos.EnableHeadersVisualStyles = false;
            dgvTopAtrasos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvTopAtrasos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
            dgvTopAtrasos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTopAtrasos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            dgvTopAtrasos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvTopAtrasos.ColumnHeadersHeight = 44;
            dgvTopAtrasos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvTopAtrasos.AllowUserToResizeColumns = false;
            dgvTopAtrasos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // double buffered
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, dgvTopAtrasos, new object[] { true });

            dgvTopAtrasos.CellFormatting -= DgvTopAtrasos_CellFormatting;
            dgvTopAtrasos.CellFormatting += DgvTopAtrasos_CellFormatting;

            panel1.Controls.Add(dgvTopAtrasos);
            dgvTopAtrasos.BringToFront();
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

        private void AtualizarRelogio()
        {
            lblRelogio.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy - HH:mm:ss")
            );
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

                var top = await Task.Run(() => ObterTopAtrasos(10));
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    BeginInvoke(new Action(() =>
                    {
                        dgvTopAtrasos.DataSource = top;
                        SetStatus($"Última atualização: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    }));
                }
            }
            catch (Exception ex)
            {
                BeginInvoke(new Action(() => SetStatus($"Erro ao carregar: {ex.Message}")));
            }
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

        private List<TopAtrasoItem> ObterTopAtrasos(int topN)
        {
            var lista = new List<TopAtrasoItem>();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    string sql = $@"
                        SELECT TOP {topN} u.Nome, u.Turma, COUNT(*) AS Qtd
                        FROM Emprestimo e
                        INNER JOIN Usuarios u ON e.Alocador = u.Id
                        WHERE e.Status = 'Atrasado'
                        GROUP BY u.Nome, u.Turma
                        ORDER BY Qtd DESC";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var nome = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            var turma = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            int qtd = 0;
                            try { qtd = reader.GetInt32(2); }
                            catch { try { qtd = Convert.ToInt32(reader.GetDecimal(2)); } catch { qtd = 0; } }
                            lista.Add(new TopAtrasoItem { Nome = nome, Turma = turma, Qtd = qtd });
                        }
                    }
                }
            }
            catch { /*silent*/ }

            return lista;
        }

        class TopAtrasoItem { public string Nome { get; set; } public string Turma { get; set; } public int Qtd { get; set; } }

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
                // find main form (top-level window that contém este control)
                Form main = this.FindForm();
                if (main == null) main = this.MdiParent; // fallback

                if (main == null)
                {
                    MessageBox.Show("Não foi possível identificar a janela principal (MainForm).", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // se já existe emprestimoRap aberto, traz para frente
                if (emprestimoRap != null && !emprestimoRap.IsDisposed)
                {
                    emprestimoRap.BringToFront();
                    return;
                }

                // nomes dos botões que você citou (sem duplicatas)
                string[] btnNames = new string[]
                {
                    "btnEmprestimoRap","btnRel","btnEmprestimo","btnLivros","btnInicio","btnDev",
                    "btnLivroCad","btnUser","btnUserCad","btnUserEdit"
                };

                // salvar estados originais e aplicar novos estados
                mainButtonsOriginalState.Clear();
                foreach (var name in btnNames)
                {
                    var ctrl = FindControlOnForm(main, name);
                    if (ctrl != null)
                    {
                        mainButtonsOriginalState[name] = ctrl.Enabled;
                    }
                }

                // aplicar o comportamento que você pediu:
                // btnEmprestimoRap.Enabled = false;
                // os demais true
                SetButtonEnabledOnForm(main, "btnEmprestimoRap", false);
                foreach (var name in btnNames.Where(n => !string.Equals(n, "btnEmprestimoRap", StringComparison.OrdinalIgnoreCase)))
                    SetButtonEnabledOnForm(main, name, true);

                // criar e abrir EmprestimoRapido como MDI child
                emprestimoRap = new EmprestimoRapidoForm();
                emprestimoRap.FormClosed += EmprestimoRap_FormClosed;
           emprestimoRap.MdiParent = this.MdiParent;

                
                // define MdiParent = main (se main for MdiContainer) senão atribui null
                try
                {
                    var mdiMain = main;
                    // se main não for MdiContainer podemos setar mesmo assim — o WinForms exige IsMdiContainer = true no main.
                    // então verificamos:
                    var mainType = main.GetType();
                    var prop = mainType.GetProperty("IsMdiContainer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    bool isMdiContainer = false;
                    if (prop != null) isMdiContainer = (bool)prop.GetValue(main);

                    if (isMdiContainer)
                    {
                        emprestimoRap.MdiParent = main;
                        emprestimoRap.Dock = DockStyle.Fill;
                        emprestimoRap.Show();
                    }
                    else
                    {
                        // Se main não for container MDI, abre como Form filho dentro de um Panel — tentaremos adicionar ao painel central se houver.
                        // Tenta localizar um panel chamado "panelContainer" ou "panel1" no MainForm; se não encontrar, abre normal com Show()
                        Control container = FindControlOnForm(main, "panelContainer") ?? FindControlOnForm(main, "panelCentral") ?? FindControlOnForm(main, "panel1") as Control;
                        if (container != null)
                        {
                            emprestimoRap.TopLevel = false;
                            emprestimoRap.FormBorderStyle = FormBorderStyle.None;
                            emprestimoRap.Dock = DockStyle.Fill;
                            container.Controls.Add(emprestimoRap);
                            emprestimoRap.Show();
                        }
                        else
                        {
                            // fallback: apenas Show() (não modal)
                            emprestimoRap.Show();
                        }
                    }
                }
                catch
                {
                    // fallback simples
                    emprestimoRap.Show();
                }
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
