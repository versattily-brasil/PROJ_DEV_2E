using Dapper;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using P2E.Main.Domain.Entities;
using P2E.Main.Domain.Repositories;
using P2E.Main.Infra.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace P2E.Main.Infra.Data.Repositories
{
    public class ServicoRepository : DapperRepository<Servico>, IServicoRepository
    {
        private readonly MainContext _mainContext;

        public ServicoRepository(MainContext mainContext) : base(mainContext.Connection)
        {
            _mainContext = mainContext;
        }

        public List<Servico> BuscarPorDescricao(string descricao)
        {
            var resultado = new List<Servico>();

            var parametros = new DynamicParameters();

            parametros.Add("TXT_DEC", descricao, DbType.String, ParameterDirection.Input);

            var query = @"SELECT * FROM TB_SRV WHERE TXT_DEC = @TXT_DEC";

            resultado = _mainContext.Connection.Query<Servico>(query, parametros).ToList();

            return resultado;
        }
    }
}
