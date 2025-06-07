using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace BibliotecaApp
{


    public partial class CadUsuario : Form
    {

        public CadUsuario(List<Usuarios> usuarios)
        {

            InitializeComponent();


        }

        public CadUsuario()
        {
            InitializeComponent();


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void CadUsuario_Load(object sender, EventArgs e)
        {
            dtpDataNasc.Value = DateTime.Today;
            HabilitarCampos(false);
            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;
            chkMostrarSenha.ForeColor = Color.LightGray;

            //asteristicos combo null

            NomeAst.ForeColor = Color.Transparent;
            TurmaAst.ForeColor = Color.Transparent;
            TelefoneAst.ForeColor = Color.Transparent;
            DataNascAst.ForeColor = Color.Transparent;
            SenhaAst.ForeColor = Color.Transparent;
            ConfirmSenhaAst.ForeColor = Color.Transparent;



        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
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




        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }


        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void roundedMaskedTextBox2_Load(object sender, EventArgs e)
        {

        }





        private void txtNome_Load(object sender, EventArgs e)
        {

        }



        private void dtpDataNasc_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtEmail_Load(object sender, EventArgs e)
        {

        }

        private void mtxTelefone_Load(object sender, EventArgs e)
        {

        }

        private void mtxCPF_Load(object sender, EventArgs e)
        {

        }

        private void txtTurma_Load(object sender, EventArgs e)
        {

        }





        private void cbUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            // chama o combobox
            string funcaoSelecionada = cbUsuario.SelectedItem?.ToString();


            if (string.IsNullOrWhiteSpace(funcaoSelecionada))
            {

                HabilitarCampos(true);

                lblNome.ForeColor = Color.FromArgb(20, 41, 60);
                lblSenha.ForeColor = Color.FromArgb(20, 41, 60);
                lblConfirmSenha.ForeColor = Color.FromArgb(20, 41, 60);
                lblCPF.ForeColor = Color.FromArgb(20, 41, 60);
                lblDataNasc.ForeColor = Color.FromArgb(20, 41, 60);
                lblTelefone.ForeColor = Color.FromArgb(20, 41, 60);
                lblEmail.ForeColor = Color.FromArgb(20, 41, 60);
                lblTurma.ForeColor = Color.FromArgb(20, 41, 60);
                mtxCPF.ForeColor = Color.WhiteSmoke;

                NomeAst.ForeColor = Color.Transparent;


                TurmaAst.ForeColor = Color.Red;

                chkMostrarSenha.ForeColor = Color.LightGray;

                return;
            }


            if (funcaoSelecionada == "Bibliotecário(a)")
            {

                txtEmail.Enabled = false;
                txtTurma.Enabled = false;
                txtNome.Enabled = true;
                txtSenha.Enabled = true;
                txtConfirmSenha.Enabled = true;
                mtxCPF.Enabled = true;
                mtxTelefone.Enabled = true;
                dtpDataNasc.Enabled = true;


                lblNome.ForeColor = Color.FromArgb(20, 41, 60);
                lblSenha.ForeColor = Color.FromArgb(20, 41, 60);
                lblConfirmSenha.ForeColor = Color.FromArgb(20, 41, 60);
                lblCPF.ForeColor = Color.FromArgb(20, 41, 60);
                lblDataNasc.ForeColor = Color.FromArgb(20, 41, 60);
                lblTelefone.ForeColor = Color.FromArgb(20, 41, 60);
                lblEmail.ForeColor = Color.LightGray;
                lblTurma.ForeColor = Color.LightGray;
                txtEmail.BackColor = Color.White;
                txtTurma.BackColor = Color.White;
                txtEmail.BorderColor = Color.WhiteSmoke;
                txtTurma.BorderColor = Color.WhiteSmoke;
                txtSenha.BorderColor = Color.LightGray;
                txtConfirmSenha.BorderColor = Color.LightGray;
                txtSenha.BackColor = Color.WhiteSmoke;
                txtConfirmSenha.BackColor = Color.WhiteSmoke;
                txtNome.BackColor = Color.WhiteSmoke;
                txtNome.BorderColor = Color.FromArgb(204, 204, 204);

                dtpDataNasc.BackColor = Color.WhiteSmoke;

                mtxCPF.BackColor = Color.WhiteSmoke;
                mtxCPF.BorderColor = Color.FromArgb(204, 204, 204);
                mtxTelefone.BackColor = Color.WhiteSmoke;
                mtxTelefone.BorderColor = Color.FromArgb(204, 204, 204);





                chkMostrarSenha.Enabled = true;


                TurmaAst.ForeColor = Color.Transparent;
                SenhaAst.ForeColor = Color.Red;
                NomeAst.ForeColor = Color.Red;
                ConfirmSenhaAst.ForeColor = Color.Red;
                TelefoneAst.ForeColor = Color.Red;
                DataNascAst.ForeColor = Color.Red;


                txtSenha.Text = "";
                txtConfirmSenha.Text = "";

                txtTurma.Text = "";
                txtEmail.Text = "";

                txtSenha.PlaceholderText = $"Digite aqui uma senha...";
                txtConfirmSenha.PlaceholderText = $"Confirme a senha...";
                txtEmail.PlaceholderText = $"";
                txtEmail.PlaceholderColor = Color.WhiteSmoke;
                txtTurma.PlaceholderText = $"";
                txtTurma.PlaceholderColor = Color.WhiteSmoke;

                chkMostrarSenha.ForeColor = Color.FromArgb(20, 41, 60);
            }
            else if (funcaoSelecionada == "Professor(a)")
            {
                txtEmail.Enabled = true;
                txtTurma.Enabled = false;
                txtNome.Enabled = true;
                txtSenha.Enabled = false;
                txtConfirmSenha.Enabled = false;
                mtxCPF.Enabled = true;
                mtxTelefone.Enabled = true;
                dtpDataNasc.Enabled = true;


                lblNome.ForeColor = Color.FromArgb(20, 41, 60);
                lblSenha.ForeColor = Color.LightGray;
                lblConfirmSenha.ForeColor = Color.LightGray;
                lblCPF.ForeColor = Color.FromArgb(20, 41, 60);
                lblDataNasc.ForeColor = Color.FromArgb(20, 41, 60);
                lblTelefone.ForeColor = Color.FromArgb(20, 41, 60);
                lblEmail.ForeColor = Color.FromArgb(20, 41, 60);
                lblTurma.ForeColor = Color.LightGray;

                chkMostrarSenha.Enabled = false;


                TurmaAst.ForeColor = Color.Transparent;
                SenhaAst.ForeColor = Color.Transparent;
                ConfirmSenhaAst.ForeColor = Color.Transparent;
                NomeAst.ForeColor = Color.Red;
                TelefoneAst.ForeColor = Color.Red;
                DataNascAst.ForeColor = Color.Red;

                txtSenha.BorderColor = Color.WhiteSmoke;
                txtConfirmSenha.BorderColor = Color.WhiteSmoke;
                txtSenha.BackColor = Color.White;
                txtConfirmSenha.BackColor = Color.White;
                txtEmail.BackColor = Color.WhiteSmoke;
                txtEmail.BorderColor = Color.LightGray;
                txtTurma.BorderColor = Color.WhiteSmoke;
                txtTurma.BackColor = Color.White;
                txtNome.BackColor = Color.WhiteSmoke;
                txtNome.BorderColor = Color.FromArgb(204, 204, 204);

                mtxCPF.BackColor = Color.WhiteSmoke;
                mtxCPF.BorderColor = Color.FromArgb(204, 204, 204);
                mtxTelefone.BackColor = Color.WhiteSmoke;
                mtxTelefone.BorderColor = Color.FromArgb(204, 204, 204);


                dtpDataNasc.BackColor = Color.WhiteSmoke;


                txtTurma.Text = "";
                txtSenha.Text = "";
                txtConfirmSenha.Text = "";
                chkMostrarSenha.Checked = false;

                txtTurma.PlaceholderText = $"";
                txtSenha.PlaceholderText = $"";
                txtConfirmSenha.PlaceholderText = $"";
                txtEmail.PlaceholderText = "Digite aqui o email...";
                txtEmail.PlaceholderColor = Color.Gray;


                chkMostrarSenha.ForeColor = Color.LightGray;


            }
            else if (funcaoSelecionada == "Outros")
            {
                txtEmail.Enabled = false;
                txtTurma.Enabled = false;
                txtNome.Enabled = true;
                txtSenha.Enabled = false;
                txtConfirmSenha.Enabled = false;
                mtxCPF.Enabled = true;
                mtxTelefone.Enabled = true;
                dtpDataNasc.Enabled = true;

                lblNome.ForeColor = Color.FromArgb(20, 41, 60);
                lblSenha.ForeColor = Color.LightGray;
                lblConfirmSenha.ForeColor = Color.LightGray;
                lblCPF.ForeColor = Color.FromArgb(20, 41, 60);
                lblDataNasc.ForeColor = Color.FromArgb(20, 41, 60);
                lblTelefone.ForeColor = Color.FromArgb(20, 41, 60);
                lblEmail.ForeColor = Color.LightGray;
                lblTurma.ForeColor = Color.LightGray;

                chkMostrarSenha.Enabled = false;
                chkMostrarSenha.Checked = false;


                TurmaAst.ForeColor = Color.Transparent;
                SenhaAst.ForeColor = Color.Transparent;
                ConfirmSenhaAst.ForeColor = Color.Transparent;
                NomeAst.ForeColor = Color.Red;
                TelefoneAst.ForeColor = Color.Red;
                DataNascAst.ForeColor = Color.Red;

                txtNome.BackColor = Color.WhiteSmoke;
                txtNome.BorderColor = Color.FromArgb(204, 204, 204);
                mtxCPF.BackColor = Color.WhiteSmoke;
                mtxCPF.BorderColor = Color.FromArgb(204, 204, 204);
                mtxTelefone.BackColor = Color.WhiteSmoke;
                mtxTelefone.BorderColor = Color.FromArgb(204, 204, 204);
                dtpDataNasc.BackColor = Color.WhiteSmoke;


                txtSenha.BorderColor = Color.WhiteSmoke;
                txtConfirmSenha.BorderColor = Color.WhiteSmoke;
                txtSenha.BackColor = Color.White;
                txtConfirmSenha.BackColor = Color.White;
                txtTurma.BackColor = Color.White;
                txtEmail.BackColor = Color.White;
                txtTurma.BorderColor = Color.WhiteSmoke;
                txtEmail.BorderColor = Color.WhiteSmoke;

                txtEmail.Text = "";
                txtTurma.Text = "";
                txtSenha.Text = "";
                txtConfirmSenha.Text = "";

                txtEmail.PlaceholderText = $"";
                txtTurma.PlaceholderText = $"";
                txtSenha.PlaceholderText = $"";
                txtConfirmSenha.PlaceholderText = $"";

                chkMostrarSenha.ForeColor = Color.LightGray;
            }
            else
            {

                txtEmail.Enabled = true;
                txtTurma.Enabled = true;
                txtNome.Enabled = true;
                txtSenha.Enabled = false;
                txtConfirmSenha.Enabled = false;
                mtxCPF.Enabled = true;
                mtxTelefone.Enabled = true;
                dtpDataNasc.Enabled = true;

                txtSenha.PlaceholderText = $"";
                txtConfirmSenha.PlaceholderText = $"";


                chkMostrarSenha.ForeColor = Color.LightGray;

                dtpDataNasc.BackColor = Color.WhiteSmoke;


                lblNome.ForeColor = Color.FromArgb(20, 41, 60);
                lblSenha.ForeColor = Color.LightGray;
                lblConfirmSenha.ForeColor = Color.LightGray;
                lblCPF.ForeColor = Color.FromArgb(20, 41, 60);
                lblDataNasc.ForeColor = Color.FromArgb(20, 41, 60);
                lblTelefone.ForeColor = Color.FromArgb(20, 41, 60);
                lblEmail.ForeColor = Color.FromArgb(20, 41, 60);
                lblTurma.ForeColor = Color.FromArgb(20, 41, 60);
                txtSenha.BackColor = Color.White;
                txtConfirmSenha.BackColor = Color.White;
                txtEmail.BackColor = Color.WhiteSmoke;




                chkMostrarSenha.Checked = false;
                chkMostrarSenha.Enabled = false;


                TurmaAst.ForeColor = Color.Red;
                SenhaAst.ForeColor = Color.Transparent;
                ConfirmSenhaAst.ForeColor = Color.Transparent;
                NomeAst.ForeColor = Color.Red;
                TelefoneAst.ForeColor = Color.Red;
                DataNascAst.ForeColor = Color.Red;

                txtEmail.BorderColor = Color.LightGray;
                txtTurma.BorderColor = Color.LightGray;
                txtTurma.BackColor = Color.WhiteSmoke;
                txtSenha.BackColor = Color.White;
                txtConfirmSenha.BackColor = Color.White;
                txtSenha.BorderColor = Color.WhiteSmoke;
                txtConfirmSenha.BorderColor = Color.WhiteSmoke;
                txtNome.BackColor = Color.WhiteSmoke;
                txtNome.BorderColor = Color.FromArgb(204, 204, 204);
                mtxCPF.BackColor = Color.WhiteSmoke;
                mtxCPF.BorderColor = Color.FromArgb(204, 204, 204);
                mtxTelefone.BackColor = Color.WhiteSmoke;
                mtxTelefone.BorderColor = Color.FromArgb(204, 204, 204);


                txtTurma.PlaceholderText = "Digite aqui a turma...";
                txtEmail.PlaceholderText = "Digite aqui o email...";
                txtTurma.PlaceholderColor = Color.Gray;
                txtEmail.PlaceholderColor = Color.Gray;


            }
        }





        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            // Coleta os dados dos campos do formulário
            string nome = txtNome.Text.Trim();
            string tipoUsuario = cbUsuario.SelectedItem?.ToString() ?? string.Empty;
            string email = string.IsNullOrWhiteSpace(txtEmail.Text) ? "" : txtEmail.Text.Trim();
            string cpf = string.IsNullOrWhiteSpace(mtxCPF.Text) ? "" : RemoverMascara(mtxCPF.Text);
            DateTime nascimento = dtpDataNasc.Value;
            string turma = txtTurma.Text.Trim();
            string telefone = RemoverMascara(mtxTelefone.Text);
            string senha = txtSenha.Text.Trim();
            string confirmar = txtConfirmSenha.Text.Trim();

            // Verificação de campos obrigatórios somente se estiverem habilitados
            if (string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(tipoUsuario) ||
                string.IsNullOrWhiteSpace(telefone) ||
                (txtTurma.Enabled && string.IsNullOrWhiteSpace(turma)) ||
                (txtSenha.Enabled && string.IsNullOrWhiteSpace(senha)) ||
                (txtConfirmSenha.Enabled && string.IsNullOrWhiteSpace(confirmar)))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Verificação de turma para Aluno
            if (tipoUsuario == "Aluno(a)" && string.IsNullOrWhiteSpace(turma))
            {
                MessageBox.Show("A turma é obrigatória para alunos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Verificação de senhas coincidentes
            if (senha != confirmar)
            {
                MessageBox.Show("As senhas não coincidem.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Verificação do tipo de usuário selecionado
            if (cbUsuario.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um tipo de usuário.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validação do CPF, caso tenha sido preenchido
            if (!string.IsNullOrWhiteSpace(cpf) && !ValidarCPF(cpf))
            {
                MessageBox.Show("CPF inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validação do telefone
            if (!ValidarTelefone(telefone))
            {
                MessageBox.Show("Telefone inválido. Insira o DDD e o número corretamente. Confira se falta um '9' no começo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validação do email, caso tenha sido preenchido
            if (!string.IsNullOrWhiteSpace(email) && !ValidarEmail(email))
            {
                MessageBox.Show("O e-mail informado não é válido. Verifique o formato.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validação da senha (mínimo de 6 caracteres)
            if (!string.IsNullOrWhiteSpace(senha) && senha.Length < 4)
            {
                MessageBox.Show("A senha deve ter pelo menos 4 caracteres.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Limpar campos
            LimparCampos();

            int novoId = Usuarios.ListaUsuarios.Count + 1;

            Usuarios usuario = new Usuarios
            {
                Id = novoId,
                Nome = nome,
                TipoUsuario = tipoUsuario,
                CPF = cpf,
                DataNascimento = nascimento,
                Telefone = telefone,
                Email = email,
                Turma = turma,
                Senha = senha,
                ConfirmarSenha = confirmar
            };

            Usuarios.ListaUsuarios.Add(usuario);
            MessageBox.Show("Cadastro concluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }



        public bool ValidarEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        // Função para remover a máscara de CPF e telefone
        private string RemoverMascara(string texto)
        {
            return string.Concat(texto.Where(c => char.IsDigit(c)));
        }



        private void LimparCampos()
        {

            // Limpar todos os campos
            txtNome.Text = ""; // Para RoundedTextBox, usamos Text = ""
            txtEmail.Text = ""; // Para RoundedTextBox, usamos Text = ""
            mtxCPF.Text = "";   // Para RoundedMaskedTextBox, usamos Text = ""
            txtTurma.Text = ""; // Para RoundedTextBox, usamos Text = ""
            mtxTelefone.Text = ""; // Para RoundedMaskedTextBox, usamos Text = ""
            txtSenha.Text = ""; // Para RoundedTextBox, usamos Text = ""
            txtConfirmSenha.Text = "";

            dtpDataNasc.Value = DateTime.Today;  // Para DateTimePicker, definimos o valor para hoje
        }

        private void txtSenha_Load(object sender, EventArgs e)
        {

        }


        private void txtSenha_TextChanged(object sender, EventArgs e)
        {

        }



        private void label1_Click_2(object sender, EventArgs e)
        {

        }



        private void lblCPF_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void aviso_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_3(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void TelefoneAst_Click(object sender, EventArgs e)
        {

        }

        private void TurmaAst_Click(object sender, EventArgs e)
        {

        }

        private void DataNascAst_Click(object sender, EventArgs e)
        {

        }

        private void CpfAst_Click(object sender, EventArgs e)
        {

        }

        private void ConfirmSenhaAst_Click(object sender, EventArgs e)
        {

        }

        private void SenhaAst_Click(object sender, EventArgs e)
        {

        }

        private void EmailAst_Click(object sender, EventArgs e)
        {

        }

        private void NomeAst_Click(object sender, EventArgs e)
        {

        }

        private void lblConfirmSenha_Click(object sender, EventArgs e)
        {

        }

        private void lblSenha_Click(object sender, EventArgs e)
        {

        }

        private void txtConfirmSenha_Load_1(object sender, EventArgs e)
        {

        }

        private void roundedPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtpDataNasc_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void Panel1_ControlAdded(object sender, ControlEventArgs e)
        {

        }









        private void chkMostrarSenha_CheckedChanged(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
            txtConfirmSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
        }

        private void progressBarForca_Click(object sender, EventArgs e)
        {

        }

        private void lblReq1_Click(object sender, EventArgs e)
        {

        }

        private void lblReq2_Click(object sender, EventArgs e)
        {

        }

        private void lblForca_Click(object sender, EventArgs e)
        {

        }

        private void panelRequisitos_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
        "Tem certeza de que deseja limpar tudo?",
        "Confirmação",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

            if (resultado == DialogResult.Yes)
            {

                txtNome.Text = "";
                txtEmail.Text = "";
                txtTurma.Text = "";
                mtxTelefone.Text = "";
                mtxCPF.Text = "";
                dtpDataNasc.Text = "";
                txtSenha.Text = "";
                txtConfirmSenha.Text = "";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNome_Load_1(object sender, EventArgs e)
        {

        }

        private void txtNome_Load_2(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
       "Tem certeza de que deseja limpar tudo?",
       "Confirmação",
       MessageBoxButtons.YesNo,
       MessageBoxIcon.Question
   );

            if (resultado == DialogResult.Yes)
            {

                txtNome.Text = "";
                txtEmail.Text = "";
                txtTurma.Text = "";
                mtxTelefone.Text = "";
                mtxCPF.Text = "";
                dtpDataNasc.Text = "";
                txtSenha.Text = "";
                txtConfirmSenha.Text = "";
            }
        }

        private void txtConfirmSenha_Load(object sender, EventArgs e)
        {

        }

      
    }


}