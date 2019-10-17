using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Importacao.Domain.Entities;
using P2E.Shared.Model;

namespace P2E.Importacao.Domain.Repositories
{
    public interface IDetalheNCMRepository : IDapperRepository<DetalheNCM>
    {
        DataPage<DetalheNCM> GetByPage(DataPage<DetalheNCM> page, string tx_status);

        int GetTotalRows(PredicateGroup predicateGroup);

        void DeleteAll();
    }
}
