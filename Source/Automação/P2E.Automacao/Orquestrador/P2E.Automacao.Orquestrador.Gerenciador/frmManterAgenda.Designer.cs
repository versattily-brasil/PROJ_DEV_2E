namespace P2E.Automacao.Orquestrador.Gerenciador
{
    partial class frmManterAgenda
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescricao = new System.Windows.Forms.TextBox();
            this.agendaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDataProgramada = new System.Windows.Forms.MaskedTextBox();
            this.txtHoraProgramada = new System.Windows.Forms.MaskedTextBox();
            this.chkAtivo = new System.Windows.Forms.CheckBox();
            this.chkRepete = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lblDataIniExec = new System.Windows.Forms.Label();
            this.lblDataFimExec = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblstatus = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.rdFormaExecManual = new System.Windows.Forms.RadioButton();
            this.rdFormaExecAuto = new System.Windows.Forms.RadioButton();
            this.grpFormaExec = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdTipoExecSemanal = new System.Windows.Forms.RadioButton();
            this.rdTipoExecMensal = new System.Windows.Forms.RadioButton();
            this.rdTipoRepeteHorario = new System.Windows.Forms.RadioButton();
            this.rdTipoRepeteDiario = new System.Windows.Forms.RadioButton();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.chkRetentar = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.agendaBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.grpFormaExec.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 87);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Descrição";
            // 
            // txtDescricao
            // 
            this.txtDescricao.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.agendaBindingSource, "TX_DESCRICAO", true));
            this.txtDescricao.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.agendaBindingSource, "TX_DESCRICAO", true));
            this.txtDescricao.Enabled = false;
            this.txtDescricao.Location = new System.Drawing.Point(10, 104);
            this.txtDescricao.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtDescricao.Multiline = true;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(538, 19);
            this.txtDescricao.TabIndex = 4;
            // 
            // agendaBindingSource
            // 
            this.agendaBindingSource.DataSource = typeof(P2E.Automacao.Orquestrador.Lib.Entidades.Agenda);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 147);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Data Programada";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(103, 145);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Hora Programada";
            // 
            // txtDataProgramada
            // 
            this.txtDataProgramada.Location = new System.Drawing.Point(13, 163);
            this.txtDataProgramada.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtDataProgramada.Mask = "00/00/0000";
            this.txtDataProgramada.Name = "txtDataProgramada";
            this.txtDataProgramada.Size = new System.Drawing.Size(89, 20);
            this.txtDataProgramada.TabIndex = 5;
            this.txtDataProgramada.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDataProgramada.ValidatingType = typeof(System.DateTime);
            // 
            // txtHoraProgramada
            // 
            this.txtHoraProgramada.Location = new System.Drawing.Point(106, 162);
            this.txtHoraProgramada.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtHoraProgramada.Mask = "00:00";
            this.txtHoraProgramada.Name = "txtHoraProgramada";
            this.txtHoraProgramada.Size = new System.Drawing.Size(90, 20);
            this.txtHoraProgramada.TabIndex = 6;
            this.txtHoraProgramada.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHoraProgramada.ValidatingType = typeof(System.DateTime);
            // 
            // chkAtivo
            // 
            this.chkAtivo.AutoSize = true;
            this.chkAtivo.Location = new System.Drawing.Point(11, 57);
            this.chkAtivo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkAtivo.Name = "chkAtivo";
            this.chkAtivo.Size = new System.Drawing.Size(50, 17);
            this.chkAtivo.TabIndex = 2;
            this.chkAtivo.Text = "Ativo";
            this.chkAtivo.UseVisualStyleBackColor = true;
            // 
            // chkRepete
            // 
            this.chkRepete.AutoSize = true;
            this.chkRepete.Checked = true;
            this.chkRepete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRepete.Location = new System.Drawing.Point(65, 57);
            this.chkRepete.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkRepete.Name = "chkRepete";
            this.chkRepete.Size = new System.Drawing.Size(66, 17);
            this.chkRepete.TabIndex = 3;
            this.chkRepete.Text = "Repetir?";
            this.chkRepete.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.btnSalvar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 212);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(585, 49);
            this.panel1.TabIndex = 16;
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(6, 9);
            this.btnSalvar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(74, 30);
            this.btnSalvar.TabIndex = 0;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(198, 144);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Início Última Execução";
            // 
            // lblDataIniExec
            // 
            this.lblDataIniExec.AutoSize = true;
            this.lblDataIniExec.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataIniExec.Location = new System.Drawing.Point(198, 166);
            this.lblDataIniExec.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDataIniExec.Name = "lblDataIniExec";
            this.lblDataIniExec.Size = new System.Drawing.Size(129, 13);
            this.lblDataIniExec.TabIndex = 18;
            this.lblDataIniExec.Text = "00/00/0000 00:00:00";
            // 
            // lblDataFimExec
            // 
            this.lblDataFimExec.AutoSize = true;
            this.lblDataFimExec.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataFimExec.Location = new System.Drawing.Point(329, 166);
            this.lblDataFimExec.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDataFimExec.Name = "lblDataFimExec";
            this.lblDataFimExec.Size = new System.Drawing.Size(129, 13);
            this.lblDataFimExec.TabIndex = 20;
            this.lblDataFimExec.Text = "00/00/0000 00:00:00";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(329, 144);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Fim Última Execução";
            // 
            // lblstatus
            // 
            this.lblstatus.AutoSize = true;
            this.lblstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblstatus.Location = new System.Drawing.Point(460, 166);
            this.lblstatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(11, 13);
            this.lblstatus.TabIndex = 22;
            this.lblstatus.Text = "-";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(460, 144);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Status Execução";
            // 
            // rdFormaExecManual
            // 
            this.rdFormaExecManual.AutoSize = true;
            this.rdFormaExecManual.Checked = true;
            this.rdFormaExecManual.Location = new System.Drawing.Point(14, 17);
            this.rdFormaExecManual.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdFormaExecManual.Name = "rdFormaExecManual";
            this.rdFormaExecManual.Size = new System.Drawing.Size(60, 17);
            this.rdFormaExecManual.TabIndex = 23;
            this.rdFormaExecManual.TabStop = true;
            this.rdFormaExecManual.Text = "Manual";
            this.rdFormaExecManual.UseVisualStyleBackColor = true;
            // 
            // rdFormaExecAuto
            // 
            this.rdFormaExecAuto.AutoSize = true;
            this.rdFormaExecAuto.Location = new System.Drawing.Point(82, 17);
            this.rdFormaExecAuto.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdFormaExecAuto.Name = "rdFormaExecAuto";
            this.rdFormaExecAuto.Size = new System.Drawing.Size(78, 17);
            this.rdFormaExecAuto.TabIndex = 24;
            this.rdFormaExecAuto.Text = "Automático";
            this.rdFormaExecAuto.UseVisualStyleBackColor = true;
            // 
            // grpFormaExec
            // 
            this.grpFormaExec.Controls.Add(this.rdFormaExecManual);
            this.grpFormaExec.Controls.Add(this.rdFormaExecAuto);
            this.grpFormaExec.Location = new System.Drawing.Point(11, 11);
            this.grpFormaExec.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpFormaExec.Name = "grpFormaExec";
            this.grpFormaExec.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpFormaExec.Size = new System.Drawing.Size(170, 42);
            this.grpFormaExec.TabIndex = 0;
            this.grpFormaExec.TabStop = false;
            this.grpFormaExec.Text = "Forma de Execução";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdTipoExecSemanal);
            this.groupBox2.Controls.Add(this.rdTipoExecMensal);
            this.groupBox2.Controls.Add(this.rdTipoRepeteHorario);
            this.groupBox2.Controls.Add(this.rdTipoRepeteDiario);
            this.groupBox2.Location = new System.Drawing.Point(185, 11);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(290, 42);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tipo de Repetição";
            // 
            // rdTipoExecSemanal
            // 
            this.rdTipoExecSemanal.AutoSize = true;
            this.rdTipoExecSemanal.Location = new System.Drawing.Point(151, 17);
            this.rdTipoExecSemanal.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdTipoExecSemanal.Name = "rdTipoExecSemanal";
            this.rdTipoExecSemanal.Size = new System.Drawing.Size(66, 17);
            this.rdTipoExecSemanal.TabIndex = 25;
            this.rdTipoExecSemanal.Text = "Semanal";
            this.rdTipoExecSemanal.UseVisualStyleBackColor = true;
            // 
            // rdTipoExecMensal
            // 
            this.rdTipoExecMensal.AutoSize = true;
            this.rdTipoExecMensal.Location = new System.Drawing.Point(228, 17);
            this.rdTipoExecMensal.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdTipoExecMensal.Name = "rdTipoExecMensal";
            this.rdTipoExecMensal.Size = new System.Drawing.Size(52, 17);
            this.rdTipoExecMensal.TabIndex = 26;
            this.rdTipoExecMensal.Text = "Diário";
            this.rdTipoExecMensal.UseVisualStyleBackColor = true;
            // 
            // rdTipoRepeteHorario
            // 
            this.rdTipoRepeteHorario.AutoSize = true;
            this.rdTipoRepeteHorario.Checked = true;
            this.rdTipoRepeteHorario.Location = new System.Drawing.Point(15, 17);
            this.rdTipoRepeteHorario.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdTipoRepeteHorario.Name = "rdTipoRepeteHorario";
            this.rdTipoRepeteHorario.Size = new System.Drawing.Size(59, 17);
            this.rdTipoRepeteHorario.TabIndex = 23;
            this.rdTipoRepeteHorario.TabStop = true;
            this.rdTipoRepeteHorario.Text = "Horário";
            this.rdTipoRepeteHorario.UseVisualStyleBackColor = true;
            // 
            // rdTipoRepeteDiario
            // 
            this.rdTipoRepeteDiario.AutoSize = true;
            this.rdTipoRepeteDiario.Location = new System.Drawing.Point(88, 17);
            this.rdTipoRepeteDiario.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdTipoRepeteDiario.Name = "rdTipoRepeteDiario";
            this.rdTipoRepeteDiario.Size = new System.Drawing.Size(52, 17);
            this.rdTipoRepeteDiario.TabIndex = 24;
            this.rdTipoRepeteDiario.Text = "Diário";
            this.rdTipoRepeteDiario.UseVisualStyleBackColor = true;
            // 
            // chkLoop
            // 
            this.chkLoop.AutoSize = true;
            this.chkLoop.Location = new System.Drawing.Point(185, 57);
            this.chkLoop.Margin = new System.Windows.Forms.Padding(2);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(56, 17);
            this.chkLoop.TabIndex = 23;
            this.chkLoop.Text = "Loop?";
            this.chkLoop.UseVisualStyleBackColor = true;
            // 
            // chkRetentar
            // 
            this.chkRetentar.AutoSize = true;
            this.chkRetentar.Checked = true;
            this.chkRetentar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRetentar.Location = new System.Drawing.Point(255, 57);
            this.chkRetentar.Margin = new System.Windows.Forms.Padding(2);
            this.chkRetentar.Name = "chkRetentar";
            this.chkRetentar.Size = new System.Drawing.Size(220, 17);
            this.chkRetentar.TabIndex = 24;
            this.chkRetentar.Text = "Em caso de Falha executar novamente ?";
            this.chkRetentar.UseVisualStyleBackColor = true;
            // 
            // frmManterAgenda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 261);
            this.Controls.Add(this.chkRetentar);
            this.Controls.Add(this.chkLoop);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpFormaExec);
            this.Controls.Add(this.lblstatus);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblDataFimExec);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblDataIniExec);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkRepete);
            this.Controls.Add(this.chkAtivo);
            this.Controls.Add(this.txtHoraProgramada);
            this.Controls.Add(this.txtDataProgramada);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDescricao);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "frmManterAgenda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manutenção da Agenda";
            this.Load += new System.EventHandler(this.frmManterAgenda_Load);
            ((System.ComponentModel.ISupportInitialize)(this.agendaBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.grpFormaExec.ResumeLayout(false);
            this.grpFormaExec.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescricao;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.BindingSource agendaBindingSource;
        private System.Windows.Forms.MaskedTextBox txtDataProgramada;
        private System.Windows.Forms.MaskedTextBox txtHoraProgramada;
        private System.Windows.Forms.CheckBox chkAtivo;
        private System.Windows.Forms.CheckBox chkRepete;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblDataIniExec;
        private System.Windows.Forms.Label lblDataFimExec;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblstatus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RadioButton rdFormaExecManual;
        private System.Windows.Forms.RadioButton rdFormaExecAuto;
        private System.Windows.Forms.GroupBox grpFormaExec;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdTipoRepeteHorario;
        private System.Windows.Forms.RadioButton rdTipoRepeteDiario;
        private System.Windows.Forms.RadioButton rdTipoExecSemanal;
        private System.Windows.Forms.RadioButton rdTipoExecMensal;
        private System.Windows.Forms.CheckBox chkLoop;
        private System.Windows.Forms.CheckBox chkRetentar;
    }
}