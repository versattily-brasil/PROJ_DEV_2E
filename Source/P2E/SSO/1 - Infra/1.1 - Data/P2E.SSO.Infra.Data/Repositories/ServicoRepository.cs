using Dapper;
using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class ServicoRepository : DapperRepository<Servico>, IServicoRepository
    {
        private readonly SSOContext _ssoContext;

        public ServicoRepository(SSOContext ssoContext) : base(ssoContext.Connection)
        {
            _ssoContext = ssoContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>
        /// <returns></returns>
        public DataPage<Servico> GetByPage(DataPage<Servico> dataPage, string descricao)
        {
            var sSQL = new StringBuilder();
            dataPage.OrderBy = dataPage.OrderBy ?? "TXT_DEC";
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
                var predicate = Predicates.Field<Servico>(p => p.TXT_DEC, Operator.Like, "%" + descricao.Trim() + "%", false);
                predicateGroup.Predicates.Add(predicate);
            }

            #endregion

            predicateGroup.Operator = (predicateGroup.Predicates.Count > 1 ? GroupOperator.And : GroupOperator.Or);

            dataPage.Items = _ssoContext.Connection.GetPage<Servico>(predicateGroup, listSort, dataPage.CurrentPage - 1, dataPage.PageSize).ToList();

            dataPage.TotalItems = GetTotalRows(predicateGroup);

            return dataPage;
        }

        public int GetTotalRows(PredicateGroup predicateGroup)
        {
            return _ssoContext.Connection.GetList<Servico>(predicateGroup).Count();
        }

        public bool ValidarDuplicidades(Servico servico)
        {
            if (servico.CD_SRV > 0)
            {
                if (FindAll(p => p.TXT_DEC == servico.TXT_DEC && p.CD_SRV != servico.CD_SRV).Any())
                {
                    servico.AddNotification("TXT_DEC", $"A Descrição do Serviço {servico.TXT_DEC} já está cadastrado.");
                }
            }
            else
            {
                if (FindAll(p => p.TXT_DEC == servico.TXT_DEC).Any())
                {
                    servico.AddNotification("TXT_DEC", $"A Descrição do Serviço {servico.TXT_DEC} já está cadastrado.");
                }
            }

            return servico.IsValid();
        }
    }
}
