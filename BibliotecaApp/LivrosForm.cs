using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
                    string query = "SELECT * FROM livros WHERE 1=1";

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
                            query += " AND disponibilidade = '1'";
                        else if (status == "Indisponíveis")
                            query += " AND disponibilidade = '0'";
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

                if (valor == "0")
                {
                    // Destaca a linha toda se indisponível
                    Lista.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    Lista.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
                    Lista.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(Lista.Font, FontStyle.Italic);
                }
                else if (valor == "1")
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

        #region Ação: Abrir Formulário de Reservas

        private void picReserva_Click(object sender, EventArgs e)
        {
            ReservaForm popup = new ReservaForm();
            Location = popup.Location;
            popup.ShowDialog();
        }

        #endregion

        #region Ação: Carregar tabelas e exibir campos

        private void btnCarregarTabelas_Click(object sender, EventArgs e)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();

                    // Listar tabelas
                    SqlCeCommand cmdTabelas = new SqlCeCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES", conexao);
                    SqlCeDataReader leitorTabelas = cmdTabelas.ExecuteReader();

                    lstTabelas.Items.Clear();
                    while (leitorTabelas.Read())
                    {
                        lstTabelas.Items.Add(leitorTabelas["TABLE_NAME"].ToString());
                    }
                    leitorTabelas.Close();

                    // Exibir campos da primeira tabela (se houver)
                    if (lstTabelas.Items.Count > 0)
                    {
                        string tabela = lstTabelas.Items[0].ToString();
                        ExibirCamposTabela(tabela);
                        lstTabelas.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao inspecionar banco: " + ex.Message);
                }
            }
        }

        #endregion

        #region Ação: Exibir campos ao selecionar tabela

        private void lstTabelas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTabelas.SelectedItem != null)
            {
                string tabelaSelecionada = lstTabelas.SelectedItem.ToString();
                ExibirCamposTabela(tabelaSelecionada);
            }
        }

        private void ExibirCamposTabela(string nomeTabela)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();

                    string query = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @nome";
                    SqlCeCommand comando = new SqlCeCommand(query, conexao);
                    comando.Parameters.AddWithValue("@nome", nomeTabela);

                    SqlCeDataReader leitor = comando.ExecuteReader();
                    lvCampos.Items.Clear();

                    while (leitor.Read())
                    {
                        ListViewItem item = new ListViewItem(leitor["COLUMN_NAME"].ToString());
                        item.SubItems.Add(leitor["DATA_TYPE"].ToString());
                        lvCampos.Items.Add(item);
                    }

                    leitor.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao listar campos: " + ex.Message);
                }
            }
        }

        #endregion

        #region Ação: Configurar visual do ListView no carregamento do Formulário

        private void LivrosForm_Load(object sender, EventArgs e)
        {
            lvCampos.View = View.Details;
            lvCampos.Columns.Clear();
            lvCampos.Columns.Add("Coluna", 150);
            lvCampos.Columns.Add("Tipo de Dado", 100);
        }


        #endregion

   
        #region Ação: Criar Tabela de Livros (Desabilitado)

        private void btnCriarTablea_Click(object sender, EventArgs e)
        {
            //    Essa região está comentada porque a tabela já Foi criada
            //     Caso precise criar novamente, descomente e execute

            //        SqlCeConnection conexao = Conexao.ObterConexao();

            //        try
            //        {
            //            conexao.Open();

            //            SqlCeCommand comando = new SqlCeCommand();
            //            comando.Connection = conexao;

            //            comando.CommandText =
            //@"CREATE TABLE Livros (
            //            Id INT IDENTITY(1,1) PRIMARY KEY,
            //            Nome NVARCHAR(80) NOT NULL,
            //            Autor NVARCHAR(80) NOT NULL,
            //            Genero NVARCHAR(30) NOT NULL,
            //            Quantidade INT NOT NULL DEFAULT 0,
            //            CodigoBarras NVARCHAR(13) NOT NULL UNIQUE,
            //            Disponibilidade BIT NOT NULL DEFAULT 1
            //        );";



            //            comando.ExecuteNonQuery();
            //            lblTeste.Text = "Tabela criada com sucesso!";
            //        }
            //        catch (Exception ex)
            //        {
            //            lblTeste.Text = $"Erro: {ex.Message}";
            //        }
            //        finally
            //        {
            //            conexao.Close();
            //        }
        }


        #endregion


        #region Ação: Criar Tabela Emprestimo (Desabilitado)
//private void btnCriarEmprestimo_Click(object sender, EventArgs e)
//{
//        using (SqlCeConnection conexao = Conexao.ObterConexao())
//        {
//            try
//            {
//                conexao.Open();

//                string sql = @"
//CREATE TABLE Emprestimo (
//    Id INT IDENTITY(1,1) PRIMARY KEY,
//    Alocador INT NOT NULL,
//    Livro INT NOT NULL,
//    Responsavel INT NOT NULL,
//    DataEmprestimo DATETIME NOT NULL,
//    DataDevolucao DATETIME NOT NULL,
//    DataProrrogacao DATETIME NULL,
//    DataRealDevolucao DATETIME NULL,
//    Status NVARCHAR(15) NOT NULL
//);";


//                SqlCeCommand comando = new SqlCeCommand(sql, conexao);
//                comando.ExecuteNonQuery();
//
//                MessageBox.Show("Tabela 'Emprestimo' criada com sucesso!");
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Erro ao criar tabela: " + ex.Message);
//            }
//        }
//}
#endregion



        #region Criar Banco de Dados (Desabilitado)
       
        #endregion

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void lstTabelas_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void lvCampos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnCriarTablea_Click_1(object sender, EventArgs e)
        {

        }

        //private void btnCriarTablea_Click_1(object sender, EventArgs e)
        //{
        //try
        //  {
        //  using (SqlCeConnection conexao = Conexao.ObterConexao())
        //   {
        //         conexao.Open();
        //
        //                 // Verifica se a tabela já existe
        //               bool tabelaExiste = VerificarTabelaExistente(conexao, "Reservas");
        //
        //               if (tabelaExiste)
        //                {
        //                   DialogResult resposta = MessageBox.Show("A tabela 'Reservas' já existe. Deseja recriá-la? Todos os dados serão perdidos.",
        //                       "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        //
        //                  if (resposta != DialogResult.Yes)
        //                  {
        //                      return;
        //                 }
        //
        //                // Remove a tabela existente
        //                using (SqlCeCommand comando = new SqlCeCommand("DROP TABLE Reservas", conexao))
        //                {
        //                    comando.ExecuteNonQuery();
        //              }
        //         }
        //
        // Cria a nova tabela
        //             string sql = @"
        //       CREATE TABLE Reservas (
        //            Id int IDENTITY(1,1) PRIMARY KEY,
        //           UsuarioId int NOT NULL,
        //          LivroId int NOT NULL,
        //         BibliotecariaId int NOT NULL,
        //          DataReserva datetime NOT NULL,
        //         DataDisponibilidade datetime NULL,
        //        DataLimiteRetirada datetime NULL,
        //        Status nvarchar(20) NOT NULL, -- 'Pendente', 'Disponível', 'Cancelada', 'Concluída', 'Expirada'
        //       FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
        //       FOREIGN KEY (LivroId) REFERENCES Livros(Id),
        //       FOREIGN KEY (BibliotecariaId) REFERENCES Usuarios(Id)
        //    )";
        //
        //       using (SqlCeCommand comando = new SqlCeCommand(sql, conexao))
        //     {
        //         comando.ExecuteNonQuery();
        //        MessageBox.Show("Tabela 'Reservas' criada com sucesso!", "Sucesso",
        //            MessageBoxButtons.OK, MessageBoxIcon.Information);
        //  }
        // }
        // }
        //  catch (Exception ex)
        //  {
        //      MessageBox.Show($"Erro ao criar tabela:\n{ex.Message}", "Erro",
        //         MessageBoxButtons.OK, MessageBoxIcon.Error);
        //  }
        //   }


        //private void LimparDadosBanco()
        //{
        //    try
        //    {
        //        using (SqlCeConnection conexao = Conexao.ObterConexao())
        //        {
        //            conexao.Open();

        //            // Lista das tabelas que quer limpar (adicione todas que desejar)
        //            string[] tabelas = { "Reservas", "Emprestimo", "usuarios", "Livros" };

        //            foreach (var tabela in tabelas)
        //            {
        //                string sqlDelete = $"DELETE FROM {tabela}";
        //                using (SqlCeCommand comando = new SqlCeCommand(sqlDelete, conexao))
        //                {
        //                    comando.ExecuteNonQuery();
        //                }
        //            }

        //            MessageBox.Show("Dados do banco limpos com sucesso!", "Sucesso",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Erro ao limpar dados do banco:\n{ex.Message}", "Erro",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        }


        //private void AtualizarEstruturaUsuarios()
        //{
        //    using (var conexao = Conexao.ObterConexao())
        //    {
        //        try
        //        {
        //            conexao.Open();

        //            // Verifica e adiciona senha_hash se não existir
        //            using (var checkCmd = new SqlCeCommand(
        //                "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'usuarios' AND COLUMN_NAME = 'Senha_hash'", conexao))
        //            {
        //                var exists = checkCmd.ExecuteScalar();
        //                if (exists == null)
        //                {
        //                    var cmdHash = new SqlCeCommand("ALTER TABLE usuarios ADD Senha_hash NVARCHAR(64) NULL;", conexao);
        //                    cmdHash.ExecuteNonQuery();
        //                }
        //            }

        //            // Verifica e adiciona senha_salt se não existir
        //            using (var checkCmd = new SqlCeCommand(
        //                "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'usuarios' AND COLUMN_NAME = 'Senha_salt'", conexao))
        //            {
        //                var exists = checkCmd.ExecuteScalar();
        //                if (exists == null)
        //                {
        //                    var cmdSalt = new SqlCeCommand("ALTER TABLE usuarios ADD Senha_salt NVARCHAR(32) NULL;", conexao);
        //                    cmdSalt.ExecuteNonQuery();
        //                }
        //            }

        //            // Verifica e remove a coluna senha se existir
        //            using (var checkCmd = new SqlCeCommand(
        //                "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'usuarios' AND COLUMN_NAME = 'Senha'", conexao))
        //            {
        //                var exists = checkCmd.ExecuteScalar();
        //                if (exists != null)
        //                {
        //                    var dropCmd = new SqlCeCommand("ALTER TABLE usuarios DROP COLUMN Senha;", conexao);
        //                    dropCmd.ExecuteNonQuery();
        //                }
        //            }

        //            MessageBox.Show("Estrutura da tabela 'usuarios' atualizada com sucesso!", "Sucesso",
        //                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Erro ao atualizar tabela 'usuarios': " + ex.Message, "Erro",
        //                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //        finally
        //        {
        //            conexao.Close();
        //        }
        //    }
        //}

    }


                                


