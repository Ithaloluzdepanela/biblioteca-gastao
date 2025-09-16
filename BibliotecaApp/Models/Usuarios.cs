using System;
using System.Collections.Generic;

namespace BibliotecaApp.Models
{
    /// <summary>
    /// Representa um usuário do sistema de biblioteca.
    /// Contém informações pessoais, credenciais de acesso e tipo de usuário.
    /// </summary>
    /// <remarks>
    /// Esta classe suporta diferentes tipos de usuário:
    /// - Aluno: Estudantes da instituição
    /// - Professor: Docentes e educadores
    /// - Bibliotecário: Funcionários da biblioteca
    /// - Administrador: Gestores do sistema
    /// - Outros: Visitantes ou casos especiais
    /// </remarks>
    public class Usuarios
    {
        #region Propriedades

        /// <summary>
        /// Identificador único do usuário no banco de dados
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do usuário
        /// </summary>
        /// <remarks>Campo obrigatório para cadastro</remarks>
        public string Nome { get; set; }

        /// <summary>
        /// Tipo de usuário no sistema (Aluno, Professor, Bibliotecário, Administrador, Outros)
        /// </summary>
        /// <remarks>
        /// Define as permissões e funcionalidades disponíveis:
        /// - Aluno: Consultas e empréstimos básicos
        /// - Professor: Empréstimos estendidos
        /// - Bibliotecário: Gestão completa de acervo
        /// - Administrador: Acesso total ao sistema
        /// </remarks>
        public string TipoUsuario { get; set; }

        /// <summary>
        /// CPF do usuário no formato XXX.XXX.XXX-XX
        /// </summary>
        /// <remarks>Campo opcional, usado para identificação única</remarks>
        public string CPF { get; set; }

        /// <summary>
        /// Data de nascimento do usuário
        /// </summary>
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// Telefone de contato no formato (XX) XXXXX-XXXX
        /// </summary>
        public string Telefone { get; set; }

        /// <summary>
        /// Email do usuário para comunicações e recuperação de senha
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Turma do usuário (obrigatório para alunos)
        /// </summary>
        /// <remarks>
        /// Formato sugerido: "3° Desenvolvimento A" ou "6° Ano B"
        /// Usado para relatórios e controle acadêmico
        /// </remarks>
        public string Turma { get; set; }

        /// <summary>
        /// Senha do usuário (será criptografada com BCrypt)
        /// </summary>
        /// <remarks>
        /// A senha é armazenada como hash BCrypt no banco de dados.
        /// Nunca é armazenada em texto plano.
        /// </remarks>
        public string Senha { get; set; }

        /// <summary>
        /// Confirmação da senha (usado apenas durante cadastro/alteração)
        /// </summary>
        /// <remarks>
        /// Campo temporário para validação, não é persistido no banco
        /// </remarks>
        public string ConfirmarSenha { get; set; }

        #endregion

        #region Propriedades Estáticas

        /// <summary>
        /// Lista estática para cache temporário de usuários
        /// </summary>
        /// <remarks>
        /// Usado para otimização de performance em operações de consulta.
        /// Não substitui o banco de dados como fonte primária.
        /// </remarks>
        public static List<Usuarios> ListaUsuarios { get; set; } = new List<Usuarios>();

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor padrão para inicialização vazia
        /// </summary>
        public Usuarios() { }

        /// <summary>
        /// Construtor completo para inicialização com todos os dados
        /// </summary>
        /// <param name="id">Identificador único do usuário</param>
        /// <param name="nome">Nome completo do usuário</param>
        /// <param name="tipoUsuario">Tipo de usuário no sistema</param>
        /// <param name="cpf">CPF do usuário</param>
        /// <param name="dataNascimento">Data de nascimento</param>
        /// <param name="telefone">Telefone de contato</param>
        /// <param name="email">Email do usuário</param>
        /// <param name="turma">Turma do usuário (para alunos)</param>
        /// <param name="senha">Senha de acesso</param>
        /// <param name="confirmarSenha">Confirmação da senha</param>
        public Usuarios(int id, string nome, string tipoUsuario, string cpf, DateTime dataNascimento,
                       string telefone, string email, string turma, string senha, string confirmarSenha)
        {
            Id = id;
            Nome = nome;
            TipoUsuario = tipoUsuario;
            CPF = cpf;
            DataNascimento = dataNascimento;
            Telefone = telefone;
            Email = email;
            Turma = turma;
            Senha = senha;
            ConfirmarSenha = confirmarSenha;
        }

        #endregion

        #region Métodos de Validação

        /// <summary>
        /// Verifica se a senha e sua confirmação são idênticas
        /// </summary>
        /// <returns>True se as senhas coincidem, false caso contrário</returns>
        /// <remarks>
        /// Método usado durante cadastro e alteração de senha para garantir
        /// que o usuário digitou a senha corretamente duas vezes
        /// </remarks>
        public bool SenhasCoincidem()
        {
            return Senha == ConfirmarSenha;
        }

        /// <summary>
        /// Verifica se todos os campos obrigatórios foram preenchidos
        /// </summary>
        /// <returns>True se todos os campos obrigatórios estão preenchidos</returns>
        /// <remarks>
        /// Campos obrigatórios: Nome, TipoUsuario, CPF, Senha, ConfirmarSenha
        /// Este método deve ser chamado antes de salvar o usuário no banco
        /// </remarks>
        public bool CamposObrigatoriosPreenchidos()
        {
            return !string.IsNullOrWhiteSpace(Nome)
                && !string.IsNullOrWhiteSpace(TipoUsuario)
                && !string.IsNullOrWhiteSpace(CPF)
                && !string.IsNullOrWhiteSpace(Senha)
                && !string.IsNullOrWhiteSpace(ConfirmarSenha);
        }

        #endregion

        #region Métodos Sobrescritos

        /// <summary>
        /// Retorna uma representação em string do usuário
        /// </summary>
        /// <returns>String no formato "Nome - Turma"</returns>
        /// <remarks>
        /// Usado para exibição em ComboBoxes e ListBoxes.
        /// Se a turma estiver vazia, retorna apenas o nome.
        /// </remarks>
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Turma) ? Nome : $"{Nome} - {Turma}";
        }

        #endregion

        #region Classes Internas

        /// <summary>
        /// Classe especializada para dados específicos de alunos
        /// </summary>
        /// <remarks>
        /// Esta classe contém apenas os dados essenciais para alunos,
        /// sendo usada em operações específicas onde nem todos os dados
        /// de usuário são necessários, como relatórios e consultas rápidas.
        /// </remarks>
        public class Aluno
        {
            /// <summary>
            /// Nome completo do aluno
            /// </summary>
            public string Nome { get; set; }

            /// <summary>
            /// Email do aluno para contato
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Turma atual do aluno
            /// </summary>
            public string Turma { get; set; }

            /// <summary>
            /// Telefone de contato do aluno
            /// </summary>
            public string Telefone { get; set; }

            /// <summary>
            /// CPF do aluno
            /// </summary>
            public string CPF { get; set; }

            /// <summary>
            /// Data de nascimento do aluno
            /// </summary>
            public DateTime DataNascimento { get; set; }

            /// <summary>
            /// Construtor padrão para a classe Aluno
            /// </summary>
            public Aluno() { }

            /// <summary>
            /// Cria uma instância de Aluno a partir de um usuário completo
            /// </summary>
            /// <param name="usuario">Usuário do qual extrair os dados do aluno</param>
            /// <returns>Nova instância de Aluno com os dados copiados</returns>
            public static Aluno DeUsuario(Usuarios usuario)
            {
                return new Aluno
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Turma = usuario.Turma,
                    Telefone = usuario.Telefone,
                    CPF = usuario.CPF,
                    DataNascimento = usuario.DataNascimento
                };
            }
        }

        #endregion
    }
}