namespace BibliotecaApp.Forms.Login
{
    partial class EsqueceuSenhaForm
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
            this.lblReenviar = new System.Windows.Forms.Label();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.txtTeste = new System.Windows.Forms.TextBox();
            this.btnTeste = new System.Windows.Forms.Button();
            this.pnBarra = new System.Windows.Forms.Panel();
            this.lblDigite = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblTop = new System.Windows.Forms.Label();
            this.picExit = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.pnBarra2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblReenviar
            // 
            this.lblReenviar.AutoSize = true;
            this.lblReenviar.Location = new System.Drawing.Point(288, 436);
            this.lblReenviar.Name = "lblReenviar";
            this.lblReenviar.Size = new System.Drawing.Size(62, 16);
            this.lblReenviar.TabIndex = 1;
            this.lblReenviar.Text = "Reenviar";
            this.lblReenviar.Visible = false;
            this.lblReenviar.Click += new System.EventHandler(this.lblReenviar_Click);
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(245, 278);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(175, 55);
            this.btnEnviar.TabIndex = 2;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // txtTeste
            // 
            this.txtTeste.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTeste.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.txtTeste.Location = new System.Drawing.Point(206, 221);
            this.txtTeste.Name = "txtTeste";
            this.txtTeste.Size = new System.Drawing.Size(277, 19);
            this.txtTeste.TabIndex = 3;
            // 
            // btnTeste
            // 
            this.btnTeste.Location = new System.Drawing.Point(0, 430);
            this.btnTeste.Name = "btnTeste";
            this.btnTeste.Size = new System.Drawing.Size(149, 29);
            this.btnTeste.TabIndex = 4;
            this.btnTeste.Text = "Testar";
            this.btnTeste.UseVisualStyleBackColor = true;
            this.btnTeste.Click += new System.EventHandler(this.btnTestar_Click);
            // 
            // pnBarra
            // 
            this.pnBarra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(154)))), ((int)(((byte)(245)))));
            this.pnBarra.Location = new System.Drawing.Point(205, 178);
            this.pnBarra.Name = "pnBarra";
            this.pnBarra.Size = new System.Drawing.Size(280, 2);
            this.pnBarra.TabIndex = 14;
            // 
            // lblDigite
            // 
            this.lblDigite.AutoSize = true;
            this.lblDigite.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigite.Location = new System.Drawing.Point(206, 131);
            this.lblDigite.Name = "lblDigite";
            this.lblDigite.Size = new System.Drawing.Size(93, 13);
            this.lblDigite.TabIndex = 11;
            this.lblDigite.Text = "Digite Seu Email ";
            // 
            // txtEmail
            // 
            this.txtEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail.BackColor = System.Drawing.Color.White;
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(206, 157);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(277, 19);
            this.txtEmail.TabIndex = 12;
            // 
            // lblTop
            // 
            this.lblTop.AutoEllipsis = true;
            this.lblTop.AutoSize = true;
            this.lblTop.BackColor = System.Drawing.Color.Transparent;
            this.lblTop.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTop.Location = new System.Drawing.Point(202, 1);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(280, 47);
            this.lblTop.TabIndex = 15;
            this.lblTop.Text = "Recuperar Senha";
            // 
            // picExit
            // 
            this.picExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picExit.Image = global::BibliotecaApp.Properties.Resources.icons8_x_20;
            this.picExit.Location = new System.Drawing.Point(659, 3);
            this.picExit.Name = "picExit";
            this.picExit.Size = new System.Drawing.Size(20, 20);
            this.picExit.TabIndex = 16;
            this.picExit.TabStop = false;
            this.picExit.Click += new System.EventHandler(this.picExit_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblTop);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(684, 59);
            this.panel2.TabIndex = 17;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(512, 378);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 29);
            this.button1.TabIndex = 18;
            this.button1.Text = "Testar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigo.Location = new System.Drawing.Point(207, 202);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(133, 13);
            this.lblCodigo.TabIndex = 19;
            this.lblCodigo.Text = "Digite o Codigo Enviado";
            // 
            // pnBarra2
            // 
            this.pnBarra2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(154)))), ((int)(((byte)(245)))));
            this.pnBarra2.Location = new System.Drawing.Point(205, 242);
            this.pnBarra2.Name = "pnBarra2";
            this.pnBarra2.Size = new System.Drawing.Size(280, 2);
            this.pnBarra2.TabIndex = 15;
            // 
            // EsqueceuSenhaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.pnBarra2);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.pnBarra);
            this.Controls.Add(this.lblDigite);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.txtTeste);
            this.Controls.Add(this.picExit);
            this.Controls.Add(this.btnTeste);
            this.Controls.Add(this.btnEnviar);
            this.Controls.Add(this.lblReenviar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EsqueceuSenhaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EsqueceuSenhaForm";
            this.Load += new System.EventHandler(this.EsqueceuSenhaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblReenviar;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.TextBox txtTeste;
        private System.Windows.Forms.Button btnTeste;
        private System.Windows.Forms.Panel pnBarra;
        private System.Windows.Forms.Label lblDigite;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.PictureBox picExit;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.Panel pnBarra2;
    }
}