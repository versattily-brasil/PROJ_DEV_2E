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
        public Work() => _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];

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
                    _agendas = CarregarAgendasAsync().Result;

                    if(_agendas.Any())
                    {
                        LogController.RegistrarLog("Iniciando processo de execução das agendas pendentes");

                        if (_agendas.Any(p => p.OP_STATUS == eStatusExec.Aguardando_Processamento))
                        {
                            Parallel.ForEach(_agendas.Where(p => p.OP_STATUS == eStatusExec.Aguardando_Processamento), async agenda =>
                            {
                                if (agenda.AgendaProgramada != null)
                                {
                                    if (agenda.OP_STATUS == eStatusExec.Aguardando_Processamento)
                                    {
                                        await ExecutarAgendaAsync(agenda);
                                    }
                                }
                            });
                        }
                        else 
                        {
                            LogController.RegistrarLog("Nenhuma agenda para executar.");
                        }
                    }

                    Thread.Sleep(30000);
                }
                catch (Exception ex)
                {
                    LogController.RegistrarLog(ex.Message, eTipoLog.ERRO);
                }
            }
        }

        public async Task ExecutarAgendaAsync(Agenda agenda)
        {
            LogController.RegistrarLog($"Executando agenda '{agenda.TX_DESCRICAO}'");

            await AlterarStatusAgendaAsync(agenda, eStatusExec.Executando);

            LogController.RegistrarLog($"Executando bots.", eTipoLog.INFO, agenda.AgendaProgramada.CD_AGENDA_EXEC, "agenda", "");
            if (agenda.Bots != null)
            {
                foreach (var bot in agenda.Bots)
                {
                    if (bot.BotProgramado != null)
                    {
                        AlterarStatusBotAsync(bot, eStatusExec.Executando).Wait();
                        ExecutarBotAsync(bot).Wait();
                    }
                }

                if (agenda.Bots.Any(p => p.CD_ULTIMO_STATUS_EXEC_BOT == eStatusExec.Falha))
                {
                    AlterarStatusAgendaAsync(agenda, eStatusExec.Falha).Wait();
                }
                else
                {
                    AlterarStatusAgendaAsync(agenda, eStatusExec.Concluído).Wait();
                }
            }
        }

        public async Task AlterarStatusAgendaAsync(Agenda agenda, eStatusExec novoStatus)
        {
            if (agenda.AgendaProgramada != null)
            {
                LogController.RegistrarLog($"Alterando status da agenda ['{agenda.TX_DESCRICAO}'] de ['{agenda.OP_STATUS.GetDescription()}'] " +
                       $"para ['{novoStatus.GetDescription()}']", eTipoLog.INFO, agenda.AgendaProgramada.CD_AGENDA_EXEC, "agenda", "");

                using (var context = new OrquestradorContext())
                {
                    var agendaRep = new AgendaRepository(context);
                    var agendaExecRep = new AgendaExecRepository(context);

                    agenda.AgendaProgramada.OP_STATUS_AGENDA_EXEC = novoStatus;
                    agenda.OP_STATUS = novoStatus;

                    if (novoStatus == eStatusExec.Executando)
                    {
                        agenda.AgendaProgramada.DT_INICIO_EXEC = DateTime.Now;
                        agenda.DT_DATA_INICIO_ULTIMA_EXEC = agenda.AgendaProgramada.DT_INICIO_EXEC;
                        agenda.DT_DATA_FIM_ULTIMA_EXEC = null;
                    }

                    if (novoStatus == eStatusExec.Falha || novoStatus == eStatusExec.Concluído)
                    {
                        agenda.AgendaProgramada.DT_FIM_EXEC = DateTime.Now;
                        agenda.CD_ULTIMA_EXEC = agenda.AgendaProgramada.CD_AGENDA_EXEC;
                        agenda.DT_DATA_FIM_ULTIMA_EXEC = agenda.AgendaProgramada.DT_FIM_EXEC;
                    }

                    await agendaExecRep.UpdateAsync(agenda.AgendaProgramada);
                    await agendaRep.UpdateAsync(agenda);
                } 
            }
        }

        public async Task AlterarStatusBotAsync(AgendaBot bot, eStatusExec novoStatus)
        {
            if (bot.BotProgramado != null)
            {
                LogController.RegistrarLog($"alterando status do bot '{bot.Bot.TX_DESCRICAO}' de [{bot.BotProgramado.OP_STATUS_BOT_EXEC.GetDescription()}] " +
                       $"para ['{novoStatus.GetDescription()}']", eTipoLog.INFO, bot.BotProgramado.CD_BOT_EXEC, "bot", "");

                using (var context = new OrquestradorContext())
                {
                    var agendaBotRep = new AgendaBotRepository(context);
                    var botExecRep = new BotExecRepository(context);

                    bot.BotProgramado.OP_STATUS_BOT_EXEC = novoStatus;
                    bot.UltimoBotExec = bot.BotProgramado;

                    if (novoStatus == eStatusExec.Executando)
                    {
                        bot.BotProgramado.DT_INICIO_EXEC = DateTime.Now;
                    }

                    if (novoStatus == eStatusExec.Falha || novoStatus == eStatusExec.Concluído)
                    {
                        bot.BotProgramado.DT_FIM_EXEC = DateTime.Now;

                        bot.CD_ULTIMA_EXEC_BOT = bot.BotProgramado.CD_BOT_EXEC;
                        bot.CD_ULTIMO_STATUS_EXEC_BOT = bot.BotProgramado.OP_STATUS_BOT_EXEC;
                    }

                    await botExecRep.UpdateAsync(bot.BotProgramado);
                    await agendaBotRep.UpdateAsync(bot);
                } 
            }
        }

        private async Task ExecutarBotAsync(AgendaBot bot)
        {
            try
            {
                switch (bot.Bot.TX_NOME.ToUpper())
                {
                    case "ROBÔ 01":
                        await Task.Factory.StartNew(async () =>
                        {
                            try
                            {
                                Task task = new BaixarExtratos.Lib.Work(bot.BotProgramado.CD_BOT_EXEC).ExecutarAsync();
                                task.Wait();

                                await AlterarStatusBotAsync(bot, eStatusExec.Concluído);
                            }
                            catch (Exception ex)
                            {
                                await AlterarStatusBotAsync(bot, eStatusExec.Falha);
                                LogController.RegistrarLog(ex.Message, eTipoLog.ERRO, bot.BotProgramado.CD_BOT_EXEC, "bot");
                            }
                        });
                        break;
                    case "ROBÔ 02":
                        await Task.Factory.StartNew(async () =>
                        {
                            try
                            {
                                await new Processos.AcompanharDespachos.Lib.Work(bot.BotProgramado.CD_BOT_EXEC).ExecutarAsync();
                                await AlterarStatusBotAsync(bot, eStatusExec.Concluído);
                            }
                            catch (Exception ex)
                            {
                                await AlterarStatusBotAsync(bot, eStatusExec.Falha);
                                LogController.RegistrarLog(ex.Message, eTipoLog.ERRO, bot.BotProgramado.CD_BOT_EXEC, "bot");
                            }
                        });
                        break;
                    case "ROBÔ 03":
                        await Task.Factory.StartNew(async () =>
                        {
                            try
                            {
                                await new Processos.ComprovanteImportacao.Lib.Work(bot.BotProgramado.CD_BOT_EXEC).ExecutarAsync();
                                await AlterarStatusBotAsync(bot, eStatusExec.Concluído);
                            }
                            catch (Exception ex)
                            {
                                await AlterarStatusBotAsync(bot, eStatusExec.Falha);
                                LogController.RegistrarLog(ex.Message, eTipoLog.ERRO, bot.BotProgramado.CD_BOT_EXEC, "bot");
                            }
                        });
                        break;
                    case "ROBÔ 04":
                        await Task.Factory.StartNew(async () =>
                        {
                            try
                            {
                                await new ExonerarIcms.Lib.Work().ExecutarAsync();
                                await AlterarStatusBotAsync(bot, eStatusExec.Concluído);
                            }
                            catch (Exception ex)
                            {
                                await AlterarStatusBotAsync(bot, eStatusExec.Falha);
                                LogController.RegistrarLog(ex.Message, eTipoLog.ERRO, bot.BotProgramado.CD_BOT_EXEC, "bot");
                            }
                        });
                        break;
                    case "ROBÔ 05":
                        await Task.Factory.StartNew(async () =>
                        {
                            try
                            {
                                await new Processos.ExtratoRetificacao.Lib.Work().ExecutarAsync();
                                await AlterarStatusBotAsync(bot, eStatusExec.Concluído);
                            }
                            catch (Exception ex)
                            {
                                await AlterarStatusBotAsync(bot, eStatusExec.Falha);
                                LogController.RegistrarLog(ex.Message, eTipoLog.ERRO, bot.BotProgramado.CD_BOT_EXEC, "bot");
                            }
                        });
                        break;
                    case "ROBÔ 06":
                        await Task.Factory.StartNew(async () =>
                        {
                            try
                            {
                                await new Processos.TelaDebito.Lib.Work().ExecutarAsync();
                                await AlterarStatusBotAsync(bot, eStatusExec.Concluído);
                            }
                            catch (Exception ex)
                            {
                                await AlterarStatusBotAsync(bot, eStatusExec.Falha);
                                LogController.RegistrarLog(ex.Message, eTipoLog.ERRO, bot.BotProgramado.CD_BOT_EXEC, "bot");
                            }
                        });
                        break;
                    case "ROBÔ 07":
                        break;
                    case "ROBÔ 08":
                        await Task.Factory.StartNew(async () =>
                        {
                            try
                            {
                                await new Processos.TaxaConversaoCambio.Lib.Work().ExecutarAsync();
                                await AlterarStatusBotAsync(bot, eStatusExec.Concluído);
                            }
                            catch (Exception ex)
                            {
                                await AlterarStatusBotAsync(bot, eStatusExec.Falha);
                                LogController.RegistrarLog(ex.Message, eTipoLog.ERRO, bot.BotProgramado.CD_BOT_EXEC, "bot");
                            }
                        });
                        break;
                    case "ROBÔ 09":
                        await Task.Factory.StartNew(async () =>
                        {
                            try
                            {

                                new TomarCiencia.Lib.Work().Start();
                                await AlterarStatusBotAsync(bot, eStatusExec.Concluído);
                            }
                            catch (Exception ex)
                            {
                                await AlterarStatusBotAsync(bot, eStatusExec.Falha);
                                LogController.RegistrarLog(ex.Message, eTipoLog.ERRO, bot.BotProgramado.CD_BOT_EXEC, "bot");
                            }
                        });
                        break;
                    case "ROBÔ 10":
                        break;
                    case "ROBÔ 11":
                        break;
                    case "ROBÔ 12":
                        await Task.Factory.StartNew(async () =>
                        {
                            try
                            {

                                await new P2E.Automacao.Processos.StatusDesembaracoSefaz.Lib.Work().ExecutarAsync();
                                await AlterarStatusBotAsync(bot, eStatusExec.Concluído);
                            }
                            catch (Exception ex)
                            {
                                await AlterarStatusBotAsync(bot, eStatusExec.Falha);
                                LogController.RegistrarLog(ex.Message, eTipoLog.ERRO, bot.BotProgramado.CD_BOT_EXEC, "bot");
                            }
                        });
                        break;
                    case "ROBÔ 15":
                        await Task.Factory.StartNew(async () =>
                        {
                            try
                            {

                                await new P2E.Automacao.Processos.AtualizaListaSuframa.Lib.Work(bot.BotProgramado.CD_BOT_EXEC).ExecutarAsync();
                                await AlterarStatusBotAsync(bot, eStatusExec.Concluído);
                            }
                            catch (Exception ex)
                            {
                                await AlterarStatusBotAsync(bot, eStatusExec.Falha);
                                LogController.RegistrarLog(ex.Message, eTipoLog.ERRO, bot.BotProgramado.CD_BOT_EXEC, "bot");
                            }
                        });
                        break;
                }
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog($"Erro em [ExecutarBotAsync]. {ex.Message}", eTipoLog.ERRO, bot.BotProgramado.CD_BOT_EXEC, "bot");
            }
        }

        private async Task<List<Agenda>> CarregarAgendasAsync()
        {
            try
            {
                var agendas = new List<Agenda>();

                LogController.RegistrarLog($"-------------------------------------------------------------------------------------------------------");
                LogController.RegistrarLog("Carregando Agendas.");
                using (var context = new OrquestradorContext())
                {
                    var agendaRep = new AgendaRepository(context);
                    var agendaBotRep = new AgendaBotRepository(context);
                    var agendaExecRep = new AgendaExecRepository(context);
                    var botRep = new BotRepository(context);
                    var botExecRep = new BotExecRepository(context);

                    LogController.RegistrarLog($"Obtendo agendas ativas.");

                    agendas = agendaRep.FindAll(o => o.OP_ATIVO == 1).ToList();

                    if (agendas.Any())
                    {
                        LogController.RegistrarLog($"{agendas.Count()} agenda(s) localizada(s).");

                        foreach (var agenda in agendas.Where(p=> p.OP_STATUS == eStatusExec.Programado))
                        {
                            LogController.RegistrarLog($"Carregando os bots associados a agenda '{agenda.TX_DESCRICAO}.'");
                            
                            if (VerificaProgramacaoAgenda(agenda))
                            {
                                await CarregarBotsAsync(agenda);

                                if ( agenda.Bots != null && agenda.Bots.Any())
                                {
                                    LogController.RegistrarLog($"{agenda.Bots.Count()} bots encontrados.", eTipoLog.INFO, agenda.AgendaProgramada.CD_AGENDA_EXEC, "agenda");


                                    await AlterarStatusAgendaAsync(agenda, eStatusExec.Aguardando_Processamento);
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

                LogController.RegistrarLog($"-------------------------------------------------------------------------------------------------------");

                //await Task.Delay(TimeSpan.FromSeconds(5));
                Thread.Sleep(5000);
                return agendas;
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog($"Erro em CarregarAgendasAsync. {ex.Message}");
                return null;
            }
        }

        public List<Agenda> CarregarProgramacaoAsync()
        {
            try
            {
                var agendas = new List<Agenda>();
                using (var context = new OrquestradorContext())
                {
                    var agendaRep = new AgendaRepository(context);
                    var agendaBotRep = new AgendaBotRepository(context);
                    var agendaExecRep = new AgendaExecRepository(context);
                    var botRep = new BotRepository(context);
                    var botExecRep = new BotExecRepository(context);

                    agendas = agendaRep.FindAll().ToList();

                    if (agendas.Any())
                    {
                        foreach (var agenda in agendas)
                        {
                            if (agenda.OP_STATUS == eStatusExec.Programado || agenda.OP_STATUS == eStatusExec.Aguardando_Processamento)
                            {
                                agenda.AgendaProgramada = agendaExecRep.Find(p => p.CD_AGENDA == agenda.CD_AGENDA && (p.OP_STATUS_AGENDA_EXEC == eStatusExec.Programado));
                            }

                            agenda.Bots = agendaBotRep.FindAll(p => p.CD_AGENDA == agenda.CD_AGENDA);

                            foreach (var bot in agenda.Bots)
                            {
                                bot.Bot = botRep.Find(o => o.CD_BOT == bot.CD_BOT);
                                if (agenda.OP_STATUS == eStatusExec.Programado)
                                {
                                    bot.BotProgramado = botExecRep.Find(o => o.CD_BOT == bot.CD_BOT && o.CD_AGENDA_EXEC == agenda.AgendaProgramada.CD_AGENDA_EXEC);
                                }
                            }
                        }
                    }
                }
                return agendas;
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog($"Erro em CarregarAgendasAsync. {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Carregar bots associados a agenda
        /// </summary>
        /// <param name="agenda"></param>
        /// <returns></returns>
        private async Task CarregarBotsAsync(Agenda agenda)
        {
            try
            {
                using (var context = new OrquestradorContext())
                {
                    var agendaBotRep = new AgendaBotRepository(context);
                    var botExecRep = new BotExecRepository(context);
                    var botRep = new BotRepository(context);

                    agenda.Bots = agendaBotRep.FindAll(p => p.CD_AGENDA == agenda.CD_AGENDA);


                    foreach (var bot in agenda.Bots)
                    {
                        bot.Bot = await botRep.FindAsync(o => o.CD_BOT == bot.CD_BOT);
                        if (agenda.OP_STATUS == eStatusExec.Programado && agenda.AgendaProgramada != null)
                        {
                            bot.BotProgramado = await botExecRep.FindAsync(o => o.CD_BOT == bot.CD_BOT && o.CD_AGENDA_EXEC == agenda.AgendaProgramada.CD_AGENDA_EXEC);
                            await AlterarStatusBotAsync(bot, eStatusExec.Aguardando_Processamento);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog($"Erro em [CarregarBotsAsync]. {ex.Message}", eTipoLog.ERRO, agenda.AgendaProgramada.CD_AGENDA_EXEC, "agenda");
            }
        }

        /// <summary>
        /// verifica se a agenda está na hora de executar
        /// </summary>
        /// <param name="agenda"></param>
        /// <returns></returns>
        private bool VerificaProgramacaoAgenda(Agenda agenda)
        {
            try
            {
                using (var context = new OrquestradorContext())
                {
                    var agendaBotRep = new AgendaBotRepository(context);
                    var agendaRep = new AgendaRepository(context);
                    var agendaExecRep = new AgendaExecRepository(context);
                    var botExecRep = new BotExecRepository(context);

                    if (agenda.OP_STATUS == eStatusExec.Programado || agenda.OP_STATUS == eStatusExec.Aguardando_Processamento)
                    {
                        agenda.AgendaProgramada = agendaExecRep.Find(p => p.CD_AGENDA == agenda.CD_AGENDA && (p.OP_STATUS_AGENDA_EXEC == eStatusExec.Programado));
                    }

                    if (agenda.OP_FORMA_EXEC == eFormaExec.Manual)
                    {
                        return true;
                    }
                    else
                    if (agenda.OP_REPETE == 1)
                    {
                        switch (agenda.OP_TIPO_REP)
                        {
                            case eTipoRepete.Horário:
                                if (!agenda.DT_DATA_FIM_ULTIMA_EXEC.HasValue)
                                {
                                    if (agenda.HR_HORA_EXEC_PROG <= new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
                                    {
                                        return true;
                                    }
                                }
                                else
                                if (DateTime.Now.Subtract(agenda.DT_DATA_FIM_ULTIMA_EXEC.Value).TotalMinutes > agenda.HR_HORA_EXEC_PROG.TotalMinutes)
                                {
                                    return true;
                                }
                                break;
                            case eTipoRepete.Diário:
                                if (!agenda.DT_DATA_FIM_ULTIMA_EXEC.HasValue)
                                {
                                    if (agenda.HR_HORA_EXEC_PROG <= new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
                                    {
                                        return true;
                                    }
                                }
                                else
                                if (DateTime.Now.Subtract(agenda.DT_DATA_FIM_ULTIMA_EXEC.Value).TotalDays > 1)
                                {
                                    return true;
                                }
                                break;
                            case eTipoRepete.Semanal:
                                if (!agenda.DT_DATA_FIM_ULTIMA_EXEC.HasValue)
                                {
                                    return true;
                                }
                                else
                                if (DateTime.Now.Subtract(agenda.DT_DATA_FIM_ULTIMA_EXEC.Value).TotalDays > 7)
                                {
                                    return true;
                                }
                                break;
                            case eTipoRepete.Mensal:
                                if (!agenda.DT_DATA_FIM_ULTIMA_EXEC.HasValue)
                                {
                                    return true;
                                }
                                else
                                 if (DateTime.Now.Subtract(agenda.DT_DATA_FIM_ULTIMA_EXEC.Value).TotalDays > 30)
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
                        if (agenda.DT_DATA_EXEC_PROG.HasValue && agenda.DT_DATA_EXEC_PROG.Value <= DateTime.Now)
                        {
                            return true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogController.RegistrarLog($"Erro em VerificaProgramacaoAgenda. {ex.Message}", eTipoLog.ERRO, agenda.AgendaProgramada.CD_AGENDA_EXEC, "agenda");
            }

            LogController.RegistrarLog($"Agenda {agenda.TX_DESCRICAO} fora da hora de execução.", eTipoLog.ALERTA, agenda.AgendaProgramada.CD_AGENDA_EXEC, "agenda");
            return false;
        }

        public async Task ProgramarAgendaAsync(Agenda agenda, eFormaExec formaExec)
        {
            try
            {
                using (var context = new OrquestradorContext())
                {
                    var agendaBotRep = new AgendaBotRepository(context);
                    var agendaRep = new AgendaRepository(context);
                    var agendaExecRep = new AgendaExecRepository(context);
                    var botExecRep = new BotExecRepository(context);
                    {
                        switch (agenda.OP_STATUS)
                        {
                            case eStatusExec.Falha:
                            case eStatusExec.Concluído:
                            case eStatusExec.Não_Programado:
                                {
                                    agenda.AgendaProgramada = new AgendaExec()
                                    {
                                        CD_AGENDA = agenda.CD_AGENDA,
                                        OP_STATUS_AGENDA_EXEC = eStatusExec.Programado
                                    };

                                    agendaExecRep.Insert(agenda.AgendaProgramada);

                                    agenda.CD_ULTIMA_EXEC = agenda.AgendaProgramada.CD_AGENDA_EXEC;
                                    agenda.OP_STATUS = agenda.AgendaProgramada.OP_STATUS_AGENDA_EXEC;
                                    agenda.OP_FORMA_EXEC = formaExec;
                                    agendaRep.Update(agenda);

                                    IEnumerable<AgendaBot> bots = agendaBotRep.FindAll(p => p.CD_AGENDA == agenda.CD_AGENDA);
                                    if (bots != null)
                                    {
                                        foreach (var bot in bots)
                                        {
                                            bot.BotProgramado = new BotExec()
                                            {
                                                CD_AGENDA_EXEC = agenda.AgendaProgramada.CD_AGENDA_EXEC,
                                                NR_ORDEM_EXEC = bot.NR_ORDEM_EXEC,
                                                OP_STATUS_BOT_EXEC = eStatusExec.Programado,
                                                CD_BOT = bot.CD_BOT,
                                            };

                                            botExecRep.Insert(bot.BotProgramado);
                                            bot.CD_ULTIMA_EXEC_BOT = bot.BotProgramado.CD_BOT_EXEC;
                                            bot.CD_ULTIMO_STATUS_EXEC_BOT = bot.BotProgramado.OP_STATUS_BOT_EXEC;
                                            agendaBotRep.Update(bot);
                                        }
                                    }

                                    break;
                                }

                            case eStatusExec.Programado:
                                await AlterarStatusAgendaAsync(agenda, eStatusExec.Aguardando_Processamento).ConfigureAwait(false);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog($"Erro ao programar agenda. {ex.Message}");
            }
        }

        public List<AgendaExecLog> ObterAgendaExecLogs(int cdAgendaExec)
        {
            using (var context = new OrquestradorContext())
            {
                var agendaExecLogRep = new AgendaExecLogRepository(context);
                return agendaExecLogRep.FindAll(p => p.CD_AGENDA_EXEC == cdAgendaExec).ToList();
            }
        }

        public List<BotExecLog> ObterLogsExecLogs(int cd_bot_exec)
        {
            using (var context = new OrquestradorContext())
            {
                var logRep = new BotExecLogRepository(context);
                return logRep.FindAll(p => p.CD_BOT_EXEC == cd_bot_exec).ToList();
            }
        }
    }
}
