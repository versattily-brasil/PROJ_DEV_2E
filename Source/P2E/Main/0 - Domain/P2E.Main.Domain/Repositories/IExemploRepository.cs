using MicroOrm.Dapper.Repositories;
using P2E.Main.Domain.Entities;
using System.Collections.Generic;

namespace P2E.Main.Domain.Repositories
{
    public interface IExemploRepository : IDapperRepository<Exemplo>
    {
        List<Exemplo> MetodoCustomizado(int id);
    }
}
