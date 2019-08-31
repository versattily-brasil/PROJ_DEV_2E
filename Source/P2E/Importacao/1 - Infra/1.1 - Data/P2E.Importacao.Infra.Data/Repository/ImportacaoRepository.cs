using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Importacao.Domain.Entities;
using P2E.Importacao.Domain.Repositories;
using P2E.Shared.Model;
using P2E.SSO.Infra.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2E.Importacao.Infra.Data.Repository
{
    public class ImportacaoRepository : DapperRepository<TBImportacao>, IImportacaoRepository
    {
        private readonly ImportacaoContext _importacaoContext;

        public ImportacaoRepository(ImportacaoContext impContext) : base(impContext.Connection)
        {
            _importacaoContext = impContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>      
        /// <returns></returns>
        public DataPage<TBImportacao> GetByPage(DataPage<TBImportacao> dataPage, string descricao)
        {

            var sSQL = new StringBuilder();
            dataPage.OrderBy = dataPage.OrderBy ?? "tx_status";
            var sort = new Sort() { PropertyName = dataPage.OrderBy, Ascending = !dataPage.Descending };

            #region Ordenação
            var listSort = new List<ISort>();
            listSort.Add(sort);
            #endregion

            #region Filtros
            var predicateGroup = new PredicateGroup();
            predicateGroup.Predicates = new List<IPredicate>();

            if (!string.IsNullOrEmpty(descricao))
            {
                var predicate = Predicates.Field<TBImportacao>(p => p.TX_STATUS, Operator.Like, "%" + descricao.Trim() + "%", false);
                predicateGroup.Predicates.Add(predicate);
            }

            #endregion

            predicateGroup.Operator = (predicateGroup.Predicates.Count > 1 ? GroupOperator.And : GroupOperator.Or);

            dataPage.Items = _importacaoContext.Connection.GetPage<TBImportacao>(predicateGroup, listSort, dataPage.CurrentPage - 1, dataPage.PageSize).ToList();

            return dataPage;
        }
    }
}
