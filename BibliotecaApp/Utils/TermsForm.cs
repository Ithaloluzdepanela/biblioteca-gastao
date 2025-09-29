using System;
using System.Windows.Forms;

namespace BibliotecaApp.Utils
{
    public partial class TermsForm : Form
    {
        public bool Accepted { get; private set; } = false;

        public TermsForm()
        {
            InitializeComponent();
            GetTermsRtf();

            rtbTerms.Rtf = GetTermsRtf();
        }

        private string GetTermsRtf()
        {
            // RichText formatado: \i ... \i0 deixa em itálico
            return @"{\rtf1\ansi
Última atualização: 13/09/2025\line
\line
Ao utilizar o Sistema BibliotecaApp (“Sistema”), você concorda integralmente com os termos e condições abaixo. Caso não concorde, não utilize o Sistema.\line
\line
1. Licença de Uso\line
- A {\i Beverso}\i0  concede ao usuário uma licença não exclusiva, intransferível, limitada e revogável para utilizar o Sistema apenas para gestão de bibliotecas.\line
- O uso do Sistema requer ativação por chave fornecida exclusivamente pela {\i Beverso}\i0.\line
- A licença é válida apenas na(s) máquina(s) em que a chave for ativada.\line
- A {\i Beverso}\i0  reserva-se o direito de suspender ou revogar a licença em caso de violação destes termos.\line
\line
2. Requisitos de Ativação\line
- Cada chave de ativação possui limite de ativações e uso pessoal.\line
- É proibido compartilhar, distribuir ou vender chaves.\line
- Qualquer tentativa de burlar o mecanismo de ativação ou gerar chaves indevidas é crime de violação de software.\line
\line
3. Uso Aceitável\line
- O Sistema deve ser utilizado somente para fins administrativos de bibliotecas.\line
- É proibido: modificar, copiar, descompilar, traduzir, engenharia reversa, redistribuir, usar para fins ilícitos ou transferir licença sem autorização da {\i Beverso}\i0.\line
\line
4. Responsabilidades do Usuário\line
- Manter backup regular de todos os dados armazenados.\line
- Proteger credenciais e controlar acesso ao computador.\line
- A {\i Beverso}\i0  não se responsabiliza por perda, alteração ou corrupção de dados.\line
\line
5. Privacidade e Proteção de Dados\line
- O Sistema pode armazenar informações de usuários da biblioteca, cadastros de livros e registros de empréstimos.\line
- A {\i Beverso}\i0  não coleta dados pessoais sem consentimento explícito.\line
- O usuário deve proteger os dados contra acessos não autorizados.\line
- Em caso de violação, o usuário deve notificar a {\i Beverso}\i0  imediatamente.\line
\line
6. Suporte e Atualizações\line
- Atualizações podem incluir correções de bugs, melhorias de segurança ou novas funcionalidades.\line
- Sujeitas a novos termos de uso.\line
- O suporte técnico não garante recuperação de dados perdidos ou falhas de terceiros.\line
\line
7. Garantia Limitada\line
- O Sistema é fornecido “no estado em que se encontra”, sem garantias expressas ou implícitas.\line
- A {\i Beverso}\i0  não será responsável por danos diretos, indiretos, incidentais, especiais ou consequenciais.\line
- A responsabilidade total da {\i Beverso}\i0, se houver, será limitada ao valor pago pelo Sistema.\line
\line
8. Propriedade Intelectual\line
- O Sistema, incluindo código, design, imagens e documentação, é propriedade exclusiva da {\i Beverso}\i0.\line
- Nenhum direito de propriedade intelectual é transferido ao usuário.\line
- Uso indevido de direitos autorais, patentes ou marcas registradas poderá resultar em ação judicial.\line
\line
9. Rescisão\line
- A {\i Beverso}\i0  pode rescindir a licença em caso de violação destes termos.\line
- Após a rescisão, o usuário deve desinstalar o Sistema e excluir todos os arquivos relacionados.\line
- Violação pode implicar responsabilidade civil e criminal.\line
\line
10. Alterações nos Termos\line
- A {\i Beverso}\i0  pode modificar ou atualizar os termos a qualquer momento.\line
- É responsabilidade do usuário verificar periodicamente a versão mais recente.\line
\line
11. Legislação Aplicável\line
- Estes termos são regidos pelas leis da República Federativa do Brasil.\line
- Litígios serão resolvidos no foro da cidade de Bocaiuva, Estado de Minas Gerais, Brasil.\line
\line
12. Aceitação do Usuário\line
- Ao clicar em “Li e aceito os termos de uso”, você declara que leu, compreendeu e concorda integralmente.\line
- Caso não aceite os termos, não poderá utilizar o Sistema.\line
}";
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (!chkAccept.Checked)
            {
                MessageBox.Show("Você precisa aceitar os termos de uso para continuar.",
                                "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Accepted = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        

        private void picExit_Click(object sender, EventArgs e)
        {
            const string msg = "Tem certeza de que quer fechar a Aplicação?";
            const string box = "Confirmação de Encerramento";
            var confirma = MessageBox.Show(msg, box, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirma == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
