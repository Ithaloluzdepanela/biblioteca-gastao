namespace BibliotecaApp
{
    partial class LoginForm
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
            this.picExit = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.BtnEntrar = new System.Windows.Forms.Button();
            this.gradientPanel1 = new BibliotecaApp.ElementosPersonalizados.GradientPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bemVindo = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // picExit
            // 
            this.picExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picExit.Image = global::BibliotecaApp.Properties.Resources.icons8_x_20;
            this.picExit.Location = new System.Drawing.Point(377, 3);
            this.picExit.Name = "picExit";
            this.picExit.Size = new System.Drawing.Size(20, 20);
            this.picExit.TabIndex = 2;
            this.picExit.TabStop = false;
            this.picExit.Click += new System.EventHandler(this.picExit_Click);
            this.picExit.MouseEnter += new System.EventHandler(this.picExit_MouseEnter);
            this.picExit.MouseLeave += new System.EventHandler(this.picExit_MouseLeave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(57, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(299, 37);
            this.label2.TabIndex = 10;
            this.label2.Text = "Faça login na sua conta.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(-2, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(3, 35);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(271, 19);
            this.txtEmail.TabIndex = 12;
            this.txtEmail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEmail_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Location = new System.Drawing.Point(65, 220);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 60);
            this.panel1.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(154)))), ((int)(((byte)(245)))));
            this.panel2.Location = new System.Drawing.Point(3, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(280, 2);
            this.panel2.TabIndex = 14;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.txtSenha);
            this.panel3.Location = new System.Drawing.Point(65, 302);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(280, 60);
            this.panel3.TabIndex = 15;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(154)))), ((int)(((byte)(245)))));
            this.panel4.Location = new System.Drawing.Point(3, 56);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(280, 2);
            this.panel4.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(-2, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Senha";
            // 
            // txtSenha
            // 
            this.txtSenha.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSenha.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSenha.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSenha.Location = new System.Drawing.Point(3, 35);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Size = new System.Drawing.Size(271, 19);
            this.txtSenha.TabIndex = 12;
            this.txtSenha.UseSystemPasswordChar = true;
            this.txtSenha.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSenha_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(154)))), ((int)(((byte)(245)))));
            this.label5.Location = new System.Drawing.Point(199, 365);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 15);
            this.label5.TabIndex = 16;
            this.label5.Text = "Esqueceu sua senha?";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.BtnEntrar);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.picExit);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.panel1);
            this.panel5.Controls.Add(this.panel3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(500, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(400, 600);
            this.panel5.TabIndex = 3;
            // 
            // BtnEntrar
            // 
            this.BtnEntrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(74)))), ((int)(((byte)(158)))));
            this.BtnEntrar.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BtnEntrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnEntrar.Font = new System.Drawing.Font("Segoe UI", 16.2F);
            this.BtnEntrar.ForeColor = System.Drawing.Color.White;
            this.BtnEntrar.Location = new System.Drawing.Point(96, 462);
            this.BtnEntrar.Name = "BtnEntrar";
            this.BtnEntrar.Size = new System.Drawing.Size(220, 52);
            this.BtnEntrar.TabIndex = 18;
            this.BtnEntrar.Text = "Entrar";
            this.BtnEntrar.UseVisualStyleBackColor = false;
            this.BtnEntrar.Click += new System.EventHandler(this.BtnEntrar_Click);
            this.BtnEntrar.Enter += new System.EventHandler(this.BtnEntrar_MouseEnter);
            this.BtnEntrar.Leave += new System.EventHandler(this.BtnEntrar_MouseLeave);
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.Controls.Add(this.label6);
            this.gradientPanel1.Controls.Add(this.label1);
            this.gradientPanel1.Controls.Add(this.bemVindo);
            this.gradientPanel1.Controls.Add(this.pictureBox2);
            this.gradientPanel1.Controls.Add(this.pictureBox1);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.gradientPanel1.Gradientebottom = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(145)))), ((int)(((byte)(245)))));
            this.gradientPanel1.Gradientetop = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(74)))), ((int)(((byte)(158)))));
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(500, 600);
            this.gradientPanel1.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(145)))), ((int)(((byte)(245)))));
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(3, 576);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "InDev Version";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(96, 383);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 169);
            this.label1.TabIndex = 2;
            this.label1.Text = "Seja bem-vindo ao app da Biblioteca Monteiro Lobato! Aqui você pode consultar o a" +
    "cervo, registrar empréstimos, devoluções e organizar sua leitura.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bemVindo
            // 
            this.bemVindo.AutoSize = true;
            this.bemVindo.BackColor = System.Drawing.Color.Transparent;
            this.bemVindo.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bemVindo.ForeColor = System.Drawing.Color.White;
            this.bemVindo.Location = new System.Drawing.Point(158, 67);
            this.bemVindo.Name = "bemVindo";
            this.bemVindo.Size = new System.Drawing.Size(147, 37);
            this.bemVindo.TabIndex = 1;
            this.bemVindo.Text = "Bem Vindo";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::BibliotecaApp.Properties.Resources.clouds_upright_2x1;
            this.pictureBox2.Location = new System.Drawing.Point(421, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(98, 600);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::BibliotecaApp.Properties.Resources.Brasão_da_Gastão3;
            this.pictureBox1.Location = new System.Drawing.Point(147, 168);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(176, 189);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.gradientPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LoginForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InicioForm";
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox picExit;
        private ElementosPersonalizados.GradientPanel gradientPanel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label bemVindo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BtnEntrar;
    }
}