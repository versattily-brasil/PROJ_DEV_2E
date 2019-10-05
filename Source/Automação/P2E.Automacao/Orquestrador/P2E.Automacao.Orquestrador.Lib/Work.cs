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
                    await CarregarAgendasAsync();

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

                    Thread.Sleep(10000);
                }
                catch (Exception ex)
                {
                    LogController.RegistrarLog(ex.Message, eTipoLog.ERRO);
                }
            }
        }

        private async Task ExecutarAgendaAsync(Agenda agenda)
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
                    AlterarStatusAgendaAsync(agenda, eStatusExec.Conclúído).Wait();
                }
            }
        }

        private async Task AlterarStatusAgendaAsync(Agenda agenda, eStatusExec novoStatus)
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
                    }

                    if (novoStatus == eStatusExec.Falha || novoStatus == eStatusExec.Conclúído)
                    {
                        agenda.AgendaProgramada.DT_FIM_EXEC = DateTime.Now;
                        agenda.CD_ULTIMA_EXEC = agenda.AgendaProgramada.CD_AGENDA_EXEC;
                        agenda.DT_DATA_ULTIMA_EXEC = agenda.AgendaProgramada.DT_FIM_EXEC;
                    }

                    await agendaExecRep.UpdateAsync(agenda.AgendaProgramada);
                    await agendaRep.UpdateAsync(agenda);
                } 
            }
        }

        private async Task AlterarStatusBotAsync(AgendaBot bot, eStatusExec novoStatus)
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

                    if (novoStatus == eStatusExec.Falha || novoStatus == eStatusExec.Conclúído)
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
                                Task task = new BaixarExtratos.Lib.Work().ExecutarAsync();
                                task.Wait();

                                await AlterarStatusBotAsync(bot, eStatusExec.Conclúído);
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
                                await new Processos.AcompanharDespachos.Lib.Work().ExecutarAsync();
                                await AlterarStatusBotAsync(bot, eStatusExec.Conclúído);
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
                                await new Processos.ComprovanteImportacao.Lib.Work().ExecutarAsync();
                                await AlterarStatusBotAsync(bot, eStatusExec.Conclúído);
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
                                await AlterarStatusBotAsync(bot, eStatusExec.Conclúído);
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
                                await AlterarStatusBotAsync(bot, eStatusExec.Conclúído);
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
                                await AlterarStatusBotAsync(bot, eStatusExec.Conclúído);
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
                        break;
                    case "ROBÔ 09":
                        await Task.Factory.StartNew(async () =>
                        {
                            try
                            {

                                new TomarCiencia.Lib.Work().Start();
                                await AlterarStatusBotAsync(bot, eStatusExec.Conclúído);
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
                }
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog($"Erro em [ExecutarBotAsync]. {ex.Message}", eTipoLog.ERRO, bot.BotProgramado.CD_BOT_EXEC, "bot");
            }
        }

        private async Task CarregarAgendasAsync()
        {
            try
            {
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

                    _agendas = agendaRep.FindAll(o => o.OP_ATIVO == 1);

                    if (_agendas.Any())
                    {
                        LogController.RegistrarLog($"{_agendas.Count()} agendas programadas foram localizadas.");

                        foreach (var agenda in _agendas.Where(p=> p.OP_STATUS == eStatusExec.Programado))
                        {
                            if (VerificaProgramacaoAgenda(agenda))
                            {
                                LogController.RegistrarLog($"Carregando os bots associados a agenda '{agenda.TX_DESCRICAO}.'");

                                await CarregarBotsAsync(agenda);

                                if (agenda.Bots.Any())
                                {
                                    LogController.RegistrarLog($"{agenda.Bots.Count()} bots encontrados.");
                                }
                                else
                                {
                                    LogController.RegistrarLog($"Nenhum bot localizado para a agenda '{agenda.TX_DESCRICAO}.'");
                                }
                            }

                            await AlterarStatusAgendaAsync(agenda, eStatusExec.Aguardando_Processamento);
                        }
                    }
                    else
                    {
                        LogController.RegistrarLog("Nenhuma programação localizada.");
                    }
                }

                LogController.RegistrarLog($"-------------------------------------------------------------------------------------------------------");

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog($"Erro em CarregarAgendasAsync. {ex.Message}");
            }
        }

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
                        bot.BotProgramado = await botExecRep.FindAsync(o => o.CD_BOT == bot.CD_BOT && o.CD_AGENDA_EXEC == agenda.AgendaProgramada.CD_AGENDA_EXEC);
                        await AlterarStatusBotAsync(bot, eStatusExec.Aguardando_Processamento);
                    }
                }
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog($"Erro em [CarregarBotsAsync]. {ex.Message}", eTipoLog.ERRO, agenda.AgendaProgramada.CD_AGENDA_EXEC, "agenda");
            }
        }

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

                    if (agenda.OP_REPETE == 1)
                    {
                        switch (agenda.OP_TIPO_REP)
                        {
                            case eTipoRepete.Horario:
                                if (!agenda.DT_DATA_ULTIMA_EXEC.HasValue)
                                {
                                    if (agenda.HR_HORA_EXEC_PROG <= new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
                                    {
                                        return true;
                                    }
                                }
                                else
                                if (DateTime.Now.Subtract(agenda.DT_DATA_ULTIMA_EXEC.Value).TotalMinutes > agenda.HR_HORA_EXEC_PROG.TotalMinutes)
                                {
                                    return true;
                                }
                                break;
                            case eTipoRepete.Diario:
                                if (!agenda.DT_DATA_ULTIMA_EXEC.HasValue)
                                {
                                    if (agenda.HR_HORA_EXEC_PROG <= new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
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
