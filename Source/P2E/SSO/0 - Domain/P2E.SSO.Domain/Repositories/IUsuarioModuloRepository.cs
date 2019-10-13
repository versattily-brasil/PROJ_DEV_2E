using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;

namespace P2E.SSO.Domain.Repositories
{
    public interface IUsuarioModuloRepository : IDapperRepository<UsuarioModulo>
    {
        bool ExcluirUsuarioModulos(int usuarioId);
    }
}
