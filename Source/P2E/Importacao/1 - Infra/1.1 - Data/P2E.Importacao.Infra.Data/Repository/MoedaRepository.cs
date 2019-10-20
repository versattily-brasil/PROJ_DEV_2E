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
    public class MoedaRepository : DapperRepository<Moeda>, IMoedaRepository
    {
        private readonly ImportacaoContext _importacaoContext;

        public MoedaRepository(ImportacaoContext impContext) : base(impContext.Connection)
        {
            _importacaoContext = impContext;
        }

        public void DeleteAll()
        {
            //string sql = "delete from TB_DETALHE_NCM ";
            //using (var connection = new SqlConnection(_importacaoContext.Connection.ConnectionString))
            //{
            //    connection.Execute(sql);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>      
        /// <returns></returns>
        public DataPage<Moeda> GetByPage(DataPage<Moeda> dataPage, string descricao)
        {

            var sSQL = new StringBuilder();
            dataPage.OrderBy = dataPage.OrderBy ?? "tx_descricao";
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
                var predicate = Predicates.Field<Moeda>(p => p.CD_MOEDA, Operator.Like, "%" + descricao.Trim() + "%", false);
                predicateGroup.Predicates.Add(predicate);
            }

            #endregion

            predicateGroup.Operator = (predicateGroup.Predicates.Count > 1 ? GroupOperator.And : GroupOperator.Or);

            dataPage.Items = _importacaoContext.Connection.GetPage<Moeda>(predicateGroup, listSort, dataPage.CurrentPage - 1, dataPage.PageSize).ToList();

            dataPage.TotalItems = GetTotalRows(predicateGroup);

            return dataPage;
        }

        public int GetTotalRows(PredicateGroup predicateGroup)
        {
            return _importacaoContext.Connection.GetList<Moeda>(predicateGroup).Count();
        }
    }
}
