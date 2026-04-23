using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;

namespace BibliotecaApp.Utils
{
    public static class TurmasUtil
    {
        // --- Lista padronizada de turmas permitidas (adicione aqui outras que precise) ---
        private static List<string> _turmasPermitidas;
        public static List<string> TurmasPermitidas
        {
            get
            {
                if (_turmasPermitidas == null)
                    CarregarTurmas();
                return _turmasPermitidas;
            }
        }

        // Abreviações que os usuários podem digitar (mapeia pra forma curta usada na normalização)
        private static readonly Dictionary<string, string> Abreviacoes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "d", "desenv" }, { "des", "desenv" }, { "desenv", "desenv" },
            { "ag", "agroneg" }, { "agroneg", "agroneg" }, { "adm", "admin" },
            { "em", "em" }, { "reg", "reg" }, { "int", "int" }, { "ef", "ef" }, { "af", "af" },
            { "pro", "pro" }, { "eletro", "eletromec" }, { "sis", "sistemas" }
        };

        private static readonly string CaminhoTurmasJson = 
    Path.Combine(BibliotecaApp.Utils.AppPaths.MappingFolder, "turmas.json");

        /// <summary>
        /// Busca sugestões inteligentes a partir do texto digitado pelo usuário.
        /// Suporta variações como "2d", "2 d", "2 des", "2ºd", abreviações e erros leves de digitação.
        /// </summary>
        public static List<string> BuscarSugestoes(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return new List<string>();

            string entradaOriginal = texto.Trim();
            var variants = GerarVariantesEntrada(entradaOriginal);

            // Preprocessa turmas
            var tabelaTurmas = TurmasPermitidas
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(t => new
                {
                    Original = t,
                    Normalizada = NormalizarParaComparacao(t),
                    Restante = RemoverPrefixoNumeroENormalizar(t)
                })
                .ToList();

            // --- NOVO FILTRO ---
            var matchNumero = Regex.Match(entradaOriginal, @"^(\d+)");
            if (matchNumero.Success)
            {
                string numeroDigitado = matchNumero.Groups[1].Value + "º";
                tabelaTurmas = tabelaTurmas
                    .Where(t => t.Original.StartsWith(numeroDigitado, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var candidatos = new List<(string Turma, int Score)>();
            var vistos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var varEnt in variants)
            {
                string varNorm = NormalizarParaComparacao(varEnt);

                foreach (var t in tabelaTurmas)
                {
                    if (vistos.Contains(t.Original)) continue;

                    int score = CalcularScore(varNorm, t.Normalizada, t.Restante);
                    if (score < 100)
                    {
                        candidatos.Add((t.Original, score));
                        vistos.Add(t.Original);
                    }
                }
            }

            if (!candidatos.Any())
            {
                string varNorm = NormalizarParaComparacao(entradaOriginal);
                foreach (var t in tabelaTurmas)
                {
                    int score = CalcularScore(varNorm, t.Normalizada, t.Restante);
                    if (score < 100)
                        candidatos.Add((t.Original, score));
                }
            }

            return candidatos
                .OrderBy(c => c.Score)
                .ThenBy(c => c.Turma, StringComparer.OrdinalIgnoreCase)
                .Select(c => c.Turma)
                .ToList();
        }


        // --- Helpers privados ---

        // Gera variantes úteis a partir do que o usuário digitou:
        // ex: "2d" -> ["2d", "2 d", "2º d", "2ºd", "2 desenv"]
        private static IEnumerable<string> GerarVariantesEntrada(string entrada)
        {
            var list = new List<string>();
            if (string.IsNullOrWhiteSpace(entrada)) return list;

            var s = entrada.Trim();
            list.Add(s);

            // inserir formas com/sem espaço, com º, etc
            var m = Regex.Match(s, @"^(\d+)[°º]?\s*(.*)$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                var num = m.Groups[1].Value;
                var resto = (m.Groups[2].Value ?? "").Trim();

                list.Add($"{num}º {resto}".Trim());
                list.Add($"{num}º{resto}".Trim());
                list.Add($"{num} {resto}".Trim());
                list.Add($"{num}{resto}".Trim());

                // se o "resto" for uma abreviação conhecida, expandir
                if (!string.IsNullOrWhiteSpace(resto))
                {
                    var token = Regex.Replace(resto.ToLowerInvariant(), @"[^a-z0-9]", "");
                    foreach (var kv in Abreviacoes)
                    {
                        if (token.StartsWith(kv.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            var exp = kv.Value;
                            list.Add($"{num} {exp}".Trim());
                            list.Add($"{num}º {exp}".Trim());
                        }
                    }
                }
                else
                {
                    list.Add($"{num}º");
                    list.Add($"{num}");
                }
            }
            else
            {
                // tenta separar letras e números caso escrevam como "2des" sem espaço
                var mm = Regex.Match(s, @"^(\d+)([A-Za-z].*)$", RegexOptions.IgnoreCase);
                if (mm.Success)
                {
                    list.Add($"{mm.Groups[1].Value} {mm.Groups[2].Value}".Trim());
                    list.Add($"{mm.Groups[1].Value}º {mm.Groups[2].Value}".Trim());
                }
            }

            // formas sem espaços ex: transformar "2 d" em "2d"
            list.Add(s.Replace(" ", ""));

            // garantir unicidade e limpar
            return list
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => Regex.Replace(x, @"\s+", " ").Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        // Normaliza string para comparação (lower, sem acentos, sem pontuação exceto números e letras)
        private static string NormalizarParaComparacao(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            s = s.ToLowerInvariant().Trim();
            s = s.Normalize(System.Text.NormalizationForm.FormD);
            s = Regex.Replace(s, @"\p{M}", ""); // remove acentos
            s = Regex.Replace(s, @"[^a-z0-9\s]", ""); // mantém letras/números/espaços
            s = Regex.Replace(s, @"\s+", " ").Trim();
            return s;
        }

        // Remove prefixo numérico (ex: "2º DESENV ...") e normaliza o restante
        private static string RemoverPrefixoNumeroENormalizar(string turma)
        {
            if (string.IsNullOrWhiteSpace(turma)) return "";
            var m = Regex.Match(turma.Trim(), @"^\s*(\d+)[°º]?\s*(.+)$", RegexOptions.IgnoreCase);
            if (m.Success) return NormalizarParaComparacao(m.Groups[2].Value);
            return NormalizarParaComparacao(turma);
        }

        // Score: menor é melhor. >=100 = rejeitado
        // Estratégia:
        // 0 = exato (normalizado)
        // 1 = startswith
        // 2 = restante (sem número) exatamente igual
        // 3 = contains
        // 4+ = distancia Levenshtein aceitável (4..7)
        // 100 = irrelevante
        private static int CalcularScore(string entradaNorm, string turmaNorm, string restanteNorm)
        {
            if (string.IsNullOrWhiteSpace(entradaNorm)) return 100;

            // exato
            if (turmaNorm.Equals(entradaNorm, StringComparison.OrdinalIgnoreCase)) return 0;

            // startswith
            if (turmaNorm.StartsWith(entradaNorm, StringComparison.OrdinalIgnoreCase)) return 1;

            // restante exato
            if (!string.IsNullOrWhiteSpace(restanteNorm) &&
                restanteNorm.Equals(entradaNorm, StringComparison.OrdinalIgnoreCase)) return 2;

            // contains (compatível com .NET Framework)
            if (turmaNorm.IndexOf(entradaNorm, StringComparison.OrdinalIgnoreCase) >= 0) return 3;

            // sem espaços
            if (turmaNorm.Replace(" ", "")
                         .IndexOf(entradaNorm.Replace(" ", ""), StringComparison.OrdinalIgnoreCase) >= 0)
                return 3;

            // Levenshtein tolerante
            int lev = Levenshtein(entradaNorm, turmaNorm);
            if (lev <= 3) return 4 + lev; // 4..7

            // entrada curta (1–2 chars) e contida no restante
            if (entradaNorm.Length <= 2 &&
                !string.IsNullOrWhiteSpace(restanteNorm) &&
                restanteNorm.IndexOf(entradaNorm, StringComparison.OrdinalIgnoreCase) >= 0)
                return 6;

            return 100;
        }


        // Levenshtein (implementação padrão)
        private static int Levenshtein(string s, string t)
        {
            if (string.IsNullOrEmpty(s)) return t?.Length ?? 0;
            if (string.IsNullOrEmpty(t)) return s.Length;

            int[,] d = new int[s.Length + 1, t.Length + 1];
            for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = s[i - 1] == t[j - 1] ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[s.Length, t.Length];
        }

        /// <summary>
        /// Adiciona uma turma dinamicamente ao sistema sem alterar o banco de dados.
        /// A turma é armazenada em arquivo JSON local e persiste entre sessões.
        /// </summary>
        public static void AdicionarTurmaDinamicamente(string turma)
        {
            if (string.IsNullOrWhiteSpace(turma))
                return;

            string turmaNormalizada = turma.Trim().ToUpperInvariant();

            // Verifica se já existe (case-insensitive)
            if (TurmasPermitidas.Any(t => t.Equals(turmaNormalizada, StringComparison.OrdinalIgnoreCase)))
                return;

            // Adiciona à lista em memória
            TurmasPermitidas.Add(turmaNormalizada);
            TurmasPermitidas.Sort(); // Manter ordenado

            // Persiste em arquivo JSON local
            PersistirTurmasDinamicas();
        }

        /// <summary>
        /// Salva as turmas dinâmicas em arquivo JSON na pasta de mapeamento.
        /// </summary>
        private static void PersistirTurmasDinamicas()
        {
            try
            {
                BibliotecaApp.Utils.AppPaths.EnsureFolders();
                string pastaMapeamento = BibliotecaApp.Utils.AppPaths.MappingFolder;
                string caminhoArquivo = Path.Combine(pastaMapeamento, "turmas_dinamicas.json");

                var dados = new
                {
                    Turmas = TurmasPermitidas,
                    DataUltimaAtualizacao = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                string json = JsonConvert.SerializeObject(dados, Formatting.Indented);
                File.WriteAllText(caminhoArquivo, json);

                System.Diagnostics.Trace.WriteLine($"[TurmasUtil] Turmas dinâmicas persistidas: {caminhoArquivo}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"[TurmasUtil] Erro ao persistir turmas: {ex.Message}");
            }
        }

        /// <summary>
        /// Carrega turmas dinâmicas do arquivo JSON se ele existir.
        /// Chamado durante inicialização para restaurar turmas de sessões anteriores.
        /// </summary>
        public static void CarregarTurmasDinamicas()
        {
            try
            {
                BibliotecaApp.Utils.AppPaths.EnsureFolders();
                string pastaMapeamento = BibliotecaApp.Utils.AppPaths.MappingFolder;
                string caminhoArquivo = Path.Combine(pastaMapeamento, "turmas_dinamicas.json");

                if (File.Exists(caminhoArquivo))
                {
                    string json = File.ReadAllText(caminhoArquivo);
                    dynamic dados = JsonConvert.DeserializeObject(json);

                    if (dados?.Turmas != null)
                    {
                        foreach (var turma in dados.Turmas)
                        {
                            string turmaNormalizada = ((string)turma).Trim().ToUpperInvariant();
                            if (!TurmasPermitidas.Any(t => t.Equals(turmaNormalizada, StringComparison.OrdinalIgnoreCase)))
                            {
                                TurmasPermitidas.Add(turmaNormalizada);
                            }
                        }
                        TurmasPermitidas.Sort();
                        System.Diagnostics.Trace.WriteLine($"[TurmasUtil] {dados.Turmas.Count} turmas dinâmicas carregadas");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"[TurmasUtil] Erro ao carregar turmas dinâmicas: {ex.Message}");
            }
        }

        public static void Salvar(List<string> turmas)
{
    _turmasPermitidas = new List<string>(turmas);
    try
    {
        BibliotecaApp.Utils.AppPaths.EnsureFolders();
        var dados = new
        {
            Turmas = _turmasPermitidas,
            DataUltimaAtualizacao = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        string json = JsonConvert.SerializeObject(dados, Formatting.Indented);
        File.WriteAllText(CaminhoTurmasJson, json);
    }
    catch (Exception ex)
    {
        throw new Exception("Erro ao salvar turmas: " + ex.Message, ex);
    }
}

public static List<string> Carregar()
{
    CarregarTurmas();
    return new List<string>(_turmasPermitidas);
}

private static void CarregarTurmas()
{
    BibliotecaApp.Utils.AppPaths.EnsureFolders();
    if (File.Exists(CaminhoTurmasJson))
    {
        try
        {
            string json = File.ReadAllText(CaminhoTurmasJson);
            dynamic dados = JsonConvert.DeserializeObject(json);
            var lista = new List<string>();
            if (dados?.Turmas != null)
            {
                foreach (var turma in dados.Turmas)
                {
                    string t = ((string)turma).Trim();
                    if (!string.IsNullOrEmpty(t))
                        lista.Add(t);
                }
            }
            if (lista.Count > 0)
            {
                _turmasPermitidas = lista;
                return;
            }
        }
        catch { /* fallback para padrão */ }
    }
    _turmasPermitidas = TurmasPadrao();
}

public static List<string> TurmasPadrao()
{
    return new List<string>
    {
        "1º ADMINISTRAÇÃO EP PRO 1",
        "1º DESENV DE SISTEMAS EM INT 1",
        "1º DESENV DE SISTEMAS EM INT 2",
        "1º EM INT 1",
        "1º EM INT 2",
        "1º EM REG 1",
        "2º AGRONEGÓCIO EM INT 1",
        "2º DESENV DE SISTEMAS EM INT 1",
        "2º DESENV DE SISTEMAS EM INT 2",
        "2º ELETROMECÂNICA EP PRO 1",
        "2º ELETROMECÂNICA EP PRO 2",
        "2º EM INT 1",
        "2º EM INT 2",
        "2º EM REG 1",
        "3º AGRONEGÓCIO EM INT 1",
        "3º DESENV DE SISTEMAS EM INT 1",
        "3º DESENV DE SISTEMAS EM INT 2",
        "3º EM INT 1",
        "3º EM REG 1",
        "3º EM REG 2",
        "6º EF AF REG 1", "6º EF AF REG 2", "6º EF AF REG 3", "6º EF AF REG 4",
        "7º EF AF REG 1", "7º EF AF REG 2", "7º EF AF REG 3", "7º EF AF REG 4",
        "8º EF AF REG 1", "8º EF AF REG 2", "8º EF AF REG 3", "8º EF AF REG 4", "8º EF AF REG 5",
        "9º EF AF REG 1", "9º EF AF REG 2", "9º EF AF REG 3", "9º EF AF REG 4", "9º EF AF REG 5"
    };
}
    }
}
