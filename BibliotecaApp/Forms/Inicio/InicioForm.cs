using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BibliotecaApp.Forms.Inicio
{
    public partial class InicioForm : Form
    {
        public InicioForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region Table usuarios
            string basedados = Application.StartupPath + @"\bibliotecaDB\bibliotecaDB.sdf";
            string conect = $@"Datasource = {basedados}; Password = '123'";

            SqlCeConnection conexao = new SqlCeConnection(conect);


            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = 
                @"CREATE TABLE usuarios(
                 id INT IDENTITY(1,1) PRIMARY KEY,
                 nome NVARCHAR(40) NOT NULL,
                 email NVARCHAR(30),
                 senha NVARCHAR(30) NOT NULL,
                 cpf NVARCHAR(14),
                 datanascimento DATETIME,
                 turma NVARCHAR(30),
                 telefone NVARCHAR(20),
                 tipousuario NVARCHAR(20))";
                comando.ExecuteNonQuery();

                lblResultado.Text = "Tabela criada";
                comando.Dispose();
            }
            catch (Exception ex)
            {
                lblResultado.Text = ex.Message;
            }
            finally
            {
                conexao.Close();

            }
            #endregion
        }

        private void lblResultado_Click(object sender, EventArgs e)
        {

        }

        private void InicioForm_Load(object sender, EventArgs e)
        {

        }
    }
}
