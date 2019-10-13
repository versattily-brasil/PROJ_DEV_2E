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
    public class OperacaoRepository : DapperRepository<Operacao>, IOperacaoRepository
    {
        private readonly SSOContext _context;

        public OperacaoRepository(SSOContext context) : base(context.Connection)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>
        /// <returns></returns>
        public DataPage<Operacao> GetByPage(DataPage<Operacao> dataPage, string descricao)
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
                var predicate = Predicates.Field<Operacao>(p => p.TX_DSC, Operator.Like, "%" + descricao.Trim() + "%", false);
                predicateGroup.Predicates.Add(predicate);
            }

            #endregion

            predicateGroup.Operator = (predicateGroup.Predicates.Count > 1 ? GroupOperator.And : GroupOperator.Or);

            dataPage.Items = _context.Connection.GetPage<Operacao>(predicateGroup, listSort, dataPage.CurrentPage - 1, dataPage.PageSize).ToList();

            dataPage.TotalItems = GetTotalRows(predicateGroup);
            return dataPage;
        }

        public int GetTotalRows(PredicateGroup predicateGroup)
        {
            return _context.Connection.GetList<Operacao>(predicateGroup).Count();
        }

        public bool ValidarDuplicidades(Operacao operacao)
        {
            if (operacao.CD_OPR > 0)
            {
                if (FindAll(p => p.TX_DSC == operacao.TX_DSC && p.CD_OPR != operacao.CD_OPR).Any())
                {
                    operacao.AddNotification("TX_DSC", $"A Descrição da Operação {operacao.TX_DSC} já está cadastrada.");
                }
            }
            else
            {
                if (FindAll(p => p.TX_DSC == operacao.TX_DSC).Any())
                {
                    operacao.AddNotification("TX_DSC", $"A Descrição da Operação {operacao.TX_DSC} já está cadastrada.");
                }
            }

            return operacao.IsValid();
        }
    }
}
