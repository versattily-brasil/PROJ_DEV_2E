using System;
using System.Collections.Generic;
using System.Text;
using MicroOrm.Dapper.Repositories;
using P2E.Main.Domain.Entities;
using P2E.Main.Domain.Repositories;
using P2E.Main.Infra.Data.DataContext;

namespace P2E.Main.Infra.Data.Repositories
{
    public class ExemploRepository : DapperRepository<Exemplo>, IExemploRepository
    {
        private readonly MainContext _mainContext; 

        public ExemploRepository(MainContext mainContext) : base(mainContext.Connection)
        {
            _mainContext = mainContext;
        }

        public List<Exemplo> MetodoCustomizado(int id)
        {
            throw new NotImplementedException();
        }
    }
}
