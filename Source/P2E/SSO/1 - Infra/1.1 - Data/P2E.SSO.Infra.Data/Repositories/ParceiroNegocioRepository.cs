using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class ParceiroNegocioRepository : DapperRepository<ParceiroNegocio>, IParceiroNegocioRepository
    {
        private readonly SSOContext _context; 

        public ParceiroNegocioRepository(SSOContext context) : base(context.Connection)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna uma lista paginada
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<ParceiroNegocio> GetByPage(int currentPage, int pageSize)
        {
            var sSQL = new StringBuilder();
            IEnumerable<ParceiroNegocio> results;
            var parameters = new DynamicParameters();

            parameters.Add("@Offset", currentPage == 1 ? 0 : currentPage * pageSize, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@PageSize", pageSize, DbType.String, ParameterDirection.Input);

            sSQL.Append("SELECT * FROM TB_PAR_NEG order by TXT_RZSOC");
            sSQL.Append("OFFSET @Offset ROWS");
            sSQL.Append("FETCH NEXT @PageSize ROWS ONLY");

            results = _context.Connection.Query<ParceiroNegocio>(sSQL.ToString(), parameters);

            return results;
        }
    }
}
