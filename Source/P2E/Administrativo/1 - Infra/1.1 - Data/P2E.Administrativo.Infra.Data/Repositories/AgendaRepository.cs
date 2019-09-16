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
using P2E.Administrativo.Domain.Entities;
using P2E.Administrativo.Domain.Repositories;
using P2E.Administrativo.Infra.Data.DataContext;

namespace P2E.Administrativo.Infra.Data.Repositories
{
    public class AgendaRepository : DapperRepository<Agenda>, IAgendaRepository
    {
        private readonly AdmContext _admContext;

        public AgendaRepository(AdmContext admContext) : base(admContext.Connection)
        {
            _admContext = admContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>      
        /// <returns></returns>
        public DataPage<Agenda> GetByPage(DataPage<Agenda> dataPage, string descricao)
        {

            var sSQL = new StringBuilder();
            dataPage.OrderBy = dataPage.OrderBy ?? "CD_AGENDA";
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
                var predicate = Predicates.Field<Agenda>(p => p.TX_DESCRICAO, Operator.Like, "%" + descricao.Trim() + "%", false);
                predicateGroup.Predicates.Add(predicate);
            }

            #endregion

            predicateGroup.Operator = (predicateGroup.Predicates.Count > 1 ? GroupOperator.And : GroupOperator.Or);

            dataPage.Items = _admContext.Connection.GetPage<Agenda>(predicateGroup, listSort, dataPage.CurrentPage - 1, dataPage.PageSize).ToList();

            dataPage.TotalItems = GetTotalRows(predicateGroup);

            return dataPage;
        }

        public int GetTotalRows(PredicateGroup predicateGroup)
        {
            return _admContext.Connection.GetList<Agenda>(predicateGroup).Count();
        }

        public bool ValidarDuplicidades(Agenda agenda)
        {
            if (agenda.CD_AGENDA > 0)
            {
                if (FindAll(p => p.TX_DESCRICAO == agenda.TX_DESCRICAO && p.CD_AGENDA != agenda.CD_AGENDA).Any())
                {
                    agenda.AddNotification("TX_DESCRICAO", $"A Descrição da Agenda {agenda.TX_DESCRICAO} já está cadastrada.");
                }
            }
            else
            {
                if (FindAll(p => p.TX_DESCRICAO == agenda.TX_DESCRICAO).Any())
                {
                    agenda.AddNotification("TX_DESCRICAO", $"A Descrição da Agenda {agenda.TX_DESCRICAO} já está cadastrada.");
                }
            }

            return agenda.IsValid();
        }


    }
}
