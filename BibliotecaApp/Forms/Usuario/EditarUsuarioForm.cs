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
         

            EstilizarListBoxSugestao(lstSugestoesUsuario);
            EstilizarListBoxSugestao(lstSugestoesUsuario);

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

                string email = txtEmail.Text.Trim();
                if (!string.IsNullOrEmpty(email) && !EmailValido(email))
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
            OnUsuarioSelecionado(true);

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
                HabilitarCampos();
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir: " + ex.Message);
            }
        }

        

        
        private void lstSugestoesUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSugestoesUsuario.SelectedIndex >= 0)
            {
                SelecionarUsuario(lstSugestoesUsuario.SelectedIndex);
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

        private void txtNomeUsuario_TextChanged(object sender, EventArgs e)
        {
            lstSugestoesUsuario.Items.Clear();
            lstSugestoesUsuario.Visible = false;
            _cacheUsuarios.Clear();

            string nomeBusca = txtNomeUsuario.Text.Trim();

            if (string.IsNullOrWhiteSpace(nomeBusca))
                return;

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
                                lstSugestoesUsuario.Items.Add(usuario); // ToString mostra Nome - Turma
                            }
                        }
                    }
                }
                lstSugestoesUsuario.Visible = lstSugestoesUsuario.Items.Count > 0;
                lstSugestoesUsuario.Enabled = lstSugestoesUsuario.Items.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na busca: " + ex.Message);
            }
        }

        // Configurações de exibição para Edição de Usuário


        private void HabilitarCampos()
        {
            txtNome.Visible = true;
            txtNome.Enabled = true;
            txtEmail.Visible = true;
            txtEmail.Enabled = true;
            txtTurma.Visible = true;
            txtTurma.Enabled = true;
            lblTurma.Visible = true;
            lblTurma.Enabled = true;
            lblEmail.Visible = true;
            lblEmail.Enabled = true;
            lblNome.Visible = true;
            lblNome.Enabled = true;
            lblCPF.Visible = true;
            lblCPF.Enabled = true;
            lblDataNasc.Visible = true;
            lblDataNasc.Enabled = true;
            lblTelefone.Visible = true;
            lblTelefone.Enabled = true;
            mtxCPF.Visible = true;
            mtxCPF.Enabled = true;
            mtxTelefone.Visible = true;
            mtxTelefone.Enabled = true;
            dtpDataNasc.Visible = true;
            dtpDataNasc.Enabled = true;
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



        private void HabilitarCampos(bool ativo)
        {
            txtNome.Enabled = ativo;
            txtEmail.Enabled = ativo;
            txtTurma.Enabled = ativo;
            mtxCPF.Enabled = ativo;
            mtxTelefone.Enabled = ativo;
            dtpDataNasc.Enabled = ativo;

            SetCamposColors(ativo);
            SetLabelColors(ativo);
        }

        private void SetCamposColors(bool enabled)
        {
            Color backColor = enabled ? Color.WhiteSmoke : Color.White;
            Color borderColor = enabled ? Color.FromArgb(204, 204, 204) : Color.LightGray;

            if (txtNome is RoundedTextBox rtbNome)
            {
                rtbNome.BackColor = backColor;
                rtbNome.BorderColor = borderColor;
            }
            if (txtEmail is RoundedTextBox rtbEmail)
            {
                rtbEmail.BackColor = backColor;
                rtbEmail.BorderColor = borderColor;
            }
            if (txtTurma is RoundedTextBox rtbTurma)
            {
                rtbTurma.BackColor = backColor;
                rtbTurma.BorderColor = borderColor;
            }
            if (mtxCPF is RoundedMaskedTextBox rmtxCPF)
            {
                rmtxCPF.BackColor = backColor;
                rmtxCPF.BorderColor = borderColor;
            }
            if (mtxTelefone is RoundedMaskedTextBox rmtxTel)
            {
                rmtxTel.BackColor = backColor;
                rmtxTel.BorderColor = borderColor;
            }
            dtpDataNasc.BackColor = backColor;
        }

        private void SetLabelColors(bool enabled)
        {
            Color color = enabled ? Color.FromArgb(20, 41, 60) : Color.LightGray;
            lblNome.ForeColor = color;
            lblEmail.ForeColor = color;
            lblTurma.ForeColor = color;
            lblCPF.ForeColor = color;
            lblDataNasc.ForeColor = color;
            lblTelefone.ForeColor = color;
        }

        // Chame este método quando um usuário for selecionado ou desmarcado
        private void OnUsuarioSelecionado(bool selecionado)
        {
            HabilitarCampos(selecionado);
        }

       
        

        // No método LimparCampos ou quando não houver seleção:
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
            OnUsuarioSelecionado(false);
        }

        public void PreencherUsuario(Usuarios usuario)
        {
            HabilitarCampos(true);

            _usuarioSelecionado = usuario;
            txtNomeUsuario.Text = usuario.Nome;
            txtNome.Text = usuario.Nome;
            txtEmail.Text = usuario.Email;
            mtxCPF.Text = usuario.CPF;
            dtpDataNasc.Value = usuario.DataNascimento == DateTime.MinValue ? DateTime.Today : usuario.DataNascimento;
            mtxTelefone.Text = usuario.Telefone;
            txtTurma.Text = usuario.Turma;
            lblTipoUsuario.Text = $"Tipo: {usuario.TipoUsuario}";
            lblTipoUsuario.Visible = true;
           

            AplicarConfiguracaoEdicaoUsuario();
            OnUsuarioSelecionado(true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Remove o foco de qualquer controle
            this.ActiveControl = null;

            // Garante que nenhum controle receba foco após o carregamento
            this.BeginInvoke(new Action(() => this.ActiveControl = null));

            // Desabilita campos inicialmente
            HabilitarCampos(false);
        }
    }
}
