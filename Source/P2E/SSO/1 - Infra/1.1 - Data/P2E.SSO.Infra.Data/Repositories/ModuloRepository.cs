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

        //public List<Modulo> MetodoCustomizado(int id)
        //{
        //    var resultado = new List<Modulo>();
        //    var parametros = new DynamicParameters();

        //    parametros.Add("CD_MOD", id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

        //    var query = @" SELECT * FROM TB_MOD WHERE CD_MOD > @CD_MOD";

        //    resultado = _ssoContext.Connection.Query<Modulo>(query, parametros).ToList();

        //    return resultado;
        //}
    }
}
