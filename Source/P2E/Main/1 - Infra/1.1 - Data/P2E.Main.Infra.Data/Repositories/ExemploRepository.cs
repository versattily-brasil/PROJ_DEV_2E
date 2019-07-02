using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
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
            var resultado = new List<Exemplo>();
            var parametros = new DynamicParameters();

            parametros.Add("ID", id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            var query = @" SELECT * FROM EXEMPLO WHERE EXEMPLOID > @ID";

            resultado = _mainContext.Connection.Query<Exemplo>(query, parametros).ToList();

            return resultado;
        }
    }
}
