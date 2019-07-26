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
    public class TabelaAuxiliarRepository : DapperRepository<TabelaAuxiliar>, ITabelaAuxiliarRepository
    {
        private readonly MainContext _mainContext;

        public TabelaAuxiliarRepository(MainContext mainContext) : base(mainContext.Connection)
        {
            _mainContext = mainContext;
        }

        public List<TabelaAuxiliar> MetodoCustomizado(string id)
        {
            var resultado = new List<TabelaAuxiliar>();
            var parametros = new DynamicParameters();

            parametros.Add("TX_TABELA", id, System.Data.DbType.String, System.Data.ParameterDirection.Input);

            var query = @" SELECT * FROM TB_AUX_TAB WHERE TX_TABELA > @TX_TABELA";

            resultado = _mainContext.Connection.Query<TabelaAuxiliar>(query, parametros).ToList();

            return resultado;
        }
    }
}
