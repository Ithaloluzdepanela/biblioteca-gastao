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
            this.lista = new System.Windows.Forms.DataGridView();
            this.IdEmpréstimo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdLivro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdUsuário = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataDevolucao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataEmprestimo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtIdLivro = new System.Windows.Forms.TextBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.lblIdLivro = new System.Windows.Forms.Label();
            this.lblIdUsuario = new System.Windows.Forms.Label();
            this.txtIdUsuario = new System.Windows.Forms.TextBox();
            this.lblResultado = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lista)).BeginInit();
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
            this.lblRelatorios.Size = new System.Drawing.Size(211, 45);
            this.lblRelatorios.TabIndex = 0;
            this.lblRelatorios.Text = "RELATÓRIOS";
            // 
            // lista
            // 
            this.lista.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.lista.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lista.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdEmpréstimo,
            this.IdLivro,
            this.IdUsuário,
            this.DataDevolucao,
            this.DataEmprestimo,
            this.Status});
            this.lista.Location = new System.Drawing.Point(12, 62);
            this.lista.Name = "lista";
            this.lista.RowHeadersWidth = 51;
            this.lista.RowTemplate.Height = 24;
            this.lista.Size = new System.Drawing.Size(973, 606);
            this.lista.TabIndex = 1;
            // 
            // IdEmpréstimo
            // 
            this.IdEmpréstimo.HeaderText = "Id Empréstimo";
            this.IdEmpréstimo.MinimumWidth = 6;
            this.IdEmpréstimo.Name = "IdEmpréstimo";
            // 
            // IdLivro
            // 
            this.IdLivro.HeaderText = "IdLivro";
            this.IdLivro.MinimumWidth = 6;
            this.IdLivro.Name = "IdLivro";
            // 
            // IdUsuário
            // 
            this.IdUsuário.HeaderText = "IdUsuário";
            this.IdUsuário.MinimumWidth = 6;
            this.IdUsuário.Name = "IdUsuário";
            // 
            // DataDevolucao
            // 
            this.DataDevolucao.HeaderText = "Data de Devolução";
            this.DataDevolucao.MinimumWidth = 6;
            this.DataDevolucao.Name = "DataDevolucao";
            // 
            // DataEmprestimo
            // 
            this.DataEmprestimo.HeaderText = "Data do Empréstimo";
            this.DataEmprestimo.MinimumWidth = 6;
            this.DataEmprestimo.Name = "DataEmprestimo";
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            // 
            // txtIdLivro
            // 
            this.txtIdLivro.Location = new System.Drawing.Point(1002, 188);
            this.txtIdLivro.Name = "txtIdLivro";
            this.txtIdLivro.Size = new System.Drawing.Size(196, 27);
            this.txtIdLivro.TabIndex = 2;
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Location = new System.Drawing.Point(1088, 62);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(84, 31);
            this.btnFiltrar.TabIndex = 3;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // lblIdLivro
            // 
            this.lblIdLivro.AutoSize = true;
            this.lblIdLivro.Location = new System.Drawing.Point(998, 153);
            this.lblIdLivro.Name = "lblIdLivro";
            this.lblIdLivro.Size = new System.Drawing.Size(54, 20);
            this.lblIdLivro.TabIndex = 4;
            this.lblIdLivro.Text = "IdLivro";
            // 
            // lblIdUsuario
            // 
            this.lblIdUsuario.AutoSize = true;
            this.lblIdUsuario.Location = new System.Drawing.Point(998, 237);
            this.lblIdUsuario.Name = "lblIdUsuario";
            this.lblIdUsuario.Size = new System.Drawing.Size(72, 20);
            this.lblIdUsuario.TabIndex = 6;
            this.lblIdUsuario.Text = "IdUsuario";
            // 
            // txtIdUsuario
            // 
            this.txtIdUsuario.Location = new System.Drawing.Point(1002, 272);
            this.txtIdUsuario.Name = "txtIdUsuario";
            this.txtIdUsuario.Size = new System.Drawing.Size(196, 27);
            this.txtIdUsuario.TabIndex = 5;
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Location = new System.Drawing.Point(12, 681);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(50, 20);
            this.lblResultado.TabIndex = 7;
            this.lblResultado.Text = "label1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1088, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 31);
            this.button1.TabIndex = 8;
            this.button1.Text = "Filtrar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 873);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.lblIdUsuario);
            this.Controls.Add(this.txtIdUsuario);
            this.Controls.Add(this.lblIdLivro);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.txtIdLivro);
            this.Controls.Add(this.lista);
            this.Controls.Add(this.lblRelatorios);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "RelForm";
            this.Text = "InicioForm";
            this.Load += new System.EventHandler(this.RelForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lista)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRelatorios;
        private System.Windows.Forms.DataGridView lista;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdEmpréstimo;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdLivro;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdUsuário;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataDevolucao;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataEmprestimo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.TextBox txtIdLivro;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Label lblIdLivro;
        private System.Windows.Forms.Label lblIdUsuario;
        private System.Windows.Forms.TextBox txtIdUsuario;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Button button1;
    }
}