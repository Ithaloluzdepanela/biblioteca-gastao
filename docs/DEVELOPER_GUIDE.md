# Guia do Desenvolvedor - BibliotecaApp

## 📋 Índice

- [Ambiente de Desenvolvimento](#ambiente-de-desenvolvimento)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Convenções de Código](#convenções-de-código)
- [Padrões de Desenvolvimento](#padrões-de-desenvolvimento)
- [Banco de Dados](#banco-de-dados)
- [Testes](#testes)
- [Debugging e Logging](#debugging-e-logging)
- [Build e Deploy](#build-e-deploy)
- [Contribuição](#contribuição)

## 🔧 Ambiente de Desenvolvimento

### Pré-requisitos

- **Visual Studio 2019** ou superior (Community/Professional/Enterprise)
- **.NET Framework 4.8 Developer Pack**
- **SQL Server Compact Edition 4.0**
- **Git** para controle de versão
- **NuGet Package Manager** (integrado no VS)

### Configuração Inicial

1. **Clone do Repositório**
   ```bash
   git clone https://github.com/Ithaloluzdepanela/biblioteca-gastao.git
   cd biblioteca-gastao/BibliotecaApp
   ```

2. **Abertura no Visual Studio**
   ```bash
   # Abrir solução
   start BibliotecaApp.sln
   ```

3. **Restauração de Pacotes**
   - Visual Studio restaura automaticamente os pacotes NuGet
   - Ou execute manualmente: `Tools > NuGet Package Manager > Restore`

4. **Configuração do Banco**
   - O arquivo `bibliotecaDB.sdf` já vem configurado
   - Localização: `BibliotecaApp/BibliotecaDB/bibliotecaDB.sdf`
   - Senha: `123`

### Extensões Recomendadas

- **ReSharper** - Refactoring e análise de código
- **CodeMaid** - Limpeza e organização automática
- **Git Extensions** - Interface gráfica para Git
- **Productivity Power Tools** - Produtividade no VS

## 📁 Estrutura do Projeto

### Organização de Pastas

```
BibliotecaApp/
├── 📁 Forms/                    # Interface do usuário
│   ├── 📁 Inicio/              # Telas principais e dashboard
│   │   ├── MainForm.cs         # Form principal MDI
│   │   └── InicioForm.cs       # Dashboard com estatísticas
│   ├── 📁 Login/               # Autenticação
│   │   ├── LoginForm.cs        # Tela de login
│   │   ├── AboutForm.cs        # Informações do sistema
│   │   └── EsqueceuSenhaForm.cs # Recuperação de senha
│   ├── 📁 Usuario/             # Gestão de usuários
│   │   ├── UsuarioForm.cs      # Listagem de usuários
│   │   ├── CadUsuario.cs       # Cadastro de usuários
│   │   └── EditarUsuarioForm.cs # Edição de usuários
│   ├── 📁 Livros/              # Gestão de livros e empréstimos
│   │   ├── LivrosForm.cs       # Listagem de livros
│   │   ├── CadastroLivroForm.cs # Cadastro de livros
│   │   ├── EmprestimoForm.cs   # Empréstimos tradicionais
│   │   ├── EmprestimoRapidoForm.cs # Empréstimos rápidos
│   │   ├── DevoluçãoForm.cs    # Devoluções
│   │   └── FichaAlunoForm.cs   # Ficha do aluno
│   ├── 📁 Relatorio/           # Relatórios e exportações
│   │   └── RelForm.cs          # Geração de relatórios
│   └── 📁 Mapeamento/          # Funcionalidades especiais
│       └── MapeamentoDeTurmasWizardForm.cs # Mapeamento de turmas
├── 📁 Models/                   # Modelos de dados
│   ├── Usuarios.cs             # Entidade de usuário
│   ├── Livro.cs                # Entidade de livro
│   ├── Emprestimo.cs           # Entidade de empréstimo
│   ├── Sessao.cs               # Controle de sessão
│   └── MapeamentoModel.cs      # Modelos auxiliares
├── 📁 Utils/                    # Utilitários e helpers
│   ├── Conexao.cs              # Gerenciamento de conexões
│   ├── CriptografiaSenha.cs    # Criptografia BCrypt
│   ├── LicenseValidator.cs     # Sistema de licenças
│   ├── BackupManager.cs        # Backup automático
│   ├── EmailService.cs         # Serviços de email
│   ├── AppPaths.cs             # Gerenciamento de caminhos
│   ├── TurmasUtil.cs           # Utilitários de turmas
│   └── mdiProperties.cs        # Propriedades MDI
├── 📁 Elements/                 # Componentes customizados
│   ├── RoundedTextBox.cs       # TextBox personalizado
│   ├── RoundedComboBox.cs      # ComboBox personalizado
│   ├── GradientPanel.cs        # Panel com gradiente
│   └── AnimatedToggle.cs       # Toggle animado
├── 📁 BibliotecaDB/            # Banco de dados
│   └── bibliotecaDB.sdf        # Arquivo do banco SQL CE
├── 📁 Resources/                # Recursos (imagens, ícones)
└── 📄 Program.cs               # Ponto de entrada da aplicação
```

### Responsabilidades por Pasta

#### Forms/
**Responsabilidade**: Interface do usuário e lógica de apresentação
- Cada pasta representa um módulo funcional
- Forms seguem padrão MVP
- Validações de entrada e UX

#### Models/
**Responsabilidade**: Entidades de negócio e regras de domínio
- POCOs (Plain Old CLR Objects)
- Validações de negócio
- Relacionamentos entre entidades

#### Utils/
**Responsabilidade**: Funcionalidades transversais e infraestrutura
- Acesso a dados
- Segurança e criptografia
- Serviços externos
- Configurações globais

#### Elements/
**Responsabilidade**: Componentes de interface reutilizáveis
- Controles customizados
- Material Design elements
- Componentes com lógica específica

## 📝 Convenções de Código

### Nomenclatura

#### Classes
```csharp
// ✅ Correto
public class UsuarioForm : Form { }
public class CriptografiaSenha { }
public class BackupManager { }

// ❌ Incorreto
public class usuarioform { }
public class cryptoPassword { }
```

#### Métodos
```csharp
// ✅ Correto
public void CadastrarUsuario() { }
public bool ValidarCampos() { }
private void btnSalvar_Click(object sender, EventArgs e) { }

// ❌ Incorreto
public void cadastrarusuario() { }
public bool validar_campos() { }
```

#### Variáveis e Campos
```csharp
// ✅ Correto
private string nomeUsuario;
private int idUsuario;
private List<Livro> livrosDisponiveis;

// ❌ Incorreto
private string nome_usuario;
private int ID_USUARIO;
```

#### Constantes
```csharp
// ✅ Correto
private const string CONEXAO_STRING = "Data Source=...";
private const int TIMEOUT_SEGUNDOS = 30;

// ❌ Incorreto
private const string conexaoString = "Data Source=...";
```

### Organização de Código

#### Regiões (Regions)
```csharp
public partial class UsuarioForm : Form
{
    #region Campos e Propriedades
    private List<Usuario> usuarios;
    private Usuario usuarioSelecionado;
    #endregion

    #region Construtores
    public UsuarioForm()
    {
        InitializeComponent();
    }
    #endregion

    #region Eventos
    private void btnCadastrar_Click(object sender, EventArgs e)
    {
        // Implementação
    }
    #endregion

    #region Métodos Públicos
    public void AtualizarLista()
    {
        // Implementação
    }
    #endregion

    #region Métodos Privados
    private bool ValidarCampos()
    {
        // Implementação
    }
    #endregion

    #region Métodos de Banco de Dados
    private void CarregarUsuarios()
    {
        // Implementação
    }
    #endregion
}
```

#### Comentários e Documentação
```csharp
/// <summary>
/// Valida os campos obrigatórios do formulário de usuário
/// </summary>
/// <returns>True se todos os campos estão válidos, false caso contrário</returns>
private bool ValidarCampos()
{
    // Verifica se o nome foi preenchido
    if (string.IsNullOrWhiteSpace(txtNome.Text))
    {
        MessageBox.Show("Nome é obrigatório", "Validação", 
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    // TODO: Adicionar validação de CPF
    // HACK: Validação temporária até implementar regex
    
    return true;
}
```

## 🏗 Padrões de Desenvolvimento

### MVP Pattern

#### Estrutura Básica
```csharp
// Model
public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    // ... outras propriedades

    public bool ValidarDados()
    {
        return !string.IsNullOrWhiteSpace(Nome);
    }
}

// View (Interface)
public interface IUsuarioView
{
    string Nome { get; set; }
    void ExibirMensagem(string mensagem);
    void LimparCampos();
}

// Presenter
public class UsuarioPresenter
{
    private readonly IUsuarioView view;
    
    public UsuarioPresenter(IUsuarioView view)
    {
        this.view = view;
    }
    
    public void SalvarUsuario()
    {
        var usuario = new Usuario { Nome = view.Nome };
        
        if (!usuario.ValidarDados())
        {
            view.ExibirMensagem("Dados inválidos");
            return;
        }
        
        // Salvar no banco...
        view.ExibirMensagem("Usuário salvo com sucesso");
        view.LimparCampos();
    }
}
```

### Repository Pattern (Conceitual)

```csharp
public interface IUsuarioRepository
{
    void Inserir(Usuario usuario);
    Usuario BuscarPorId(int id);
    List<Usuario> BuscarTodos();
    void Atualizar(Usuario usuario);
    void Excluir(int id);
}

public class UsuarioRepository : IUsuarioRepository
{
    public void Inserir(Usuario usuario)
    {
        using (var conexao = Conexao.ObterConexao())
        {
            conexao.Open();
            var comando = new SqlCeCommand(
                "INSERT INTO Usuario (Nome, Email) VALUES (@nome, @email)", 
                conexao);
                
            comando.Parameters.AddWithValue("@nome", usuario.Nome);
            comando.Parameters.AddWithValue("@email", usuario.Email);
            
            comando.ExecuteNonQuery();
        }
    }
    
    // ... outras implementações
}
```

### Service Layer

```csharp
public class UsuarioService
{
    private readonly IUsuarioRepository repository;
    
    public UsuarioService(IUsuarioRepository repository)
    {
        this.repository = repository;
    }
    
    public void CadastrarUsuario(Usuario usuario)
    {
        // Validações de negócio
        if (ExisteUsuarioComCpf(usuario.CPF))
        {
            throw new InvalidOperationException("CPF já cadastrado");
        }
        
        // Criptografar senha
        usuario.Senha = CriptografiaSenha.CriptografarSenha(usuario.Senha);
        
        // Salvar
        repository.Inserir(usuario);
        
        // Log da operação
        Log.Info($"Usuário {usuario.Nome} cadastrado com sucesso");
    }
}
```

## 🗃 Banco de Dados

### Estrutura das Tabelas

#### Tabela Usuario
```sql
CREATE TABLE [Usuario] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Nome] NVARCHAR(100) NOT NULL,
    [TipoUsuario] NVARCHAR(20) NOT NULL,
    [CPF] NVARCHAR(14) NULL,
    [DataNascimento] DATETIME NULL,
    [Telefone] NVARCHAR(15) NULL,
    [Email] NVARCHAR(100) NULL,
    [Turma] NVARCHAR(50) NULL,
    [Senha] NVARCHAR(255) NOT NULL,
    [DataCadastro] DATETIME DEFAULT GETDATE()
);
```

#### Tabela Livro
```sql
CREATE TABLE [Livro] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Nome] NVARCHAR(200) NOT NULL,
    [Autor] NVARCHAR(100) NOT NULL,
    [Genero] NVARCHAR(50) NOT NULL,
    [Quantidade] INT NOT NULL DEFAULT 1,
    [Disponibilidade] BIT NOT NULL DEFAULT 1,
    [CodigoDeBarras] NVARCHAR(50) NULL,
    [DataCadastro] DATETIME DEFAULT GETDATE()
);
```

#### Tabela Emprestimo
```sql
CREATE TABLE [Emprestimo] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [UsuarioId] INT NOT NULL,
    [LivroId] INT NOT NULL,
    [DataEmprestimo] DATETIME NOT NULL,
    [DataPrevistaDevolucao] DATETIME NOT NULL,
    [DataRealDevolucao] DATETIME NULL,
    [ResponsavelId] INT NOT NULL,
    [Observacoes] NVARCHAR(500) NULL,
    
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id),
    FOREIGN KEY (LivroId) REFERENCES Livro(Id),
    FOREIGN KEY (ResponsavelId) REFERENCES Usuario(Id)
);
```

### Padrões de Acesso a Dados

#### Conexão Segura
```csharp
public static class Conexao
{
    private static readonly string connectionString = 
        $"Data Source={CaminhoBanco}; Password=123";
    
    public static SqlCeConnection ObterConexao()
    {
        return new SqlCeConnection(connectionString);
    }
    
    // Método helper para execução de comandos
    public static T ExecutarComando<T>(string sql, Func<SqlCeCommand, T> executor, 
        params SqlCeParameter[] parametros)
    {
        using (var conexao = ObterConexao())
        {
            conexao.Open();
            using (var comando = new SqlCeCommand(sql, conexao))
            {
                if (parametros != null)
                    comando.Parameters.AddRange(parametros);
                    
                return executor(comando);
            }
        }
    }
}
```

#### Exemplo de Uso
```csharp
public List<Usuario> BuscarUsuariosPorTipo(string tipo)
{
    var sql = "SELECT * FROM Usuario WHERE TipoUsuario = @tipo";
    var parametro = new SqlCeParameter("@tipo", tipo);
    
    return Conexao.ExecutarComando(sql, comando =>
    {
        var usuarios = new List<Usuario>();
        using (var reader = comando.ExecuteReader())
        {
            while (reader.Read())
            {
                usuarios.Add(new Usuario
                {
                    Id = reader.GetInt32("Id"),
                    Nome = reader.GetString("Nome"),
                    TipoUsuario = reader.GetString("TipoUsuario")
                    // ... outros campos
                });
            }
        }
        return usuarios;
    }, parametro);
}
```

## 🧪 Testes

### Estrutura de Testes (Proposta)

```csharp
[TestClass]
public class UsuarioServiceTests
{
    private UsuarioService service;
    private Mock<IUsuarioRepository> mockRepository;
    
    [TestInitialize]
    public void Setup()
    {
        mockRepository = new Mock<IUsuarioRepository>();
        service = new UsuarioService(mockRepository.Object);
    }
    
    [TestMethod]
    public void CadastrarUsuario_ComDadosValidos_DeveSalvarComSucesso()
    {
        // Arrange
        var usuario = new Usuario 
        { 
            Nome = "João Silva", 
            CPF = "123.456.789-00" 
        };
        
        // Act
        service.CadastrarUsuario(usuario);
        
        // Assert
        mockRepository.Verify(r => r.Inserir(usuario), Times.Once);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CadastrarUsuario_ComCpfDuplicado_DeveLancarExcecao()
    {
        // Arrange
        var usuario = new Usuario { CPF = "123.456.789-00" };
        mockRepository.Setup(r => r.ExisteCpf(usuario.CPF)).Returns(true);
        
        // Act
        service.CadastrarUsuario(usuario);
        
        // Assert é feito pela exceção esperada
    }
}
```

### Testes de Interface (Manual)

#### Checklist de Testes
```markdown
## Tela de Login
- [ ] Login com credenciais válidas
- [ ] Login com credenciais inválidas
- [ ] Validação de campos obrigatórios
- [ ] Funcionalidade "Esqueci a senha"

## Cadastro de Usuários
- [ ] Cadastro com dados completos
- [ ] Validação de CPF
- [ ] Validação de email
- [ ] Campos obrigatórios
- [ ] Duplicação de dados

## Empréstimos
- [ ] Empréstimo com livro disponível
- [ ] Tentativa de empréstimo com livro indisponível
- [ ] Validação de datas
- [ ] Cálculo de multas
```

## 🔍 Debugging e Logging

### Configuração de Debug

#### Visual Studio
1. **Breakpoints**: Configurar pontos de parada estratégicos
2. **Watch**: Monitorar variáveis específicas
3. **Immediate Window**: Testar expressões em runtime

#### Exemplo de Logging
```csharp
public static class Logger
{
    private static readonly string logPath = 
        Path.Combine(AppPaths.AppDataFolder, "logs");
    
    public static void Info(string mensagem)
    {
        EscreverLog("INFO", mensagem);
    }
    
    public static void Error(string mensagem, Exception ex = null)
    {
        var logEntry = ex != null ? 
            $"{mensagem}\nException: {ex}" : mensagem;
        EscreverLog("ERROR", logEntry);
    }
    
    private static void EscreverLog(string nivel, string mensagem)
    {
        var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{nivel}] {mensagem}";
        
        try
        {
            Directory.CreateDirectory(logPath);
            var fileName = $"app_{DateTime.Now:yyyyMMdd}.log";
            File.AppendAllText(Path.Combine(logPath, fileName), 
                logEntry + Environment.NewLine);
        }
        catch
        {
            // Falha silenciosa em logging
        }
    }
}
```

### Debugging de Problemas Comuns

#### Problemas de Conexão
```csharp
public static void TestarConexao()
{
    try
    {
        using (var conexao = Conexao.ObterConexao())
        {
            conexao.Open();
            Logger.Info("Conexão com banco estabelecida com sucesso");
        }
    }
    catch (Exception ex)
    {
        Logger.Error("Erro ao conectar com banco", ex);
        MessageBox.Show($"Erro de conexão: {ex.Message}");
    }
}
```

## 🚀 Build e Deploy

### Configuração de Build

#### Configurações do Projeto
```xml
<!-- Exemplo de PropertyGroup para Release -->
<PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
</PropertyGroup>
```

#### Script de Build
```batch
@echo off
echo === Build BibliotecaApp ===

:: Limpar build anterior
if exist bin\Release rmdir /s /q bin\Release

:: Build em modo Release
msbuild BibliotecaApp.sln /p:Configuration=Release /p:Platform="Any CPU"

if %ERRORLEVEL% EQU 0 (
    echo Build concluído com sucesso!
) else (
    echo Erro no build!
    exit /b 1
)

:: Copiar arquivos necessários
copy BibliotecaDB\bibliotecaDB.sdf bin\Release\BibliotecaDB\
copy credentials.json bin\Release\
copy publicKey.xml bin\Release\

echo === Build finalizado ===
pause
```

### Versionamento

#### AssemblyInfo.cs
```csharp
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0-beta")]
```

#### Git Tags
```bash
# Criar tag de versão
git tag -a v1.0.0 -m "Versão 1.0.0 - Release inicial"

# Enviar tags
git push origin --tags
```

## 🤝 Contribuição

### Fluxo de Desenvolvimento

1. **Fork** do repositório
2. **Clone** do fork local
3. **Branch** para feature/bugfix
4. **Desenvolvimento** com commits pequenos
5. **Testes** locais
6. **Pull Request** com descrição detalhada

### Commit Messages

#### Formato
```
tipo(escopo): descrição curta

Descrição mais detalhada se necessário

Fixes #123
```

#### Exemplos
```bash
feat(usuarios): adiciona validação de CPF no cadastro

Implementa validação de CPF usando algoritmo padrão
Adiciona mensagens de erro específicas

Fixes #45

fix(emprestimos): corrige cálculo de data de devolução

O cálculo estava considerando dias úteis incorretamente

refactor(conexao): melhora tratamento de exceções
```

### Code Review

#### Checklist
- [ ] Código segue convenções estabelecidas
- [ ] Funcionalidade testada manualmente
- [ ] Não quebra funcionalidades existentes
- [ ] Documentação atualizada se necessário
- [ ] Performance adequada
- [ ] Tratamento de erros implementado

---

Este guia serve como referência para desenvolvedores trabalhando no BibliotecaApp. Mantenha-o atualizado conforme o projeto evolui.