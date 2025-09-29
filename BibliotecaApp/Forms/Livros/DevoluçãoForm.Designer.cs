namespace BibliotecaApp.Forms.Livros
{
    partial class DevoluçãoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnConfirmarDevolucao = new System.Windows.Forms.Button();
            this.btnProrrogar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblNome = new System.Windows.Forms.Label();
            this.Titulo = new System.Windows.Forms.Label();
            this.dgvEmprestimos = new System.Windows.Forms.DataGridView();
            this.lblDadosLivro = new System.Windows.Forms.Label();
            this.btnBuscarEmprestimo = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbFiltroEmprestimo = new RoundedComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNome = new RoundedTextBox();
            this.mtxCodigoBarras = new RoundedMaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmprestimos)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConfirmarDevolucao
            // 
            this.btnConfirmarDevolucao.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnConfirmarDevolucao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnConfirmarDevolucao.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnConfirmarDevolucao.ForeColor = System.Drawing.Color.White;
            this.btnConfirmarDevolucao.Location = new System.Drawing.Point(745, 706);
            this.btnConfirmarDevolucao.Name = "btnConfirmarDevolucao";
            this.btnConfirmarDevolucao.Size = new System.Drawing.Size(150, 60);
            this.btnConfirmarDevolucao.TabIndex = 118;
            this.btnConfirmarDevolucao.Text = "DEVOLVER";
            this.btnConfirmarDevolucao.UseVisualStyleBackColor = false;
            this.btnConfirmarDevolucao.Click += new System.EventHandler(this.btnConfirmarDevolucao_Click);
            // 
            // btnProrrogar
            // 
            this.btnProrrogar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnProrrogar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnProrrogar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnProrrogar.ForeColor = System.Drawing.Color.White;
            this.btnProrrogar.Location = new System.Drawing.Point(389, 706);
            this.btnProrrogar.Name = "btnProrrogar";
            this.btnProrrogar.Size = new System.Drawing.Size(150, 60);
            this.btnProrrogar.TabIndex = 117;
            this.btnProrrogar.Text = "PRORROGAR";
            this.btnProrrogar.UseVisualStyleBackColor = false;
            this.btnProrrogar.Click += new System.EventHandler(this.btnProrrogar_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label4.Location = new System.Drawing.Point(436, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 25);
            this.label4.TabIndex = 116;
            this.label4.Text = "Código de barras:";
            // 
            // lblNome
            // 
            this.lblNome.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblNome.AutoSize = true;
            this.lblNome.BackColor = System.Drawing.Color.Transparent;
            this.lblNome.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblNome.Location = new System.Drawing.Point(151, 162);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(140, 25);
            this.lblNome.TabIndex = 107;
            this.lblNome.Text = "Nome do livro:";
            this.lblNome.Click += new System.EventHandler(this.lblNome_Click);
            // 
            // Titulo
            // 
            this.Titulo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Titulo.Font = new System.Drawing.Font("Segoe UI", 25.25F, System.Drawing.FontStyle.Bold);
            this.Titulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.Titulo.Location = new System.Drawing.Point(448, 57);
            this.Titulo.Name = "Titulo";
            this.Titulo.Size = new System.Drawing.Size(384, 46);
            this.Titulo.TabIndex = 106;
            this.Titulo.Text = "DEVOLUÇÃO DE LIVRO";
            // 
            // dgvEmprestimos
            // 
            this.dgvEmprestimos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dgvEmprestimos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEmprestimos.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvEmprestimos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvEmprestimos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEmprestimos.Location = new System.Drawing.Point(27, 290);
            this.dgvEmprestimos.Name = "dgvEmprestimos";
            this.dgvEmprestimos.ReadOnly = true;
            this.dgvEmprestimos.RowHeadersWidth = 51;
            this.dgvEmprestimos.RowTemplate.Height = 24;
            this.dgvEmprestimos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEmprestimos.Size = new System.Drawing.Size(1226, 407);
            this.dgvEmprestimos.TabIndex = 119;
            this.dgvEmprestimos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEmprestimos_CellContentClick);
            this.dgvEmprestimos.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvEmprestimos_CellFormatting);
            this.dgvEmprestimos.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvEmprestimos_CellPainting);
            // 
            // lblDadosLivro
            // 
            this.lblDadosLivro.AutoSize = true;
            this.lblDadosLivro.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDadosLivro.Location = new System.Drawing.Point(22, 321);
            this.lblDadosLivro.Name = "lblDadosLivro";
            this.lblDadosLivro.Size = new System.Drawing.Size(0, 19);
            this.lblDadosLivro.TabIndex = 121;
            // 
            // btnBuscarEmprestimo
            // 
            this.btnBuscarEmprestimo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnBuscarEmprestimo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnBuscarEmprestimo.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnBuscarEmprestimo.ForeColor = System.Drawing.Color.White;
            this.btnBuscarEmprestimo.Location = new System.Drawing.Point(974, 176);
            this.btnBuscarEmprestimo.Name = "btnBuscarEmprestimo";
            this.btnBuscarEmprestimo.Size = new System.Drawing.Size(150, 60);
            this.btnBuscarEmprestimo.TabIndex = 123;
            this.btnBuscarEmprestimo.Text = "Procurar";
            this.btnBuscarEmprestimo.UseVisualStyleBackColor = false;
            this.btnBuscarEmprestimo.Click += new System.EventHandler(this.btnBuscarEmprestimo_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.cbFiltroEmprestimo);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.Titulo);
            this.panel1.Controls.Add(this.btnBuscarEmprestimo);
            this.panel1.Controls.Add(this.lblNome);
            this.panel1.Controls.Add(this.txtNome);
            this.panel1.Controls.Add(this.mtxCodigoBarras);
            this.panel1.Controls.Add(this.dgvEmprestimos);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnConfirmarDevolucao);
            this.panel1.Controls.Add(this.btnProrrogar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 845);
            this.panel1.TabIndex = 124;
            // 
            // cbFiltroEmprestimo
            // 
            this.cbFiltroEmprestimo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbFiltroEmprestimo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbFiltroEmprestimo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbFiltroEmprestimo.BorderRadius = 8;
            this.cbFiltroEmprestimo.BorderThickness = 1;
            this.cbFiltroEmprestimo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbFiltroEmprestimo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltroEmprestimo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFiltroEmprestimo.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.cbFiltroEmprestimo.FormattingEnabled = true;
            this.cbFiltroEmprestimo.Items.AddRange(new object[] {
            "Todos",
            "Devolvido",
            "Atrasado",
            "Ativo"});
            this.cbFiltroEmprestimo.ItemsFont = new System.Drawing.Font("Segoe UI", 14.25F);
            this.cbFiltroEmprestimo.Location = new System.Drawing.Point(660, 193);
            this.cbFiltroEmprestimo.Name = "cbFiltroEmprestimo";
            this.cbFiltroEmprestimo.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14F);
            this.cbFiltroEmprestimo.PlaceholderMargin = 10;
            this.cbFiltroEmprestimo.PlaceholderText = "Selecione uma situação...";
            this.cbFiltroEmprestimo.Size = new System.Drawing.Size(259, 34);
            this.cbFiltroEmprestimo.TabIndex = 125;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label6.Location = new System.Drawing.Point(655, 165);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 25);
            this.label6.TabIndex = 124;
            this.label6.Text = "Situação:";
            // 
            // txtNome
            // 
            this.txtNome.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtNome.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtNome.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtNome.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtNome.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtNome.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtNome.BorderRadius = 10;
            this.txtNome.BorderThickness = 1;
            this.txtNome.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNome.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtNome.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtNome.Location = new System.Drawing.Point(156, 190);
            this.txtNome.Name = "txtNome";
            this.txtNome.Padding = new System.Windows.Forms.Padding(7);
            this.txtNome.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtNome.PlaceholderFont = new System.Drawing.Font("Segoe UI", 12.2F);
            this.txtNome.PlaceholderMarginLeft = 12;
            this.txtNome.PlaceholderText = "Digite o nome do livro...";
            this.txtNome.SelectedText = "";
            this.txtNome.SelectionLength = 0;
            this.txtNome.SelectionStart = 0;
            this.txtNome.Size = new System.Drawing.Size(254, 40);
            this.txtNome.TabIndex = 108;
            this.txtNome.TextColor = System.Drawing.Color.Black;
            this.txtNome.UseSystemPasswordChar = false;
            // 
            // mtxCodigoBarras
            // 
            this.mtxCodigoBarras.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mtxCodigoBarras.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mtxCodigoBarras.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.mtxCodigoBarras.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.mtxCodigoBarras.BorderRadius = 10;
            this.mtxCodigoBarras.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mtxCodigoBarras.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtxCodigoBarras.ForeColor = System.Drawing.Color.Gray;
            this.mtxCodigoBarras.HoverBackColor = System.Drawing.Color.LightGray;
            this.mtxCodigoBarras.HoverBorderColor = System.Drawing.Color.DarkGray;
            this.mtxCodigoBarras.LeftMargin = 0;
            this.mtxCodigoBarras.Location = new System.Drawing.Point(441, 190);
            this.mtxCodigoBarras.Mask = "0 000000 000000";
            this.mtxCodigoBarras.MaskTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.mtxCodigoBarras.Name = "mtxCodigoBarras";
            this.mtxCodigoBarras.Padding = new System.Windows.Forms.Padding(10, 2, 7, 6);
            this.mtxCodigoBarras.Size = new System.Drawing.Size(188, 40);
            this.mtxCodigoBarras.TabIndex = 115;
            // 
            // DevoluçãoForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1280, 845);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblDadosLivro);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Name = "DevoluçãoForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Devolução";
            this.Activated += new System.EventHandler(this.DevoluçãoForm_Activated);
            this.Load += new System.EventHandler(this.DevoluçãoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmprestimos)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConfirmarDevolucao;
        private System.Windows.Forms.Button btnProrrogar;
        public System.Windows.Forms.Label label4;
        private RoundedMaskedTextBox mtxCodigoBarras;
        private RoundedTextBox txtNome;
        public System.Windows.Forms.Label lblNome;
        public System.Windows.Forms.Label Titulo;
        private System.Windows.Forms.DataGridView dgvEmprestimos;
        private System.Windows.Forms.Label lblDadosLivro;
        private System.Windows.Forms.Button btnBuscarEmprestimo;
        private System.Windows.Forms.Panel panel1;
        private RoundedComboBox cbFiltroEmprestimo;
        private System.Windows.Forms.Label label6;
    }
}