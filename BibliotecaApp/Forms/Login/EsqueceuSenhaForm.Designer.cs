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
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblReenviar = new System.Windows.Forms.Label();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.txtTeste = new System.Windows.Forms.TextBox();
            this.btnTeste = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(245, 68);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(325, 22);
            this.txtEmail.TabIndex = 0;
            // 
            // lblReenviar
            // 
            this.lblReenviar.AutoSize = true;
            this.lblReenviar.Location = new System.Drawing.Point(365, 425);
            this.lblReenviar.Name = "lblReenviar";
            this.lblReenviar.Size = new System.Drawing.Size(62, 16);
            this.lblReenviar.TabIndex = 1;
            this.lblReenviar.Text = "Reenviar";
            this.lblReenviar.Click += new System.EventHandler(this.lblReenviar_Click);
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(316, 174);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(175, 55);
            this.btnEnviar.TabIndex = 2;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // txtTeste
            // 
            this.txtTeste.Location = new System.Drawing.Point(230, 292);
            this.txtTeste.Name = "txtTeste";
            this.txtTeste.Size = new System.Drawing.Size(362, 22);
            this.txtTeste.TabIndex = 3;
            // 
            // btnTeste
            // 
            this.btnTeste.Location = new System.Drawing.Point(342, 349);
            this.btnTeste.Name = "btnTeste";
            this.btnTeste.Size = new System.Drawing.Size(149, 29);
            this.btnTeste.TabIndex = 4;
            this.btnTeste.Text = "Testar";
            this.btnTeste.UseVisualStyleBackColor = true;
            this.btnTeste.Click += new System.EventHandler(this.btnTestar_Click);
            // 
            // EsqueciSenha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnTeste);
            this.Controls.Add(this.txtTeste);
            this.Controls.Add(this.btnEnviar);
            this.Controls.Add(this.lblReenviar);
            this.Controls.Add(this.txtEmail);
            this.Name = "EsqueciSenha";
            this.Text = "EsqueceuSenhaForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblReenviar;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.TextBox txtTeste;
        private System.Windows.Forms.Button btnTeste;
    }
}