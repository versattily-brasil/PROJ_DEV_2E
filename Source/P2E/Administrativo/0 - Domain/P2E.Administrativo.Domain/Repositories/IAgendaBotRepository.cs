using MicroOrm.Dapper.Repositories;
using P2E.Administrativo.Domain.Entities;
using P2E.Shared.Model;

namespace P2E.Administrativo.Domain.Repositories
{
    public interface IAgendaRepository : IDapperRepository<Agenda>
    {
        bool ValidarDuplicidades(Agenda agenda);
        DataPage<Agenda> GetByPage(DataPage<Agenda> page, string tx_dsc);
    }
}
