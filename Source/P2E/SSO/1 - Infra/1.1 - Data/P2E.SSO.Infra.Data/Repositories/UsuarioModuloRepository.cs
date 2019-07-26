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
    public class UsuarioModuloRepository : DapperRepository<UsuarioModulo>, IUsuarioModuloRepository
    {
        private readonly SSOContext _ssoContext;

        public UsuarioModuloRepository(SSOContext ssoContext) : base(ssoContext.Connection)
        {
            _ssoContext = ssoContext;
        }

        public List<UsuarioModulo> MetodoCustomizado(int id)
        {
            var resultado = new List<UsuarioModulo>();
            var parametros = new DynamicParameters();

            parametros.Add("CD_USR_MOD", id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            var query = @" SELECT * FROM TB_USR_MOD WHERE CD_USR_MOD > @CD_USR_MOD";

            resultado = _ssoContext.Connection.Query<UsuarioModulo>(query, parametros).ToList();

            return resultado;
        }
    }
}
