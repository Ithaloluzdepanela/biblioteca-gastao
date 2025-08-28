// EmprestimoRapidoForm.Designer.cs
namespace BibliotecaApp.Forms.Livros
{
    partial class EmprestimoRapidoForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lstSugestoesProfessor = new System.Windows.Forms.ListBox();
            this.lstSugestoesLivro = new System.Windows.Forms.ListBox();
            this.cbBibliotecaria = new RoundedComboBox();
            this.txtTurma = new RoundedTextBox();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.lblBibliotecaria = new System.Windows.Forms.Label();
            this.lblDevolucao = new System.Windows.Forms.Label();
            this.lblQuantidade = new System.Windows.Forms.Label();
            this.lblTurma = new System.Windows.Forms.Label();
            this.txtLivro = new RoundedTextBox();
            this.lblLivro = new System.Windows.Forms.Label();
            this.lblProfessor = new System.Windows.Forms.Label();
            this.txtProfessor = new RoundedTextBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.lstSugestoesTurma = new System.Windows.Forms.ListBox();
            this.numQuantidade = new System.Windows.Forms.NumericUpDown();
            this.dtpDevolucao = new System.Windows.Forms.DateTimePicker();
            this.dgvRapidos = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRapidos)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lstSugestoesProfessor);
            this.panel1.Controls.Add(this.lstSugestoesLivro);
            this.panel1.Controls.Add(this.cbBibliotecaria);
            this.panel1.Controls.Add(this.txtTurma);
            this.panel1.Controls.Add(this.btnRegistrar);
            this.panel1.Controls.Add(this.lblBibliotecaria);
            this.panel1.Controls.Add(this.lblDevolucao);
            this.panel1.Controls.Add(this.lblQuantidade);
            this.panel1.Controls.Add(this.lblTurma);
            this.panel1.Controls.Add(this.txtLivro);
            this.panel1.Controls.Add(this.lblLivro);
            this.panel1.Controls.Add(this.lblProfessor);
            this.panel1.Controls.Add(this.txtProfessor);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Controls.Add(this.lstSugestoesTurma);
            this.panel1.Controls.Add(this.numQuantidade);
            this.panel1.Controls.Add(this.dtpDevolucao);
            this.panel1.Controls.Add(this.dgvRapidos);
            this.panel1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.panel1.Location = new System.Drawing.Point(160, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(980, 845);
            this.panel1.TabIndex = 0;
            // 
            // lstSugestoesProfessor
            // 
            this.lstSugestoesProfessor.ItemHeight = 21;
            this.lstSugestoesProfessor.Location = new System.Drawing.Point(64, 160);
            this.lstSugestoesProfessor.Name = "lstSugestoesProfessor";
            this.lstSugestoesProfessor.Size = new System.Drawing.Size(372, 109);
            this.lstSugestoesProfessor.TabIndex = 3;
            this.lstSugestoesProfessor.Visible = false;
            this.lstSugestoesProfessor.Click += new System.EventHandler(this.lstSugestoesProfessor_Click);
            // 
            // lstSugestoesLivro
            // 
            this.lstSugestoesLivro.ItemHeight = 21;
            this.lstSugestoesLivro.Location = new System.Drawing.Point(464, 160);
            this.lstSugestoesLivro.Name = "lstSugestoesLivro";
            this.lstSugestoesLivro.Size = new System.Drawing.Size(364, 109);
            this.lstSugestoesLivro.TabIndex = 6;
            this.lstSugestoesLivro.Visible = false;
            this.lstSugestoesLivro.Click += new System.EventHandler(this.lstSugestoesLivro_Click);
            // 
            // cbBibliotecaria
            // 
            this.cbBibliotecaria.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbBibliotecaria.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbBibliotecaria.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbBibliotecaria.BorderRadius = 8;
            this.cbBibliotecaria.BorderThickness = 1;
            this.cbBibliotecaria.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbBibliotecaria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBibliotecaria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBibliotecaria.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBibliotecaria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.cbBibliotecaria.FormattingEnabled = true;
            this.cbBibliotecaria.ItemsFont = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBibliotecaria.Location = new System.Drawing.Point(639, 228);
            this.cbBibliotecaria.Name = "cbBibliotecaria";
            this.cbBibliotecaria.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBibliotecaria.PlaceholderMargin = 10;
            this.cbBibliotecaria.PlaceholderText = "Selecione a bibliotecária ...";
            this.cbBibliotecaria.Size = new System.Drawing.Size(285, 34);
            this.cbBibliotecaria.TabIndex = 93;
            // 
            // txtTurma
            // 
            this.txtTurma.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtTurma.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtTurma.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtTurma.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtTurma.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtTurma.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtTurma.BorderRadius = 10;
            this.txtTurma.BorderThickness = 1;
            this.txtTurma.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTurma.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTurma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtTurma.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtTurma.Location = new System.Drawing.Point(64, 225);
            this.txtTurma.Name = "txtTurma";
            this.txtTurma.Padding = new System.Windows.Forms.Padding(7);
            this.txtTurma.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtTurma.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTurma.PlaceholderMarginLeft = 12;
            this.txtTurma.PlaceholderText = "Digite aqui a turma...";
            this.txtTurma.Size = new System.Drawing.Size(263, 40);
            this.txtTurma.TabIndex = 92;
            this.txtTurma.TextColor = System.Drawing.Color.Black;
            this.txtTurma.UseSystemPasswordChar = false;
            this.txtTurma.TextChanged += new System.EventHandler(this.txtTurma_TextChanged);
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRegistrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnRegistrar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnRegistrar.ForeColor = System.Drawing.Color.White;
            this.btnRegistrar.Location = new System.Drawing.Point(775, 317);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(149, 57);
            this.btnRegistrar.TabIndex = 91;
            this.btnRegistrar.Text = "REGISTRAR";
            this.btnRegistrar.UseVisualStyleBackColor = false;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            // 
            // lblBibliotecaria
            // 
            this.lblBibliotecaria.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBibliotecaria.AutoSize = true;
            this.lblBibliotecaria.BackColor = System.Drawing.Color.Transparent;
            this.lblBibliotecaria.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBibliotecaria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblBibliotecaria.Location = new System.Drawing.Point(634, 197);
            this.lblBibliotecaria.Name = "lblBibliotecaria";
            this.lblBibliotecaria.Size = new System.Drawing.Size(123, 25);
            this.lblBibliotecaria.TabIndex = 90;
            this.lblBibliotecaria.Text = "Bibliotecária:";
            // 
            // lblDevolucao
            // 
            this.lblDevolucao.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDevolucao.AutoSize = true;
            this.lblDevolucao.BackColor = System.Drawing.Color.Transparent;
            this.lblDevolucao.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDevolucao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblDevolucao.Location = new System.Drawing.Point(373, 197);
            this.lblDevolucao.Name = "lblDevolucao";
            this.lblDevolucao.Size = new System.Drawing.Size(217, 25);
            this.lblDevolucao.TabIndex = 89;
            this.lblDevolucao.Text = "Horario do Emprestimo:";
            // 
            // lblQuantidade
            // 
            this.lblQuantidade.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblQuantidade.AutoSize = true;
            this.lblQuantidade.BackColor = System.Drawing.Color.Transparent;
            this.lblQuantidade.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantidade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblQuantidade.Location = new System.Drawing.Point(846, 92);
            this.lblQuantidade.Name = "lblQuantidade";
            this.lblQuantidade.Size = new System.Drawing.Size(49, 25);
            this.lblQuantidade.TabIndex = 88;
            this.lblQuantidade.Text = "Qtd:";
            // 
            // lblTurma
            // 
            this.lblTurma.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTurma.AutoSize = true;
            this.lblTurma.BackColor = System.Drawing.Color.Transparent;
            this.lblTurma.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTurma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblTurma.Location = new System.Drawing.Point(59, 197);
            this.lblTurma.Name = "lblTurma";
            this.lblTurma.Size = new System.Drawing.Size(71, 25);
            this.lblTurma.TabIndex = 87;
            this.lblTurma.Text = "Turma:";
            // 
            // txtLivro
            // 
            this.txtLivro.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtLivro.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtLivro.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtLivro.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtLivro.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtLivro.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtLivro.BorderRadius = 10;
            this.txtLivro.BorderThickness = 1;
            this.txtLivro.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLivro.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLivro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtLivro.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtLivro.Location = new System.Drawing.Point(460, 120);
            this.txtLivro.Name = "txtLivro";
            this.txtLivro.Padding = new System.Windows.Forms.Padding(7);
            this.txtLivro.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtLivro.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLivro.PlaceholderMarginLeft = 12;
            this.txtLivro.PlaceholderText = "Digite aqui o livro...";
            this.txtLivro.Size = new System.Drawing.Size(368, 40);
            this.txtLivro.TabIndex = 86;
            this.txtLivro.TextColor = System.Drawing.Color.Black;
            this.txtLivro.UseSystemPasswordChar = false;
            this.txtLivro.TextChanged += new System.EventHandler(this.txtLivro_TextChanged);
            // 
            // lblLivro
            // 
            this.lblLivro.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblLivro.AutoSize = true;
            this.lblLivro.BackColor = System.Drawing.Color.Transparent;
            this.lblLivro.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLivro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblLivro.Location = new System.Drawing.Point(455, 92);
            this.lblLivro.Name = "lblLivro";
            this.lblLivro.Size = new System.Drawing.Size(59, 25);
            this.lblLivro.TabIndex = 85;
            this.lblLivro.Text = "Livro:";
            // 
            // lblProfessor
            // 
            this.lblProfessor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblProfessor.AutoSize = true;
            this.lblProfessor.BackColor = System.Drawing.Color.Transparent;
            this.lblProfessor.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfessor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblProfessor.Location = new System.Drawing.Point(59, 92);
            this.lblProfessor.Name = "lblProfessor";
            this.lblProfessor.Size = new System.Drawing.Size(98, 25);
            this.lblProfessor.TabIndex = 84;
            this.lblProfessor.Text = "Professor:";
            // 
            // txtProfessor
            // 
            this.txtProfessor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtProfessor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtProfessor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtProfessor.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtProfessor.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtProfessor.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtProfessor.BorderRadius = 10;
            this.txtProfessor.BorderThickness = 1;
            this.txtProfessor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtProfessor.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProfessor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtProfessor.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtProfessor.Location = new System.Drawing.Point(64, 120);
            this.txtProfessor.Name = "txtProfessor";
            this.txtProfessor.Padding = new System.Windows.Forms.Padding(7);
            this.txtProfessor.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtProfessor.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProfessor.PlaceholderMarginLeft = 12;
            this.txtProfessor.PlaceholderText = "Digite aqui o professor...";
            this.txtProfessor.Size = new System.Drawing.Size(372, 40);
            this.txtProfessor.TabIndex = 64;
            this.txtProfessor.TextColor = System.Drawing.Color.Black;
            this.txtProfessor.UseSystemPasswordChar = false;
            this.txtProfessor.TextChanged += new System.EventHandler(this.txtProfessor_TextChanged);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.labelTitle.Location = new System.Drawing.Point(346, 20);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(336, 41);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "EMPRÉSTIMO RÁPIDO";
            // 
            // lstSugestoesTurma
            // 
            this.lstSugestoesTurma.ItemHeight = 21;
            this.lstSugestoesTurma.Location = new System.Drawing.Point(64, 265);
            this.lstSugestoesTurma.Name = "lstSugestoesTurma";
            this.lstSugestoesTurma.Size = new System.Drawing.Size(263, 109);
            this.lstSugestoesTurma.TabIndex = 9;
            this.lstSugestoesTurma.Visible = false;
            this.lstSugestoesTurma.Click += new System.EventHandler(this.lstSugestoesTurma_Click);
            // 
            // numQuantidade
            // 
            this.numQuantidade.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.numQuantidade.Location = new System.Drawing.Point(851, 124);
            this.numQuantidade.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numQuantidade.Name = "numQuantidade";
            this.numQuantidade.Size = new System.Drawing.Size(73, 33);
            this.numQuantidade.TabIndex = 11;
            this.numQuantidade.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // dtpDevolucao
            // 
            this.dtpDevolucao.CustomFormat = " HH:mm";
            this.dtpDevolucao.Enabled = false;
            this.dtpDevolucao.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.dtpDevolucao.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDevolucao.Location = new System.Drawing.Point(378, 229);
            this.dtpDevolucao.Name = "dtpDevolucao";
            this.dtpDevolucao.Size = new System.Drawing.Size(80, 33);
            this.dtpDevolucao.TabIndex = 13;
            // 
            // dgvRapidos
            // 
            this.dgvRapidos.AllowUserToAddRows = false;
            this.dgvRapidos.Location = new System.Drawing.Point(64, 449);
            this.dgvRapidos.Name = "dgvRapidos";
            this.dgvRapidos.ReadOnly = true;
            this.dgvRapidos.RowTemplate.Height = 40;
            this.dgvRapidos.Size = new System.Drawing.Size(860, 382);
            this.dgvRapidos.TabIndex = 17;
            this.dgvRapidos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRapidos_CellContentClick);
            this.dgvRapidos.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvRapidos_CellFormatting);
            this.dgvRapidos.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvRapidos_CellPainting);
            // 
            // EmprestimoRapidoForm
            // 
            this.ClientSize = new System.Drawing.Size(1280, 845);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EmprestimoRapidoForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRapidos)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ListBox lstSugestoesProfessor;
        private System.Windows.Forms.ListBox lstSugestoesLivro;
        private System.Windows.Forms.ListBox lstSugestoesTurma;
        private System.Windows.Forms.NumericUpDown numQuantidade;
        private System.Windows.Forms.DataGridView dgvRapidos;
        private System.Windows.Forms.DateTimePicker dtpDevolucao;
        private RoundedTextBox txtProfessor;
        public System.Windows.Forms.Label lblProfessor;
        public System.Windows.Forms.Label lblLivro;
        private RoundedTextBox txtLivro;
        public System.Windows.Forms.Label lblBibliotecaria;
        public System.Windows.Forms.Label lblDevolucao;
        public System.Windows.Forms.Label lblQuantidade;
        public System.Windows.Forms.Label lblTurma;
        private RoundedTextBox txtTurma;
        private System.Windows.Forms.Button btnRegistrar;
        private RoundedComboBox cbBibliotecaria;
    }
}
