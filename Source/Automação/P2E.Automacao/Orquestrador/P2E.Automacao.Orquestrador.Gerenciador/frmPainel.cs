using P2E.Automacao.Orquestrador.Lib;
using P2E.Automacao.Orquestrador.Lib.Entidades;
using P2E.Automacao.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2E.Automacao.Orquestrador.Gerenciador
{
    public partial class frmPainel : Form
    {
        private string _urlApiBase;
        public readonly Work _work;
        public frmPainel()
        {
            InitializeComponent();
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];

            _work = new Orquestrador.Lib.Work();
        }

        private async void BtnExecutarAgenda_Click(object sender, EventArgs e)
        {
            if (gvAgendamentos.SelectedRows.Count > 0)
            {

                var agendaSelecionada = (Agenda)gvAgendamentos.SelectedRows[0].DataBoundItem;
                if (agendaSelecionada.OP_STATUS == eStatusExec.Aguardando_Processamento)
                {
                    MessageBox.Show($"A agenda {agendaSelecionada.TX_DESCRICAO} está aguardando para ser executada.");
                }
                else
                if (agendaSelecionada.OP_STATUS == eStatusExec.Programado)
                {
                    MessageBox.Show($"A agenda {agendaSelecionada.TX_DESCRICAO} está programada para ser executada.");
                }
                else
                {
                    await _work.ProgramarAgendaAsync(agendaSelecionada, agendaSelecionada.OP_FORMA_EXEC);

                    if (!bgwConsultar.IsBusy)
                    {
                        bgwConsultar.RunWorkerAsync();
                        btnExecutarAgenda.Enabled = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Nenhum registro selecionado.");
            }
        }

        public void BtnConsultar_Click(object sender, EventArgs e)
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
            this.Invoke((MethodInvoker)delegate ()
            {
                agendaBindingSource.DataSource = null;
                agendaBindingSource.DataSource = _work.CarregarProgramacaoAsync();
                gvAgendaBots.Refresh();
            });
        }

        private void BgwConsultar_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            barraProgresso.Style = ProgressBarStyle.Blocks;
            btnConsultar.Enabled = true;
            btnExecutarAgenda.Enabled = true;
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

        private void CarregarBotExecLog(AgendaBot selecionado)
        {
            if (selecionado.CD_ULTIMA_EXEC_BOT.HasValue)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    bsBotExecLog.DataSource = null;
                    bsBotExecLog.DataSource = _work.ObterLogsExecLogs(selecionado.CD_ULTIMA_EXEC_BOT.Value);
                });
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void gvAgendaBots_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gvAgendaBots_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void interromperExecuçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var agendaSelecionada = (Agenda)gvAgendamentos.SelectedRows[0].DataBoundItem;
            if (agendaSelecionada.OP_STATUS == eStatusExec.Executando || agendaSelecionada.OP_STATUS == eStatusExec.Programado || agendaSelecionada.OP_STATUS == eStatusExec.Aguardando_Processamento)
            {
                if (MessageBox.Show($"Confirmar interrupção da execução da agenda '{agendaSelecionada.TX_DESCRICAO}'?", "Confirma",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _work.AlterarStatusAgendaAsync(agendaSelecionada, eStatusExec.Interrompido);
                    //MessageBox.Show($"Execução interrompida.");
                    btnConsultar.PerformClick();
                }
            }
            else
            {
                MessageBox.Show($"A agenda {agendaSelecionada.TX_DESCRICAO} não está em execução e nem programada.");
            }
        }

        private void alterarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var agendaSelecionada = (Agenda)gvAgendamentos.SelectedRows[0].DataBoundItem;

            new frmManterAgenda(agendaSelecionada, this).Show();
        }

        private void frmPainel_Load(object sender, EventArgs e)
        {
            try
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                Version version = entryAssembly.GetName().Version;

                toolStripVersao.Text = "Versão: "+string.Format("v{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

                if (!bgwConsultar.IsBusy)
                {
                    bgwConsultar.RunWorkerAsync();
                    btnConsultar.Enabled = false;
                }

                Process.Start("P2E.Automacao.Orquestrador.Console.exe", Directory.GetCurrentDirectory());
            }
            catch (Exception)
            {

            }
        }

        private void btnAtualizacao_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;

            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completo);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressoFeito);
            webClient.DownloadFileAsync(new Uri("http://deploy.2e.versattily.com/Orquestrador/Orquestrador.exe"), @"C:\ProgramData\Orquestrador.exe");
        }

        private void ProgressoFeito(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Completo(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download efetuado!","Informação",MessageBoxButtons.OK,MessageBoxIcon.Information);

            System.Diagnostics.Process.Start(@"C:\\ProgramData\\Orquestrador.exe", "/silent");
        }
    }
}
