using P2E.Automacao.Orquestrador.DataContext;
using P2E.Automacao.Orquestrador.Lib.Entidades;
using P2E.Automacao.Orquestrador.Lib.Util.Enum;
using P2E.Automacao.Orquestrador.Lib.Util.Extensions;
using P2E.Automacao.Orquestrador.Repositories;
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
        protected IEnumerable<Agenda> _agendas;
        private string _urlApiBase;
        public Work()
        {
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            LogController.RegistrarLog("========================================ORQUESTRADOR====================================================");
            LogController.RegistrarLog($"Execução iniciada em {DateTime.Now}");
            LogController.RegistrarLog($"-------------------------------------------------------------------------------------------------------");
            LogController.RegistrarLog($"Iniciando monitoramento--------------------------------------------------------------------------------");
            
            while (true)
            {
                try
                {
                    string data = DateTime.Today.ToString("dd-MM-yyyy", null);
                    CarregarAgendas();

                    if(_agendas.Any())
                    {
                        LogController.RegistrarLog("Iniciando processo de execução das agendas pendentes");

                        //Parallel.ForEach(_agendas, agenda =>
                        foreach (var agenda in _agendas)
                        {
                            if (agenda.AgendaProgramada != null)
                            {
                                if (agenda.OP_STATUS == eStatusExec.Aguardando_Processamento)
                                {
                                    LogController.RegistrarLog($"Executando agenda '{agenda.TX_DESCRICAO}'");
                                    
                                    LogController.RegistrarLog($"Alterando status de '{agenda.OP_STATUS.GetDescription()}' para {eStatusExec.Executando.GetDescription()}", eTipoLog.INFO, agenda.AgendaProgramada.CD_AGENDA_EXEC, "agenda", "");
                                    agenda.AgendaProgramada.OP_STATUS_AGENDA_EXEC = eStatusExec.Executando;

                                    LogController.RegistrarLog($"Executando bots.", eTipoLog.INFO, agenda.AgendaProgramada.CD_AGENDA_EXEC, "agenda", "");
                                    foreach (var bot in agenda.Bots)
                                    {
                                        if (bot.BotProgramado != null)
                                        {
                                            LogController.RegistrarLog($"alterando status de {bot.BotProgramado.OP_STATUS_BOT_EXEC.GetDescription()} para {bot.BotProgramado.OP_STATUS_BOT_EXEC.GetDescription()}", eTipoLog.INFO, bot.BotProgramado.CD_BOT_EXEC, "bot", "");
                                            bot.BotProgramado.OP_STATUS_BOT_EXEC = eStatusExec.Executando;

                                            using (var context = new OrquestradorContext())
                                            {
                                                var agendaBotRep = new AgendaBotRepository(context);
                                                var botExecRep = new BotExecRepository(context);
                                                botExecRep.Update(bot.BotProgramado);
                                                bot.UltimoBotExec = bot.BotProgramado;
                                                agendaBotRep.Update(bot);

                                                ExecutarBotAsync(bot);
                                            }
                                        }
                                    }
                                }
                            }
                        }//);
                    }

                    //    // recupera os registros.
                    //    Agendas = await result.Content.ReadAsAsync<List<Agenda>>();

                    //    if ((Agendas.Where(p => p.OP_ULTIMO_STATUS_EXEC == Util.Enum.eStatusExec.Aguardando_Processamento).Any()))
                    //    {
                    //        Console.WriteLine($"Agendamento(s) Localizados, iniciando processamento.");

                    //        Parallel.ForEach(Agendas.Where(p => p.OP_ULTIMO_STATUS_EXEC == Util.Enum.eStatusExec.Aguardando_Processamento), async reg =>
                    //        {
                    //            await ControlarAgenda(reg);

                    //        });
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine($"Nenhum Agendamento encontrado para ser processado.");
                    //    }

                    Thread.Sleep(10000);
                    //}
                }
                catch (Exception ex)
                {
                    LogController.RegistrarLog(ex.Message, eTipoLog.ERRO);
                }
            }
        }

        private async Task ExecutarBotAsync(AgendaBot bot)
        {
            switch (bot.Bot.TX_NOME.ToUpper())
            {
                case "ROBÔ 01":
                    await Task.Factory.StartNew(async () =>
                    {
                        //await new BaixarExtratos.Lib.Work().ExecutarAsync();
                        //item.CD_ULTIMO_STATUS_EXEC_BOT = Util.Enum.eStatusExec.Conclúído;
                        //await AlteraStatusBotAsync(item);
                    });
                    break;
                case "ROBÔ 02":
                    await Task.Factory.StartNew(async () =>
                    {
                        //await new Processos.AcompanharDespachos.Lib.Work().ExecutarAsync();
                        //item.CD_ULTIMO_STATUS_EXEC_BOT = Util.Enum.eStatusExec.Conclúído;
                        //await AlteraStatusBotAsync(item);
                    });
                    break;
                case "ROBÔ 03":
                    await Task.Factory.StartNew(async () =>
                    {
                        //await new Processos.ComprovanteImportacao.Lib.Work().ExecutarAsync();
                        //item.CD_ULTIMO_STATUS_EXEC_BOT = Util.Enum.eStatusExec.Conclúído;
                        //await AlteraStatusBotAsync(item);
                    });
                    break;
            }
        }


        //private async Task ControlarAgenda(Agenda agenda)
        //{
        //    agenda.OP_ULTIMO_STATUS_EXEC = Util.Enum.eStatusExec.Executando;

        //    await AlteraStatusAgendaAsync(agenda);

        //    foreach (var item in agenda.Bots)
        //    {
        //        //var botExec = ObterBotExec(item);

        //        item.CD_ULTIMO_STATUS_EXEC_BOT = Util.Enum.eStatusExec.Executando;
        //        await AlteraStatusBotAsync(item);

        //        switch (item.Bot.TX_NOME.ToUpper())
        //        {
        //            case "ROBÔ 01":
        //                await Task.Factory.StartNew(async () =>
        //                {
        //                    //await new BaixarExtratos.Lib.Work().ExecutarAsync();
        //                    //item.CD_ULTIMO_STATUS_EXEC_BOT = Util.Enum.eStatusExec.Conclúído;
        //                    //await AlteraStatusBotAsync(item);
        //                });
        //                break;
        //            case "ROBÔ 02":
        //                await Task.Factory.StartNew(async () =>
        //                {
        //                    await new Processos.AcompanharDespachos.Lib.Work().ExecutarAsync();
        //                    item.CD_ULTIMO_STATUS_EXEC_BOT = Util.Enum.eStatusExec.Conclúído;
        //                    await AlteraStatusBotAsync(item);
        //                });
        //                break;
        //            case "ROBÔ 03":
        //                await Task.Factory.StartNew(async () =>
        //                {
        //                    //await new Processos.ComprovanteImportacao.Lib.Work().ExecutarAsync();
        //                    //item.CD_ULTIMO_STATUS_EXEC_BOT = Util.Enum.eStatusExec.Conclúído;
        //                    await AlteraStatusBotAsync(item);
        //                });
        //                break;
        //        }
        //    }

        //    agenda.OP_ULTIMO_STATUS_EXEC = Util.Enum.eStatusExec.Conclúído;

        //    await AlteraStatusAgendaAsync(agenda);
        //}


        private void CarregarAgendas()
        {
            LogController.RegistrarLog("Carregando Agendas.");
            using (var context = new OrquestradorContext())
            {
                var agendaRep = new AgendaRepository(context);
                var agendaBotRep = new AgendaBotRepository(context);
                var agendaExecRep = new AgendaExecRepository(context);
                var botRep = new BotRepository(context);
                var botExecRep = new BotExecRepository(context);

                LogController.RegistrarLog($"Obtendo agendas ativas.");

                _agendas = agendaRep.FindAll(o=> o.OP_ATIVO == 1); 
                
                if (_agendas.Any())
                {
                    LogController.RegistrarLog($"{_agendas.Count()} agendas programadas pendentes de execução.");

                    foreach (var agenda in _agendas)
                    {
                        if (VerificaProgramacaoAgenda(agenda))
                        {
                            LogController.RegistrarLog($"Carregando os bots associados a agenda '{agenda.TX_DESCRICAO}.'");

                            CarregarBots(agenda);

                            if (agenda.Bots.Any())
                            {
                                LogController.RegistrarLog($"{agenda.Bots.Count()} bots encontrados.");
                            }
                            else
                            {
                                LogController.RegistrarLog($"Nenhum bot localizado para a agenda '{agenda.TX_DESCRICAO}.'");
                            }
                        }
                    }
                }
                else 
                {
                    LogController.RegistrarLog("Nenhuma programação localizada.");
                }
            }
        }

        private void CarregarBots(Agenda agenda)
        {
            using (var context = new OrquestradorContext())
            {
                var agendaBotRep = new AgendaBotRepository(context);
                var botExecRep = new BotExecRepository(context);

                agenda.Bots = agendaBotRep.FindAll(p => p.CD_AGENDA == agenda.CD_AGENDA);

                foreach (var bot in agenda.Bots)
                {
                    bot.BotProgramado = botExecRep.Find(o => o.CD_BOT == bot.CD_BOT && o.CD_AGENDA_EXEC == agenda.AgendaProgramada.CD_AGENDA_EXEC);
                }
            }
        }

        private bool VerificaProgramacaoAgenda(Agenda agenda)
        {
            using (var context = new OrquestradorContext())
            {
                var agendaBotRep = new AgendaBotRepository(context);
                var agendaRep = new AgendaRepository(context);
                var agendaExecRep = new AgendaExecRepository(context);
                var botExecRep = new BotExecRepository(context);

                // obtem a agenda programada [AgendaExec]
                if (agenda.OP_STATUS == eStatusExec.Programado || agenda.OP_STATUS == eStatusExec.Aguardando_Processamento)
                {
                    agenda.AgendaProgramada = agendaExecRep.Find(p => p.CD_AGENDA == agenda.CD_AGENDA && (p.OP_STATUS_AGENDA_EXEC == eStatusExec.Programado));
                }
                //else
                //if(agenda.OP_STATUS == eStatusExec.Aguardando_Processamento)
                //{
                //    // Cria uma programação
                //    ProgramarAgenda(agenda, agendaExecRep, agendaRep);
                //}

                if (agenda.OP_REPETE == 1)
                {
                    switch (agenda.OP_TIPO_REP)
                    {
                        case eTipoRepete.Horario:
                            break;
                        case eTipoRepete.Diario:
                            if (!agenda.DT_DATA_ULTIMA_EXEC.HasValue)
                            {
                                if (agenda.HR_HORA_EXEC_PROG <= new TimeSpan( DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
                                {
                                    return true;
                                }
                            }
                            else
                            if (DateTime.Now.Subtract(agenda.DT_DATA_ULTIMA_EXEC.Value).TotalDays > 1)
                            {
                                return true;
                            }
                            break;
                        case eTipoRepete.Semanal:
                            if (!agenda.DT_DATA_ULTIMA_EXEC.HasValue)
                            {
                                return true;
                            }
                            else
                            if (DateTime.Now.Subtract(agenda.DT_DATA_ULTIMA_EXEC.Value).TotalDays > 7)
                            {
                                return true;
                            }
                            break;
                        case eTipoRepete.Mensal:
                            if (!agenda.DT_DATA_ULTIMA_EXEC.HasValue)
                            {
                                return true;
                            }
                            else
                             if (DateTime.Now.Subtract(agenda.DT_DATA_ULTIMA_EXEC.Value).TotalDays > 30)
                            {
                                return true;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (agenda.DT_DATA_EXEC_PROG.HasValue && agenda.DT_DATA_EXEC_PROG.Value == DateTime.Now)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void ProgramarAgenda(Agenda agenda, AgendaExecRepository agendaExecRep, AgendaRepository agendaRep)
        {
            try
            {
                agenda.AgendaProgramada = new AgendaExec()
                {
                    CD_AGENDA = agenda.CD_AGENDA,
                    OP_STATUS_AGENDA_EXEC = eStatusExec.Programado
                };

                agendaExecRep.Insert(agenda.AgendaProgramada);
                agenda.OP_STATUS = eStatusExec.Programado;
                agendaRep.Update(agenda);
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog($"Erro ao programar agenda. {ex.Message}");
            }
        }
    }
}
