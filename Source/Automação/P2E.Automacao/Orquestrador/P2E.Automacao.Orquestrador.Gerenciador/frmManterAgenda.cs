using P2E.Automacao.Orquestrador.Lib.Entidades;
using P2E.Automacao.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2E.Automacao.Orquestrador.Gerenciador
{
    public partial class frmManterAgenda : Form
    {
        private Agenda _agenda;
        public frmManterAgenda(Agenda agenda)
        {
            InitializeComponent();
            _agenda = agenda;

            agendaBindingSource.DataSource = agenda;

            CarregarCampos();
        }

        private void CarregarCampos()
        {
            cmbFormaExec.DataSource = Enum.GetValues(typeof(eFormaExec));
            cmbFormaExec.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFormaExec.SelectedIndex = (int)_agenda.OP_FORMA_EXEC;

            txtDataProgramada.Text = _agenda.DT_DATA_EXEC_PROG.HasValue ? _agenda.DT_DATA_EXEC_PROG.Value.ToShortDateString() : "" ;

            txtHoraProgramada.Text = new DateTime(_agenda.HR_HORA_EXEC_PROG.Ticks, DateTimeKind.Local).ToShortTimeString();

        }
    }
}
