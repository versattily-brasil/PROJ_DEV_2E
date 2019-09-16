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
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gvAgendaBots = new System.Windows.Forms.DataGridView();
            this.barraStatus = new System.Windows.Forms.StatusStrip();
            this.barraProgresso = new System.Windows.Forms.ToolStripProgressBar();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.btnPararExecAgenda = new System.Windows.Forms.Button();
            this.btnExecutarAgenda = new System.Windows.Forms.Button();
            this.bgwConsultar = new System.ComponentModel.BackgroundWorker();
            this.agendaBotBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cDAGENDADataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tXDESCRICAODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oPATIVODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oPFORMAEXECDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oPREPETEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oPTIPOREPDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dTDATAEXECPLANDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hRHORAEXECPLANDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dTDATAULTIMAEXECDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oPULTIMOSTATUSDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.agendaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cDAGENDABOTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDAGENDADataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDBOTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nRORDEMEXECDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tXPARAMEXECDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.agendaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBotDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBotStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAgendamentos)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAgendaBots)).BeginInit();
            this.barraStatus.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.agendaBotBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.agendaBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 70);
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
            this.tabControl1.Size = new System.Drawing.Size(800, 212);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gvAgendamentos);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 186);
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
            this.gvAgendamentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAgendamentos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cDAGENDADataGridViewTextBoxColumn,
            this.tXDESCRICAODataGridViewTextBoxColumn,
            this.oPATIVODataGridViewTextBoxColumn,
            this.oPFORMAEXECDataGridViewTextBoxColumn,
            this.oPREPETEDataGridViewTextBoxColumn,
            this.oPTIPOREPDataGridViewTextBoxColumn,
            this.dTDATAEXECPLANDataGridViewTextBoxColumn,
            this.hRHORAEXECPLANDataGridViewTextBoxColumn,
            this.dTDATAULTIMAEXECDataGridViewTextBoxColumn,
            this.oPULTIMOSTATUSDataGridViewTextBoxColumn});
            this.gvAgendamentos.DataSource = this.agendaBindingSource;
            this.gvAgendamentos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvAgendamentos.Location = new System.Drawing.Point(3, 3);
            this.gvAgendamentos.MultiSelect = false;
            this.gvAgendamentos.Name = "gvAgendamentos";
            this.gvAgendamentos.ReadOnly = true;
            this.gvAgendamentos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvAgendamentos.Size = new System.Drawing.Size(786, 180);
            this.gvAgendamentos.TabIndex = 0;
            this.gvAgendamentos.SelectionChanged += new System.EventHandler(this.GvAgendamentos_SelectionChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 212);
            this.panel2.TabIndex = 2;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 282);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(800, 168);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gvAgendaBots);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 142);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Bots do Agendamento";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gvAgendaBots
            // 
            this.gvAgendaBots.AllowUserToAddRows = false;
            this.gvAgendaBots.AllowUserToDeleteRows = false;
            this.gvAgendaBots.AutoGenerateColumns = false;
            this.gvAgendaBots.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAgendaBots.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cDAGENDABOTDataGridViewTextBoxColumn,
            this.cDAGENDADataGridViewTextBoxColumn1,
            this.cDBOTDataGridViewTextBoxColumn,
            this.nRORDEMEXECDataGridViewTextBoxColumn,
            this.tXPARAMEXECDataGridViewTextBoxColumn,
            this.agendaDataGridViewTextBoxColumn,
            this.Bot,
            this.colBotDesc,
            this.colBotStatus});
            this.gvAgendaBots.DataSource = this.agendaBotBindingSource;
            this.gvAgendaBots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvAgendaBots.Location = new System.Drawing.Point(3, 3);
            this.gvAgendaBots.MultiSelect = false;
            this.gvAgendaBots.Name = "gvAgendaBots";
            this.gvAgendaBots.ReadOnly = true;
            this.gvAgendaBots.Size = new System.Drawing.Size(786, 136);
            this.gvAgendaBots.TabIndex = 0;
            this.gvAgendaBots.CellStyleChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvAgendaBots_CellStyleChanged);
            this.gvAgendaBots.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvAgendaBots_CellValueChanged);
            this.gvAgendaBots.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.GvAgendaBots_CellValueNeeded);
            this.gvAgendaBots.ColumnDefaultCellStyleChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.GvAgendaBots_ColumnDefaultCellStyleChanged);
            this.gvAgendaBots.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.GvAgendaBots_RowStateChanged);
            this.gvAgendaBots.BindingContextChanged += new System.EventHandler(this.GvAgendaBots_BindingContextChanged);
            // 
            // barraStatus
            // 
            this.barraStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barraProgresso});
            this.barraStatus.Location = new System.Drawing.Point(0, 428);
            this.barraStatus.Name = "barraStatus";
            this.barraStatus.Size = new System.Drawing.Size(800, 22);
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
            this.panel3.Controls.Add(this.btnPararExecAgenda);
            this.panel3.Controls.Add(this.btnExecutarAgenda);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 388);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(800, 40);
            this.panel3.TabIndex = 5;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel3_Paint);
            // 
            // btnConsultar
            // 
            this.btnConsultar.Location = new System.Drawing.Point(7, 2);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(75, 34);
            this.btnConsultar.TabIndex = 2;
            this.btnConsultar.Text = "Consultar";
            this.btnConsultar.UseVisualStyleBackColor = true;
            this.btnConsultar.Click += new System.EventHandler(this.BtnConsultar_Click);
            // 
            // btnPararExecAgenda
            // 
            this.btnPararExecAgenda.Location = new System.Drawing.Point(168, 3);
            this.btnPararExecAgenda.Name = "btnPararExecAgenda";
            this.btnPararExecAgenda.Size = new System.Drawing.Size(75, 34);
            this.btnPararExecAgenda.TabIndex = 1;
            this.btnPararExecAgenda.Text = "Parar";
            this.btnPararExecAgenda.UseVisualStyleBackColor = true;
            // 
            // btnExecutarAgenda
            // 
            this.btnExecutarAgenda.Location = new System.Drawing.Point(88, 3);
            this.btnExecutarAgenda.Name = "btnExecutarAgenda";
            this.btnExecutarAgenda.Size = new System.Drawing.Size(75, 34);
            this.btnExecutarAgenda.TabIndex = 0;
            this.btnExecutarAgenda.Text = "Executar";
            this.btnExecutarAgenda.UseVisualStyleBackColor = true;
            this.btnExecutarAgenda.Click += new System.EventHandler(this.BtnExecutarAgenda_Click);
            // 
            // bgwConsultar
            // 
            this.bgwConsultar.WorkerReportsProgress = true;
            this.bgwConsultar.WorkerSupportsCancellation = true;
            this.bgwConsultar.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BgwConsultar_DoWork);
            this.bgwConsultar.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BgwConsultar_ProgressChanged);
            this.bgwConsultar.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgwConsultar_RunWorkerCompleted);
            // 
            // agendaBotBindingSource
            // 
            this.agendaBotBindingSource.DataSource = typeof(P2E.Automacao.Orquestrador.Lib.Entidades.AgendaBot);
            // 
            // cDAGENDADataGridViewTextBoxColumn
            // 
            this.cDAGENDADataGridViewTextBoxColumn.DataPropertyName = "CD_AGENDA";
            this.cDAGENDADataGridViewTextBoxColumn.HeaderText = "Agenda Nº";
            this.cDAGENDADataGridViewTextBoxColumn.Name = "cDAGENDADataGridViewTextBoxColumn";
            this.cDAGENDADataGridViewTextBoxColumn.ReadOnly = true;
            this.cDAGENDADataGridViewTextBoxColumn.Visible = false;
            // 
            // tXDESCRICAODataGridViewTextBoxColumn
            // 
            this.tXDESCRICAODataGridViewTextBoxColumn.DataPropertyName = "TX_DESCRICAO";
            this.tXDESCRICAODataGridViewTextBoxColumn.HeaderText = "Descrição";
            this.tXDESCRICAODataGridViewTextBoxColumn.Name = "tXDESCRICAODataGridViewTextBoxColumn";
            this.tXDESCRICAODataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // oPATIVODataGridViewTextBoxColumn
            // 
            this.oPATIVODataGridViewTextBoxColumn.DataPropertyName = "OP_ATIVO";
            this.oPATIVODataGridViewTextBoxColumn.HeaderText = "Ativo";
            this.oPATIVODataGridViewTextBoxColumn.Name = "oPATIVODataGridViewTextBoxColumn";
            this.oPATIVODataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // oPFORMAEXECDataGridViewTextBoxColumn
            // 
            this.oPFORMAEXECDataGridViewTextBoxColumn.DataPropertyName = "OP_FORMA_EXEC";
            this.oPFORMAEXECDataGridViewTextBoxColumn.HeaderText = "Forma Exec.";
            this.oPFORMAEXECDataGridViewTextBoxColumn.Name = "oPFORMAEXECDataGridViewTextBoxColumn";
            this.oPFORMAEXECDataGridViewTextBoxColumn.ReadOnly = true;
            this.oPFORMAEXECDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // oPREPETEDataGridViewTextBoxColumn
            // 
            this.oPREPETEDataGridViewTextBoxColumn.DataPropertyName = "OP_REPETE";
            this.oPREPETEDataGridViewTextBoxColumn.HeaderText = "Repete?";
            this.oPREPETEDataGridViewTextBoxColumn.Name = "oPREPETEDataGridViewTextBoxColumn";
            this.oPREPETEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // oPTIPOREPDataGridViewTextBoxColumn
            // 
            this.oPTIPOREPDataGridViewTextBoxColumn.DataPropertyName = "OP_TIPO_REP";
            this.oPTIPOREPDataGridViewTextBoxColumn.HeaderText = "Tipo Repetição";
            this.oPTIPOREPDataGridViewTextBoxColumn.Name = "oPTIPOREPDataGridViewTextBoxColumn";
            this.oPTIPOREPDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dTDATAEXECPLANDataGridViewTextBoxColumn
            // 
            this.dTDATAEXECPLANDataGridViewTextBoxColumn.DataPropertyName = "DT_DATA_EXEC_PLAN";
            this.dTDATAEXECPLANDataGridViewTextBoxColumn.HeaderText = "Data Programada";
            this.dTDATAEXECPLANDataGridViewTextBoxColumn.Name = "dTDATAEXECPLANDataGridViewTextBoxColumn";
            this.dTDATAEXECPLANDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // hRHORAEXECPLANDataGridViewTextBoxColumn
            // 
            this.hRHORAEXECPLANDataGridViewTextBoxColumn.DataPropertyName = "HR_HORA_EXEC_PLAN";
            this.hRHORAEXECPLANDataGridViewTextBoxColumn.HeaderText = "Hora Programada";
            this.hRHORAEXECPLANDataGridViewTextBoxColumn.Name = "hRHORAEXECPLANDataGridViewTextBoxColumn";
            this.hRHORAEXECPLANDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dTDATAULTIMAEXECDataGridViewTextBoxColumn
            // 
            this.dTDATAULTIMAEXECDataGridViewTextBoxColumn.DataPropertyName = "DT_DATA_ULTIMA_EXEC";
            this.dTDATAULTIMAEXECDataGridViewTextBoxColumn.HeaderText = "Data da Ultima Execução";
            this.dTDATAULTIMAEXECDataGridViewTextBoxColumn.Name = "dTDATAULTIMAEXECDataGridViewTextBoxColumn";
            this.dTDATAULTIMAEXECDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // oPULTIMOSTATUSDataGridViewTextBoxColumn
            // 
            this.oPULTIMOSTATUSDataGridViewTextBoxColumn.DataPropertyName = "OP_ULTIMO_STATUS";
            this.oPULTIMOSTATUSDataGridViewTextBoxColumn.HeaderText = "Status Última Execução";
            this.oPULTIMOSTATUSDataGridViewTextBoxColumn.Name = "oPULTIMOSTATUSDataGridViewTextBoxColumn";
            this.oPULTIMOSTATUSDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // agendaBindingSource
            // 
            this.agendaBindingSource.DataSource = typeof(P2E.Automacao.Orquestrador.Lib.Entidades.Agenda);
            // 
            // cDAGENDABOTDataGridViewTextBoxColumn
            // 
            this.cDAGENDABOTDataGridViewTextBoxColumn.DataPropertyName = "CD_AGENDA_BOT";
            this.cDAGENDABOTDataGridViewTextBoxColumn.HeaderText = "CD_AGENDA_BOT";
            this.cDAGENDABOTDataGridViewTextBoxColumn.Name = "cDAGENDABOTDataGridViewTextBoxColumn";
            this.cDAGENDABOTDataGridViewTextBoxColumn.ReadOnly = true;
            this.cDAGENDABOTDataGridViewTextBoxColumn.Visible = false;
            // 
            // cDAGENDADataGridViewTextBoxColumn1
            // 
            this.cDAGENDADataGridViewTextBoxColumn1.DataPropertyName = "CD_AGENDA";
            this.cDAGENDADataGridViewTextBoxColumn1.HeaderText = "CD_AGENDA";
            this.cDAGENDADataGridViewTextBoxColumn1.Name = "cDAGENDADataGridViewTextBoxColumn1";
            this.cDAGENDADataGridViewTextBoxColumn1.ReadOnly = true;
            this.cDAGENDADataGridViewTextBoxColumn1.Visible = false;
            // 
            // cDBOTDataGridViewTextBoxColumn
            // 
            this.cDBOTDataGridViewTextBoxColumn.DataPropertyName = "CD_BOT";
            this.cDBOTDataGridViewTextBoxColumn.HeaderText = "CD_BOT";
            this.cDBOTDataGridViewTextBoxColumn.Name = "cDBOTDataGridViewTextBoxColumn";
            this.cDBOTDataGridViewTextBoxColumn.ReadOnly = true;
            this.cDBOTDataGridViewTextBoxColumn.Visible = false;
            // 
            // nRORDEMEXECDataGridViewTextBoxColumn
            // 
            this.nRORDEMEXECDataGridViewTextBoxColumn.DataPropertyName = "NR_ORDEM_EXEC";
            this.nRORDEMEXECDataGridViewTextBoxColumn.HeaderText = "Ordem";
            this.nRORDEMEXECDataGridViewTextBoxColumn.Name = "nRORDEMEXECDataGridViewTextBoxColumn";
            this.nRORDEMEXECDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tXPARAMEXECDataGridViewTextBoxColumn
            // 
            this.tXPARAMEXECDataGridViewTextBoxColumn.DataPropertyName = "TX_PARAM_EXEC";
            this.tXPARAMEXECDataGridViewTextBoxColumn.HeaderText = "TX_PARAM_EXEC";
            this.tXPARAMEXECDataGridViewTextBoxColumn.Name = "tXPARAMEXECDataGridViewTextBoxColumn";
            this.tXPARAMEXECDataGridViewTextBoxColumn.ReadOnly = true;
            this.tXPARAMEXECDataGridViewTextBoxColumn.Visible = false;
            // 
            // agendaDataGridViewTextBoxColumn
            // 
            this.agendaDataGridViewTextBoxColumn.DataPropertyName = "Agenda";
            this.agendaDataGridViewTextBoxColumn.HeaderText = "Agenda";
            this.agendaDataGridViewTextBoxColumn.Name = "agendaDataGridViewTextBoxColumn";
            this.agendaDataGridViewTextBoxColumn.ReadOnly = true;
            this.agendaDataGridViewTextBoxColumn.Visible = false;
            // 
            // Bot
            // 
            this.Bot.DataPropertyName = "Bot.TX_NOME";
            this.Bot.HeaderText = "Bot";
            this.Bot.Name = "Bot";
            this.Bot.ReadOnly = true;
            this.Bot.Visible = false;
            // 
            // colBotDesc
            // 
            this.colBotDesc.DataPropertyName = "DescBot";
            this.colBotDesc.HeaderText = "Descrição";
            this.colBotDesc.Name = "colBotDesc";
            this.colBotDesc.ReadOnly = true;
            // 
            // colBotStatus
            // 
            this.colBotStatus.HeaderText = "Status";
            this.colBotStatus.Name = "colBotStatus";
            this.colBotStatus.ReadOnly = true;
            // 
            // frmPainel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
            this.panel2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAgendaBots)).EndInit();
            this.barraStatus.ResumeLayout(false);
            this.barraStatus.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.agendaBotBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.agendaBindingSource)).EndInit();
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
        private System.Windows.Forms.BindingSource agendaBindingSource;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.StatusStrip barraStatus;
        private System.Windows.Forms.DataGridView gvAgendaBots;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExecutarAgenda;
        private System.Windows.Forms.Button btnPararExecAgenda;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.ToolStripProgressBar barraProgresso;
        private System.ComponentModel.BackgroundWorker bgwConsultar;
        private System.Windows.Forms.BindingSource agendaBotBindingSource;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn cDAGENDABOTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDAGENDADataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDBOTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nRORDEMEXECDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tXPARAMEXECDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn agendaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bot;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBotDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBotStatus;
    }
}