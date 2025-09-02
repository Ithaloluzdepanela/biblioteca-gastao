using System;
using System.Collections.Generic;
using System.Data.SqlServerCe; // SQL Server Compact Edition
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Livros
{
    public partial class CadastroLivroForm : Form
    {
        #region Inicialização do Formulário

        public CadastroLivroForm()
        {
            InitializeComponent();                 // Inicializa os controles visuais
            this.StartPosition = FormStartPosition.CenterScreen; // Abre no centro da tela
            
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

        #region Métodos Utilitários

        // Método que limpa todos os campos do formulário
        private void LimparTabela()
        {
            txtNome.Text = "";
            txtAutor.Text = "";
            txtGenero.Text = "";
            txtQuantidade.Text = "";
            mtxCodigoBarras.Text = "";
            txtNome.Focus(); // Foco volta para o primeiro campo
        }

        // Método que extrai somente os números do código de barras
        private string ObterCodigoDeBarrasFormatado()
        {
            return new string(mtxCodigoBarras.Text.Where(char.IsDigit).ToArray());
        }

        #endregion

        #region Evento: Botão "Limpar"

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            // Confirma antes de limpar os campos
            DialogResult resultado = MessageBox.Show(
                "Tem certeza de que deseja limpar tudo?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                LimparTabela(); // Se sim, limpa todos os campos
            }
        }

        #endregion

        #region Evento: Botão "Cadastrar"

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            // ✅ Validação de preenchimento obrigatório
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtAutor.Text) ||
                string.IsNullOrWhiteSpace(txtGenero.Text) ||
                string.IsNullOrWhiteSpace(txtQuantidade.Text) ||
                string.IsNullOrWhiteSpace(mtxCodigoBarras.Text.Replace(" ", "")))
            {
                MessageBox.Show("Por favor, preencha todos os campos antes de cadastrar.",
                                "Campos obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔢 Verifica se a quantidade é um número inteiro
            if (!int.TryParse(txtQuantidade.Text, out int quantidade))
            {
                MessageBox.Show("Por favor, insira apenas números no campo 'Quantidade'.",
                                "Erro de formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 📌 Remove máscara e valida o código de barras
            string codigoDeBarras = ObterCodigoDeBarrasFormatado();

            if (codigoDeBarras.Length != 13)
            {
                MessageBox.Show("O código de barras deve conter exatamente 13 dígitos.",
                                "Código inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🗄️ Insere o novo livro na tabela atualizada
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();

                    using (SqlCeCommand comando = conexao.CreateCommand())
                    {
                        comando.CommandText = @"INSERT INTO Livros 
                (Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade)
                VALUES
                (@Nome, @Autor, @Genero, @Quantidade, @CodigoBarras, @Disponibilidade)";

                        comando.Parameters.AddWithValue("@Nome", txtNome.Text);
                        comando.Parameters.AddWithValue("@Autor", txtAutor.Text);
                        comando.Parameters.AddWithValue("@Genero", txtGenero.Text);
                        comando.Parameters.AddWithValue("@Quantidade", quantidade);
                        comando.Parameters.AddWithValue("@CodigoBarras", codigoDeBarras);
                        comando.Parameters.AddWithValue("@Disponibilidade", 1); // Cadastra como disponível

                        comando.ExecuteNonQuery();
                    }

                    MessageBox.Show("Livro salvo com sucesso!",
                                    "Cadastro realizado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimparTabela(); // Limpa campos após o cadastro
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao cadastrar livro: {ex.Message}",
                                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        #endregion

        private void CadastroLivroForm_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }

    }
}
