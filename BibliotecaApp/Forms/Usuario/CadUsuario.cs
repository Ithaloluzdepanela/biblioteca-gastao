using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BibliotecaApp.Froms.Usuario
{
    public partial class CadUsuario : Form
    {
        #region Construtores e Variáveis
        private List<string> turmasCadastradas = new List<string>();

        // Dicionário de turmas padrão
        private Dictionary<string, string[]> dicionarioTurmas = new Dictionary<string, string[]>
        {
            { "Ano", new[] { "6° Ano", "7° Ano", "8° Ano", "9° Ano" } },
            { "Desenvolvimento", new[] { "1° Desenvolvimento", "2° Desenvolvimento", "3° Desenvolvimento" } },
            { "Agronegócio", new[] { "1° Agronegócio", "2° Agronegócio", "3° Agronegócio" } },
            { "Propedêutico", new[] { "1° Propedêutico", "2° Propedêutico", "3° Propedêutico" } }
        };

        private List<string> todasTurmasPadrao;

        public CadUsuario(List<Usuarios> usuarios)
        {
            InitializeComponent();
        }

        public CadUsuario()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos do Formulário
        private void CadUsuario_Load(object sender, EventArgs e)
        {
            // Configuração inicial do formulário
            dtpDataNasc.Value = DateTime.Today;
            HabilitarCampos(false);
            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;
            chkMostrarSenha.ForeColor = Color.LightGray;
            SetAsteriscoVisibility(false);
            CarregarTurmasDoBanco();

            // Inicializar lista de todas as turmas padrão
            InicializarTurmasPadrao();

            // Eventos para o autocomplete de Turma
            txtTurma.KeyDown += txtTurma_KeyDown;
            txtTurma.Leave += txtTurma_Leave;

            lstSugestoesTurma.Click += lstSugestoesTurma_Click;
            lstSugestoesTurma.KeyDown += lstSugestoesTurma_KeyDown;
            lstSugestoesTurma.Leave += lstSugestoesTurma_Leave;

            // Estilo do ListBox e z-order
            EstilizarListBoxSugestao(lstSugestoesTurma);
            lstSugestoesTurma.BringToFront();
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            // Navegação entre campos com Enter
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }
        #endregion

        #region Classe Conexao
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

        #region Métodos Públicos
        public void LimparCampos()
        {
            cbUsuario.SelectedIndex = -1;
            txtNome.Text = "";
            txtEmail.Text = "";
            txtTurma.Text = "";
            mtxTelefone.Text = "";
            mtxCPF.Text = "";
            dtpDataNasc.Value = DateTime.Today;
            txtSenha.Text = "";
            txtConfirmSenha.Text = "";
            cbUsuario.Focus();
        }

        public bool ValidarCPF(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        public bool ValidarTelefone(string telefone)
        {
            string apenasNumeros = new string(telefone.Where(char.IsDigit).ToArray());
            return apenasNumeros.Length >= 11 && apenasNumeros.Length <= 12;
        }

        public bool ValidarEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
        #endregion

        #region Eventos de Controles
        private void cbUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            string funcaoSelecionada = cbUsuario.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(funcaoSelecionada))
            {
                ConfigurarCamposGenericos();
                return;
            }

            switch (funcaoSelecionada)
            {
                case "Bibliotecário(a)":
                    ConfigurarParaBibliotecario();
                    break;

                case "Professor(a)":
                    ConfigurarParaProfessor();
                    break;

                case "Outros":
                    ConfigurarParaOutros();
                    break;

                default:
                    ConfigurarParaAluno();
                    break;
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposObrigatorios())
                return;

            if (!ValidarDadosUsuario())
                return;

            CadastrarNovoUsuario();
        }

        private void chkMostrarSenha_CheckedChanged(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
            txtConfirmSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            var confirmar = MessageBox.Show(
                "Tem certeza de que deseja limpar tudo?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmar == DialogResult.Yes)
            {
                LimparCampos();
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            lblAvisoEmail.Visible = string.IsNullOrWhiteSpace(txtEmail.Text);
        }
        #endregion

        #region Métodos Privados
        private void HabilitarCampos(bool ativo)
        {
            txtNome.Enabled = ativo;
            txtEmail.Enabled = ativo;
            txtTurma.Enabled = ativo;
            mtxCPF.Enabled = ativo;
            mtxTelefone.Enabled = ativo;
            txtSenha.Enabled = ativo;
            txtConfirmSenha.Enabled = ativo;
            dtpDataNasc.Enabled = ativo;
        }

        private string RemoverMascara(string texto)
        {
            return string.Concat(texto.Where(c => char.IsDigit(c)));
        }
        #endregion

        #region Métodos de Configuração de Interface
        private void ConfigurarCamposGenericos()
        {
            HabilitarCampos(true);
            SetLabelColors(enabled: true);
            SetAsteriscoVisibility(false);
            chkMostrarSenha.ForeColor = Color.LightGray;
        }

        private void ConfigurarParaBibliotecario()
        {
            HabilitarCampos(true);
            txtTurma.Enabled = false;

            txtSenha.Visible = true;
            txtConfirmSenha.Visible = true;
            lblSenha.Visible = true;
            lblConfirmSenha.Visible = true;
            chkMostrarSenha.Visible = true;
            btnCadastrar.Location = new Point(541, 932);
            btnLimpar.Location = new Point(73, 932);
            CentralizarBotoes();

            SetLabelColors(enabled: true);
            lblTurma.ForeColor = Color.LightGray;

            SetAsteriscoVisibility(true);
            TurmaAst.ForeColor = Color.Transparent;

            txtTurma.Text = "";

            ConfigurarApparence(
                txtTurmaEnabled: false,
                txtEmailEnabled: true,
                txtSenhaEnabled: true
            );

            txtSenha.PlaceholderText = "Digite aqui uma senha...";
            txtConfirmSenha.PlaceholderText = "Confirme a senha...";
            txtEmail.PlaceholderText = "Digite aqui o email...";

            chkMostrarSenha.Enabled = true;
            chkMostrarSenha.ForeColor = Color.FromArgb(20, 41, 60);
        }

        private void ConfigurarParaProfessor()
        {
            HabilitarCampos(true);
            txtTurma.Enabled = false;

            txtSenha.Visible = false;
            txtConfirmSenha.Visible = false;
            lblSenha.Visible = false;
            lblConfirmSenha.Visible = false;
            chkMostrarSenha.Visible = false;
            btnCadastrar.Location = new Point(541, 788);
            btnLimpar.Location = new Point(73, 788);
            CentralizarBotoes();

            SetLabelColors(enabled: true);

            lblTurma.ForeColor = Color.LightGray;

            SetAsteriscoVisibility(true);
            TurmaAst.ForeColor = Color.Transparent;
            SenhaAst.ForeColor = Color.Transparent;
            ConfirmSenhaAst.ForeColor = Color.Transparent;
            EmailAst.ForeColor = Color.Transparent;
            txtTurma.Text = "";
            txtSenha.Text = "";
            txtConfirmSenha.Text = "";

            ConfigurarApparence(
                txtTurmaEnabled: false,
                txtEmailEnabled: true,
                txtSenhaEnabled: false
            );

            txtEmail.PlaceholderText = "Digite aqui o email...";
            chkMostrarSenha.ForeColor = Color.LightGray;
            chkMostrarSenha.Enabled = false;
        }

        private void ConfigurarParaOutros()
        {
            HabilitarCampos(true);
            txtEmail.Enabled = false;
            txtTurma.Enabled = false;
            txtSenha.Enabled = false;
            txtConfirmSenha.Enabled = false;

            txtSenha.Visible = false;
            txtConfirmSenha.Visible = false;
            lblSenha.Visible = false;
            lblConfirmSenha.Visible = false;
            chkMostrarSenha.Visible = false;
            btnCadastrar.Location = new Point(541, 788);
            btnLimpar.Location = new Point(73, 788);
            CentralizarBotoes();

            SetLabelColors(enabled: true);
            lblSenha.ForeColor = Color.LightGray;
            lblConfirmSenha.ForeColor = Color.LightGray;
            lblEmail.ForeColor = Color.LightGray;
            lblTurma.ForeColor = Color.LightGray;
            txtTurma.Text = "";
            txtSenha.Text = "";
            txtConfirmSenha.Text = "";

            SetAsteriscoVisibility(true);
            TurmaAst.ForeColor = Color.Transparent;
            SenhaAst.ForeColor = Color.Transparent;
            ConfirmSenhaAst.ForeColor = Color.Transparent;
            EmailAst.ForeColor = Color.Transparent;
            lblAvisoEmail.Visible = false;

            ConfigurarApparence(
                txtTurmaEnabled: false,
                txtEmailEnabled: false,
                txtSenhaEnabled: false
            );

            chkMostrarSenha.ForeColor = Color.LightGray;
            chkMostrarSenha.Enabled = false;
            chkMostrarSenha.Checked = false;
        }

        private void ConfigurarParaAluno()
        {
            HabilitarCampos(true);
            txtSenha.Enabled = false;
            txtConfirmSenha.Enabled = false;

            SetLabelColors(enabled: true);
            lblSenha.ForeColor = Color.LightGray;
            lblConfirmSenha.ForeColor = Color.LightGray;
            txtSenha.Text = "";
            txtConfirmSenha.Text = "";

            txtSenha.Visible = false;
            txtConfirmSenha.Visible = false;
            lblSenha.Visible = false;
            lblConfirmSenha.Visible = false;
            chkMostrarSenha.Visible = false;
            btnCadastrar.Location = new Point(541, 788);
            btnLimpar.Location = new Point(73, 788);
            CentralizarBotoes();

            SetAsteriscoVisibility(true);
            SenhaAst.ForeColor = Color.Transparent;
            ConfirmSenhaAst.ForeColor = Color.Transparent;
            EmailAst.ForeColor = Color.Transparent;

            ConfigurarApparence(
                txtTurmaEnabled: true,
                txtEmailEnabled: true,
                txtSenhaEnabled: false
            );

            txtTurma.PlaceholderText = "Digite aqui a turma...";
            txtEmail.PlaceholderText = "Digite aqui o email...";

            chkMostrarSenha.ForeColor = Color.LightGray;
            chkMostrarSenha.Enabled = false;
        }

        private void ConfigurarApparence(bool txtTurmaEnabled, bool txtEmailEnabled, bool txtSenhaEnabled)
        {
            txtNome.BackColor = Color.WhiteSmoke;
            txtNome.BorderColor = Color.FromArgb(204, 204, 204);

            mtxCPF.BackColor = Color.WhiteSmoke;
            mtxCPF.BorderColor = Color.FromArgb(204, 204, 204);

            mtxTelefone.BackColor = Color.WhiteSmoke;
            mtxTelefone.BorderColor = Color.FromArgb(204, 204, 204);

            dtpDataNasc.BackColor = Color.WhiteSmoke;

            txtEmail.BackColor = txtEmailEnabled ? Color.WhiteSmoke : Color.White;
            txtEmail.BorderColor = txtEmailEnabled ? Color.LightGray : Color.WhiteSmoke;

            txtTurma.BackColor = txtTurmaEnabled ? Color.WhiteSmoke : Color.White;
            txtTurma.BorderColor = txtTurmaEnabled ? Color.LightGray : Color.WhiteSmoke;

            txtSenha.BackColor = txtSenhaEnabled ? Color.WhiteSmoke : Color.White;
            txtSenha.BorderColor = txtSenhaEnabled ? Color.LightGray : Color.WhiteSmoke;

            txtConfirmSenha.BackColor = txtSenhaEnabled ? Color.WhiteSmoke : Color.White;
            txtConfirmSenha.BorderColor = txtSenhaEnabled ? Color.LightGray : Color.WhiteSmoke;
        }

        private void SetLabelColors(bool enabled)
        {
            Color color = enabled ? Color.FromArgb(20, 41, 60) : Color.LightGray;

            lblNome.ForeColor = color;
            lblSenha.ForeColor = color;
            lblConfirmSenha.ForeColor = color;
            lblCPF.ForeColor = color;
            lblDataNasc.ForeColor = color;
            lblTelefone.ForeColor = color;
            lblEmail.ForeColor = color;
            lblTurma.ForeColor = color;
        }

        private void SetAsteriscoVisibility(bool visible)
        {
            Color color = visible ? Color.Red : Color.Transparent;
            NomeAst.ForeColor = color;
            EmailAst.ForeColor = color;
            TurmaAst.ForeColor = color;
            TelefoneAst.ForeColor = color;
            DataNascAst.ForeColor = color;
            SenhaAst.ForeColor = color;
            ConfirmSenhaAst.ForeColor = color;
        }
        #endregion

        #region Métodos de Validação
        private bool ValidarCamposObrigatorios()
        {
            string nome = txtNome.Text.Trim();
            string tipoUsuario = cbUsuario.SelectedItem?.ToString() ?? string.Empty;
            string telefone = RemoverMascara(mtxTelefone.Text);
            string turma = txtTurma.Text.Trim();
            string senha = txtSenha.Text.Trim();
            string confirmar = txtConfirmSenha.Text.Trim();

            if (string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(tipoUsuario) ||
                string.IsNullOrWhiteSpace(telefone) ||
                (txtTurma.Enabled && string.IsNullOrWhiteSpace(turma)) ||
                (txtSenha.Visible && string.IsNullOrWhiteSpace(senha)) ||
                (txtConfirmSenha.Visible && string.IsNullOrWhiteSpace(confirmar)))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (tipoUsuario == "Aluno(a)" && string.IsNullOrWhiteSpace(turma))
            {
                MessageBox.Show("A turma é obrigatória para alunos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool ValidarDadosUsuario()
        {
            string cpf = RemoverMascara(mtxCPF.Text);
            string telefone = RemoverMascara(mtxTelefone.Text);
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text.Trim();
            string confirmar = txtConfirmSenha.Text.Trim();

            if (!string.IsNullOrWhiteSpace(cpf) && !ValidarCPF(cpf))
            {
                MessageBox.Show("CPF inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!ValidarTelefone(telefone))
            {
                MessageBox.Show("Telefone inválido. Insira o DDD e o número corretamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(email) && !ValidarEmail(email))
            {
                MessageBox.Show("E-mail inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (senha != confirmar)
            {
                MessageBox.Show("As senhas não coincidem.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(senha) && senha.Length < 4)
            {
                MessageBox.Show("A senha deve ter pelo menos 4 caracteres.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void CadastrarNovoUsuario()
        {
            string tipoUsuario = cbUsuario.Text;
            string hash = null;
            string salt = null;

            if (tipoUsuario == "Bibliotecário(a)")
            {
                BibliotecaApp.Utils.CriptografiaSenha.CriarHash(txtSenha.Text, out hash, out salt);
            }

            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();

                    using (SqlCeCommand comando = conexao.CreateCommand())
                    {
                        comando.CommandText = @"INSERT INTO usuarios
(Nome, Email, Senha_Hash, Senha_Salt, CPF, DataNascimento, Turma, Telefone, TipoUsuario) 
VALUES 
(@Nome, @Email, @Senha_hash, @Senha_salt, @CPF, @DataNascimento, @Turma, @Telefone, @TipoUsuario)";

                        comando.Parameters.AddWithValue("@Nome", txtNome.Text.Trim());
                        comando.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        comando.Parameters.AddWithValue("@Senha_hash", string.IsNullOrEmpty(hash) ? (object)DBNull.Value : hash);
                        comando.Parameters.AddWithValue("@Senha_salt", string.IsNullOrEmpty(salt) ? (object)DBNull.Value : salt);
                        comando.Parameters.AddWithValue("@CPF", mtxCPF.Text);
                        comando.Parameters.AddWithValue("@DataNascimento", dtpDataNasc.Value);
                        comando.Parameters.AddWithValue("@Turma", txtTurma.Text.Trim());
                        comando.Parameters.AddWithValue("@Telefone", mtxTelefone.Text);
                        comando.Parameters.AddWithValue("@TipoUsuario", tipoUsuario);

                        comando.ExecuteNonQuery();
                        LimparCampos();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    conexao.Close();
                }
            }

            MessageBox.Show("Cadastro concluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            AdicionarTurmaSeNova(txtTurma.Text.Trim());
            LimparCampos();
        }

        private void CentralizarBotoes()
        {
            int espacoEntre = 330;
            int larguraTotal = btnCadastrar.Width + btnLimpar.Width + espacoEntre;
            int xInicial = (panel1.Width - larguraTotal) / 2;
            int y = btnCadastrar.Location.Y;

            btnLimpar.Location = new Point(xInicial, y);
            btnCadastrar.Location = new Point(xInicial + btnLimpar.Width + espacoEntre, y);
        }
        #endregion

        #region Métodos de Turma
        private void InicializarTurmasPadrao()
        {
            todasTurmasPadrao = new List<string>();
            foreach (var categoria in dicionarioTurmas.Values)
            {
                todasTurmasPadrao.AddRange(categoria);
            }
        }

        private string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            texto = texto.ToLower().Trim();

            // Remover acentos
            texto = Regex.Replace(texto.Normalize(NormalizationForm.FormD), @"[\p{Mn}]", "");

            // Remover caracteres especiais, exceto números, letras e °
            texto = Regex.Replace(texto, @"[^a-z0-9°\s]", "");

            // Remover espaços extras
            texto = Regex.Replace(texto, @"\s+", " ");

            return texto;
        }

        private string CorrigirTurma(string turmaDigitada)
        {
            if (string.IsNullOrWhiteSpace(turmaDigitada))
                return turmaDigitada;

            // Adicionar ° automaticamente se não tiver
            if (!turmaDigitada.Contains("°") && Regex.IsMatch(turmaDigitada, @"^\d+"))
            {
                turmaDigitada = Regex.Replace(turmaDigitada, @"^(\d+)", "$1°");
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
            else
            {
                // Se não encontrou o padrão com °, tentar sem °
                matchNumero = Regex.Match(turmaDigitada, @"^(\d+)\s");
                if (matchNumero.Success)
                {
                    numeroStr = matchNumero.Groups[1].Value;
                    turmaDigitada = turmaDigitada.Replace(matchNumero.Value, matchNumero.Groups[1].Value + "° ");
                }
            }

            // Determinar o tipo de turma
            string turmaLower = turmaDigitada.ToLower();
            if (turmaLower.Contains("p"))
            {
                tipo = "Propedêutico";
            }
            else if (turmaLower.Contains("d"))
            {
                tipo = "Desenvolvimento";
            }
            else if (turmaLower.Contains("ag"))
            {
                tipo = "Agronegócio";
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
                    // Não há limite máximo para o número da turma
                    numeroTurma = numero.ToString();
                }
            }

            // NOVA LÓGICA: Remover apenas o número "1" no final para cursos técnicos
            if (!string.IsNullOrEmpty(tipo) && tipo != "Ano" && !string.IsNullOrEmpty(numeroTurma) && numeroTurma == "1")
            {
                // Para cursos técnicos, remover apenas o número "1" da turma (final)
                numeroTurma = "";
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

        private void CarregarTurmasDoBanco()
        {
            turmasCadastradas.Clear();

            using (var conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();
                    using (var comando = conexao.CreateCommand())
                    {
                        comando.CommandText = "SELECT DISTINCT Turma FROM usuarios WHERE Turma IS NOT NULL AND Turma <> ''";

                        using (var leitor = comando.ExecuteReader())
                        {
                            while (leitor.Read())
                            {
                                turmasCadastradas.Add(leitor["Turma"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar turmas: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conexao.Close();
                }
            }
        }

        private void txtTurma_TextChanged(object sender, EventArgs e)
        {
            string texto = txtTurma.Text.Trim();

            // Não aplicar correções automáticas durante a digitação para não atrapalhar o usuário
            // Apenas mostrar sugestões baseadas nas turmas já cadastradas

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

        private void AdicionarTurmaSeNova(string turma)
        {
            turma = CorrigirTurma(turma);
            if (string.IsNullOrEmpty(turma)) return;

            // Verificar se já existe (case insensitive)
            bool existe = turmasCadastradas.Any(t =>
                string.Equals(t, turma, StringComparison.OrdinalIgnoreCase));

            if (!existe)
            {
                turmasCadastradas.Add(turma);
                turmasCadastradas.Sort();
            }
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

        #region Estilizacao do ListBox de Sugestões
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

            Color backColor = hovered
                ? Color.FromArgb(235, 235, 235)
                : Color.White;
            Color textColor = Color.FromArgb(60, 60, 60);

            using (SolidBrush b = new SolidBrush(backColor))
                e.Graphics.FillRectangle(b, e.Bounds);

            string text = listBox.Items[e.Index].ToString();
            Font font = listBox.Font;

            Rectangle textRect = new Rectangle(e.Bounds.Left + 12, e.Bounds.Top, e.Bounds.Width - 24, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, text, font, textRect, textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

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

        private void mtxTelefone_Load(object sender, EventArgs e)
        {
            // Não é necessário implementação adicional
        }

        
    }
}