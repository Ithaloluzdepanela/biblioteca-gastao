using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BibliotecaApp.Utils
{
    public static class TurmasUtil
    {
        public static readonly List<string> TurmasPermitidas = new List<string>
        {
            "1º ADMINISTRAÇÃO EP PRO 1", "1º DESENV DE SISTEMAS EM INT 1", "1º DESENV DE SISTEMAS EM INT 2", "1º EM INT 1", "1º EM INT 2", "1º EM REG 1",
            "2º AGRONEGÓCIO EM INT 1", "2º DESENV DE SISTEMAS EM INT 1", "2º DESENV DE SISTEMAS EM INT 1", "2º ELETROMECÂNICA EP PRO 1", "2º ELETROMECÂNICA EP PRO 2",
            "2º EM INT 1", "2º EM REG 1", "3º AGRONEGÓCIO EM INT 1", "3º DESENV DE SISTEMAS 1", "3º EM INT 1", "3º EM REG 1", "3º EM INT 2",
            "6º EF AF REG 1", "6º EF AF REG 2", "6º EF AF REG 3", "6º EF AF REG 4", "7º EF AF REG 1", "7º EF AF REG 2", "7º EF AF REG 3", "7º EF AF REG 4",
            "8º EF AF REG 1", "8º EF AF REG 2", "8º EF AF REG 3", "8º EF AF REG 4", "8º EF AF REG 5", "9º EF AF REG 1", "9º EF AF REG 2", "9º EF AF REG 3",
            "9º EF AF REG 4", "9º EF AF REG 5"
        };

        // Mapeamento de abreviações para palavras-chave
        private static readonly Dictionary<string, string> Abreviacoes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "d", "desenv" }, { "des", "desenv" }, { "desenv", "desenv" },
            { "ag", "agroneg" }, { "adm", "admin" }, { "em", "em" },
            { "reg", "reg" }, { "int", "int" }, { "ef", "ef" }, { "af", "af" },
            { "pro", "pro" }, { "eletro", "eletromec" }, { "sis", "sistemas" }
        };

        public static List<string> BuscarSugestoes(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return new List<string>();
            texto = texto.Trim().ToLower();

            // Normaliza entrada: trata abreviações coladas e separadas
            string normalizado = NormalizarTurma(texto);

            // Busca inteligente: prefixo, substring, abreviação, similaridade
            var sugestoes = TurmasPermitidas
                .Select(t => new { Turma = t, Score = CalcularScore(normalizado, t.ToLower()) })
                .Where(x => x.Score < 100)
                .OrderBy(x => x.Score)
                .ThenBy(x => x.Turma)
                .Select(x => x.Turma)
                .ToList();

            return sugestoes;
        }

        // Normaliza a entrada do usuário para facilitar busca
        private static string NormalizarTurma(string texto)
        {
            // Adiciona ° se houver número no início
            texto = Regex.Replace(texto, @"^(\d+)", "$1º");

            // Se for colado (ex: 1d, 2ag), separa número de letras
            texto = Regex.Replace(texto, @"^(\d+º?)([a-z]+)", "$1 $2");

            // Substitui abreviações por palavras-chave
            foreach (var abrev in Abreviacoes)
            {
                texto = Regex.Replace(texto, $@"\\b{Regex.Escape(abrev.Key)}\\b", abrev.Value, RegexOptions.IgnoreCase);
            }
            return texto;
        }

        // Score: quanto menor, mais relevante
        private static int CalcularScore(string entrada, string turma)
        {
            if (turma.StartsWith(entrada)) return 0;
            if (turma.Contains(entrada)) return 1;
            if (entrada.Length > 1 && turma.Contains(entrada.Replace(" ", ""))) return 2;
            int lev = Levenshtein(entrada, turma);
            if (lev <= 3) return 3 + lev;
            return 100;
        }

        // Levenshtein para tolerância a erros
        private static int Levenshtein(string s, string t)
        {
            if (string.IsNullOrEmpty(s)) return t?.Length ?? 0;
            if (string.IsNullOrEmpty(t)) return s.Length;
            int[,] d = new int[s.Length + 1, t.Length + 1];
            for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) d[0, j] = j;
            for (int i = 1; i <= s.Length; i++)
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = s[i - 1] == t[j - 1] ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            return d[s.Length, t.Length];
        }
    }
}
