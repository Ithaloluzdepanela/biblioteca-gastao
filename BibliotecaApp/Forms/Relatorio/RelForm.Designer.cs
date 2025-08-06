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
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();
            this.lblIdUsuario = new System.Windows.Forms.Label();
            this.txtIdUsuario = new System.Windows.Forms.TextBox();
            this.lblIdLivro = new System.Windows.Forms.Label();
            this.txtIdLivro = new System.Windows.Forms.TextBox();
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPageEmprestimos = new System.Windows.Forms.TabPage();
            this.lista = new System.Windows.Forms.DataGridView();
            this.IdEmpréstimo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdLivro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdUsuário = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataDevolucao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataEmprestimo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageUsuarios = new System.Windows.Forms.TabPage();
            this.txtTurma = new System.Windows.Forms.TextBox();
            this.lblTurma = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtNomeU = new System.Windows.Forms.TextBox();
            this.lista2 = new System.Windows.Forms.DataGridView();
            this.IDUsuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NomeUsuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CPF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataNascimento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Turma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Telefone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TipoUsuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblNome = new System.Windows.Forms.Label();
            this.btnFiltrar2 = new System.Windows.Forms.Button();
            this.lblIdU = new System.Windows.Forms.Label();
            this.txtIdU = new System.Windows.Forms.TextBox();
            this.tabPageLivros = new System.Windows.Forms.TabPage();
            this.txtAutor = new System.Windows.Forms.TextBox();
            this.lista3 = new System.Windows.Forms.DataGridView();
            this.IdL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NomeLivro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Genero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Autor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CodigoBarras = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Disponibilidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblAutor = new System.Windows.Forms.Label();
            this.btnFiltrar3 = new System.Windows.Forms.Button();
            this.txtGenero = new System.Windows.Forms.TextBox();
            this.txtIdLivroL = new System.Windows.Forms.TextBox();
            this.lblGenero = new System.Windows.Forms.Label();
            this.lblLivroId = new System.Windows.Forms.Label();
            this.txtNomeL = new System.Windows.Forms.TextBox();
            this.lblNomeL = new System.Windows.Forms.Label();
            this.tabPageReservas = new System.Windows.Forms.TabPage();
            this.lista4 = new System.Windows.Forms.DataGridView();
            this.IdReserva = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsuarioId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LivroId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdBibliotecaria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataReserva = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataDisponibilidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataLimiteRetirada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusReserva = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.lblDataReserva = new System.Windows.Forms.Label();
            this.txtIdLivroReservado = new System.Windows.Forms.TextBox();
            this.lblIdLivroReservado = new System.Windows.Forms.Label();
            this.txtIdUsuarioReservista = new System.Windows.Forms.TextBox();
            this.lblIdUsuarioReservista = new System.Windows.Forms.Label();
            this.Filtrar4 = new System.Windows.Forms.Button();
            this.lblIdReserva = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.txtDataReserva = new System.Windows.Forms.MaskedTextBox();
            this.txtDataDisponibilidade = new System.Windows.Forms.MaskedTextBox();
            this.lblDataDisponibilidade = new System.Windows.Forms.Label();
            this.txtStatusReserva = new System.Windows.Forms.TextBox();
            this.lblStatusReserva = new System.Windows.Forms.Label();
            this.materialTabControl1.SuspendLayout();
            this.tabPageEmprestimos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lista)).BeginInit();
            this.tabPageUsuarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lista2)).BeginInit();
            this.tabPageLivros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lista3)).BeginInit();
            this.tabPageReservas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lista4)).BeginInit();
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(127, 813);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 31);
            this.button1.TabIndex = 16;
            this.button1.Text = "Pasta";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Location = new System.Drawing.Point(956, 16);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(213, 59);
            this.btnFiltrar.TabIndex = 11;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Location = new System.Drawing.Point(24, 813);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(50, 20);
            this.lblResultado.TabIndex = 22;
            this.lblResultado.Text = "label1";
            // 
            // lblIdUsuario
            // 
            this.lblIdUsuario.AutoSize = true;
            this.lblIdUsuario.Location = new System.Drawing.Point(959, 190);
            this.lblIdUsuario.Name = "lblIdUsuario";
            this.lblIdUsuario.Size = new System.Drawing.Size(72, 20);
            this.lblIdUsuario.TabIndex = 21;
            this.lblIdUsuario.Text = "IdUsuario";
            // 
            // txtIdUsuario
            // 
            this.txtIdUsuario.Location = new System.Drawing.Point(963, 225);
            this.txtIdUsuario.Name = "txtIdUsuario";
            this.txtIdUsuario.Size = new System.Drawing.Size(194, 27);
            this.txtIdUsuario.TabIndex = 20;
            // 
            // lblIdLivro
            // 
            this.lblIdLivro.AutoSize = true;
            this.lblIdLivro.Location = new System.Drawing.Point(959, 106);
            this.lblIdLivro.Name = "lblIdLivro";
            this.lblIdLivro.Size = new System.Drawing.Size(54, 20);
            this.lblIdLivro.TabIndex = 19;
            this.lblIdLivro.Text = "IdLivro";
            // 
            // txtIdLivro
            // 
            this.txtIdLivro.Location = new System.Drawing.Point(963, 141);
            this.txtIdLivro.Name = "txtIdLivro";
            this.txtIdLivro.Size = new System.Drawing.Size(194, 27);
            this.txtIdLivro.TabIndex = 18;
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Controls.Add(this.tabPageEmprestimos);
            this.materialTabControl1.Controls.Add(this.tabPageUsuarios);
            this.materialTabControl1.Controls.Add(this.tabPageLivros);
            this.materialTabControl1.Controls.Add(this.tabPageReservas);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.Location = new System.Drawing.Point(12, 48);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(1183, 746);
            this.materialTabControl1.TabIndex = 23;
            // 
            // tabPageEmprestimos
            // 
            this.tabPageEmprestimos.Controls.Add(this.lblStatus);
            this.tabPageEmprestimos.Controls.Add(this.txtStatus);
            this.tabPageEmprestimos.Controls.Add(this.lista);
            this.tabPageEmprestimos.Controls.Add(this.txtIdLivro);
            this.tabPageEmprestimos.Controls.Add(this.lblIdLivro);
            this.tabPageEmprestimos.Controls.Add(this.lblIdUsuario);
            this.tabPageEmprestimos.Controls.Add(this.btnFiltrar);
            this.tabPageEmprestimos.Controls.Add(this.txtIdUsuario);
            this.tabPageEmprestimos.Location = new System.Drawing.Point(4, 29);
            this.tabPageEmprestimos.Name = "tabPageEmprestimos";
            this.tabPageEmprestimos.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEmprestimos.Size = new System.Drawing.Size(1175, 713);
            this.tabPageEmprestimos.TabIndex = 0;
            this.tabPageEmprestimos.Text = "Empréstimos";
            this.tabPageEmprestimos.UseVisualStyleBackColor = true;
            this.tabPageEmprestimos.Click += new System.EventHandler(this.tabPageEmprestimos_Click);
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
            this.lista.Dock = System.Windows.Forms.DockStyle.Left;
            this.lista.Location = new System.Drawing.Point(3, 3);
            this.lista.Name = "lista";
            this.lista.RowHeadersWidth = 51;
            this.lista.RowTemplate.Height = 24;
            this.lista.Size = new System.Drawing.Size(950, 707);
            this.lista.TabIndex = 25;
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
            // tabPageUsuarios
            // 
            this.tabPageUsuarios.Controls.Add(this.txtTurma);
            this.tabPageUsuarios.Controls.Add(this.lblTurma);
            this.tabPageUsuarios.Controls.Add(this.txtEmail);
            this.tabPageUsuarios.Controls.Add(this.lblEmail);
            this.tabPageUsuarios.Controls.Add(this.txtNomeU);
            this.tabPageUsuarios.Controls.Add(this.lista2);
            this.tabPageUsuarios.Controls.Add(this.lblNome);
            this.tabPageUsuarios.Controls.Add(this.btnFiltrar2);
            this.tabPageUsuarios.Controls.Add(this.lblIdU);
            this.tabPageUsuarios.Controls.Add(this.txtIdU);
            this.tabPageUsuarios.Location = new System.Drawing.Point(4, 29);
            this.tabPageUsuarios.Name = "tabPageUsuarios";
            this.tabPageUsuarios.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUsuarios.Size = new System.Drawing.Size(1175, 713);
            this.tabPageUsuarios.TabIndex = 1;
            this.tabPageUsuarios.Text = "Usuários";
            this.tabPageUsuarios.UseVisualStyleBackColor = true;
            // 
            // txtTurma
            // 
            this.txtTurma.Location = new System.Drawing.Point(963, 375);
            this.txtTurma.Name = "txtTurma";
            this.txtTurma.Size = new System.Drawing.Size(194, 27);
            this.txtTurma.TabIndex = 31;
            this.txtTurma.TextChanged += new System.EventHandler(this.txtTurma_TextChanged);
            // 
            // lblTurma
            // 
            this.lblTurma.AutoSize = true;
            this.lblTurma.Location = new System.Drawing.Point(959, 343);
            this.lblTurma.Name = "lblTurma";
            this.lblTurma.Size = new System.Drawing.Size(51, 20);
            this.lblTurma.TabIndex = 32;
            this.lblTurma.Text = "Turma";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(963, 300);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(194, 27);
            this.txtEmail.TabIndex = 29;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(959, 268);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(52, 20);
            this.lblEmail.TabIndex = 30;
            this.lblEmail.Text = "E-mail";
            // 
            // txtNomeU
            // 
            this.txtNomeU.Location = new System.Drawing.Point(963, 225);
            this.txtNomeU.Name = "txtNomeU";
            this.txtNomeU.Size = new System.Drawing.Size(194, 27);
            this.txtNomeU.TabIndex = 25;
            // 
            // lista2
            // 
            this.lista2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.lista2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lista2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IDUsuario,
            this.NomeUsuario,
            this.Email,
            this.CPF,
            this.DataNascimento,
            this.Turma,
            this.Telefone,
            this.TipoUsuario});
            this.lista2.Dock = System.Windows.Forms.DockStyle.Left;
            this.lista2.Location = new System.Drawing.Point(3, 3);
            this.lista2.Name = "lista2";
            this.lista2.RowHeadersWidth = 51;
            this.lista2.RowTemplate.Height = 24;
            this.lista2.Size = new System.Drawing.Size(950, 707);
            this.lista2.TabIndex = 0;
            // 
            // IDUsuario
            // 
            this.IDUsuario.HeaderText = "Id Usuário";
            this.IDUsuario.MinimumWidth = 6;
            this.IDUsuario.Name = "IDUsuario";
            // 
            // NomeUsuario
            // 
            this.NomeUsuario.HeaderText = "Nome";
            this.NomeUsuario.MinimumWidth = 6;
            this.NomeUsuario.Name = "NomeUsuario";
            // 
            // Email
            // 
            this.Email.HeaderText = "E-mail";
            this.Email.MinimumWidth = 6;
            this.Email.Name = "Email";
            // 
            // CPF
            // 
            this.CPF.HeaderText = "CPF";
            this.CPF.MinimumWidth = 6;
            this.CPF.Name = "CPF";
            // 
            // DataNascimento
            // 
            this.DataNascimento.HeaderText = "Data de Nascimento";
            this.DataNascimento.MinimumWidth = 6;
            this.DataNascimento.Name = "DataNascimento";
            // 
            // Turma
            // 
            this.Turma.HeaderText = "Turma";
            this.Turma.MinimumWidth = 6;
            this.Turma.Name = "Turma";
            // 
            // Telefone
            // 
            this.Telefone.HeaderText = "Telefone";
            this.Telefone.MinimumWidth = 6;
            this.Telefone.Name = "Telefone";
            // 
            // TipoUsuario
            // 
            this.TipoUsuario.HeaderText = "Tipo de Usuário";
            this.TipoUsuario.MinimumWidth = 6;
            this.TipoUsuario.Name = "TipoUsuario";
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(959, 193);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(50, 20);
            this.lblNome.TabIndex = 26;
            this.lblNome.Text = "Nome";
            this.lblNome.Click += new System.EventHandler(this.lblNome_Click);
            // 
            // btnFiltrar2
            // 
            this.btnFiltrar2.Location = new System.Drawing.Point(956, 16);
            this.btnFiltrar2.Name = "btnFiltrar2";
            this.btnFiltrar2.Size = new System.Drawing.Size(213, 59);
            this.btnFiltrar2.TabIndex = 24;
            this.btnFiltrar2.Text = "Filtrar";
            this.btnFiltrar2.UseVisualStyleBackColor = true;
            // 
            // lblIdU
            // 
            this.lblIdU.AutoSize = true;
            this.lblIdU.Location = new System.Drawing.Point(959, 112);
            this.lblIdU.Name = "lblIdU";
            this.lblIdU.Size = new System.Drawing.Size(72, 20);
            this.lblIdU.TabIndex = 28;
            this.lblIdU.Text = "IdUsuario";
            // 
            // txtIdU
            // 
            this.txtIdU.Location = new System.Drawing.Point(963, 147);
            this.txtIdU.Name = "txtIdU";
            this.txtIdU.Size = new System.Drawing.Size(194, 27);
            this.txtIdU.TabIndex = 27;
            // 
            // tabPageLivros
            // 
            this.tabPageLivros.Controls.Add(this.txtAutor);
            this.tabPageLivros.Controls.Add(this.lista3);
            this.tabPageLivros.Controls.Add(this.lblAutor);
            this.tabPageLivros.Controls.Add(this.btnFiltrar3);
            this.tabPageLivros.Controls.Add(this.txtGenero);
            this.tabPageLivros.Controls.Add(this.txtIdLivroL);
            this.tabPageLivros.Controls.Add(this.lblGenero);
            this.tabPageLivros.Controls.Add(this.lblLivroId);
            this.tabPageLivros.Controls.Add(this.txtNomeL);
            this.tabPageLivros.Controls.Add(this.lblNomeL);
            this.tabPageLivros.Location = new System.Drawing.Point(4, 29);
            this.tabPageLivros.Name = "tabPageLivros";
            this.tabPageLivros.Size = new System.Drawing.Size(1175, 713);
            this.tabPageLivros.TabIndex = 2;
            this.tabPageLivros.Text = "Livros";
            this.tabPageLivros.UseVisualStyleBackColor = true;
            // 
            // txtAutor
            // 
            this.txtAutor.Location = new System.Drawing.Point(963, 362);
            this.txtAutor.Name = "txtAutor";
            this.txtAutor.Size = new System.Drawing.Size(194, 27);
            this.txtAutor.TabIndex = 40;
            // 
            // lista3
            // 
            this.lista3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.lista3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lista3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdL,
            this.NomeLivro,
            this.Genero,
            this.Autor,
            this.Quantidade,
            this.CodigoBarras,
            this.Disponibilidade});
            this.lista3.Dock = System.Windows.Forms.DockStyle.Left;
            this.lista3.Location = new System.Drawing.Point(0, 0);
            this.lista3.Name = "lista3";
            this.lista3.RowHeadersWidth = 51;
            this.lista3.RowTemplate.Height = 24;
            this.lista3.Size = new System.Drawing.Size(950, 713);
            this.lista3.TabIndex = 24;
            // 
            // IdL
            // 
            this.IdL.HeaderText = "Id Livro";
            this.IdL.MinimumWidth = 6;
            this.IdL.Name = "IdL";
            // 
            // NomeLivro
            // 
            this.NomeLivro.HeaderText = "Nome";
            this.NomeLivro.MinimumWidth = 6;
            this.NomeLivro.Name = "NomeLivro";
            // 
            // Genero
            // 
            this.Genero.HeaderText = "Gênero";
            this.Genero.MinimumWidth = 6;
            this.Genero.Name = "Genero";
            // 
            // Autor
            // 
            this.Autor.HeaderText = "Autor";
            this.Autor.MinimumWidth = 6;
            this.Autor.Name = "Autor";
            // 
            // Quantidade
            // 
            this.Quantidade.HeaderText = "Quantidade";
            this.Quantidade.MinimumWidth = 6;
            this.Quantidade.Name = "Quantidade";
            // 
            // CodigoBarras
            // 
            this.CodigoBarras.HeaderText = "Código de Barras";
            this.CodigoBarras.MinimumWidth = 6;
            this.CodigoBarras.Name = "CodigoBarras";
            // 
            // Disponibilidade
            // 
            this.Disponibilidade.HeaderText = "Disponibilidade";
            this.Disponibilidade.MinimumWidth = 6;
            this.Disponibilidade.Name = "Disponibilidade";
            // 
            // lblAutor
            // 
            this.lblAutor.AutoSize = true;
            this.lblAutor.Location = new System.Drawing.Point(959, 330);
            this.lblAutor.Name = "lblAutor";
            this.lblAutor.Size = new System.Drawing.Size(46, 20);
            this.lblAutor.TabIndex = 41;
            this.lblAutor.Text = "Autor";
            // 
            // btnFiltrar3
            // 
            this.btnFiltrar3.Location = new System.Drawing.Point(956, 3);
            this.btnFiltrar3.Name = "btnFiltrar3";
            this.btnFiltrar3.Size = new System.Drawing.Size(213, 59);
            this.btnFiltrar3.TabIndex = 33;
            this.btnFiltrar3.Text = "Filtrar";
            this.btnFiltrar3.UseVisualStyleBackColor = true;
            // 
            // txtGenero
            // 
            this.txtGenero.Location = new System.Drawing.Point(963, 287);
            this.txtGenero.Name = "txtGenero";
            this.txtGenero.Size = new System.Drawing.Size(194, 27);
            this.txtGenero.TabIndex = 38;
            // 
            // txtIdLivroL
            // 
            this.txtIdLivroL.Location = new System.Drawing.Point(963, 134);
            this.txtIdLivroL.Name = "txtIdLivroL";
            this.txtIdLivroL.Size = new System.Drawing.Size(194, 27);
            this.txtIdLivroL.TabIndex = 36;
            // 
            // lblGenero
            // 
            this.lblGenero.AutoSize = true;
            this.lblGenero.Location = new System.Drawing.Point(959, 255);
            this.lblGenero.Name = "lblGenero";
            this.lblGenero.Size = new System.Drawing.Size(57, 20);
            this.lblGenero.TabIndex = 39;
            this.lblGenero.Text = "Gênero";
            // 
            // lblLivroId
            // 
            this.lblLivroId.AutoSize = true;
            this.lblLivroId.Location = new System.Drawing.Point(959, 99);
            this.lblLivroId.Name = "lblLivroId";
            this.lblLivroId.Size = new System.Drawing.Size(54, 20);
            this.lblLivroId.TabIndex = 37;
            this.lblLivroId.Text = "IdLivro";
            // 
            // txtNomeL
            // 
            this.txtNomeL.Location = new System.Drawing.Point(963, 212);
            this.txtNomeL.Name = "txtNomeL";
            this.txtNomeL.Size = new System.Drawing.Size(194, 27);
            this.txtNomeL.TabIndex = 34;
            // 
            // lblNomeL
            // 
            this.lblNomeL.AutoSize = true;
            this.lblNomeL.Location = new System.Drawing.Point(959, 180);
            this.lblNomeL.Name = "lblNomeL";
            this.lblNomeL.Size = new System.Drawing.Size(50, 20);
            this.lblNomeL.TabIndex = 35;
            this.lblNomeL.Text = "Nome";
            // 
            // tabPageReservas
            // 
            this.tabPageReservas.Controls.Add(this.txtStatusReserva);
            this.tabPageReservas.Controls.Add(this.lblStatusReserva);
            this.tabPageReservas.Controls.Add(this.txtDataDisponibilidade);
            this.tabPageReservas.Controls.Add(this.lblDataDisponibilidade);
            this.tabPageReservas.Controls.Add(this.txtDataReserva);
            this.tabPageReservas.Controls.Add(this.lista4);
            this.tabPageReservas.Controls.Add(this.lblDataReserva);
            this.tabPageReservas.Controls.Add(this.Filtrar4);
            this.tabPageReservas.Controls.Add(this.txtIdLivroReservado);
            this.tabPageReservas.Controls.Add(this.textBox4);
            this.tabPageReservas.Controls.Add(this.lblIdLivroReservado);
            this.tabPageReservas.Controls.Add(this.lblIdReserva);
            this.tabPageReservas.Controls.Add(this.txtIdUsuarioReservista);
            this.tabPageReservas.Controls.Add(this.lblIdUsuarioReservista);
            this.tabPageReservas.Location = new System.Drawing.Point(4, 29);
            this.tabPageReservas.Name = "tabPageReservas";
            this.tabPageReservas.Size = new System.Drawing.Size(1175, 713);
            this.tabPageReservas.TabIndex = 3;
            this.tabPageReservas.Text = "Reservas";
            this.tabPageReservas.UseVisualStyleBackColor = true;
            // 
            // lista4
            // 
            this.lista4.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.lista4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lista4.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdReserva,
            this.UsuarioId,
            this.LivroId,
            this.IdBibliotecaria,
            this.DataReserva,
            this.DataDisponibilidade,
            this.DataLimiteRetirada,
            this.StatusReserva});
            this.lista4.Dock = System.Windows.Forms.DockStyle.Left;
            this.lista4.Location = new System.Drawing.Point(0, 0);
            this.lista4.Name = "lista4";
            this.lista4.RowHeadersWidth = 51;
            this.lista4.RowTemplate.Height = 24;
            this.lista4.Size = new System.Drawing.Size(950, 713);
            this.lista4.TabIndex = 24;
            // 
            // IdReserva
            // 
            this.IdReserva.HeaderText = "Id Reserva";
            this.IdReserva.MinimumWidth = 6;
            this.IdReserva.Name = "IdReserva";
            // 
            // UsuarioId
            // 
            this.UsuarioId.HeaderText = "IdUsuário";
            this.UsuarioId.MinimumWidth = 6;
            this.UsuarioId.Name = "UsuarioId";
            // 
            // LivroId
            // 
            this.LivroId.HeaderText = "Id Livro";
            this.LivroId.MinimumWidth = 6;
            this.LivroId.Name = "LivroId";
            // 
            // IdBibliotecaria
            // 
            this.IdBibliotecaria.HeaderText = "Id Bibliotecaria";
            this.IdBibliotecaria.MinimumWidth = 6;
            this.IdBibliotecaria.Name = "IdBibliotecaria";
            // 
            // DataReserva
            // 
            this.DataReserva.HeaderText = "Data da Reserva";
            this.DataReserva.MinimumWidth = 6;
            this.DataReserva.Name = "DataReserva";
            // 
            // DataDisponibilidade
            // 
            this.DataDisponibilidade.HeaderText = "Data da Disponibilidade";
            this.DataDisponibilidade.MinimumWidth = 6;
            this.DataDisponibilidade.Name = "DataDisponibilidade";
            // 
            // DataLimiteRetirada
            // 
            this.DataLimiteRetirada.HeaderText = "Data Limite para Retirada";
            this.DataLimiteRetirada.MinimumWidth = 6;
            this.DataLimiteRetirada.Name = "DataLimiteRetirada";
            // 
            // StatusReserva
            // 
            this.StatusReserva.HeaderText = "Status";
            this.StatusReserva.MinimumWidth = 6;
            this.StatusReserva.Name = "StatusReserva";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(959, 269);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(49, 20);
            this.lblStatus.TabIndex = 27;
            this.lblStatus.Text = "Status";
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(963, 304);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(194, 27);
            this.txtStatus.TabIndex = 26;
            // 
            // lblDataReserva
            // 
            this.lblDataReserva.AutoSize = true;
            this.lblDataReserva.Location = new System.Drawing.Point(959, 413);
            this.lblDataReserva.Name = "lblDataReserva";
            this.lblDataReserva.Size = new System.Drawing.Size(117, 20);
            this.lblDataReserva.TabIndex = 41;
            this.lblDataReserva.Text = "Data da Reserva";
            // 
            // txtIdLivroReservado
            // 
            this.txtIdLivroReservado.Location = new System.Drawing.Point(963, 287);
            this.txtIdLivroReservado.Name = "txtIdLivroReservado";
            this.txtIdLivroReservado.Size = new System.Drawing.Size(194, 27);
            this.txtIdLivroReservado.TabIndex = 38;
            // 
            // lblIdLivroReservado
            // 
            this.lblIdLivroReservado.AutoSize = true;
            this.lblIdLivroReservado.Location = new System.Drawing.Point(959, 255);
            this.lblIdLivroReservado.Name = "lblIdLivroReservado";
            this.lblIdLivroReservado.Size = new System.Drawing.Size(54, 20);
            this.lblIdLivroReservado.TabIndex = 39;
            this.lblIdLivroReservado.Text = "IdLivro";
            // 
            // txtIdUsuarioReservista
            // 
            this.txtIdUsuarioReservista.Location = new System.Drawing.Point(963, 212);
            this.txtIdUsuarioReservista.Name = "txtIdUsuarioReservista";
            this.txtIdUsuarioReservista.Size = new System.Drawing.Size(194, 27);
            this.txtIdUsuarioReservista.TabIndex = 34;
            // 
            // lblIdUsuarioReservista
            // 
            this.lblIdUsuarioReservista.AutoSize = true;
            this.lblIdUsuarioReservista.Location = new System.Drawing.Point(959, 180);
            this.lblIdUsuarioReservista.Name = "lblIdUsuarioReservista";
            this.lblIdUsuarioReservista.Size = new System.Drawing.Size(72, 20);
            this.lblIdUsuarioReservista.TabIndex = 35;
            this.lblIdUsuarioReservista.Text = "IdUsuario";
            // 
            // Filtrar4
            // 
            this.Filtrar4.Location = new System.Drawing.Point(956, 3);
            this.Filtrar4.Name = "Filtrar4";
            this.Filtrar4.Size = new System.Drawing.Size(213, 59);
            this.Filtrar4.TabIndex = 33;
            this.Filtrar4.Text = "Filtrar";
            this.Filtrar4.UseVisualStyleBackColor = true;
            // 
            // lblIdReserva
            // 
            this.lblIdReserva.AutoSize = true;
            this.lblIdReserva.Location = new System.Drawing.Point(959, 99);
            this.lblIdReserva.Name = "lblIdReserva";
            this.lblIdReserva.Size = new System.Drawing.Size(73, 20);
            this.lblIdReserva.TabIndex = 37;
            this.lblIdReserva.Text = "IdReserva";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(963, 134);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(194, 27);
            this.textBox4.TabIndex = 36;
            // 
            // txtDataReserva
            // 
            this.txtDataReserva.Location = new System.Drawing.Point(963, 436);
            this.txtDataReserva.Mask = "99/99/9999";
            this.txtDataReserva.Name = "txtDataReserva";
            this.txtDataReserva.Size = new System.Drawing.Size(75, 27);
            this.txtDataReserva.TabIndex = 42;
            // 
            // txtDataDisponibilidade
            // 
            this.txtDataDisponibilidade.Location = new System.Drawing.Point(963, 514);
            this.txtDataDisponibilidade.Mask = "99/99/9999";
            this.txtDataDisponibilidade.Name = "txtDataDisponibilidade";
            this.txtDataDisponibilidade.Size = new System.Drawing.Size(75, 27);
            this.txtDataDisponibilidade.TabIndex = 44;
            // 
            // lblDataDisponibilidade
            // 
            this.lblDataDisponibilidade.AutoSize = true;
            this.lblDataDisponibilidade.Location = new System.Drawing.Point(956, 482);
            this.lblDataDisponibilidade.Name = "lblDataDisponibilidade";
            this.lblDataDisponibilidade.Size = new System.Drawing.Size(172, 20);
            this.lblDataDisponibilidade.TabIndex = 43;
            this.lblDataDisponibilidade.Text = "Data da Disponibilidade";
            // 
            // txtStatusReserva
            // 
            this.txtStatusReserva.Location = new System.Drawing.Point(963, 363);
            this.txtStatusReserva.Name = "txtStatusReserva";
            this.txtStatusReserva.Size = new System.Drawing.Size(194, 27);
            this.txtStatusReserva.TabIndex = 45;
            // 
            // lblStatusReserva
            // 
            this.lblStatusReserva.AutoSize = true;
            this.lblStatusReserva.Location = new System.Drawing.Point(959, 331);
            this.lblStatusReserva.Name = "lblStatusReserva";
            this.lblStatusReserva.Size = new System.Drawing.Size(49, 20);
            this.lblStatusReserva.TabIndex = 46;
            this.lblStatusReserva.Text = "Status";
            // 
            // RelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 873);
            this.Controls.Add(this.materialTabControl1);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.lblRelatorios);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "RelForm";
            this.Text = "InicioForm";
            this.Load += new System.EventHandler(this.RelForm_Load);
            this.materialTabControl1.ResumeLayout(false);
            this.tabPageEmprestimos.ResumeLayout(false);
            this.tabPageEmprestimos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lista)).EndInit();
            this.tabPageUsuarios.ResumeLayout(false);
            this.tabPageUsuarios.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lista2)).EndInit();
            this.tabPageLivros.ResumeLayout(false);
            this.tabPageLivros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lista3)).EndInit();
            this.tabPageReservas.ResumeLayout(false);
            this.tabPageReservas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lista4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRelatorios;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Label lblIdUsuario;
        private System.Windows.Forms.TextBox txtIdUsuario;
        private System.Windows.Forms.Label lblIdLivro;
        private System.Windows.Forms.TextBox txtIdLivro;
        private MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage tabPageEmprestimos;
        private System.Windows.Forms.DataGridView lista;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdEmpréstimo;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdLivro;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdUsuário;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataDevolucao;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataEmprestimo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.TabPage tabPageUsuarios;
        private System.Windows.Forms.TabPage tabPageLivros;
        private System.Windows.Forms.TabPage tabPageReservas;
        private System.Windows.Forms.DataGridView lista2;
        private System.Windows.Forms.DataGridView lista3;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDUsuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn NomeUsuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn CPF;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataNascimento;
        private System.Windows.Forms.DataGridViewTextBoxColumn Turma;
        private System.Windows.Forms.DataGridViewTextBoxColumn Telefone;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoUsuario;
        private System.Windows.Forms.DataGridView lista4;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdL;
        private System.Windows.Forms.DataGridViewTextBoxColumn NomeLivro;
        private System.Windows.Forms.DataGridViewTextBoxColumn Genero;
        private System.Windows.Forms.DataGridViewTextBoxColumn Autor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodigoBarras;
        private System.Windows.Forms.DataGridViewTextBoxColumn Disponibilidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdReserva;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsuarioId;
        private System.Windows.Forms.DataGridViewTextBoxColumn LivroId;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdBibliotecaria;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataReserva;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataDisponibilidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataLimiteRetirada;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusReserva;
        private System.Windows.Forms.TextBox txtNomeU;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.Button btnFiltrar2;
        private System.Windows.Forms.Label lblIdU;
        private System.Windows.Forms.TextBox txtIdU;
        private System.Windows.Forms.TextBox txtTurma;
        private System.Windows.Forms.Label lblTurma;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtAutor;
        private System.Windows.Forms.Label lblAutor;
        private System.Windows.Forms.Button btnFiltrar3;
        private System.Windows.Forms.TextBox txtGenero;
        private System.Windows.Forms.TextBox txtIdLivroL;
        private System.Windows.Forms.Label lblGenero;
        private System.Windows.Forms.Label lblLivroId;
        private System.Windows.Forms.TextBox txtNomeL;
        private System.Windows.Forms.Label lblNomeL;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.MaskedTextBox txtDataReserva;
        private System.Windows.Forms.Label lblDataReserva;
        private System.Windows.Forms.Button Filtrar4;
        private System.Windows.Forms.TextBox txtIdLivroReservado;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label lblIdLivroReservado;
        private System.Windows.Forms.Label lblIdReserva;
        private System.Windows.Forms.TextBox txtIdUsuarioReservista;
        private System.Windows.Forms.Label lblIdUsuarioReservista;
        private System.Windows.Forms.TextBox txtStatusReserva;
        private System.Windows.Forms.Label lblStatusReserva;
        private System.Windows.Forms.MaskedTextBox txtDataDisponibilidade;
        private System.Windows.Forms.Label lblDataDisponibilidade;
    }
}