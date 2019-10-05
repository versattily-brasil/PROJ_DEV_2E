using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Automacao.Orquestrador.DataContext;
using P2E.Automacao.Orquestrador.Lib.Entidades;

namespace P2E.Automacao.Orquestrador.Repositories
{
    public class BotExecLogRepository : DapperRepository<BotExecLog>
    {
        private readonly OrquestradorContext _orquestradorContext;

        public BotExecLogRepository(OrquestradorContext orquestradorContext) : base(orquestradorContext.Connection)
        {
            _orquestradorContext = orquestradorContext;
        }
    }
}
