using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Administrativo.Domain.Entities;
using P2E.Administrativo.Domain.Repositories;
using P2E.Shared.Enum;
using P2E.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace P2E.Administrativo.API.Controllers
{
    public class AgendaController : ControllerBase
    {
        private readonly IAgendaRepository _AgendaRepository;
        private readonly IAgendaExecRepository _AgendaExecRepository;
        private readonly IAgendaExecLogRepository _AgendaExecLogRepository;
        private readonly IAgendaBotRepository _AgendaBotRepository;
        private readonly IBotRepository _BotRepository;
        private readonly IBotExecRepository _BotExecRepository;
        private readonly IBotExecLogRepository _BotExecLogRepository;

        public AgendaController(IAgendaRepository AgendaRepository
            , IAgendaBotRepository AgendaBotRepository         
            , IAgendaExecRepository agendaExecRepository
            , IAgendaExecLogRepository agendaExecLogRepository
            , IBotRepository BotRepository
            , IBotExecRepository BotExecRepository
            , IBotExecLogRepository BotExecLogRepository
            )
        {
            _AgendaRepository = AgendaRepository;
            _AgendaBotRepository = AgendaBotRepository;
            _BotRepository = BotRepository;
            _AgendaExecRepository = agendaExecRepository;
            _AgendaExecLogRepository = agendaExecLogRepository;
            _BotExecRepository = BotExecRepository;
            _BotExecLogRepository = BotExecLogRepository;
        }

        // GET: api/Agenda
        [HttpGet]
        [Route("api/v1/Agenda/todos")]
        public IEnumerable<Agenda> Get()
        {
            var result = _AgendaRepository.FindAll();
            return result;
        }


        // GET: api/rotina
        [HttpGet]
        [Route("api/v1/agenda/")]
        public DataPage<Agenda> Get([FromQuery] string descricao, [FromQuery] DataPage<Agenda> page)
        {
            page = _AgendaRepository.GetByPage(page, descricao);
            return page;
        }

        // GET: api/Agenda/5
        [HttpGet]
        [Route("api/v1/Agenda/{id}")]
        public Agenda Get(int id)
        {
            Agenda Agenda = _AgendaRepository.Find(p => p.CD_AGENDA == id);

            return Agenda;
        }

        // GET: api/Agenda/5
        [HttpGet]
        [Route("api/v1/Agenda/por-data/{data}")]
        public IEnumerable<Agenda> Get(string data)
        {
            try
            {
                //var dataPlan = new DateTime();

                var dataProg = DateTime.ParseExact(data, "dd-MM-yyyy", null);

                var Agendas = _AgendaRepository.FindAll(p => p.DT_DATA_EXEC_PROG == null || p.DT_DATA_EXEC_PROG == dataProg);

                foreach (var agenda in Agendas)
                {
                    agenda.Bots = _AgendaBotRepository.FindAll(p => p.CD_AGENDA == agenda.CD_AGENDA);

                    var ultimaAgendaExec = _AgendaExecRepository.FindAll(p => p.CD_AGENDA == agenda.CD_AGENDA).OrderByDescending(p => p.CD_AGENDA_EXEC).FirstOrDefault();

                    if (ultimaAgendaExec != null)
                    {
                        agenda.CD_ULTIMA_EXEC = ultimaAgendaExec?.CD_AGENDA_EXEC;
                        agenda.OP_ULTIMO_STATUS_EXEC = ultimaAgendaExec.OP_STATUS_AGENDA_EXEC;
                    }
                    else {
                        agenda.OP_ULTIMO_STATUS_EXEC = eStatusExec.Nao_Programado;
                    }

                    if (agenda.Bots != null)
                    {
                        foreach (var bot in agenda.Bots)
                        {
                            bot.Bot = _BotRepository.Find(p => p.CD_BOT == bot.CD_BOT);

                            var ultimoBotExec = _BotExecRepository.FindAll(p => p.CD_AGENDA_EXEC == agenda.CD_ULTIMA_EXEC && p.CD_BOT == bot.CD_BOT).FirstOrDefault();
                            if (ultimoBotExec != null)
                            {
                                bot.CD_ULTIMA_EXEC_BOT = ultimoBotExec?.CD_BOT_EXEC;
                                bot.CD_ULTIMO_STATUS_EXEC_BOT = (int)ultimoBotExec.OP_STATUS_BOT_EXEC;
                            }
                            else {
                                bot.CD_ULTIMO_STATUS_EXEC_BOT = (int)eStatusExec.Nao_Programado;
                            }
                        }
                    }

                    // obtem a execução programada
                    agenda.AgendaProgramada = _AgendaExecRepository.Find(
                        p => p.CD_AGENDA == agenda.CD_AGENDA 
                        && (p.OP_STATUS_AGENDA_EXEC == eStatusExec.Aguardando_Processamento || p.OP_STATUS_AGENDA_EXEC == eStatusExec.Programado));
                }

                return Agendas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        // GET: api/Agenda/5
        [HttpGet]
        [Route("api/v1/Agenda/altera-status/{id}/{status}")]
        public IActionResult AlterarStatus(int id, int status)
        {
            try
            {
                // recupera a agenda
                var agenda = _AgendaRepository.Find(p => p.CD_AGENDA == id);
                agenda.OP_ULTIMO_STATUS_EXEC = (eStatusExec)status;

                //se o novo status == programado, agenda uma execução dos robos associados a agenda.
                if (agenda.OP_ULTIMO_STATUS_EXEC == eStatusExec.Programado)
                {
                    // cria agendaExec
                    var agendaExec = new AgendaExec()
                    {
                        CD_AGENDA = agenda.CD_AGENDA,
                        OP_STATUS_AGENDA_EXEC = eStatusExec.Aguardando_Processamento
                    };

                    _AgendaExecRepository.Insert(agendaExec);
                    RegistrarLogAgenda(eTipoLog.MSG, $"Agenda '{agenda.TX_DESCRICAO}' programada.", agendaExec.CD_AGENDA_EXEC);
                    RegistrarLogAgenda(eTipoLog.MSG, $"Programando Bots.", agendaExec.CD_AGENDA_EXEC);

                    agenda.CD_ULTIMA_EXEC = agendaExec.CD_AGENDA_EXEC;

                    var bots = _AgendaBotRepository.FindAll(p => p.CD_AGENDA == id);
                    if (bots != null)
                    {
                        foreach (var bot in bots)
                        {
                            var botExec = new BotExec()
                            {
                                CD_AGENDA_EXEC = agendaExec.CD_AGENDA_EXEC,
                                CD_BOT = bot.CD_BOT,
                                NR_ORDEM_EXEC = bot.NR_ORDEM_EXEC,
                                OP_STATUS_BOT_EXEC = eStatusExec.Aguardando_Processamento,
                                Bot = _BotRepository.Find(p => p.CD_BOT == bot.CD_BOT)
                            };

                            _BotExecRepository.Insert(botExec);
                            RegistrarLogBot(eTipoLog.MSG, $"Bot '{botExec.Bot.TX_DESCRICAO}' programado e aguardando execução.", botExec.CD_BOT_EXEC);

                            bot.CD_ULTIMA_EXEC_BOT = botExec.CD_BOT_EXEC;
                            bot.CD_ULTIMO_STATUS_EXEC_BOT = (int)botExec.OP_STATUS_BOT_EXEC;

                            _AgendaBotRepository.Update(bot);
                        }


                        agendaExec.OP_STATUS_AGENDA_EXEC = eStatusExec.Aguardando_Processamento;
                        _AgendaExecRepository.Update(agendaExec);
                        RegistrarLogAgenda(eTipoLog.MSG, $"Agenda '{agenda.TX_DESCRICAO}' aguardando execução.", agendaExec.CD_AGENDA_EXEC);
                    }
                }
                else
                {
                    var agendaExec = _AgendaExecRepository.Find(p=> p.CD_AGENDA_EXEC == agenda.CD_ULTIMA_EXEC);
                    agendaExec.OP_STATUS_AGENDA_EXEC = agenda.OP_ULTIMO_STATUS_EXEC;

                    RegistrarLogAgenda(eTipoLog.MSG, $"Agenda '{agenda.TX_DESCRICAO}' alterada status para {agenda.OP_ULTIMO_STATUS_EXEC}.", agendaExec.CD_AGENDA_EXEC);

                    if (agendaExec.OP_STATUS_AGENDA_EXEC == eStatusExec.Executando)
                    {
                        agendaExec.DT_INICIO_EXEC = DateTime.Now;
                        RegistrarLogAgenda(eTipoLog.MSG, $"Processamento da agenda iniciado em {agendaExec.DT_INICIO_EXEC}.", agendaExec.CD_AGENDA_EXEC);
                    }

                    if (agendaExec.OP_STATUS_AGENDA_EXEC == eStatusExec.Falha || agendaExec.OP_STATUS_AGENDA_EXEC == eStatusExec.Conclúído)
                    {
                        agendaExec.DT_FIM_EXEC = DateTime.Now;
                        RegistrarLogAgenda(eTipoLog.MSG, $"Processamento da agenda finalizado em {agendaExec.DT_FIM_EXEC}.", agendaExec.CD_AGENDA_EXEC);
                    }

                    _AgendaExecRepository.Update(agendaExec);

                    //var botsExec = _BotExecRepository.FindAll(p => p.CD_AGENDA_EXEC == agendaExec.CD_AGENDA_EXEC);
                    //if (botsExec != null)
                    //{
                    //    foreach (var item in botsExec)
                    //    {
                    //        item.OP_STATUS_BOT_EXEC = agendaExec.OP_STATUS_AGENDA_EXEC;
                    //        RegistrarLogAgenda(eTipoLog.MSG, $"alterado status para {item.OP_STATUS_BOT_EXEC}.", item.CD_BOT_EXEC);

                    //        if (item.OP_STATUS_BOT_EXEC == eStatusExec.Executando)
                    //        {
                    //            item.DT_INICIO_EXEC = DateTime.Now;
                    //            RegistrarLogAgenda(eTipoLog.MSG, $"Processamento do bot iniciado em {item.DT_INICIO_EXEC}.", item.CD_BOT_EXEC);
                    //        }

                    //        if(item.OP_STATUS_BOT_EXEC == eStatusExec.Falha || item.OP_STATUS_BOT_EXEC == eStatusExec.Conclúído)
                    //        {
                    //            item.DT_FIM_EXEC = DateTime.Now;
                    //            RegistrarLogAgenda(eTipoLog.MSG, $"Processamento do bot finalizado em {item.DT_FIM_EXEC}.", item.CD_BOT_EXEC);
                    //        }

                    //        _BotExecRepository.Update(item);
                    //    }
                    //}
                }

                _AgendaRepository.Update(agenda);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Agenda/5
        [HttpGet]
        [Route("api/v1/Agenda/altera-status-bot/{id}/{status}")]
        public IActionResult AlterarStatusBot(int id, int status)
        {
            try
            {
                var botExec = _BotExecRepository.Find(p => p.CD_BOT_EXEC == id);
                botExec.OP_STATUS_BOT_EXEC = (eStatusExec)status;
                RegistrarLogAgenda(eTipoLog.MSG, $"alterado status para {botExec.OP_STATUS_BOT_EXEC}.", botExec.CD_BOT_EXEC);

                if (botExec.OP_STATUS_BOT_EXEC == eStatusExec.Executando)
                {
                    botExec.DT_INICIO_EXEC = DateTime.Now;
                    RegistrarLogAgenda(eTipoLog.MSG, $"Processamento do bot iniciado em {botExec.DT_INICIO_EXEC}.", botExec.CD_BOT_EXEC);
                }

                if (botExec.OP_STATUS_BOT_EXEC == eStatusExec.Falha || botExec.OP_STATUS_BOT_EXEC == eStatusExec.Conclúído)
                {
                    botExec.DT_FIM_EXEC = DateTime.Now;
                    RegistrarLogAgenda(eTipoLog.MSG, $"Processamento do bot finalizado em {botExec.DT_FIM_EXEC}.", botExec.CD_BOT_EXEC);
                }

                _BotExecRepository.Update(botExec);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Agenda
        [HttpPost]
        [Route("api/v1/agenda")]
        public IActionResult Post([FromBody] Agenda item)
        {
            try
            {
                if (item.IsValid() && _AgendaRepository.ValidarDuplicidades(item))
                {
                    _AgendaRepository.Insert(item);
                    return Ok();
                }
                else
                {
                    return Ok(item.Notifications.FirstOrDefault().Message);
                }

            }
            catch (Exception ex)
            {
                return BadRequest ("Error." + ex.Message);
            }
        }

        // PUT: api/Agenda/5
        [HttpPut]
        [Route("api/v1/Agenda/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Agenda item)
        {
            try
            {
                if (item.IsValid() && _AgendaRepository.ValidarDuplicidades(item))
                {
                    if (id > 0)
                        _AgendaRepository.Update(item);
                    else
                        _AgendaRepository.Insert(item);

                    return Ok(item);
                }
                else
                {
                    return BadRequest(item.Messages);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Route("api/v1/Agenda/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var objeto = _AgendaRepository.FindById(id);
                _AgendaRepository.Delete(objeto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir o registro. {ex.Message}");
            }
        }

        protected void RegistrarLogAgenda(eTipoLog tipo, string msg, int cdAgendaExec)
        {
            var log = new AgendaExecLog() {
                CD_AGENDA_EXEC = cdAgendaExec,
                OP_TIPO_LOG = eTipoLog.MSG,
                TX_MENSAGEM = msg,
                DT_DATAHORA_REG = DateTime.Now
            };

            _AgendaExecLogRepository.Insert(log);
        }


        protected void RegistrarLogBot(eTipoLog tipo, string msg, int cdBotExec)
        {
            var log = new BotExecLog()
            {
                CD_BOT_EXEC = cdBotExec,
                OP_TIPO_LOG = eTipoLog.MSG,
                TX_MENSAGEM = msg,
                DT_DATAHORA_REG = DateTime.Now
            };

            _BotExecLogRepository.Insert(log);
        }
    }
}
