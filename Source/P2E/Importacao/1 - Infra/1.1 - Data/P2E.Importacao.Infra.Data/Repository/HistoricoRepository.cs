using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Importacao.Domain.Entities;
using P2E.Importacao.Domain.Repositories;
using P2E.Importacao.Infra.Data.DataContext;
using P2E.Shared.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2E.Importacao.Infra.Data.Repository
{
    public class HistoricoRepository : DapperRepository<Historico>, IHistoricoRepository
    {
        private readonly ImportacaoContext _historicoContext;

        public HistoricoRepository(ImportacaoContext histContext) : base(histContext.Connection)
        {
            _historicoContext = histContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>      
        /// <returns></returns>
        public DataPage<Historico> GetByPage(DataPage<Historico> dataPage, string descricao)
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
                var predicate = Predicates.Field<Historico>(p => p.TX_STATUS, Operator.Like, "%" + descricao.Trim() + "%", false);
                predicateGroup.Predicates.Add(predicate);
            }

            #endregion

            predicateGroup.Operator = (predicateGroup.Predicates.Count > 1 ? GroupOperator.And : GroupOperator.Or);

            dataPage.Items = _historicoContext.Connection.GetPage<Historico>(predicateGroup, listSort, dataPage.CurrentPage - 1, dataPage.PageSize).ToList();

            return dataPage;
        }
    }
}
