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
        #region Inicialização do Formulário

        public LivrosForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Classe de Conexão com o Banco de Dados

        // Centraliza as configurações de conexão com o banco
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

        #region Função de Normalização de Texto

        // Remove acentos e converte para minúsculas para facilitar buscas
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

        #region Ação: Abrir Formulário de Cadastro

        private void Pic_Cadastrar_Click(object sender, EventArgs e)
        {
            CadastroLivroForm popup = new CadastroLivroForm();
            Location = popup.Location;
            popup.ShowDialog();
        }

        #endregion

        #region Ação: Criar Tabela de Livros (Desabilitado)

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

        #region Ação: Buscar Livros com Filtro Dinâmico

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
                            query += " AND disponibilidade = 'S'";
                        else if (status == "Indisponíveis")
                            query += " AND disponibilidade = 'N'";
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

                        //Gerar Itens da coluna
                        Lista.AutoGenerateColumns = true;
                        Lista.DataSource = tabela;

                        lblTotal.Text = $"Total de livros encontrados: {tabela.Rows.Count}";

                        //Ocultar a coluna Disponibilidade
                        if (Lista.Columns.Contains("disponibilidade"))
                        {
                            Lista.Columns["disponibilidade"].Visible = false;
                        }

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

        #region Ação: Abrir Formulário de Devolução

        private void btnDevolução_Click(object sender, EventArgs e)
        {
            DevoluçãoForm poup = new DevoluçãoForm();
            Location = poup.Location;
            poup.ShowDialog();
        }

        #endregion

        #region Estilização da Tabela de Livros

        private void Lista_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Lista.Columns[e.ColumnIndex].Name == "disponibilidade" && e.Value != null)
            {
                string valor = e.Value.ToString();

                if (valor == "N")
                {
                    // Destaca a linha toda se indisponível
                    Lista.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    Lista.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
                    Lista.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(Lista.Font, FontStyle.Italic);
                }
                else if (valor == "S")
                {
                    // Estilo para disponível
                    Lista.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    Lista.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        #endregion

        #region Ação: Abrir Formulário de Empréstimo

        private void picEmprestimo_Click(object sender, EventArgs e)
        {
            EmprestimoForm popup = new EmprestimoForm();
            Location = popup.Location;
            popup.ShowDialog();
        }

        #endregion

        #region Ação: Modificar tabela Livros (Desabilitado)
        private void button1_Click(object sender, EventArgs e)
        {


            // Essa região está comentada porque a tabela já Foi Criada
            // Caso precise criar novamente, descomente e execute


            SqlCeConnection conexao = Conexao.ObterConexao();

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "EXEC sp_rename 'livro', 'livros';";

                comando.ExecuteNonQuery();
                lblTeste.Text = "Tabela Modificada com sucesso!";
            }
            catch (Exception ex)
            {
                lblTeste.Text = $"Erro: {ex.Message}";
            }
            finally
            {
                conexao.Close();
            }



        }



        #endregion


        private void btnCarregarTabelas_Click(object sender, EventArgs e)
        {

            try
            {
                using (SqlCeConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // 👉 Listar tabelas
                    var cmdTabelas = new SqlCeCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES", conexao);
                    var leitorTabelas = cmdTabelas.ExecuteReader();
                    lstTabelas.Items.Clear();

                    while (leitorTabelas.Read())
                    {
                        lstTabelas.Items.Add(leitorTabelas["TABLE_NAME"].ToString());
                    }

                    leitorTabelas.Close();

                    // 👉 Se houver ao menos uma tabela, exibe seus campos
                    if (lstTabelas.Items.Count > 0)
                    {
                        string tabela = lstTabelas.Items[0].ToString();

                        var cmdCampos = new SqlCeCommand(
                            "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @nome",
                            conexao);
                        cmdCampos.Parameters.AddWithValue("@nome", tabela);

                        var leitorCampos = cmdCampos.ExecuteReader();
                        lvCampos.Items.Clear();

                        while (leitorCampos.Read())
                        {
                            ListViewItem item = new ListViewItem(leitorCampos["COLUMN_NAME"].ToString());
                            item.SubItems.Add(leitorCampos["DATA_TYPE"].ToString());
                            lvCampos.Items.Add(item);
                        }

                        leitorCampos.Close();
                        lstTabelas.SelectedIndex = 0; // marca visualmente
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao inspecionar banco: " + ex.Message);
            }
        }



        private void lvCampos_SelectedIndexChanged(object sender, EventArgs e)
        {
        //    if (lstTabelas.SelectedItem == null) return;

        //    string tabela = lstTabelas.SelectedItem.ToString();

        //    try
        //    {
        //        using (SqlCeConnection conexao = Conexao.ObterConexao())
        //        {
        //            conexao.Open();

        //            string query = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @nome";
        //            SqlCeCommand comando = new SqlCeCommand(query, conexao);
        //            comando.Parameters.AddWithValue("@nome", tabela);

        //            SqlCeDataReader leitor = comando.ExecuteReader();
        //            lvCampos.Items.Clear();

        //            while (leitor.Read())
        //            {
        //                ListViewItem item = new ListViewItem(leitor["COLUMN_NAME"].ToString());
        //                item.SubItems.Add(leitor["DATA_TYPE"].ToString());
        //                lvCampos.Items.Add(item);
        //            }

        //            leitor.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Erro ao listar campos: " + ex.Message);
        //    }
        }

        private void LivrosForm_Load(object sender, EventArgs e)
        {
            lvCampos.View = View.Details;
            lvCampos.Columns.Clear();
            lvCampos.Columns.Add("Coluna", 150);
            lvCampos.Columns.Add("Tipo de Dado", 100);

        }
    }
}