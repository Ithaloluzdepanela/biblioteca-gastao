# Guia de Instalação - BibliotecaApp

## 📋 Índice

- [Requisitos do Sistema](#requisitos-do-sistema)
- [Preparação do Ambiente](#preparação-do-ambiente)
- [Processo de Instalação](#processo-de-instalação)
- [Configuração Inicial](#configuração-inicial)
- [Primeiro Acesso](#primeiro-acesso)
- [Configurações Opcionais](#configurações-opcionais)
- [Solução de Problemas](#solução-de-problemas)
- [Backup e Restauração](#backup-e-restauração)

## 💻 Requisitos do Sistema

### Requisitos Mínimos

- **Sistema Operacional**: Windows 7 SP1 ou superior
- **Processador**: Intel Pentium 4 ou AMD equivalente (1.5 GHz)
- **Memória RAM**: 2 GB
- **Espaço em Disco**: 500 MB livres
- **Resolução de Tela**: 1024x768
- **Framework**: .NET Framework 4.8

### Requisitos Recomendados

- **Sistema Operacional**: Windows 10 ou Windows 11
- **Processador**: Intel Core i3 ou AMD equivalente (2.0 GHz)
- **Memória RAM**: 4 GB ou mais
- **Espaço em Disco**: 2 GB livres
- **Resolução de Tela**: 1366x768 ou superior
- **Conectividade**: Internet para backups automáticos (opcional)

### Dependências de Software

#### Obrigatórias
1. **.NET Framework 4.8**
   - Download: [Microsoft .NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
   - Necessário para execução da aplicação

2. **SQL Server Compact Edition 4.0**
   - Download: [Microsoft SQL Server Compact 4.0](https://www.microsoft.com/en-us/download/details.aspx?id=30709)
   - Necessário para acesso ao banco de dados

#### Opcionais
3. **Microsoft Visual C++ Redistributable**
   - Pode ser necessário em alguns sistemas
   - Download: [Microsoft Visual C++ Downloads](https://support.microsoft.com/help/2977003/the-latest-supported-visual-c-downloads)

## 🔧 Preparação do Ambiente

### Verificação de Pré-requisitos

1. **Verificar .NET Framework**
   - Abra o Prompt de Comando
   - Digite: `reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" /v Release`
   - Se o valor for >= 528040, o .NET 4.8 está instalado

2. **Verificar SQL Server CE**
   - Verifique se existe a pasta: `C:\Program Files\Microsoft SQL Server Compact Edition`
   - Ou tente executar: `sqlcmd` no prompt (deve reconhecer o comando)

### Download do Sistema

1. **Obtenção dos Arquivos**
   - Baixe o pacote BibliotecaApp.zip
   - Ou clone o repositório: `git clone https://github.com/Ithaloluzdepanela/biblioteca-gastao.git`

2. **Verificação de Integridade**
   - Confirme que todos os arquivos foram baixados corretamente
   - Verifique se não há arquivos corrompidos

## 📦 Processo de Instalação

### Instalação Simples (Usuário Final)

1. **Extrair Arquivos**
   ```
   1. Extraia BibliotecaApp.zip para uma pasta de sua escolha
   2. Recomendado: C:\Biblioteca\ ou C:\Program Files\BibliotecaApp\
   3. Evite pastas com espaços ou caracteres especiais
   ```

2. **Estrutura de Arquivos**
   ```
   BibliotecaApp/
   ├── BibliotecaApp.exe          # Executável principal
   ├── publicKey.xml              # Chave de licença (OBRIGATÓRIO)
   ├── credentials.json           # Credenciais Google Drive (opcional)
   ├── App.config                 # Configurações da aplicação
   ├── BibliotecaDB/             # Pasta do banco de dados
   │   └── bibliotecaDB.sdf      # Arquivo do banco
   ├── [Diversas DLLs]           # Bibliotecas necessárias
   └── Resources/                # Recursos (imagens, ícones)
   ```

3. **Arquivo de Licença**
   - **IMPORTANTE**: Coloque o arquivo `publicKey.xml` na pasta raiz
   - Sem este arquivo, o sistema não iniciará
   - Obtenha o arquivo com o fornecedor do sistema

### Instalação para Desenvolvimento

1. **Clone do Repositório**
   ```bash
   git clone https://github.com/Ithaloluzdepanela/biblioteca-gastao.git
   cd biblioteca-gastao/BibliotecaApp
   ```

2. **Restaurar Pacotes NuGet**
   ```bash
   # No Visual Studio: Tools > NuGet Package Manager > Restore
   # Ou via comando:
   nuget restore BibliotecaApp.sln
   ```

3. **Build da Solução**
   ```bash
   # Visual Studio: Build > Build Solution
   # Ou via comando:
   msbuild BibliotecaApp.sln /p:Configuration=Release
   ```

## ⚙️ Configuração Inicial

### Primeira Execução

1. **Executar BibliotecaApp.exe**
   - Clique duplo no arquivo executável
   - O sistema verificará automaticamente as dependências

2. **Verificação de Licença**
   - Se `publicKey.xml` não estiver presente, será exibido um aviso
   - Coloque o arquivo na pasta e reinicie

3. **Termos de Uso**
   - Leia os termos de uso cuidadosamente
   - Clique em "Aceitar" para continuar
   - É obrigatório aceitar para usar o sistema

4. **Ativação da Licença**
   - Insira a chave de ativação fornecida
   - Formato típico: XXXX-XXXX-XXXX-XXXX
   - Clique em "Ativar"
   - Aguarde a confirmação

### Configuração do Banco de Dados

O banco de dados é configurado automaticamente na primeira execução:

1. **Localização**: `BibliotecaApp/BibliotecaDB/bibliotecaDB.sdf`
2. **Senha**: `123` (padrão)
3. **Tamanho Inicial**: Aproximadamente 1 MB
4. **Limite**: 4 GB (SQL Server CE)

#### Estrutura Criada Automaticamente
- Tabela `Usuario`: Dados de usuários do sistema
- Tabela `Livro`: Acervo da biblioteca
- Tabela `Emprestimo`: Controle de empréstimos
- Tabela `Log`: Registro de atividades (se aplicável)

## 👤 Primeiro Acesso

### Credenciais Padrão

**Para primeira configuração, use:**
- **Usuário**: `admin`
- **Senha**: `admin123`

**⚠️ IMPORTANTE**: Altere essas credenciais imediatamente após o primeiro login!

### Configuração do Administrador

1. **Login Inicial**
   - Use as credenciais padrão
   - Sistema solicitará alteração de senha

2. **Criação de Usuários**
   - Acesse: Menu → Usuários → Cadastrar
   - Crie pelo menos um bibliotecário
   - Configure tipos de usuário apropriados

3. **Configurações Básicas**
   - Verifique configurações do sistema
   - Configure backups (se necessário)
   - Teste funcionalidades principais

### Validação da Instalação

Execute os seguintes testes para garantir que tudo está funcionando:

1. **Teste de Login**
   - ✅ Login com credenciais padrão
   - ✅ Logout e novo login

2. **Teste de Usuários**
   - ✅ Cadastrar novo usuário
   - ✅ Editar usuário existente
   - ✅ Pesquisar usuários

3. **Teste de Livros**
   - ✅ Cadastrar novo livro
   - ✅ Pesquisar no acervo
   - ✅ Verificar disponibilidade

4. **Teste de Empréstimos**
   - ✅ Realizar empréstimo
   - ✅ Processar devolução
   - ✅ Verificar histórico

5. **Teste de Relatórios**
   - ✅ Gerar relatório simples
   - ✅ Exportar para Excel
   - ✅ Verificar dados

## 🔧 Configurações Opcionais

### Backup Automático com Google Drive

1. **Obter Credenciais**
   - Acesse [Google Cloud Console](https://console.cloud.google.com/)
   - Crie um projeto ou use existente
   - Ative a API do Google Drive
   - Baixe o arquivo `credentials.json`

2. **Configurar no Sistema**
   - Coloque `credentials.json` na pasta raiz
   - Descomente as linhas de backup no `Program.cs`
   - Recompile a aplicação

3. **Primeiro Backup**
   - Sistema solicitará autorização do Google
   - Siga as instruções na tela
   - Autorização é salva para usos futuros

### Personalização da Interface

1. **Ícones e Logos**
   - Substitua arquivos na pasta `Resources/`
   - Mantenha os mesmos nomes e formatos
   - Recomendado: PNG com transparência

2. **Cores e Temas**
   - Modifique arquivos `.Designer.cs` dos Forms
   - Use Material Design guidelines
   - Teste em diferentes resoluções

### Configuração de Rede

Para uso em múltiplos computadores (futuro):

1. **Compartilhamento de Banco**
   - Atualmente não suportado (SQL CE é single-user)
   - Planeje migração para SQL Server Express

2. **Backup Centralizado**
   - Configure backup em pasta de rede
   - Use scripts automatizados
   - Monitore espaço disponível

## ❌ Solução de Problemas

### Problemas Comuns

#### "Aplicação não inicia"
**Possíveis causas e soluções:**

1. **.NET Framework não instalado**
   - Instale .NET Framework 4.8
   - Reinicie o computador

2. **publicKey.xml ausente**
   - Copie o arquivo para a pasta da aplicação
   - Verifique se não há caracteres especiais no nome

3. **SQL Server CE não instalado**
   - Instale SQL Server Compact Edition 4.0
   - Execute como administrador se necessário

#### "Erro de conexão com banco"
**Diagnóstico e solução:**

1. **Verificar arquivo de banco**
   ```
   - Caminho: BibliotecaApp\BibliotecaDB\bibliotecaDB.sdf
   - Tamanho: Deve ser > 0 bytes
   - Permissões: Leitura/escrita para o usuário
   ```

2. **Testar conexão**
   - Use ferramenta de diagnóstico do sistema
   - Verifique mensagens de erro específicas

3. **Restaurar banco**
   - Use backup mais recente
   - Ou recrie banco vazio (perda de dados)

#### "Licença expirada"
**Soluções:**

1. **Verificar data do sistema**
   - Confirme se a data está correta
   - Não altere data para "burlar" licença

2. **Renovar licença**
   - Entre em contato com fornecedor
   - Obtenha nova chave de ativação

#### "Erro de permissões"
**Correções:**

1. **Executar como administrador**
   - Clique direito → "Executar como administrador"
   - Configure permissões da pasta

2. **Antivírus bloqueando**
   - Adicione exceção no antivírus
   - Temporariamente desabilite proteção

### Logs e Diagnóstico

#### Localização de Logs
```
C:\Users\[Usuário]\AppData\Local\BibliotecaApp\Logs\
```

#### Tipos de Log
- `app_[data].log`: Log geral da aplicação
- `error_[data].log`: Erros específicos
- `backup_[data].log`: Logs de backup

#### Coleta de Informações para Suporte

Antes de entrar em contato com o suporte, colete:

1. **Informações do Sistema**
   - Versão do Windows
   - Versão do .NET Framework
   - Versão do BibliotecaApp

2. **Arquivos de Log**
   - Últimos 3 dias de logs
   - Screenshot da mensagem de erro

3. **Reprodução do Problema**
   - Passos exatos para reproduzir
   - Quando o problema começou
   - Frequência de ocorrência

## 💾 Backup e Restauração

### Backup Manual

#### Arquivos Essenciais para Backup
```
BibliotecaApp/
├── BibliotecaDB/bibliotecaDB.sdf    # PRINCIPAL - Dados
├── publicKey.xml                    # Licença
├── credentials.json                 # Configurações (se usado)
└── App.config                       # Configurações locais
```

#### Procedimento de Backup
1. **Feche o BibliotecaApp completamente**
2. **Copie os arquivos essenciais**
3. **Comprima em arquivo ZIP**
4. **Armazene em local seguro**
5. **Teste o backup periodicamente**

### Restauração

#### Procedimento Completo
1. **Instale o BibliotecaApp** (versão igual ou superior)
2. **Substitua bibliotecaDB.sdf** pelo backup
3. **Copie demais arquivos** de configuração
4. **Execute e teste** todas as funcionalidades

#### Restauração Parcial (apenas dados)
1. **Feche a aplicação**
2. **Substitua apenas bibliotecaDB.sdf**
3. **Reinicie e verifique** integridade

### Agenda de Backup Recomendada

- **Diário**: Backup automático (se configurado)
- **Semanal**: Backup manual verificado
- **Mensal**: Backup completo arquivado
- **Semestral**: Teste de restauração

---

## 📞 Suporte Técnico

### Contatos
- **Email**: [inserir email de suporte]
- **Telefone**: [inserir telefone]
- **Site**: [inserir site]

### Antes de Entrar em Contato
1. Consulte este guia de instalação
2. Verifique os logs de erro
3. Tente soluções básicas (reiniciar, reboot)
4. Prepare informações do sistema

### Informações para Suporte
- Versão do sistema operacional
- Versão do BibliotecaApp
- Arquivo de log mais recente
- Screenshot do erro (se aplicável)
- Descrição detalhada do problema

---

Este guia cobre os cenários mais comuns de instalação. Para situações específicas ou personalizadas, consulte a documentação técnica ou entre em contato com o suporte.