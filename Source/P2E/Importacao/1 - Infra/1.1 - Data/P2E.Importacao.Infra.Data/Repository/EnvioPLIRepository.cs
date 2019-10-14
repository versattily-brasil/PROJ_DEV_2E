using MicroOrm.Dapper.Repositories;
using P2E.Importacao.Domain.Entities;
using P2E.Importacao.Domain.Repositories;
using P2E.Importacao.Infra.Data.DataContext;

namespace P2E.Importacao.Infra.Data.Repository
{
    public class EnvioPLIRepository : DapperRepository<EnvioPLI>, IEnvioPLIRepository
    {
        private readonly ImportacaoContext _context;

        public EnvioPLIRepository(ImportacaoContext context) : base(context.Connection)
        {
            _context = context;
        }
    }
}
