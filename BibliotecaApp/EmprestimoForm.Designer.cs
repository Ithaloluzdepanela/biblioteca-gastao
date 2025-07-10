namespace BibliotecaApp
{
    partial class EmprestimoForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbBibliotecaria = new RoundedComboBox();
            this.lstLivros = new System.Windows.Forms.ListBox();
            this.txtBarcode = new RoundedTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkDevolucaoPersonalizada = new System.Windows.Forms.CheckBox();
            this.lstSugestoesUsuario = new System.Windows.Forms.ListBox();
            this.txtLivro = new RoundedTextBox();
            this.txtNomeUsuario = new RoundedTextBox();
            this.btnEmprestar = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDataEmprestimo = new System.Windows.Forms.DateTimePicker();
            this.dtpDataDevolucao = new System.Windows.Forms.DateTimePicker();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.dtpDataDevolucao);
            this.panel1.Controls.Add(this.dtpDataEmprestimo);
            this.panel1.Controls.Add(this.cbBibliotecaria);
            this.panel1.Controls.Add(this.lstLivros);
            this.panel1.Controls.Add(this.txtBarcode);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.chkDevolucaoPersonalizada);
            this.panel1.Controls.Add(this.lstSugestoesUsuario);
            this.panel1.Controls.Add(this.txtLivro);
            this.panel1.Controls.Add(this.txtNomeUsuario);
            this.panel1.Controls.Add(this.btnEmprestar);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(602, 703);
            this.panel1.TabIndex = 0;
            // 
            // cbBibliotecaria
            // 
            this.cbBibliotecaria.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbBibliotecaria.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbBibliotecaria.BorderRadius = 8;
            this.cbBibliotecaria.BorderThickness = 1;
            this.cbBibliotecaria.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbBibliotecaria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBibliotecaria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBibliotecaria.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbBibliotecaria.FormattingEnabled = true;
            this.cbBibliotecaria.ItemsFont = new System.Drawing.Font("Segoe UI", 10F);
            this.cbBibliotecaria.Location = new System.Drawing.Point(103, 368);
            this.cbBibliotecaria.Name = "cbBibliotecaria";
            this.cbBibliotecaria.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Italic);
            this.cbBibliotecaria.PlaceholderMargin = 10;
            this.cbBibliotecaria.PlaceholderText = "Selecione...";
            this.cbBibliotecaria.Size = new System.Drawing.Size(350, 31);
            this.cbBibliotecaria.TabIndex = 103;
            // 
            // lstLivros
            // 
            this.lstLivros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstLivros.FormattingEnabled = true;
            this.lstLivros.ItemHeight = 17;
            this.lstLivros.Location = new System.Drawing.Point(108, 227);
            this.lstLivros.Margin = new System.Windows.Forms.Padding(4);
            this.lstLivros.Name = "lstLivros";
            this.lstLivros.Size = new System.Drawing.Size(265, 104);
            this.lstLivros.TabIndex = 102;
            this.lstLivros.Visible = false;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtBarcode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtBarcode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtBarcode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtBarcode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtBarcode.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtBarcode.BorderRadius = 10;
            this.txtBarcode.BorderThickness = 1;
            this.txtBarcode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBarcode.Enabled = false;
            this.txtBarcode.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtBarcode.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtBarcode.Location = new System.Drawing.Point(103, 279);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(4);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Padding = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.txtBarcode.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtBarcode.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.PlaceholderMarginLeft = 12;
            this.txtBarcode.PlaceholderText = "Escaneie para buscar informacoes...";
            this.txtBarcode.Size = new System.Drawing.Size(350, 37);
            this.txtBarcode.TabIndex = 101;
            this.txtBarcode.TextColor = System.Drawing.Color.Black;
            this.txtBarcode.UseSystemPasswordChar = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.LightGray;
            this.label7.Location = new System.Drawing.Point(103, 238);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(184, 30);
            this.label7.TabIndex = 100;
            this.label7.Text = "Codigo de barras:";
            // 
            // chkDevolucaoPersonalizada
            // 
            this.chkDevolucaoPersonalizada.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkDevolucaoPersonalizada.AutoSize = true;
            this.chkDevolucaoPersonalizada.BackColor = System.Drawing.Color.White;
            this.chkDevolucaoPersonalizada.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDevolucaoPersonalizada.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.chkDevolucaoPersonalizada.Location = new System.Drawing.Point(103, 582);
            this.chkDevolucaoPersonalizada.Margin = new System.Windows.Forms.Padding(4);
            this.chkDevolucaoPersonalizada.Name = "chkDevolucaoPersonalizada";
            this.chkDevolucaoPersonalizada.Size = new System.Drawing.Size(165, 24);
            this.chkDevolucaoPersonalizada.TabIndex = 99;
            this.chkDevolucaoPersonalizada.Text = "Estender devolução";
            this.chkDevolucaoPersonalizada.UseVisualStyleBackColor = false;
            this.chkDevolucaoPersonalizada.CheckedChanged += new System.EventHandler(this.chkDevolucaoPersonalizada_CheckedChanged);
            // 
            // lstSugestoesUsuario
            // 
            this.lstSugestoesUsuario.FormattingEnabled = true;
            this.lstSugestoesUsuario.ItemHeight = 17;
            this.lstSugestoesUsuario.Location = new System.Drawing.Point(108, 137);
            this.lstSugestoesUsuario.Margin = new System.Windows.Forms.Padding(4);
            this.lstSugestoesUsuario.Name = "lstSugestoesUsuario";
            this.lstSugestoesUsuario.Size = new System.Drawing.Size(266, 106);
            this.lstSugestoesUsuario.TabIndex = 98;
            this.lstSugestoesUsuario.Visible = false;
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
            this.txtLivro.Location = new System.Drawing.Point(103, 190);
            this.txtLivro.Margin = new System.Windows.Forms.Padding(4);
            this.txtLivro.Name = "txtLivro";
            this.txtLivro.Padding = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.txtLivro.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtLivro.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLivro.PlaceholderMarginLeft = 12;
            this.txtLivro.PlaceholderText = "Digite aqui o livro...";
            this.txtLivro.Size = new System.Drawing.Size(350, 37);
            this.txtLivro.TabIndex = 95;
            this.txtLivro.TextColor = System.Drawing.Color.Black;
            this.txtLivro.UseSystemPasswordChar = false;
            // 
            // txtNomeUsuario
            // 
            this.txtNomeUsuario.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtNomeUsuario.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtNomeUsuario.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtNomeUsuario.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtNomeUsuario.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtNomeUsuario.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtNomeUsuario.BorderRadius = 10;
            this.txtNomeUsuario.BorderThickness = 1;
            this.txtNomeUsuario.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNomeUsuario.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtNomeUsuario.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtNomeUsuario.Location = new System.Drawing.Point(103, 101);
            this.txtNomeUsuario.Margin = new System.Windows.Forms.Padding(4);
            this.txtNomeUsuario.Name = "txtNomeUsuario";
            this.txtNomeUsuario.Padding = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.txtNomeUsuario.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtNomeUsuario.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeUsuario.PlaceholderMarginLeft = 12;
            this.txtNomeUsuario.PlaceholderText = "Digite aqui o usuário...";
            this.txtNomeUsuario.Size = new System.Drawing.Size(350, 37);
            this.txtNomeUsuario.TabIndex = 94;
            this.txtNomeUsuario.TextColor = System.Drawing.Color.Black;
            this.txtNomeUsuario.UseSystemPasswordChar = false;
            // 
            // btnEmprestar
            // 
            this.btnEmprestar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnEmprestar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnEmprestar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnEmprestar.ForeColor = System.Drawing.Color.White;
            this.btnEmprestar.Location = new System.Drawing.Point(222, 629);
            this.btnEmprestar.Margin = new System.Windows.Forms.Padding(4);
            this.btnEmprestar.Name = "btnEmprestar";
            this.btnEmprestar.Size = new System.Drawing.Size(155, 70);
            this.btnEmprestar.TabIndex = 91;
            this.btnEmprestar.Text = "EMPRESTRAR";
            this.btnEmprestar.UseVisualStyleBackColor = false;
            this.btnEmprestar.Click += new System.EventHandler(this.btnEmprestar_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.LightGray;
            this.label6.Location = new System.Drawing.Point(103, 327);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(237, 30);
            this.label6.TabIndex = 21;
            this.label6.Text = "Bliotecária responsável:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.LightGray;
            this.label5.Location = new System.Drawing.Point(103, 496);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(200, 30);
            this.label5.TabIndex = 20;
            this.label5.Text = "Devolução Prevista:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.LightGray;
            this.label4.Location = new System.Drawing.Point(103, 410);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(215, 30);
            this.label4.TabIndex = 19;
            this.label4.Text = "Data de Empréstimo:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.LightGray;
            this.label3.Location = new System.Drawing.Point(103, 149);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 30);
            this.label3.TabIndex = 18;
            this.label3.Text = "Livro:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.LightGray;
            this.label2.Location = new System.Drawing.Point(103, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 30);
            this.label2.TabIndex = 17;
            this.label2.Text = "Usuário:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.label1.Location = new System.Drawing.Point(99, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(383, 50);
            this.label1.TabIndex = 16;
            this.label1.Text = "Empréstimo de Livro";
            // 
            // dtpDataEmprestimo
            // 
            this.dtpDataEmprestimo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDataEmprestimo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataEmprestimo.Location = new System.Drawing.Point(103, 451);
            this.dtpDataEmprestimo.Margin = new System.Windows.Forms.Padding(4);
            this.dtpDataEmprestimo.Name = "dtpDataEmprestimo";
            this.dtpDataEmprestimo.Size = new System.Drawing.Size(350, 34);
            this.dtpDataEmprestimo.TabIndex = 104;
            this.dtpDataEmprestimo.Value = new System.DateTime(2025, 7, 10, 0, 0, 0, 0);
            // 
            // dtpDataDevolucao
            // 
            this.dtpDataDevolucao.Enabled = false;
            this.dtpDataDevolucao.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.dtpDataDevolucao.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataDevolucao.Location = new System.Drawing.Point(103, 537);
            this.dtpDataDevolucao.Margin = new System.Windows.Forms.Padding(4);
            this.dtpDataDevolucao.Name = "dtpDataDevolucao";
            this.dtpDataDevolucao.Size = new System.Drawing.Size(350, 34);
            this.dtpDataDevolucao.TabIndex = 105;
            this.dtpDataDevolucao.Value = new System.DateTime(2025, 7, 10, 0, 0, 0, 0);
            // 
            // EmprestimoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(602, 703);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EmprestimoForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EmprestimoForm";
            this.Load += new System.EventHandler(this.EmprestimoForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEmprestar;
        private RoundedTextBox txtLivro;
        private RoundedTextBox txtNomeUsuario;
        private System.Windows.Forms.ListBox lstSugestoesUsuario;
        private System.Windows.Forms.CheckBox chkDevolucaoPersonalizada;
        private RoundedTextBox txtBarcode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox lstLivros;
        private RoundedComboBox cbBibliotecaria;
        private System.Windows.Forms.DateTimePicker dtpDataDevolucao;
        private System.Windows.Forms.DateTimePicker dtpDataEmprestimo;
    }
}