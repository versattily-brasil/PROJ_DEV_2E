using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class UsuarioGrupoRepository : DapperRepository<UsuarioGrupo>, IUsuarioGrupoRepository
    {
        private readonly SSOContext _ssoContext;

        public UsuarioGrupoRepository(SSOContext ssoContext) : base(ssoContext.Connection)
        {
            _ssoContext = ssoContext;
        }

        public bool ExcluirUsuarioGrupo(int usuarioId)
        {
            this.Delete(o => o.CD_USR == usuarioId);
            return false;
        }
    }
}
