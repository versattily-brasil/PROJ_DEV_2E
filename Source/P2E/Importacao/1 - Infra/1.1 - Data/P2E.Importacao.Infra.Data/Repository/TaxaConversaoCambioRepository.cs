﻿using DapperExtensions;
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
    public class TaxaConversaoCambioRepository : DapperRepository<TaxaConversaoCambio>, ITaxaConversaoCambioRepository
    {
        private readonly ImportacaoContext _importacaoContext;

        public TaxaConversaoCambioRepository(ImportacaoContext impContext) : base(impContext.Connection)
        {
            _importacaoContext = impContext;
        }

        public DataPage<TaxaConversaoCambio> GetByPage(DataPage<TaxaConversaoCambio> dataPage, string descricao)
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
                var predicate = Predicates.Field<TaxaConversaoCambio>(p => p.CD_TAXA_CAMBIO, Operator.Like, "%" + descricao.Trim() + "%", false);
                predicateGroup.Predicates.Add(predicate);
            }

            #endregion

            predicateGroup.Operator = (predicateGroup.Predicates.Count > 1 ? GroupOperator.And : GroupOperator.Or);

            dataPage.Items = _importacaoContext.Connection.GetPage<TaxaConversaoCambio>(predicateGroup, listSort, dataPage.CurrentPage - 1, dataPage.PageSize).ToList();

            dataPage.TotalItems = GetTotalRows(predicateGroup);

            return dataPage;
        }

        public int GetTotalRows(PredicateGroup predicateGroup)
        {
            return _importacaoContext.Connection.GetList<TaxaConversaoCambio>(predicateGroup).Count();
        }
    }
}