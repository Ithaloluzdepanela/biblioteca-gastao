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
    /// <summary>
    /// Classe principal do programa que controla a inicialização e configuração da aplicação.
    /// </summary>
    /// <remarks>
    /// Esta classe é responsável por:
    /// - Verificação e validação de licenças
    /// - Apresentação dos termos de uso
    /// - Controle do ciclo de vida da aplicação
    /// - Gerenciamento de sessões de usuário
    /// - Configuração inicial do sistema
    /// </remarks>
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// Gerencia todo o fluxo de inicialização, desde a validação de licença até o loop principal da aplicação.
        /// </summary>
        /// <remarks>
        /// O fluxo de inicialização segue esta ordem:
        /// 1. Configuração do Windows Forms
        /// 2. Verificação da chave pública de licença
        /// 3. Validação da licença ativa
        /// 4. Apresentação dos termos de uso (se necessário)
        /// 5. Ativação da licença (se necessário)
        /// 6. Loop principal de login/logout
        /// 7. Backup automático (comentado, disponível para ativação)
        /// </remarks>
        [STAThread]
        static void Main()
        {
            // Configuração padrão do Windows Forms para aparência moderna
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #region Verificação de Licença

            // Verifica se a chave pública de validação de licenças existe
            string pubPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "publicKey.xml");
            if (!System.IO.File.Exists(pubPath))
            {
                // Exibe aviso sobre chave pública ausente
                var result = MessageBox.Show(
                    "Arquivo publicKey.xml não encontrado ao lado do executável.\n" +
                    "Sem a chave pública o app não poderá validar licenças.\n\n" +
                    "Coloque publicKey.xml na pasta do programa e reinicie.\n\n" +
                    "Deseja abrir a pasta do programa agora para colar o arquivo?",
                    "Atenção: publicKey.xml ausente",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Abre o diretório da aplicação para o usuário adicionar a chave
                    System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory);
                }

                // Encerra a aplicação se não há chave pública válida
                return;
            }

            #endregion

            #region Processo de Ativação

            LicenseData license;
            
            // Verifica se existe uma licença válida e se os termos foram aceitos
            if (!LicenseValidator.TryGetActivation(out license) || !license.TermsAccepted)
            {
                // Apresenta os termos de uso para aceitação
                using (var termsForm = new TermsForm())
                {
                    if (termsForm.ShowDialog() != DialogResult.OK || !termsForm.Accepted)
                        return; // Usuário não aceitou os termos
                }

                // Solicita ativação da licença
                using (var activationForm = new ActivationForm())
                {
                    if (activationForm.ShowDialog() != DialogResult.OK)
                        return; // Usuário cancelou a ativação
                }

                // Recarrega a licença após ativação
                LicenseValidator.TryGetActivation(out license);
            }

            // Verifica se a licença não expirou
            if (LicenseValidator.IsExpired(license))
            {
                MessageBox.Show("Sua licença expirou!", "Licença expirada", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #endregion

            #region Loop Principal da Aplicação

            // Loop principal que permite logout e novo login sem fechar a aplicação
            // Continua executando até que o usuário feche explicitamente o programa
            while (true)
            {
                using (LoginForm login = new LoginForm())
                {
                    // Exibe tela de login e aguarda autenticação
                    if (login.ShowDialog() == DialogResult.OK)
                    {
                        #region Backup Automático (Desativado)

                        // Sistema de backup diário automático
                        // Desativado por padrão - ative na versão final se necessário
                        // Inclui sincronização com Google Drive para backup em nuvem

                        /*
                        try
                        {
                            // Garante que as pastas necessárias existam
                            AppPaths.EnsureFolders();

                            // Configuração dos caminhos para backup
                            var caminhoSdf = Path.Combine(Application.StartupPath, "bibliotecaDB", "bibliotecaDB.sdf");
                            var registroPath = AppPaths.RegistroBackupFile;
                            var backuplocais = AppPaths.BackupCacheFolder;
                            var credentials = Path.Combine(Application.StartupPath, "credentials.json");

                            // Executa processo de backup
                            // 1. Reenvia backups pendentes que falharam anteriormente
                            // 2. Executa backup do dia atual
                            BibliotecaApp.Services.BackupDiario.ReenviarPendentes(AppPaths.AppDataFolder, backuplocais, credentials);
                            BibliotecaApp.Services.BackupDiario.Executar(caminhoSdf, registroPath, AppPaths.AppDataFolder, backuplocais, credentials);
                        }
                        catch (Exception ex)
                        {
                            // Em caso de erro no backup, avisa o usuário mas não interrompe a aplicação
                            MessageBox.Show("Erro ao tentar realizar backup automático:\n" + ex.Message,
                                "Backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        */

                        #endregion

                        // Login bem-sucedido - abre a interface principal
                        Application.Run(new MainForm());
                    }
                    else
                    {
                        // Usuário cancelou o login - encerra a aplicação
                        break;
                    }
                }
            }

            #endregion
        }
    }
}
