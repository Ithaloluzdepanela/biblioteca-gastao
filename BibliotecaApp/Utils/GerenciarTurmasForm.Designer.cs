namespace BibliotecaApp.Utils
{
    partial class GerenciarTurmasForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.GroupBox grpLista;
        private System.Windows.Forms.ListBox listBoxTurmas;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnRemover;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.Label lblContagem;
        private System.Windows.Forms.GroupBox grpAdicionar;
        private System.Windows.Forms.Label lblNovaTurma;
        private System.Windows.Forms.TextBox txtNovaTurma;
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnDesfazer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GerenciarTurmasForm));
            this.grpLista = new System.Windows.Forms.GroupBox();
            this.listBoxTurmas = new System.Windows.Forms.ListBox();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnRemover = new System.Windows.Forms.Button();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.lblContagem = new System.Windows.Forms.Label();
            this.grpAdicionar = new System.Windows.Forms.GroupBox();
            this.lblNovaTurma = new System.Windows.Forms.Label();
            this.txtNovaTurma = new System.Windows.Forms.TextBox();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnDesfazer = new System.Windows.Forms.Button();
            this.grpLista.SuspendLayout();
            this.grpAdicionar.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpLista
            // 
            this.grpLista.Controls.Add(this.listBoxTurmas);
            this.grpLista.Controls.Add(this.btnEditar);
            this.grpLista.Controls.Add(this.btnRemover);
            this.grpLista.Controls.Add(this.btnRestaurar);
            this.grpLista.Controls.Add(this.lblContagem);
            this.grpLista.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpLista.Location = new System.Drawing.Point(12, 12);
            this.grpLista.Name = "grpLista";
            this.grpLista.Size = new System.Drawing.Size(486, 340);
            this.grpLista.TabIndex = 0;
            this.grpLista.TabStop = false;
            this.grpLista.Text = "Turmas cadastradas";
            // 
            // listBoxTurmas
            // 
            this.listBoxTurmas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxTurmas.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.listBoxTurmas.ItemHeight = 17;
            this.listBoxTurmas.Location = new System.Drawing.Point(10, 24);
            this.listBoxTurmas.Name = "listBoxTurmas";
            this.listBoxTurmas.ScrollAlwaysVisible = true;
            this.listBoxTurmas.Size = new System.Drawing.Size(352, 274);
            this.listBoxTurmas.TabIndex = 0;
            this.listBoxTurmas.SelectedIndexChanged += new System.EventHandler(this.listBoxTurmas_SelectedIndexChanged);
            // 
            // btnEditar
            // 
            this.btnEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnEditar.Enabled = false;
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.ForeColor = System.Drawing.Color.White;
            this.btnEditar.Location = new System.Drawing.Point(378, 24);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(94, 34);
            this.btnEditar.TabIndex = 1;
            this.btnEditar.Text = "✏ Editar";
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnRemover
            // 
            this.btnRemover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.btnRemover.Enabled = false;
            this.btnRemover.FlatAppearance.BorderSize = 0;
            this.btnRemover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemover.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRemover.ForeColor = System.Drawing.Color.White;
            this.btnRemover.Location = new System.Drawing.Point(377, 64);
            this.btnRemover.Name = "btnRemover";
            this.btnRemover.Size = new System.Drawing.Size(96, 35);
            this.btnRemover.TabIndex = 2;
            this.btnRemover.Text = "✖ Remover";
            this.btnRemover.UseVisualStyleBackColor = false;
            this.btnRemover.Click += new System.EventHandler(this.btnRemover_Click);
            // 
            // btnRestaurar
            // 
            this.btnRestaurar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnRestaurar.FlatAppearance.BorderSize = 0;
            this.btnRestaurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestaurar.ForeColor = System.Drawing.Color.White;
            this.btnRestaurar.Location = new System.Drawing.Point(378, 297);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(94, 34);
            this.btnRestaurar.TabIndex = 3;
            this.btnRestaurar.Text = "↩ Padrões";
            this.btnRestaurar.UseVisualStyleBackColor = false;
            this.btnRestaurar.Click += new System.EventHandler(this.btnRestaurar_Click);
            // 
            // lblContagem
            // 
            this.lblContagem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblContagem.ForeColor = System.Drawing.Color.Gray;
            this.lblContagem.Location = new System.Drawing.Point(11, 305);
            this.lblContagem.Name = "lblContagem";
            this.lblContagem.Size = new System.Drawing.Size(250, 18);
            this.lblContagem.TabIndex = 4;
            // 
            // grpAdicionar
            // 
            this.grpAdicionar.Controls.Add(this.lblNovaTurma);
            this.grpAdicionar.Controls.Add(this.txtNovaTurma);
            this.grpAdicionar.Controls.Add(this.btnAdicionar);
            this.grpAdicionar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpAdicionar.Location = new System.Drawing.Point(12, 369);
            this.grpAdicionar.Name = "grpAdicionar";
            this.grpAdicionar.Size = new System.Drawing.Size(486, 60);
            this.grpAdicionar.TabIndex = 1;
            this.grpAdicionar.TabStop = false;
            this.grpAdicionar.Text = "Adicionar nova turma";
            // 
            // lblNovaTurma
            // 
            this.lblNovaTurma.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNovaTurma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblNovaTurma.Location = new System.Drawing.Point(10, 28);
            this.lblNovaTurma.Name = "lblNovaTurma";
            this.lblNovaTurma.Size = new System.Drawing.Size(50, 20);
            this.lblNovaTurma.TabIndex = 0;
            this.lblNovaTurma.Text = "Nome:";
            // 
            // txtNovaTurma
            // 
            this.txtNovaTurma.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtNovaTurma.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNovaTurma.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNovaTurma.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNovaTurma.Location = new System.Drawing.Point(60, 25);
            this.txtNovaTurma.MaxLength = 80;
            this.txtNovaTurma.Name = "txtNovaTurma";
            this.txtNovaTurma.Size = new System.Drawing.Size(284, 25);
            this.txtNovaTurma.TabIndex = 1;
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnAdicionar.FlatAppearance.BorderSize = 0;
            this.btnAdicionar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdicionar.ForeColor = System.Drawing.Color.White;
            this.btnAdicionar.Location = new System.Drawing.Point(372, 20);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(108, 30);
            this.btnAdicionar.TabIndex = 2;
            this.btnAdicionar.Text = "+ Adicionar";
            this.btnAdicionar.UseVisualStyleBackColor = false;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnSalvar.Enabled = false;
            this.btnSalvar.FlatAppearance.BorderSize = 0;
            this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(12, 442);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(110, 36);
            this.btnSalvar.TabIndex = 2;
            this.btnSalvar.Text = "💾 Salvar";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // btnDesfazer
            // 
            this.btnDesfazer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnDesfazer.Enabled = false;
            this.btnDesfazer.FlatAppearance.BorderSize = 0;
            this.btnDesfazer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDesfazer.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDesfazer.ForeColor = System.Drawing.Color.White;
            this.btnDesfazer.Location = new System.Drawing.Point(128, 442);
            this.btnDesfazer.Name = "btnDesfazer";
            this.btnDesfazer.Size = new System.Drawing.Size(110, 36);
            this.btnDesfazer.TabIndex = 3;
            this.btnDesfazer.Text = "↩ Desfazer";
            this.btnDesfazer.UseVisualStyleBackColor = false;
            this.btnDesfazer.Click += new System.EventHandler(this.btnDesfazer_Click);
            // 
            // GerenciarTurmasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(510, 497);
            this.Controls.Add(this.grpLista);
            this.Controls.Add(this.grpAdicionar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.btnDesfazer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GerenciarTurmasForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gerenciar turmas padrão";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GerenciarTurmasForm_FormClosing);
            this.Load += new System.EventHandler(this.GerenciarTurmasForm_Load);
            this.grpLista.ResumeLayout(false);
            this.grpAdicionar.ResumeLayout(false);
            this.grpAdicionar.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}