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
            // RichText formatado: \i ... \i0 para itálico, \b ... \b0 para negrito, \line para quebra de linha
            return @"{\rtf1\ansi
{\b Termos de Uso — BibliotecaApp}\line
\line
{\b Última atualização:} 13/09/2025\line
\line
Este documento estabelece os termos e condições aplicáveis ao uso do software {\b BibliotecaApp}\b0  (doravante, “Sistema”), desenvolvido e licenciado por {\b Beverso}\b0  (doravante, “Beverso” ou “Licenciante”). O Licenciado indicado neste Termo é a {\b Escola Estadual Professor Gastão Valle}\b0  (doravante, “Instituição” ou “Licenciado”).\line
\line
Ao utilizar o Sistema, a Instituição concorda integralmente com estes Termos. Caso não concorde, não instale, ative ou utilize o Sistema.\line
\line
{\b 1. Concessão de Licença}\line
1.1. A Beverso concede à Instituição uma {\b licença limitada, não exclusiva, intransferível e revogável} para utilização do Sistema, exclusivamente para fins administrativos da biblioteca da Instituição.\line
1.2. Esta licença refere-se {\b apenas à edição única e exclusiva} do Sistema disponibilizada gratuitamente à Instituição nesta data. Essa edição única {\b não implicará transferência de titularidade} do software nem de quaisquer direitos de propriedade intelectual.\line
1.3. A licença é válida somente nas máquinas ou ambientes em que a chave de ativação fornecida pela Beverso for corretamente ativada, observados os limites de ativações comunicados pela Beverso.\line
\line
{\b 2. Ativação e Controle de Chaves}\line
2.1. A ativação do Sistema depende de chave fornecida pela Beverso. Cada chave possui limite de ativações e é destinada ao uso exclusivo da Instituição.\line
2.2. É proibido compartilhar, distribuir, alugar, sublicenciar, ceder, vender ou de qualquer forma transferir as chaves de ativação a terceiros. Qualquer tentativa de fraudar, burlar ou manipular o mecanismo de ativação poderá ensejar responsabilização civil e criminal.\line
\line
{\b 3. Uso Aceitável e Restrições}\line
3.1. O Sistema deverá ser utilizado somente para a gestão da biblioteca da Instituição.\line
3.2. Fica expressamente vedado: (i) copiar, modificar, adaptar, traduzir, descompilar, fazer engenharia reversa, desmontar, produzir trabalhos derivados, ou de qualquer forma tentar acessar o código-fonte; (ii) redistribuir, sublicenciar, revender ou disponibilizar o Sistema a terceiros; (iii) utilizar o Sistema para fins ilícitos ou em desacordo com estes Termos.\line
3.3. A Instituição declara que não realizará publicações, distribuições ou disponibilizações do Sistema ou de quaisquer partes do mesmo sem autorização escrita e prévia da Beverso.\line
\line
{\b 4. Propriedade Intelectual}\line
4.1. Todo o código-fonte, documentação, design, interfaces, banco de dados, imagens, fluxos, marcas, e demais componentes do Sistema são {\b propriedade exclusiva da Beverso}.\line
4.2. Nenhum direito de propriedade intelectual é transferido à Instituição em razão destes Termos, salvo o direito limitado de uso descrito na cláusula 1.\line
4.3. A Instituição reconhece a autoria e paternidade da Beverso sobre o Sistema e concorda em não contestar tais direitos.\line
\line
{\b 5. Edição Única e Sem Custos Adicionais}\line
5.1. A edição do Sistema fornecida à Instituição nesta data foi disponibilizada {\b uma única vez} e {\b sem custos de licenciamento adicionais}.\line
5.2. A Beverso reserva-se o direito de condicionar atualizações, novas versões, suporte técnico continuado ou customizações à contratação de serviços pagos, conforme previsto na Cláusula 6.\line
\line
{\b 6. Suporte, Atualizações e Customizações}\line
6.1. Atualizações, manutenção, correções de segurança, funcionalidades novas ou melhorias {\b serão fornecidas somente mediante contratação} específica entre a Instituição e a Beverso, salvo quando expressamente previsto em documento contrário assinado por ambas as partes.\line
6.2. Customizações, integrações com sistemas de terceiros ou adaptações do Sistema também dependem de contrato específico e remuneração adicional.\line
6.3. O suporte técnico prestado pela Beverso, quando contratado, terá o escopo definido no respectivo contrato de prestação de serviços.\line
\line
{\b 7. Responsabilidades da Instituição}\line
7.1. A Instituição é responsável por: (i) manter cópias de segurança (backup) atualizadas de todos os dados introduzidos no Sistema; (ii) proteger credenciais de acesso (usuários e senhas); (iii) garantir a segurança física e lógica das máquinas e da rede onde o Sistema estiver instalado; (iv) cumprir a legislação aplicável ao tratamento de dados pessoais.\line
7.2. A Beverso não assume responsabilidade pela perda, dano, corrupção ou indisponibilidade de dados causada por má utilização, falhas de hardware, falhas de terceiros, ataques, ou por ausência de backups adequados pela Instituição.\line
\line
{\b 8. Privacidade e Proteção de Dados}\line
8.1. O Sistema pode processar e armazenar dados pessoais de usuários da biblioteca (alunos, professores, servidores, terceiros). A Instituição, enquanto controladora dos dados, declara e garante que observará a legislação aplicável, inclusive a Lei Geral de Proteção de Dados Pessoais — LGPD (Lei nº 13.709/2018), quando aplicável.\line
8.2. A Beverso não coletará dados pessoais da Instituição ou dos usuários sem consentimento explícito ou previsão contratual, exceto para finalidades técnicas estritamente necessárias (por exemplo, logs de erro, telemetria) quando previamente informadas.\line
8.3. Em caso de incidente de segurança que envolva dados pessoais, a Instituição deverá notificar a Beverso tempestivamente, e as partes cooperarão para mitigar impactos, observados os deveres legais de comunicação às autoridades competentes e titulares, quando exigido.\line
\line
{\b 9. Auditoria e Monitoramento}\line
9.1. A Beverso poderá, mediante aviso prévio e observadas as restrições legais aplicáveis, realizar auditorias técnicas ou operacionais para verificar o cumprimento destes Termos, ou em razão de indícios de uso indevido do Sistema.\line
9.2. Caso seja constatado uso indevido, a Beverso poderá suspender o acesso até regularização, sem prejuízo da adoção das medidas legais cabíveis.\line
\line
{\b 10. Vigência}\line
10.1. Estes Termos vigoram por prazo {\b vitalício} quanto à edição única e exclusiva fornecida gratuitamente à Instituição nesta data, ressalvadas situações de rescisão previstas nestes Termos ou por decisão judicial.\line
10.2. A Beverso poderá rescindir a licença em caso de descumprimento destes Termos, fraude ou uso indevido. Após a rescisão, a Instituição deverá desinstalar o Sistema e excluir todos os arquivos relacionados, salvo disposição contratual em contrário.\line
\line
{\b 11. Garantia e Limitação de Responsabilidade}\line
11.1. O Sistema é fornecido “no estado em que se encontra” ({\i as is}\i0). A Beverso não concede garantias expressas ou implícitas quanto a adequação a um propósito específico, ausência de erros ou compatibilidade com equipamentos da Instituição.\line
11.2. A Beverso não será responsável por danos indiretos, especiais, incidentais, emergentes ou lucros cessantes decorrentes do uso ou incapacidade de uso do Sistema.\line
11.3. A responsabilidade total da Beverso, quando existente, fica limitada ao valor efetivamente pago pela Instituição pela contratação de serviços vinculados ao Sistema, se houver.\line
\line
{\b 12. Rescisão}\line
12.1. A Beverso poderá rescindir a licença e o acesso ao Sistema em caso de violação destes Termos, mediante notificação e prazo para regularização quando aplicável.\line
12.2. A rescisão não exime as partes de responsabilidades por atos praticados anteriormente.\line
\line
{\b 13. Alterações destes Termos}\line
13.1. A Beverso poderá modificar estes Termos a qualquer momento. Quando houver alterações relevantes, a Beverso informará a Instituição por meio do canal de comunicação previamente estabelecido.\line
13.2. É responsabilidade da Instituição verificar periodicamente a versão mais recente destes Termos.\line
\line
{\b 14. Lei Aplicável e Foro}\line
14.1. Estes Termos serão regidos e interpretados de acordo com as leis da República Federativa do Brasil.\line
14.2. Fica eleito o foro da comarca de Bocaiuva, Estado de Minas Gerais, Brasil, como competente para dirimir quaisquer controvérsias decorrentes destes Termos, com renúncia expressa a qualquer outro, por mais privilegiado que seja.\line
\line
{\b 15. Disposições Finais}\line
15.1. Caso qualquer disposição destes Termos seja considerada inválida ou inexequível, as demais disposições permanecerão em pleno vigor.\line
15.2. A eventual tolerância da Beverso quanto ao descumprimento de quaisquer das obrigações estabelecidas nestes Termos não importará em renúncia de direitos.\line
\line
Ao clicar em “Li e aceito os termos de uso”, a Instituição declara ter lido, compreendido e concordado com todos os termos e condições aqui descritos.\line
\line
{\b Beverso.}\line
\line
{\b Licenciado:} Escola Estadual Professor Gastão Valle\line
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
