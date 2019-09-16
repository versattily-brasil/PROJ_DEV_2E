using P2E.Automacao.Orquestrador.Lib.Entidades;
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

        private void BtnExecutarAgenda_Click(object sender, EventArgs e)
        {
            if (gvAgendamentos.SelectedRows.Count > 0)
            {
                var agendaSelecionada = (Agenda)gvAgendamentos.SelectedRows[0].DataBoundItem;
                   
                // programar agenda exec

                // programar bot exe
                
            // alterar o campo ultimo status de exe para AGUARDANDO PROCESSAMENTO
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
            string url = _urlApiBase + $"adm/v1/agenda/por-data/{txtData.Value.ToString("dd-MM-yyyy")}";

            // realiza a requisição para a api de importação
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(url);

                // recupera os registros.
                var registros = await result.Content.ReadAsAsync<List<Agenda>>();


                this.Invoke((MethodInvoker)delegate ()
                {
                    agendaBindingSource.DataSource = null;
                    agendaBindingSource.DataSource = registros.OrderByDescending(p => p.DT_DATA_EXEC_PLAN);
                });
            }
        }

        private void BgwConsultar_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void BgwConsultar_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            barraProgresso.Style = ProgressBarStyle.Blocks;
            btnConsultar.Enabled = true;
        }

        private void GvAgendamentos_SelectionChanged(object sender, EventArgs e)
        {
            if (gvAgendamentos.SelectedRows.Count > 0)
            {
                var agendaSelecionada = (Agenda)gvAgendamentos.SelectedRows[0].DataBoundItem;

                agendaBotBindingSource.DataSource = null;
                agendaBotBindingSource.DataSource = agendaSelecionada.Bots;
            }
            else
            {
                agendaBotBindingSource.DataSource = null;
            }
        }

        private void GvAgendaBots_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
          
        }

        private void Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GvAgendaBots_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                var bot = (AgendaBot)gvAgendaBots.SelectedRows[0].DataBoundItem;
                e.Value = bot.Bot.TX_DESCRICAO;
            }
        }

        private void GvAgendaBots_BindingContextChanged(object sender, EventArgs e)
        {

        }

        private void GvAgendaBots_ColumnDefaultCellStyleChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.Name.Contains("colBotDesc"))
            {
                var bot = (AgendaBot)gvAgendaBots.SelectedRows[0].DataBoundItem;
               
            }
        }

        private void GvAgendaBots_CellStyleChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void GvAgendaBots_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 7 && e.RowIndex >= 0)
            {
                var bot = (AgendaBot)gvAgendaBots.Rows[e.RowIndex]?.DataBoundItem;
                gvAgendaBots.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = bot.Bot.TX_DESCRICAO;
            }
        }
    }
}
