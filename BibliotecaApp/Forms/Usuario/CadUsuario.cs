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
            txtNome.Text = "";
            txtEmail.Text = "";
            txtTurma.Text = "";
            mtxTelefone.Text = "";
            mtxCPF.Text = "";
            dtpDataNasc.Value = DateTime.Today;
            txtSenha.Text = "";
            txtConfirmSenha.Text = "";
            txtNome.Focus();
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
            txtSenha.Enabled = false;
            txtConfirmSenha.Enabled = false;

            SetLabelColors(enabled: true);
            lblSenha.ForeColor = Color.LightGray;
            lblConfirmSenha.ForeColor = Color.LightGray;
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
                (txtSenha.Enabled && string.IsNullOrWhiteSpace(senha)) ||
                (txtConfirmSenha.Enabled && string.IsNullOrWhiteSpace(confirmar)))
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
                        comando.Parameters.AddWithValue("@Senha_Hash", string.IsNullOrEmpty(hash) ? (object)DBNull.Value : hash);
                        comando.Parameters.AddWithValue("@Senha_Salt", string.IsNullOrEmpty(salt) ? (object)DBNull.Value : salt);
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
            this.Close();
        }


        #endregion

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
