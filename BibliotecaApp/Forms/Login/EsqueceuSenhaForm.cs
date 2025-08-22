using BibliotecaApp.Services;
using System;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static BibliotecaApp.Forms.Livros.CadastroLivroForm;

namespace BibliotecaApp.Forms.Login
{
    public partial class EsqueceuSenhaForm : Form
    {
        // Ajuste estes nomes se necessário
        private const string TabelaUsuarios = "usuarios";
        private const string ColunaNome = "nome";
        private const string ColunaEmail = "email";
        // Estado do código gerado
        private string _codigoAtual;
        private DateTime? _expiraEm;
        private string _emailDestino;
        // Cooldown "Reenviar Código"
        private readonly Timer _cooldownTimer = new Timer { Interval = 1000 };
        private int _secondsRemaining = 0;
        // Exibição temporária do código
        private readonly Timer _revealTimer = new Timer { Interval = 1000 };
        private int _revealSecondsLeft = 0;
        private bool _showingCode = false;
        // Guardas de reentrância (impede cliques duplos gerarem ações duplicadas)
        private bool _isSending = false;
        private bool _isVerifying = false;
        public EsqueceuSenhaForm()
        {
            InitializeComponent();
            ConfigurarTimers();
            AtualizarLblReenviar();
        }

        private void ConfigurarTimers()
        {
            // Timer do cooldown
            _cooldownTimer.Tick += (s, e) =>
            {
                if (_secondsRemaining > 0)
                {
                    _secondsRemaining--;
                    AtualizarLblReenviar();
                }

                if (_secondsRemaining <= 0)
                {
                    _cooldownTimer.Stop();
                    AtualizarLblReenviar();
                }
            };

            // Timer da exibição temporária do código
            _revealTimer.Tick += (s, e) =>
            {
                if (_revealSecondsLeft > 0)
                {
                    _revealSecondsLeft--;
                    if (_showingCode)
                    {
                        lblReenviar.Text = $"Código: {_codigoAtual} (some em {_revealSecondsLeft}s)";
                    }
                }

                if (_revealSecondsLeft <= 0)
                {
                    _revealTimer.Stop();
                    _showingCode = false;
                    AtualizarLblReenviar();
                }
            };
        }

        private void AtualizarLblReenviar()
        {
            if (_showingCode)
            {
                // Enquanto exibindo o código, não alteramos o texto aqui.
                return;
            }

            if (_secondsRemaining > 0)
            {
                lblReenviar.Text = $"Reenviar em {_secondsRemaining}s";
                lblReenviar.ForeColor = SystemColors.GrayText;
                lblReenviar.Cursor = Cursors.No;
            }
            else
            {
                lblReenviar.Text = "Reenviar código";
                lblReenviar.ForeColor = Color.RoyalBlue;
                lblReenviar.Cursor = Cursors.Hand;
            }
        }

        private void IniciarCooldown(int seconds)
        {
            _secondsRemaining = seconds;
            _cooldownTimer.Stop();
            _cooldownTimer.Start();
            AtualizarLblReenviar();
        }

        // ========== Eventos (ligue-os no Designer apenas UMA vez) ==========

        // Botão Enviar (Click -> btnEnviar_Click)
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            // Captura o e-mail antes de ocultar o campo
            string email = txtEmail.Text?.Trim();

            // Verifica se o e-mail é válido antes de fazer qualquer mudança na UI
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show(
                    "O campo de e-mail está vazio. Por favor, preencha o e-mail.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Tenta enviar o código - só muda a UI se for bem-sucedido
            if (EnviarCodigoComValidacao(email))
            {
                // Só atualiza a UI se o email foi validado e o código enviado com sucesso
                lblTop.Text = "Digite o Código de Verificação";


                // Oculta e desativa o campo de e-mail
                txtEmail.Visible = false;
                txtEmail.Enabled = false;
                pnBarra.Visible = false;
                lblDigite.Visible = false; // Oculta a barra de progresso (ou outro painel)

                txtTeste.Focus();


                lblReenviar.Visible = true; // Certifique-se de que o label de reenviar código esteja visível

                txtTeste.Location = txtEmail.Location;
                pnBarra2.Location = pnBarra.Location;
                lblCodigo.Location = lblDigite.Location;
            }
            // Se EnviarCodigoComValidacao retornar false, a UI permanece inalterada
        }

        // Botão Verificar (Click -> btnTestar_Click)
        private void btnTestar_Click(object sender, EventArgs e)
        {
            if (_isVerifying) return; // guarda anti-duplo clique
            _isVerifying = true;
            try
            {
                var codigoDigitado = txtTeste.Text?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(_emailDestino) || string.IsNullOrWhiteSpace(_codigoAtual) || _expiraEm == null)
                {
                    MessageBox.Show("Nenhum código válido foi gerado. Envie um novo código.");
                    return;
                }

                if (DateTime.UtcNow > _expiraEm)
                {
                    MessageBox.Show("O código expirou. Solicite um novo código.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(codigoDigitado))
                {
                    MessageBox.Show("Digite o código recebido por e-mail.");
                    return;
                }

                if (codigoDigitado == _codigoAtual)
                {
                    MessageBox.Show("Código correto! Você pode prosseguir com a redefinição de senha.");
                    // Ex.: new RedefinirSenhaForm(_emailDestino).Show();
                    InutilizarCodigo(); // invalidar após sucesso
                    AtualizarLblReenviar();
                }
                else
                {
                    MessageBox.Show("Código incorreto. Tente novamente.");
                }
            }
            finally
            {
                _isVerifying = false;
            }
        }

        // Label Reenviar (Click -> lblReenviar_Click)
        private void lblReenviar_Click(object sender, EventArgs e)
        {
            // Clique normal: reenvia se cooldown terminou
            if (_secondsRemaining > 0)
            {
                MessageBox.Show("Aguarde o término da contagem para reenviar o código.");
                return;
            }

            var email = txtEmail.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Informe o e-mail antes de reenviar o código.");
                return;
            }

            EnviarCodigo(email, reiniciarCooldown: true);
        }

        // Label Reenviar (DoubleClick -> lblReenviar_DoubleClick)
        private void lblReenviar_DoubleClick(object sender, EventArgs e)
        {
            // Duplo clique: mostrar o código por 8s (sem clique direito)
            MostrarCodigoTemporariamente();
        }

        // ========== Núcleo da lógica ==========

        private bool EnviarCodigoComValidacao(string email)
        {
            if (_isSending) return false; // guarda anti-duplo clique
            _isSending = true;

            // Desabilita UI durante envio para evitar cliques duplos
            var prevBtnEnviar = btnEnviar.Enabled;
            var prevLblReenviar = lblReenviar.Enabled;
            btnEnviar.Enabled = false;
            lblReenviar.Enabled = false;

            try
            {
                // 1) Validar preenchimento e formato
                if (string.IsNullOrWhiteSpace(email))
                {
                    MessageBox.Show("Por favor, preencha o campo de e-mail.");
                    return false;
                }
                if (!EmailValido(email))
                {
                    MessageBox.Show("E-mail inválido.");
                    return false;
                }

                // 2) Buscar nome no banco
                string nome;
                try
                {
                    nome = ObterNomePorEmail(email);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao acessar o banco de dados: " + ex.Message);
                    return false;
                }

                if (string.IsNullOrEmpty(nome))
                {
                    MessageBox.Show("E-mail não encontrado no sistema.");
                    return false;
                }

                // 3) Gerar código (6 dígitos) e expiração
                _codigoAtual = GerarCodigoSeisDigitos();
                _expiraEm = DateTime.UtcNow.AddMinutes(10);
                _emailDestino = email;

                // 4) Montar e enviar e-mail
                var assunto = "🔐 Código de Verificação - Biblioteca Monteiro Lobato";
                var corpoHtml = $@"
<html>
  <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #fff; border-radius: 8px; padding: 20px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>
      <h2 style='color: #2c3e50;'>Olá, {Html(nome)} 👋</h2>
      <p>Recebemos uma solicitação para verificação de e-mail no sistema da biblioteca.</p>
      <p style='font-size: 18px;'><strong>🔐 Seu código de verificação:</strong></p>
      <div style='font-size: 32px; font-weight: bold; color: #27ae60; margin: 20px 0;'>{_codigoAtual}</div>
      <p>Use este código para concluir sua verificação. Ele expira em 10 minutos.</p>
      <hr />
      <p style='font-size: 14px; color: #888;'>Este é um e-mail automático enviado pela Biblioteca Monteiro Lobato.</p>
    </div>
  </body>
</html>";
                try
                {
                    EmailService.Enviar(email, assunto, corpoHtml);
                    MessageBox.Show("Código enviado com sucesso para " + email);

                    _showingCode = false; // se estiver mostrando, interrompe
                    _revealTimer.Stop();
                    IniciarCooldown(60);

                    return true; // Sucesso
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao enviar e-mail: " + ex.Message);
                    return false;
                }
            }
            finally
            {
                _isSending = false;
                btnEnviar.Enabled = prevBtnEnviar;
                lblReenviar.Enabled = prevLblReenviar;
            }
        }

        private void EnviarCodigo(string email, bool reiniciarCooldown)
        {
            EnviarCodigoComValidacao(email);
        }

        private void MostrarCodigoTemporariamente()
        {
            if (string.IsNullOrEmpty(_codigoAtual) || _expiraEm == null || DateTime.UtcNow > _expiraEm)
            {
                MessageBox.Show("Nenhum código ativo para exibir. Gere um novo código.");
                return;
            }

            _showingCode = true;
            _revealSecondsLeft = 8; // tempo de exibição do código
            lblReenviar.Text = $"Código: {_codigoAtual} (some em {_revealSecondsLeft}s)";
            lblReenviar.ForeColor = Color.DarkGreen;

            _revealTimer.Stop();
            _revealTimer.Start();
        }

        // ========== Utilitários ==========

        private static bool EmailValido(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return string.Equals(addr.Address, email, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static string GerarCodigoSeisDigitos()
        {
            // Gera um número entre 100000 e 999999 de forma criptograficamente segura (sem GetInt32)
            int numero = GetSecureInt32(100000, 1_000_000); // [100000, 1000000)
            return numero.ToString("D6");
        }

        // Gera um inteiro seguro e uniforme no intervalo [minValue, maxValue)
        private static int GetSecureInt32(int minValue, int maxValue)
        {
            if (minValue >= maxValue)
                throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue deve ser maior que minValue.");

            long range = (long)maxValue - minValue;
            if (range > uint.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(maxValue), "Intervalo muito grande.");

            uint rangeU = (uint)range;
            ulong twoPow32 = 1UL << 32;
            ulong limit = twoPow32 - (twoPow32 % rangeU);

            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[4];
                while (true)
                {
                    rng.GetBytes(bytes);
                    uint value = BitConverter.ToUInt32(bytes, 0);

                    if ((ulong)value >= limit)
                        continue;

                    uint withinRange = value % rangeU;
                    return (int)(minValue + withinRange);
                }
            }
        }

        private static string Html(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#39;");
        }

        private void InutilizarCodigo()
        {
            _codigoAtual = null;
            _expiraEm = null;
            // Mantemos _emailDestino se for útil para o próximo passo (ex.: redefinir senha)
        }

        private string ObterNomePorEmail(string email)
        {
            var sql = $"SELECT {ColunaNome} FROM {TabelaUsuarios} WHERE {ColunaEmail} = @Email";

            using (SqlCeConnection conn = Conexao.ObterConexao())
            {
                conn.Open();
                using (SqlCeCommand cmd = new SqlCeCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                        return null;

                    return Convert.ToString(result);
                }
            }
        }

        private void picExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Só atualiza a UI se o email foi validado e o código enviado com sucesso
            lblTop.Text = "Digite o Código de Verificação";


            // Oculta e desativa o campo de e-mail
            txtEmail.Visible = false;
            txtEmail.Enabled = false;
            pnBarra.Visible = false;
            lblDigite.Visible = false; // Oculta a barra de progresso (ou outro painel)

            txtTeste.Focus();


            lblReenviar.Visible= true; // Certifique-se de que o label de reenviar código esteja visível

            txtTeste.Location = txtEmail.Location;
            pnBarra2.Location = pnBarra.Location;
            lblCodigo.Location = lblDigite.Location;

        }

        private void EsqueceuSenhaForm_Load(object sender, EventArgs e)
        {
            txtTeste.BackColor=Color.White;
            // Coloca o TextBox fora da área visível
            txtTeste.Location = new Point(-200, -200); // Fora da tela
            lblCodigo.Location= new Point(-200, -200); // Fora da tela
            pnBarra2.Location= new Point(-200, -200); // Fora da tela
        }
    }
}
