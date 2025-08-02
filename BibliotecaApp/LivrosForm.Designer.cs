namespace BibliotecaApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LivrosForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Pic_Cadastrar = new System.Windows.Forms.PictureBox();
            this.picEmprestimo = new System.Windows.Forms.PictureBox();
            this.btnDevolução = new System.Windows.Forms.Button();
            this.btnProcurar = new System.Windows.Forms.Button();
            this.Lista = new System.Windows.Forms.DataGridView();
            this.btnAlterar = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnBancoDados = new System.Windows.Forms.Button();
            this.btnCriarTablea = new System.Windows.Forms.Button();
            this.lvCampos = new System.Windows.Forms.ListView();
            this.lstTabelas = new System.Windows.Forms.ListBox();
            this.btnCarregarTabelas = new System.Windows.Forms.Button();
            this.txtNome = new RoundedTextBox();
            this.cbDisponibilidade = new RoundedComboBox();
            this.cbFiltro = new RoundedComboBox();
            this.lblTeste = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Cadastrar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEmprestimo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Lista)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pic_Cadastrar
            // 
            this.Pic_Cadastrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(80)))), ((int)(((byte)(115)))));
            this.Pic_Cadastrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Pic_Cadastrar.Image = ((System.Drawing.Image)(resources.GetObject("Pic_Cadastrar.Image")));
            this.Pic_Cadastrar.Location = new System.Drawing.Point(713, 365);
            this.Pic_Cadastrar.Name = "Pic_Cadastrar";
            this.Pic_Cadastrar.Size = new System.Drawing.Size(40, 40);
            this.Pic_Cadastrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.Pic_Cadastrar.TabIndex = 2;
            this.Pic_Cadastrar.TabStop = false;
            this.Pic_Cadastrar.Click += new System.EventHandler(this.Pic_Cadastrar_Click);
            // 
            // picEmprestimo
            // 
            this.picEmprestimo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(80)))), ((int)(((byte)(115)))));
            this.picEmprestimo.BackgroundImage = global::BibliotecaApp.Properties.Resources.icons8_cardápio_30;
            this.picEmprestimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picEmprestimo.Location = new System.Drawing.Point(713, 422);
            this.picEmprestimo.Name = "picEmprestimo";
            this.picEmprestimo.Size = new System.Drawing.Size(40, 40);
            this.picEmprestimo.TabIndex = 15;
            this.picEmprestimo.TabStop = false;
            this.picEmprestimo.Click += new System.EventHandler(this.picEmprestimo_Click);
            // 
            // btnDevolução
            // 
            this.btnDevolução.Location = new System.Drawing.Point(697, 303);
            this.btnDevolução.Name = "btnDevolução";
            this.btnDevolução.Size = new System.Drawing.Size(73, 56);
            this.btnDevolução.TabIndex = 12;
            this.btnDevolução.Text = "Devolução";
            this.btnDevolução.UseVisualStyleBackColor = true;
            this.btnDevolução.Click += new System.EventHandler(this.btnDevolução_Click);
            // 
            // btnProcurar
            // 
            this.btnProcurar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProcurar.Location = new System.Drawing.Point(565, 12);
            this.btnProcurar.Name = "btnProcurar";
            this.btnProcurar.Size = new System.Drawing.Size(108, 45);
            this.btnProcurar.TabIndex = 7;
            this.btnProcurar.Text = "Procurar";
            this.btnProcurar.UseVisualStyleBackColor = true;
            this.btnProcurar.Click += new System.EventHandler(this.btnProcurar_Click);
            // 
            // Lista
            // 
            this.Lista.AllowUserToAddRows = false;
            this.Lista.AllowUserToDeleteRows = false;
            this.Lista.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Lista.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Lista.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Lista.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Lista.Location = new System.Drawing.Point(49, 105);
            this.Lista.Name = "Lista";
            this.Lista.ReadOnly = true;
            this.Lista.RowHeadersWidth = 51;
            this.Lista.RowTemplate.Height = 24;
            this.Lista.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Lista.Size = new System.Drawing.Size(642, 435);
            this.Lista.TabIndex = 3;
            this.Lista.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.Lista_CellFormatting);
            // 
            // btnAlterar
            // 
            this.btnAlterar.Location = new System.Drawing.Point(697, 226);
            this.btnAlterar.Name = "btnAlterar";
            this.btnAlterar.Size = new System.Drawing.Size(73, 56);
            this.btnAlterar.TabIndex = 14;
            this.btnAlterar.Text = "Alterar";
            this.btnAlterar.UseVisualStyleBackColor = true;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(364, 70);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(0, 19);
            this.lblTotal.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Livros";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnBancoDados);
            this.panel1.Controls.Add(this.btnCriarTablea);
            this.panel1.Controls.Add(this.lvCampos);
            this.panel1.Controls.Add(this.lstTabelas);
            this.panel1.Controls.Add(this.btnCarregarTabelas);
            this.panel1.Controls.Add(this.txtNome);
            this.panel1.Controls.Add(this.picEmprestimo);
            this.panel1.Controls.Add(this.Pic_Cadastrar);
            this.panel1.Controls.Add(this.btnAlterar);
            this.panel1.Controls.Add(this.Lista);
            this.panel1.Controls.Add(this.cbDisponibilidade);
            this.panel1.Controls.Add(this.btnProcurar);
            this.panel1.Controls.Add(this.btnDevolução);
            this.panel1.Controls.Add(this.cbFiltro);
            this.panel1.Controls.Add(this.lblTotal);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1300, 663);
            this.panel1.TabIndex = 16;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1096, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 83);
            this.button2.TabIndex = 23;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(903, -5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(170, 100);
            this.button1.TabIndex = 22;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnBancoDados
            // 
            this.btnBancoDados.Location = new System.Drawing.Point(723, 29);
            this.btnBancoDados.Name = "btnBancoDados";
            this.btnBancoDados.Size = new System.Drawing.Size(115, 60);
            this.btnBancoDados.TabIndex = 21;
            this.btnBancoDados.Text = "Criar Banco De Dados";
            this.btnBancoDados.UseVisualStyleBackColor = true;
            this.btnBancoDados.Click += new System.EventHandler(this.btnBancoDados_Click);
            // 
            // btnCriarTablea
            // 
            this.btnCriarTablea.Location = new System.Drawing.Point(0, 0);
            this.btnCriarTablea.Name = "btnCriarTablea";
            this.btnCriarTablea.Size = new System.Drawing.Size(75, 23);
            this.btnCriarTablea.TabIndex = 0;
            this.btnCriarTablea.Click += new System.EventHandler(this.btnCriarTablea_Click);
            // 
            // lvCampos
            // 
            this.lvCampos.HideSelection = false;
            this.lvCampos.Location = new System.Drawing.Point(1062, 101);
            this.lvCampos.Name = "lvCampos";
            this.lvCampos.Size = new System.Drawing.Size(240, 304);
            this.lvCampos.TabIndex = 20;
            this.lvCampos.UseCompatibleStateImageBehavior = false;
            this.lvCampos.View = System.Windows.Forms.View.Details;
            // 
            // lstTabelas
            // 
            this.lstTabelas.FormattingEnabled = true;
            this.lstTabelas.ItemHeight = 15;
            this.lstTabelas.Location = new System.Drawing.Point(819, 101);
            this.lstTabelas.Name = "lstTabelas";
            this.lstTabelas.Size = new System.Drawing.Size(242, 304);
            this.lstTabelas.TabIndex = 19;
            // 
            // btnCarregarTabelas
            // 
            this.btnCarregarTabelas.Location = new System.Drawing.Point(854, 434);
            this.btnCarregarTabelas.Name = "btnCarregarTabelas";
            this.btnCarregarTabelas.Size = new System.Drawing.Size(98, 53);
            this.btnCarregarTabelas.TabIndex = 18;
            this.btnCarregarTabelas.Text = "Verificar Tabelas";
            this.btnCarregarTabelas.UseVisualStyleBackColor = true;
            this.btnCarregarTabelas.Click += new System.EventHandler(this.btnCarregarTabelas_Click);
            // 
            // txtNome
            // 
            this.txtNome.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtNome.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtNome.BackColor = System.Drawing.Color.White;
            this.txtNome.BorderColor = System.Drawing.Color.Black;
            this.txtNome.BorderFocusColor = System.Drawing.Color.Blue;
            this.txtNome.BorderRadius = 10;
            this.txtNome.BorderThickness = 2;
            this.txtNome.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNome.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtNome.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtNome.Location = new System.Drawing.Point(103, 12);
            this.txtNome.Name = "txtNome";
            this.txtNome.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtNome.PlaceholderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.PlaceholderMarginLeft = 10;
            this.txtNome.PlaceholderText = "Procurar Livro";
            this.txtNome.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtNome.Size = new System.Drawing.Size(437, 47);
            this.txtNome.TabIndex = 8;
            this.txtNome.TextColor = System.Drawing.Color.Black;
            this.txtNome.UseSystemPasswordChar = false;
            // 
            // cbDisponibilidade
            // 
            this.cbDisponibilidade.BorderColor = System.Drawing.Color.Black;
            this.cbDisponibilidade.BorderRadius = 8;
            this.cbDisponibilidade.BorderThickness = 2;
            this.cbDisponibilidade.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbDisponibilidade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDisponibilidade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbDisponibilidade.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbDisponibilidade.FormattingEnabled = true;
            this.cbDisponibilidade.Items.AddRange(new object[] {
            "Todos",
            "Disponíveis",
            "Indisponíveis"});
            this.cbDisponibilidade.ItemsFont = new System.Drawing.Font("Segoe UI", 10F);
            this.cbDisponibilidade.Location = new System.Drawing.Point(314, 65);
            this.cbDisponibilidade.Name = "cbDisponibilidade";
            this.cbDisponibilidade.PlaceholderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDisponibilidade.PlaceholderMargin = 10;
            this.cbDisponibilidade.PlaceholderText = "Selecionar Filtro";
            this.cbDisponibilidade.Size = new System.Drawing.Size(190, 26);
            this.cbDisponibilidade.TabIndex = 13;
            // 
            // cbFiltro
            // 
            this.cbFiltro.BorderColor = System.Drawing.Color.Black;
            this.cbFiltro.BorderRadius = 8;
            this.cbFiltro.BorderThickness = 2;
            this.cbFiltro.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFiltro.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbFiltro.FormattingEnabled = true;
            this.cbFiltro.Items.AddRange(new object[] {
            "Nome",
            "Autor",
            "Gênero"});
            this.cbFiltro.ItemsFont = new System.Drawing.Font("Segoe UI", 10F);
            this.cbFiltro.Location = new System.Drawing.Point(103, 65);
            this.cbFiltro.Name = "cbFiltro";
            this.cbFiltro.PlaceholderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbFiltro.PlaceholderMargin = 10;
            this.cbFiltro.PlaceholderText = "Selecionar Filtro";
            this.cbFiltro.Size = new System.Drawing.Size(190, 26);
            this.cbFiltro.TabIndex = 9;
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
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTeste);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LivrosForm";
            this.Text = " ";
            this.Load += new System.EventHandler(this.LivrosForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Cadastrar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEmprestimo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Lista)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Pic_Cadastrar;
        private System.Windows.Forms.PictureBox picEmprestimo;
        private System.Windows.Forms.Button btnDevolução;
        private System.Windows.Forms.Button btnProcurar;
        private System.Windows.Forms.DataGridView Lista;
        private RoundedComboBox cbFiltro;
        private System.Windows.Forms.Button btnAlterar;
        private System.Windows.Forms.Label lblTotal;
        private RoundedComboBox cbDisponibilidade;
        private RoundedTextBox txtNome;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTeste;
        private System.Windows.Forms.ListBox lstTabelas;
        private System.Windows.Forms.Button btnCarregarTabelas;
        private System.Windows.Forms.ListView lvCampos;
        private System.Windows.Forms.Button btnCriarTablea;
        private System.Windows.Forms.Button btnBancoDados;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}