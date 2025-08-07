using BibliotecaApp.Forms.Livros;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Usuario
{
    public partial class EditarUsuarioForm : Form
    {
        public EditarUsuarioForm()
        {
            InitializeComponent();
            txtNome.Focus();
        }

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

        private List<Usuarios> _cacheUsuarios = new List<Usuarios>();
        private Usuarios _usuarioSelecionado;   

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (_usuarioSelecionado == null)
            {
                MessageBox.Show("Selecione um usuário primeiro.");
                return;
            }

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"UPDATE usuarios SET 
                            Nome = @nome, Email = @email, CPF = @cpf,
                            DataNascimento = @data, Telefone = @tel, Turma = @turma
                           WHERE Id = @id";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@cpf", mtxCPF.Text);
                        cmd.Parameters.AddWithValue("@data", dtpDataNasc.Value);
                        cmd.Parameters.AddWithValue("@tel", mtxTelefone.Text);
                        cmd.Parameters.AddWithValue("@turma", txtTurma.Text);
                        cmd.Parameters.AddWithValue("@id", _usuarioSelecionado.Id);

                        cmd.ExecuteNonQuery();
                    }
                }

                if (CpfJaExiste(mtxCPF.Text.Trim(), _usuarioSelecionado.Id))

                {
                    MessageBox.Show("Já existe outro usuário com esse CPF.", "CPF duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!EmailValido(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("E-mail inválido. Digite um e-mail válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("Usuário atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar: " + ex.Message);
            }
        }


        private void SelecionarUsuario(int index)
        {
            _usuarioSelecionado = _cacheUsuarios[index];
            
            txtNomeUsuario.Text = _usuarioSelecionado.Nome;
            lstSugestoesUsuario.Visible = false;
            AplicarConfiguracaoEdicaoUsuario();

            txtNome.Text = _usuarioSelecionado.Nome;
            txtEmail.Text = _usuarioSelecionado.Email;
            mtxCPF.Text = _usuarioSelecionado.CPF;
            dtpDataNasc.Value = _usuarioSelecionado.DataNascimento == DateTime.MinValue ? DateTime.Today : _usuarioSelecionado.DataNascimento;
            mtxTelefone.Text = _usuarioSelecionado.Telefone;
            txtTurma.Text = _usuarioSelecionado.Turma;
            

            lblTipoUsuario.Text = $"Tipo: {_usuarioSelecionado.TipoUsuario}";
            lblTipoUsuario.Visible = true;
        }


        

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (_usuarioSelecionado == null)
            {
                MessageBox.Show("Selecione um usuário primeiro.");
                return;
            }

            var confirm = MessageBox.Show("Tem certeza que deseja excluir este usuário?", "Confirmação", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "DELETE FROM Usuarios WHERE Id = @id";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", _usuarioSelecionado.Id);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Usuário excluído com sucesso!");
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir: " + ex.Message);
            }
        }

        private void LimparCampos()
        {
            txtNomeUsuario.Text = "";
            txtNome.Text = "";
            txtEmail.Text = "";
            mtxCPF.Text = "";
            dtpDataNasc.Value = DateTime.Today;
            mtxTelefone.Text = "";
            txtTurma.Text = "";
            lblTipoUsuario.Text = "";
            _usuarioSelecionado = null;
            lblTipoUsuario.Visible = false;
            lblTipoUsuario.Text = "";
        }

        
        private void lstSugestoesUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSugestoesUsuario.SelectedIndex >= 0)
            {
                SelecionarUsuario(lstSugestoesUsuario.SelectedIndex);
            }
        }



        private void btnBuscarUsuario_Click(object sender, EventArgs e)
        {
            lstSugestoesUsuario.Items.Clear();
            lstSugestoesUsuario.Visible = false;
            _cacheUsuarios.Clear();

            string nomeBusca = txtNomeUsuario.Text.Trim();

            if (string.IsNullOrWhiteSpace(nomeBusca))
            {
                MessageBox.Show("Digite um nome para buscar.");
                return;
            }

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT * FROM usuarios WHERE Nome LIKE @nome ORDER BY Nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", nomeBusca + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var usuario = new Usuarios
                                {
                                    Id = (int)reader["Id"],
                                    Nome = reader["Nome"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    CPF = reader["CPF"].ToString(),
                                    DataNascimento = reader["DataNascimento"] != DBNull.Value ? Convert.ToDateTime(reader["DataNascimento"]) : DateTime.MinValue,
                                    Telefone = reader["Telefone"].ToString(),
                                    Turma = reader["Turma"].ToString(),
                                    TipoUsuario = reader["TipoUsuario"].ToString()
                                };

                                _cacheUsuarios.Add(usuario);
                                lstSugestoesUsuario.Items.Add(usuario); // Lembre do ToString() que mostra Nome - Turma
                            }
                        }
                    }

                }

                if (lstSugestoesUsuario.Items.Count > 0)
                {
                    lstSugestoesUsuario.Visible = true;
                    lstSugestoesUsuario.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Nenhum usuário encontrado com esse nome.");
                    lstSugestoesUsuario.Visible = false;
                    lstSugestoesUsuario.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na busca: " + ex.Message);
            }
        }



        private void txtNomeUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && lstSugestoesUsuario.Visible && lstSugestoesUsuario.Items.Count > 0)
            {
                lstSugestoesUsuario.Focus();
                lstSugestoesUsuario.SelectedIndex = 0;
            }
        }


        // Configurações de exibição para Edição de Usuário


        private void HabilitarCampos()
        {
            txtNome.Visible = true;
            txtEmail.Visible = true;
            txtTurma.Visible = true;
            lblTurma.Visible = true;
            lblEmail.Visible = true;
            lblNome.Visible = true;
            lblCPF.Visible = true;
            lblDataNasc.Visible = true;
            lblTelefone.Visible = true;
            mtxCPF.Visible = true;
            mtxTelefone.Visible = true;
            dtpDataNasc.Visible = true;
        }

        private void ConfigurarEdicaoParaBibliotecario()
        {
            HabilitarCampos();
            txtTurma.Visible = false;

          

            lblTurma.Visible = false;

           
        }

        private void ConfigurarEdicaoParaProfessor()
        {
            
            txtTurma.Visible = false;

           
            lblTurma.Visible=false;


        }

        private void ConfigurarEdicaoParaAluno()
        {
            HabilitarCampos();
        


         
        }

        private void ConfigurarEdicaoParaOutros()
        {
           
            txtEmail.Visible = false;
            txtTurma.Visible = false;


            lblTurma.Visible = false;
            lblEmail.Visible = false;

           
        }

        private void AplicarConfiguracaoEdicaoUsuario()
        {
            string tipo = _usuarioSelecionado.TipoUsuario;

            if (tipo.Equals("Bibliotecário(a)", StringComparison.OrdinalIgnoreCase))
                ConfigurarEdicaoParaBibliotecario();
            else if (tipo.Equals("Professor(a)", StringComparison.OrdinalIgnoreCase))
                ConfigurarEdicaoParaProfessor();
            else if (tipo.Equals("Outros", StringComparison.OrdinalIgnoreCase))
                ConfigurarEdicaoParaOutros();
            else
                ConfigurarEdicaoParaAluno();
        }

        private bool CpfJaExiste(string cpf, int usuarioIdAtual)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();
                string sql = "SELECT COUNT(*) FROM usuarios WHERE Cpf = @Cpf AND Id <> @IdAtual";

                using (var cmd = new SqlCeCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Cpf", cpf);
                    cmd.Parameters.AddWithValue("@IdAtual", usuarioIdAtual);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        private bool EmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }




        private void lblTipoUsuario_Click(object sender, EventArgs e)
        {

        }

        private void lblCPF_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    }
