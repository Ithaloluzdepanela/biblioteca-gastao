using BibliotecaApp.Models;
using BibliotecaApp.Services;
using BibliotecaApp.Forms.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Livros
{
    public partial class EmprestimoRapidoForm : Form
    {
        private List<string> turmasCadastradas = new List<string>();
        private List<string> livrosCadastrados = new List<string>();
        private List<string> professoresCadastrados = new List<string>();

        // Dicionário de turmas padrão
        private Dictionary<string, string[]> dicionarioTurmas = new Dictionary<string, string[]>
        {
            { "Ano", new[] { "6° Ano", "7° Ano", "8° Ano", "9° Ano" } },
            { "Desenvolvimento", new[] { "1° Desenvolvimento", "2° Desenvolvimento", "3° Desenvolvimento" } },
            { "Agronegócio", new[] { "1° Agronegócio", "2° Agronegócio", "3° Agronegócio" } },
            { "Propedêutico", new[] { "1° Propedêutico", "2° Propedêutico", "3° Propedêutico" } }
        };

        private List<string> todasTurmasPadrao;

        // Conexao (reaproveita padrão)
        public static class Conexao
        {
            public static string CaminhoBanco => Application.StartupPath + @"\bibliotecaDB\bibliotecaDB.sdf";
            public static string Conectar => $"Data Source={CaminhoBanco}; Password=123";
            public static SqlCeConnection ObterConexao() => new SqlCeConnection(Conectar);
        }

        public EmprestimoRapidoForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += EmprestimoRapidoForm_KeyDown;
        }

        private void EmprestimoRapidoForm_Load(object sender, EventArgs e)
        {
            // Inicializar lista de todas as turmas padrão
            InicializarTurmasPadrao();

            //Limpeza Automatica Semanal
            LimparEmprestimosSemana();
            // Carrega dados para sugestões (turmas, livros, professores) e bibliotecárias
            CarregarSugestoesECombo();

            txtProfessor.Focus();

            lblHoraEmprestimo.Text = $"Hora do Empréstimo: {DateTime.Now: HH:mm}";

            // Configura DataGrid
            ConfigurarGridRapidos();
            CarregarGridRapidos();

            numQuantidade.Text = "1"; // valor inicial
            numQuantidade.KeyPress += numQuantidade_KeyPress;
            numQuantidade.TextChanged += numQuantidade_TextChanged;

            // Eventos para o autocomplete de Turma
            txtTurma.KeyDown += txtTurma_KeyDown;
            txtTurma.Leave += txtTurma_Leave;

            lstSugestoesTurma.Click += lstSugestoesTurma_Click;
            lstSugestoesTurma.KeyDown += lstSugestoesTurma_KeyDown;
            lstSugestoesTurma.Leave += lstSugestoesTurma_Leave;

            // Esconde listboxes inicialmente
            lstSugestoesProfessor.Visible = false;
            lstSugestoesLivro.Visible = false;
            lstSugestoesTurma.Visible = false;

            EstilizarListBoxSugestao(lstSugestoesProfessor);
            EstilizarListBoxSugestao(lstSugestoesLivro);
            EstilizarListBoxSugestao(lstSugestoesTurma);
        }

        

        private void EmprestimoRapidoForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // evita bip ou comportamento padrão

                // Se o foco estiver no botão Registrar -> dispara o clique
                if (this.ActiveControl == btnRegistrar)
                {
                    btnRegistrar.PerformClick();
                }
                else
                {
                    // Senão, navega para o próximo campo
                    this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
                }
            }
        }

        private void LimparEmprestimosSemana()
        {
            try
            {
                // Caminho para a pasta AppData dentro do diretório do programa
                string pastaAppData = Path.Combine(Application.StartupPath, "AppData");

                // Cria a pasta se não existir
                if (!Directory.Exists(pastaAppData))
                    Directory.CreateDirectory(pastaAppData);

                string arquivoControle = Path.Combine(pastaAppData, "limpeza.txt");

                DateTime ultimaLimpeza = DateTime.MinValue;
                if (File.Exists(arquivoControle))
                {
                    DateTime.TryParse(File.ReadAllText(arquivoControle), out ultimaLimpeza);
                }

                // Obtem a data da segunda-feira desta semana
                DateTime segundaDaSemana = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);

                // Só limpa se a última limpeza não foi nesta semana
                if (ultimaLimpeza < segundaDaSemana)
                {
                    using (var conexao = Conexao.ObterConexao())
                    {
                        conexao.Open();

                        // Apaga todos os registros
                        using (var cmd = new SqlCeCommand("DELETE FROM EmprestimoRapido", conexao))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        // Reseta o ID
                        using (var cmdReset = new SqlCeCommand(
                            "ALTER TABLE EmprestimoRapido ALTER COLUMN Id IDENTITY (1,1)", conexao))
                        {
                            cmdReset.ExecuteNonQuery();
                        }
                    }

                    // Atualiza data da última limpeza
                    File.WriteAllText(arquivoControle, DateTime.Now.ToString("yyyy-MM-dd"));

                    MessageBox.Show("Histórico de Empréstimos foi limpo. Novo ciclo semanal iniciado.",
                                    "Limpeza Semanal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao limpar empréstimos rápidos: " + ex.Message);
            }
        }

        private void CarregarSugestoesECombo()
        {
            turmasCadastradas.Clear();
            livrosCadastrados.Clear();
            professoresCadastrados.Clear();
            cbBibliotecaria.Items.Clear();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // Turmas (distinct)
                    using (var cmd = new SqlCeCommand("SELECT DISTINCT Turma FROM Usuarios WHERE Turma IS NOT NULL AND Turma <> ''", conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            turmasCadastradas.Add(reader.GetString(0));
                        }
                    }

                    // Livros disponíveis
                    using (var cmd = new SqlCeCommand("SELECT Nome FROM Livros WHERE Nome IS NOT NULL", conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            livrosCadastrados.Add(reader.GetString(0));
                        }
                    }

                    // Professores
                    using (var cmd = new SqlCeCommand("SELECT Nome FROM Usuarios WHERE TipoUsuario LIKE '%Professor%'", conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            professoresCadastrados.Add(reader.GetString(0));
                        }
                    }

                    // Bibliotecárias (para combo)
                    using (var cmd = new SqlCeCommand("SELECT Nome FROM Usuarios WHERE TipoUsuario = 'Bibliotecário(a)'", conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbBibliotecaria.Items.Add(reader.GetString(0));
                        }
                    }

                    // Seleciona automaticamente a logada
                    if (!string.IsNullOrWhiteSpace(Sessao.NomeBibliotecariaLogada))
                    {
                        int idx = cbBibliotecaria.Items.IndexOf(Sessao.NomeBibliotecariaLogada);
                        if (idx >= 0)
                            cbBibliotecaria.SelectedIndex = idx;
                        else
                        {

                        }
                        {
                            cbBibliotecaria.Items.Add(Sessao.NomeBibliotecariaLogada);
                            cbBibliotecaria.SelectedIndex = cbBibliotecaria.Items.Count - 1;
                        }
                    }

                    // fallback
                    if (cbBibliotecaria.Items.Count == 0 && !string.IsNullOrWhiteSpace(Sessao.NomeBibliotecariaLogada))
                    {
                        cbBibliotecaria.Items.Add(Sessao.NomeBibliotecariaLogada);
                        cbBibliotecaria.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados iniciais: " + ex.Message);
            }
        }

        #region Métodos de Turma
        private void InicializarTurmasPadrao()
        {
            todasTurmasPadrao = new List<string>();
            foreach (var categoria in dicionarioTurmas.Values)
            {
                todasTurmasPadrao.AddRange(categoria);
            }
        }

        private string CorrigirTurma(string turmaDigitada)
        {
            if (string.IsNullOrWhiteSpace(turmaDigitada))
                return turmaDigitada;

            // Adicionar ° automaticamente se não tiver e começar com número
            if (!turmaDigitada.Contains("°") && Regex.IsMatch(turmaDigitada, @"^\d+"))
            {
                // Encontrar onde termina o número
                int i = 0;
                while (i < turmaDigitada.Length && char.IsDigit(turmaDigitada[i]))
                {
                    i++;
                }

                // Inserir o símbolo de grau após o número
                if (i < turmaDigitada.Length)
                {
                    turmaDigitada = turmaDigitada.Insert(i, "°");
                }
                else
                {
                    turmaDigitada = turmaDigitada + "°";
                }
            }

            // Extrair número e tipo
            string numeroStr = "";
            string tipo = "";
            string numeroTurma = "";

            // Extrair o número principal (antes do °)
            Match matchNumero = Regex.Match(turmaDigitada, @"^(\d+)°");
            if (matchNumero.Success)
            {
                numeroStr = matchNumero.Groups[1].Value;
            }

            // Determinar o tipo de turma
            string turmaLower = turmaDigitada.ToLower();
            if (turmaLower.Contains("d"))
            {
                tipo = "Desenvolvimento";
            }
            else if (turmaLower.Contains("ag"))
            {
                tipo = "Agronegócio";
            }
            else if (turmaLower.Contains("p"))
            {
                tipo = "Propedêutico";
            }
            else if (turmaLower.Contains("an"))
            {
                tipo = "Ano";
            }

            // Extrair número da turma (no final)
            Match matchNumeroTurma = Regex.Match(turmaDigitada, @"(\d+)$");
            if (matchNumeroTurma.Success)
            {
                numeroTurma = matchNumeroTurma.Value;
            }

            // Corrigir número principal se for impossível
            if (!string.IsNullOrEmpty(numeroStr))
            {
                int numero;
                if (int.TryParse(numeroStr, out numero))
                {
                    if (tipo == "Ano")
                    {
                        // Para turmas de Ano: 6° a 9°
                        if (numero < 6) numero = 6;
                        else if (numero > 9) numero = 9;
                    }
                    else if (!string.IsNullOrEmpty(tipo))
                    {
                        // Para outras turmas: 1° a 3°
                        if (numero < 1) numero = 1;
                        else if (numero > 3) numero = 3;
                    }

                    numeroStr = numero.ToString();
                }
            }

            // Corrigir número da turma se for impossível
            if (!string.IsNullOrEmpty(numeroTurma))
            {
                int numero;
                if (int.TryParse(numeroTurma, out numero))
                {
                    if (numero < 1) numero = 1;
                    numeroTurma = numero.ToString();
                }
            }

            // Montar turma corrigida
            string turmaCorrigida = !string.IsNullOrEmpty(numeroStr) ? numeroStr + "°" : "";

            if (!string.IsNullOrEmpty(tipo))
            {
                turmaCorrigida += " " + tipo;
            }

            if (!string.IsNullOrEmpty(numeroTurma))
            {
                turmaCorrigida += " " + numeroTurma;
            }

            return !string.IsNullOrEmpty(turmaCorrigida) ? turmaCorrigida.Trim() : turmaDigitada;
        }

        private void txtTurma_TextChanged(object sender, EventArgs e)
        {
            string texto = txtTurma.Text.Trim();

            if (string.IsNullOrEmpty(texto))
            {
                lstSugestoesTurma.Visible = false;
                return;
            }

            // Buscar sugestões apenas nas turmas já cadastradas
            var sugestoes = turmasCadastradas
                .Where(t => t.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0)
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            lstSugestoesTurma.Items.Clear();

            if (sugestoes.Count > 0)
            {
                foreach (var s in sugestoes)
                    lstSugestoesTurma.Items.Add(s);

                int visibleItems = Math.Min(5, sugestoes.Count);
                int extraPadding = 8;
                lstSugestoesTurma.Height = visibleItems * lstSugestoesTurma.ItemHeight + extraPadding;
                lstSugestoesTurma.Width = txtTurma.Width;
                lstSugestoesTurma.Left = txtTurma.Left;
                lstSugestoesTurma.Top = txtTurma.Bottom;
                lstSugestoesTurma.Visible = true;
            }
            else
            {
                lstSugestoesTurma.Visible = false;
            }
        }

        private void txtTurma_Leave(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                if (!lstSugestoesTurma.Focused)
                {
                    lstSugestoesTurma.Visible = false;

                    string texto = txtTurma.Text.Trim();
                    if (!string.IsNullOrEmpty(texto))
                    {
                        // Corrigir automaticamente a formatação ao sair do campo
                        string turmaCorrigida = CorrigirTurma(texto);
                        if (turmaCorrigida != texto)
                        {
                            txtTurma.Text = turmaCorrigida;
                        }
                    }
                }
            }));
        }

        private void lstSugestoesTurma_Click(object sender, EventArgs e)
        {
            if (lstSugestoesTurma.SelectedItem != null)
            {
                string turmaSelecionada = lstSugestoesTurma.SelectedItem.ToString();
                txtTurma.Text = turmaSelecionada;
                lstSugestoesTurma.Visible = false;
                txtTurma.Focus();
            }
        }

        private void txtTurma_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesTurma.Visible || lstSugestoesTurma.Items.Count == 0)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    this.SelectNextControl((Control)sender, true, true, true, true);
                }
                return;
            }

            if (e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
                lstSugestoesTurma.Focus();
                if (lstSugestoesTurma.Items.Count > 0)
                    lstSugestoesTurma.SelectedIndex = 0;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (lstSugestoesTurma.SelectedItem != null)
                    txtTurma.Text = lstSugestoesTurma.SelectedItem.ToString();
                else if (lstSugestoesTurma.Items.Count > 0)
                    txtTurma.Text = lstSugestoesTurma.Items[0].ToString();

                lstSugestoesTurma.Visible = false;
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesTurma.Visible = false;
            }
        }

        private void lstSugestoesTurma_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstSugestoesTurma.SelectedItem != null)
            {
                e.SuppressKeyPress = true;
                txtTurma.Text = lstSugestoesTurma.SelectedItem.ToString();
                lstSugestoesTurma.Visible = false;
                txtTurma.Focus();
                this.SelectNextControl(txtTurma, true, true, true, true);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesTurma.Visible = false;
                txtTurma.Focus();
            }
        }

        private void lstSugestoesTurma_Leave(object sender, EventArgs e)
        {
            lstSugestoesTurma.Visible = false;
        }
        #endregion

        #region Autocomplete listboxes (Professor / Livro)
        private void txtProfessor_TextChanged(object sender, EventArgs e)
        {
            var txt = txtProfessor.Text.Trim().ToLower();
            lstSugestoesProfessor.Items.Clear();
            if (string.IsNullOrWhiteSpace(txt)) { lstSugestoesProfessor.Visible = false; return; }

            var sugest = professoresCadastrados.Where(x => x.ToLower().Contains(txt)).ToArray();
            if (sugest.Any())
            {
                lstSugestoesProfessor.Items.AddRange(sugest);
                lstSugestoesProfessor.Visible = true;
            }
            else lstSugestoesProfessor.Visible = false;
        }

        private void lstSugestoesProfessor_Click(object sender, EventArgs e)
        {
            if (lstSugestoesProfessor.SelectedItem != null)
            {
                txtProfessor.Text = lstSugestoesProfessor.SelectedItem.ToString();
                lstSugestoesProfessor.Visible = false;
            }
        }

        private void txtLivro_TextChanged(object sender, EventArgs e)
        {
            var txt = txtLivro.Text.Trim().ToLower();
            lstSugestoesLivro.Items.Clear();
            if (string.IsNullOrWhiteSpace(txt)) { lstSugestoesLivro.Visible = false; return; }

            var sugest = livrosCadastrados.Where(x => x.ToLower().Contains(txt)).ToArray();
            if (sugest.Any())
            {
                lstSugestoesLivro.Items.AddRange(sugest);
                lstSugestoesLivro.Visible = true;
            }
            else lstSugestoesLivro.Visible = false;
        }

        private void lstSugestoesLivro_Click(object sender, EventArgs e)
        {
            if (lstSugestoesLivro.SelectedItem != null)
            {
                txtLivro.Text = lstSugestoesLivro.SelectedItem.ToString();
                lstSugestoesLivro.Visible = false;
            }
        }
        #endregion

        #region Registrar Empréstimo Rápido
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // validações
            if (string.IsNullOrWhiteSpace(txtProfessor.Text) ||
                string.IsNullOrWhiteSpace(txtLivro.Text) ||
                string.IsNullOrWhiteSpace(txtTurma.Text) ||
                !int.TryParse(numQuantidade.Text, out int qtd) || qtd <= 0 ||
                cbBibliotecaria.SelectedItem == null)
            {
                MessageBox.Show("Preencha todos os campos obrigatórios.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // checar livro e quantidade disponível
                    int livroId;
                    int quantidadeDisponivel;
                    using (var cmd = new SqlCeCommand("SELECT Id, Quantidade FROM Livros WHERE Nome = @nome", conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", txtLivro.Text.Trim());
                        using (var r = cmd.ExecuteReader())
                        {
                            if (!r.Read())
                            {
                                MessageBox.Show("Livro não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            livroId = Convert.ToInt32(r["Id"]);
                            quantidadeDisponivel = Convert.ToInt32(r["Quantidade"]);
                        }
                    }

                    if (qtd > quantidadeDisponivel)
                    {
                        MessageBox.Show($"Quantidade indisponível. Estoque atual: {quantidadeDisponivel}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // obter professor id
                    int professorId;
                    using (var cmd = new SqlCeCommand("SELECT Id FROM Usuarios WHERE Nome = @nome AND TipoUsuario LIKE @tipo", conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", txtProfessor.Text.Trim());
                        cmd.Parameters.AddWithValue("@tipo", "%Professor%");
                        var obj = cmd.ExecuteScalar();
                        if (obj == null)
                        {
                            MessageBox.Show("Professor não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        professorId = Convert.ToInt32(obj);
                    }

                    // Inserir EmprestimoRapido
                    string insertSql = @"INSERT INTO EmprestimoRapido
(ProfessorId, LivroId, Turma, Quantidade, DataHoraEmprestimo, DataHoraDevolucaoReal, Bibliotecaria, Status)
VALUES (@prof, @livro, @turma, @qt, @dataEmp, NULL, @bibli, 'Ativo')";

                    using (var cmd = new SqlCeCommand(insertSql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@prof", professorId);
                        cmd.Parameters.AddWithValue("@livro", livroId);
                        cmd.Parameters.AddWithValue("@turma", txtTurma.Text.Trim());
                        cmd.Parameters.AddWithValue("@qt", qtd);
                        cmd.Parameters.AddWithValue("@dataEmp", DateTime.Now);
                        cmd.Parameters.AddWithValue("@bibli", cbBibliotecaria.SelectedItem.ToString());
                        cmd.ExecuteNonQuery();
                    }

                    // Atualizar quantidade do livro
                    int novaQuantidade = quantidadeDisponivel - qtd;
                    bool disponivel = novaQuantidade > 0;
                    using (var cmd = new SqlCeCommand("UPDATE Livros SET Quantidade = @qt, Disponibilidade = @disp WHERE Id = @id", conexao))
                    {
                        cmd.Parameters.AddWithValue("@qt", novaQuantidade);
                        cmd.Parameters.AddWithValue("@disp", disponivel);
                        cmd.Parameters.AddWithValue("@id", livroId);
                        cmd.ExecuteNonQuery();
                    }
                } // using conexao

                MessageBox.Show("Empréstimo rápido registrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                CarregarSugestoesECombo(); // recarrega quantidades / listas
                CarregarGridRapidos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao registrar empréstimo rápido: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCampos()
        {
            txtProfessor.Text = "";
            txtLivro.Text = "";
            txtTurma.Text = "";
            numQuantidade.Text = "1";
        }
        #endregion

        #region Grid: configurar / carregar / finalizar
        private void ConfigurarGridRapidos()
        {
            dgvRapidos.SuspendLayout();
            dgvRapidos.AutoGenerateColumns = false;
            dgvRapidos.Columns.Clear();

            DataGridViewTextBoxColumn AddTextCol(string prop, string hdr, int minw, DataGridViewContentAlignment align)
            {
                var c = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = prop,
                    Name = prop,
                    HeaderText = hdr,
                    ReadOnly = true,
                    MinimumWidth = minw,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = align }
                };
                dgvRapidos.Columns.Add(c);
                return c;
            }

            var colId = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                Name = "Id",
                HeaderText = "ID",
                Visible = false,
            };
            dgvRapidos.Columns.Add(colId);

            var ColPoint = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Point",
                Name = "Point",
                HeaderText = "•",
                ReadOnly = true,
                Width = 30,  // largura fixa
                MinimumWidth = 30,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None, // não cresce
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };
            dgvRapidos.Columns.Add(ColPoint);

            AddTextCol("Professor", "Professor", 180, DataGridViewContentAlignment.MiddleLeft);
            AddTextCol("Livro", "Livro", 200, DataGridViewContentAlignment.MiddleLeft);
            AddTextCol("Turma", "Turma", 120, DataGridViewContentAlignment.MiddleLeft);
            AddTextCol("Quantidade", "Qtd", 60, DataGridViewContentAlignment.MiddleCenter);

            var colEmp = AddTextCol("DataHoraEmprestimo", "Emprestado em", 130, DataGridViewContentAlignment.MiddleLeft);
            colEmp.DefaultCellStyle.Format = "dd/MM HH:mm";
            var colDev = AddTextCol("DataHoraDevolucaoReal", "Devolução", 130, DataGridViewContentAlignment.MiddleLeft);
            colDev.DefaultCellStyle.Format = "dd/MM HH:mm";

            AddTextCol("Bibliotecaria", "Bibliotecaria", 120, DataGridViewContentAlignment.MiddleLeft);
            AddTextCol("Status", "Status", 100, DataGridViewContentAlignment.MiddleLeft);

            var btnFinalizar = new DataGridViewButtonColumn
            {
                Name = "Finalizar",
                HeaderText = "",
                Text = "",
                UseColumnTextForButtonValue = true,
                Width = 100,                   // largura fixa
                MinimumWidth = 70,            // impede encolher
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None, // impede crescer
                FlatStyle = FlatStyle.Flat
            };
            dgvRapidos.Columns.Add(btnFinalizar);

            // styling (copiado do UsuarioForm)
            dgvRapidos.BackgroundColor = Color.White;
            dgvRapidos.BorderStyle = BorderStyle.None;
            dgvRapidos.GridColor = Color.FromArgb(235, 239, 244);
            dgvRapidos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRapidos.RowHeadersVisible = false;
            dgvRapidos.ReadOnly = true;
            dgvRapidos.MultiSelect = false;
            dgvRapidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRapidos.AllowUserToAddRows = false;
            dgvRapidos.AllowUserToDeleteRows = false;
            dgvRapidos.AllowUserToResizeRows = false;


            dgvRapidos.DefaultCellStyle.BackColor = Color.White;
            dgvRapidos.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
            dgvRapidos.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            dgvRapidos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgvRapidos.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvRapidos.RowTemplate.Height = 40;
            dgvRapidos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

            dgvRapidos.EnableHeadersVisualStyles = false;
            dgvRapidos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvRapidos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
            dgvRapidos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRapidos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            dgvRapidos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvRapidos.ColumnHeadersHeight = 44;
            dgvRapidos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvRapidos.AllowUserToResizeColumns = false;
            dgvRapidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // double buffer
            typeof(DataGridView).InvokeMember(
       "DoubleBuffered",
       System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
       null,
       dgvRapidos,
       new object[] { true });

            dgvRapidos.ResumeLayout();
        }

        private void CarregarGridRapidos()
        {
            try
            {
                var tabela = new DataTable();
                tabela.Columns.Add("Id", typeof(int));
                tabela.Columns.Add("Professor", typeof(string));
                tabela.Columns.Add("Livro", typeof(string));
                tabela.Columns.Add("Turma", typeof(string));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("DataHoraEmprestimo", typeof(DateTime));
                tabela.Columns.Add("DataHoraDevolucaoReal", typeof(DateTime));

                tabela.Columns.Add("Bibliotecaria", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"
    SELECT r.Id, u.Nome as Professor, l.Nome as Livro, r.Turma, r.Quantidade,
           r.DataHoraEmprestimo, r.DataHoraDevolucaoReal, r.Bibliotecaria, r.Status, r.LivroId
    FROM EmprestimoRapido r
    INNER JOIN Usuarios u ON r.ProfessorId = u.Id
    INNER JOIN Livros l ON r.LivroId = l.Id
    ORDER BY r.Id DESC";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = tabela.NewRow();
                            row["Id"] = reader.GetInt32(0);
                            row["Professor"] = reader.GetString(1);
                            row["Livro"] = reader.GetString(2);
                            row["Turma"] = reader.GetString(3);
                            row["Quantidade"] = reader.GetInt32(4);
                            row["DataHoraEmprestimo"] = reader.GetDateTime(5);

                            if (!reader.IsDBNull(6))
                                row["DataHoraDevolucaoReal"] = reader.GetDateTime(6); // coluna do DataTable (ver nota abaixo)
                            else
                                row["DataHoraDevolucaoReal"] = DBNull.Value;

                            row["Bibliotecaria"] = reader.GetString(7);
                            row["Status"] = reader.GetString(8);
                            tabela.Rows.Add(row);
                        }
                    }

                }

                dgvRapidos.DataSource = tabela;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar empréstimos rápidos: " + ex.Message);
            }
        }

        private void dgvRapidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvRapidos.Columns[e.ColumnIndex].Name != "Finalizar") return;

            var row = dgvRapidos.Rows[e.RowIndex];
            var status = row.Cells["Status"].Value?.ToString();
            if (string.Equals(status, "Devolvido", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Empréstimo já finalizado.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = Convert.ToInt32(row.Cells["Id"].Value);
            var quantidade = Convert.ToInt32(row.Cells["Quantidade"].Value);
            var livroNome = row.Cells["Livro"].Value?.ToString();

            var confirm = MessageBox.Show($"Finalizar empréstimo rápido de \"{livroNome}\" (qt {quantidade})?", "Confirmar Devolução", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // pegar LivroId and atualizar quantidade
                    int livroId = -1;
                    using (var cmd = new SqlCeCommand("SELECT LivroId FROM EmprestimoRapido WHERE Id = @id", conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        var obj = cmd.ExecuteScalar();
                        if (obj == null)
                            throw new Exception("Registro não encontrado.");
                        livroId = Convert.ToInt32(obj);
                    }

                    // Atualiza registro EmprestimoRapido -> Status = Devolvido
                    using (var cmd = new SqlCeCommand("UPDATE EmprestimoRapido SET Status = 'Devolvido', DataHoraDevolucaoReal = @dataReal WHERE Id = @id", conexao))
                    {
                        cmd.Parameters.AddWithValue("@dataReal", DateTime.Now);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }

                    // Atualiza quantidade no Livros (repor)
                    using (var cmd = new SqlCeCommand("SELECT Quantidade FROM Livros WHERE Id = @id", conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", livroId);
                        var obj = cmd.ExecuteScalar();
                        int atual = obj != null ? Convert.ToInt32(obj) : 0;
                        int novo = atual + quantidade;

                        using (var cmd2 = new SqlCeCommand("UPDATE Livros SET Quantidade = @qt, Disponibilidade = @disp WHERE Id = @id", conexao))
                        {
                            cmd2.Parameters.AddWithValue("@qt", novo);
                            cmd2.Parameters.AddWithValue("@disp", novo > 0);
                            cmd2.Parameters.AddWithValue("@id", livroId);
                            cmd2.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Empréstimo finalizado com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarSugestoesECombo();
                CarregarGridRapidos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao finalizar empréstimo: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // CellPainting para desenhar botão arredondado (igual UsuarioForm)
        private void dgvRapidos_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRapidos.Columns[e.ColumnIndex].Name == "Finalizar")
            {
                e.PaintBackground(e.CellBounds, true);

                Color corFundo = Color.FromArgb(30, 61, 88);
                Color corTexto = Color.White;

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

                TextRenderer.DrawText(e.Graphics, "Finalizar",
                    new Font("Segoe UI Semibold", 9F),
                    rect,
                    corTexto,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
            }
        }

        private void dgvRapidos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvRapidos.Columns[e.ColumnIndex].Name != "Status") return;
            if (e.Value == null) return;

            var status = e.Value.ToString();
            if (status.Equals("Atrasado", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.ForeColor = Color.FromArgb(178, 34, 34);
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
            else if (status.Equals("Ativo", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.ForeColor = Color.FromArgb(34, 139, 34);
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
            else if (status.Equals("Devolvido", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.ForeColor = Color.Gray;
            }
        }

        #endregion

        #region estilizacao listbox
        private int hoveredIndex = -1;

        private void EstilizarListBoxSugestao(ListBox listBox)
        {
            listBox.DrawMode = DrawMode.OwnerDrawFixed;
            listBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            listBox.ItemHeight = 40;

            listBox.BackColor = Color.White;
            listBox.ForeColor = Color.FromArgb(30, 61, 88);
            listBox.BorderStyle = BorderStyle.FixedSingle;
            listBox.IntegralHeight = false;

            listBox.DrawItem -= ListBoxSugestao_DrawItem;
            listBox.DrawItem += ListBoxSugestao_DrawItem;

            listBox.MouseMove -= ListBoxSugestao_MouseMove;
            listBox.MouseMove += ListBoxSugestao_MouseMove;

            listBox.MouseLeave -= ListBoxSugestao_MouseLeave;
            listBox.MouseLeave += ListBoxSugestao_MouseLeave;
        }

        private void ListBoxSugestao_DrawItem(object sender, DrawItemEventArgs e)
        {
            var listBox = sender as ListBox;
            if (e.Index < 0) return;

            bool hovered = (e.Index == hoveredIndex);

            // Tons de cinza
            Color backColor = hovered
                ? Color.FromArgb(235, 235, 235) // cinza claro no hover
                : Color.White;                  // fundo branco

            Color textColor = Color.FromArgb(60, 60, 60); // cinza escuro

            using (SolidBrush b = new SolidBrush(backColor))
                e.Graphics.FillRectangle(b, e.Bounds);

            string text = listBox.Items[e.Index].ToString();
            Font font = listBox.Font;

            Rectangle textRect = new Rectangle(e.Bounds.Left + 12, e.Bounds.Top, e.Bounds.Width - 24, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, text, font, textRect, textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            // Linha divisória entre itens (cinza bem suave)
            if (e.Index < listBox.Items.Count - 1)
            {
                using (Pen p = new Pen(Color.FromArgb(220, 220, 220)))
                    e.Graphics.DrawLine(p, e.Bounds.Left + 8, e.Bounds.Bottom - 1, e.Bounds.Right - 8, e.Bounds.Bottom - 1);
            }
        }

        private void ListBoxSugestao_MouseMove(object sender, MouseEventArgs e)
        {
            var listBox = sender as ListBox;
            int index = listBox.IndexFromPoint(e.Location);
            if (index != hoveredIndex)
            {
                hoveredIndex = index;
                listBox.Invalidate();
            }
        }

        private void ListBoxSugestao_MouseLeave(object sender, EventArgs e)
        {
            hoveredIndex = -1;
            (sender as ListBox).Invalidate();
        }
        #endregion

        private void numQuantidade_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;// cancela caracteres não numéricos
        }

        private void numQuantidade_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(numQuantidade.Text, out int valor) || valor < 1)
            {

                numQuantidade.SelectionStart = numQuantidade.Text.Length; // cursor no final
            }
        }

        private void btnMais_Click(object sender, EventArgs e)
        {
            int valor = int.Parse(numQuantidade.Text);
            valor++;
            numQuantidade.Text = valor.ToString();

        }

        private void btnMenos_Click(object sender, EventArgs e)
        {
            int valor = int.Parse(numQuantidade.Text);
            if (valor > 1)
                valor--;
            numQuantidade.Text = valor.ToString();
        }
    }
}