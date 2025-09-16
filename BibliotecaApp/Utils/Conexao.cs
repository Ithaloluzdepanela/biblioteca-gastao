using System;
using System.Data.SqlServerCe;
using System.IO;
using System.Windows.Forms;

namespace BibliotecaApp.Utils
{
    /// <summary>
    /// Classe estática responsável pelo gerenciamento de conexões com o banco de dados.
    /// Utiliza SQL Server Compact Edition (SQLCE) para armazenamento local.
    /// </summary>
    /// <remarks>
    /// Esta classe centraliza toda a configuração de conexão com o banco,
    /// facilitando manutenção e garantindo consistência em todo o sistema.
    /// O banco utilizado é o SQL Server Compact Edition, ideal para aplicações desktop.
    /// </remarks>
    public static class Conexao
    {
        #region Constantes

        /// <summary>
        /// Nome do arquivo do banco de dados
        /// </summary>
        private const string NOME_ARQUIVO_BANCO = "bibliotecaDB.sdf";

        /// <summary>
        /// Nome da pasta que contém o banco de dados
        /// </summary>
        private const string PASTA_BANCO = "bibliotecaDB";

        /// <summary>
        /// Senha padrão do banco de dados
        /// </summary>
        /// <remarks>
        /// Em um ambiente de produção, esta senha deveria ser armazenada
        /// de forma mais segura, como em um arquivo de configuração criptografado
        /// </remarks>
        private const string SENHA_BANCO = "123";

        #endregion

        #region Propriedades

        /// <summary>
        /// Caminho completo para o arquivo de banco de dados .sdf
        /// </summary>
        /// <returns>Caminho absoluto para o arquivo bibliotecaDB.sdf</returns>
        /// <remarks>
        /// O banco é localizado na pasta da aplicação, em uma subpasta específica.
        /// Estrutura: [DiretórioDoExecutável]\bibliotecaDB\bibliotecaDB.sdf
        /// </remarks>
        public static string CaminhoBanco => Path.Combine(Application.StartupPath, PASTA_BANCO, NOME_ARQUIVO_BANCO);

        /// <summary>
        /// String de conexão completa com o banco de dados
        /// </summary>
        /// <returns>String de conexão formatada para SQL Server Compact Edition</returns>
        /// <remarks>
        /// Inclui o caminho do arquivo e a senha de acesso.
        /// Formato: "Data Source=[caminho]; Password=[senha]"
        /// </remarks>
        public static string Conectar => $"Data Source={CaminhoBanco}; Password={SENHA_BANCO}";

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Cria e retorna uma nova conexão com o banco de dados
        /// </summary>
        /// <returns>Nova instância de SqlCeConnection configurada e pronta para uso</returns>
        /// <remarks>
        /// Este método não abre a conexão automaticamente. É responsabilidade
        /// do código cliente chamar Open() na conexão retornada.
        /// Lembre-se de usar using ou dispose adequadamente para liberar recursos.
        /// </remarks>
        /// <example>
        /// <code>
        /// using (var conexao = Conexao.ObterConexao())
        /// {
        ///     conexao.Open();
        ///     // Realizar operações com o banco
        /// } // Conexão é fechada automaticamente
        /// </code>
        /// </example>
        public static SqlCeConnection ObterConexao()
        {
            return new SqlCeConnection(Conectar);
        }

        /// <summary>
        /// Testa se a conexão com o banco de dados pode ser estabelecida
        /// </summary>
        /// <returns>True se a conexão foi estabelecida com sucesso, false caso contrário</returns>
        /// <remarks>
        /// Útil para verificar a integridade do banco antes de realizar operações.
        /// Este método abre e fecha a conexão automaticamente.
        /// </remarks>
        public static bool TestarConexao()
        {
            try
            {
                using (var conexao = ObterConexao())
                {
                    conexao.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica se o arquivo de banco de dados existe no local esperado
        /// </summary>
        /// <returns>True se o arquivo existe, false caso contrário</returns>
        /// <remarks>
        /// Este método apenas verifica a existência física do arquivo,
        /// não garante que o banco esteja íntegro ou acessível.
        /// </remarks>
        public static bool ArquivoBancoExiste()
        {
            return File.Exists(CaminhoBanco);
        }

        /// <summary>
        /// Obtém o tamanho do arquivo de banco em bytes
        /// </summary>
        /// <returns>Tamanho do arquivo em bytes, ou -1 se o arquivo não existir</returns>
        /// <remarks>
        /// Útil para monitoramento do crescimento do banco e planejamento de backup.
        /// SQL Server CE tem limite de 4GB por arquivo.
        /// </remarks>
        public static long ObterTamanhoBanco()
        {
            try
            {
                if (ArquivoBancoExiste())
                {
                    var fileInfo = new FileInfo(CaminhoBanco);
                    return fileInfo.Length;
                }
                return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Formata o tamanho do banco para exibição amigável
        /// </summary>
        /// <returns>String formatada com o tamanho (ex: "2.5 MB")</returns>
        public static string ObterTamanhoBancoFormatado()
        {
            var tamanhoBytes = ObterTamanhoBanco();
            
            if (tamanhoBytes < 0)
                return "Arquivo não encontrado";

            // Converte para unidades apropriadas
            if (tamanhoBytes < 1024)
                return $"{tamanhoBytes} bytes";
            
            if (tamanhoBytes < 1024 * 1024)
                return $"{tamanhoBytes / 1024.0:F1} KB";
            
            if (tamanhoBytes < 1024 * 1024 * 1024)
                return $"{tamanhoBytes / (1024.0 * 1024.0):F1} MB";
            
            return $"{tamanhoBytes / (1024.0 * 1024.0 * 1024.0):F1} GB";
        }

        #endregion

        #region Métodos de Diagnóstico

        /// <summary>
        /// Realiza diagnóstico completo da conectividade do banco
        /// </summary>
        /// <returns>Relatório de diagnóstico como string</returns>
        /// <remarks>
        /// Este método executa várias verificações para diagnosticar
        /// problemas de conectividade e fornecer informações úteis
        /// para troubleshooting.
        /// </remarks>
        public static string DiagnosticarBanco()
        {
            var relatorio = new System.Text.StringBuilder();
            relatorio.AppendLine("=== DIAGNÓSTICO DO BANCO DE DADOS ===");
            relatorio.AppendLine($"Data/Hora: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            relatorio.AppendLine();

            // Verificar caminho
            relatorio.AppendLine($"Caminho esperado: {CaminhoBanco}");
            
            // Verificar existência do arquivo
            var arquivoExiste = ArquivoBancoExiste();
            relatorio.AppendLine($"Arquivo existe: {(arquivoExiste ? "SIM" : "NÃO")}");

            if (arquivoExiste)
            {
                // Verificar tamanho
                relatorio.AppendLine($"Tamanho do arquivo: {ObterTamanhoBancoFormatado()}");

                // Testar conexão
                var conexaoOk = TestarConexao();
                relatorio.AppendLine($"Conexão OK: {(conexaoOk ? "SIM" : "NÃO")}");

                if (!conexaoOk)
                {
                    relatorio.AppendLine("ERRO: Não foi possível conectar ao banco!");
                    relatorio.AppendLine("Possíveis causas:");
                    relatorio.AppendLine("- Arquivo corrompido");
                    relatorio.AppendLine("- Senha incorreta");
                    relatorio.AppendLine("- SQL Server CE não instalado");
                }
            }
            else
            {
                relatorio.AppendLine("ERRO: Arquivo de banco não encontrado!");
                relatorio.AppendLine("Possíveis causas:");
                relatorio.AppendLine("- Primeira execução (banco não criado)");
                relatorio.AppendLine("- Arquivo movido ou excluído");
                relatorio.AppendLine("- Problemas de permissão de acesso");
            }

            relatorio.AppendLine();
            relatorio.AppendLine("String de conexão utilizada:");
            relatorio.AppendLine(Conectar);

            return relatorio.ToString();
        }

        #endregion
    }
}

