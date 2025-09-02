using BibliotecaApp.Forms.Inicio;
using BibliotecaApp.Forms.Login;
using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BibliotecaApp.Utils;

namespace BibliotecaApp
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new LivrosForm());
            //Repetição para quando clicar no logout reiniciar a aplicação
            while (true)
            {
                using (LoginForm login = new LoginForm())
                {
                    if (login.ShowDialog() == DialogResult.OK)
                    {

                        // ---- Backup diario desativado / ative na versao final juntamente com o relatorio para secretaria //

                        //try
                        //{
                        //    AppPaths.EnsureFolders();

                        //var caminhoSdf = Path.Combine(Application.StartupPath, "bibliotecaDB", "bibliotecaDB.sdf"); // não movemos o DB
                        //var registroPath = AppPaths.RegistroBackupFile;
                        //var backuplocais = AppPaths.BackupCacheFolder;
                        //var credentials = Path.Combine(Application.StartupPath, "credentials.json"); // onde vc guarda credenciais

                        //// Reenvia backups pendentes e executa backup de hoje (bloqueante)
                        //BibliotecaApp.Services.BackupDiario.ReenviarPendentes(AppPaths.AppDataFolder, backuplocais, credentials);
                        //BibliotecaApp.Services.BackupDiario.Executar(caminhoSdf, registroPath, AppPaths.AppDataFolder, backuplocais, credentials);
                        //}
                        //catch (Exception ex)
                        //{
                        //    MessageBox.Show("Erro ao tentar realizar backup automático:\n" + ex.Message,
                        //        "Backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //}

                        Application.Run(new MainForm());
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
