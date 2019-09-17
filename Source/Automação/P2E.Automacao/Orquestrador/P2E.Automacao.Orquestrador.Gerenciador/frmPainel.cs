using P2E.Automacao.Orquestrador.Lib.Entidades;
using P2E.Automacao.Orquestrador.Lib.Util.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2E.Automacao.Orquestrador.Gerenciador
{
    public partial class frmPainel : Form
    {
        private string _urlApiBase;
        public frmPainel()
        {
            InitializeComponent();
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        private async void BtnExecutarAgenda_Click(object sender, EventArgs e)
        {
            if (gvAgendamentos.SelectedRows.Count > 0)
            {
                var agendaSelecionada = (Agenda)gvAgendamentos.SelectedRows[0].DataBoundItem;
                agendaSelecionada.OP_ULTIMO_STATUS_EXEC = eStatusExec.Programado;
                // programar agenda exec
                await ExecutarAgendaAsync(agendaSelecionada);

                if (!bgwConsultar.IsBusy)
                {
                    bgwConsultar.RunWorkerAsync();
                    btnConsultar.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Nenhum registro selecionado.");
            }
        }

        private void BtnConsultar_Click(object sender, EventArgs e)
        {
            if (!bgwConsultar.IsBusy)
            {
                bgwConsultar.RunWorkerAsync();
                btnConsultar.Enabled = false;
            }
        }

        private async void BgwConsultar_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                barraProgresso.Style = ProgressBarStyle.Marquee;
            });

            Thread.Sleep(3000);
            await ConsultarAsync();
        }

        private async Task ConsultarAsync()
        {
            // monta url para api de importação.
            string url = _urlApiBase + $"adm/v1/agenda/por-data/{DateTime.Today.ToString("dd-MM-yyyy")}";

            // realiza a requisição para a api de importação
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(url);

                // recupera os registros.
                var registros = await result.Content.ReadAsAsync<List<Agenda>>();


                this.Invoke((MethodInvoker)delegate ()
                {
                    agendaBindingSource.DataSource = null;
                    if(registros != null)
                    agendaBindingSource.DataSource = registros.OrderByDescending(p => p.DT_DATA_EXEC_PROG);
                });
            }
        }

        private async Task ExecutarAgendaAsync(Agenda agenda)
        {
            barraProgresso.Style = ProgressBarStyle.Marquee;
            // monta url para api de importação.
            string url = _urlApiBase + $"adm/v1/agenda/altera-status/{agenda.CD_AGENDA}/{(int)agenda.OP_ULTIMO_STATUS_EXEC}";

            // realiza a requisição para a api de importação
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(url);
                barraProgresso.Style = ProgressBarStyle.Blocks;
            }
        }

        private void BgwConsultar_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            barraProgresso.Style = ProgressBarStyle.Blocks;
            btnConsultar.Enabled = true;
        }

        private void GvAgendamentos_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (gvAgendamentos.SelectedRows.Count > 0)
                {
                    var agendaSelecionada = (Agenda)gvAgendamentos.SelectedRows[0].DataBoundItem;

                    bsAgendaBots.DataSource = null;
                    bsAgendaBots.DataSource = agendaSelecionada.Bots.ToList();

                }
                else
                {
                    bsAgendaBots.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GvAgendaBots_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 7 && e.RowIndex >= 0)
                {
                    var bot = (AgendaBot)gvAgendaBots.Rows[e.RowIndex]?.DataBoundItem;
                    gvAgendaBots.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = bot.Bot.TX_DESCRICAO;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        

        private void GvAgendaBots_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (gvAgendaBots.SelectedRows.Count > 0)
                {
                    var selecionado = (AgendaBot)gvAgendaBots.SelectedRows[0].DataBoundItem;
                    if (selecionado != null && selecionado.CD_ULTIMA_EXEC_BOT > 0)
                    {
                        CarregarBotExecLog(selecionado);
                    }
                }
                else
                {
                    bsBotExecLog.DataSource = null;
                }
            }
            catch (Exception)
            {
            }
        }

        private async void CarregarBotExecLog(AgendaBot selecionado)
        {

            // monta url para api.
            string url = _urlApiBase + $"adm/v1/BotExecLog/{selecionado.CD_ULTIMA_EXEC_BOT}";

            // realiza a requisição para a api
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(url);

                // recupera os registros.
                var registros = await result.Content.ReadAsAsync<List<BotExecLog>>();


                this.Invoke((MethodInvoker)delegate ()
                {
                    bsBotExecLog.DataSource = null;
                    bsBotExecLog.DataSource = registros.OrderBy(p => p.DT_DATAHORA_REG);
                });
            }
        }
    }
}
