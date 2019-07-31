using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;

namespace P2E.SSO.Domain.Repositories
{
    public interface IGrupoRepository : IDapperRepository<Grupo>
    {
        DataPage<Grupo> GetByPage(DataPage<Grupo> page, string tx_dsc);

        int GetTotalRows(PredicateGroup predicateGroup);
    }
}
