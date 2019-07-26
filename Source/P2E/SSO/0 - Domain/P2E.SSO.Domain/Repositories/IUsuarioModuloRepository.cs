using System.Collections.Generic;
using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;


namespace P2E.SSO.Domain.Repositories
{
    public interface IUsuarioModuloRepository : IDapperRepository<UsuarioModulo>
    {
        List<UsuarioModulo> MetodoCustomizado(int id);
    }
}
