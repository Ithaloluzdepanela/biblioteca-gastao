using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaApp.Models
{
    /// <summary>
    /// Representa um livro no acervo da biblioteca.
    /// Contém informações bibliográficas, controle de estoque e disponibilidade.
    /// </summary>
    /// <remarks>
    /// Esta classe gerencia tanto as informações catalográ​ficas quanto
    /// o controle operacional dos livros, incluindo quantidade disponível
    /// e status para empréstimos.
    /// </remarks>
    public class Livro
    {
        #region Propriedades

        /// <summary>
        /// Identificador único do livro no banco de dados
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Título completo do livro
        /// </summary>
        /// <remarks>Campo obrigatório para cadastro</remarks>
        public string Nome { get; set; }

        /// <summary>
        /// Nome do autor principal do livro
        /// </summary>
        /// <remarks>Campo obrigatório. Para múltiplos autores, usar formato "Autor1; Autor2"</remarks>
        public string Autor { get; set; }

        /// <summary>
        /// Gênero ou categoria do livro
        /// </summary>
        /// <remarks>
        /// Usado para classificação e busca. O sistema oferece sugestões
        /// padronizadas para manter consistência no acervo.
        /// Exemplos: Romance, Ficção, Didático, História, etc.
        /// </remarks>
        public string Genero { get; set; }

        /// <summary>
        /// Indica se o livro está disponível para empréstimo
        /// </summary>
        /// <remarks>
        /// False quando todos os exemplares estão emprestados ou
        /// quando o livro foi marcado como indisponível manualmente
        /// (ex: em manutenção, extraviado, etc.)
        /// </remarks>
        public bool Disponibilidade { get; set; }

        /// <summary>
        /// Número total de exemplares do livro no acervo
        /// </summary>
        /// <remarks>
        /// Inclui tanto os exemplares disponíveis quanto os emprestados.
        /// Deve ser sempre maior ou igual ao número de empréstimos ativos.
        /// </remarks>
        public int Quantidade { get; set; }

        /// <summary>
        /// Código de barras do livro para identificação rápida
        /// </summary>
        /// <remarks>
        /// Campo opcional. Usado para agilizar empréstimos e devoluções
        /// com leitor de código de barras. Se não fornecido, o sistema
        /// pode gerar um código interno automaticamente.
        /// </remarks>
        public string CodigoDeBarras { get; set; }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor padrão para inicialização vazia
        /// </summary>
        public Livro() 
        {
            // Inicializa com valores padrão seguros
            Disponibilidade = true;
            Quantidade = 1;
        }

        /// <summary>
        /// Construtor completo para inicialização com dados específicos
        /// </summary>
        /// <param name="nome">Título do livro</param>
        /// <param name="autor">Autor do livro</param>
        /// <param name="genero">Gênero do livro</param>
        /// <param name="disponivel">Status de disponibilidade inicial</param>
        /// <param name="quantidade">Número de exemplares</param>
        /// <param name="codigoDeBarras">Código de barras (opcional)</param>
        public Livro(string nome, string autor, string genero, bool disponivel, int quantidade, string codigoDeBarras)
        {
            Nome = nome;
            Autor = autor;
            Genero = genero;
            Disponibilidade = disponivel;
            Quantidade = Math.Max(1, quantidade); // Garante pelo menos 1 exemplar
            CodigoDeBarras = codigoDeBarras;
        }

        #endregion

        #region Métodos de Negócio

        /// <summary>
        /// Verifica se o livro pode ser reservado por um usuário
        /// </summary>
        /// <returns>True se o livro pode ser reservado, false caso contrário</returns>
        /// <remarks>
        /// Um livro pode ser reservado quando:
        /// - Não está disponível (todos os exemplares emprestados)
        /// - Ou quando a quantidade total é zero (livro temporariamente fora de circulação)
        /// </remarks>
        public bool PodeSerReservado()
        {
            return !Disponibilidade && Quantidade == 0;
        }

        /// <summary>
        /// Verifica se há exemplares disponíveis para empréstimo
        /// </summary>
        /// <returns>True se há exemplares disponíveis</returns>
        public bool TemExemplaresDisponiveis()
        {
            return Disponibilidade && Quantidade > 0;
        }

        /// <summary>
        /// Calcula o número de exemplares atualmente emprestados
        /// </summary>
        /// <param name="totalEmprestimos">Número total de empréstimos ativos deste livro</param>
        /// <returns>Número de exemplares emprestados</returns>
        /// <remarks>
        /// Este método auxiliar pode ser usado em conjunto com consultas ao banco
        /// para determinar quantos exemplares estão em circulação
        /// </remarks>
        public int ExemplaresEmprestados(int totalEmprestimos)
        {
            return Math.Min(totalEmprestimos, Quantidade);
        }

        /// <summary>
        /// Calcula o número de exemplares disponíveis na biblioteca
        /// </summary>
        /// <param name="totalEmprestimos">Número total de empréstimos ativos deste livro</param>
        /// <returns>Número de exemplares disponíveis</returns>
        public int ExemplaresDisponiveis(int totalEmprestimos)
        {
            return Math.Max(0, Quantidade - totalEmprestimos);
        }

        /// <summary>
        /// Valida se todos os dados obrigatórios foram preenchidos
        /// </summary>
        /// <returns>True se todos os campos obrigatórios estão válidos</returns>
        public bool DadosValidos()
        {
            return !string.IsNullOrWhiteSpace(Nome) &&
                   !string.IsNullOrWhiteSpace(Autor) &&
                   !string.IsNullOrWhiteSpace(Genero) &&
                   Quantidade > 0;
        }

        #endregion

        #region Métodos Utilitários

        /// <summary>
        /// Gera um código de barras interno se não houver um fornecido
        /// </summary>
        /// <returns>Código de barras gerado ou existente</returns>
        /// <remarks>
        /// Formato gerado: "LIV" + ID do livro com 8 dígitos
        /// Exemplo: LIV00000123
        /// </remarks>
        public string ObterCodigoDeBarras()
        {
            if (!string.IsNullOrWhiteSpace(CodigoDeBarras))
                return CodigoDeBarras;

            return $"LIV{Id:D8}";
        }

        /// <summary>
        /// Formata o nome do livro para exibição, truncando se necessário
        /// </summary>
        /// <param name="tamanhoMaximo">Tamanho máximo da string retornada</param>
        /// <returns>Nome formatado</returns>
        public string NomeFormatado(int tamanhoMaximo = 50)
        {
            if (string.IsNullOrWhiteSpace(Nome))
                return "Sem título";

            if (Nome.Length <= tamanhoMaximo)
                return Nome;

            return Nome.Substring(0, tamanhoMaximo - 3) + "...";
        }

        /// <summary>
        /// Cria um resumo do livro para exibição em listas
        /// </summary>
        /// <returns>String com título e autor</returns>
        public string ResumoFormatado()
        {
            var nome = NomeFormatado(30);
            var autor = string.IsNullOrWhiteSpace(Autor) ? "Autor desconhecido" : Autor;
            
            if (autor.Length > 20)
                autor = autor.Substring(0, 17) + "...";
                
            return $"{nome} - {autor}";
        }

        #endregion

        #region Métodos Sobrescritos

        /// <summary>
        /// Retorna uma representação em string do livro
        /// </summary>
        /// <returns>String no formato "Título - Autor"</returns>
        public override string ToString()
        {
            return ResumoFormatado();
        }

        /// <summary>
        /// Determina se o objeto especificado é igual ao livro atual
        /// </summary>
        /// <param name="obj">Objeto a ser comparado</param>
        /// <returns>True se os objetos são iguais</returns>
        public override bool Equals(object obj)
        {
            if (obj is Livro outroLivro)
            {
                return Id == outroLivro.Id && Id > 0;
            }
            return false;
        }

        /// <summary>
        /// Serve como função hash padrão
        /// </summary>
        /// <returns>Código hash do livro</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
}
