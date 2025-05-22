namespace BibliotecaGastao
{
    partial class loginForm
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
            this.nightControlBox2 = new ReaLTaiizor.Controls.NightControlBox();
            this.label = new ReaLTaiizor.Controls.BigLabel();
            this.Txt_Usuario = new System.Windows.Forms.TextBox();
            this.Txt_Senha = new System.Windows.Forms.TextBox();
            this.Btn_Entrar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // nightControlBox2
            // 
            this.nightControlBox2.BackColor = System.Drawing.Color.Transparent;
            this.nightControlBox2.CloseHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.nightControlBox2.CloseHoverForeColor = System.Drawing.Color.White;
            this.nightControlBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nightControlBox2.DefaultLocation = true;
            this.nightControlBox2.DisableMaximizeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.nightControlBox2.DisableMinimizeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.nightControlBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.nightControlBox2.EnableCloseColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.nightControlBox2.EnableMaximizeButton = true;
            this.nightControlBox2.EnableMaximizeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.nightControlBox2.EnableMinimizeButton = true;
            this.nightControlBox2.EnableMinimizeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.nightControlBox2.Location = new System.Drawing.Point(875, 0);
            this.nightControlBox2.MaximizeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.nightControlBox2.MaximizeHoverForeColor = System.Drawing.Color.White;
            this.nightControlBox2.MinimizeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.nightControlBox2.MinimizeHoverForeColor = System.Drawing.Color.White;
            this.nightControlBox2.Name = "nightControlBox2";
            this.nightControlBox2.Size = new System.Drawing.Size(139, 31);
            this.nightControlBox2.TabIndex = 16;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.BackColor = System.Drawing.Color.Transparent;
            this.label.Font = new System.Drawing.Font("Segoe UI", 25F);
            this.label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label.Location = new System.Drawing.Point(158, 63);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(129, 57);
            this.label.TabIndex = 17;
            this.label.Text = "Login";
            // 
            // Txt_Usuario
            // 
            this.Txt_Usuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Txt_Usuario.Location = new System.Drawing.Point(117, 206);
            this.Txt_Usuario.Multiline = true;
            this.Txt_Usuario.Name = "Txt_Usuario";
            this.Txt_Usuario.Size = new System.Drawing.Size(230, 30);
            this.Txt_Usuario.TabIndex = 19;
            this.Txt_Usuario.UseSystemPasswordChar = true;
            // 
            // Txt_Senha
            // 
            this.Txt_Senha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Txt_Senha.Location = new System.Drawing.Point(117, 302);
            this.Txt_Senha.Multiline = true;
            this.Txt_Senha.Name = "Txt_Senha";
            this.Txt_Senha.PasswordChar = '*';
            this.Txt_Senha.Size = new System.Drawing.Size(230, 30);
            this.Txt_Senha.TabIndex = 21;
            this.Txt_Senha.Leave += new System.EventHandler(this.Txt_Senha_Leave);
            // 
            // Btn_Entrar
            // 
            this.Btn_Entrar.Location = new System.Drawing.Point(132, 433);
            this.Btn_Entrar.Name = "Btn_Entrar";
            this.Btn_Entrar.Size = new System.Drawing.Size(178, 80);
            this.Btn_Entrar.TabIndex = 22;
            this.Btn_Entrar.Text = "Entrar";
            this.Btn_Entrar.UseVisualStyleBackColor = true;
            this.Btn_Entrar.Click += new System.EventHandler(this.Btn_Entrar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(114, 187);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 16);
            this.label1.TabIndex = 23;
            this.label1.Text = "Email";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 280);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 16);
            this.label2.TabIndex = 24;
            this.label2.Text = "Senha";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(117, 335);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 16);
            this.label3.TabIndex = 25;
            this.label3.Text = "Esqueci a senha";
            // 
            // loginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 647);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Btn_Entrar);
            this.Controls.Add(this.Txt_Senha);
            this.Controls.Add(this.Txt_Usuario);
            this.Controls.Add(this.label);
            this.Controls.Add(this.nightControlBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "loginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ReaLTaiizor.Controls.NightControlBox nightControlBox2;
        private ReaLTaiizor.Controls.BigLabel label;
        private System.Windows.Forms.TextBox Txt_Usuario;
        private System.Windows.Forms.TextBox Txt_Senha;
        private System.Windows.Forms.Button Btn_Entrar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}