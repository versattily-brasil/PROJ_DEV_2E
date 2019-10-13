using MicroOrm.Dapper.Repositories;
using P2E.Administrativo.Domain.Entities;
using P2E.Administrativo.Domain.Repositories;
using P2E.Administrativo.Infra.Data.DataContext;

namespace P2E.Administrativo.Infra.Data.Repositories
{
    public class AgendaBotRepository : DapperRepository<AgendaBot>, IAgendaBotRepository
    {
        private readonly AdmContext _admContext;

        public AgendaBotRepository(AdmContext admContext) : base(admContext.Connection)
        {
            _admContext = admContext;
        }
    }
}
