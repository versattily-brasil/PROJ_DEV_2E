using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class UsuarioModuloRepository : DapperRepository<UsuarioModulo>, IUsuarioModuloRepository
    {
        private readonly SSOContext _ssoContext;

        public UsuarioModuloRepository(SSOContext ssoContext) : base(ssoContext.Connection)
        {
            _ssoContext = ssoContext;
        }

        public bool ExcluirUsuarioModulos(int usuarioId)
        {
            this.Delete(o => o.CD_USR == usuarioId);
            return false;
        }
    }
}
