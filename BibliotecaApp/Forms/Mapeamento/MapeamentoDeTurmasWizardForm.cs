using BibliotecaApp.Forms.Usuario;
using BibliotecaApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;



//Para Acessar o form de mapeamento altere o arquivo txt em AppData para um ano anterior ao ano atual
    

namespace BibliotecaApp.Forms.Usuario
{
    public partial class MapeamentoDeTurmasWizardForm : Form
    {
        #region Campos e Variáveis
        private readonly string _baseFolder;
        private MapeamentoAnualModel _model;
        private int _etapaAtual = 1;
        private Dictionary<string, string> _padroesTurma = new Dictionary<string, string>();
        private List<MapeamentoRegistro> _registrosOriginais = new List<MapeamentoRegistro>();

        private Dictionary<string, string[]> dicionarioTurmas = new Dictionary<string, string[]>
        {
            { "Ano", new[] { "6° Ano", "7° Ano", "8° Ano", "9° Ano" } },
            { "Desenvolvimento", new[] { "1° Desenvolvimento", "2° Desenvolvimento", "3° Desenvolvimento", "1° Desenvolvimento 2", "2° Desenvolvimento ", "2° Desenvolvimento 2", "3° Desenvolvimento" } },
            { "Agronegócio", new[] { "1° Agronegócio", "1° Agronegócio 2 ", "2° Agronegócio", "2° Agronegócio 2", "3° Agronegócio" } },
            { "Propedêutico", new[] { "1° Propedêutico", "1° Propedêutico 2", "2° Propedêutico", "2° Propedêutico 2", "3° Propedêutico" } }
        };
        #endregion

        #region Construtor e Inicialização
        public MapeamentoDeTurmasWizardForm()
        {
            InitializeComponent();
            _baseFolder = Path.Combine(Application.StartupPath, "AppData", "mapeamentoanual");
            Directory.CreateDirectory(_baseFolder);

            this.Load += MapeamentoDeTurmasWizardForm_Load;
            btnProximo.Click += BtnProximo_Click;
            btnAnterior.Click += BtnAnterior_Click;
            btnCancelar.Click += BtnCancelar_Click;
        }

        private void MapeamentoDeTurmasWizardForm_Load(object sender, EventArgs e)
        {
            try
            {
                int ano = DateTime.Now.Year;
                _model = LoadMapping(ano) ?? new MapeamentoAnualModel { Ano = ano, GeradoEm = DateTime.Now, Status = "pendente" };

                CarregarDadosIniciais();
                MostrarEtapa(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar o wizard: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Conexão com Banco
        public static class Conexao
        {
            public static string CaminhoBanco => Path.Combine(Application.StartupPath, "bibliotecaDB", "bibliotecaDB.sdf");
            public static string Conectar => $"Data Source={CaminhoBanco}; Password=123";

            public static SqlCeConnection ObterConexao()
            {
                if (!File.Exists(CaminhoBanco))
                    throw new FileNotFoundException("Arquivo .sdf não encontrado no caminho: " + CaminhoBanco);

                return new SqlCeConnection(Conectar);
            }
        }
        #endregion

        #region Carregamento de Dados
        private void CarregarDadosIniciais()
        {
            _registrosOriginais.Clear();
            _padroesTurma.Clear();

            using (var cn = Conexao.ObterConexao())
            {
                cn.Open();
                using (var cmd = new SqlCeCommand("SELECT Id, Nome, Turma FROM usuarios WHERE TipoUsuario LIKE '%Aluno%' ORDER BY Turma, Nome", cn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var turmaAtual = r["Turma"]?.ToString() ?? "";
                        var sugestao = SugerirProximaTurma(turmaAtual);

                        _registrosOriginais.Add(new MapeamentoRegistro
                        {
                            UsuarioId = Convert.ToInt32(r["Id"]),
                            Nome = r["Nome"]?.ToString() ?? "",
                            TurmaAtual = turmaAtual,
                            Sugestao = sugestao,
                            NovaTurma = sugestao,
                            Observacao = ""
                        });
                    }
                }
            }

            // Agrupar por turma para definir padrões
            var turmasDistintas = _registrosOriginais
                .Where(r => !string.IsNullOrWhiteSpace(r.TurmaAtual))
                .GroupBy(r => r.TurmaAtual)
                .Select(g => g.Key)
                .OrderBy(t => t)
                .ToList();

            foreach (var turma in turmasDistintas)
            {
                _padroesTurma[turma] = SugerirProximaTurma(turma);
            }
        }

        private string SugerirProximaTurma(string turmaAtual)
        {
            if (string.IsNullOrEmpty(turmaAtual))
                return "EGRESSO";

            var numeroTurma = ExtrairNumeroTurma(turmaAtual);
            var serieAtual = ExtrairNumeroSerie(turmaAtual);
            var cursoAtual = ExtrairCurso(turmaAtual);

            // REGRA: 9º ano -> sugerir 1° Propedêutico (sem número) como padrão
            if (cursoAtual == "Ano" && serieAtual == 9)
                return "1° Propedêutico";

            // Se estiver em 3° de curso técnico, sugere EGRESSO
            if ((cursoAtual == "Desenvolvimento" || cursoAtual == "Agronegócio" || cursoAtual == "Propedêutico") && serieAtual == 3)
                return "EGRESSO";

            // Ensino técnico: manter número da turma se existir nos registros, senão sugerir sem número
            if (cursoAtual == "Desenvolvimento" || cursoAtual == "Agronegócio" || cursoAtual == "Propedêutico")
            {
                if (serieAtual == 1)
                    return numeroTurma > 0 ? $"2° {cursoAtual} {numeroTurma}" : "2° " + cursoAtual;
                if (serieAtual == 2)
                    return numeroTurma > 0 ? $"3° {cursoAtual} {numeroTurma}" : "3° " + cursoAtual;
            }

            // Ensino fundamental: mantemos numeração da turma quando possível
            if (cursoAtual == "Ano")
            {
                if (serieAtual == 6) return numeroTurma > 0 ? $"7° Ano {numeroTurma}" : "7° Ano";
                if (serieAtual == 7) return numeroTurma > 0 ? $"8° Ano {numeroTurma}" : "8° Ano";
                if (serieAtual == 8) return numeroTurma > 0 ? $"9° Ano {numeroTurma}" : "9° Ano";
            }

            return "EGRESSO";
        }

        private int ExtrairNumeroTurma(string turma)
        {
            if (string.IsNullOrWhiteSpace(turma)) return 0;

            var partes = turma.Split(' ');
            if (partes.Length > 2 && int.TryParse(partes[partes.Length - 1], out int numero))
                return numero;

            return 0;
        }

        private string RemoverNumeroTurma(string turma)
        {
            if (string.IsNullOrEmpty(turma)) return turma;

            var numero = ExtrairNumeroTurma(turma);
            if (numero > 0)
                return turma.Substring(0, turma.LastIndexOf(" " + numero.ToString())).Trim();

            return turma;
        }
        #endregion

        #region Controle do Wizard
        private void MostrarEtapa(int etapa)
        {
            _etapaAtual = etapa;

            // Esconder todos os painéis
            panelEtapa1.Visible = false;
            panelEtapa2.Visible = false;
            panelEtapa3.Visible = false;

            // Mostrar painel da etapa atual
            switch (etapa)
            {
                case 1:
                    panelEtapa1.Visible = true;
                    lblTituloEtapa.Text = "ETAPA 1: DEFINIR PADRÕES POR TURMA";
                    btnAnterior.Enabled = false;
                    btnProximo.Text = "Avançar";
                    ConfigurarEtapa1();
                    break;
                case 2:
                    panelEtapa2.Visible = true;
                    lblTituloEtapa.Text = "ETAPA 2: AJUSTES INDIVIDUAIS";
                    btnAnterior.Enabled = true;
                    btnProximo.Text = "Avançar";
                    ConfigurarEtapa2();
                    break;
                case 3:
                    panelEtapa3.Visible = true;
                    lblTituloEtapa.Text = "ETAPA 3: CONFIRMAÇÃO E APLICAÇÃO";
                    btnAnterior.Enabled = true;
                    btnProximo.Text = "Aplicar";
                    ConfigurarEtapa3();
                    break;
            }

            AtualizarProgressoWizard();
        }

        private void AtualizarProgressoWizard()
        {
            progressBarWizard.Value = (_etapaAtual * 100) / 3;
            lblProgressoWizard.Text = $"Etapa {_etapaAtual} de 3";
        }

        private void BtnProximo_Click(object sender, EventArgs e)
        {
            switch (_etapaAtual)
            {
                case 1 when ValidarEtapa1():
                    AplicarPadroesDefinidos();
                    MostrarEtapa(2);
                    break;
                case 2 when ValidarEtapa2():
                    MostrarEtapa(3);
                    break;
                case 3:
                    AplicarMapeamento();
                    break;
            }
        }

        private void BtnAnterior_Click(object sender, EventArgs e)
        {
            if (_etapaAtual > 1)
                MostrarEtapa(_etapaAtual - 1);
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Deseja realmente cancelar o mapeamento? Todas as alterações serão perdidas.",
                                       "Confirmar Cancelamento", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
        #endregion

        #region Etapa 1: Definir Padrões
        private void ConfigurarEtapa1()
        {
            dgvPadroes.Rows.Clear();
            dgvPadroes.Columns.Clear();

            dgvPadroes.Columns.Add("TurmaAtual", "Turma Atual");
            dgvPadroes.Columns.Add("QtdAlunos", "Qtd Alunos");
            dgvPadroes.Columns.Add("SugestaoSistema", "Sugestão do Sistema");

            var colNovoPadrao = new DataGridViewComboBoxColumn
            {
                Name = "NovoPadrao",
                HeaderText = "Novo Padrão",
                FlatStyle = FlatStyle.Flat
            };
            dgvPadroes.Columns.Add(colNovoPadrao);

            dgvPadroes.CellValueChanged += DgvPadroes_CellValueChanged;
            dgvPadroes.CurrentCellDirtyStateChanged += DgvPadroes_CurrentCellDirtyStateChanged;
            dgvPadroes.DataError += DgvPadroes_DataError;

            // Preencher dados
            foreach (var kvp in _padroesTurma)
            {
                var qtdAlunos = _registrosOriginais.Count(r => r.TurmaAtual == kvp.Key);
                var rowIndex = dgvPadroes.Rows.Add(kvp.Key, qtdAlunos, kvp.Value);

                ConfigurarOpcoesValidasPorTurma(rowIndex, kvp.Key);
                dgvPadroes.Rows[rowIndex].Cells["NovoPadrao"].Value = kvp.Value;
            }

            EstilizarDataGrid(dgvPadroes);
            GarantirApenasComboEditavel(dgvPadroes, new[] { "NovoPadrao" }, alturaLinhaFixa: 35);
        }

        private void DgvPadroes_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception is ArgumentException && dgvPadroes.Columns[e.ColumnIndex].Name == "NovoPadrao")
            {
                var turmaAtual = dgvPadroes.Rows[e.RowIndex].Cells["TurmaAtual"].Value?.ToString();
                if (!string.IsNullOrEmpty(turmaAtual) && _padroesTurma.ContainsKey(turmaAtual))
                {
                    dgvPadroes.Rows[e.RowIndex].Cells["NovoPadrao"].Value = _padroesTurma[turmaAtual];
                }
                e.ThrowException = false;
            }
        }

        private void ConfigurarOpcoesValidasPorTurma(int rowIndex, string turmaAtual)
        {
            var cell = (DataGridViewComboBoxCell)dgvPadroes.Rows[rowIndex].Cells["NovoPadrao"];
            var opcoesValidas = ObterOpcoesValidasParaTurma(turmaAtual);

            cell.Items.Clear();

            // Usar HashSet para evitar duplicatas
            var itensUnicos = new HashSet<string>();
            foreach (var opcao in opcoesValidas)
            {
                if (itensUnicos.Add(opcao))
                    cell.Items.Add(opcao);
            }

            var valorAtual = _padroesTurma.ContainsKey(turmaAtual) ? _padroesTurma[turmaAtual] : "";

            // Verificar se o valor atual já existe antes de adicionar
            if (!string.IsNullOrEmpty(valorAtual) && itensUnicos.Add(valorAtual))
                cell.Items.Add(valorAtual);
        }

        private List<string> ObterOpcoesValidasParaTurma(string turmaAtual)
        {
            var opcoes = new List<string>();

            // Status especiais
            if (turmaAtual == "EGRESSO" || turmaAtual == "TRANSFERIDO" || turmaAtual == "DESISTENTE")
            {
                opcoes.Add(turmaAtual);
                return opcoes.Distinct().OrderBy(o => o).ToList();
            }

            // Coletar turmas reais cadastradas no sistema
            var turmasCadastradas = _registrosOriginais
                .Select(r => r.TurmaAtual)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .ToList();

            var numeroAtual = ExtrairNumeroSerie(turmaAtual);
            var cursoAtual = ExtrairCurso(turmaAtual);

            // Ensino Fundamental
            if (cursoAtual == "Ano")
            {
                if (numeroAtual >= 6 && numeroAtual <= 8)
                {
                    var proxPrefix = $"{numeroAtual + 1}° Ano";
                    var candidatos = turmasCadastradas
                        .Where(t => RemoverNumeroTurma(t).Equals(proxPrefix, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (candidatos.Any())
                        opcoes.AddRange(candidatos);
                    else
                        opcoes.Add(proxPrefix);
                }
                else if (numeroAtual == 9)
                {
                    var tecnicosPrefixes = new[] { "1° Propedêutico", "1° Desenvolvimento", "1° Agronegócio" };
                    foreach (var prefix in tecnicosPrefixes)
                    {
                        var encontrados = turmasCadastradas
                            .Where(t => RemoverNumeroTurma(t).Equals(prefix, StringComparison.OrdinalIgnoreCase))
                            .ToList();

                        if (encontrados.Any())
                            opcoes.AddRange(encontrados);
                        else
                            opcoes.Add(prefix);
                    }
                }
            }
            // Cursos Técnicos
            else if (cursoAtual == "Desenvolvimento" || cursoAtual == "Agronegócio" || cursoAtual == "Propedêutico")
            {
                if (numeroAtual >= 1 && numeroAtual <= 2)
                {
                    var proxPrefix = $"{numeroAtual + 1}° {cursoAtual}";
                    var candidatos = turmasCadastradas
                        .Where(t => RemoverNumeroTurma(t).Equals(proxPrefix, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (candidatos.Any())
                        opcoes.AddRange(candidatos);
                    else
                        opcoes.Add(proxPrefix);
                }
                else if (numeroAtual == 3)
                {
                    opcoes.Add("EGRESSO");
                }
            }

            // Fallback seguro
            if (opcoes.Count == 0)
                opcoes.Add("EGRESSO");

            return opcoes.Distinct().OrderBy(o => o).ToList();
        }

        private void DgvPadroes_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPadroes.IsCurrentCellDirty)
                dgvPadroes.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DgvPadroes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvPadroes.Columns[e.ColumnIndex].Name == "NovoPadrao")
            {
                var turmaAtual = dgvPadroes.Rows[e.RowIndex].Cells["TurmaAtual"].Value?.ToString();
                var novaEscolha = dgvPadroes.Rows[e.RowIndex].Cells["NovoPadrao"].Value?.ToString();

                if (!string.IsNullOrEmpty(turmaAtual) && !string.IsNullOrEmpty(novaEscolha) &&
                    IsEscolhaInvalidaParaPadrao(turmaAtual, novaEscolha))
                {
                    MessageBox.Show($"Escolha inválida: {turmaAtual} não pode ir para {novaEscolha}",
                                  "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvPadroes.Rows[e.RowIndex].Cells["NovoPadrao"].Value = _padroesTurma[turmaAtual];
                }
            }
        }

        private bool IsEscolhaInvalidaParaPadrao(string turmaAtual, string novaEscolha)
        {
            if ((turmaAtual == "EGRESSO" || turmaAtual == "TRANSFERIDO" || turmaAtual == "DESISTENTE") &&
                novaEscolha != turmaAtual)
                return true;

            if ((novaEscolha == "DESISTENTE" || novaEscolha == "TRANSFERIDO") &&
                !(turmaAtual == "DESISTENTE" || turmaAtual == "TRANSFERIDO"))
                return true;

            return IsProgressaoInvalida(turmaAtual, novaEscolha);
        }
        #endregion

        #region Etapa 2: Ajustes Individuais
        private void ConfigurarEtapa2()
        {
            cmbFiltroTurmaEtapa2.Items.Clear();
            cmbFiltroTurmaEtapa2.Items.Add("(Todas as Turmas)");

            var turmasComAlunos = _registrosOriginais
                .Where(r => !string.IsNullOrWhiteSpace(r.TurmaAtual))
                .GroupBy(r => r.TurmaAtual)
                .Select(g => g.Key)
                .OrderBy(t => t);

            foreach (var turma in turmasComAlunos)
                cmbFiltroTurmaEtapa2.Items.Add(turma);

            cmbFiltroTurmaEtapa2.SelectedIndex = 0;
            cmbFiltroTurmaEtapa2.SelectedIndexChanged += CmbFiltroTurmaEtapa2_SelectedIndexChanged;

            ConfigurarDataGridEtapa2();
            AtualizarGridEtapa2();
            GarantirApenasComboEditavel(dgvAjustesIndividuais, new[] { "NovaEscolha", "Observacao" }, alturaLinhaFixa: 35);
        }

        private void ConfigurarDataGridEtapa2()
        {
            dgvAjustesIndividuais.Columns.Clear();

            dgvAjustesIndividuais.Columns.Add("Nome", "Nome do Aluno");
            dgvAjustesIndividuais.Columns.Add("TurmaAtual", "Turma Atual");
            dgvAjustesIndividuais.Columns.Add("PadraoDefinido", "Padrão Definido");

            var colNovaEscolha = new DataGridViewComboBoxColumn
            {
                Name = "NovaEscolha",
                HeaderText = "Nova Escolha",
                FlatStyle = FlatStyle.Flat
            };

            dgvAjustesIndividuais.Columns.Add(colNovaEscolha);
            dgvAjustesIndividuais.Columns.Add("Observacao", "Observação");

            dgvAjustesIndividuais.CellValueChanged += DgvAjustesIndividuais_CellValueChanged;
            dgvAjustesIndividuais.CurrentCellDirtyStateChanged += DgvAjustesIndividuais_CurrentCellDirtyStateChanged;
            dgvAjustesIndividuais.DataError += DgvAjustesIndividuais_DataError;

            EstilizarDataGrid(dgvAjustesIndividuais);
        }

        private void DgvAjustesIndividuais_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception is ArgumentException && dgvAjustesIndividuais.Columns[e.ColumnIndex].Name == "NovaEscolha")
            {
                var padraoDefinido = dgvAjustesIndividuais.Rows[e.RowIndex].Cells["PadraoDefinido"].Value?.ToString();
                if (!string.IsNullOrEmpty(padraoDefinido))
                {
                    dgvAjustesIndividuais.Rows[e.RowIndex].Cells["NovaEscolha"].Value = padraoDefinido;
                }
                e.ThrowException = false;
            }
        }

        private void DgvAjustesIndividuais_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvAjustesIndividuais.IsCurrentCellDirty)
                dgvAjustesIndividuais.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DgvAjustesIndividuais_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvAjustesIndividuais.Columns[e.ColumnIndex].Name == "NovaEscolha")
            {
                var turmaAtual = dgvAjustesIndividuais.Rows[e.RowIndex].Cells["TurmaAtual"].Value?.ToString();
                var novaEscolha = dgvAjustesIndividuais.Rows[e.RowIndex].Cells["NovaEscolha"].Value?.ToString();
                var nome = dgvAjustesIndividuais.Rows[e.RowIndex].Cells["Nome"].Value?.ToString();

                if (!string.IsNullOrEmpty(turmaAtual) && !string.IsNullOrEmpty(novaEscolha) &&
                    IsProgressaoInvalida(turmaAtual, novaEscolha))
                {
                    MessageBox.Show($"Progressão inválida detectada para {nome}: {turmaAtual} → {novaEscolha}",
                                  "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvAjustesIndividuais.Rows[e.RowIndex].Cells["NovaEscolha"].Value =
                        dgvAjustesIndividuais.Rows[e.RowIndex].Cells["PadraoDefinido"].Value;
                }
            }
        }

        private void AtualizarGridEtapa2()
        {
            dgvAjustesIndividuais.Rows.Clear();

            var filtro = cmbFiltroTurmaEtapa2.SelectedItem?.ToString();
            var registrosFiltrados = _registrosOriginais.AsEnumerable();

            if (!string.IsNullOrEmpty(filtro) && filtro != "(Todas as Turmas)")
                registrosFiltrados = registrosFiltrados.Where(r => r.TurmaAtual == filtro);

            foreach (var registro in registrosFiltrados.OrderBy(r => r.Nome))
            {
                var rowIndex = dgvAjustesIndividuais.Rows.Add(
                    registro.Nome,
                    registro.TurmaAtual,
                    registro.NovaTurma,
                    registro.NovaTurma,
                    registro.Observacao
                );

                ConfigurarOpcoesValidasParaAluno(rowIndex, registro.TurmaAtual);
            }
        }

        private void ConfigurarOpcoesValidasParaAluno(int rowIndex, string turmaAtual)
        {
            var cell = (DataGridViewComboBoxCell)dgvAjustesIndividuais.Rows[rowIndex].Cells["NovaEscolha"];

            try
            {
                // ✅ 1. Turmas do BANCO (prioridade máxima)
                var turmasBanco = _registrosOriginais
                    .Select(r => r.TurmaAtual)
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct()
                    .ToList();

                // ✅ 2. Turmas do DICIONÁRIO (apenas as que não existem no banco)
                var turmasDic = dicionarioTurmas.Values
                    .SelectMany(x => x)
                    .Where(td => !turmasBanco.Any(tb =>
                        string.Equals(tb, td, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                // ✅ 3. Status especiais (sempre incluir)
                var statusEspeciais = new[] { "EGRESSO", "DESISTENTE", "TRANSFERIDO" };

                // ✅ 4. Combinar TUDO sem duplicatas
                var todasOpcoes = turmasBanco
                    .Concat(turmasDic)
                    .Concat(statusEspeciais)
                    .Distinct()
                    .ToList();

                // ✅ 5. Aplicar regras de filtragem
                var opcoesValidas = FiltrarOpcoesValidasParaAluno(turmaAtual, todasOpcoes);

                // ✅ 6. Remover duplicata com "Padrão Definido"
                var padraoDefinido = dgvAjustesIndividuais.Rows[rowIndex].Cells["PadraoDefinido"].Value?.ToString();
                if (!string.IsNullOrEmpty(padraoDefinido))
                {
                    opcoesValidas.RemoveAll(opcao => opcao == padraoDefinido);
                }

                // ✅ 7. Preencher o ComboBox
                cell.Items.Clear();
                foreach (var opcao in opcoesValidas.OrderBy(o => o))
                {
                    cell.Items.Add(opcao);
                }

                // ✅ 8. Garantir valor atual
                var valorAtual = dgvAjustesIndividuais.Rows[rowIndex].Cells["NovaEscolha"].Value?.ToString();
                if (!string.IsNullOrEmpty(valorAtual) && !cell.Items.Contains(valorAtual))
                {
                    cell.Items.Add(valorAtual);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao configurar opções: {ex.Message}");
                cell.Items.Clear();
            }
        }


        private List<string> FiltrarOpcoesValidasParaAluno(string turmaAtual, List<string> todasOpcoes)
        {
            var opcoesValidas = new HashSet<string>();

            if (string.IsNullOrWhiteSpace(turmaAtual))
                return todasOpcoes.Distinct().ToList();

            // Regra específica: SE já for EGRESSO => só pode permanecer EGRESSO
            if (turmaAtual == "EGRESSO")
                return new List<string> { "EGRESSO" };

            // Se a origem já é TRANSFERIDO ou DESISTENTE, permitir apenas status especiais
            if (turmaAtual == "TRANSFERIDO" || turmaAtual == "DESISTENTE")
                return new List<string> { "EGRESSO", "DESISTENTE", "TRANSFERIDO" };

            var opcoesDistintas = todasOpcoes.Distinct();
            var serieAtual = ExtrairNumeroSerie(turmaAtual);
            var cursoAtual = ExtrairCurso(turmaAtual);
            var cursosTecnicos = new[] { "Propedêutico", "Desenvolvimento", "Agronegócio" };

            foreach (var opcao in opcoesDistintas)
            {
                // Não permitir a própria turma atual como opção
                if (string.Equals(opcao, turmaAtual, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (string.IsNullOrWhiteSpace(opcao))
                    continue;

                var cursoOpcao = ExtrairCurso(opcao);
                var serieOpcao = ExtrairNumeroSerie(opcao);

                // MANTER A REGRA ORIGINAL de remover turmas terminadas em "1" para cursos técnicos
                if (cursosTecnicos.Contains(cursoOpcao) && opcao.Trim().EndsWith(" 1"))
                {
                    // Verificar se existe uma versão sem o "1" antes de remover
                    var opcaoSemNumero = opcao.Substring(0, opcao.LastIndexOf(" 1")).Trim();
                    if (todasOpcoes.Contains(opcaoSemNumero))
                    {
                        // Se existir a versão sem número, pule a versão com "1"
                        continue;
                    }
                }

                // sempre permitir DESISTENTE e TRANSFERIDO
                if (opcao == "DESISTENTE" || opcao == "TRANSFERIDO")
                {
                    opcoesValidas.Add(opcao);
                    continue;
                }

                // permitir EGRESSO apenas quando aplicável
                if (opcao == "EGRESSO")
                {
                    if (cursosTecnicos.Contains(cursoAtual) && serieAtual == 3)
                        opcoesValidas.Add(opcao);

                    continue;
                }

                // caso especial: 9° Ano -> qualquer 1° técnico permitido
                if (cursoAtual == "Ano" && serieAtual == 9)
                {
                    if (serieOpcao == 1 && (cursoOpcao == "Propedêutico" || cursoOpcao == "Desenvolvimento" || cursoOpcao == "Agronegócio"))
                        opcoesValidas.Add(opcao);

                    continue;
                }

                // Mesmo curso:
                if (cursoAtual == cursoOpcao)
                {
                    if (serieAtual > 0 && serieOpcao > 0)
                    {
                        if (serieOpcao < serieAtual) continue;
                        if ((serieOpcao - serieAtual) > 1) continue;
                        opcoesValidas.Add(opcao);
                    }
                    else
                    {
                        opcoesValidas.Add(opcao);
                    }
                    continue;
                }

                // Cursos técnicos entre si
                if (cursosTecnicos.Contains(cursoAtual) && cursosTecnicos.Contains(cursoOpcao))
                {
                    if (serieOpcao >= serieAtual && (serieOpcao - serieAtual) <= 1)
                        opcoesValidas.Add(opcao);

                    continue;
                }

                // movimento entre anos do Fundamental
                if (cursoAtual == "Ano" && cursoOpcao == "Ano")
                {
                    if (serieOpcao == serieAtual + 1)
                        opcoesValidas.Add(opcao);

                    continue;
                }
            }

            return opcoesValidas.OrderBy(o => o).ToList();
        }

        private bool IsProgressaoInvalida(string turmaAtual, string novaTurma)
        {
            if (string.IsNullOrWhiteSpace(turmaAtual) || string.IsNullOrWhiteSpace(novaTurma))
                return true;

            // regra fundamental: se origem é EGRESSO, só pode permanecer EGRESSO
            if (turmaAtual == "EGRESSO")
                return novaTurma != "EGRESSO";

            // se origem é TRANSFERIDO ou DESISTENTE: só aceitar status especiais
            if (turmaAtual == "TRANSFERIDO" || turmaAtual == "DESISTENTE")
                return !(novaTurma == "EGRESSO" || novaTurma == "TRANSFERIDO" || novaTurma == "DESISTENTE");

            // permitir DESISTENTE e TRANSFERIDO sempre (ajuste individual)
            if (novaTurma == "DESISTENTE" || novaTurma == "TRANSFERIDO")
                return false;

            var serieAtual = ExtrairNumeroSerie(turmaAtual);
            var serieNova = ExtrairNumeroSerie(novaTurma);
            var cursoAtual = ExtrairCurso(turmaAtual);
            var cursoNovo = ExtrairCurso(novaTurma);

            // permitir EGRESSO apenas para alunos que estão no 3° ano de curso técnico
            if (novaTurma == "EGRESSO")
            {
                if ((cursoAtual == "Desenvolvimento" || cursoAtual == "Agronegócio" || cursoAtual == "Propedêutico") && serieAtual == 3)
                    return false;

                return true;
            }

            // 9° Ano -> 1° de técnico é permitido
            if (cursoAtual == "Ano" && serieAtual == 9 && serieNova == 1 &&
                (cursoNovo == "Propedêutico" || cursoNovo == "Desenvolvimento" || cursoNovo == "Agronegócio"))
                return false;

            // mesmo curso e séries detectadas
            if (cursoAtual == cursoNovo && serieAtual > 0 && serieNova > 0)
            {
                if (serieNova < serieAtual) return true;
                if ((serieNova - serieAtual) > 1) return true;
                return false;
            }

            // mudança entre cursos técnicos: só se sérieNova >= serieAtual e pulo <= 1
            var tecnicos = new[] { "Propedêutico", "Desenvolvimento", "Agronegócio" };
            if (tecnicos.Contains(cursoAtual) && tecnicos.Contains(cursoNovo))
            {
                if (serieNova >= serieAtual && (serieNova - serieAtual) <= 1)
                    return false;

                return true;
            }

            return true;
        }
        #endregion

        #region Etapa 3: Confirmação
        private void ConfigurarEtapa3()
        {
            var alteracoes = _registrosOriginais.Where(r => r.TurmaAtual != r.NovaTurma).ToList();
            var egressos = _registrosOriginais.Where(r => r.NovaTurma == "EGRESSO" || r.NovaTurma == "TRANSFERIDO").ToList();

            txtResumoFinal.Text = $"RESUMO DO MAPEAMENTO:\n\n";
            txtResumoFinal.Text += $"• Total de alunos: {_registrosOriginais.Count}\n";
            txtResumoFinal.Text += $"• Alterações de turma: {alteracoes.Count}\n";
            txtResumoFinal.Text += $"• Egressos/Transferidos: {egressos.Count}\n\n";

            txtResumoFinal.Text += "ALTERAÇÕES DETALHADAS:\n";
            foreach (var alt in alteracoes.Take(20))
                txtResumoFinal.Text += $"• {alt.Nome}: {alt.TurmaAtual} → {alt.NovaTurma}\n";

            if (alteracoes.Count > 20)
                txtResumoFinal.Text += $"... e mais {alteracoes.Count - 20} alterações.\n";
        }

        private void AplicarMapeamento()
        {
            var result = MessageBox.Show("Confirma a aplicação do mapeamento ao banco de dados? Esta ação não pode ser desfeita.",
                                       "Confirmar Aplicação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            if (!SolicitarConfirmacaoSenha())
                return;

            try
            {
                _model.Registros = _registrosOriginais;
                _model.Status = "pendente";
                SaveMapping(_model);

                using (var cn = Conexao.ObterConexao())
                {
                    cn.Open();
                    using (var tx = cn.BeginTransaction())
                    {
                        foreach (var registro in _registrosOriginais.Where(r => r.TurmaAtual != r.NovaTurma))
                        {
                            using (var cmd = new SqlCeCommand("UPDATE usuarios SET Turma = @turma WHERE Id = @id", cn, tx))
                            {
                                cmd.Parameters.AddWithValue("@turma", registro.NovaTurma ?? "");
                                cmd.Parameters.AddWithValue("@id", registro.UsuarioId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tx.Commit();
                    }
                }

                _model.Status = "concluido";
                SaveMapping(_model);

                MessageBox.Show("Mapeamento aplicado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar mapeamento: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool SolicitarConfirmacaoSenha()
        {
            for (int tentativas = 0; tentativas < 3; tentativas++)
            {
                string primeiraSenha, segundaSenha;

                // Primeira entrada de senha
                using (var pf = new PasswordForm())
                {
                    pf.Titulo = "Confirmação de Aplicação";
                    pf.Mensagem = $"Digite sua senha (tentativa {tentativas + 1}/3):";
                    if (pf.ShowDialog(this) != DialogResult.OK)
                        return false;

                    primeiraSenha = pf.SenhaDigitada;
                }

                // Segunda entrada de senha para confirmação
                using (var pf = new PasswordForm())
                {
                    pf.Titulo = "Confirmação de Aplicação";
                    pf.Mensagem = $"Confirme sua senha (tentativa {tentativas + 1}/3):";
                    if (pf.ShowDialog(this) != DialogResult.OK)
                        return false;

                    segundaSenha = pf.SenhaDigitada;
                }

                // Verificar se as senhas coincidem
                if (primeiraSenha != segundaSenha)
                {
                    MessageBox.Show("As senhas não coincidem. Tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                // Verificar senha no banco de dados
                if (VerificarSenha(primeiraSenha))
                    return true;

                MessageBox.Show("Senha incorreta.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            MessageBox.Show("Falha na autenticação. Operação cancelada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private bool VerificarSenha(string senha)
        {
            var nome = Sessao.NomeBibliotecariaLogada;
            if (string.IsNullOrWhiteSpace(nome))
                return false;

            try
            {
                using (var cn = Conexao.ObterConexao())
                {
                    cn.Open();
                    using (var cmd = new SqlCeCommand("SELECT Senha_hash, Senha_salt FROM usuarios WHERE Nome = @nome AND TipoUsuario LIKE '%Bibliotec%'", cn))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                var hash = r["Senha_hash"]?.ToString() ?? "";
                                var salt = r["Senha_salt"]?.ToString() ?? "";
                                return BibliotecaApp.Utils.CriptografiaSenha.VerificarSenha(senha, hash, salt);
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
        #endregion

        #region Arquivo de Mapeamento
        private string MappingFilePath(int ano) => Path.Combine(_baseFolder, $"mapeamento_{ano}.txt");

        private MapeamentoAnualModel LoadMapping(int ano)
        {
            var path = MappingFilePath(ano);
            if (!File.Exists(path))
                return null;

            try
            {
                var content = File.ReadAllText(path);
                var lines = content.Split('\n');
                var model = new MapeamentoAnualModel();

                foreach (var line in lines)
                {
                    if (line.StartsWith("ANO="))
                        model.Ano = int.Parse(line.Substring(4));
                    else if (line.StartsWith("STATUS="))
                        model.Status = line.Substring(7);
                    else if (line.StartsWith("GERADO_EM="))
                        model.GeradoEm = DateTime.Parse(line.Substring(10));
                }

                return model;
            }
            catch
            {
                return null;
            }
        }

        private bool SaveMapping(MapeamentoAnualModel model)
        {
            var path = MappingFilePath(model.Ano);

            try
            {
                var content = $"ANO={model.Ano}\n";
                content += $"STATUS={model.Status}\n";
                content += $"GERADO_EM={model.GeradoEm:O}\n";
                content += "REGISTROS_START\n";

                foreach (var reg in model.Registros)
                    content += $"{reg.UsuarioId}|{reg.Nome}|{reg.TurmaAtual}|{reg.NovaTurma}|{reg.Observacao}\n";

                content += "REGISTROS_END\n";

                File.WriteAllText(path, content);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Utilitários e Métodos Auxiliares
        private void EstilizarDataGrid(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.RowTemplate.Height = 35;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;

            dgv.GridColor = Color.FromArgb(235, 239, 244);
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
        }

        private bool ValidarEtapa1()
        {
            foreach (DataGridViewRow row in dgvPadroes.Rows)
            {
                if (row.Cells["NovoPadrao"].Value == null || string.IsNullOrWhiteSpace(row.Cells["NovoPadrao"].Value.ToString()))
                {
                    MessageBox.Show("Todos os padrões devem ser definidos antes de prosseguir.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        private bool ValidarEtapa2()
        {
            foreach (DataGridViewRow row in dgvAjustesIndividuais.Rows)
            {
                var turmaAtual = row.Cells["TurmaAtual"].Value?.ToString();
                var novaEscolha = row.Cells["NovaEscolha"].Value?.ToString();

                if (!string.IsNullOrEmpty(turmaAtual) && !string.IsNullOrEmpty(novaEscolha) &&
                    IsProgressaoInvalida(turmaAtual, novaEscolha))
                {
                    var nome = row.Cells["Nome"].Value?.ToString();
                    MessageBox.Show($"Progressão inválida detectada para {nome}: {turmaAtual} → {novaEscolha}",
                                  "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        private void AplicarPadroesDefinidos()
        {
            foreach (DataGridViewRow row in dgvPadroes.Rows)
            {
                var turmaAtual = row.Cells["TurmaAtual"].Value?.ToString();
                var novoPadrao = row.Cells["NovoPadrao"].Value?.ToString();

                if (!string.IsNullOrEmpty(turmaAtual) && !string.IsNullOrEmpty(novoPadrao))
                {
                    foreach (var registro in _registrosOriginais.Where(r => r.TurmaAtual == turmaAtual))
                        registro.NovaTurma = novoPadrao;

                    _padroesTurma[turmaAtual] = novoPadrao;
                }
            }
        }

        private int ExtrairNumeroSerie(string turma)
        {
            if (string.IsNullOrWhiteSpace(turma))
                return 0;

            var match = Regex.Match(turma, @"(\d+)°");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int numero))
                return numero;

            return 0;
        }

        private string ExtrairCurso(string turma)
        {
            if (string.IsNullOrWhiteSpace(turma))
                return "";

            turma = turma.ToLower();

            if (turma.Contains("desenvolvimento")) return "Desenvolvimento";
            if (turma.Contains("agronegócio")) return "Agronegócio";
            if (turma.Contains("propedêutico")) return "Propedêutico";
            if (turma.Contains("ano")) return "Ano";

            return "";
        }

        private void CmbFiltroTurmaEtapa2_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarGridEtapa2();
            GarantirApenasComboEditavel(dgvAjustesIndividuais, new[] { "NovaEscolha", "Observacao" }, alturaLinhaFixa: 35);
        }

        private void GarantirApenasComboEditavel(DataGridView dgv, string[] colunasComboEditaveis, int alturaLinhaFixa = 35)
        {
            if (dgv == null) return;

            // Configurações básicas
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            var setEditaveis = new HashSet<string>(colunasComboEditaveis ?? new string[0], StringComparer.OrdinalIgnoreCase);

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.Resizable = DataGridViewTriState.False;
                col.ReadOnly = !setEditaveis.Contains(col.Name);
            }

            dgv.ReadOnly = false;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            if (alturaLinhaFixa > 0)
                dgv.RowTemplate.Height = alturaLinhaFixa;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Resizable = DataGridViewTriState.False;
                row.Height = alturaLinhaFixa;
            }

            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv.CellBeginEdit -= Dgv_CellBeginEdit_BlockNonCombo;
            dgv.CellBeginEdit += Dgv_CellBeginEdit_BlockNonCombo;

            void Dgv_CellBeginEdit_BlockNonCombo(object sender, DataGridViewCellCancelEventArgs ev)
            {
                try
                {
                    var dv = sender as DataGridView;
                    if (dv == null) return;
                    var colName = dv.Columns[ev.ColumnIndex].Name;
                    if (!setEditaveis.Contains(colName))
                        ev.Cancel = true;
                }
                catch
                {
                    // Não propagar erro
                }
            }
        }
        #endregion
    }
}