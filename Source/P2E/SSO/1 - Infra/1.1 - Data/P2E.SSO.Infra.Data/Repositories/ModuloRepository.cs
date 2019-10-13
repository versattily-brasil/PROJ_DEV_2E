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
    public class ModuloRepository : DapperRepository<Modulo>, IModuloRepository
    {
        private readonly SSOContext _ssoContext;

        public ModuloRepository(SSOContext ssoContext) : base(ssoContext.Connection)
        {
            _ssoContext = ssoContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>      
        /// <returns></returns>
        public DataPage<Modulo> GetByPage(DataPage<Modulo> dataPage, string descricao)
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
                var predicate = Predicates.Field<Modulo>(p => p.TX_DSC, Operator.Like, "%" + descricao.Trim() + "%", false);
                predicateGroup.Predicates.Add(predicate);
            }

            #endregion

            predicateGroup.Operator = (predicateGroup.Predicates.Count > 1 ? GroupOperator.And : GroupOperator.Or);

            dataPage.Items = _ssoContext.Connection.GetPage<Modulo>(predicateGroup, listSort, dataPage.CurrentPage - 1, dataPage.PageSize).ToList();

            dataPage.TotalItems = GetTotalRows(predicateGroup);
            return dataPage;
        }

        public int GetTotalRows(PredicateGroup predicateGroup)
        {
            return _ssoContext.Connection.GetList<Modulo>(predicateGroup).Count();
        }

        public bool ValidarDuplicidades(Modulo modulo)
        {
            if (modulo.CD_MOD > 0)
            {
                if (FindAll(p => p.TX_DSC == modulo.TX_DSC && p.CD_MOD != modulo.CD_MOD).Any())
                {
                    modulo.AddNotification("TX_DSC", $"A Descrição do Módulo {modulo.TX_DSC} já está cadastrado.");
                }
            }
            else
            {
                if (FindAll(p => p.TX_DSC == modulo.TX_DSC).Any())
                {
                    modulo.AddNotification("TX_DSC", $"A Descrição do Módulo {modulo.TX_DSC} já está cadastrado.");
                }
            }

            return modulo.IsValid();
        }
    }
}
