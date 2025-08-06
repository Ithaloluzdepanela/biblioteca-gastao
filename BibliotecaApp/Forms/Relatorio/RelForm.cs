using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace BibliotecaApp.Forms.Relatorio
{

    public partial class RelForm : Form
    {

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
        public RelForm()
        {
            InitializeComponent();
        }

        private void btnEmprestimos_Click(object sender, EventArgs e)
        {

        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {

            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();


                    string query = @"SELECT * FROM Emprestimo";
                    if (txtIdLivro.Text != "")
                    {
                        query = @"SELECT * FROM Emprestimo WHERE LivroID LIKE '" + txtIdLivro.Text + "'";
                    }
                    DataTable dados = new DataTable();
                    SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, conexao);

                    conexao.Open();

                    adaptador.Fill(dados);

                    foreach (DataRow linha in dados.Rows)
                    {
                        lista.Rows.Add(linha.ItemArray);
                    }
                }

                catch (Exception ex)
                {
                    lista.Rows.Clear();
                    lblResultado.Text = $"Erro: {ex.Message}";
                }
                finally
                {
                    conexao.Close();
                }
                txtIdLivro.Text = "";
                txtIdUsuario.Text = "";
            }
        }

        private void RelForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pastaRelatorios = @"\BibliotecaApp\txt.relatorios";
            string arquivo = "txt.relatorios";
            if (!File.Exists(pastaRelatorios + arquivo))
            {
                File.Create(pastaRelatorios + arquivo).Close();
            }
            File.WriteAllText(pastaRelatorios + arquivo, "Teste de escrita em arquivo", Encoding.Default);
        }
    }
}
