using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp
{
    public partial class LivrosForm : Form
    {
        #region Inicialização

        public LivrosForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Conexão com o Banco

        // Classe auxiliar para centralizar a conexão com o banco
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

        #region Normalização de Texto

        // Remove acentos e transforma o texto em minúsculo para busca inteligente
        private string NormalizarTexto(string texto)
        {
            string semAcento = new string(
                texto.Normalize(NormalizationForm.FormD)
                     .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                     .ToArray()
            );
            return semAcento.ToLower();
        }

        #endregion

        #region Ação: Abrir formulário de cadastro

        private void Pic_Cadastrar_Click(object sender, EventArgs e)
        {
            CadastroLivroForm popup = new CadastroLivroForm();
            Location = popup.Location;
            popup.ShowDialog();
        }

        #endregion

        #region Ação: Criar tabela de livros (desabilitado)

        private void btnCriarTablea_Click(object sender, EventArgs e)
        {
            // Essa região está comentada porque a tabela já deve existir
            // Caso precise criar novamente, descomente e execute

            /*
            SqlCeConnection conexao = Conexao.ObterConexao();

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText =
                @"CREATE TABLE livro (
                    id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                    nome NVARCHAR(50) NOT NULL,
                    autor NVARCHAR(40) NOT NULL,
                    genero NVARCHAR(20) NOT NULL,
                    quantidade INT NOT NULL DEFAULT 0,
                    codigoBarras NVARCHAR(13) NOT NULL UNIQUE,
                    disponibilidade NCHAR(1) NOT NULL
                )";

                comando.ExecuteNonQuery();
                lblTeste.Text = "Tabela criada com sucesso!";
            }
            catch (Exception ex)
            {
                lblTeste.Text = $"Erro: {ex.Message}";
            }
            finally
            {
                conexao.Close();
            }
            */
        }

        #endregion

        #region Ação: Buscar livros com filtro dinâmico

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();

                    // Define campo da pesquisa: nome, autor ou gênero
                    string campo = "nome"; // padrão
                    if (cbFiltro.SelectedItem != null)
                    {
                        string selecionado = cbFiltro.SelectedItem.ToString();
                        if (selecionado == "Autor") campo = "autor";
                        else if (selecionado == "Gênero") campo = "genero";
                    }

                    // Começa com consulta base
                    string query = "SELECT * FROM livro WHERE 1=1";

                    // Aplica filtro de texto se houver
                    if (!string.IsNullOrWhiteSpace(txtNome.Text))
                    {
                        query += $" AND {campo} LIKE @termo";
                    }

                    // Aplica filtro de disponibilidade
                    if (cbDisponibilidade.SelectedItem != null)
                    {
                        string status = cbDisponibilidade.SelectedItem.ToString();
                        if (status == "Disponíveis")
                            query += " AND disponibilidade = 'Disponivel'";
                        else if (status == "Indisponíveis")
                            query += " AND disponibilidade = 'Indisponivel'";
                    }

                    // Ordena por nome
                    query += " ORDER BY nome ASC";

                    using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                    {
                        if (!string.IsNullOrWhiteSpace(txtNome.Text))
                        {
                            comando.Parameters.AddWithValue("@termo", "%" + txtNome.Text.Trim() + "%");
                        }

                        SqlCeDataAdapter adaptador = new SqlCeDataAdapter(comando);
                        DataTable tabela = new DataTable();
                        adaptador.Fill(tabela);

                        Lista.AutoGenerateColumns = true;
                        Lista.DataSource = tabela;

                        lblTotal.Text = $"Total de livros encontrados: {tabela.Rows.Count}";
                    }
                }
                catch (Exception ex)
                {
                    Lista.DataSource = null;
                    MessageBox.Show("Erro ao procurar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        #endregion

        #region Ação: Abrir formulário de Devolução
        private void btnDevolução_Click(object sender, EventArgs e)
        {

            DevoluçãoForm poup = new DevoluçãoForm();
            Location = poup.Location;
            poup.ShowDialog();
        }
        #endregion


        private void Lista_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Lista.Columns[e.ColumnIndex].Name == "disponibilidade" && e.Value != null)
            {
                string valor = e.Value.ToString();

                if (valor == "Indisponivel")
                {
                    // Destaca a linha toda se indisponível
                    Lista.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    Lista.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
                    Lista.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(Lista.Font, FontStyle.Italic);
                }
                else if (valor == "Disponivel")
                {
                    // (Opcional) estilo para disponível
                    Lista.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    Lista.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }

        }

        private void picEmprestimo_Click(object sender, EventArgs e)
        {
            EmprestimoForm popup = new EmprestimoForm();
            Location = popup.Location;
            popup.ShowDialog();
        
        }
    }
}
