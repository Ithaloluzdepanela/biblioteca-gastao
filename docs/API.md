# Documentação de API/Classes - BibliotecaApp

## 📋 Índice

- [Visão Geral](#visão-geral)
- [Modelos de Dados](#modelos-de-dados)
- [Utilitários](#utilitários)
- [Forms Principais](#forms-principais)
- [Serviços](#serviços)
- [Enumerações](#enumerações)
- [Interfaces](#interfaces)
- [Exemplos de Uso](#exemplos-de-uso)

## 🏗 Visão Geral

Esta documentação descreve as principais classes, métodos e propriedades do sistema BibliotecaApp. O sistema segue o padrão MVP (Model-View-Presenter) com Windows Forms.

### Convenções de Documentação

- **Namespace Principal**: `BibliotecaApp`
- **Subnamespaces**: `Models`, `Utils`, `Forms`, `Elements`
- **Padrão de Comentários**: XML Documentation Comments (C#)

## 📊 Modelos de Dados

### Classe `Usuarios`

**Namespace**: `BibliotecaApp.Models`  
**Descrição**: Representa um usuário do sistema de biblioteca

#### Propriedades

| Propriedade | Tipo | Descrição | Obrigatório |
|-------------|------|-----------|-------------|
| `Id` | `int` | Identificador único no banco | Sim (auto) |
| `Nome` | `string` | Nome completo do usuário | Sim |
| `TipoUsuario` | `string` | Tipo: Aluno, Professor, Bibliotecário, Admin | Sim |
| `CPF` | `string` | CPF no formato XXX.XXX.XXX-XX | Não |
| `DataNascimento` | `DateTime` | Data de nascimento | Não |
| `Telefone` | `string` | Telefone no formato (XX) XXXXX-XXXX | Não |
| `Email` | `string` | Email para comunicações | Não |
| `Turma` | `string` | Turma do usuário (obrigatório para alunos) | Condicional |
| `Senha` | `string` | Hash BCrypt da senha | Sim |
| `ConfirmarSenha` | `string` | Campo temporário para validação | Não |

#### Métodos Principais

```csharp
/// <summary>
/// Verifica se a senha e confirmação coincidem
/// </summary>
/// <returns>True se as senhas são iguais</returns>
public bool SenhasCoincidem()

/// <summary>
/// Valida se campos obrigatórios estão preenchidos
/// </summary>
/// <returns>True se todos os campos obrigatórios são válidos</returns>
public bool CamposObrigatoriosPreenchidos()

/// <summary>
/// Representação string do usuário
/// </summary>
/// <returns>Formato "Nome - Turma"</returns>
public override string ToString()
```

#### Classe Aninhada: `Aluno`

**Descrição**: Versão simplificada para operações específicas de alunos

```csharp
public class Aluno
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Turma { get; set; }
    public string Telefone { get; set; }
    public string CPF { get; set; }
    public DateTime DataNascimento { get; set; }
    
    /// <summary>
    /// Cria Aluno a partir de Usuario completo
    /// </summary>
    public static Aluno DeUsuario(Usuarios usuario)
}
```

---

### Classe `Livro`

**Namespace**: `BibliotecaApp.Models`  
**Descrição**: Representa um livro no acervo da biblioteca

#### Propriedades

| Propriedade | Tipo | Descrição | Obrigatório |
|-------------|------|-----------|-------------|
| `Id` | `int` | Identificador único no banco | Sim (auto) |
| `Nome` | `string` | Título do livro | Sim |
| `Autor` | `string` | Autor principal | Sim |
| `Genero` | `string` | Gênero/categoria | Sim |
| `Disponibilidade` | `bool` | Se está disponível para empréstimo | Sim |
| `Quantidade` | `int` | Número total de exemplares | Sim |
| `CodigoDeBarras` | `string` | Código de barras (opcional) | Não |

#### Métodos de Negócio

```csharp
/// <summary>
/// Verifica se o livro pode ser reservado
/// </summary>
/// <returns>True se pode ser reservado</returns>
public bool PodeSerReservado()

/// <summary>
/// Verifica se há exemplares disponíveis
/// </summary>
/// <returns>True se há exemplares livres</returns>
public bool TemExemplaresDisponiveis()

/// <summary>
/// Calcula exemplares emprestados
/// </summary>
/// <param name="totalEmprestimos">Total de empréstimos ativos</param>
/// <returns>Número de exemplares emprestados</returns>
public int ExemplaresEmprestados(int totalEmprestimos)

/// <summary>
/// Calcula exemplares disponíveis
/// </summary>
/// <param name="totalEmprestimos">Total de empréstimos ativos</param>
/// <returns>Número de exemplares disponíveis</returns>
public int ExemplaresDisponiveis(int totalEmprestimos)

/// <summary>
/// Valida se dados obrigatórios estão preenchidos
/// </summary>
/// <returns>True se dados são válidos</returns>
public bool DadosValidos()
```

#### Métodos Utilitários

```csharp
/// <summary>
/// Obtém código de barras (gera se necessário)
/// </summary>
/// <returns>Código de barras válido</returns>
public string ObterCodigoDeBarras()

/// <summary>
/// Formata nome para exibição com limite de caracteres
/// </summary>
/// <param name="tamanhoMaximo">Tamanho máximo (padrão: 50)</param>
/// <returns>Nome formatado</returns>
public string NomeFormatado(int tamanhoMaximo = 50)

/// <summary>
/// Cria resumo do livro para listas
/// </summary>
/// <returns>String "Título - Autor"</returns>
public string ResumoFormatado()
```

---

### Classe `Emprestimo`

**Namespace**: `BibliotecaApp.Models`  
**Descrição**: Representa um empréstimo de livro

#### Propriedades

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `Id` | `int` | Identificador único |
| `UsuarioId` | `int` | ID do usuário |
| `LivroId` | `int` | ID do livro |
| `DataEmprestimo` | `DateTime` | Data do empréstimo |
| `DataPrevistaDevolucao` | `DateTime` | Data prevista para devolução |
| `DataRealDevolucao` | `DateTime?` | Data real da devolução (null se ativo) |
| `ResponsavelId` | `int` | ID do bibliotecário responsável |
| `Observacoes` | `string` | Observações sobre o empréstimo |

---

## 🔧 Utilitários

### Classe `Conexao`

**Namespace**: `BibliotecaApp.Utils`  
**Descrição**: Gerenciamento de conexões com banco de dados SQL Server CE

#### Propriedades Estáticas

```csharp
/// <summary>
/// Caminho completo para o arquivo do banco
/// </summary>
public static string CaminhoBanco { get; }

/// <summary>
/// String de conexão completa
/// </summary>
public static string Conectar { get; }
```

#### Métodos Principais

```csharp
/// <summary>
/// Cria nova conexão com o banco
/// </summary>
/// <returns>SqlCeConnection configurada</returns>
public static SqlCeConnection ObterConexao()

/// <summary>
/// Testa se conexão pode ser estabelecida
/// </summary>
/// <returns>True se conexão é válida</returns>
public static bool TestarConexao()

/// <summary>
/// Verifica se arquivo de banco existe
/// </summary>
/// <returns>True se arquivo existe</returns>
public static bool ArquivoBancoExiste()

/// <summary>
/// Obtém tamanho do banco em bytes
/// </summary>
/// <returns>Tamanho do arquivo ou -1 se erro</returns>
public static long ObterTamanhoBanco()

/// <summary>
/// Tamanho do banco formatado para exibição
/// </summary>
/// <returns>String formatada (ex: "2.5 MB")</returns>
public static string ObterTamanhoBancoFormatado()

/// <summary>
/// Diagnóstico completo do banco
/// </summary>
/// <returns>Relatório de diagnóstico</returns>
public static string DiagnosticarBanco()
```

---

### Classe `CriptografiaSenha`

**Namespace**: `BibliotecaApp.Utils`  
**Descrição**: Utilitários para criptografia de senhas usando BCrypt

```csharp
/// <summary>
/// Criptografa senha usando BCrypt
/// </summary>
/// <param name="senha">Senha em texto plano</param>
/// <returns>Hash BCrypt da senha</returns>
public static string CriptografarSenha(string senha)

/// <summary>
/// Verifica se senha corresponde ao hash
/// </summary>
/// <param name="senha">Senha em texto plano</param>
/// <param name="hash">Hash armazenado</param>
/// <returns>True se senha é válida</returns>
public static bool VerificarSenha(string senha, string hash)
```

---

### Classe `LicenseValidator`

**Namespace**: `BibliotecaApp.Utils`  
**Descrição**: Validação e gerenciamento de licenças

```csharp
/// <summary>
/// Tenta obter licença ativa
/// </summary>
/// <param name="license">Dados da licença (output)</param>
/// <returns>True se licença encontrada</returns>
public static bool TryGetActivation(out LicenseData license)

/// <summary>
/// Verifica se licença expirou
/// </summary>
/// <param name="license">Dados da licença</param>
/// <returns>True se expirou</returns>
public static bool IsExpired(LicenseData license)
```

---

## 🖼 Forms Principais

### MainForm

**Namespace**: `BibliotecaApp.Forms.Inicio`  
**Descrição**: Form principal MDI que hospeda outros forms

#### Métodos Principais

```csharp
/// <summary>
/// Abre form filho no container MDI
/// </summary>
/// <param name="childForm">Form a ser aberto</param>
/// <param name="keepPreviousHidden">Manter form anterior oculto</param>
public void OpenChild(Form childForm, bool keepPreviousHidden = false)

/// <summary>
/// Verifica se há mapeamento de turmas pendente
/// </summary>
private void ChecarMapeamentoPendenteAoInicializar()

/// <summary>
/// Alterna entre maximizado e restaurado
/// </summary>
private void AlternarMaximizado()
```

---

### LoginForm

**Namespace**: `BibliotecaApp.Forms.Login`  
**Descrição**: Form de autenticação de usuários

#### Eventos Principais

```csharp
/// <summary>
/// Processa tentativa de login
/// </summary>
private void btnEntrar_Click(object sender, EventArgs e)

/// <summary>
/// Abre form de recuperação de senha
/// </summary>
private void btnEsqueceuSenha_Click(object sender, EventArgs e)
```

---

### InicioForm (Dashboard)

**Namespace**: `BibliotecaApp.Forms.Inicio`  
**Descrição**: Dashboard com estatísticas e acesso rápido

#### Métodos de Dados

```csharp
/// <summary>
/// Obtém estatísticas gerais do sistema
/// </summary>
/// <returns>Dicionário com estatísticas</returns>
private Dictionary<string, int> ObterEstatisticas()

/// <summary>
/// Carrega lista de livros populares
/// </summary>
/// <param name="topN">Número de livros no top</param>
/// <returns>Lista de livros mais emprestados</returns>
private List<LivroPopular> ObterLivrosPopulares(int topN)

/// <summary>
/// Atualiza cards de estatísticas
/// </summary>
/// <param name="stats">Estatísticas atualizadas</param>
private void AtualizarCards(Dictionary<string, int> stats)
```

---

## 🔨 Serviços

### BackupManager

**Namespace**: `BibliotecaApp.Utils`  
**Descrição**: Gerenciamento de backups locais e em nuvem

```csharp
/// <summary>
/// Executa backup manual
/// </summary>
/// <param name="destino">Caminho de destino</param>
/// <returns>True se backup foi criado com sucesso</returns>
public static bool ExecutarBackupManual(string destino)

/// <summary>
/// Restaura backup
/// </summary>
/// <param name="origem">Caminho do arquivo de backup</param>
/// <returns>True se restauração foi bem-sucedida</returns>
public static bool RestaurarBackup(string origem)
```

---

### EmailService

**Namespace**: `BibliotecaApp.Utils`  
**Descrição**: Serviços de envio de email

```csharp
/// <summary>
/// Envia email de recuperação de senha
/// </summary>
/// <param name="destinatario">Email do usuário</param>
/// <param name="novaSenha">Nova senha temporária</param>
/// <returns>True se email foi enviado</returns>
public static bool EnviarRecuperacaoSenha(string destinatario, string novaSenha)
```

---

## 📋 Enumerações

### TipoUsuario

```csharp
public enum TipoUsuario
{
    Aluno,
    Professor,
    Bibliotecario,
    Administrador,
    Outros
}
```

### StatusEmprestimo

```csharp
public enum StatusEmprestimo
{
    Ativo,
    Devolvido,
    Atrasado,
    Reservado
}
```

### TipoRelatorio

```csharp
public enum TipoRelatorio
{
    Emprestimos,
    LivrosPopulares,
    UsuariosAtrasados,
    MovimentacaoDiaria
}
```

---

## 🎯 Interfaces

### IRepository<T>

**Descrição**: Interface padrão para repositórios (padrão conceitual)

```csharp
public interface IRepository<T>
{
    void Inserir(T entity);
    T BuscarPorId(int id);
    List<T> BuscarTodos();
    void Atualizar(T entity);
    void Excluir(int id);
}
```

### IValidavel

**Descrição**: Interface para entidades que podem ser validadas

```csharp
public interface IValidavel
{
    bool DadosValidos();
    List<string> ObterErrosValidacao();
}
```

---

## 💡 Exemplos de Uso

### Conectar ao Banco e Consultar Usuários

```csharp
using (var conexao = Conexao.ObterConexao())
{
    conexao.Open();
    
    var comando = new SqlCeCommand(
        "SELECT * FROM Usuario WHERE TipoUsuario = @tipo", 
        conexao);
    comando.Parameters.AddWithValue("@tipo", "Aluno");
    
    var usuarios = new List<Usuarios>();
    using (var reader = comando.ExecuteReader())
    {
        while (reader.Read())
        {
            usuarios.Add(new Usuarios
            {
                Id = reader.GetInt32("Id"),
                Nome = reader.GetString("Nome"),
                TipoUsuario = reader.GetString("TipoUsuario")
            });
        }
    }
}
```

### Validar e Criptografar Senha

```csharp
var usuario = new Usuarios
{
    Nome = "João Silva",
    TipoUsuario = "Aluno",
    Senha = "123456",
    ConfirmarSenha = "123456"
};

// Validar dados
if (!usuario.CamposObrigatoriosPreenchidos())
{
    MessageBox.Show("Preencha todos os campos obrigatórios");
    return;
}

if (!usuario.SenhasCoincidem())
{
    MessageBox.Show("Senhas não coincidem");
    return;
}

// Criptografar senha antes de salvar
usuario.Senha = CriptografiaSenha.CriptografarSenha(usuario.Senha);
```

### Verificar Disponibilidade de Livro

```csharp
var livro = new Livro
{
    Nome = "Dom Casmurro",
    Autor = "Machado de Assis",
    Genero = "Literatura",
    Quantidade = 3
};

// Simular 2 empréstimos ativos
int emprestimosAtivos = 2;

if (livro.TemExemplaresDisponiveis())
{
    int disponiveis = livro.ExemplaresDisponiveis(emprestimosAtivos);
    Console.WriteLine($"Exemplares disponíveis: {disponiveis}");
    
    if (disponiveis > 0)
    {
        // Pode emprestar
    }
    else if (livro.PodeSerReservado())
    {
        // Pode reservar
    }
}
```

### Diagnóstico do Sistema

```csharp
// Verificar estado do banco
if (!Conexao.ArquivoBancoExiste())
{
    MessageBox.Show("Banco de dados não encontrado!");
    return;
}

if (!Conexao.TestarConexao())
{
    string diagnostico = Conexao.DiagnosticarBanco();
    MessageBox.Show($"Problema com banco:\n{diagnostico}");
    return;
}

// Sistema OK para usar
string tamanho = Conexao.ObterTamanhoBancoFormatado();
Console.WriteLine($"Banco OK - Tamanho: {tamanho}");
```

### Validação de Login

```csharp
string usuario = txtUsuario.Text;
string senha = txtSenha.Text;

// Buscar hash da senha no banco
string hashArmazenado = BuscarHashSenhaUsuario(usuario);

if (string.IsNullOrEmpty(hashArmazenado))
{
    MessageBox.Show("Usuário não encontrado");
    return;
}

// Verificar senha
if (CriptografiaSenha.VerificarSenha(senha, hashArmazenado))
{
    // Login válido
    DialogResult = DialogResult.OK;
}
else
{
    MessageBox.Show("Senha incorreta");
}
```

---

## 🔗 Referências Úteis

### Documentação Relacionada
- [README.md](../README.md) - Visão geral do projeto
- [ARCHITECTURE.md](ARCHITECTURE.md) - Arquitetura do sistema
- [DEVELOPER_GUIDE.md](DEVELOPER_GUIDE.md) - Guia do desenvolvedor
- [USER_MANUAL.md](USER_MANUAL.md) - Manual do usuário

### Tecnologias Utilizadas
- [.NET Framework 4.8](https://docs.microsoft.com/dotnet/framework/)
- [Windows Forms](https://docs.microsoft.com/dotnet/desktop/winforms/)
- [SQL Server Compact Edition](https://docs.microsoft.com/sql/relational-databases/sql-server-compact/)
- [BCrypt.Net](https://github.com/BcryptNet/bcrypt.net)

---

Esta documentação de API fornece uma referência completa das classes e métodos principais do BibliotecaApp. Para detalhes de implementação específicos, consulte o código-fonte ou a documentação do desenvolvedor.