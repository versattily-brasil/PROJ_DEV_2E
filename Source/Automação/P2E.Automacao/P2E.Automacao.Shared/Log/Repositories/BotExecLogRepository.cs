using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Automacao.Shared.Log.DataContext;
using P2E.Automacao.Shared.Log.Entidades;

namespace P2E.Automacao.Shared.Log.Repositories
{
    public class BotExecLogRepository : DapperRepository<BotExecLog>
    {
        private readonly LogContext _logContext;

        public BotExecLogRepository(LogContext logContext) : base(logContext.Connection)
        {
            _logContext = logContext;
        }
    }
}
