using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace BibliotecaApp.Utils
{
    public class LicenseData
    {
        public string Type { get; set; } // "User" ou "Developer"
        public string Key { get; set; }
        public string TargetMachineId { get; set; } // para User
        public DateTime DateIssued { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool TermsAccepted { get; set; }
        public string Signature { get; set; }
    }

    public static class LicenseValidator
    {
        private static readonly string activationFile =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                         "BibliotecaApp", "activation.dat");

        private const string PublicKeyFileName = "publicKey.xml";

        // fallback opcional (recomendo deixar vazio e usar always publicKey.xml ao lado do exe)
        private static readonly string EmbeddedPublicKey = @"<RSAKeyValue>...</RSAKeyValue>";

        private static string LoadPublicKeyXml()
        {
            try
            {
                string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
                string candidate = Path.Combine(exeFolder, PublicKeyFileName);
                if (File.Exists(candidate))
                {
                    return File.ReadAllText(candidate, Encoding.UTF8);
                }
            }
            catch
            {
                // ignore e usa fallback
            }
            return EmbeddedPublicKey;
        }

        /// <summary>
        /// Validação detalhada: carrega .lic, verifica assinatura, MachineId (se User) e expiração.
        /// Retorna true se válido; error contém motivo se inválido.
        /// </summary>
        public static bool ValidateLicenseFileDetailed(string filePath, out LicenseData license, out string error)
        {
            license = null;
            error = null;
            try
            {
                if (!File.Exists(filePath))
                {
                    error = "Arquivo de licença não encontrado.";
                    return false;
                }

                string json = File.ReadAllText(filePath, Encoding.UTF8);
                license = JsonConvert.DeserializeObject<LicenseData>(json);
                if (license == null)
                {
                    error = "Formato de licença inválido.";
                    return false;
                }

                if (string.IsNullOrEmpty(license.Signature))
                {
                    error = "Licença não contém campo 'Signature'.";
                    return false;
                }

                string signature = license.Signature;

                // O gerador serializa com Signature = "" antes de assinar, então fazemos o mesmo para verificar
                license.Signature = "";

                string jsonToVerify = JsonConvert.SerializeObject(license);

                string publicKeyXml = LoadPublicKeyXml();
                if (string.IsNullOrWhiteSpace(publicKeyXml) || publicKeyXml.Contains("..."))
                {
                    error = "Chave pública não encontrada ou inválida. Coloque publicKey.xml ao lado do executável.";
                    return false;
                }

                using (var rsa = RSA.Create())
                {
                    try
                    {
                        rsa.FromXmlString(publicKeyXml);
                    }
                    catch (Exception ex)
                    {
                        error = "Erro ao carregar publicKey.xml: " + ex.Message;
                        return false;
                    }

                    byte[] content = Encoding.UTF8.GetBytes(jsonToVerify);
                    byte[] sig;
                    try
                    {
                        sig = Convert.FromBase64String(signature);
                    }
                    catch
                    {
                        error = "Signature não é Base64 válida.";
                        return false;
                    }

                    bool ok;
                    try
                    {
                        ok = rsa.VerifyData(content, sig, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    }
                    catch (Exception ex)
                    {
                        error = "Erro ao verificar assinatura: " + ex.Message;
                        return false;
                    }

                    if (!ok)
                    {
                        error = "Assinatura inválida (licença corrompida ou chave pública diferente).";
                        return false;
                    }
                }

                // Se for User, verifica MachineGuid (sem fallback para MachineName)
                if (string.Equals(license.Type, "User", StringComparison.OrdinalIgnoreCase))
                {
                    string localMachine = GetMachineId();

                    if (string.IsNullOrWhiteSpace(license.TargetMachineId))
                    {
                        error = "Licença do tipo User não contém TargetMachineId.";
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(localMachine))
                    {
                        error = "Não foi possível obter o MachineGuid local.";
                        return false;
                    }

                    if (!string.Equals(localMachine.Trim(), license.TargetMachineId.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        error = "Licença é para outra máquina (TargetMachineId não confere).";
                        return false;
                    }
                }

                // Expiração: DateTime.MaxValue = permanente
                if (license.ExpireDate != DateTime.MaxValue && license.ExpireDate < DateTime.Now)
                {
                    error = "Licença expirou em " + license.ExpireDate.ToString("dd/MM/yyyy") + ".";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                license = null;
                error = "Erro ao validar licença: " + ex.Message;
                return false;
            }
        }

        public static void SaveActivation(LicenseData license)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(activationFile));
            string json = JsonConvert.SerializeObject(license);
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(activationFile, encrypted);
        }

        public static bool TryGetActivation(out LicenseData license)
        {
            license = null;
            try
            {
                if (!File.Exists(activationFile)) return false;
                byte[] encrypted = File.ReadAllBytes(activationFile);
                byte[] data = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
                string json = Encoding.UTF8.GetString(data);
                license = JsonConvert.DeserializeObject<LicenseData>(json);
                return license != null;
            }
            catch
            {
                license = null;
                return false;
            }
        }

        public static bool IsExpired(LicenseData license)
        {
            if (license == null) return true;
            if (license.ExpireDate == DateTime.MaxValue) return false;
            return license.ExpireDate < DateTime.Now;
        }

        // Retorna estritamente o MachineGuid do Windows (sem hash e sem fallback para MachineName).
        public static string GetMachineId()
        {
            try
            {
                var guid = TryReadMachineGuid();
                return string.IsNullOrWhiteSpace(guid) ? null : guid.Trim();
            }
            catch
            {
                return null;
            }
        }

        private static string TryReadMachineGuid()
        {
            // Tenta primeiro no view de 64 bits (em SO 64 bits), depois no de 32 bits
            var guid = ReadMachineGuid(RegistryView.Registry64);
            if (string.IsNullOrWhiteSpace(guid))
                guid = ReadMachineGuid(RegistryView.Registry32);
            return guid;
        }

        private static string ReadMachineGuid(RegistryView view)
        {
            try
            {
                using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view))
                using (var key = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography"))
                {
                    var val = key?.GetValue("MachineGuid") as string;
                    return string.IsNullOrWhiteSpace(val) ? null : val;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
