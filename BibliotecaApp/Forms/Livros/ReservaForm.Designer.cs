namespace BibliotecaApp.Forms.Livros
{
    partial class ReservaForm
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
            this.dtpDataReserva = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReservar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBarcode = new RoundedTextBox();
            this.txtNomeUsuario = new RoundedTextBox();
            this.txtLivro = new RoundedTextBox();
            this.cbBibliotecaria = new RoundedComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpDataReserva
            // 
            this.dtpDataReserva.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpDataReserva.Enabled = false;
            this.dtpDataReserva.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.dtpDataReserva.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataReserva.Location = new System.Drawing.Point(81, 542);
            this.dtpDataReserva.Margin = new System.Windows.Forms.Padding(4);
            this.dtpDataReserva.Name = "dtpDataReserva";
            this.dtpDataReserva.Size = new System.Drawing.Size(376, 33);
            this.dtpDataReserva.TabIndex = 120;
            this.dtpDataReserva.Value = new System.DateTime(2025, 7, 10, 0, 0, 0, 0);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label7.Location = new System.Drawing.Point(76, 355);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(162, 25);
            this.label7.TabIndex = 116;
            this.label7.Text = "Codigo de barras:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label6.Location = new System.Drawing.Point(76, 438);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(213, 25);
            this.label6.TabIndex = 112;
            this.label6.Text = "Bliotecária responsável:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label4.Location = new System.Drawing.Point(77, 513);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 25);
            this.label4.TabIndex = 110;
            this.label4.Text = "Data da Reserva:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label2.Location = new System.Drawing.Point(76, 190);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 25);
            this.label2.TabIndex = 108;
            this.label2.Text = "Nome do Usuario:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 25.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.label1.Location = new System.Drawing.Point(219, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(362, 46);
            this.label1.TabIndex = 124;
            this.label1.Text = "RESERVAR UM LIVRO";
            // 
            // btnReservar
            // 
            this.btnReservar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnReservar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnReservar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnReservar.ForeColor = System.Drawing.Color.White;
            this.btnReservar.Location = new System.Drawing.Point(543, 667);
            this.btnReservar.Margin = new System.Windows.Forms.Padding(4);
            this.btnReservar.Name = "btnReservar";
            this.btnReservar.Size = new System.Drawing.Size(155, 70);
            this.btnReservar.TabIndex = 125;
            this.btnReservar.Text = "RESERVE";
            this.btnReservar.UseVisualStyleBackColor = false;
            this.btnReservar.Click += new System.EventHandler(this.btnReservar_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtBarcode);
            this.panel1.Controls.Add(this.btnReservar);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtNomeUsuario);
            this.panel1.Controls.Add(this.dtpDataReserva);
            this.panel1.Controls.Add(this.txtLivro);
            this.panel1.Controls.Add(this.cbBibliotecaria);
            this.panel1.Controls.Add(this.label7);
            this.panel1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.panel1.Location = new System.Drawing.Point(253, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 845);
            this.panel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label3.Location = new System.Drawing.Point(96, 140);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(384, 25);
            this.label3.TabIndex = 130;
            this.label3.Text = "Confirme as informaçoes antes de reservar:";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(79, 667);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(155, 70);
            this.btnCancelar.TabIndex = 129;
            this.btnCancelar.Text = "CANCELAR";
            this.btnCancelar.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label5.Location = new System.Drawing.Point(76, 271);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 25);
            this.label5.TabIndex = 126;
            this.label5.Text = "Livro:";
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
            this.txtBarcode.Location = new System.Drawing.Point(81, 384);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(4);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Padding = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.txtBarcode.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtBarcode.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.PlaceholderMarginLeft = 12;
            this.txtBarcode.PlaceholderText = "Codigo de barras...";
            this.txtBarcode.SelectionStart = 0;
            this.txtBarcode.Size = new System.Drawing.Size(376, 40);
            this.txtBarcode.TabIndex = 117;
            this.txtBarcode.TextColor = System.Drawing.Color.Black;
            this.txtBarcode.UseSystemPasswordChar = false;
            this.txtBarcode.Leave += new System.EventHandler(this.txtBarcode_Leave);
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
            this.txtNomeUsuario.Enabled = false;
            this.txtNomeUsuario.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtNomeUsuario.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtNomeUsuario.Location = new System.Drawing.Point(81, 219);
            this.txtNomeUsuario.Margin = new System.Windows.Forms.Padding(4);
            this.txtNomeUsuario.Name = "txtNomeUsuario";
            this.txtNomeUsuario.Padding = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.txtNomeUsuario.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtNomeUsuario.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeUsuario.PlaceholderMarginLeft = 12;
            this.txtNomeUsuario.PlaceholderText = "Usuario ...";
            this.txtNomeUsuario.SelectionStart = 0;
            this.txtNomeUsuario.Size = new System.Drawing.Size(617, 40);
            this.txtNomeUsuario.TabIndex = 113;
            this.txtNomeUsuario.TextColor = System.Drawing.Color.Black;
            this.txtNomeUsuario.UseSystemPasswordChar = false;
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
            this.txtLivro.Enabled = false;
            this.txtLivro.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLivro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtLivro.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtLivro.Location = new System.Drawing.Point(81, 300);
            this.txtLivro.Margin = new System.Windows.Forms.Padding(4);
            this.txtLivro.Name = "txtLivro";
            this.txtLivro.Padding = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.txtLivro.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtLivro.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLivro.PlaceholderMarginLeft = 12;
            this.txtLivro.PlaceholderText = "Livro...";
            this.txtLivro.SelectionStart = 0;
            this.txtLivro.Size = new System.Drawing.Size(617, 40);
            this.txtLivro.TabIndex = 114;
            this.txtLivro.TextColor = System.Drawing.Color.Black;
            this.txtLivro.UseSystemPasswordChar = false;
            // 
            // cbBibliotecaria
            // 
            this.cbBibliotecaria.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbBibliotecaria.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbBibliotecaria.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbBibliotecaria.BorderRadius = 8;
            this.cbBibliotecaria.BorderThickness = 1;
            this.cbBibliotecaria.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbBibliotecaria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBibliotecaria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBibliotecaria.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.cbBibliotecaria.FormattingEnabled = true;
            this.cbBibliotecaria.ItemsFont = new System.Drawing.Font("Segoe UI", 14.25F);
            this.cbBibliotecaria.Location = new System.Drawing.Point(81, 466);
            this.cbBibliotecaria.Name = "cbBibliotecaria";
            this.cbBibliotecaria.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14F);
            this.cbBibliotecaria.PlaceholderMargin = 10;
            this.cbBibliotecaria.PlaceholderText = "Selecione a Bliotecária...";
            this.cbBibliotecaria.Size = new System.Drawing.Size(376, 34);
            this.cbBibliotecaria.TabIndex = 119;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(72, 128);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 40);
            this.label8.TabIndex = 131;
            this.label8.Text = "*";
            // 
            // ReservaForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1280, 845);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReservaForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ReservaForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dtpDataReserva;
        private RoundedComboBox cbBibliotecaria;
        private RoundedTextBox txtBarcode;
        private System.Windows.Forms.Label label7;
        private RoundedTextBox txtNomeUsuario;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        public RoundedTextBox txtLivro;
        private System.Windows.Forms.Button btnCancelar;
        public System.Windows.Forms.Button btnReservar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
    }
}