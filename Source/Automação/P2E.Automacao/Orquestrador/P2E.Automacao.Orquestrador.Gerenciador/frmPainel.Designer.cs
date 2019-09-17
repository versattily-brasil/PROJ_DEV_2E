namespace P2E.Automacao.Orquestrador.Gerenciador
{
    partial class frmPainel
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtData = new System.Windows.Forms.DateTimePicker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gvAgendamentos = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dTDATAEXECPROGDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hRHORAEXECPROGDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oPULTIMOSTATUSEXECDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDULTIMAEXECDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.botsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.agendaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gvAgendaBots = new System.Windows.Forms.DataGridView();
            this.bsAgendaBots = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.bsBotExecLog = new System.Windows.Forms.BindingSource(this.components);
            this.barraStatus = new System.Windows.Forms.StatusStrip();
            this.barraProgresso = new System.Windows.Forms.ToolStripProgressBar();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.btnExecutarAgenda = new System.Windows.Forms.Button();
            this.bgwConsultar = new System.ComponentModel.BackgroundWorker();
            this.agendaBotBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cDAGENDABOTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDBOTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tXPARAMEXECDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.agendaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nRORDEMEXECDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescBot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAgendamentos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.agendaBindingSource)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAgendaBots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsAgendaBots)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsBotExecLog)).BeginInit();
            this.barraStatus.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.agendaBotBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(977, 70);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Data";
            // 
            // txtData
            // 
            this.txtData.CustomFormat = "dd/MM/yyyy";
            this.txtData.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txtData.Location = new System.Drawing.Point(16, 29);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(200, 20);
            this.txtData.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(977, 186);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gvAgendamentos);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(969, 160);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Agendamentos";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gvAgendamentos
            // 
            this.gvAgendamentos.AllowUserToAddRows = false;
            this.gvAgendamentos.AllowUserToDeleteRows = false;
            this.gvAgendamentos.AllowUserToOrderColumns = true;
            this.gvAgendamentos.AutoGenerateColumns = false;
            this.gvAgendamentos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvAgendamentos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.gvAgendamentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAgendamentos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dTDATAEXECPROGDataGridViewTextBoxColumn,
            this.hRHORAEXECPROGDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn7,
            this.oPULTIMOSTATUSEXECDataGridViewTextBoxColumn,
            this.cDULTIMAEXECDataGridViewTextBoxColumn,
            this.botsDataGridViewTextBoxColumn});
            this.gvAgendamentos.DataSource = this.agendaBindingSource;
            this.gvAgendamentos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvAgendamentos.Location = new System.Drawing.Point(3, 3);
            this.gvAgendamentos.MultiSelect = false;
            this.gvAgendamentos.Name = "gvAgendamentos";
            this.gvAgendamentos.ReadOnly = true;
            this.gvAgendamentos.RowHeadersWidth = 51;
            this.gvAgendamentos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvAgendamentos.Size = new System.Drawing.Size(963, 154);
            this.gvAgendamentos.TabIndex = 0;
            this.gvAgendamentos.SelectionChanged += new System.EventHandler(this.GvAgendamentos_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CD_AGENDA";
            this.dataGridViewTextBoxColumn1.HeaderText = "#";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "TX_DESCRICAO";
            this.dataGridViewTextBoxColumn2.HeaderText = "Descrição";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "OP_ATIVO";
            this.dataGridViewTextBoxColumn3.HeaderText = "Ativo";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "OP_FORMA_EXEC";
            this.dataGridViewTextBoxColumn4.HeaderText = "Forma Exec.";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "OP_REPETE";
            this.dataGridViewTextBoxColumn5.HeaderText = "Repete?";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "OP_TIPO_REP";
            this.dataGridViewTextBoxColumn6.HeaderText = "Tipo Repetição";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dTDATAEXECPROGDataGridViewTextBoxColumn
            // 
            this.dTDATAEXECPROGDataGridViewTextBoxColumn.DataPropertyName = "DT_DATA_EXEC_PROG";
            this.dTDATAEXECPROGDataGridViewTextBoxColumn.HeaderText = "Data Programada";
            this.dTDATAEXECPROGDataGridViewTextBoxColumn.Name = "dTDATAEXECPROGDataGridViewTextBoxColumn";
            this.dTDATAEXECPROGDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // hRHORAEXECPROGDataGridViewTextBoxColumn
            // 
            this.hRHORAEXECPROGDataGridViewTextBoxColumn.DataPropertyName = "HR_HORA_EXEC_PROG";
            this.hRHORAEXECPROGDataGridViewTextBoxColumn.HeaderText = "Hora Programada";
            this.hRHORAEXECPROGDataGridViewTextBoxColumn.Name = "hRHORAEXECPROGDataGridViewTextBoxColumn";
            this.hRHORAEXECPROGDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "DT_DATA_ULTIMA_EXEC";
            this.dataGridViewTextBoxColumn7.HeaderText = "DT_DATA_ULTIMA_EXEC";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // oPULTIMOSTATUSEXECDataGridViewTextBoxColumn
            // 
            this.oPULTIMOSTATUSEXECDataGridViewTextBoxColumn.DataPropertyName = "OP_ULTIMO_STATUS_EXEC";
            this.oPULTIMOSTATUSEXECDataGridViewTextBoxColumn.HeaderText = "Status";
            this.oPULTIMOSTATUSEXECDataGridViewTextBoxColumn.Name = "oPULTIMOSTATUSEXECDataGridViewTextBoxColumn";
            this.oPULTIMOSTATUSEXECDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cDULTIMAEXECDataGridViewTextBoxColumn
            // 
            this.cDULTIMAEXECDataGridViewTextBoxColumn.DataPropertyName = "CD_ULTIMA_EXEC";
            this.cDULTIMAEXECDataGridViewTextBoxColumn.HeaderText = "CD_ULTIMA_EXEC";
            this.cDULTIMAEXECDataGridViewTextBoxColumn.Name = "cDULTIMAEXECDataGridViewTextBoxColumn";
            this.cDULTIMAEXECDataGridViewTextBoxColumn.ReadOnly = true;
            this.cDULTIMAEXECDataGridViewTextBoxColumn.Visible = false;
            // 
            // botsDataGridViewTextBoxColumn
            // 
            this.botsDataGridViewTextBoxColumn.DataPropertyName = "Bots";
            this.botsDataGridViewTextBoxColumn.HeaderText = "Bots";
            this.botsDataGridViewTextBoxColumn.Name = "botsDataGridViewTextBoxColumn";
            this.botsDataGridViewTextBoxColumn.ReadOnly = true;
            this.botsDataGridViewTextBoxColumn.Visible = false;
            // 
            // agendaBindingSource
            // 
            this.agendaBindingSource.DataSource = typeof(P2E.Automacao.Orquestrador.Lib.Entidades.Agenda);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(977, 186);
            this.panel2.TabIndex = 2;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 256);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(977, 284);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(969, 258);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Bots do Agendamento";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gvAgendaBots);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(963, 252);
            this.splitContainer1.SplitterDistance = 649;
            this.splitContainer1.TabIndex = 2;
            // 
            // gvAgendaBots
            // 
            this.gvAgendaBots.AllowUserToAddRows = false;
            this.gvAgendaBots.AllowUserToDeleteRows = false;
            this.gvAgendaBots.AutoGenerateColumns = false;
            this.gvAgendaBots.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAgendaBots.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cDAGENDABOTDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn8,
            this.cDBOTDataGridViewTextBoxColumn,
            this.tXPARAMEXECDataGridViewTextBoxColumn,
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn,
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn,
            this.agendaDataGridViewTextBoxColumn,
            this.nRORDEMEXECDataGridViewTextBoxColumn,
            this.colDescBot,
            this.statusDataGridViewTextBoxColumn});
            this.gvAgendaBots.DataSource = this.bsAgendaBots;
            this.gvAgendaBots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvAgendaBots.Location = new System.Drawing.Point(0, 0);
            this.gvAgendaBots.Name = "gvAgendaBots";
            this.gvAgendaBots.ReadOnly = true;
            this.gvAgendaBots.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvAgendaBots.Size = new System.Drawing.Size(649, 252);
            this.gvAgendaBots.TabIndex = 0;
            this.gvAgendaBots.SelectionChanged += new System.EventHandler(this.GvAgendaBots_SelectionChanged);
            // 
            // bsAgendaBots
            // 
            this.bsAgendaBots.DataSource = typeof(P2E.Automacao.Orquestrador.Lib.Entidades.AgendaBot);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 252);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log de Execução";
            // 
            // listBox1
            // 
            this.listBox1.DataSource = this.bsBotExecLog;
            this.listBox1.DisplayMember = "TX_MENSAGEM";
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 16);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(304, 233);
            this.listBox1.TabIndex = 0;
            this.listBox1.ValueMember = "CD_BOT_EXEC_LOG";
            // 
            // bsBotExecLog
            // 
            this.bsBotExecLog.DataSource = typeof(P2E.Automacao.Orquestrador.Lib.Entidades.BotExecLog);
            // 
            // barraStatus
            // 
            this.barraStatus.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.barraStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barraProgresso});
            this.barraStatus.Location = new System.Drawing.Point(0, 518);
            this.barraStatus.Name = "barraStatus";
            this.barraStatus.Size = new System.Drawing.Size(977, 22);
            this.barraStatus.TabIndex = 4;
            this.barraStatus.Text = "statusStrip1";
            // 
            // barraProgresso
            // 
            this.barraProgresso.Name = "barraProgresso";
            this.barraProgresso.Size = new System.Drawing.Size(100, 16);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnConsultar);
            this.panel3.Controls.Add(this.btnExecutarAgenda);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 478);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(977, 40);
            this.panel3.TabIndex = 5;
            // 
            // btnConsultar
            // 
            this.btnConsultar.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnConsultar.Location = new System.Drawing.Point(0, 0);
            this.btnConsultar.Margin = new System.Windows.Forms.Padding(0);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(98, 40);
            this.btnConsultar.TabIndex = 2;
            this.btnConsultar.Text = "Consultar";
            this.btnConsultar.UseVisualStyleBackColor = true;
            this.btnConsultar.Click += new System.EventHandler(this.BtnConsultar_Click);
            // 
            // btnExecutarAgenda
            // 
            this.btnExecutarAgenda.Location = new System.Drawing.Point(98, 0);
            this.btnExecutarAgenda.Name = "btnExecutarAgenda";
            this.btnExecutarAgenda.Size = new System.Drawing.Size(87, 40);
            this.btnExecutarAgenda.TabIndex = 0;
            this.btnExecutarAgenda.Text = "Executar";
            this.btnExecutarAgenda.UseVisualStyleBackColor = true;
            this.btnExecutarAgenda.Click += new System.EventHandler(this.BtnExecutarAgenda_Click);
            // 
            // bgwConsultar
            // 
            this.bgwConsultar.WorkerReportsProgress = true;
            this.bgwConsultar.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BgwConsultar_DoWork);
            this.bgwConsultar.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgwConsultar_RunWorkerCompleted);
            // 
            // agendaBotBindingSource
            // 
            this.agendaBotBindingSource.DataSource = typeof(P2E.Automacao.Orquestrador.Lib.Entidades.AgendaBot);
            // 
            // cDAGENDABOTDataGridViewTextBoxColumn
            // 
            this.cDAGENDABOTDataGridViewTextBoxColumn.DataPropertyName = "CD_AGENDA_BOT";
            this.cDAGENDABOTDataGridViewTextBoxColumn.HeaderText = "CD_AGENDA_BOT";
            this.cDAGENDABOTDataGridViewTextBoxColumn.Name = "cDAGENDABOTDataGridViewTextBoxColumn";
            this.cDAGENDABOTDataGridViewTextBoxColumn.ReadOnly = true;
            this.cDAGENDABOTDataGridViewTextBoxColumn.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "CD_AGENDA";
            this.dataGridViewTextBoxColumn8.HeaderText = "CD_AGENDA";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // cDBOTDataGridViewTextBoxColumn
            // 
            this.cDBOTDataGridViewTextBoxColumn.DataPropertyName = "CD_BOT";
            this.cDBOTDataGridViewTextBoxColumn.HeaderText = "CD_BOT";
            this.cDBOTDataGridViewTextBoxColumn.Name = "cDBOTDataGridViewTextBoxColumn";
            this.cDBOTDataGridViewTextBoxColumn.ReadOnly = true;
            this.cDBOTDataGridViewTextBoxColumn.Visible = false;
            // 
            // tXPARAMEXECDataGridViewTextBoxColumn
            // 
            this.tXPARAMEXECDataGridViewTextBoxColumn.DataPropertyName = "TX_PARAM_EXEC";
            this.tXPARAMEXECDataGridViewTextBoxColumn.HeaderText = "TX_PARAM_EXEC";
            this.tXPARAMEXECDataGridViewTextBoxColumn.Name = "tXPARAMEXECDataGridViewTextBoxColumn";
            this.tXPARAMEXECDataGridViewTextBoxColumn.ReadOnly = true;
            this.tXPARAMEXECDataGridViewTextBoxColumn.Visible = false;
            // 
            // cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn
            // 
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn.DataPropertyName = "CD_ULTIMO_STATUS_EXEC_BOT";
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn.HeaderText = "CD_ULTIMO_STATUS_EXEC_BOT";
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn.Name = "cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn";
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn.ReadOnly = true;
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn.Visible = false;
            // 
            // cDULTIMAEXECBOTDataGridViewTextBoxColumn
            // 
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn.DataPropertyName = "CD_ULTIMA_EXEC_BOT";
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn.HeaderText = "CD_ULTIMA_EXEC_BOT";
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn.Name = "cDULTIMAEXECBOTDataGridViewTextBoxColumn";
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn.ReadOnly = true;
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn.Visible = false;
            // 
            // agendaDataGridViewTextBoxColumn
            // 
            this.agendaDataGridViewTextBoxColumn.DataPropertyName = "Agenda";
            this.agendaDataGridViewTextBoxColumn.HeaderText = "Agenda";
            this.agendaDataGridViewTextBoxColumn.Name = "agendaDataGridViewTextBoxColumn";
            this.agendaDataGridViewTextBoxColumn.ReadOnly = true;
            this.agendaDataGridViewTextBoxColumn.Visible = false;
            // 
            // nRORDEMEXECDataGridViewTextBoxColumn
            // 
            this.nRORDEMEXECDataGridViewTextBoxColumn.DataPropertyName = "NR_ORDEM_EXEC";
            this.nRORDEMEXECDataGridViewTextBoxColumn.HeaderText = "Ordem Exec.";
            this.nRORDEMEXECDataGridViewTextBoxColumn.Name = "nRORDEMEXECDataGridViewTextBoxColumn";
            this.nRORDEMEXECDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // colDescBot
            // 
            this.colDescBot.DataPropertyName = "DescBot";
            this.colDescBot.HeaderText = "Descrição";
            this.colDescBot.Name = "colDescBot";
            this.colDescBot.ReadOnly = true;
            this.colDescBot.Width = 400;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "CD_ULTIMO_STATUS_EXEC_BOT";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // frmPainel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 540);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.barraStatus);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmPainel";
            this.Text = "Painel de Agendamentos";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAgendamentos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.agendaBindingSource)).EndInit();
            this.panel2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAgendaBots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsAgendaBots)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsBotExecLog)).EndInit();
            this.barraStatus.ResumeLayout(false);
            this.barraStatus.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.agendaBotBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker txtData;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView gvAgendamentos;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.StatusStrip barraStatus;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExecutarAgenda;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.ToolStripProgressBar barraProgresso;
        private System.ComponentModel.BackgroundWorker bgwConsultar;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDAGENDADataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tXDESCRICAODataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oPATIVODataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oPFORMAEXECDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oPREPETEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oPTIPOREPDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dTDATAEXECPLANDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hRHORAEXECPLANDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dTDATAULTIMAEXECDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oPULTIMOSTATUSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn agendaProgramadaDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource agendaBindingSource;
        private System.Windows.Forms.BindingSource agendaBotBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn nomeBotDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descBotDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView gvAgendaBots;
        private System.Windows.Forms.BindingSource bsAgendaBots;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dTDATAEXECPROGDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hRHORAEXECPROGDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn oPULTIMOSTATUSEXECDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDULTIMAEXECDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn botsDataGridViewTextBoxColumn;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.BindingSource bsBotExecLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDAGENDABOTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDBOTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tXPARAMEXECDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDULTIMAEXECBOTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn agendaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nRORDEMEXECDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescBot;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
    }
}