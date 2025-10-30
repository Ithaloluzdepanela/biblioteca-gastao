using BibliotecaApp.Forms.Inicio;
using BibliotecaApp.Forms.Login;
using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BibliotecaApp
{
    internal static class Program
    {
        public static bool RequestLogout = false;

        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            string pubPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "publicKey.xml");
            if (!System.IO.File.Exists(pubPath))
            {
                var result = MessageBox.Show(
                    "Arquivo publicKey.xml não encontrado ao lado do executável.\n" +
                    "Sem a chave pública o app não poderá validar licenças.\n\n" +
                    "Coloque publicKey.xml na pasta do programa e reinicie.\n\n" +
                    "Deseja abrir a pasta do programa agora para colar o arquivo?",
                    "Atenção: publicKey.xml ausente",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory);
                }

                // opcional: parar a execução para evitar erros posteriores
                return;
            }

            LicenseData license;
            if (!LicenseValidator.TryGetActivation(out license) || !license.TermsAccepted)
            {
                using (var termsForm = new TermsForm())
                {
                    if (termsForm.ShowDialog() != DialogResult.OK || !termsForm.Accepted)
                        return;
                }

                using (var activationForm = new ActivationForm())
                {
                    if (activationForm.ShowDialog() != DialogResult.OK)
                        return;
                }

                // recarrega licença
                LicenseValidator.TryGetActivation(out license);
            }

            if (LicenseValidator.IsExpired(license))
            {
                MessageBox.Show("Sua licença expirou!", "Licença expirada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            //Application.Run(new LivrosForm());
            //Repetição para quando clicar no logout reiniciar a aplicação

            while (true)
            {
                using (LoginForm login = new LoginForm())
                {
                    if (login.ShowDialog() == DialogResult.OK)
                    {

                        // ---- Backup diario desativado / ative na versao final juntamente com o relatorio para secretaria em loginform //

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
