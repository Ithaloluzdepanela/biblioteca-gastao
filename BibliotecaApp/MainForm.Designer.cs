namespace BibliotecaApp
{
    partial class MainForm
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelControl = new System.Windows.Forms.Panel();
            this.ControlPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.picExit = new System.Windows.Forms.PictureBox();
            this.picMax = new System.Windows.Forms.PictureBox();
            this.picMin = new System.Windows.Forms.PictureBox();
            this.picMenu = new System.Windows.Forms.PictureBox();
            this.menu = new System.Windows.Forms.FlowLayoutPanel();
            this.incioContainer = new System.Windows.Forms.Panel();
            this.btnInicio = new System.Windows.Forms.Button();
            this.usuarioContainer = new System.Windows.Forms.Panel();
            this.btnUsuario = new System.Windows.Forms.Button();
            this.livroContainer = new System.Windows.Forms.Panel();
            this.btnLivro = new System.Windows.Forms.Button();
            this.relContainer = new System.Windows.Forms.Panel();
            this.btnRel = new System.Windows.Forms.Button();
            this.sairContainer = new System.Windows.Forms.Panel();
            this.btnSair = new System.Windows.Forms.Button();
            this.menuTransition = new System.Windows.Forms.Timer(this.components);
            this.panelControl.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenu)).BeginInit();
            this.menu.SuspendLayout();
            this.incioContainer.SuspendLayout();
            this.usuarioContainer.SuspendLayout();
            this.livroContainer.SuspendLayout();
            this.relContainer.SuspendLayout();
            this.sairContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            this.panelControl.AllowDrop = true;
            this.panelControl.BackColor = System.Drawing.Color.White;
            this.panelControl.Controls.Add(this.ControlPanel);
            this.panelControl.Controls.Add(this.picMenu);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl.Location = new System.Drawing.Point(0, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(1024, 30);
            this.panelControl.TabIndex = 0;
            this.panelControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelControl_MouseDown);
            this.panelControl.MouseEnter += new System.EventHandler(this.panelControl_MouseEnter);
            // 
            // ControlPanel
            // 
            this.ControlPanel.BackColor = System.Drawing.Color.Transparent;
            this.ControlPanel.Controls.Add(this.picExit);
            this.ControlPanel.Controls.Add(this.picMax);
            this.ControlPanel.Controls.Add(this.picMin);
            this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ControlPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.ControlPanel.Location = new System.Drawing.Point(916, 0);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(108, 30);
            this.ControlPanel.TabIndex = 4;
            // 
            // picExit
            // 
            this.picExit.BackColor = System.Drawing.Color.Transparent;
            this.picExit.BackgroundImage = global::BibliotecaApp.Properties.Resources.icons8_x_20;
            this.picExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picExit.Location = new System.Drawing.Point(80, 3);
            this.picExit.Name = "picExit";
            this.picExit.Size = new System.Drawing.Size(25, 25);
            this.picExit.TabIndex = 3;
            this.picExit.TabStop = false;
            this.picExit.Click += new System.EventHandler(this.picExit_Click);
            this.picExit.MouseEnter += new System.EventHandler(this.picExit_MouseEnter);
            this.picExit.MouseLeave += new System.EventHandler(this.picExit_MouseLeave);
            // 
            // picMax
            // 
            this.picMax.BackColor = System.Drawing.Color.Transparent;
            this.picMax.BackgroundImage = global::BibliotecaApp.Properties.Resources.icons8_verificar_todos_os_20;
            this.picMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picMax.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMax.Location = new System.Drawing.Point(49, 3);
            this.picMax.Name = "picMax";
            this.picMax.Size = new System.Drawing.Size(25, 25);
            this.picMax.TabIndex = 6;
            this.picMax.TabStop = false;
            this.picMax.Click += new System.EventHandler(this.picMax_Click);
            this.picMax.MouseEnter += new System.EventHandler(this.picMax_MouseEnter);
            this.picMax.MouseLeave += new System.EventHandler(this.picMax_MouseLeave);
            // 
            // picMin
            // 
            this.picMin.BackColor = System.Drawing.Color.Transparent;
            this.picMin.BackgroundImage = global::BibliotecaApp.Properties.Resources.icons8_menos_20;
            this.picMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picMin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMin.Location = new System.Drawing.Point(18, 3);
            this.picMin.Name = "picMin";
            this.picMin.Size = new System.Drawing.Size(25, 25);
            this.picMin.TabIndex = 5;
            this.picMin.TabStop = false;
            this.picMin.Click += new System.EventHandler(this.picMin_Click);
            this.picMin.MouseEnter += new System.EventHandler(this.picMin_MouseEnter);
            this.picMin.MouseLeave += new System.EventHandler(this.picMin_MouseLeave);
            // 
            // picMenu
            // 
            this.picMenu.BackgroundImage = global::BibliotecaApp.Properties.Resources.icons8_cardápio_25;
            this.picMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMenu.Location = new System.Drawing.Point(5, 0);
            this.picMenu.Name = "picMenu";
            this.picMenu.Size = new System.Drawing.Size(30, 30);
            this.picMenu.TabIndex = 2;
            this.picMenu.TabStop = false;
            this.picMenu.Click += new System.EventHandler(this.picMenu_Click);
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.menu.Controls.Add(this.incioContainer);
            this.menu.Controls.Add(this.usuarioContainer);
            this.menu.Controls.Add(this.livroContainer);
            this.menu.Controls.Add(this.relContainer);
            this.menu.Controls.Add(this.sairContainer);
            this.menu.Dock = System.Windows.Forms.DockStyle.Left;
            this.menu.Location = new System.Drawing.Point(0, 30);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(40, 738);
            this.menu.TabIndex = 1;
            // 
            // incioContainer
            // 
            this.incioContainer.Controls.Add(this.btnInicio);
            this.incioContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.incioContainer.Location = new System.Drawing.Point(3, 3);
            this.incioContainer.Name = "incioContainer";
            this.incioContainer.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.incioContainer.Size = new System.Drawing.Size(200, 70);
            this.incioContainer.TabIndex = 2;
            // 
            // btnInicio
            // 
            this.btnInicio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnInicio.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnInicio.ForeColor = System.Drawing.Color.White;
            this.btnInicio.Image = global::BibliotecaApp.Properties.Resources.icons8_página_inicial_25;
            this.btnInicio.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInicio.Location = new System.Drawing.Point(-6, -24);
            this.btnInicio.Name = "btnInicio";
            this.btnInicio.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnInicio.Size = new System.Drawing.Size(218, 119);
            this.btnInicio.TabIndex = 3;
            this.btnInicio.Text = "Início";
            this.btnInicio.UseVisualStyleBackColor = false;
            this.btnInicio.Click += new System.EventHandler(this.btnInicio_Click);
            // 
            // usuarioContainer
            // 
            this.usuarioContainer.Controls.Add(this.btnUsuario);
            this.usuarioContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.usuarioContainer.Location = new System.Drawing.Point(3, 79);
            this.usuarioContainer.Name = "usuarioContainer";
            this.usuarioContainer.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.usuarioContainer.Size = new System.Drawing.Size(200, 70);
            this.usuarioContainer.TabIndex = 4;
            // 
            // btnUsuario
            // 
            this.btnUsuario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnUsuario.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnUsuario.ForeColor = System.Drawing.Color.White;
            this.btnUsuario.Image = global::BibliotecaApp.Properties.Resources.icons8_chamada_em_conferência_25;
            this.btnUsuario.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUsuario.Location = new System.Drawing.Point(-6, -24);
            this.btnUsuario.Name = "btnUsuario";
            this.btnUsuario.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnUsuario.Size = new System.Drawing.Size(218, 119);
            this.btnUsuario.TabIndex = 3;
            this.btnUsuario.Text = "Usuários";
            this.btnUsuario.UseVisualStyleBackColor = false;
            this.btnUsuario.Click += new System.EventHandler(this.btnUsuario_Click);
            // 
            // livroContainer
            // 
            this.livroContainer.Controls.Add(this.btnLivro);
            this.livroContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.livroContainer.Location = new System.Drawing.Point(3, 155);
            this.livroContainer.Name = "livroContainer";
            this.livroContainer.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.livroContainer.Size = new System.Drawing.Size(200, 70);
            this.livroContainer.TabIndex = 5;
            // 
            // btnLivro
            // 
            this.btnLivro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnLivro.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnLivro.ForeColor = System.Drawing.Color.White;
            this.btnLivro.Image = global::BibliotecaApp.Properties.Resources.icons8_livros_25;
            this.btnLivro.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLivro.Location = new System.Drawing.Point(-6, -24);
            this.btnLivro.Name = "btnLivro";
            this.btnLivro.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnLivro.Size = new System.Drawing.Size(218, 119);
            this.btnLivro.TabIndex = 3;
            this.btnLivro.Text = "Livros";
            this.btnLivro.UseVisualStyleBackColor = false;
            this.btnLivro.Click += new System.EventHandler(this.btnLivro_Click);
            // 
            // relContainer
            // 
            this.relContainer.Controls.Add(this.btnRel);
            this.relContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.relContainer.Location = new System.Drawing.Point(3, 231);
            this.relContainer.Name = "relContainer";
            this.relContainer.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.relContainer.Size = new System.Drawing.Size(200, 70);
            this.relContainer.TabIndex = 6;
            // 
            // btnRel
            // 
            this.btnRel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnRel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnRel.ForeColor = System.Drawing.Color.White;
            this.btnRel.Image = global::BibliotecaApp.Properties.Resources.icons8_relatório_25;
            this.btnRel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRel.Location = new System.Drawing.Point(-6, -24);
            this.btnRel.Name = "btnRel";
            this.btnRel.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnRel.Size = new System.Drawing.Size(218, 119);
            this.btnRel.TabIndex = 3;
            this.btnRel.Text = "Relatório";
            this.btnRel.UseVisualStyleBackColor = false;
            this.btnRel.Click += new System.EventHandler(this.btnRel_Click);
            // 
            // sairContainer
            // 
            this.sairContainer.Controls.Add(this.btnSair);
            this.sairContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sairContainer.Location = new System.Drawing.Point(3, 307);
            this.sairContainer.Name = "sairContainer";
            this.sairContainer.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.sairContainer.Size = new System.Drawing.Size(200, 70);
            this.sairContainer.TabIndex = 7;
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnSair.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnSair.ForeColor = System.Drawing.Color.White;
            this.btnSair.Image = global::BibliotecaApp.Properties.Resources.icons8_sair_25;
            this.btnSair.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSair.Location = new System.Drawing.Point(-6, -24);
            this.btnSair.Name = "btnSair";
            this.btnSair.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnSair.Size = new System.Drawing.Size(218, 119);
            this.btnSair.TabIndex = 3;
            this.btnSair.Text = "Sair";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // menuTransition
            // 
            this.menuTransition.Interval = 15;
            this.menuTransition.Tick += new System.EventHandler(this.menuTransition_Tick);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.panelControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelControl.ResumeLayout(false);
            this.ControlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenu)).EndInit();
            this.menu.ResumeLayout(false);
            this.incioContainer.ResumeLayout(false);
            this.usuarioContainer.ResumeLayout(false);
            this.livroContainer.ResumeLayout(false);
            this.relContainer.ResumeLayout(false);
            this.sairContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.PictureBox picMenu;
        private System.Windows.Forms.FlowLayoutPanel menu;
        private System.Windows.Forms.Panel incioContainer;
        private System.Windows.Forms.Button btnInicio;
        private System.Windows.Forms.Panel usuarioContainer;
        private System.Windows.Forms.Button btnUsuario;
        private System.Windows.Forms.Panel relContainer;
        private System.Windows.Forms.Button btnRel;
        private System.Windows.Forms.Panel livroContainer;
        private System.Windows.Forms.Button btnLivro;
        private System.Windows.Forms.Panel sairContainer;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Timer menuTransition;
        private System.Windows.Forms.PictureBox picExit;
        private System.Windows.Forms.FlowLayoutPanel ControlPanel;
        private System.Windows.Forms.PictureBox picMin;
        private System.Windows.Forms.PictureBox picMax;
    }
}

