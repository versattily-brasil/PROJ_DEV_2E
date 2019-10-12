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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPainel));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.btnExecutarAgenda = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gvAgendamentos = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sAtivo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sREPETE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tipoRepeticao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dTDATAEXECPROGDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hRHORAEXECPROGDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDULTIMAEXECDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.botsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OP_STATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DT_DATA_INICIO_ULTIMA_EXEC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DT_DATA_FIM_ULTIMA_EXEC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuAgenda = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.interromperExecuçãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alterarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.agendaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gvAgendaBots = new System.Windows.Forms.DataGridView();
            this.cDAGENDABOTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescBot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDBOTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tXPARAMEXECDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.agendaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nRORDEMEXECDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescBot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsAgendaBots = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.bsBotExecLog = new System.Windows.Forms.BindingSource(this.components);
            this.barraStatus = new System.Windows.Forms.StatusStrip();
            this.barraProgresso = new System.Windows.Forms.ToolStripProgressBar();
            this.bgwConsultar = new System.ComponentModel.BackgroundWorker();
            this.agendaBotBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAgendamentos)).BeginInit();
            this.menuAgenda.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.agendaBotBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnConsultar);
            this.panel1.Controls.Add(this.btnExecutarAgenda);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1390, 55);
            this.panel1.TabIndex = 0;
            // 
            // btnConsultar
            // 
            this.btnConsultar.Image = ((System.Drawing.Image)(resources.GetObject("btnConsultar.Image")));
            this.btnConsultar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnConsultar.Location = new System.Drawing.Point(0, 1);
            this.btnConsultar.Margin = new System.Windows.Forms.Padding(0);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(223, 49);
            this.btnConsultar.TabIndex = 2;
            this.btnConsultar.Text = "Consultar";
            this.btnConsultar.UseVisualStyleBackColor = true;
            this.btnConsultar.Click += new System.EventHandler(this.BtnConsultar_Click);
            // 
            // btnExecutarAgenda
            // 
            this.btnExecutarAgenda.Image = ((System.Drawing.Image)(resources.GetObject("btnExecutarAgenda.Image")));
            this.btnExecutarAgenda.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExecutarAgenda.Location = new System.Drawing.Point(225, 1);
            this.btnExecutarAgenda.Margin = new System.Windows.Forms.Padding(4);
            this.btnExecutarAgenda.Name = "btnExecutarAgenda";
            this.btnExecutarAgenda.Size = new System.Drawing.Size(222, 49);
            this.btnExecutarAgenda.TabIndex = 0;
            this.btnExecutarAgenda.Text = "Enviar para Execução";
            this.btnExecutarAgenda.UseVisualStyleBackColor = true;
            this.btnExecutarAgenda.Click += new System.EventHandler(this.BtnExecutarAgenda_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1390, 422);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gvAgendamentos);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1382, 393);
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
            this.gvAgendamentos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.gvAgendamentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAgendamentos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.sAtivo,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.sREPETE,
            this.tipoRepeticao,
            this.dataGridViewTextBoxColumn6,
            this.dTDATAEXECPROGDataGridViewTextBoxColumn,
            this.hRHORAEXECPROGDataGridViewTextBoxColumn,
            this.cDULTIMAEXECDataGridViewTextBoxColumn,
            this.botsDataGridViewTextBoxColumn,
            this.OP_STATUS,
            this.Status,
            this.DT_DATA_INICIO_ULTIMA_EXEC,
            this.DT_DATA_FIM_ULTIMA_EXEC});
            this.gvAgendamentos.ContextMenuStrip = this.menuAgenda;
            this.gvAgendamentos.DataSource = this.agendaBindingSource;
            this.gvAgendamentos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvAgendamentos.Location = new System.Drawing.Point(4, 4);
            this.gvAgendamentos.Margin = new System.Windows.Forms.Padding(4);
            this.gvAgendamentos.MultiSelect = false;
            this.gvAgendamentos.Name = "gvAgendamentos";
            this.gvAgendamentos.ReadOnly = true;
            this.gvAgendamentos.RowHeadersWidth = 51;
            this.gvAgendamentos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvAgendamentos.Size = new System.Drawing.Size(1374, 385);
            this.gvAgendamentos.TabIndex = 0;
            this.gvAgendamentos.SelectionChanged += new System.EventHandler(this.GvAgendamentos_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CD_AGENDA";
            this.dataGridViewTextBoxColumn1.FillWeight = 160.4278F;
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "#";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 25;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "TX_DESCRICAO";
            this.dataGridViewTextBoxColumn2.FillWeight = 93.2858F;
            this.dataGridViewTextBoxColumn2.Frozen = true;
            this.dataGridViewTextBoxColumn2.HeaderText = "Descrição";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 400;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "OP_ATIVO";
            this.dataGridViewTextBoxColumn3.Frozen = true;
            this.dataGridViewTextBoxColumn3.HeaderText = "Ativo";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            this.dataGridViewTextBoxColumn3.Width = 125;
            // 
            // sAtivo
            // 
            this.sAtivo.DataPropertyName = "sAtivo";
            this.sAtivo.FillWeight = 93.2858F;
            this.sAtivo.Frozen = true;
            this.sAtivo.HeaderText = "Ativo";
            this.sAtivo.MinimumWidth = 6;
            this.sAtivo.Name = "sAtivo";
            this.sAtivo.ReadOnly = true;
            this.sAtivo.Width = 50;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "OP_FORMA_EXEC";
            this.dataGridViewTextBoxColumn4.FillWeight = 93.2858F;
            this.dataGridViewTextBoxColumn4.Frozen = true;
            this.dataGridViewTextBoxColumn4.HeaderText = "Forma Exec.";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Visible = false;
            this.dataGridViewTextBoxColumn4.Width = 150;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "OP_REPETE";
            this.dataGridViewTextBoxColumn5.Frozen = true;
            this.dataGridViewTextBoxColumn5.HeaderText = "Repete?";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            this.dataGridViewTextBoxColumn5.Width = 125;
            // 
            // sREPETE
            // 
            this.sREPETE.DataPropertyName = "sREPETE";
            this.sREPETE.FillWeight = 93.2858F;
            this.sREPETE.Frozen = true;
            this.sREPETE.HeaderText = "Repete?";
            this.sREPETE.MinimumWidth = 6;
            this.sREPETE.Name = "sREPETE";
            this.sREPETE.ReadOnly = true;
            this.sREPETE.Width = 125;
            // 
            // tipoRepeticao
            // 
            this.tipoRepeticao.DataPropertyName = "tipoRepeticao";
            this.tipoRepeticao.Frozen = true;
            this.tipoRepeticao.HeaderText = "Tipo Repetição";
            this.tipoRepeticao.MinimumWidth = 6;
            this.tipoRepeticao.Name = "tipoRepeticao";
            this.tipoRepeticao.ReadOnly = true;
            this.tipoRepeticao.Width = 125;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "OP_TIPO_REP";
            this.dataGridViewTextBoxColumn6.FillWeight = 93.2858F;
            this.dataGridViewTextBoxColumn6.Frozen = true;
            this.dataGridViewTextBoxColumn6.HeaderText = "Tipo Repetição";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            this.dataGridViewTextBoxColumn6.Width = 150;
            // 
            // dTDATAEXECPROGDataGridViewTextBoxColumn
            // 
            this.dTDATAEXECPROGDataGridViewTextBoxColumn.DataPropertyName = "DT_DATA_EXEC_PROG";
            this.dTDATAEXECPROGDataGridViewTextBoxColumn.FillWeight = 93.2858F;
            this.dTDATAEXECPROGDataGridViewTextBoxColumn.Frozen = true;
            this.dTDATAEXECPROGDataGridViewTextBoxColumn.HeaderText = "Data Programada";
            this.dTDATAEXECPROGDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.dTDATAEXECPROGDataGridViewTextBoxColumn.Name = "dTDATAEXECPROGDataGridViewTextBoxColumn";
            this.dTDATAEXECPROGDataGridViewTextBoxColumn.ReadOnly = true;
            this.dTDATAEXECPROGDataGridViewTextBoxColumn.Width = 150;
            // 
            // hRHORAEXECPROGDataGridViewTextBoxColumn
            // 
            this.hRHORAEXECPROGDataGridViewTextBoxColumn.DataPropertyName = "HR_HORA_EXEC_PROG";
            dataGridViewCellStyle1.Format = "T";
            dataGridViewCellStyle1.NullValue = null;
            this.hRHORAEXECPROGDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.hRHORAEXECPROGDataGridViewTextBoxColumn.FillWeight = 93.2858F;
            this.hRHORAEXECPROGDataGridViewTextBoxColumn.Frozen = true;
            this.hRHORAEXECPROGDataGridViewTextBoxColumn.HeaderText = "Hora Programada";
            this.hRHORAEXECPROGDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.hRHORAEXECPROGDataGridViewTextBoxColumn.Name = "hRHORAEXECPROGDataGridViewTextBoxColumn";
            this.hRHORAEXECPROGDataGridViewTextBoxColumn.ReadOnly = true;
            this.hRHORAEXECPROGDataGridViewTextBoxColumn.Width = 150;
            // 
            // cDULTIMAEXECDataGridViewTextBoxColumn
            // 
            this.cDULTIMAEXECDataGridViewTextBoxColumn.DataPropertyName = "CD_ULTIMA_EXEC";
            this.cDULTIMAEXECDataGridViewTextBoxColumn.Frozen = true;
            this.cDULTIMAEXECDataGridViewTextBoxColumn.HeaderText = "CD_ULTIMA_EXEC";
            this.cDULTIMAEXECDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.cDULTIMAEXECDataGridViewTextBoxColumn.Name = "cDULTIMAEXECDataGridViewTextBoxColumn";
            this.cDULTIMAEXECDataGridViewTextBoxColumn.ReadOnly = true;
            this.cDULTIMAEXECDataGridViewTextBoxColumn.Visible = false;
            this.cDULTIMAEXECDataGridViewTextBoxColumn.Width = 125;
            // 
            // botsDataGridViewTextBoxColumn
            // 
            this.botsDataGridViewTextBoxColumn.DataPropertyName = "Bots";
            this.botsDataGridViewTextBoxColumn.Frozen = true;
            this.botsDataGridViewTextBoxColumn.HeaderText = "Bots";
            this.botsDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.botsDataGridViewTextBoxColumn.Name = "botsDataGridViewTextBoxColumn";
            this.botsDataGridViewTextBoxColumn.ReadOnly = true;
            this.botsDataGridViewTextBoxColumn.Visible = false;
            this.botsDataGridViewTextBoxColumn.Width = 125;
            // 
            // OP_STATUS
            // 
            this.OP_STATUS.DataPropertyName = "OP_STATUS";
            this.OP_STATUS.FillWeight = 93.2858F;
            this.OP_STATUS.Frozen = true;
            this.OP_STATUS.HeaderText = "Status";
            this.OP_STATUS.MinimumWidth = 6;
            this.OP_STATUS.Name = "OP_STATUS";
            this.OP_STATUS.ReadOnly = true;
            this.OP_STATUS.Visible = false;
            this.OP_STATUS.Width = 115;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            this.Status.Frozen = true;
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 125;
            // 
            // DT_DATA_INICIO_ULTIMA_EXEC
            // 
            this.DT_DATA_INICIO_ULTIMA_EXEC.DataPropertyName = "DT_DATA_INICIO_ULTIMA_EXEC";
            dataGridViewCellStyle2.Format = "G";
            dataGridViewCellStyle2.NullValue = null;
            this.DT_DATA_INICIO_ULTIMA_EXEC.DefaultCellStyle = dataGridViewCellStyle2;
            this.DT_DATA_INICIO_ULTIMA_EXEC.Frozen = true;
            this.DT_DATA_INICIO_ULTIMA_EXEC.HeaderText = "Início Última Execução";
            this.DT_DATA_INICIO_ULTIMA_EXEC.MinimumWidth = 6;
            this.DT_DATA_INICIO_ULTIMA_EXEC.Name = "DT_DATA_INICIO_ULTIMA_EXEC";
            this.DT_DATA_INICIO_ULTIMA_EXEC.ReadOnly = true;
            this.DT_DATA_INICIO_ULTIMA_EXEC.Width = 125;
            // 
            // DT_DATA_FIM_ULTIMA_EXEC
            // 
            this.DT_DATA_FIM_ULTIMA_EXEC.DataPropertyName = "DT_DATA_FIM_ULTIMA_EXEC";
            dataGridViewCellStyle3.Format = "G";
            dataGridViewCellStyle3.NullValue = null;
            this.DT_DATA_FIM_ULTIMA_EXEC.DefaultCellStyle = dataGridViewCellStyle3;
            this.DT_DATA_FIM_ULTIMA_EXEC.Frozen = true;
            this.DT_DATA_FIM_ULTIMA_EXEC.HeaderText = "Fim Última Execução";
            this.DT_DATA_FIM_ULTIMA_EXEC.MinimumWidth = 6;
            this.DT_DATA_FIM_ULTIMA_EXEC.Name = "DT_DATA_FIM_ULTIMA_EXEC";
            this.DT_DATA_FIM_ULTIMA_EXEC.ReadOnly = true;
            this.DT_DATA_FIM_ULTIMA_EXEC.Width = 125;
            // 
            // menuAgenda
            // 
            this.menuAgenda.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuAgenda.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.interromperExecuçãoToolStripMenuItem,
            this.alterarToolStripMenuItem});
            this.menuAgenda.Name = "contextMenuStrip1";
            this.menuAgenda.Size = new System.Drawing.Size(215, 52);
            // 
            // interromperExecuçãoToolStripMenuItem
            // 
            this.interromperExecuçãoToolStripMenuItem.Name = "interromperExecuçãoToolStripMenuItem";
            this.interromperExecuçãoToolStripMenuItem.ShowShortcutKeys = false;
            this.interromperExecuçãoToolStripMenuItem.Size = new System.Drawing.Size(214, 24);
            this.interromperExecuçãoToolStripMenuItem.Text = "Interromper Execução";
            this.interromperExecuçãoToolStripMenuItem.Click += new System.EventHandler(this.interromperExecuçãoToolStripMenuItem_Click);
            // 
            // alterarToolStripMenuItem
            // 
            this.alterarToolStripMenuItem.Name = "alterarToolStripMenuItem";
            this.alterarToolStripMenuItem.Size = new System.Drawing.Size(214, 24);
            this.alterarToolStripMenuItem.Text = "Alterar";
            this.alterarToolStripMenuItem.Click += new System.EventHandler(this.alterarToolStripMenuItem_Click);
            // 
            // agendaBindingSource
            // 
            this.agendaBindingSource.DataSource = typeof(P2E.Automacao.Orquestrador.Lib.Entidades.Agenda);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 55);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1390, 422);
            this.panel2.TabIndex = 2;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 477);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1390, 188);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1382, 159);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Bots do Agendamento";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.gvAgendaBots);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1374, 151);
            this.splitContainer1.SplitterDistance = 707;
            this.splitContainer1.SplitterWidth = 11;
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
            this.DescBot,
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
            this.gvAgendaBots.Margin = new System.Windows.Forms.Padding(4);
            this.gvAgendaBots.Name = "gvAgendaBots";
            this.gvAgendaBots.ReadOnly = true;
            this.gvAgendaBots.RowHeadersWidth = 51;
            this.gvAgendaBots.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvAgendaBots.Size = new System.Drawing.Size(707, 151);
            this.gvAgendaBots.StandardTab = true;
            this.gvAgendaBots.TabIndex = 0;
            this.gvAgendaBots.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvAgendaBots_CellContentClick);
            this.gvAgendaBots.SelectionChanged += new System.EventHandler(this.GvAgendaBots_SelectionChanged);
            this.gvAgendaBots.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gvAgendaBots_MouseClick);
            // 
            // cDAGENDABOTDataGridViewTextBoxColumn
            // 
            this.cDAGENDABOTDataGridViewTextBoxColumn.DataPropertyName = "CD_AGENDA_BOT";
            this.cDAGENDABOTDataGridViewTextBoxColumn.HeaderText = "CD_AGENDA_BOT";
            this.cDAGENDABOTDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.cDAGENDABOTDataGridViewTextBoxColumn.Name = "cDAGENDABOTDataGridViewTextBoxColumn";
            this.cDAGENDABOTDataGridViewTextBoxColumn.ReadOnly = true;
            this.cDAGENDABOTDataGridViewTextBoxColumn.Visible = false;
            this.cDAGENDABOTDataGridViewTextBoxColumn.Width = 125;
            // 
            // DescBot
            // 
            this.DescBot.DataPropertyName = "DescBot";
            this.DescBot.HeaderText = "DescBot";
            this.DescBot.MinimumWidth = 6;
            this.DescBot.Name = "DescBot";
            this.DescBot.ReadOnly = true;
            this.DescBot.Visible = false;
            this.DescBot.Width = 125;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "CD_AGENDA";
            this.dataGridViewTextBoxColumn8.HeaderText = "CD_AGENDA";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Visible = false;
            this.dataGridViewTextBoxColumn8.Width = 125;
            // 
            // cDBOTDataGridViewTextBoxColumn
            // 
            this.cDBOTDataGridViewTextBoxColumn.DataPropertyName = "CD_BOT";
            this.cDBOTDataGridViewTextBoxColumn.HeaderText = "CD_BOT";
            this.cDBOTDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.cDBOTDataGridViewTextBoxColumn.Name = "cDBOTDataGridViewTextBoxColumn";
            this.cDBOTDataGridViewTextBoxColumn.ReadOnly = true;
            this.cDBOTDataGridViewTextBoxColumn.Visible = false;
            this.cDBOTDataGridViewTextBoxColumn.Width = 125;
            // 
            // tXPARAMEXECDataGridViewTextBoxColumn
            // 
            this.tXPARAMEXECDataGridViewTextBoxColumn.DataPropertyName = "TX_PARAM_EXEC";
            this.tXPARAMEXECDataGridViewTextBoxColumn.HeaderText = "TX_PARAM_EXEC";
            this.tXPARAMEXECDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.tXPARAMEXECDataGridViewTextBoxColumn.Name = "tXPARAMEXECDataGridViewTextBoxColumn";
            this.tXPARAMEXECDataGridViewTextBoxColumn.ReadOnly = true;
            this.tXPARAMEXECDataGridViewTextBoxColumn.Visible = false;
            this.tXPARAMEXECDataGridViewTextBoxColumn.Width = 125;
            // 
            // cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn
            // 
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn.DataPropertyName = "CD_ULTIMO_STATUS_EXEC_BOT";
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn.HeaderText = "CD_ULTIMO_STATUS_EXEC_BOT";
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn.Name = "cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn";
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn.ReadOnly = true;
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn.Visible = false;
            this.cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn.Width = 125;
            // 
            // cDULTIMAEXECBOTDataGridViewTextBoxColumn
            // 
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn.DataPropertyName = "CD_ULTIMA_EXEC_BOT";
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn.HeaderText = "CD_ULTIMA_EXEC_BOT";
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn.Name = "cDULTIMAEXECBOTDataGridViewTextBoxColumn";
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn.ReadOnly = true;
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn.Visible = false;
            this.cDULTIMAEXECBOTDataGridViewTextBoxColumn.Width = 125;
            // 
            // agendaDataGridViewTextBoxColumn
            // 
            this.agendaDataGridViewTextBoxColumn.DataPropertyName = "Agenda";
            this.agendaDataGridViewTextBoxColumn.HeaderText = "Agenda";
            this.agendaDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.agendaDataGridViewTextBoxColumn.Name = "agendaDataGridViewTextBoxColumn";
            this.agendaDataGridViewTextBoxColumn.ReadOnly = true;
            this.agendaDataGridViewTextBoxColumn.Visible = false;
            this.agendaDataGridViewTextBoxColumn.Width = 125;
            // 
            // nRORDEMEXECDataGridViewTextBoxColumn
            // 
            this.nRORDEMEXECDataGridViewTextBoxColumn.DataPropertyName = "NR_ORDEM_EXEC";
            this.nRORDEMEXECDataGridViewTextBoxColumn.HeaderText = "Ordem Exec.";
            this.nRORDEMEXECDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.nRORDEMEXECDataGridViewTextBoxColumn.Name = "nRORDEMEXECDataGridViewTextBoxColumn";
            this.nRORDEMEXECDataGridViewTextBoxColumn.ReadOnly = true;
            this.nRORDEMEXECDataGridViewTextBoxColumn.Width = 125;
            // 
            // colDescBot
            // 
            this.colDescBot.DataPropertyName = "DescBot";
            this.colDescBot.HeaderText = "Descrição";
            this.colDescBot.MinimumWidth = 6;
            this.colDescBot.Name = "colDescBot";
            this.colDescBot.ReadOnly = true;
            this.colDescBot.Width = 400;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            this.statusDataGridViewTextBoxColumn.Width = 125;
            // 
            // bsAgendaBots
            // 
            this.bsAgendaBots.DataSource = typeof(P2E.Automacao.Orquestrador.Lib.Entidades.AgendaBot);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(656, 151);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log de Execução do Bot";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // listBox1
            // 
            this.listBox1.DataSource = this.bsBotExecLog;
            this.listBox1.DisplayMember = "Log";
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.Font = new System.Drawing.Font("Cambria", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 17;
            this.listBox1.Location = new System.Drawing.Point(4, 19);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(648, 128);
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
            this.barraStatus.Location = new System.Drawing.Point(0, 640);
            this.barraStatus.Name = "barraStatus";
            this.barraStatus.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.barraStatus.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.barraStatus.Size = new System.Drawing.Size(1390, 25);
            this.barraStatus.TabIndex = 4;
            this.barraStatus.Text = "statusStrip1";
            // 
            // barraProgresso
            // 
            this.barraProgresso.Name = "barraProgresso";
            this.barraProgresso.Size = new System.Drawing.Size(500, 17);
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
            // frmPainel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1390, 665);
            this.Controls.Add(this.barraStatus);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmPainel";
            this.Text = "Painel de Agendamentos";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAgendamentos)).EndInit();
            this.menuAgenda.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.agendaBindingSource)).EndInit();
            this.panel2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAgendaBots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsAgendaBots)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsBotExecLog)).EndInit();
            this.barraStatus.ResumeLayout(false);
            this.barraStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.agendaBotBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView gvAgendamentos;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.StatusStrip barraStatus;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn oPULTIMOSTATUSEXECDataGridViewTextBoxColumn;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDAGENDABOTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescBot;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDBOTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tXPARAMEXECDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDULTIMOSTATUSEXECBOTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDULTIMAEXECBOTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn agendaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nRORDEMEXECDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescBot;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        public System.Windows.Forms.BindingSource bsBotExecLog;
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn sAtivo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn sREPETE;
        private System.Windows.Forms.DataGridViewTextBoxColumn tipoRepeticao;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dTDATAEXECPROGDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hRHORAEXECPROGDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDULTIMAEXECDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn botsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn OP_STATUS;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn DT_DATA_INICIO_ULTIMA_EXEC;
        private System.Windows.Forms.DataGridViewTextBoxColumn DT_DATA_FIM_ULTIMA_EXEC;
        private System.Windows.Forms.ContextMenuStrip menuAgenda;
        private System.Windows.Forms.ToolStripMenuItem interromperExecuçãoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alterarToolStripMenuItem;
    }
}