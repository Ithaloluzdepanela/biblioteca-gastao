using System;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using BibliotecaApp.Utils;
using BibliotecaApp.Forms.Usuario;
using BibliotecaApp.Models;

namespace BibliotecaApp
{
    public partial class AlterarCadLivroForm : Form
    {
        #region Campos e Propriedades
        private int livroId; // Armazena o ID do livro para edição
        #endregion

        #region Eventos
        public event EventHandler LivroAtualizado;
        #endregion

        #region Construtor
        public AlterarCadLivroForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Métodos Públicos

        // Preenche os campos do formulário com os dados do livro selecionado

        public void PreencherLivro(Livro livro)
        {
            livroId = livro.Id; // Armazena o ID internamente

            txtNome.Text = livro.Nome;
            txtAutor.Text = livro.Autor;
            txtGenero.Text = livro.Genero;
            txtQuantidade.Text = livro.Quantidade.ToString();
            mtxCodigoBarras.Text = livro.CodigoDeBarras;
        }
        #endregion

        #region Eventos de Botões

        // Confirmação de senha para operações sensíveis
        private string ObterSenha(string titulo, string mensagem)
        {
            using (var passwordForm = new PasswordForm())
            {
                passwordForm.Titulo = titulo;
                passwordForm.Mensagem = mensagem;

                if (passwordForm.ShowDialog() == DialogResult.OK)
                {
                    return passwordForm.SenhaDigitada;
                }
            }
            return null;
        }

        private bool VerificarSenhaBibliotecaria(string senha)
        {
            string nomeBibliotecaria = Sessao.NomeBibliotecariaLogada;
            if (string.IsNullOrEmpty(nomeBibliotecaria))
            {
                MessageBox.Show("Nenhum bibliotecário está logado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string query = @"SELECT Senha_hash, Senha_salt FROM usuarios 
                             WHERE Nome = @nome AND TipoUsuario LIKE '%Bibliotec%'";

                    using (var comando = new SqlCeCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@nome", nomeBibliotecaria);
                        using (var reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hashSalvo = reader["Senha_hash"].ToString();
                                string saltSalvo = reader["Senha_salt"].ToString();

                                // Use a mesma classe de criptografia do login
                                return CriptografiaSenha.VerificarSenha(senha, hashSalvo, saltSalvo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar senha: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string autor = txtAutor.Text.Trim();
            string genero = txtGenero.Text.Trim();
            int quantidade = int.Parse(txtQuantidade.Text);
            string codigoBarras = mtxCodigoBarras.Text.Trim();

            bool houveAlteracao = false;
            string nomeAntigo = "", autorAntigo = "", generoAntigo = "", codigoBarrasAntigo = "";
            int quantidadeAntiga = 0;

            using (SqlCeConnection conn = Conexao.ObterConexao())
            {
                conn.Open();
                // Busca dados atuais do livro
                string queryLivro = "SELECT Nome, Autor, Genero, Quantidade, CodigoBarras FROM Livros WHERE Id = @id";
                using (SqlCeCommand cmdLivro = new SqlCeCommand(queryLivro, conn))
                {
                    cmdLivro.Parameters.AddWithValue("@id", livroId);
                    using (var reader = cmdLivro.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nomeAntigo = reader.GetString(0);
                            autorAntigo = reader.GetString(1);
                            generoAntigo = reader.GetString(2);
                            quantidadeAntiga = reader.GetInt32(3);
                            codigoBarrasAntigo = reader.GetString(4);
                            houveAlteracao = nome != nomeAntigo || autor != autorAntigo || genero != generoAntigo || quantidade != quantidadeAntiga || codigoBarras != codigoBarrasAntigo;
                        }
                    }
                }

                if (!houveAlteracao)
                {
                    MessageBox.Show("Nenhuma alteração foi feita.");
                    return;
                }

                // Exibe confirmação detalhada
                var mensagemConfirmacao = MontarMensagemConfirmacaoLivro(nomeAntigo, autorAntigo, generoAntigo, quantidadeAntiga, codigoBarrasAntigo,
                                                                         nome, autor, genero, quantidade, codigoBarras);
                var resultado = MessageBox.Show(mensagemConfirmacao, "Confirmar Alterações", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado != DialogResult.Yes)
                    return;

                // Atualiza o livro normalmente
                string query = @"UPDATE Livros 
                         SET Nome = @nome, Autor = @autor, Genero = @genero, 
                             Quantidade = @quantidade, CodigoBarras = @codigo 
                         WHERE Id = @id";
                using (SqlCeCommand cmd = new SqlCeCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@autor", autor);
                    cmd.Parameters.AddWithValue("@genero", genero);
                    cmd.Parameters.AddWithValue("@quantidade", quantidade);
                    cmd.Parameters.AddWithValue("@codigo", codigoBarras);
                    cmd.Parameters.AddWithValue("@id", livroId);
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    if (linhasAfetadas > 0)
                    {
                        MessageBox.Show("Livro atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LivroAtualizado?.Invoke(this, EventArgs.Empty);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Nenhuma alteração foi feita.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // Confirmação de senha dupla
            string senha1 = ObterSenha("Confirmação de Senha", "Digite sua senha:");
            if (string.IsNullOrEmpty(senha1))
            {
                MessageBox.Show("Operação cancelada.");
                return;
            }
            string senha2 = ObterSenha("Confirmação de Senha", "Digite sua senha novamente para confirmar:");
            if (string.IsNullOrEmpty(senha2))
            {
                MessageBox.Show("Operação cancelada.");
                return;
            }
            if (senha1 != senha2)
            {
                MessageBox.Show("As senhas não coincidem. Operação cancelada.");
                return;
            }
            if (!VerificarSenhaBibliotecaria(senha1))
            {
                MessageBox.Show("Senha incorreta. Operação cancelada.");
                return;
            }

            var confirm = MessageBox.Show("Tem certeza que deseja excluir este livro?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (var conn = Conexao.ObterConexao())
                    {
                        conn.Open();
                        string query = "DELETE FROM Livros WHERE Id = @id";

                        using (var cmd = new SqlCeCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", livroId);
                            int linhasAfetadas = cmd.ExecuteNonQuery();

                            if (linhasAfetadas > 0)
                            {
                                MessageBox.Show("Livro excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // 🔔 Dispara o evento para atualizar o grid no LivrosForm
                                LivroAtualizado?.Invoke(this, EventArgs.Empty);

                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Nenhum livro foi excluído.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao excluir o livro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Evento do botão Cancelar - Volta ao Form principal
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private string MontarMensagemConfirmacaoLivro(string nomeAntigo, string autorAntigo, string generoAntigo, int quantidadeAntiga, string codigoBarrasAntigo,
                                             string nomeNovo, string autorNovo, string generoNovo, int quantidadeNova, string codigoBarrasNovo)
        {
            var mensagem = new System.Text.StringBuilder();
            mensagem.AppendLine("Confirme as alterações a serem salvas:");
            mensagem.AppendLine();

            if (nomeAntigo != nomeNovo)
                mensagem.AppendLine($"Nome: {nomeAntigo} → {nomeNovo}");
            if (autorAntigo != autorNovo)
                mensagem.AppendLine($"Autor: {autorAntigo} → {autorNovo}");
            if (generoAntigo != generoNovo)
                mensagem.AppendLine($"Gênero: {generoAntigo} → {generoNovo}");
            if (quantidadeAntiga != quantidadeNova)
                mensagem.AppendLine($"Quantidade: {quantidadeAntiga} → {quantidadeNova}");
            if (codigoBarrasAntigo != codigoBarrasNovo)
                mensagem.AppendLine($"Código de Barras: {codigoBarrasAntigo} → {codigoBarrasNovo}");

            mensagem.AppendLine();
            mensagem.AppendLine("Deseja salvar estas alterações?");
            return mensagem.ToString();
        }
    }
}
