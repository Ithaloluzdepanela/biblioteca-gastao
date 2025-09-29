using BibliotecaApp.Utils;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using System.IO;

namespace BibliotecaApp.Utils
{
    public partial class ActivationForm : Form
    {
        public ActivationForm()
        {
            InitializeComponent();
           
        }

        private LicenseData loadedLicense;

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "License files (*.lic)|*.lic|All files (*.*)|*.*";
                ofd.Title = "Selecione o arquivo de licença (.lic)";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    txtFilePath.Text = ofd.FileName;

                    // Valida e já preenche informações
                    LicenseData license;
                    string error;
                    bool ok = LicenseValidator.ValidateLicenseFileDetailed(ofd.FileName, out license, out error);

                    txtInfo.Clear();
                    loadedLicense = null;

                    if (!ok)
                    {
                        txtInfo.Text = "VALIDAÇÃO FALHOU: " + error + Environment.NewLine;
                        MessageBox.Show(this, "Licença inválida: " + error, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        loadedLicense = license;
                        txtInfo.Text = "Licença carregada com sucesso!" + Environment.NewLine;
                        txtInfo.AppendText("Tipo: " + license.Type + Environment.NewLine);
                        txtInfo.AppendText("Chave: " + license.Key + Environment.NewLine);
                        txtInfo.AppendText("TargetMachineId: " + (license.TargetMachineId ?? "<nenhum>") + Environment.NewLine);
                        txtInfo.AppendText("Emitida: " + license.DateIssued.ToString("dd/MM/yyyy HH:mm") + Environment.NewLine);
                        txtInfo.AppendText("Expira: " + (license.ExpireDate == DateTime.MaxValue ? "Permanente" : license.ExpireDate.ToString("dd/MM/yyyy")) + Environment.NewLine);
                    }
                }
            }
        }

        private void BtnValidate_Click(object sender, EventArgs e)
        {
            if (loadedLicense == null)
            {
                MessageBox.Show(this, "Nenhuma licença válida carregada. Selecione um arquivo primeiro.", "Atenção",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                LicenseValidator.SaveActivation(loadedLicense);
                MessageBox.Show(this, "Ativação concluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Erro ao salvar ativação local: " + ex.Message, "Erro",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picExit_Click(object sender, EventArgs e)
        { 
            const string msg = "Tem certeza de que quer abortar a Ativaçao do Sistema?";
            const string box = "Confirmação de Encerramento";
            var confirma = MessageBox.Show(msg, box, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirma == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
    }
