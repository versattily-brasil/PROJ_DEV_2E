using System;
using System.Collections.Generic;
using System.Text;
using MicroOrm.Dapper.Repositories;
using P2E.Main.Domain.Entities;

namespace P2E.Main.Domain.Repositories
{
    public interface ITabelaAuxiliarRepository : IDapperRepository<TabelaAuxiliar>
    {
        List<TabelaAuxiliar> MetodoCustomizado(string id);
    }
}
