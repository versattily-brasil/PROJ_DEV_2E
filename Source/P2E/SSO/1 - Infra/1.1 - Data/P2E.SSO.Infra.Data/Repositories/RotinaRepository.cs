using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class RotinaRepository : DapperRepository<Rotina>, IRotinaRepository
    {
        private readonly SSOContext _ssoContext;

        public RotinaRepository(SSOContext ssoContext) : base(ssoContext.Connection)
        {
            _ssoContext = ssoContext;
        }
    }
}
