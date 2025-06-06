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
            this.label1 = new System.Windows.Forms.Label();
            this.Pic_Cadastrar = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Cadastrar)).BeginInit();
            this.SuspendLayout();
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
            // Pic_Cadastrar
            // 
            this.Pic_Cadastrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(80)))), ((int)(((byte)(115)))));
            this.Pic_Cadastrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Pic_Cadastrar.Image = global::BibliotecaApp.Properties.Resources.icons8_livros_30;
            this.Pic_Cadastrar.Location = new System.Drawing.Point(712, 528);
            this.Pic_Cadastrar.Name = "Pic_Cadastrar";
            this.Pic_Cadastrar.Size = new System.Drawing.Size(40, 40);
            this.Pic_Cadastrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.Pic_Cadastrar.TabIndex = 2;
            this.Pic_Cadastrar.TabStop = false;
            this.Pic_Cadastrar.Click += new System.EventHandler(this.Pic_Cadastrar_Click);
            // 
            // LivrosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.Pic_Cadastrar);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LivrosForm";
            this.Text = "InicioForm";
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Cadastrar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox Pic_Cadastrar;
    }
}