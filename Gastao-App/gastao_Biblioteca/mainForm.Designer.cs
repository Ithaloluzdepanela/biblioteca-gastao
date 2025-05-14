namespace gastao_Biblioteca
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.nightControlBox1 = new ReaLTaiizor.Controls.NightControlBox();
            this.picTheme = new System.Windows.Forms.PictureBox();
            this.btnMenu = new System.Windows.Forms.PictureBox();
            this.menu = new System.Windows.Forms.FlowLayoutPanel();
            this.pnHome = new System.Windows.Forms.Panel();
            this.btnHome = new System.Windows.Forms.Button();
            this.pnUsers = new System.Windows.Forms.Panel();
            this.btnUsers = new System.Windows.Forms.Button();
            this.bookContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.book = new System.Windows.Forms.Button();
            this.btnLivros = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnFind = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnEmpres = new System.Windows.Forms.Button();
            this.pnRel = new System.Windows.Forms.Panel();
            this.btnRel = new System.Windows.Forms.Button();
            this.pnLogout = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.menuTransition = new System.Windows.Forms.Timer(this.components);
            this.bookTransition = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTheme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMenu)).BeginInit();
            this.menu.SuspendLayout();
            this.pnHome.SuspendLayout();
            this.pnUsers.SuspendLayout();
            this.bookContainer.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel8.SuspendLayout();
            this.pnRel.SuspendLayout();
            this.pnLogout.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.nightControlBox1);
            this.panel1.Controls.Add(this.picTheme);
            this.panel1.Controls.Add(this.btnMenu);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1014, 36);
            this.panel1.TabIndex = 12;
            // 
            // nightControlBox1
            // 
            this.nightControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nightControlBox1.BackColor = System.Drawing.Color.Transparent;
            this.nightControlBox1.CloseHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.nightControlBox1.CloseHoverForeColor = System.Drawing.Color.White;
            this.nightControlBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nightControlBox1.DefaultLocation = true;
            this.nightControlBox1.DisableMaximizeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.nightControlBox1.DisableMinimizeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.nightControlBox1.EnableCloseColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.nightControlBox1.EnableMaximizeButton = true;
            this.nightControlBox1.EnableMaximizeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.nightControlBox1.EnableMinimizeButton = true;
            this.nightControlBox1.EnableMinimizeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.nightControlBox1.Location = new System.Drawing.Point(875, 0);
            this.nightControlBox1.MaximizeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.nightControlBox1.MaximizeHoverForeColor = System.Drawing.Color.White;
            this.nightControlBox1.MinimizeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.nightControlBox1.MinimizeHoverForeColor = System.Drawing.Color.White;
            this.nightControlBox1.Name = "nightControlBox1";
            this.nightControlBox1.Size = new System.Drawing.Size(139, 31);
            this.nightControlBox1.TabIndex = 14;
            // 
            // picTheme
            // 
            this.picTheme.BackgroundImage = global::gastao_Biblioteca.Properties.Resources.icons8_símbolo_da_lua_16;
            this.picTheme.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picTheme.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picTheme.Location = new System.Drawing.Point(44, 3);
            this.picTheme.Name = "picTheme";
            this.picTheme.Size = new System.Drawing.Size(35, 33);
            this.picTheme.TabIndex = 14;
            this.picTheme.TabStop = false;
            this.picTheme.Click += new System.EventHandler(this.picTheme_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.BackgroundImage = global::gastao_Biblioteca.Properties.Resources.icons8_cardápio_16;
            this.btnMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenu.Location = new System.Drawing.Point(3, 3);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(35, 33);
            this.btnMenu.TabIndex = 11;
            this.btnMenu.TabStop = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.menu.Controls.Add(this.pnHome);
            this.menu.Controls.Add(this.pnUsers);
            this.menu.Controls.Add(this.bookContainer);
            this.menu.Controls.Add(this.pnRel);
            this.menu.Controls.Add(this.pnLogout);
            this.menu.Dock = System.Windows.Forms.DockStyle.Left;
            this.menu.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.menu.Location = new System.Drawing.Point(0, 36);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(38, 611);
            this.menu.TabIndex = 13;
            // 
            // pnHome
            // 
            this.pnHome.Controls.Add(this.btnHome);
            this.pnHome.Location = new System.Drawing.Point(3, 3);
            this.pnHome.Name = "pnHome";
            this.pnHome.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pnHome.Size = new System.Drawing.Size(205, 56);
            this.pnHome.TabIndex = 5;
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHome.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Image = global::gastao_Biblioteca.Properties.Resources.icons8_casa_20;
            this.btnHome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHome.Location = new System.Drawing.Point(-9, -15);
            this.btnHome.Name = "btnHome";
            this.btnHome.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnHome.Size = new System.Drawing.Size(224, 89);
            this.btnHome.TabIndex = 3;
            this.btnHome.Text = "Início";
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.home_Click);
            // 
            // pnUsers
            // 
            this.pnUsers.Controls.Add(this.btnUsers);
            this.pnUsers.Location = new System.Drawing.Point(3, 65);
            this.pnUsers.Name = "pnUsers";
            this.pnUsers.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pnUsers.Size = new System.Drawing.Size(205, 56);
            this.pnUsers.TabIndex = 6;
            // 
            // btnUsers
            // 
            this.btnUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnUsers.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnUsers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUsers.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUsers.ForeColor = System.Drawing.Color.White;
            this.btnUsers.Image = global::gastao_Biblioteca.Properties.Resources.icons8_grupo_de_usuários_20;
            this.btnUsers.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUsers.Location = new System.Drawing.Point(-9, -15);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnUsers.Size = new System.Drawing.Size(224, 89);
            this.btnUsers.TabIndex = 3;
            this.btnUsers.Text = "Usuários";
            this.btnUsers.UseVisualStyleBackColor = false;
            this.btnUsers.Click += new System.EventHandler(this.users_Click);
            // 
            // bookContainer
            // 
            this.bookContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(82)))), ((int)(((byte)(118)))));
            this.bookContainer.Controls.Add(this.panel6);
            this.bookContainer.Controls.Add(this.panel5);
            this.bookContainer.Controls.Add(this.panel8);
            this.bookContainer.Location = new System.Drawing.Point(3, 127);
            this.bookContainer.Name = "bookContainer";
            this.bookContainer.Size = new System.Drawing.Size(205, 56);
            this.bookContainer.TabIndex = 11;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.book);
            this.panel6.Controls.Add(this.btnLivros);
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.panel6.Size = new System.Drawing.Size(205, 56);
            this.panel6.TabIndex = 6;
            // 
            // book
            // 
            this.book.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.book.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.book.Cursor = System.Windows.Forms.Cursors.Hand;
            this.book.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.book.ForeColor = System.Drawing.Color.White;
            this.book.Image = global::gastao_Biblioteca.Properties.Resources.icons8_livros_20;
            this.book.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.book.Location = new System.Drawing.Point(-10, -16);
            this.book.Name = "book";
            this.book.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.book.Size = new System.Drawing.Size(224, 89);
            this.book.TabIndex = 4;
            this.book.Text = "Livros";
            this.book.UseVisualStyleBackColor = false;
            this.book.Click += new System.EventHandler(this.book_Click);
            // 
            // btnLivros
            // 
            this.btnLivros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnLivros.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnLivros.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLivros.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLivros.ForeColor = System.Drawing.Color.White;
            this.btnLivros.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLivros.Location = new System.Drawing.Point(-9, -15);
            this.btnLivros.Name = "btnLivros";
            this.btnLivros.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnLivros.Size = new System.Drawing.Size(224, 89);
            this.btnLivros.TabIndex = 3;
            this.btnLivros.Text = "Livros";
            this.btnLivros.UseVisualStyleBackColor = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnFind);
            this.panel5.Location = new System.Drawing.Point(0, 56);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(5, 3, 0, 0);
            this.panel5.Size = new System.Drawing.Size(205, 56);
            this.panel5.TabIndex = 12;
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(82)))), ((int)(((byte)(118)))));
            this.btnFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnFind.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFind.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.ForeColor = System.Drawing.Color.White;
            this.btnFind.Image = global::gastao_Biblioteca.Properties.Resources.icons8_pesquisar_20;
            this.btnFind.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFind.Location = new System.Drawing.Point(-9, -10);
            this.btnFind.Name = "btnFind";
            this.btnFind.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFind.Size = new System.Drawing.Size(224, 89);
            this.btnFind.TabIndex = 3;
            this.btnFind.Text = "Pequisar";
            this.btnFind.UseVisualStyleBackColor = false;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btnEmpres);
            this.panel8.Location = new System.Drawing.Point(0, 112);
            this.panel8.Margin = new System.Windows.Forms.Padding(0);
            this.panel8.Name = "panel8";
            this.panel8.Padding = new System.Windows.Forms.Padding(5, 3, 0, 0);
            this.panel8.Size = new System.Drawing.Size(205, 56);
            this.panel8.TabIndex = 13;
            // 
            // btnEmpres
            // 
            this.btnEmpres.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(82)))), ((int)(((byte)(118)))));
            this.btnEmpres.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnEmpres.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEmpres.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmpres.ForeColor = System.Drawing.Color.White;
            this.btnEmpres.Image = global::gastao_Biblioteca.Properties.Resources.icons8_livrosair_20;
            this.btnEmpres.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmpres.Location = new System.Drawing.Point(-9, -15);
            this.btnEmpres.Name = "btnEmpres";
            this.btnEmpres.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnEmpres.Size = new System.Drawing.Size(224, 89);
            this.btnEmpres.TabIndex = 3;
            this.btnEmpres.Text = "Empréstimo";
            this.btnEmpres.UseVisualStyleBackColor = false;
            this.btnEmpres.Click += new System.EventHandler(this.btnEmpres_Click);
            // 
            // pnRel
            // 
            this.pnRel.Controls.Add(this.btnRel);
            this.pnRel.Location = new System.Drawing.Point(3, 189);
            this.pnRel.Name = "pnRel";
            this.pnRel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pnRel.Size = new System.Drawing.Size(205, 56);
            this.pnRel.TabIndex = 6;
            // 
            // btnRel
            // 
            this.btnRel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnRel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRel.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRel.ForeColor = System.Drawing.Color.White;
            this.btnRel.Image = global::gastao_Biblioteca.Properties.Resources.icons8_relatório_20;
            this.btnRel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRel.Location = new System.Drawing.Point(-9, -15);
            this.btnRel.Name = "btnRel";
            this.btnRel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnRel.Size = new System.Drawing.Size(224, 89);
            this.btnRel.TabIndex = 3;
            this.btnRel.Text = "Relatório";
            this.btnRel.UseVisualStyleBackColor = false;
            this.btnRel.Click += new System.EventHandler(this.btnRel_Click);
            // 
            // pnLogout
            // 
            this.pnLogout.Controls.Add(this.btnLogout);
            this.pnLogout.Location = new System.Drawing.Point(3, 251);
            this.pnLogout.Name = "pnLogout";
            this.pnLogout.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pnLogout.Size = new System.Drawing.Size(205, 56);
            this.pnLogout.TabIndex = 6;
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnLogout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Image = global::gastao_Biblioteca.Properties.Resources.icons8_sair_20;
            this.btnLogout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.Location = new System.Drawing.Point(-9, -15);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnLogout.Size = new System.Drawing.Size(224, 89);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Sair";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.logout_Click);
            // 
            // menuTransition
            // 
            this.menuTransition.Interval = 10;
            this.menuTransition.Tick += new System.EventHandler(this.menuTransition_Tick);
            // 
            // bookTransition
            // 
            this.bookTransition.Interval = 10;
            this.bookTransition.Tick += new System.EventHandler(this.bookTransition_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1014, 647);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picTheme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMenu)).EndInit();
            this.menu.ResumeLayout(false);
            this.pnHome.ResumeLayout(false);
            this.pnUsers.ResumeLayout(false);
            this.bookContainer.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.pnRel.ResumeLayout(false);
            this.pnLogout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox btnMenu;
        private System.Windows.Forms.FlowLayoutPanel menu;
        private System.Windows.Forms.Panel pnHome;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Panel pnUsers;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.FlowLayoutPanel bookContainer;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button book;
        private System.Windows.Forms.Button btnLivros;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btnEmpres;
        private System.Windows.Forms.Panel pnRel;
        private System.Windows.Forms.Button rel;
        private System.Windows.Forms.Panel pnLogout;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Timer menuTransition;
        private System.Windows.Forms.Timer bookTransition;
        private System.Windows.Forms.PictureBox picTheme;
        private ReaLTaiizor.Controls.NightControlBox nightControlBox1;
        private System.Windows.Forms.Button btnRel;
    }
}

