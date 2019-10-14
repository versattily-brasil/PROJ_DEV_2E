using MicroOrm.Dapper.Repositories;
using P2E.Main.Domain.Entities;
using System.Collections.Generic;

namespace P2E.Main.Domain.Repositories
{
    public interface ITabelaAuxiliarRepository : IDapperRepository<TabelaAuxiliar>
    {
        List<TabelaAuxiliar> MetodoCustomizado(string id);
    }
}
