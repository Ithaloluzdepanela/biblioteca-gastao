using BibliotecaApp.Forms.Utils;
using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Login
{
   
    public partial class LoginForm : Form
    {
        #region Variáveis e Propriedades

        
        // Variável estática para controle externo do processo de login
        // Utilizada para sinalizar se o login foi cancelado ou bem-sucedido
        
        public static bool cancelar = false;

        #endregion

        #region Construtor e Inicialização

        
        // Construtor do formulário de login
        // Inicializa os componentes e configura os eventos de teclado
        
        public LoginForm()
        {
            // Inicializa os componentes visuais do formulário
            InitializeComponent();

            // Configura eventos de teclado para navegação entre campos
            txtEmail.KeyDown += txtEmail_KeyDown;
            txtSenha.KeyDown += txtSenha_KeyDown;
        }

        #endregion

        #region Eventos de Interface - Botão de Saída


        // Evento de clique no botão de saída
        // Encerra completamente a aplicação

        private void picExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        // Evento de mouse sobre o botão de saída
        // Altera a cor de fundo para indicar interação

        private void picExit_MouseEnter(object sender, EventArgs e)
        {
            picExit.BackColor = Color.Gainsboro;
        }

        /// <summary>
        /// Evento de mouse saindo do botão de saída
        /// Restaura a cor de fundo original (transparente)
        /// </summary>
        private void picExit_MouseLeave(object sender, EventArgs e)
        {
            picExit.BackColor = Color.Transparent;
        }

        #endregion

        #region Sistema de Autenticação e Login


        // Evento principal de login - processa as credenciais do usuário
        // Verifica tanto acesso administrativo quanto autenticação por banco de dados

        private async void BtnEntrar_Click(object sender, EventArgs e)
        {
            // Captura e limpa os dados de entrada
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text;

            // Validação de campos obrigatórios
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Campos obrigatórios",
                              MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // Foca no primeiro campo vazio
                if (string.IsNullOrEmpty(email))
                    txtEmail.Focus();
                else
                    txtSenha.Focus();
                return;
            }

            // Verificação de acesso administrativo (credenciais fixas)
            if (email == "admin@admin.com" && senha == "1234")
            {
                await ProcessarLoginSucesso("Administrador", true);
                return;
            }

            // Autenticação via banco de dados
            await AutenticarUsuarioBancoDados(email, senha);
        }


        // Realiza a autenticação do usuário consultando o banco de dados

        // <param name="email">Email do usuário</param>
        // <param name="senha">Senha em texto plano</param>
        private async Task AutenticarUsuarioBancoDados(string email, string senha)
        {
            try
            {
                using (SqlCeConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // Query para buscar dados do bibliotecário
                    string query = @"SELECT Nome, Senha_hash, Senha_salt 
                                   FROM usuarios 
                                   WHERE Email = @email AND TipoUsuario = 'Bibliotecário(a)'";

                    using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@email", email);

                        using (SqlCeDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Recupera dados do usuário do banco
                                string hashSalvo = reader["Senha_hash"].ToString();
                                string saltSalvo = reader["Senha_salt"].ToString();
                                string nomeUsuario = reader["Nome"].ToString();

                                // Verifica se a senha está correta usando criptografia
                                bool senhaCorreta = CriptografiaSenha.VerificarSenha(senha, hashSalvo, saltSalvo);

                                if (senhaCorreta)
                                {
                                    await ProcessarLoginSucesso(nomeUsuario, false);
                                }
                                else
                                {
                                    // Senha incorreta
                                    MessageBox.Show("Senha incorreta.", "Erro de Login",
                                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    txtSenha.Clear();
                                    txtSenha.Focus();
                                }
                            }
                            else
                            {
                                // Usuário não encontrado ou não é bibliotecário
                                MessageBox.Show("E-mail não encontrado ou não é um bibliotecário.", "Erro de Login",
                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                LimparCamposLogin();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na autenticação: " + ex.Message, "Erro",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Processa um login bem-sucedido
        /// </summary>
        /// <param name="nomeUsuario">Nome do usuário logado</param>
        /// <param name="isAdmin">Se é login administrativo</param>
        private async Task ProcessarLoginSucesso(string nomeUsuario, bool isAdmin)
        {
            // Define o usuário na sessão (apenas se não for admin)
            if (!isAdmin)
            {
                Sessao.NomeBibliotecariaLogada = nomeUsuario;
            }

            // Atualiza status dos empréstimos e reservas
            await AtualizarStatusEmprestimosAsync();

            // Sinaliza sucesso e fecha o formulário
            cancelar = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Limpa todos os campos de login
        /// </summary>
        private void LimparCamposLogin()
        {
            txtEmail.Clear();
            txtSenha.Clear();
            txtEmail.Focus();
        }

        #endregion

        #region Eventos Visuais do Botão Entrar


        //Evento de mouse saindo do botão Entrar
        // Restaura a cor original do botão

        private void BtnEntrar_MouseLeave(object sender, EventArgs e)
        {
            BtnEntrar.BackColor = Color.FromArgb(9, 74, 158);
            BtnEntrar.Refresh();
        }

        // Evento de mouse sobre o botão Entrar
        // Altera a cor para indicar hover

        private void BtnEntrar_MouseEnter(object sender, EventArgs e)
        {
            BtnEntrar.BackColor = Color.FromArgb(33, 145, 245);
            BtnEntrar.Refresh();
        }

        #endregion

        #region Sistema de Atualização de Empréstimos e Reservas


        // Método principal para atualização assíncrona de empréstimos e reservas
        // Executa em thread separada para não bloquear a interface

        private async Task AtualizarStatusEmprestimosAsync()
        {
            using (var progressForm = new frmProgresso())
            {
                progressForm.Show();

                // Executa as atualizações em thread separada
                await Task.Run(() =>
                {
                    AtualizarEmprestimos(progressForm);
                    AtualizarReservas(progressForm);
                });
            }
        }

        // Atualiza o status de todos os empréstimos ativos no sistema
        // Verifica datas de devolução e prorrogação para determinar status atual

        // <param name="progressForm">Formulário de progresso para feedback visual</param>
        private void AtualizarEmprestimos(frmProgresso progressForm)
        {
            try
            {
                using (var connection = Conexao.ObterConexao())
                {
                    connection.Open();

                    // Conta quantos empréstimos precisam ser atualizados
                    string countQuery = "SELECT COUNT(*) FROM Emprestimo WHERE Status <> 'Devolvido'";
                    var countCommand = new SqlCeCommand(countQuery, connection);
                    int totalEmprestimos = (int)countCommand.ExecuteScalar();

                    // Se não há empréstimos para atualizar, sai do método
                    if (totalEmprestimos == 0)
                    {
                        progressForm.AtualizarProgresso(100, "Nenhum empréstimo para atualizar");
                        return;
                    }

                    // Busca todos os empréstimos ativos
                    string selectQuery = @"SELECT Id, DataDevolucao, DataProrrogacao, DataRealDevolucao 
                                         FROM Emprestimo 
                                         WHERE Status <> 'Devolvido'";

                    var selectCommand = new SqlCeCommand(selectQuery, connection);
                    var reader = selectCommand.ExecuteReader();

                    int processados = 0;

                    // Processa cada empréstimo individualmente
                    while (reader.Read())
                    {
                        // Extrai dados do empréstimo
                        int id = reader.GetInt32(0);
                        DateTime dataDevolucao = reader.GetDateTime(1);
                        DateTime? dataProrrogacao = reader.IsDBNull(2) ? null : (DateTime?)reader.GetDateTime(2);
                        DateTime? dataRealDevolucao = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3);

                        // Calcula o novo status baseado nas datas
                        string novoStatus = CalcularStatus(dataDevolucao, dataProrrogacao, dataRealDevolucao);

                        // Atualiza o status no banco de dados
                        string updateQuery = "UPDATE Emprestimo SET Status = @Status WHERE Id = @Id";
                        var updateCommand = new SqlCeCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@Status", novoStatus);
                        updateCommand.Parameters.AddWithValue("@Id", id);
                        updateCommand.ExecuteNonQuery();

                        // Atualiza o progresso na interface
                        processados++;
                        int progresso = (int)((double)processados / totalEmprestimos * 100);
                        progressForm.AtualizarProgresso(progresso,
                            $"Atualizando empréstimo {processados} de {totalEmprestimos}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar empréstimos: {ex.Message}", "Erro",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Atualiza o status das reservas, expirando aquelas que passaram do prazo
        /// </summary>
        /// <param name="progressForm">Formulário de progresso para feedback visual</param>
        private void AtualizarReservas(frmProgresso progressForm)
        {
            try
            {
                using (var connection = Conexao.ObterConexao())
                {
                    connection.Open();

                    // Busca reservas com status 'Disponível' que podem ter expirado
                    string selectQuery = @"
                        SELECT Id, DataLimiteRetirada
                        FROM Reservas
                        WHERE Status = 'Disponível'";

                    using (var selectCommand = new SqlCeCommand(selectQuery, connection))
                    using (var reader = selectCommand.ExecuteReader())
                    {
                        // Coleta todas as reservas que precisam ser verificadas
                        var reservasParaExpirar = new List<(int Id, DateTime? DataLimiteRetirada)>();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            DateTime? dataLimite = reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1);
                            reservasParaExpirar.Add((id, dataLimite));
                        }

                        int total = reservasParaExpirar.Count;
                        int processadas = 0;

                        // Verifica cada reserva individualmente
                        foreach (var reserva in reservasParaExpirar)
                        {
                            // Se passou da data limite, expira a reserva
                            if (reserva.DataLimiteRetirada.HasValue &&
                                DateTime.Now > reserva.DataLimiteRetirada.Value)
                            {
                                using (var updateCmd = new SqlCeCommand(
                                    "UPDATE Reservas SET Status = 'Expirada' WHERE Id = @Id", connection))
                                {
                                    updateCmd.Parameters.AddWithValue("@Id", reserva.Id);
                                    updateCmd.ExecuteNonQuery();
                                }
                            }

                            // Atualiza o progresso
                            processadas++;
                            int progresso = (int)((double)processadas / total * 100);
                            progressForm.AtualizarProgresso(progresso,
                                $"Verificando reservas ({processadas}/{total})");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar reservas: {ex.Message}", "Erro",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Calcula o status atual de um empréstimo baseado nas datas
        /// </summary>
        /// <param name="dataDevolucao">Data original de devolução</param>
        /// <param name="dataProrrogacao">Data de prorrogação (se houver)</param>
        /// <param name="dataRealDevolucao">Data real de devolução (se já devolvido)</param>
        /// <returns>Status calculado: "Devolvido", "Atrasado" ou "Ativo"</returns>
        private string CalcularStatus(DateTime dataDevolucao, DateTime? dataProrrogacao, DateTime? dataRealDevolucao)
        {
            // Usa a data de prorrogação se existir, senão usa a data original
            DateTime dataReferencia = dataProrrogacao ?? dataDevolucao;

            // Se já foi devolvido
            if (dataRealDevolucao.HasValue)
            {
                return "Devolvido";
            }
            // Se passou da data de devolução
            else if (DateTime.Now > dataReferencia)
            {
                return "Atrasado";
            }
            // Se ainda está dentro do prazo
            else
            {
                return "Ativo";
            }
        }

        #endregion

        #region Eventos de Navegação por Teclado


        // Evento de tecla pressionada no campo de email
        // Permite navegação com Enter para o próximo campo

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Impede o som de erro
                txtSenha.Focus(); // Move o foco para o campo de senha
            }
        }


        // Evento de tecla pressionada no campo de senha
        // Permite executar o login com Enter

        private void txtSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Impede o som de erro
                BtnEntrar.PerformClick(); // Simula clique no botão Entrar
            }
        }

        #endregion
    }
}