using BibliotecaApp.Forms.Utils;
using BibliotecaApp.Models;
using BibliotecaApp.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Livros
{
    public partial class ReservaForm : Form
    {
        // caches e listas
        private List<Usuarios> Usuarios = new List<Usuarios>();
        private List<Livro> Livros = new List<Livro>();
        private List<Usuarios> _cacheUsuarios = new List<Usuarios>();
        private List<Livro> _cacheLivros = new List<Livro>();

        public ReservaForm()
        {
            InitializeComponent();

            // eventos principais
            this.Load += ReservaForm_Load;
            txtBarcode.Leave += txtBarcode_Leave;
            btnReservar.Click += btnReservar_Click;
            btnCancelar.Click += btnCancelar_Click;

            
           
        }

        private void ReservaForm_Load(object sender, EventArgs e)
        {
            CarregarDadosIniciais();
        }

        /// <summary>
        /// Preenche a reserva com informações vindas do formulário de empréstimo.
        /// </summary>
        public void PreFillFromEmprestimo(Usuarios usuario, Livro livro, Usuarios bibliotecaria, string codigoBarras, DateTime sugestaoDataDevolucao)
        {
            if (Usuarios.Count == 0 || Livros.Count == 0)
                CarregarDadosIniciais();

            if (usuario != null)
            {
                txtNomeUsuario.Text = usuario.Nome;
            }

            if (livro != null)
            {
                txtLivro.Text = livro.Nome;
                txtBarcode.Text = !string.IsNullOrWhiteSpace(codigoBarras) ? codigoBarras : (livro.CodigoDeBarras ?? "");
            }

            if (bibliotecaria != null && !string.IsNullOrWhiteSpace(bibliotecaria.Nome))
            {
                var idx = cbBibliotecaria.Items.IndexOf(bibliotecaria.Nome);
                if (idx >= 0) cbBibliotecaria.SelectedIndex = idx;
            }

            dtpDataReserva.Value = DateTime.Today;
        }

        #region Carregamento Inicial

        private void CarregarDadosIniciais()
        {
            CarregarUsuariosDoBanco();
            CarregarLivrosIndisponiveisDoBanco();
            CarregarBibliotecarias();

            // caches para buscas rápidas
            _cacheUsuarios = new List<Usuarios>(Usuarios);
            _cacheLivros = new List<Livro>(Livros);
        }

        private void CarregarUsuariosDoBanco()
        {
            Usuarios.Clear();
            _cacheUsuarios.Clear();
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT Id, Nome, TipoUsuario, Email, Turma FROM usuarios ORDER BY Nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var usuario = new Usuarios
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                TipoUsuario = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Turma = reader.IsDBNull(4) ? "" : reader.GetString(4)
                            };
                            Usuarios.Add(usuario);
                            _cacheUsuarios.Add(usuario);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar usuários: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarLivrosIndisponiveisDoBanco()
        {
            Livros.Clear();
            _cacheLivros.Clear();
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT Id, Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade
                                   FROM Livros
                                   WHERE Disponibilidade = 0 OR Quantidade = 0
                                   ORDER BY Nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var livro = new Livro(
                                reader.IsDBNull(1) ? "" : reader.GetString(1),
                                reader.IsDBNull(2) ? "" : reader.GetString(2),
                                reader.IsDBNull(3) ? "" : reader.GetString(3),
                                reader.IsDBNull(6) ? false : reader.GetBoolean(6),
                                reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                reader.IsDBNull(5) ? "" : reader.GetString(5)
                            );
                            if (!reader.IsDBNull(0))
                                typeof(Livro).GetProperty("Id").SetValue(livro, reader.GetInt32(0));
                            Livros.Add(livro);
                            _cacheLivros.Add(livro);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar livros: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarBibliotecarias()
        {
            cbBibliotecaria.Items.Clear();
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT Nome FROM usuarios WHERE TipoUsuario LIKE @tipo ORDER BY Nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@tipo", "%Bibliotec%");
                        using (var r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                if (!r.IsDBNull(0))
                                {
                                    var nome = r.GetString(0);
                                    if (!cbBibliotecaria.Items.Contains(nome))
                                        cbBibliotecaria.Items.Add(nome);
                                }
                            }
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(Sessao.NomeBibliotecariaLogada))
                {
                    var idx = cbBibliotecaria.Items.IndexOf(Sessao.NomeBibliotecariaLogada);
                    if (idx >= 0) cbBibliotecaria.SelectedIndex = idx;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar bibliotecárias: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Reserva / Cancelar

        private void btnReservar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var usuario = ObterUsuarioSelecionado();
            var livro = ObterLivroSelecionado();
            var bibliotecaria = ObterBibliotecariaSelecionada();

            if (!ValidarSelecoes(usuario, livro, bibliotecaria)) return;

            if (UsuarioTemQualquerReservaAtiva(usuario.Id)) return;

            if (!ValidarDisponibilidade(livro)) return;

            if (VerificarReservaAtiva(usuario.Id, livro.Id)) return;

            DateTime dataReserva = dtpDataReserva.Value;

            RegistrarReserva(usuario, livro, bibliotecaria, dataReserva);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNomeUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtLivro.Text) ||
                cbBibliotecaria.SelectedIndex == -1)
            {
                MessageBox.Show("Preencha todos os campos antes de reservar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private Usuarios ObterUsuarioSelecionado()
        {
            return _cacheUsuarios.FirstOrDefault(u => u.Nome.Equals(txtNomeUsuario.Text, StringComparison.OrdinalIgnoreCase));
        }

        private Livro ObterLivroSelecionado()
        {
            return _cacheLivros.FirstOrDefault(l => l.Nome.Equals(txtLivro.Text, StringComparison.OrdinalIgnoreCase));
        }

        private Usuarios ObterBibliotecariaSelecionada()
        {
            if (cbBibliotecaria.SelectedItem == null) return null;
            return Usuarios.FirstOrDefault(u => u.Nome.Equals(cbBibliotecaria.SelectedItem.ToString(), StringComparison.OrdinalIgnoreCase));
        }

        private bool ValidarSelecoes(Usuarios usuario, Livro livro, Usuarios bibliotecaria)
        {
            if (usuario == null || livro == null || bibliotecaria == null)
            {
                MessageBox.Show("Usuário, livro ou bibliotecária não encontrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void RegistrarReserva(Usuarios usuario, Livro livro, Usuarios bibliotecaria, DateTime dataReserva)
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    var dataDisponibilidade = ObterDataProximaDisponibilidade(livro.Id);
                    var dataLimiteRetirada = dataDisponibilidade.AddDays(7);

                    string sql = @"
                INSERT INTO Reservas 
                    (UsuarioId, LivroId, BibliotecariaId, DataReserva, DataDisponibilidade, DataLimiteRetirada, Status)
                VALUES
                    (@usuarioId, @livroId, @bibliotecariaId, @DataReserva, @DataDisponibilidade, @DataLimiteRetirada, 'Pendente')";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@usuarioId", usuario.Id);
                        cmd.Parameters.AddWithValue("@livroId", livro.Id);
                        cmd.Parameters.AddWithValue("@bibliotecariaId", bibliotecaria.Id);
                        cmd.Parameters.AddWithValue("@DataReserva", dataReserva);
                        cmd.Parameters.AddWithValue("@DataDisponibilidade", dataDisponibilidade);
                        cmd.Parameters.AddWithValue("@DataLimiteRetirada", dataLimiteRetirada);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Reserva registrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();

                    if (!string.IsNullOrWhiteSpace(usuario.Email) && ValidarEmail(usuario.Email))
                    {
                        EnviarEmailConfirmacao(usuario, livro, dataReserva, dataDisponibilidade);
                    }
                    else
                    {
                        MessageBox.Show("Reserva registrada, mas não foi possível enviar email de confirmação.\n" +
                                      "Email do usuário inválido ou não cadastrado.",
                                      "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao registrar reserva:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnviarEmailConfirmacao(Usuarios usuario, Livro livro, DateTime dataReserva, DateTime dataDisponibilidade)
        {
            try
            {
                string assunto = "📚 Reserva Confirmada - Biblioteca Monteiro Lobato";

                string corpo = $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;'>
                    <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
                        <h2 style='color: #2c3e50;'>Olá, {usuario.Nome} 👋</h2>
                        
                        <p>Sua reserva foi registrada com sucesso! Aqui estão os detalhes:</p>
                        
                        <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                            <p><strong>📖 Livro:</strong> {livro.Nome}</p>
                            <p><strong>✍️ Autor:</strong> {livro.Autor}</p>
                            <p><strong>📅 Data da Reserva:</strong> {dataReserva:dd/MM/yyyy}</p>
                            <p><strong>📅 Previsão de Disponibilidade:</strong> {dataDisponibilidade:dd/MM/yyyy}</p>
                        </div>
                        
                        <p><strong>⏳ Aguarde:</strong> Assim que o livro estiver disponível, você será avisado por aqui.</p>
                        
                        <p style='margin-top: 20px;'>Você terá um prazo de 7 dias para retirar o livro após ele ficar disponível. Fique atento aos e-mails da biblioteca!</p>
                        
                        <hr style='margin: 30px 0; border: none; border-top: 1px solid #eee;' />
                        
                        <p style='font-size: 14px; color: #888;'>
                            Este é um e-mail automático enviado pela Biblioteca Monteiro Lobato.<br>
                            Por favor, não responda este e-mail.
                        </p>
                    </div>
                </body>
                </html>";

                string email = usuario.Email?.Trim();
                if (ValidarEmail(email))
                {
                    EmailService.Enviar(email, assunto, corpo);
                }
                else
                {
                    throw new Exception("Email inválido: " + email);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao enviar email de confirmação: {ex.Message}",
                              "Erro no Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        private DateTime ObterDataProximaDisponibilidade(int livroId)
        {
            using (var conexao = EmprestimoForm.Conexao.ObterConexao())
            {
                conexao.Open();
                string sql = @"
                    SELECT MIN(
                        CASE 
                            WHEN DataProrrogacao IS NOT NULL THEN DataProrrogacao
                            ELSE DataDevolucao
                        END
                    )
                    FROM Emprestimo
                    WHERE Livro = @livroId 
                      AND Status IN ('Ativo', 'Atrasado')";

                using (var cmd = new SqlCeCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@livroId", livroId);
                    var resultado = cmd.ExecuteScalar();
                    if (resultado != null && resultado != DBNull.Value)
                        return (DateTime)resultado;
                }
            }
            return DateTime.Today;
        }

        private bool VerificarReservaAtiva(int usuarioId, int livroId)
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT COUNT(*) FROM Reservas
                                   WHERE UsuarioId = @usuarioId AND LivroId = @livroId
                                   AND Status IN ('Pendente','Disponível')";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                        cmd.Parameters.AddWithValue("@livroId", livroId);
                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("O usuário já possui reserva ativa para este livro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar reservas: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private bool ValidarDisponibilidade(Livro livro)
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();

                    // pega quantidade total
                    string sqlTotal = @"SELECT Quantidade FROM Livros WHERE Id = @id";
                    int quantidadeTotal = 0;
                    using (var cmd = new SqlCeCommand(sqlTotal, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", livro.Id);
                        var res = cmd.ExecuteScalar();
                        quantidadeTotal = (res != null && res != DBNull.Value) ? Convert.ToInt32(res) : 0;
                    }

                    // emprestimos ativos
                    sqlTotal = @"SELECT COUNT(*) FROM Emprestimo WHERE Livro = @livroId AND Status IN ('Ativo','Atrasado')";
                    int emprestimosAtivos = 0;
                    using (var cmd = new SqlCeCommand(sqlTotal, conexao))
                    {
                        cmd.Parameters.AddWithValue("@livroId", livro.Id);
                        emprestimosAtivos = (int)cmd.ExecuteScalar();
                    }

                    // reservas ativas
                    sqlTotal = @"SELECT COUNT(*) FROM Reservas WHERE LivroId = @livroId AND Status IN ('Pendente','Disponível')";
                    int reservasAtivas = 0;
                    using (var cmd = new SqlCeCommand(sqlTotal, conexao))
                    {
                        cmd.Parameters.AddWithValue("@livroId", livro.Id);
                        reservasAtivas = (int)cmd.ExecuteScalar();
                    }

                    if (emprestimosAtivos == 0)
                    {
                        MessageBox.Show("Não há exemplares emprestados no momento.\nUse o formulário de empréstimo para retirar o livro agora.", "Sem Empréstimos Ativos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }

                    if (reservasAtivas >= emprestimosAtivos)
                    {
                        MessageBox.Show("Todas as cópias emprestadas já têm reserva ativa — aguarde liberação.", "Reservas Esgotadas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar disponibilidade: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void LimparCampos()
        {
            txtNomeUsuario.Text = "";
            txtLivro.Text = "";
            txtBarcode.Text = "";
            dtpDataReserva.Value = DateTime.Today;
        }

        #endregion

        private void txtBarcode_Leave(object sender, EventArgs e)
        {
            var codigo = txtBarcode.Text.Trim();
            if (string.IsNullOrEmpty(codigo)) return;

            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT Nome FROM Livros WHERE CodigoBarras = @codigo";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@codigo", codigo);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtLivro.Text = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            }
                            else
                            {
                                MessageBox.Show("Livro não encontrado. Escaneie novamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtBarcode.Focus();
                                txtBarcode.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar o livro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Métodos estáticos auxiliares

        public static bool ValidarEmailStatic(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        public static void EnviarEmailDisponibilidadeStatic(Usuarios usuario, Livro livro, DateTime dataDisponibilidade, DateTime dataLimiteRetirada)
        {
            try
            {
                string assunto = "📚 Seu livro está disponível para retirada - Biblioteca Monteiro Lobato";
                string corpo = $@"
        <html>
        <body style='font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;'>
            <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
                <h2 style='color: #2c3e50;'>Olá, {usuario.Nome} 👋</h2>
                <p>O livro que você reservou está disponível para retirada!</p>
                <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                    <p><strong>📖 Livro:</strong> {livro.Nome}</p>
                    <p><strong>✍️ Autor:</strong> {livro.Autor}</p>
                    <p><strong>📅 Disponível a partir de:</strong> {dataDisponibilidade:dd/MM/yyyy}</p>
                    <p><strong>⏰ Prazo para retirada:</strong> até {dataLimiteRetirada:dd/MM/yyyy}</p>
                </div>
                <p style='color: #d35400; font-weight: bold;'>
                    Atenção: Você tem <u>7 dias</u> para retirar o livro a partir da data de disponibilidade.<br>
                    Após esse prazo, a reserva será expirada автоматически.
                </p>
                <p>Compareça à biblioteca até a data limite para garantir seu exemplar.</p>
                <hr style='margin: 30px 0; border: none; border-top: 1px solid #eee;' />
                <p style='font-size: 14px; color: #888;'>
                    Este é um e-mail automático enviado pela Biblioteca Monteiro Lobato.<br>
                    Por favor, não responda este e-mail.
                </p>
            </div>
        </body>
        </html>";

                if (!string.IsNullOrWhiteSpace(usuario.Email) && ValidarEmailStatic(usuario.Email))
                {
                    BibliotecaApp.Services.EmailService.Enviar(usuario.Email, assunto, corpo);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao enviar email de disponibilidade: {ex.Message}");
            }
        }

        public static void EnviarEmailDisponibilidadeStatic(Usuarios usuario, Livro livro, DateTime dataDisponibilidade, DateTime? dataLimiteRetirada = null)
        {
            DateTime dataLimite = dataLimiteRetirada ?? dataDisponibilidade.AddDays(7);
            EnviarEmailDisponibilidadeStatic(usuario, livro, dataDisponibilidade, dataLimite);
        }

        public static void EnviarEmailDisponibilidadeStatic(string email, string nomeUsuario, string nomeLivro, DateTime dataDisponibilidade, DateTime? dataLimiteRetirada = null)
        {
            var usuario = new Usuarios { Nome = nomeUsuario ?? "", Email = email ?? "" };
            var livro = new Livro { Nome = nomeLivro ?? "" };
            EnviarEmailDisponibilidadeStatic(usuario, livro, dataDisponibilidade, dataLimiteRetirada);
        }

        #endregion

        #region Controle de reservas ativas

        private bool UsuarioTemQualquerReservaAtiva(int usuarioId)
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT COUNT(*) 
                           FROM Reservas 
                           WHERE UsuarioId = @usuarioId 
                             AND Status IN ('Pendente','Disponível')";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show(
                                "Este usuário já possui uma reserva ativa no sistema. " +
                                "Conclua ou espere a expiração antes de criar outra.",
                                "Reserva já existente",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao checar reservas do usuário: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            return false;
        }

        public static bool UsuarioPossuiReservaAtiva(int usuarioId, out string tituloLivro)
        {
            tituloLivro = null;
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"
                SELECT TOP 1 l.Nome
                FROM Reservas r
                INNER JOIN Livros l ON l.Id = r.LivroId
                WHERE r.UsuarioId = @usuarioId
                  AND r.Status IN ('Pendente','Disponível')";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                        var r = cmd.ExecuteScalar();
                        if (r != null && r != DBNull.Value)
                        {
                            tituloLivro = r.ToString();
                            return true;
                        }
                    }
                }
            }
            catch { }

            return false;
        }

        #endregion

        #region Atualização de Reservas (para uso no login)

        public static void AtualizarReservas(frmProgresso progressForm, int totalOperacoes, ref int progressoAtual)
        {
            try
            {
                using (var conexao = EmprestimoForm.Conexao.ObterConexao())
                {
                    conexao.Open();
                    // 1. Atualizar reservas quando o livro ficar disponível
                    progressForm.AtualizarProgresso(progressoAtual++, "Verificando reservas...");
                    string updateDisponivel = @"UPDATE Reservas
                                              SET Status = 'Disponível',
                                                  DataDisponibilidade = GETDATE(),
                                                 DataLimiteRetirada = DATEADD(day, 3, GETDATE())
                                             WHERE Status = 'Pendente'
                                              AND LivroId IN (SELECT Id FROM Livros WHERE Disponibilidade = 1)";
                    new SqlCeCommand(updateDisponivel, conexao).ExecuteNonQuery();

                    // 2. Expirar reservas não retiradas em 3 dias
                    progressForm.AtualizarProgresso(progressoAtual++, "Verificando prazos...");
                    string updateExpiradas = @"UPDATE Reservas
                                              SET Status = 'Expirada'
                                             WHERE Status = 'Disponível'
                                              AND DataLimiteRetirada < GETDATE()";
                    new SqlCeCommand(updateExpiradas, conexao).ExecuteNonQuery();

                    // 3. Liberar livros de reservas expiradas
                    progressForm.AtualizarProgresso(progressoAtual++, "Liberando livros...");
                    string liberarLivros = @"UPDATE Livros
                                            SET Disponibilidade = 1
                                           WHERE Id IN (
                                               SELECT LivroId FROM Reservas
                                                WHERE Status = 'Expirada'
                                           )";
                    new SqlCeCommand(liberarLivros, conexao).ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                progressForm.Invoke((MethodInvoker)(() =>
                {
                    MessageBox.Show($"Erro ao atualizar reservas: {ex.Message}",
                                  "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
        }

        #endregion
    }
}