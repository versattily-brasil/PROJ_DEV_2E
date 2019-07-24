using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class PAR_NEGORepository : DapperRepository<PAR_NEGO>, IPAR_NEGORepository
    {
        private readonly SSOContext _context; 

        public PAR_NEGORepository(SSOContext context) : base(context.Connection)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna uma lista paginada
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<PAR_NEGO> GetByPage(int page = 1, int pageSize = 10)
        {
            var sSQL = new StringBuilder();
            IEnumerable<PAR_NEGO> results;
            var parameters = new DynamicParameters();

            parameters.Add("@Offset", page  == 1 ? 0 : page * pageSize, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@PageSize", pageSize, DbType.String, ParameterDirection.Input);

            sSQL.Append("SELECT * FROM TB_PAR_NEG order by TXT_RZSOC");
            sSQL.Append("OFFSET @Offset ROWS");
            sSQL.Append("FETCH NEXT @PageSize ROWS ONLY");

            results = _context.Connection.Query<PAR_NEGO>(sSQL.ToString(), parameters
            );

            return results;
        }
    }
}
