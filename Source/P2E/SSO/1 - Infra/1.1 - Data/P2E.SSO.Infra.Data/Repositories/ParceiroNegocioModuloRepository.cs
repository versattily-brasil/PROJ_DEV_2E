using Dapper;
using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class ParceiroNegocioModuloRepository : DapperRepository<ParceiroNegocioModulo>, IParceiroNegocioModuloRepository
    {
        private readonly SSOContext _context;

        public ParceiroNegocioModuloRepository(SSOContext context) : base(context.Connection)
        {
            _context = context;
        }

        public List<ParceiroNegocioModulo> GetParceiroNegocioModulos(long cd_par, long cd_srv, long cd_mod)
        {
            var resultado = new List<ParceiroNegocioModulo>();
            var parametros = new DynamicParameters();

            parametros.Add("CD_PAR", cd_par, System.Data.DbType.Int16, System.Data.ParameterDirection.Input);
            parametros.Add("CD_SRV", cd_srv, System.Data.DbType.Int16, System.Data.ParameterDirection.Input);
            parametros.Add("CD_MOD", cd_mod, System.Data.DbType.Int16, System.Data.ParameterDirection.Input);

            var query = @" SELECT * FROM TB_PAR_SRV_MOD WHERE TX_TABELA > @TX_TABELA";

            resultado = _context.Connection.Query<ParceiroNegocioModulo>(query, parametros).ToList();

            return resultado;
        }
    }
}
