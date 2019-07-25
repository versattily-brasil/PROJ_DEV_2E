using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Shared.Model;
using P2E.Shared.ValuesObject;
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
        public IEnumerable<ParceiroNegocio> GetByPage(string razaoSocial, string cnpj, int currentPage, int pageSize)
        {
            var sSQL = new StringBuilder();
            IEnumerable<ParceiroNegocio> results;

            #region Filtros
            var predicateGroup = new PredicateGroup();

            if (!string.IsNullOrEmpty(razaoSocial))
            {
                var predicate = Predicates.Field<ParceiroNegocio>(p => p.TXT_RZSOC, Operator.Eq, razaoSocial);
                predicateGroup.Predicates.Add(predicate);
            }

            if (!string.IsNullOrEmpty(cnpj))
            {
                var predicate = Predicates.Field<ParceiroNegocio>(p => p.CNPJ, Operator.Eq, new Document(cnpj));
                predicateGroup.Predicates.Add(predicate);
            }
            #endregion

            #region Ordenação
            var listSort = new List<ISort>();
            listSort.Add(new Sort() { PropertyName = "TXT_RZSOC", Ascending = true }); 
            #endregion

            results = _context.Connection.GetPage<ParceiroNegocio>(predicateGroup, listSort, currentPage, pageSize);

            return results.ToList();
        }
    }
}
