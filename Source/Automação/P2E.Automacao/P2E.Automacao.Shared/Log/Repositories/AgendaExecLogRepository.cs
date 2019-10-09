using MicroOrm.Dapper.Repositories;
using P2E.Automacao.Shared.Log.DataContext;
using P2E.Automacao.Shared.Log.Entidades;

namespace P2E.Automacao.Shared.Log.Repositories
{
    public class AgendaExecLogRepository : DapperRepository<AgendaExecLog>
    {
        private readonly LogContext _logContext;

        public AgendaExecLogRepository(LogContext logContext) : base(logContext.Connection)
        {
            _logContext = logContext;
        }
    }
}
