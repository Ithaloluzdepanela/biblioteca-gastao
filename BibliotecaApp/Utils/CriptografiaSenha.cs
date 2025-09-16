using System;
using System.Linq;
using System.Security.Cryptography;

namespace BibliotecaApp.Utils
{
    /// <summary>
    /// Classe utilitária para criptografia segura de senhas.
    /// Utiliza PBKDF2 (Password-Based Key Derivation Function 2) com salt aleatório
    /// para garantir segurança contra ataques de dicionário e rainbow tables.
    /// </summary>
    /// <remarks>
    /// Esta implementação segue as melhores práticas de segurança:
    /// - Salt aleatório único para cada senha
    /// - 10.000 iterações PBKDF2 para slow hashing
    /// - Hash de 256 bits (32 bytes) para maior segurança
    /// - Uso de RNGCryptoServiceProvider para geração criptograficamente segura
    /// </remarks>
    public static class CriptografiaSenha
    {
        #region Constantes

        /// <summary>
        /// Número de iterações para PBKDF2 (Password-Based Key Derivation Function)
        /// </summary>
        /// <remarks>
        /// 10.000 iterações proporcionam um bom equilíbrio entre segurança e performance.
        /// Este valor torna os ataques de força bruta significativamente mais lentos.
        /// </remarks>
        private const int ITERACOES_PBKDF2 = 10000;

        /// <summary>
        /// Tamanho do salt em bytes
        /// </summary>
        /// <remarks>
        /// 16 bytes (128 bits) é considerado suficiente para evitar colisões
        /// e garantir uniqueness para cada senha.
        /// </remarks>
        private const int TAMANHO_SALT = 16;

        /// <summary>
        /// Tamanho do hash resultante em bytes
        /// </summary>
        /// <remarks>
        /// 32 bytes (256 bits) proporciona alta segurança contra ataques
        /// de força bruta e colisões.
        /// </remarks>
        private const int TAMANHO_HASH = 32;

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Cria um hash seguro da senha fornecida junto com um salt aleatório.
        /// </summary>
        /// <param name="senha">Senha em texto plano a ser criptografada</param>
        /// <param name="hash">Hash resultante em Base64 (parâmetro de saída)</param>
        /// <param name="salt">Salt gerado em Base64 (parâmetro de saída)</param>
        /// <remarks>
        /// Este método gera um salt único e criptograficamente seguro para cada senha,
        /// garantindo que duas senhas iguais resultem em hashes diferentes.
        /// 
        /// O processo segue estes passos:
        /// 1. Gera salt aleatório de 16 bytes
        /// 2. Aplica PBKDF2 com 10.000 iterações
        /// 3. Retorna hash e salt em Base64
        /// 
        /// Armazene tanto o hash quanto o salt no banco de dados para verificação posterior.
        /// </remarks>
        /// <example>
        /// <code>
        /// string hash, salt;
        /// CriptografiaSenha.CriarHash("minhasenha123", out hash, out salt);
        /// 
        /// // Armazenar hash e salt no banco de dados
        /// usuario.SenhaHash = hash;
        /// usuario.SenhaSalt = salt;
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">Lançada quando senha é null</exception>
        /// <exception cref="ArgumentException">Lançada quando senha é vazia</exception>
        public static void CriarHash(string senha, out string hash, out string salt)
        {
            // Validação de entrada
            if (senha == null)
                throw new ArgumentNullException(nameof(senha), "Senha não pode ser null");
            
            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("Senha não pode ser vazia", nameof(senha));

            // Geração de salt criptograficamente seguro
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[TAMANHO_SALT];
                rng.GetBytes(saltBytes);
                salt = Convert.ToBase64String(saltBytes);

                // Geração do hash usando PBKDF2
                using (var pbkdf2 = new Rfc2898DeriveBytes(senha, saltBytes, ITERACOES_PBKDF2))
                {
                    byte[] hashBytes = pbkdf2.GetBytes(TAMANHO_HASH);
                    hash = Convert.ToBase64String(hashBytes);
                }
            }
        }

        /// <summary>
        /// Verifica se a senha digitada corresponde ao hash e salt armazenados.
        /// </summary>
        /// <param name="senhaDigitada">Senha em texto plano fornecida pelo usuário</param>
        /// <param name="hashSalvo">Hash da senha original armazenado no banco</param>
        /// <param name="saltSalvo">Salt da senha original armazenado no banco</param>
        /// <returns>True se a senha está correta, false caso contrário</returns>
        /// <remarks>
        /// Este método recria o hash da senha digitada usando o salt original
        /// e compara com o hash armazenado para verificar autenticidade.
        /// 
        /// O processo de verificação:
        /// 1. Decodifica o salt de Base64
        /// 2. Aplica PBKDF2 na senha digitada com o salt original
        /// 3. Compara o hash resultante com o hash armazenado
        /// 
        /// A comparação é feita de forma segura para evitar timing attacks.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Durante login
        /// string senhaDigitada = txtSenha.Text;
        /// string hashArmazenado = usuario.SenhaHash;
        /// string saltArmazenado = usuario.SenhaSalt;
        /// 
        /// if (CriptografiaSenha.VerificarSenha(senhaDigitada, hashArmazenado, saltArmazenado))
        /// {
        ///     // Login válido
        /// }
        /// else
        /// {
        ///     // Senha incorreta
        /// }
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">Lançada quando algum parâmetro é null</exception>
        /// <exception cref="FormatException">Lançada quando hash ou salt não são Base64 válidos</exception>
        public static bool VerificarSenha(string senhaDigitada, string hashSalvo, string saltSalvo)
        {
            // Validação de parâmetros
            if (senhaDigitada == null)
                throw new ArgumentNullException(nameof(senhaDigitada));
            if (hashSalvo == null)
                throw new ArgumentNullException(nameof(hashSalvo));
            if (saltSalvo == null)
                throw new ArgumentNullException(nameof(saltSalvo));

            try
            {
                // Decodifica o salt e recria o hash
                byte[] saltBytes = Convert.FromBase64String(saltSalvo);
                
                using (var pbkdf2 = new Rfc2898DeriveBytes(senhaDigitada, saltBytes, ITERACOES_PBKDF2))
                {
                    byte[] hashBytes = pbkdf2.GetBytes(TAMANHO_HASH);
                    string hashCalculado = Convert.ToBase64String(hashBytes);
                    
                    // Comparação segura para evitar timing attacks
                    return CompararStringsSeguro(hashCalculado, hashSalvo);
                }
            }
            catch (FormatException)
            {
                // Hash ou salt inválidos em Base64
                return false;
            }
            catch (Exception)
            {
                // Qualquer outro erro durante verificação
                return false;
            }
        }

        #endregion

        #region Métodos Auxiliares

        /// <summary>
        /// Compara duas strings de forma segura para evitar timing attacks.
        /// </summary>
        /// <param name="a">Primeira string</param>
        /// <param name="b">Segunda string</param>
        /// <returns>True se as strings são iguais</returns>
        /// <remarks>
        /// Este método compara cada caractere independentemente do resultado,
        /// garantindo que o tempo de execução seja constante e não revele
        /// informações sobre a similaridade das strings.
        /// </remarks>
        private static bool CompararStringsSeguro(string a, string b)
        {
            if (a.Length != b.Length)
                return false;

            int diferenca = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diferenca |= a[i] ^ b[i];
            }

            return diferenca == 0;
        }

        #endregion

        #region Métodos de Utilidade

        /// <summary>
        /// Gera uma senha temporária segura para recuperação de conta.
        /// </summary>
        /// <param name="tamanho">Tamanho da senha temporária (padrão: 12)</param>
        /// <returns>Senha temporária gerada</returns>
        /// <remarks>
        /// Gera uma senha com caracteres alfanuméricos seguros,
        /// útil para funcionalidades de "esqueci minha senha".
        /// </remarks>
        public static string GerarSenhaTemporaria(int tamanho = 12)
        {
            const string caracteres = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
            var senha = new char[tamanho];

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[tamanho];
                rng.GetBytes(randomBytes);

                for (int i = 0; i < tamanho; i++)
                {
                    senha[i] = caracteres[randomBytes[i] % caracteres.Length];
                }
            }

            return new string(senha);
        }

        /// <summary>
        /// Valida a força de uma senha baseada em critérios de segurança.
        /// </summary>
        /// <param name="senha">Senha a ser validada</param>
        /// <returns>Score de força de 0 (fraca) a 5 (muito forte)</returns>
        /// <remarks>
        /// Critérios avaliados:
        /// - Tamanho mínimo (8 caracteres)
        /// - Presença de letras minúsculas
        /// - Presença de letras maiúsculas
        /// - Presença de números
        /// - Presença de caracteres especiais
        /// </remarks>
        public static int AvaliarForcaSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
                return 0;

            int score = 0;

            // Tamanho mínimo
            if (senha.Length >= 8) score++;
            if (senha.Length >= 12) score++;

            // Tipos de caracteres
            if (senha.Any(char.IsLower)) score++;
            if (senha.Any(char.IsUpper)) score++;
            if (senha.Any(char.IsDigit)) score++;
            if (senha.Any(ch => !char.IsLetterOrDigit(ch))) score++;

            return Math.Min(5, score);
        }

        #endregion
    }
}
    