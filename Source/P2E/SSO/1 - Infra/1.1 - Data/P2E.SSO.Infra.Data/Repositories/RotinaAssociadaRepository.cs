using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class RotinaAssociadaRepository : DapperRepository<RotinaAssociada>, IRotinaAssociadaRepository
    {
        private readonly SSOContext _context;

        public RotinaAssociadaRepository(SSOContext context) : base(context.Connection)
        {
            _context = context;
        }

        public bool ExcluirRotinaAssociada(long cd_rot_principal)
        {
            bool result = this.Delete(o => o.CD_ROT_PRINCIPAL == cd_rot_principal);

            return result;
        }
    }
}
