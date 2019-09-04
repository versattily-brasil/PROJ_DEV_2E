using MicroOrm.Dapper.Repositories;
using P2E.Importacao.Domain.Entities;
using P2E.Shared.Model;

namespace P2E.Importacao.Domain.Repositories
{
    public interface IVistoriaRepository : IDapperRepository<Vistoria>
    {
        DataPage<Vistoria> GetByPage(DataPage<Vistoria> page, string tx_status);
    }
}
