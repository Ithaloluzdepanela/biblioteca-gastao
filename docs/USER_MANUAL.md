# Manual do Usuário - BibliotecaApp

## 📋 Índice

- [Introdução](#introdução)
- [Primeiros Passos](#primeiros-passos)
- [Interface Principal](#interface-principal)
- [Gestão de Usuários](#gestão-de-usuários)
- [Gestão de Livros](#gestão-de-livros)
- [Empréstimos e Devoluções](#empréstimos-e-devoluções)
- [Relatórios](#relatórios)
- [Funcionalidades Especiais](#funcionalidades-especiais)
- [Solução de Problemas](#solução-de-problemas)
- [Dicas e Truques](#dicas-e-truques)

## 📖 Introdução

O **BibliotecaApp** é um sistema completo para gerenciamento de bibliotecas que permite controlar usuários, livros, empréstimos, devoluções e gerar relatórios detalhados. Este manual irá guiá-lo através de todas as funcionalidades do sistema.

### Tipos de Usuário

O sistema possui diferentes níveis de acesso:

- **Administrador**: Acesso completo a todas as funcionalidades
- **Bibliotecário**: Gestão de livros, empréstimos e relatórios
- **Professor**: Empréstimos e consultas
- **Aluno**: Consultas limitadas (quando aplicável)

### Requisitos do Sistema

- **Sistema Operacional**: Windows 7 ou superior
- **Framework**: .NET Framework 4.8
- **Resolução**: Mínima 1024x768 (recomendado 1366x768 ou superior)
- **Espaço em Disco**: 200MB livres

## 🚀 Primeiros Passos

### Instalação e Ativação

1. **Primeira Execução**
   - Execute o arquivo `BibliotecaApp.exe`
   - O sistema solicitará aceitar os termos de uso
   - Leia e aceite os termos clicando em "Aceitar"

2. **Ativação da Licença**
   - Insira a chave de ativação fornecida
   - Clique em "Ativar"
   - Aguarde a confirmação da ativação

3. **Primeiro Login**
   - Use as credenciais fornecidas pelo administrador
   - Para primeiro acesso, use as credenciais padrão (se aplicável)

### Tela de Login

![Tela de Login](screenshots/login.png)

1. **Campo Usuário**: Digite seu nome de usuário ou CPF
2. **Campo Senha**: Digite sua senha
3. **Botão Entrar**: Clica para fazer login
4. **Esqueci a Senha**: Link para recuperação de senha
5. **Sobre**: Informações sobre o sistema

**Dica**: O sistema lembra do último usuário logado (apenas o nome, não a senha).

## 🏠 Interface Principal

### Dashboard

Após o login, você verá o dashboard principal com:

#### Painel de Estatísticas
- **Total de Usuários**: Número de usuários cadastrados
- **Total de Livros**: Número de exemplares no acervo
- **Empréstimos Ativos**: Livros atualmente emprestados
- **Atrasados**: Empréstimos em atraso
- **Reservas Pendentes**: Reservas aguardando
- **Empréstimos Rápidos**: Movimentação do dia

#### Menu Lateral
- **Início**: Retorna ao dashboard
- **Usuários**: Gestão de usuários
- **Livros**: Gestão do acervo
- **Empréstimos**: Controle de empréstimos
- **Devoluções**: Processamento de devoluções
- **Relatórios**: Geração de relatórios
- **Configurações**: Ajustes do sistema

#### Botão Empréstimo Rápido
Localizado no canto superior direito, permite fazer empréstimos de forma ágil.

### Navegação

- **Menu Principal**: Use os botões do menu lateral
- **Breadcrumb**: Mostra onde você está no sistema
- **Botões de Ação**: Sempre visíveis na parte superior
- **Teclas de Atalho**: F1 (Ajuda), F5 (Atualizar), Esc (Fechar)

## 👥 Gestão de Usuários

### Listagem de Usuários

1. **Acesso**: Menu → Usuários
2. **Visualização**: Lista todos os usuários cadastrados
3. **Busca**: Use o campo de pesquisa para filtrar
4. **Filtros**: Por tipo de usuário, turma ou status

#### Colunas da Lista
- **Nome**: Nome completo do usuário
- **Tipo**: Aluno, Professor, Bibliotecário, etc.
- **Turma**: Turma do aluno (quando aplicável)
- **CPF**: Documento de identificação
- **Status**: Ativo/Inativo

### Cadastro de Usuários

1. **Acesso**: Usuários → Cadastrar Novo
2. **Preenchimento dos Campos**:

#### Dados Pessoais
- **Nome Completo** ⭐ (obrigatório)
- **CPF**: Formato XXX.XXX.XXX-XX (opcional)
- **Data de Nascimento**: Use o seletor de data
- **Telefone**: (XX) XXXXX-XXXX
- **Email**: Para recuperação de senha

#### Dados Institucionais
- **Tipo de Usuário** ⭐ (obrigatório)
  - Aluno: Para estudantes
  - Professor: Para docentes
  - Bibliotecário: Para funcionários da biblioteca
  - Administrador: Para gestores do sistema
  - Outros: Para visitantes ou casos especiais

- **Turma**: Obrigatório para alunos
  - Use o formato: "3° Desenvolvimento A"
  - O sistema oferece sugestões automáticas
  - Para turmas do ensino fundamental: "6° Ano A", "7° Ano B"

#### Credenciais de Acesso
- **Senha** ⭐ (obrigatório)
- **Confirmar Senha** ⭐ (obrigatório)

**Dica**: Use senhas com pelo menos 6 caracteres, combinando letras e números.

3. **Salvamento**
   - Clique em "Salvar" para finalizar
   - O sistema validará todos os campos
   - Senha será criptografada automaticamente

### Edição de Usuários

1. **Acesso**: Usuários → Selecionar usuário → Editar
2. **Alterações**: Modifique os campos necessários
3. **Senha**: Deixe em branco para manter a atual
4. **Confirmação**: Clique em "Salvar Alterações"

### Exclusão de Usuários

1. **Seleção**: Clique no usuário desejado
2. **Exclusão**: Botão "Excluir" → Confirmar
3. **Observação**: Usuários com histórico não podem ser excluídos, apenas desativados

## 📚 Gestão de Livros

### Listagem de Livros

1. **Acesso**: Menu → Livros
2. **Visualização**: Grade com todos os livros

#### Informações Exibidas
- **Título**: Nome do livro
- **Autor**: Autor principal
- **Gênero**: Categoria do livro
- **Quantidade Total**: Exemplares no acervo
- **Disponíveis**: Exemplares não emprestados
- **Status**: Disponível/Indisponível

#### Filtros e Busca
- **Busca por Título**: Digite no campo de pesquisa
- **Filtro por Gênero**: Use o dropdown
- **Filtro por Autor**: Busca por nome do autor
- **Disponibilidade**: Apenas disponíveis/todos

### Cadastro de Livros

1. **Acesso**: Livros → Cadastrar Novo

#### Informações do Livro
- **Título** ⭐ (obrigatório): Nome completo do livro
- **Autor** ⭐ (obrigatório): Nome do autor principal
- **Gênero** ⭐ (obrigatório): Use as sugestões automáticas

#### Gêneros Padronizados
O sistema oferece sugestões para manter consistência:
- Literatura: Romance, Ficção, Fantasia, Poesia
- Educacional: Didático, Literatura Infantil, Infantojuvenil
- Especializado: História, Filosofia, Psicologia
- Outros: Biografia, Culinária, Teatro

**Dica**: Digite as primeiras letras e selecione da lista de sugestões.

#### Controle de Estoque
- **Quantidade** ⭐ (obrigatório): Número de exemplares
- **Código de Barras**: Para controle automatizado (opcional)

**Dica**: Para livros sem código, o sistema pode gerar um automaticamente.

### Edição de Livros

1. **Seleção**: Clique no livro desejado
2. **Edição**: Botão "Editar"
3. **Alterações**: Modifique os campos necessários
4. **Quantidade**: Cuidado ao alterar com empréstimos ativos

### Controle de Disponibilidade

- **Automático**: Atualizado automaticamente com empréstimos/devoluções
- **Manual**: Admin pode marcar como indisponível temporariamente
- **Indicadores Visuais**: 
  - 🟢 Verde: Disponível
  - 🟡 Amarelo: Poucos exemplares
  - 🔴 Vermelho: Indisponível

## 📖 Empréstimos e Devoluções

### Empréstimo Tradicional

1. **Acesso**: Menu → Empréstimos → Novo Empréstimo

#### Processo Passo a Passo

**Passo 1: Seleção do Usuário**
- Busque por nome, CPF ou turma
- Clique no usuário desejado
- Verifique se não há pendências

**Passo 2: Seleção do Livro**
- Busque por título, autor ou código de barras
- Confirme disponibilidade
- Selecione o livro

**Passo 3: Configuração do Empréstimo**
- **Data de Empréstimo**: Preenchida automaticamente (hoje)
- **Data de Devolução**: Configure prazo desejado
  - Padrão: 7 dias para alunos, 14 dias para professores
  - Máximo: 30 dias
- **Observações**: Comentários opcionais

**Passo 4: Confirmação**
- Revise as informações
- Clique em "Confirmar Empréstimo"
- O sistema atualizará automaticamente o estoque

### Empréstimo Rápido

Para agilizar o processo em momentos de movimento intenso:

1. **Acesso**: Botão "Empréstimo Rápido" (dashboard) ou tecla F2

#### Processo Simplificado

**Campo Usuário**:
- Digite nome, parte do nome ou turma
- Exemplo: "João", "João Silva", "3° Desenvolvimento"
- Sistema oferece autocomplete

**Campo Livro**:
- Digite título ou escaneie código de barras
- Sistema busca automaticamente

**Confirmação Rápida**:
- Prazo padrão aplicado automaticamente
- Clique em "Confirmar" ou pressione Enter
- Pronto para o próximo empréstimo

**Dica**: Ideal para horários de pico, como intervalos e saídas.

### Devolução

1. **Acesso**: Menu → Devoluções

#### Métodos de Busca

**Por Livro**:
- Escaneie código de barras
- Digite título do livro
- Sistema mostra quem está com o livro

**Por Usuário**:
- Digite nome ou CPF
- Lista todos os livros emprestados
- Selecione o livro a devolver

#### Processo de Devolução

1. **Identificação**: Encontre o empréstimo
2. **Verificação**: Confirme estado do livro
3. **Observações**: Anote danos ou observações
4. **Confirmação**: Clique em "Processar Devolução"

#### Tratamento de Atrasos

- **Cálculo Automático**: Sistema calcula dias de atraso
- **Notificação**: Alerta visual para atrasos
- **Observações**: Registre acordos ou observações
- **Relatório**: Atraso fica registrado para relatórios

### Reservas

Para livros momentaneamente indisponíveis:

1. **Quando Usar**: Todos os exemplares emprestados
2. **Como Fazer**: No cadastro de empréstimo, opção "Reservar"
3. **Notificação**: Sistema avisa quando livro estiver disponível
4. **Prioridade**: Primeira reserva tem prioridade

## 📊 Relatórios

### Tipos de Relatórios

#### Relatório de Empréstimos
**Finalidade**: Acompanhar movimentação da biblioteca

**Filtros Disponíveis**:
- **Período**: Data inicial e final
- **Usuário**: Empréstimos de usuário específico
- **Livro**: Histórico de livro específico
- **Tipo de Ação**: Empréstimo, Devolução, Reserva
- **Responsável**: Bibliotecário que fez o registro

**Informações Exibidas**:
- Data e hora da ação
- Nome do usuário
- Título do livro
- Tipo de ação
- Responsável pelo registro
- Observações

#### Relatório de Usuários com Atraso
**Finalidade**: Controle de inadimplência

**Informações**:
- Nome do usuário
- Turma (se aluno)
- Livro em atraso
- Data prevista de devolução
- Dias de atraso
- Telefone para contato

#### Relatório de Livros Populares
**Finalidade**: Análise do acervo

**Dados**:
- Título do livro
- Número total de empréstimos
- Média de empréstimos por mês
- Última data de empréstimo
- Classificação por popularidade

### Geração de Relatórios

1. **Acesso**: Menu → Relatórios
2. **Seleção**: Escolha o tipo de relatório
3. **Filtros**: Configure os parâmetros desejados
4. **Visualização**: Clique em "Gerar Relatório"

### Exportação

#### Formatos Disponíveis

**Excel (.xlsx)**:
- Formatação profissional automática
- Filtros e ordenação habilitados
- Fórmulas para totalizações
- Gráficos automáticos (quando aplicável)

**PDF**:
- Layout otimizado para impressão
- Cabeçalho com logo da instituição
- Numeração de páginas
- Data de geração

#### Processo de Exportação

1. **Geração**: Primeiro gere o relatório na tela
2. **Exportação**: Clique em "Exportar"
3. **Formato**: Escolha Excel ou PDF
4. **Local**: Sistema abre a pasta automaticamente
5. **Nome**: Formato automático com data e hora

**Exemplo de nome**: `Relatorio_Emprestimos_2024-03-15_14-30.xlsx`

## ⭐ Funcionalidades Especiais

### Mapeamento de Turmas

**Finalidade**: Atualização anual de turmas de alunos

**Quando Usar**: 
- Início do ano letivo
- Remanejamento de alunos
- Alunos transferidos ou evadidos

**Como Funcionar**:
1. **Acesso**: Menu → Usuários → Mapeamento de Turmas
2. **Seleção**: Escolha alunos a serem remapeados
3. **Nova Turma**: Configure destino
4. **Inteligência**: Sistema sugere progressões lógicas
5. **Confirmação**: Aplica alterações em lote

**Sugestões Inteligentes**:
- 1° Desenvolvimento → 2° Desenvolvimento
- 6° Ano → 7° Ano
- 3° Ensino Médio → CONCLUÍDO

### Ficha do Aluno

**Finalidade**: Visão completa do histórico

**Informações Exibidas**:
- Dados pessoais completos
- Histórico de empréstimos
- Livros atualmente emprestados
- Estatísticas de uso
- Gráfico de atividade mensal

**Como Acessar**:
1. Menu → Usuários
2. Selecione o aluno
3. Clique em "Ficha Completa"

### Backup e Recuperação

**Backup Automático**:
- Execução diária automática
- Sincronização com Google Drive (se configurado)
- Múltiplas versões mantidas
- Notificação de status

**Backup Manual**:
1. Menu → Configurações → Backup
2. Clique em "Backup Agora"
3. Escolha local de destino
4. Aguarde confirmação

**Recuperação**:
- Entre em contato com administrador
- Mantenha backups em local seguro
- Teste periodicamente a recuperação

## ❓ Solução de Problemas

### Problemas de Login

**"Usuário ou senha incorretos"**:
- Verifique se Caps Lock está desabilitado
- Confirme usuário e senha com administrador
- Use "Esqueci a senha" se disponível

**"Licença expirada"**:
- Entre em contato com fornecedor
- Não altere data do sistema
- Aguarde nova chave de ativação

**"Erro de conexão"**:
- Verifique se arquivo de banco existe
- Reinicie a aplicação
- Entre em contato com suporte técnico

### Problemas de Performance

**Sistema lento**:
- Feche outras aplicações desnecessárias
- Verifique espaço em disco disponível
- Reinicie o computador periodicamente

**Relatórios demoram muito**:
- Use filtros para reduzir período
- Evite horários de pico
- Considere exportar em partes menores

### Problemas de Dados

**"Livro não encontrado"**:
- Verifique se foi cadastrado corretamente
- Busque por parte do título
- Verifique se não foi excluído

**"Usuário não pode emprestar"**:
- Verifique se há livros em atraso
- Confirme se usuário está ativo
- Consulte limite de empréstimos

**"Erro ao salvar"**:
- Verifique campos obrigatórios
- Confirme se há espaço em disco
- Tente novamente após alguns segundos

## 💡 Dicas e Truques

### Atalhos de Teclado

- **F1**: Ajuda contextual
- **F2**: Empréstimo rápido
- **F5**: Atualizar tela atual
- **Ctrl + F**: Buscar na tela
- **Ctrl + N**: Novo registro (onde aplicável)
- **Ctrl + S**: Salvar
- **Esc**: Cancelar/Fechar

### Produtividade

**Empréstimos Rápidos**:
- Use códigos de barras sempre que possível
- Configure atalhos para turmas frequentes
- Mantenha lista de usuários frequentes

**Cadastros Eficientes**:
- Use copy/paste para dados repetitivos
- Configure modelos para tipos de usuário
- Aproveite autocomplete e sugestões

**Relatórios Otimizados**:
- Salve filtros mais usados
- Exporte em horários de menor movimento
- Use períodos mensais para análises

### Organização

**Nomenclatura Consistente**:
- Padronize nomes de turmas
- Use formato consistente para livros
- Mantenha gêneros organizados

**Manutenção Regular**:
- Faça backup semanal manual
- Verifique relatórios mensalmente
- Atualize dados de usuários semestralmente

### Segurança

**Boas Práticas**:
- Faça logout ao sair
- Não compartilhe senhas
- Mantenha sistema atualizado
- Reporte problemas imediatamente

**Proteção de Dados**:
- Backup regular
- Senhas seguras
- Acesso restrito ao computador
- Cuidado com dados pessoais

---

## 📞 Suporte

### Contatos

- **Suporte Técnico**: [inserir contato]
- **Manual Online**: [inserir link]
- **Treinamentos**: [inserir informações]

### Recursos Adicionais

- **Vídeos Tutoriais**: Disponíveis no portal
- **FAQ**: Perguntas frequentes
- **Fórum de Usuários**: Troca de experiências
- **Updates**: Notificações de atualizações

---

Este manual é um documento vivo e será atualizado conforme o sistema evolui. Mantenha-o sempre à mão para consultas rápidas!