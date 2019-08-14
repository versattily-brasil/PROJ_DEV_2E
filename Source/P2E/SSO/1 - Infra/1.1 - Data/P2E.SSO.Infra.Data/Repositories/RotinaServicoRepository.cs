using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class RotinaServicoRepository : DapperRepository<RotinaServico>, IRotinaServicoRepository
    {
        private readonly SSOContext _context;

        public RotinaServicoRepository(SSOContext context) : base(context.Connection)
        {
            _context = context;
        }        

        public bool ExcluirRotinaServico(int rotinaServicoId)
        {
            bool result = this.Delete(o => o.CD_ROT_SRV == rotinaServicoId);

            return result;
        }        
    }
}
