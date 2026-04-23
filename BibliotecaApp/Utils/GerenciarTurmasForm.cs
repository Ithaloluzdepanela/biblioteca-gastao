using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BibliotecaApp.Utils
{
    public partial class GerenciarTurmasForm : Form
    {
        private List<string> _edicao;
        private bool _alterado = false;

        public GerenciarTurmasForm()
        {
            InitializeComponent();
        }

        private void GerenciarTurmasForm_Load(object sender, EventArgs e)
        {
            _edicao = new List<string>(TurmasUtil.TurmasPermitidas);
            RefreshListBox();
            AtualizarBotoes();
        }

        private void RefreshListBox()
        {
            string selecionado = listBoxTurmas.SelectedItem as string;
            listBoxTurmas.BeginUpdate();
            listBoxTurmas.Items.Clear();
            foreach (var turma in _edicao.OrderBy(x => x, StringComparer.OrdinalIgnoreCase))
                listBoxTurmas.Items.Add(turma);
            listBoxTurmas.EndUpdate();

            if (!string.IsNullOrEmpty(selecionado))
            {
                int idx = listBoxTurmas.FindStringExact(selecionado);
                if (idx >= 0)
                    listBoxTurmas.SelectedIndex = idx;
            }

            int n = _edicao.Count;
            lblContagem.Text = $"{n} turma(s) cadastrada(s)";
        }

        private void AtualizarBotoes()
        {
            bool temSelecao = listBoxTurmas.SelectedIndex >= 0;
            btnRemover.Enabled = temSelecao;
            btnEditar.Enabled = temSelecao;
            btnSalvar.Enabled = _alterado;
            btnDesfazer.Enabled = _alterado;
        }

        private void MarcaAlterado()
        {
            _alterado = true;
            AtualizarBotoes();
        }

        private void listBoxTurmas_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarBotoes();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            string nome = txtNovaTurma.Text.Trim().ToUpperInvariant();
            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Digite o nome da turma.", "Adicionar turma", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNovaTurma.Focus();
                return;
            }
            if (nome.Length < 4)
            {
                MessageBox.Show("O nome da turma deve ter pelo menos 4 caracteres.", "Adicionar turma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNovaTurma.Focus();
                return;
            }
            if (_edicao.Any(t => t.Equals(nome, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Já existe uma turma com este nome.", "Adicionar turma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNovaTurma.SelectAll();
                txtNovaTurma.Focus();
                return;
            }
            _edicao.Add(nome);
            RefreshListBox();
            MarcaAlterado();
            txtNovaTurma.Clear();
            txtNovaTurma.Focus();
            int idx = listBoxTurmas.FindStringExact(nome);
            if (idx >= 0)
                listBoxTurmas.SelectedIndex = idx;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (listBoxTurmas.SelectedIndex < 0) return;
            string antiga = listBoxTurmas.SelectedItem as string;
            string novo = MostrarInputBox("Editar turma", "Novo nome para a turma:", antiga);
            if (novo == null) return;
            novo = novo.Trim().ToUpperInvariant();
            if (novo == antiga) return;
            if (novo.Length < 4)
            {
                MessageBox.Show("O nome da turma deve ter pelo menos 4 caracteres.", "Editar turma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_edicao.Any(t => t.Equals(novo, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Já existe uma turma com este nome.", "Editar turma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int idx = _edicao.FindIndex(t => t.Equals(antiga, StringComparison.OrdinalIgnoreCase));
            if (idx >= 0)
            {
                _edicao[idx] = novo;
                RefreshListBox();
                MarcaAlterado();
            }
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            if (listBoxTurmas.SelectedIndex < 0) return;
            string turma = listBoxTurmas.SelectedItem as string;
            var r = MessageBox.Show(
                $"Tem certeza que deseja remover a turma:\n\n{turma}\n\nA remoção só será efetivada após salvar.",
                "Remover turma",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);
            if (r != DialogResult.Yes) return;
            _edicao.RemoveAll(t => t.Equals(turma, StringComparison.OrdinalIgnoreCase));
            RefreshListBox();
            MarcaAlterado();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (_edicao.Count == 0)
            {
                var r = MessageBox.Show(
                    "A lista está vazia. Deseja salvar assim mesmo?",
                    "Salvar turmas",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);
                if (r != DialogResult.Yes) return;
            }
            try
            {
                TurmasUtil.Salvar(new List<string>(_edicao));
                _alterado = false;
                AtualizarBotoes();
                MessageBox.Show(
                    $"{_edicao.Count} turma(s) salva(s) com sucesso.\nUm backup automático foi criado.",
                    "Salvar turmas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao salvar as turmas:\n{ex.Message}\nNenhuma alteração foi aplicada.",
                    "Salvar turmas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDesfazer_Click(object sender, EventArgs e)
        {
            var r = MessageBox.Show(
                "Deseja descartar as alterações e voltar ao estado salvo?", 
                "Desfazer alterações",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (r != DialogResult.Yes) return;
            _edicao = TurmasUtil.Carregar();
            _alterado = false;
            RefreshListBox();
            AtualizarBotoes();
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            var r = MessageBox.Show(
                "A lista atual será substituída pelos padrões originais.\nAs alterações só serão efetivas após salvar.\n\nDeseja continuar?",
                "Restaurar padrões",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (r != DialogResult.Yes) return;
            _edicao = TurmasUtil.TurmasPadrao();
            RefreshListBox();
            MarcaAlterado();
            MessageBox.Show(
                "Padrões carregados. Clique em Salvar para confirmar.",
                "Restaurar padrões",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void GerenciarTurmasForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_alterado) return;
            var r = MessageBox.Show(
                "Você tem alterações não salvas. Deseja salvar antes de fechar?",
                "Alterações pendentes",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);
            if (r == DialogResult.Yes)
            {
                btnSalvar_Click(sender, EventArgs.Empty);
                if (_alterado) e.Cancel = true;
            }
            else if (r == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            // No: fecha normalmente
        }

        // InputBox customizado
        private string MostrarInputBox(string titulo, string prompt, string valorInicial)
        {
            using (var f = new Form())
            {
                f.Text = titulo;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.MinimizeBox = false;
                f.MaximizeBox = false;
                f.StartPosition = FormStartPosition.CenterParent;
                f.BackColor = Color.White;
                f.ClientSize = new Size(440, 160);

                var lbl = new Label()
                {
                    Text = prompt,
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(16, 20),
                    Size = new Size(400, 24)
                };
                var txt = new TextBox()
                {
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(16, 52),
                    Size = new Size(400, 26),
                    CharacterCasing = CharacterCasing.Upper,
                    Text = valorInicial ?? ""
                };
                txt.SelectAll();

                var btnOK = new Button()
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    BackColor = Color.FromArgb(30, 61, 88),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(240, 100),
                    Size = new Size(80, 32)
                };
                btnOK.FlatAppearance.BorderSize = 0;

                var btnCancel = new Button()
                {
                    Text = "Cancelar",
                    DialogResult = DialogResult.Cancel,
                    BackColor = Color.FromArgb(220, 220, 220),
                    ForeColor = Color.FromArgb(20, 42, 60),
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(336, 100),
                    Size = new Size(80, 32)
                };
                btnCancel.FlatAppearance.BorderSize = 0;

                f.Controls.Add(lbl);
                f.Controls.Add(txt);
                f.Controls.Add(btnOK);
                f.Controls.Add(btnCancel);
                f.AcceptButton = btnOK;
                f.CancelButton = btnCancel;

                if (f.ShowDialog(this) == DialogResult.OK)
                    return txt.Text;
                return null;
            }
        }

       
    }
}
