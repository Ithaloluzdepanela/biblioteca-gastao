namespace BibliotecaApp.Utils
{
    partial class ActivationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActivationForm));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.picExit = new System.Windows.Forms.PictureBox();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnActivate = new System.Windows.Forms.Button();
            this.txtInfo = new System.Windows.Forms.RichTextBox();
            this.txtFilePath = new RoundedTextBox();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.panelHeader.Controls.Add(this.picExit);
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(463, 50);
            this.panelHeader.TabIndex = 2;
            // 
            // picExit
            // 
            this.picExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picExit.Image = global::BibliotecaApp.Properties.Resources.icons8_x_20;
            this.picExit.Location = new System.Drawing.Point(440, 3);
            this.picExit.Name = "picExit";
            this.picExit.Size = new System.Drawing.Size(20, 20);
            this.picExit.TabIndex = 3;
            this.picExit.TabStop = false;
            this.picExit.Click += new System.EventHandler(this.picExit_Click);
            // 
            // lblTitulo
            // 
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(0, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(463, 50);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "ATIVAÇAO DE LICENÇA";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnImport.FlatAppearance.BorderSize = 0;
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnImport.ForeColor = System.Drawing.Color.White;
            this.btnImport.Location = new System.Drawing.Point(337, 101);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(103, 35);
            this.btnImport.TabIndex = 66;
            this.btnImport.Text = "IMPORTAR";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 17);
            this.label1.TabIndex = 67;
            this.label1.Text = "Selecionar Arquivo de Licença:";
            // 
            // btnActivate
            // 
            this.btnActivate.BackColor = System.Drawing.Color.DarkGreen;
            this.btnActivate.FlatAppearance.BorderSize = 0;
            this.btnActivate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivate.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnActivate.ForeColor = System.Drawing.Color.White;
            this.btnActivate.Location = new System.Drawing.Point(337, 287);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(103, 35);
            this.btnActivate.TabIndex = 69;
            this.btnActivate.Text = "ATIVAR";
            this.btnActivate.UseVisualStyleBackColor = false;
            this.btnActivate.Click += new System.EventHandler(this.BtnValidate_Click);
            // 
            // txtInfo
            // 
            this.txtInfo.BackColor = System.Drawing.Color.White;
            this.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInfo.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInfo.Location = new System.Drawing.Point(22, 155);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtInfo.Size = new System.Drawing.Size(418, 116);
            this.txtInfo.TabIndex = 93;
            this.txtInfo.Text = "";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtFilePath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtFilePath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtFilePath.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtFilePath.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtFilePath.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtFilePath.BorderRadius = 10;
            this.txtFilePath.BorderThickness = 1;
            this.txtFilePath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFilePath.Enabled = false;
            this.txtFilePath.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtFilePath.HoverBackColor = System.Drawing.Color.White;
            this.txtFilePath.Location = new System.Drawing.Point(22, 101);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Padding = new System.Windows.Forms.Padding(7);
            this.txtFilePath.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtFilePath.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilePath.PlaceholderMarginLeft = 12;
            this.txtFilePath.PlaceholderText = "Clique em importar";
            this.txtFilePath.SelectedText = "";
            this.txtFilePath.SelectionLength = 0;
            this.txtFilePath.SelectionStart = 0;
            this.txtFilePath.Size = new System.Drawing.Size(298, 35);
            this.txtFilePath.TabIndex = 68;
            this.txtFilePath.TextColor = System.Drawing.Color.Black;
            this.txtFilePath.UseSystemPasswordChar = false;
            // 
            // ActivationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 345);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.btnActivate);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ActivationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BibliotecaApp";
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label label1;
        private RoundedTextBox txtFilePath;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.RichTextBox txtInfo;
        private System.Windows.Forms.PictureBox picExit;
    }
}