using System;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace BibliotecaApp.Utils
{
    public static class Conexao
    {
        public static string CaminhoBanco => Application.StartupPath + @"\bibliotecaDB\bibliotecaDB.sdf";

        public static string Conectar
        {
            get
            {
                var connStrTemplate = ConfigurationManager.ConnectionStrings["BibliotecaDB"]?.ConnectionString;
                if (string.IsNullOrWhiteSpace(connStrTemplate))
                    throw new InvalidOperationException("ConnectionString 'BibliotecaDB' não encontrada em App.config.");

                var senha = GetDatabasePasswordProtected();
                if (string.IsNullOrEmpty(senha))
                    throw new InvalidOperationException("Senha do banco não encontrada em AppSettings['DBPasswordProtected'].");

                var connStr = connStrTemplate.Replace("CAMINHO", CaminhoBanco).Replace("{PASSWORD}", senha);
                return connStr;
            }
        }

        public static SqlCeConnection ObterConexao()
        {
            return new SqlCeConnection(Conectar);
        }

        // Lê AppSettings["DBPasswordProtected"], decodifica Base64 e descriptografa com DPAPI (escopo máquina)
        private static string GetDatabasePasswordProtected()
        {
            try
            {
                var protectedBase64 = ConfigurationManager.AppSettings["DBPasswordProtected"];
                if (string.IsNullOrWhiteSpace(protectedBase64))
                    return null;

                var protectedBytes = Convert.FromBase64String(protectedBase64);
                var plainBytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.LocalMachine);
                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("DBPasswordProtected não é um Base64 válido.");
            }
            catch (CryptographicException ex)
            {
                throw new InvalidOperationException("Falha ao descriptografar DBPasswordProtected (DPAPI). Verifique se o valor foi gerado nesta máquina.", ex);
            }
        }

   
        public static string ProtectPasswordForConfig(string clearPassword)
        {
            if (clearPassword == null) throw new ArgumentNullException(nameof(clearPassword));
            var bytes = Encoding.UTF8.GetBytes(clearPassword);
            var protectedBytes = ProtectedData.Protect(bytes, null, DataProtectionScope.LocalMachine);
            return Convert.ToBase64String(protectedBytes);
        }
    }
}

