using P2E.Automacao.Orquestrador.Lib.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.Orquestrador.Lib
{
    public class Work
    {
        protected List<Agenda> Agendas;
        private string _urlApiBase;

        public Work()
        {
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            Console.WriteLine("==================================ORQUESTRADOR====================================================");
            Console.WriteLine("Iniciando Monitoramento");


            while (true)
            {
                try
                {
                    string data = DateTime.Today.ToString("dd-MM-yyyy", null);
                    // monta url para api de importação.
                    string url = _urlApiBase + $"adm/v1/agenda/por-data/{data}";

                    // realiza a requisição para a api
                    using (var client = new HttpClient())
                    {
                        var result = await client.GetAsync(url);

                        // recupera os registros.
                        Agendas = await result.Content.ReadAsAsync<List<Agenda>>();

                        if ((Agendas.Where(p => p.OP_ULTIMO_STATUS_EXEC == Util.Enum.eStatusExec.Aguardando_Processamento).Any()))
                        {
                            Console.WriteLine($"Agendamento(s) Localizados, iniciando processamento.");
                        }

                        Parallel.ForEach(Agendas.Where(p => p.OP_ULTIMO_STATUS_EXEC == Util.Enum.eStatusExec.Aguardando_Processamento), async reg =>
                        {
                            await ControlarAgenda(reg);

                        });

                        Thread.Sleep(10000);
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        public async Task<object> MonitorarBots()
        {

            while (true)
            {
            
                if ((Agendas.Where(p => p.OP_ULTIMO_STATUS_EXEC == Util.Enum.eStatusExec.Aguardando_Processamento).Any()))
                {
                    Console.WriteLine($"Agendamento(s) Localizados, iniciando processamento.");
                }

                Parallel.ForEach(Agendas.Where(p => p.OP_ULTIMO_STATUS_EXEC == Util.Enum.eStatusExec.Aguardando_Processamento), async reg =>
                {
                    await ControlarAgenda(reg);

                });

                Thread.Sleep(10000);
            }
        }

        private async Task ControlarAgenda(Agenda agenda)
        {
            agenda.OP_ULTIMO_STATUS_EXEC = Util.Enum.eStatusExec.Executando;

            await AlteraStatusAgendaAsync(agenda);

            foreach (var item in agenda.Bots)
            {
                item.CD_ULTIMO_STATUS_EXEC_BOT = Util.Enum.eStatusExec.Executando;
                await AlteraStatusBotAsync(item);

                switch (item.Bot.TX_NOME.ToUpper())
                {
                    case "ROBÔ 01":
                        await Task.Factory.StartNew(async () =>
                        {
                            await new BaixarExtratos.Lib.Work().ExecutarAsync();
                            item.CD_ULTIMO_STATUS_EXEC_BOT = Util.Enum.eStatusExec.Conclúído;
                            await AlteraStatusBotAsync(item);
                        });
                        break;
                    case "ROBÔ 02":
                        await Task.Factory.StartNew(async () =>
                        {
                            await new Processos.AcompanharDespachos.Lib.Work().ExecutarAsync();
                            item.CD_ULTIMO_STATUS_EXEC_BOT = Util.Enum.eStatusExec.Conclúído;
                            await AlteraStatusBotAsync(item);
                        });
                        break;
                    case "ROBÔ 03":
                        await Task.Factory.StartNew(async () =>
                        {
                            await new Processos.ComprovanteImportacao.Lib.Work().ExecutarAsync();
                            item.CD_ULTIMO_STATUS_EXEC_BOT = Util.Enum.eStatusExec.Conclúído;
                            await AlteraStatusBotAsync(item);
                        });
                        break;
                }
            }
        }

        private async Task CarregarAgendasAsync()
        {
            try
            {
                string data = DateTime.Today.ToString("dd-MM-yyyy", null);
                // monta url para api de importação.
                string url = _urlApiBase + $"adm/v1/agenda/por-data/{data}";

                // realiza a requisição para a api
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(url);

                    // recupera os registros.
                    Agendas = await result.Content.ReadAsAsync<List<Agenda>>();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task AlteraStatusAgendaAsync(Agenda agenda)
        {
            string url = _urlApiBase + $"adm/v1/agenda/altera-status/{agenda.CD_AGENDA}/{(int)agenda.OP_ULTIMO_STATUS_EXEC}";

            // realiza a requisição para a api de importação
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(url);
            }
        }

        private async Task AlteraStatusBotAsync(AgendaBot bot)
        {
            string url = _urlApiBase + $"adm/v1/agendabot/altera-status/{bot.CD_ULTIMA_EXEC_BOT}/{(int)bot.CD_ULTIMO_STATUS_EXEC_BOT}";

            // realiza a requisição para a api de importação
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(url);
            }
        }
    }
}
