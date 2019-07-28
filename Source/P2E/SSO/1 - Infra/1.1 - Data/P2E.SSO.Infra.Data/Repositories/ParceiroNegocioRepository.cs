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
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="razaoSocial"></param>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public DataPage<ParceiroNegocio> GetByPage(DataPage<ParceiroNegocio> dataPage, string razaoSocial, string cnpj)
        {
            var sSQL = new StringBuilder();
            dataPage.OrderBy = dataPage.OrderBy ?? "txt_rzsoc";
            var sort = new Sort() { PropertyName = dataPage.OrderBy, Ascending = !dataPage.Descending  };

            #region Filtros
            var predicateGroup = new PredicateGroup();
            predicateGroup.Predicates = new List<IPredicate>();

            if (!string.IsNullOrEmpty(razaoSocial))
            {
                var predicate = Predicates.Field<ParceiroNegocio>(p => p.TXT_RZSOC, Operator.Like, razaoSocial.ToLower());
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
            listSort.Add(sort); 
            #endregion

            dataPage.Items = _context.Connection.GetPage<ParceiroNegocio>(predicateGroup, listSort, dataPage.CurrentPage - 1, dataPage.PageSize).ToList();

            dataPage.TotalItems = GetTotalRows(predicateGroup);
            return dataPage;
        }

        public int GetTotalRows(PredicateGroup predicateGroup)
        {
            return _context.Connection.GetList<ParceiroNegocio>(predicateGroup).Count();
        }
        
    }
}
