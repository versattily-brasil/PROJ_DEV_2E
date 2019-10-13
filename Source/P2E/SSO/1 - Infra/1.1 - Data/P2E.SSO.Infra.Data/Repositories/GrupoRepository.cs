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
    public class GrupoRepository : DapperRepository<Grupo>, IGrupoRepository
    {
        private readonly SSOContext _ssoContext;

        public GrupoRepository(SSOContext ssoContext) : base(ssoContext.Connection)
        {
            _ssoContext = ssoContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>      
        /// <returns></returns>
        public DataPage<Grupo> GetByPage(DataPage<Grupo> dataPage, string descricao)
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
                var predicate = Predicates.Field<Grupo>(p => p.TX_DSC, Operator.Like, "%" + descricao.Trim() + "%", false);
                predicateGroup.Predicates.Add(predicate);
            }

            #endregion

            predicateGroup.Operator = (predicateGroup.Predicates.Count > 1 ? GroupOperator.And : GroupOperator.Or);

            dataPage.Items = _ssoContext.Connection.GetPage<Grupo>(predicateGroup, listSort, dataPage.CurrentPage - 1, dataPage.PageSize).ToList();

            dataPage.TotalItems = GetTotalRows(predicateGroup);

            return dataPage;
        }

        public int GetTotalRows(PredicateGroup predicateGroup)
        {
            return _ssoContext.Connection.GetList<Grupo>(predicateGroup).Count();
        }

        public bool ValidarDuplicidades(Grupo grupo)
        {
            if (grupo.CD_GRP > 0)
            {
                if (FindAll(p => p.TX_DSC == grupo.TX_DSC && p.CD_GRP != grupo.CD_GRP).Any())
                {
                    grupo.AddNotification("TX_DSC", $"A Descrição do Grupo {grupo.TX_DSC} já está cadastrada.");
                }
            }
            else
            {
                if (FindAll(p => p.TX_DSC == grupo.TX_DSC).Any())
                {
                    grupo.AddNotification("TX_DSC", $"A Descrição do Grupo {grupo.TX_DSC} já está cadastrada.");
                }
            }

            return grupo.IsValid();
        }
    }
}
