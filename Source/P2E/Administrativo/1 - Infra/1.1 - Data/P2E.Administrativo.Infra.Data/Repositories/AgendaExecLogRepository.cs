using MicroOrm.Dapper.Repositories;
using P2E.Administrativo.Domain.Entities;
using P2E.Administrativo.Domain.Repositories;
using P2E.Administrativo.Infra.Data.DataContext;

namespace P2E.Administrativo.Infra.Data.Repositories
{
    public class AgendaExecLogRepository : DapperRepository<AgendaExecLog>, IAgendaExecLogRepository
    {
        private readonly AdmContext _admContext;

        public AgendaExecLogRepository(AdmContext admContext) : base(admContext.Connection)
        {
            _admContext = admContext;
        }
    }
}
