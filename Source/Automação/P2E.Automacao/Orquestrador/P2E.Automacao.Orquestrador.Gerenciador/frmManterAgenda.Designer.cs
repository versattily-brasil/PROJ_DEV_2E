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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.agendaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbFormaExec = new System.Windows.Forms.ComboBox();
            this.txtDataProgramada = new System.Windows.Forms.MaskedTextBox();
            this.txtHoraProgramada = new System.Windows.Forms.MaskedTextBox();
            this.chkAtivo = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSalvar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.agendaBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(368, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Forma Execução";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Data Programada";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(241, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Hora Programada";
            // 
            // agendaBindingSource
            // 
            this.agendaBindingSource.DataSource = typeof(P2E.Automacao.Orquestrador.Lib.Entidades.Agenda);
            // 
            // cmbFormaExec
            // 
            this.cmbFormaExec.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.agendaBindingSource, "OP_FORMA_EXEC", true));
            this.cmbFormaExec.FormattingEnabled = true;
            this.cmbFormaExec.Location = new System.Drawing.Point(371, 85);
            this.cmbFormaExec.Name = "cmbFormaExec";
            this.cmbFormaExec.Size = new System.Drawing.Size(154, 24);
            this.cmbFormaExec.TabIndex = 9;
            this.cmbFormaExec.ValueMember = "OP_FORMA_EXEC";
            // 
            // txtDataProgramada
            // 
            this.txtDataProgramada.Location = new System.Drawing.Point(81, 88);
            this.txtDataProgramada.Mask = "00/00/0000";
            this.txtDataProgramada.Name = "txtDataProgramada";
            this.txtDataProgramada.Size = new System.Drawing.Size(157, 22);
            this.txtDataProgramada.TabIndex = 10;
            this.txtDataProgramada.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDataProgramada.ValidatingType = typeof(System.DateTime);
            // 
            // txtHoraProgramada
            // 
            this.txtHoraProgramada.Location = new System.Drawing.Point(244, 85);
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
            this.chkAtivo.Location = new System.Drawing.Point(14, 88);
            this.chkAtivo.Name = "chkAtivo";
            this.chkAtivo.Size = new System.Drawing.Size(61, 21);
            this.chkAtivo.TabIndex = 12;
            this.chkAtivo.Text = "Ativo";
            this.chkAtivo.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(531, 88);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 21);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Repetir?";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(617, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "Forma Repetição";
            // 
            // comboBox1
            // 
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.agendaBindingSource, "OP_FORMA_EXEC", true));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(620, 86);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(168, 24);
            this.comboBox1.TabIndex = 15;
            this.comboBox1.ValueMember = "OP_FORMA_EXEC";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.btnSalvar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 203);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 42);
            this.panel1.TabIndex = 16;
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(8, 11);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 0;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            // 
            // frmManterAgenda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 245);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.chkAtivo);
            this.Controls.Add(this.txtHoraProgramada);
            this.Controls.Add(this.txtDataProgramada);
            this.Controls.Add(this.cmbFormaExec);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDescricao);
            this.Controls.Add(this.label1);
            this.Name = "frmManterAgenda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manutenção da Agenda";
            ((System.ComponentModel.ISupportInitialize)(this.agendaBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescricao;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.BindingSource agendaBindingSource;
        private System.Windows.Forms.ComboBox cmbFormaExec;
        private System.Windows.Forms.MaskedTextBox txtDataProgramada;
        private System.Windows.Forms.MaskedTextBox txtHoraProgramada;
        private System.Windows.Forms.CheckBox chkAtivo;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSalvar;
    }
}