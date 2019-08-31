using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Shared.Model;
using P2E.Importacao.Domain.Entities;

namespace P2E.Importacao.Domain.Repositories
{
    public interface IImportacaoRepository : IDapperRepository<TBImportacao>
    {
        DataPage<TBImportacao> GetByPage(DataPage<TBImportacao> page, string tx_dsc);
    }
}
