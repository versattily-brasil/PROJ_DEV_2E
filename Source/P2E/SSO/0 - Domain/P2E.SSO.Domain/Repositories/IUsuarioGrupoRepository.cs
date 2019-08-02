using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;

namespace P2E.SSO.Domain.Repositories
{
    public interface IUsuarioGrupoRepository : IDapperRepository<UsuarioGrupo>
    {
        bool ExcluirUsuarioGrupo(int usuarioId);
    }
}
