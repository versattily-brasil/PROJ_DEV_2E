using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;

namespace P2E.SSO.Domain.Repositories
{
    public interface IUsuarioRepository : IDapperRepository<Usuario>
    {
        DataPage<Usuario> GetByPage(DataPage<Usuario> page, string tx_nome);

        int GetTotalRows(PredicateGroup predicateGroup);

        bool ValidarDuplicidades(Usuario usuario);
    }
}
