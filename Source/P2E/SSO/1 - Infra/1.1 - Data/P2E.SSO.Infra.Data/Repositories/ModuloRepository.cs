using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class ModuloRepository : DapperRepository<TB_MOD>, IModuloRepository
    {
        private readonly SSOContext _ssoContext;

        public ModuloRepository(SSOContext ssoContext) : base(ssoContext.Connection)
        {
            _ssoContext = ssoContext;
        }

        public List<TB_MOD> MetodoCustomizado(int id)
        {
            var resultado = new List<TB_MOD>();
            var parametros = new DynamicParameters();

            parametros.Add("CD_MOD", id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            var query = @" SELECT * FROM TB_MOD WHERE CD_MOD > @CD_MOD";

            resultado = _ssoContext.Connection.Query<TB_MOD>(query, parametros).ToList();

            return resultado;
        }
    }
}
