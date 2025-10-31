namespace BibliotecaApp.Forms.Livros
{
    partial class LivrosForm
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
            this.lblTotal = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.mtxCodigoBarras = new RoundedMaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNome = new RoundedTextBox();
            this.btnProcurar = new System.Windows.Forms.Button();
            this.cbFiltro = new RoundedComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDisponibilidade = new RoundedComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Titulo = new System.Windows.Forms.Label();
            this.dgvLivros = new System.Windows.Forms.DataGridView();
            this.lblTeste = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLivros)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(761, 171);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(0, 19);
            this.lblTotal.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.mtxCodigoBarras);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtNome);
            this.panel1.Controls.Add(this.btnProcurar);
            this.panel1.Controls.Add(this.cbFiltro);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbDisponibilidade);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.Titulo);
            this.panel1.Controls.Add(this.dgvLivros);
            this.panel1.Controls.Add(this.lblTotal);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 845);
            this.panel1.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label3.Location = new System.Drawing.Point(744, 209);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 25);
            this.label3.TabIndex = 134;
            this.label3.Text = "Código de Barras:";
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
            this.mtxCodigoBarras.Location = new System.Drawing.Point(748, 238);
            this.mtxCodigoBarras.Margin = new System.Windows.Forms.Padding(4);
            this.mtxCodigoBarras.Mask = "0 000000 000000";
            this.mtxCodigoBarras.MaskTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.mtxCodigoBarras.Name = "mtxCodigoBarras";
            this.mtxCodigoBarras.Padding = new System.Windows.Forms.Padding(14, 10, 8, 14);
            this.mtxCodigoBarras.Size = new System.Drawing.Size(202, 40);
            this.mtxCodigoBarras.TabIndex = 133;
            this.mtxCodigoBarras.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxCodigoBarras_KeyDown);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label2.Location = new System.Drawing.Point(138, 136);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 25);
            this.label2.TabIndex = 132;
            this.label2.Text = "Pesquisa:";
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
            this.txtNome.Location = new System.Drawing.Point(143, 164);
            this.txtNome.Name = "txtNome";
            this.txtNome.Padding = new System.Windows.Forms.Padding(7);
            this.txtNome.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtNome.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F);
            this.txtNome.PlaceholderMarginLeft = 12;
            this.txtNome.PlaceholderText = "Digite para filtrar...";
            this.txtNome.SelectedText = "";
            this.txtNome.SelectionLength = 0;
            this.txtNome.SelectionStart = 0;
            this.txtNome.Size = new System.Drawing.Size(578, 40);
            this.txtNome.TabIndex = 131;
            this.txtNome.TextColor = System.Drawing.Color.Black;
            this.txtNome.UseSystemPasswordChar = false;
            // 
            // btnProcurar
            // 
            this.btnProcurar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnProcurar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnProcurar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProcurar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnProcurar.ForeColor = System.Drawing.Color.White;
            this.btnProcurar.Location = new System.Drawing.Point(993, 229);
            this.btnProcurar.Name = "btnProcurar";
            this.btnProcurar.Size = new System.Drawing.Size(150, 60);
            this.btnProcurar.TabIndex = 130;
            this.btnProcurar.Text = "Procurar";
            this.btnProcurar.UseVisualStyleBackColor = false;
            this.btnProcurar.Click += new System.EventHandler(this.btnProcurar_Click);
            // 
            // cbFiltro
            // 
            this.cbFiltro.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbFiltro.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbFiltro.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbFiltro.BorderRadius = 8;
            this.cbFiltro.BorderThickness = 1;
            this.cbFiltro.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbFiltro.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFiltro.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.cbFiltro.FormattingEnabled = true;
            this.cbFiltro.Items.AddRange(new object[] {
            "Nome",
            "Autor",
            "Gênero"});
            this.cbFiltro.ItemsFont = new System.Drawing.Font("Segoe UI", 14.25F);
            this.cbFiltro.Location = new System.Drawing.Point(144, 244);
            this.cbFiltro.Name = "cbFiltro";
            this.cbFiltro.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14F);
            this.cbFiltro.PlaceholderMargin = 10;
            this.cbFiltro.PlaceholderText = "Escolha um tipo de busca...";
            this.cbFiltro.Size = new System.Drawing.Size(275, 34);
            this.cbFiltro.TabIndex = 129;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label1.Location = new System.Drawing.Point(139, 218);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 25);
            this.label1.TabIndex = 128;
            this.label1.Text = "Busque por:";
            // 
            // cbDisponibilidade
            // 
            this.cbDisponibilidade.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbDisponibilidade.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbDisponibilidade.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbDisponibilidade.BorderRadius = 8;
            this.cbDisponibilidade.BorderThickness = 1;
            this.cbDisponibilidade.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbDisponibilidade.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbDisponibilidade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDisponibilidade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbDisponibilidade.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.cbDisponibilidade.FormattingEnabled = true;
            this.cbDisponibilidade.Items.AddRange(new object[] {
            "Todos",
            "Disponíveis",
            "Indisponíveis"});
            this.cbDisponibilidade.ItemsFont = new System.Drawing.Font("Segoe UI", 14.25F);
            this.cbDisponibilidade.Location = new System.Drawing.Point(449, 244);
            this.cbDisponibilidade.Name = "cbDisponibilidade";
            this.cbDisponibilidade.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14F);
            this.cbDisponibilidade.PlaceholderMargin = 10;
            this.cbDisponibilidade.PlaceholderText = "Filtre por disponibilidade...";
            this.cbDisponibilidade.Size = new System.Drawing.Size(272, 34);
            this.cbDisponibilidade.TabIndex = 127;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label6.Location = new System.Drawing.Point(444, 218);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 25);
            this.label6.TabIndex = 126;
            this.label6.Text = "Disponibilidade:";
            // 
            // Titulo
            // 
            this.Titulo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Titulo.Font = new System.Drawing.Font("Segoe UI", 25.25F, System.Drawing.FontStyle.Bold);
            this.Titulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.Titulo.Location = new System.Drawing.Point(571, 47);
            this.Titulo.Name = "Titulo";
            this.Titulo.Size = new System.Drawing.Size(138, 46);
            this.Titulo.TabIndex = 107;
            this.Titulo.Text = "LIVROS";
            // 
            // dgvLivros
            // 
            this.dgvLivros.AllowUserToAddRows = false;
            this.dgvLivros.AllowUserToDeleteRows = false;
            this.dgvLivros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dgvLivros.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLivros.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLivros.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLivros.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLivros.Location = new System.Drawing.Point(95, 323);
            this.dgvLivros.Name = "dgvLivros";
            this.dgvLivros.ReadOnly = true;
            this.dgvLivros.RowHeadersWidth = 51;
            this.dgvLivros.RowTemplate.Height = 24;
            this.dgvLivros.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLivros.Size = new System.Drawing.Size(1090, 450);
            this.dgvLivros.TabIndex = 3;
            this.dgvLivros.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLivros_CellContentClick);
            this.dgvLivros.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.Lista_CellFormatting);
            this.dgvLivros.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvLivros_CellPainting);
            // 
            // lblTeste
            // 
            this.lblTeste.AutoSize = true;
            this.lblTeste.Location = new System.Drawing.Point(129, 9);
            this.lblTeste.Name = "lblTeste";
            this.lblTeste.Size = new System.Drawing.Size(0, 15);
            this.lblTeste.TabIndex = 17;
            // 
            // LivrosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 845);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblTeste);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LivrosForm";
            this.Load += new System.EventHandler(this.LivrosForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLivros)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTeste;
        private System.Windows.Forms.DataGridView dgvLivros;
        public System.Windows.Forms.Label Titulo;
        private RoundedComboBox cbDisponibilidade;
        private System.Windows.Forms.Label label6;
        private RoundedComboBox cbFiltro;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnProcurar;
        private RoundedTextBox txtNome;
        private System.Windows.Forms.Label label2;
        private RoundedMaskedTextBox mtxCodigoBarras;
        private System.Windows.Forms.Label label3;
    }
}