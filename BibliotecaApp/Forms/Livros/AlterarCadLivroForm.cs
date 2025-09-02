using System;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using BibliotecaApp.Utils;

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

        // Evento do botão Salvar - Atualiza os dados do livro no banco de dados

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                string nome = txtNome.Text.Trim();
                string autor = txtAutor.Text.Trim();
                string genero = txtGenero.Text.Trim();
                int quantidade = int.Parse(txtQuantidade.Text);
                string codigoBarras = mtxCodigoBarras.Text.Trim();

                using (SqlCeConnection conn = Conexao.ObterConexao())
                {
                    conn.Open();

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
                        cmd.Parameters.AddWithValue("@id", livroId); // Usa o ID armazenado

                        int linhasAfetadas = cmd.ExecuteNonQuery();

                        if (linhasAfetadas > 0)
                        {
                            MessageBox.Show("Livro atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // 🔔 Dispara o evento para avisar o LivrosForm
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
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar o livro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Evento do botão Excluir - Remove o livro do banco de dados

        private void btnExcluir_Click(object sender, EventArgs e)
        {
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

    }
}
