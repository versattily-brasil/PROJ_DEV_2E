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
    public class BotExecLogController : ControllerBase
    {
        private readonly IAgendaRepository _AgendaRepository;
        private readonly IAgendaExecRepository _AgendaExecRepository;
        private readonly IAgendaExecLogRepository _AgendaExecLogRepository;
        private readonly IAgendaBotRepository _AgendaBotRepository;
        private readonly IBotRepository _BotRepository;
        private readonly IBotExecRepository _BotExecRepository;
        private readonly IBotExecLogRepository _BotExecLogRepository;

        public BotExecLogController(IAgendaRepository AgendaRepository
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
        [Route("api/v1/BotExecLog/todos")]
        public IEnumerable<BotExecLog> Get()
        {
            var result = _BotExecLogRepository.FindAll();
            return result;
        }

        // GET: api/BotExecLog/5
        [HttpGet]
        [Route("api/v1/BotExecLog/{cd_bot_exec_log}")]
        public IEnumerable<BotExecLog> Get(int cd_bot_exec_log)
        {
            var result = _BotExecLogRepository.FindAll(p => p.CD_BOT_EXEC == cd_bot_exec_log);

            return result;
        }

        // PUT: api/Agenda/5
        [HttpPut]
        [Route("api/v1/BotExecLog/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] BotExecLog log)
        {
            try
            {
                _BotExecLogRepository.Insert(log);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
