using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Importacao.Domain.Entities;
using P2E.Shared.Model;

namespace P2E.Importacao.Domain.Repositories
{
    public interface ITriagemBotRepository : IDapperRepository<TriagemBot>
    {
        DataPage<TriagemBot> GetByPage(DataPage<TriagemBot> page, string tx_descricao);

        int GetTotalRows(PredicateGroup predicateGroup);

        void DeleteAll();
    }
}
