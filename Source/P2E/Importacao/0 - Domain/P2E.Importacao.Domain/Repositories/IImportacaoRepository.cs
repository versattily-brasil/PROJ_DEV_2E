using MicroOrm.Dapper.Repositories;
using P2E.Importacao.Domain.Entities;
using P2E.Shared.Model;

namespace P2E.Importacao.Domain.Repositories
{
    public interface IImportacaoRepository : IDapperRepository<TBImportacao>
    {
        DataPage<TBImportacao> GetByPage(DataPage<TBImportacao> page, string tx_dsc);
    }
}
