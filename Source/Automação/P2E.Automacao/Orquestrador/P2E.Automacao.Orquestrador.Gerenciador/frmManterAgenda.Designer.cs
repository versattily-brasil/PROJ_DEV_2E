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
            this.rdTipoRepeteHorario = new System.Windows.Forms.RadioButton();
            this.rdTipoRepeteDiario = new System.Windows.Forms.RadioButton();
            this.rdTipoExecSemanal = new System.Windows.Forms.RadioButton();
            this.rdTipoExecMensal = new System.Windows.Forms.RadioButton();
            this.agendaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            this.grpFormaExec.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.agendaBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Descrição";
            // 
            // txtDescricao
            // 
            this.txtDescricao.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.agendaBindingSource, "TX_DESCRICAO", true));
            this.txtDescricao.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.agendaBindingSource, "TX_DESCRICAO", true));
            this.txtDescricao.Enabled = false;
            this.txtDescricao.Location = new System.Drawing.Point(15, 29);
            this.txtDescricao.Multiline = true;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(773, 22);
            this.txtDescricao.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Data Programada";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(135, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Hora Programada";
            // 
            // txtDataProgramada
            // 
            this.txtDataProgramada.Location = new System.Drawing.Point(15, 169);
            this.txtDataProgramada.Mask = "00/00/0000";
            this.txtDataProgramada.Name = "txtDataProgramada";
            this.txtDataProgramada.Size = new System.Drawing.Size(117, 22);
            this.txtDataProgramada.TabIndex = 10;
            this.txtDataProgramada.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDataProgramada.ValidatingType = typeof(System.DateTime);
            // 
            // txtHoraProgramada
            // 
            this.txtHoraProgramada.Location = new System.Drawing.Point(138, 167);
            this.txtHoraProgramada.Mask = "00:00";
            this.txtHoraProgramada.Name = "txtHoraProgramada";
            this.txtHoraProgramada.Size = new System.Drawing.Size(118, 22);
            this.txtHoraProgramada.TabIndex = 11;
            this.txtHoraProgramada.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHoraProgramada.ValidatingType = typeof(System.DateTime);
            // 
            // chkAtivo
            // 
            this.chkAtivo.AutoSize = true;
            this.chkAtivo.Location = new System.Drawing.Point(15, 96);
            this.chkAtivo.Name = "chkAtivo";
            this.chkAtivo.Size = new System.Drawing.Size(61, 21);
            this.chkAtivo.TabIndex = 12;
            this.chkAtivo.Text = "Ativo";
            this.chkAtivo.UseVisualStyleBackColor = true;
            // 
            // chkRepete
            // 
            this.chkRepete.AutoSize = true;
            this.chkRepete.Checked = true;
            this.chkRepete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRepete.Location = new System.Drawing.Point(311, 96);
            this.chkRepete.Name = "chkRepete";
            this.chkRepete.Size = new System.Drawing.Size(84, 21);
            this.chkRepete.TabIndex = 13;
            this.chkRepete.Text = "Repetir?";
            this.chkRepete.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.btnSalvar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 235);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(816, 60);
            this.panel1.TabIndex = 16;
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(8, 11);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(98, 37);
            this.btnSalvar.TabIndex = 0;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(262, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(148, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "Início Última Execução";
            // 
            // lblDataIniExec
            // 
            this.lblDataIniExec.AutoSize = true;
            this.lblDataIniExec.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataIniExec.Location = new System.Drawing.Point(262, 172);
            this.lblDataIniExec.Name = "lblDataIniExec";
            this.lblDataIniExec.Size = new System.Drawing.Size(159, 17);
            this.lblDataIniExec.TabIndex = 18;
            this.lblDataIniExec.Text = "00/00/0000 00:00:00";
            // 
            // lblDataFimExec
            // 
            this.lblDataFimExec.AutoSize = true;
            this.lblDataFimExec.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataFimExec.Location = new System.Drawing.Point(436, 172);
            this.lblDataFimExec.Name = "lblDataFimExec";
            this.lblDataFimExec.Size = new System.Drawing.Size(159, 17);
            this.lblDataFimExec.TabIndex = 20;
            this.lblDataFimExec.Text = "00/00/0000 00:00:00";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(436, 145);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(138, 17);
            this.label9.TabIndex = 19;
            this.label9.Text = "Fim Última Execução";
            // 
            // lblstatus
            // 
            this.lblstatus.AutoSize = true;
            this.lblstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblstatus.Location = new System.Drawing.Point(611, 172);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(14, 17);
            this.lblstatus.TabIndex = 22;
            this.lblstatus.Text = "-";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(611, 145);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 17);
            this.label11.TabIndex = 21;
            this.label11.Text = "Status Execução";
            // 
            // rdFormaExecManual
            // 
            this.rdFormaExecManual.AutoSize = true;
            this.rdFormaExecManual.Checked = true;
            this.rdFormaExecManual.Location = new System.Drawing.Point(7, 25);
            this.rdFormaExecManual.Name = "rdFormaExecManual";
            this.rdFormaExecManual.Size = new System.Drawing.Size(75, 21);
            this.rdFormaExecManual.TabIndex = 23;
            this.rdFormaExecManual.TabStop = true;
            this.rdFormaExecManual.Text = "Manual";
            this.rdFormaExecManual.UseVisualStyleBackColor = true;
            // 
            // rdFormaExecAuto
            // 
            this.rdFormaExecAuto.AutoSize = true;
            this.rdFormaExecAuto.Location = new System.Drawing.Point(88, 25);
            this.rdFormaExecAuto.Name = "rdFormaExecAuto";
            this.rdFormaExecAuto.Size = new System.Drawing.Size(99, 21);
            this.rdFormaExecAuto.TabIndex = 24;
            this.rdFormaExecAuto.Text = "Automático";
            this.rdFormaExecAuto.UseVisualStyleBackColor = true;
            // 
            // grpFormaExec
            // 
            this.grpFormaExec.Controls.Add(this.rdFormaExecManual);
            this.grpFormaExec.Controls.Add(this.rdFormaExecAuto);
            this.grpFormaExec.Location = new System.Drawing.Point(90, 71);
            this.grpFormaExec.Name = "grpFormaExec";
            this.grpFormaExec.Size = new System.Drawing.Size(206, 52);
            this.grpFormaExec.TabIndex = 25;
            this.grpFormaExec.TabStop = false;
            this.grpFormaExec.Text = "Forma de Execução";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdTipoExecSemanal);
            this.groupBox2.Controls.Add(this.rdTipoExecMensal);
            this.groupBox2.Controls.Add(this.rdTipoRepeteHorario);
            this.groupBox2.Controls.Add(this.rdTipoRepeteDiario);
            this.groupBox2.Location = new System.Drawing.Point(401, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(387, 52);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tipo de Repetição";
            // 
            // rdTipoRepeteHorario
            // 
            this.rdTipoRepeteHorario.AutoSize = true;
            this.rdTipoRepeteHorario.Checked = true;
            this.rdTipoRepeteHorario.Location = new System.Drawing.Point(20, 21);
            this.rdTipoRepeteHorario.Name = "rdTipoRepeteHorario";
            this.rdTipoRepeteHorario.Size = new System.Drawing.Size(76, 21);
            this.rdTipoRepeteHorario.TabIndex = 23;
            this.rdTipoRepeteHorario.TabStop = true;
            this.rdTipoRepeteHorario.Text = "Horário";
            this.rdTipoRepeteHorario.UseVisualStyleBackColor = true;
            // 
            // rdTipoRepeteDiario
            // 
            this.rdTipoRepeteDiario.AutoSize = true;
            this.rdTipoRepeteDiario.Location = new System.Drawing.Point(118, 21);
            this.rdTipoRepeteDiario.Name = "rdTipoRepeteDiario";
            this.rdTipoRepeteDiario.Size = new System.Drawing.Size(66, 21);
            this.rdTipoRepeteDiario.TabIndex = 24;
            this.rdTipoRepeteDiario.Text = "Diário";
            this.rdTipoRepeteDiario.UseVisualStyleBackColor = true;
            // 
            // rdTipoExecSemanal
            // 
            this.rdTipoExecSemanal.AutoSize = true;
            this.rdTipoExecSemanal.Location = new System.Drawing.Point(201, 21);
            this.rdTipoExecSemanal.Name = "rdTipoExecSemanal";
            this.rdTipoExecSemanal.Size = new System.Drawing.Size(84, 21);
            this.rdTipoExecSemanal.TabIndex = 25;
            this.rdTipoExecSemanal.Text = "Semanal";
            this.rdTipoExecSemanal.UseVisualStyleBackColor = true;
            // 
            // rdTipoExecMensal
            // 
            this.rdTipoExecMensal.AutoSize = true;
            this.rdTipoExecMensal.Location = new System.Drawing.Point(304, 21);
            this.rdTipoExecMensal.Name = "rdTipoExecMensal";
            this.rdTipoExecMensal.Size = new System.Drawing.Size(66, 21);
            this.rdTipoExecMensal.TabIndex = 26;
            this.rdTipoExecMensal.Text = "Diário";
            this.rdTipoExecMensal.UseVisualStyleBackColor = true;
            // 
            // agendaBindingSource
            // 
            this.agendaBindingSource.DataSource = typeof(P2E.Automacao.Orquestrador.Lib.Entidades.Agenda);
            // 
            // frmManterAgenda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 295);
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
            this.Name = "frmManterAgenda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manutenção da Agenda";
            this.Load += new System.EventHandler(this.frmManterAgenda_Load);
            this.panel1.ResumeLayout(false);
            this.grpFormaExec.ResumeLayout(false);
            this.grpFormaExec.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.agendaBindingSource)).EndInit();
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
    }
}