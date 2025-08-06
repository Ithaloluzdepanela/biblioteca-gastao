using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Utils
{
   
        public static class Conexao
        {

            // Caminho completo para o arquivo de banco de dados .sdf

            public static string CaminhoBanco => Application.StartupPath + @"\bibliotecaDB\bibliotecaDB.sdf";

            // String de conexão com senha para o banco de dados

            public static string Conectar => $"Data Source={CaminhoBanco}; Password=123";


            // Método para obter uma nova conexão com o banco de dados

            // <returns>Nova instância de SqlCeConnection configurada</returns>
            public static SqlCeConnection ObterConexao()
            {
                return new SqlCeConnection(Conectar);
            }
        }
    }

