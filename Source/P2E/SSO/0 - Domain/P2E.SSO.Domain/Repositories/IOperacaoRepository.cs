using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;

namespace P2E.SSO.Domain.Repositories
{
    public interface IOperacaoRepository : IDapperRepository<Operacao>
    {
        DataPage<Operacao> GetByPage(DataPage<Operacao> page, string descricao);

        int GetTotalRows(PredicateGroup predicateGroup);
    }
}
