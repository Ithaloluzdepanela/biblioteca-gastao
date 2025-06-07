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
            this.dtpDataDevolucao = new System.Windows.Forms.DateTimePicker();
            this.dtpDataEmprestimo = new System.Windows.Forms.DateTimePicker();
            this.btnEmprestar = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstSugestoesUsuario = new System.Windows.Forms.ListBox();
            this.txtLivro = new RoundedTextBox();
            this.txtNomeUsuario = new RoundedTextBox();
            this.cbBibliotecaria = new RoundedComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lstSugestoesUsuario);
            this.panel1.Controls.Add(this.dtpDataDevolucao);
            this.panel1.Controls.Add(this.dtpDataEmprestimo);
            this.panel1.Controls.Add(this.txtLivro);
            this.panel1.Controls.Add(this.txtNomeUsuario);
            this.panel1.Controls.Add(this.cbBibliotecaria);
            this.panel1.Controls.Add(this.btnEmprestar);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(672, 615);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // dtpDataDevolucao
            // 
            this.dtpDataDevolucao.Location = new System.Drawing.Point(78, 484);
            this.dtpDataDevolucao.Name = "dtpDataDevolucao";
            this.dtpDataDevolucao.Size = new System.Drawing.Size(200, 20);
            this.dtpDataDevolucao.TabIndex = 97;
            // 
            // dtpDataEmprestimo
            // 
            this.dtpDataEmprestimo.Location = new System.Drawing.Point(78, 403);
            this.dtpDataEmprestimo.Name = "dtpDataEmprestimo";
            this.dtpDataEmprestimo.Size = new System.Drawing.Size(200, 20);
            this.dtpDataEmprestimo.TabIndex = 96;
            // 
            // btnEmprestar
            // 
            this.btnEmprestar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnEmprestar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnEmprestar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnEmprestar.ForeColor = System.Drawing.Color.White;
            this.btnEmprestar.Location = new System.Drawing.Point(438, 535);
            this.btnEmprestar.Name = "btnEmprestar";
            this.btnEmprestar.Size = new System.Drawing.Size(149, 57);
            this.btnEmprestar.TabIndex = 91;
            this.btnEmprestar.Text = "EMPRESTRAR";
            this.btnEmprestar.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label6.Location = new System.Drawing.Point(73, 274);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(213, 25);
            this.label6.TabIndex = 21;
            this.label6.Text = "Bliotecária responsável:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label5.Location = new System.Drawing.Point(73, 441);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(181, 25);
            this.label5.TabIndex = 20;
            this.label5.Text = "Devolução Prevista:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label4.Location = new System.Drawing.Point(73, 358);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(192, 25);
            this.label4.TabIndex = 19;
            this.label4.Text = "Data de Empréstimo:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label3.Location = new System.Drawing.Point(73, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 25);
            this.label3.TabIndex = 18;
            this.label3.Text = "Livro:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label2.Location = new System.Drawing.Point(73, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 25);
            this.label2.TabIndex = 17;
            this.label2.Text = "Usuário:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.label1.Location = new System.Drawing.Point(192, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 40);
            this.label1.TabIndex = 16;
            this.label1.Text = "Empréstimo de Livro";
            // 
            // lstSugestoesUsuario
            // 
            this.lstSugestoesUsuario.FormattingEnabled = true;
            this.lstSugestoesUsuario.Location = new System.Drawing.Point(359, 173);
            this.lstSugestoesUsuario.Name = "lstSugestoesUsuario";
            this.lstSugestoesUsuario.Size = new System.Drawing.Size(228, 82);
            this.lstSugestoesUsuario.TabIndex = 98;
            this.lstSugestoesUsuario.Visible = false;
            this.lstSugestoesUsuario.SelectedIndexChanged += new System.EventHandler(this.lstSugestoesUsuario_SelectedIndexChanged);
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
            this.txtLivro.Location = new System.Drawing.Point(78, 215);
            this.txtLivro.Name = "txtLivro";
            this.txtLivro.Padding = new System.Windows.Forms.Padding(7);
            this.txtLivro.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtLivro.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLivro.PlaceholderMarginLeft = 12;
            this.txtLivro.PlaceholderText = "Digite aqui o livro...";
            this.txtLivro.Size = new System.Drawing.Size(509, 40);
            this.txtLivro.TabIndex = 95;
            this.txtLivro.TextColor = System.Drawing.Color.Black;
            this.txtLivro.UseSystemPasswordChar = false;
            this.txtLivro.Load += new System.EventHandler(this.roundedTextBox1_Load);
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
            this.txtNomeUsuario.Location = new System.Drawing.Point(78, 137);
            this.txtNomeUsuario.Name = "txtNomeUsuario";
            this.txtNomeUsuario.Padding = new System.Windows.Forms.Padding(7);
            this.txtNomeUsuario.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtNomeUsuario.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeUsuario.PlaceholderMarginLeft = 12;
            this.txtNomeUsuario.PlaceholderText = "Digite aqui o usuário...";
            this.txtNomeUsuario.Size = new System.Drawing.Size(509, 40);
            this.txtNomeUsuario.TabIndex = 94;
            this.txtNomeUsuario.TextColor = System.Drawing.Color.Black;
            this.txtNomeUsuario.UseSystemPasswordChar = false;
            this.txtNomeUsuario.Load += new System.EventHandler(this.txtNome_Load);
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
            this.cbBibliotecaria.Items.AddRange(new object[] {
            "Aluno(a)",
            "Bibliotecário(a)",
            "Professor(a)",
            "Outros"});
            this.cbBibliotecaria.ItemsFont = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBibliotecaria.Location = new System.Drawing.Point(78, 299);
            this.cbBibliotecaria.Name = "cbBibliotecaria";
            this.cbBibliotecaria.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBibliotecaria.PlaceholderMargin = 10;
            this.cbBibliotecaria.PlaceholderText = "Selecione o Bliotecária responsável...";
            this.cbBibliotecaria.Size = new System.Drawing.Size(367, 34);
            this.cbBibliotecaria.TabIndex = 93;
            this.cbBibliotecaria.SelectedIndexChanged += new System.EventHandler(this.biblio_SelectedIndexChanged);
            // 
            // EmprestimoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 615);
            this.Controls.Add(this.panel1);
            this.Name = "EmprestimoForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EmprestimoForm";
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
        private RoundedComboBox cbBibliotecaria;
        private RoundedTextBox txtLivro;
        private RoundedTextBox txtNomeUsuario;
        private System.Windows.Forms.DateTimePicker dtpDataDevolucao;
        private System.Windows.Forms.DateTimePicker dtpDataEmprestimo;
        private System.Windows.Forms.ListBox lstSugestoesUsuario;
    }
}