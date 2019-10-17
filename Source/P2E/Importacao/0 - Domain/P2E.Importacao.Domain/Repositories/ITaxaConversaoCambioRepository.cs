using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Importacao.Domain.Entities;
using P2E.Shared.Model;

namespace P2E.Importacao.Domain.Repositories
{
    public interface ITaxaConversaoCambioRepository : IDapperRepository<TaxaConversaoCambio>
    {
        DataPage<TaxaConversaoCambio> GetByPage(DataPage<TaxaConversaoCambio> page, string tx_status);

        int GetTotalRows(PredicateGroup predicateGroup);

        void DeleteAll();
    }
}
