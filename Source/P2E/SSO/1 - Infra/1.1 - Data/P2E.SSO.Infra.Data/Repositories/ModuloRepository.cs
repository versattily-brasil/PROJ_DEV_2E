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
    public class ModuloRepository : DapperRepository<Modulo>, IModuloRepository
    {
        private readonly SSOContext _ssoContext;

        public ModuloRepository(SSOContext ssoContext) : base(ssoContext.Connection)
        {
            _ssoContext = ssoContext;
        }

        public List<Modulo> MetodoCustomizado(int id)
        {
            var resultado = new List<Modulo>();
            var parametros = new DynamicParameters();

            parametros.Add("CD_MOD", id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            var query = @" SELECT * FROM TB_MOD WHERE CD_MOD > @CD_MOD";

            resultado = _ssoContext.Connection.Query<Modulo>(query, parametros).ToList();

            return resultado;
        }
    }
}
