namespace BibliotecaApp.Forms.Relatorio
{
    partial class RelForm
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
            this.lblRelatorios = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();
            this.listaHistorico = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.listaHistorico)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRelatorios
            // 
            this.lblRelatorios.AutoSize = true;
            this.lblRelatorios.BackColor = System.Drawing.SystemColors.Control;
            this.lblRelatorios.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRelatorios.Font = new System.Drawing.Font("Segoe UI Semibold", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRelatorios.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblRelatorios.Location = new System.Drawing.Point(0, 0);
            this.lblRelatorios.Name = "lblRelatorios";
            this.lblRelatorios.Size = new System.Drawing.Size(191, 45);
            this.lblRelatorios.TabIndex = 0;
            this.lblRelatorios.Text = "HISTÓRICO";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(115, 770);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 31);
            this.button1.TabIndex = 16;
            this.button1.Text = "Pasta";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Location = new System.Drawing.Point(12, 770);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(50, 20);
            this.lblResultado.TabIndex = 22;
            this.lblResultado.Text = "label1";
            // 
            // listaHistorico
            // 
            this.listaHistorico.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.listaHistorico.Location = new System.Drawing.Point(12, 68);
            this.listaHistorico.Name = "listaHistorico";
            this.listaHistorico.RowHeadersWidth = 51;
            this.listaHistorico.RowTemplate.Height = 24;
            this.listaHistorico.Size = new System.Drawing.Size(1167, 680);
            this.listaHistorico.TabIndex = 23;
            // 
            // RelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 811);
            this.Controls.Add(this.listaHistorico);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.lblRelatorios);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "RelForm";
            this.Text = "InicioForm";
            this.Load += new System.EventHandler(this.RelForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.listaHistorico)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRelatorios;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.DataGridView listaHistorico;
    }
}