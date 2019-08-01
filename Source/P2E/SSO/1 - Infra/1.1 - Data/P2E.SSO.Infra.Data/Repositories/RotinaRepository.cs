using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class RotinaRepository : DapperRepository<Rotina>, IRotinaRepository
    {
        private readonly SSOContext _context;

        public RotinaRepository(SSOContext context) : base(context.Connection)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        public DataPage<Rotina> GetByPage(DataPage<Rotina> dataPage, string descricao, string nome)
        {

            var sSQL = new StringBuilder();
            dataPage.OrderBy = dataPage.OrderBy ?? "tx_dsc";
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
                var predicate = Predicates.Field<Rotina>(p => p.TX_DSC, Operator.Like, "%" + descricao.Trim() + "%", false);
                predicateGroup.Predicates.Add(predicate);
            }

            if (!string.IsNullOrEmpty(nome))
            {
                var predicate = Predicates.Field<Rotina>(p => p.TX_NOME, Operator.Eq, nome);
                predicateGroup.Predicates.Add(predicate);
            }

            #endregion

            predicateGroup.Operator = (predicateGroup.Predicates.Count > 1 ? GroupOperator.And : GroupOperator.Or);

            dataPage.Items = _context.Connection.GetPage<Rotina>(predicateGroup, listSort, dataPage.CurrentPage - 1, dataPage.PageSize).ToList();

            dataPage.TotalItems = GetTotalRows(predicateGroup);
            return dataPage;
        }

        public int GetTotalRows(PredicateGroup predicateGroup)
        {
            return _context.Connection.GetList<ParceiroNegocio>(predicateGroup).Count();
        }
    }
}
