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
using P2E.Automacao.Orquestrador.Lib.Util.Extensions;
using P2E.Automacao.Orquestrador.DataContext;
using P2E.Automacao.Orquestrador.Repositories;
using MicroOrm.Dapper.Repositories;

namespace P2E.Automacao.Orquestrador.Gerenciador
{
    public partial class frmManterAgenda : Form
    {
        private Agenda _agenda;
        private DateTime? _ultimaDataProgramada;
        private TimeSpan _ultimaHoraProgramada;
        private frmPainel _painel;

        public frmManterAgenda(Agenda agenda, frmPainel painel)
        {
            InitializeComponent();
            _agenda = agenda;
            _painel = painel;
            _ultimaDataProgramada = agenda.DT_DATA_EXEC_PROG;
            _ultimaHoraProgramada = _agenda.HR_HORA_EXEC_PROG;

            agendaBindingSource.DataSource = agenda;

            CarregarCampos();
        }

        private void CarregarCampos()
        {

            rdFormaExecAuto.Checked = _agenda.OP_FORMA_EXEC == eFormaExec.Automática;
            rdFormaExecManual.Checked = _agenda.OP_FORMA_EXEC == eFormaExec.Manual;

            rdTipoRepeteHorario.Checked = _agenda.OP_TIPO_REP == eTipoRepete.Horário;
            rdTipoRepeteDiario.Checked = _agenda.OP_TIPO_REP == eTipoRepete.Diário;
            rdTipoExecSemanal.Checked = _agenda.OP_TIPO_REP == eTipoRepete.Semanal;
            rdTipoExecMensal.Checked = _agenda.OP_TIPO_REP == eTipoRepete.Mensal;

            chkRepete.Checked = _agenda.OP_REPETE > 0;

            txtDataProgramada.Text = _agenda.DT_DATA_EXEC_PROG.HasValue ? _agenda.DT_DATA_EXEC_PROG.Value.ToShortDateString() : "" ;

            txtHoraProgramada.Text = new DateTime(_agenda.HR_HORA_EXEC_PROG.Ticks, DateTimeKind.Local).ToShortTimeString();

            lblDataIniExec.Text = _agenda.DT_DATA_INICIO_ULTIMA_EXEC.HasValue ? _agenda.DT_DATA_INICIO_ULTIMA_EXEC.Value.ToString() : "-";
            lblDataFimExec.Text = _agenda.DT_DATA_FIM_ULTIMA_EXEC.HasValue ? _agenda.DT_DATA_FIM_ULTIMA_EXEC.Value.ToString() : "-";
            lblstatus.Text = _agenda.OP_STATUS.GetDescription();

            chkAtivo.Checked = _agenda.OP_ATIVO > 0;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    if (MessageBox.Show("Confirma alterar o registro?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (var context = new OrquestradorContext())
                        {
                            var rep = new DapperRepository<Agenda>(context.Connection).Update(_agenda);

                            // verifica se a data e hora de execução foram alterados, para programar.
                            if (_agenda.OP_FORMA_EXEC == eFormaExec.Automática && _ultimaHoraProgramada != _agenda.HR_HORA_EXEC_PROG || (_ultimaDataProgramada.HasValue && _ultimaDataProgramada != _agenda.DT_DATA_EXEC_PROG))
                            {
                               _painel._work.ProgramarAgendaAsync(_agenda, _agenda.OP_FORMA_EXEC);
                            }
                                
                            MessageBox.Show("Registro salvo com sucesso.");
                            _painel.btnConsultar.PerformClick();
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível salvar as alterações. {ex.Message}.");
            }
        }

        private bool ValidarCampos()
        {
            DateTime dataProg;
            TimeSpan horaProg;

            if (txtDataProgramada.Text != "  /  /")
            {
                DateTime.TryParse(txtDataProgramada.Text, out dataProg);
                if (dataProg <= DateTime.MinValue)
                {
                    MessageBox.Show("A data informada é inválida.");
                    return false;
                }
                else
                if (dataProg < DateTime.Today)
                {
                    MessageBox.Show("A data programda não pode ser menor que hoje.");
                    return false;
                }

                _agenda.DT_DATA_EXEC_PROG = dataProg;
            }

            TimeSpan.TryParse(txtHoraProgramada.Text, out horaProg);

            if (horaProg == null)
            {
                MessageBox.Show("A hora programada é inválida.");
                return false;
            }

            if (horaProg < new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
            {
                MessageBox.Show("A hora programada não pode ser menor que agora.");
                return false;
            }

            _agenda.HR_HORA_EXEC_PROG = horaProg;


            _agenda.OP_ATIVO = chkAtivo.Checked ? 1 : 0;
            _agenda.OP_REPETE = chkRepete.Checked ? 1 : 0;


            _agenda.OP_FORMA_EXEC = rdFormaExecAuto.Checked ? eFormaExec.Automática : eFormaExec.Manual;

            if (rdTipoRepeteDiario.Checked)
            { 
                _agenda.OP_TIPO_REP = eTipoRepete.Diário;
            }

            if (rdTipoRepeteHorario.Checked)
            {
                _agenda.OP_TIPO_REP = eTipoRepete.Horário;
            }

            if (rdTipoExecSemanal.Checked)
            {
                _agenda.OP_TIPO_REP = eTipoRepete.Semanal;
            }

            if (rdTipoExecMensal.Checked)
            {
                _agenda.OP_TIPO_REP = eTipoRepete.Mensal;
            }

            return true;
        }

        private void frmManterAgenda_Load(object sender, EventArgs e)
        {

        }
    }
}
