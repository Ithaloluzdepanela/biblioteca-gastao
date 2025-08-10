using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BibliotecaApp.Froms.Usuario
{
    public partial class CadUsuario : Form
    {
        #region Construtores
        /// <summary>
        /// Construtor principal com lista de usuários
        /// </summary>
        /// 


        private List<string> turmasCadastradas = new List<string>();

        public CadUsuario(List<Usuarios> usuarios)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Construtor vazio para design-time
        /// </summary>
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

        // Classe estática para conectar ao banco .sdf
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
        /// <summary>
        /// Limpa todos os campos do formulário
        /// </summary>
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

        /// <summary>
        /// Valida se um CPF é válido
        /// </summary>
        public bool ValidarCPF(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            // Cálculo dos dígitos verificadores
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

        /// <summary>
        /// Valida se um telefone tem formato válido (com DDD)
        /// </summary>
        public bool ValidarTelefone(string telefone)
        {
            string apenasNumeros = new string(telefone.Where(char.IsDigit).ToArray());
            return apenasNumeros.Length >= 11 && apenasNumeros.Length <= 12;
        }

        /// <summary>
        /// Valida formato de e-mail
        /// </summary>
        public bool ValidarEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
        #endregion

        #region Eventos de Controles
        //------------------------------------------------------------
        // EVENTO: Mudança no tipo de usuário selecionado
        //------------------------------------------------------------
        private void cbUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            string funcaoSelecionada = cbUsuario.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(funcaoSelecionada))
            {
                ConfigurarCamposGenericos();
                return;
            }

            // Seleção de configuração específica para cada tipo de usuário
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

                default: // Aluno(a) ou padrão
                    ConfigurarParaAluno();
                    break;
            }
        }

        //------------------------------------------------------------
        // EVENTO: Clique no botão Cadastrar
        //------------------------------------------------------------
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            // 1. Validação dos campos obrigatórios
            if (!ValidarCamposObrigatorios())
                return;

            // 2. Validações específicas
            if (!ValidarDadosUsuario())
                return;

            // 3. Cadastro do usuário
            CadastrarNovoUsuario();
        }

        //------------------------------------------------------------
        // EVENTO: Mostrar/Ocultar senha
        //------------------------------------------------------------
        private void chkMostrarSenha_CheckedChanged(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
            txtConfirmSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
        }

        //------------------------------------------------------------
        // EVENTO: Limpar formulário
        //------------------------------------------------------------
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

        //------------------------------------------------------------
        // EVENTO: Alteração no campo de e-mail
        //------------------------------------------------------------
        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            lblAvisoEmail.Visible = string.IsNullOrWhiteSpace(txtEmail.Text);
        }


        #endregion

        #region Métodos Privados
        /// <summary>
        /// Habilita/desabilita todos os campos do formulário
        /// </summary>
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

        /// <summary>
        /// Remove máscaras de campos formatados
        /// </summary>
        private string RemoverMascara(string texto)
        {
            return string.Concat(texto.Where(c => char.IsDigit(c)));
        }

        #endregion

        #region Métodos de Configuração de Interface
        /// <summary>
        /// Configuração genérica quando nenhum tipo de usuário está selecionado
        /// </summary>
        private void ConfigurarCamposGenericos()
        {
            HabilitarCampos(true);
            SetLabelColors(enabled: true);
            SetAsteriscoVisibility(false);
            chkMostrarSenha.ForeColor = Color.LightGray;
        }

        /// <summary>
        /// Configuração específica para Bibliotecário
        /// </summary>
        private void ConfigurarParaBibliotecario()
        {
            HabilitarCampos(true);
            txtTurma.Enabled = false;

            //Visual com senha
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

            // Prepara placeholders
            txtSenha.PlaceholderText = "Digite aqui uma senha...";
            txtConfirmSenha.PlaceholderText = "Confirme a senha...";
            txtEmail.PlaceholderText = "Digite aqui o email...";

            chkMostrarSenha.Enabled = true;
            chkMostrarSenha.ForeColor = Color.FromArgb(20, 41, 60);
        }

        /// <summary>
        /// Configuração específica para Professor
        /// </summary>
        private void ConfigurarParaProfessor()
        {
            HabilitarCampos(true);
            txtTurma.Enabled = false;




            //Visual sem senha
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

        /// <summary>
        /// Configuração específica para Outros
        /// </summary>
        private void ConfigurarParaOutros()
        {
            HabilitarCampos(true);
            txtEmail.Enabled = false;
            txtTurma.Enabled = false;
            txtSenha.Enabled = false;
            txtConfirmSenha.Enabled = false;

            //Visual sem senha
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

        /// <summary>
        /// Configuração específica para Aluno
        /// </summary>
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

            //Visual sem senha
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

        /// <summary>
        /// Configuração visual dos controles
        /// </summary>
        private void ConfigurarApparence(bool txtTurmaEnabled, bool txtEmailEnabled, bool txtSenhaEnabled)
        {
            // Configuração básica dos campos
            txtNome.BackColor = Color.WhiteSmoke;
            txtNome.BorderColor = Color.FromArgb(204, 204, 204);

            mtxCPF.BackColor = Color.WhiteSmoke;
            mtxCPF.BorderColor = Color.FromArgb(204, 204, 204);

            mtxTelefone.BackColor = Color.WhiteSmoke;
            mtxTelefone.BorderColor = Color.FromArgb(204, 204, 204);

            dtpDataNasc.BackColor = Color.WhiteSmoke;

            // Configuração condicional
            txtEmail.BackColor = txtEmailEnabled ? Color.WhiteSmoke : Color.White;
            txtEmail.BorderColor = txtEmailEnabled ? Color.LightGray : Color.WhiteSmoke;

            txtTurma.BackColor = txtTurmaEnabled ? Color.WhiteSmoke : Color.White;
            txtTurma.BorderColor = txtTurmaEnabled ? Color.LightGray : Color.WhiteSmoke;

            txtSenha.BackColor = txtSenhaEnabled ? Color.WhiteSmoke : Color.White;
            txtSenha.BorderColor = txtSenhaEnabled ? Color.LightGray : Color.WhiteSmoke;

            txtConfirmSenha.BackColor = txtSenhaEnabled ? Color.WhiteSmoke : Color.White;
            txtConfirmSenha.BorderColor = txtSenhaEnabled ? Color.LightGray : Color.WhiteSmoke;
        }

        /// <summary>
        /// Define cores das labels
        /// </summary>
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

        /// <summary>
        /// Mostra/esconde asteriscos de campos obrigatórios
        /// </summary>
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
        /// <summary>
        /// Valida todos os campos obrigatórios
        /// </summary>
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

        /// <summary>
        /// Valida os dados específicos do usuário
        /// </summary>
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

        /// <summary>
        /// Efetua o cadastro do novo usuário
        /// </summary>
        private void CadastrarNovoUsuario()
        {
            string tipoUsuario = cbUsuario.Text;
            string hash = null;
            string salt = null;

            // Gera hash e salt apenas se for Bibliotecário(a)
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

        /// <summary>
        /// Centraliza os botões de cadastrar e limpar
        /// </summary>
        private void CentralizarBotoes()
        {
            int espacoEntre = 330;
            int larguraTotal = btnCadastrar.Width + btnLimpar.Width + espacoEntre;
            int xInicial = (panel1.Width - larguraTotal) / 2;
            int y = btnCadastrar.Location.Y; // mantém o Y atual

            btnLimpar.Location = new Point(xInicial, y);
            btnCadastrar.Location = new Point(xInicial + btnLimpar.Width + espacoEntre, y);
        }

        #endregion

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

            if (string.IsNullOrEmpty(texto))
            {
                lstSugestoesTurma.Visible = false;
                return;
            }

            var sugestoes = turmasCadastradas
                .Where(t => t != null && t.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(t => t)
                .ToList();

            lstSugestoesTurma.Items.Clear();

            if (sugestoes.Count > 0)
            {
                foreach (var s in sugestoes)
                    lstSugestoesTurma.Items.Add(s);

                int visibleItems = Math.Min(2, sugestoes.Count); 
                int extraPadding = 8; 
                lstSugestoesTurma.Height = visibleItems * lstSugestoesTurma.ItemHeight + extraPadding;
                lstSugestoesTurma.Width = txtTurma.Width;
                lstSugestoesTurma.Left = txtTurma.Left;
                lstSugestoesTurma.Top = txtTurma.Bottom; 
                lstSugestoesTurma.Visible = true;
            }
            else
            {
                // Nenhuma sugestão: apenas esconda a lista.
                // O usuário pode continuar digitando e cadastrar uma turma nova.
                lstSugestoesTurma.Visible = false;
            }
        }

        private void lstSugestoesTurma_Click(object sender, EventArgs e)
        {
            if (lstSugestoesTurma.SelectedItem != null)
            {
                txtTurma.Text = lstSugestoesTurma.SelectedItem.ToString();
                lstSugestoesTurma.Visible = false;
                txtTurma.Focus();
                
            }
        }

        private void AdicionarTurmaSeNova(string turma)
        {
            turma = (turma ?? "").Trim();
            if (string.IsNullOrEmpty(turma)) return;

            bool existe = turmasCadastradas.Any(t => string.Equals(t, turma, StringComparison.OrdinalIgnoreCase));
            if (!existe)
            {
                turmasCadastradas.Add(turma);
                // Sem insert em tabela de Turmas (ela não existe).
                // A turma passa a existir de fato quando um usuário é cadastrado com esse valor.
            }
        }

        private void txtTurma_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesTurma.Visible || lstSugestoesTurma.Items.Count == 0)
            {
                // Enter aqui confirma o texto digitado e segue para o próximo campo
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    this.SelectNextControl((Control)sender, forward: true, tabStopOnly: true, nested: true, wrap: true);
                }
                return;
            }

            // Com lista visível:
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
                // Se houver seleção, aplica; senão pega o primeiro item
                if (lstSugestoesTurma.SelectedItem != null)
                    txtTurma.Text = lstSugestoesTurma.SelectedItem.ToString();
                else if (lstSugestoesTurma.Items.Count > 0)
                    txtTurma.Text = lstSugestoesTurma.Items[0].ToString();

                lstSugestoesTurma.Visible = false;
                this.SelectNextControl((Control)sender, forward: true, tabStopOnly: true, nested: true, wrap: true);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesTurma.Visible = false;
            }
        }

        private void txtTurma_Leave(object sender, EventArgs e)
        {
            // Esconde a lista ao sair do campo, a menos que o foco vá para a própria lista
            BeginInvoke(new Action(() =>
            {
                if (!lstSugestoesTurma.Focused)
                    lstSugestoesTurma.Visible = false;
            }));
        }

        private void lstSugestoesTurma_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstSugestoesTurma.SelectedItem != null)
            {
                e.SuppressKeyPress = true;
                txtTurma.Text = lstSugestoesTurma.SelectedItem.ToString();
                lstSugestoesTurma.Visible = false;
                txtTurma.Focus();
                this.SelectNextControl(txtTurma, forward: true, tabStopOnly: true, nested: true, wrap: true);
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
            // Ao sair da lista, esconda
            lstSugestoesTurma.Visible = false;
        }


        #region Estilizacao do ListBox de Sugestões
        // Campo para controle do hover
        private int hoveredIndex = -1;

        // Estilo e eventos do ListBox de sugestões
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
    }
}
