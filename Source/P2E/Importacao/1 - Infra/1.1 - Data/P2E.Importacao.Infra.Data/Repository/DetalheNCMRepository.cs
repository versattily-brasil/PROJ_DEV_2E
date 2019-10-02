using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Importacao.Domain.Entities;
using P2E.Importacao.Domain.Repositories;
using P2E.Importacao.Infra.Data.DataContext;
using P2E.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2E.Importacao.Infra.Data.Repository
{
    public class DetalheNCMRepository : DapperRepository<DetalheNCM>, IDetalheNCMRepository
    {
        private readonly ImportacaoContext _DetalheNCMContext;

        public DetalheNCMRepository(ImportacaoContext histContext) : base(histContext.Connection)
        {
            _DetalheNCMContext = histContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>      
        /// <returns></returns>
        public DataPage<DetalheNCM> GetByPage(DataPage<DetalheNCM> dataPage, string descricao)
        {

            var sSQL = new StringBuilder();
            dataPage.OrderBy = dataPage.OrderBy ?? "cd_det_ncm";
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
                var predicate = Predicates.Field<DetalheNCM>(p => p.CD_DET_NCM, Operator.Like, "%" + descricao.Trim() + "%", false);
                predicateGroup.Predicates.Add(predicate);
            }

            #endregion

            predicateGroup.Operator = (predicateGroup.Predicates.Count > 1 ? GroupOperator.And : GroupOperator.Or);

            dataPage.Items = _DetalheNCMContext.Connection.GetPage<DetalheNCM>(predicateGroup, listSort, dataPage.CurrentPage - 1, dataPage.PageSize).ToList();

            return dataPage;
        }
    }
}
