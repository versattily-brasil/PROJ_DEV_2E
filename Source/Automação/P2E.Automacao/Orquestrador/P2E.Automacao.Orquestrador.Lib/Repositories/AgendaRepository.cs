using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Automacao.Orquestrador.DataContext;
using P2E.Automacao.Orquestrador.Lib.Entidades;

namespace P2E.Automacao.Orquestrador.Repositories
{
    public class AgendaRepository : DapperRepository<Agenda>
    {
        private readonly OrquestradorContext _orquestradorContext;

        public AgendaRepository(OrquestradorContext orquestradorContext) : base(orquestradorContext.Connection)
        {
            _orquestradorContext = orquestradorContext;
        }


        public int GetTotalRows(PredicateGroup predicateGroup)
        {
            return _orquestradorContext.Connection.GetList<Agenda>(predicateGroup).Count();
        }

        //public bool ValidarDuplicidades(Agenda agenda)
        //{
        //    if (agenda.CD_AGENDA > 0)
        //    {
        //        if (FindAll(p => p.TX_DESCRICAO == agenda.TX_DESCRICAO && p.CD_AGENDA != agenda.CD_AGENDA).Any())
        //        {
        //            agenda.AddNotification("TX_DESCRICAO", $"A Descrição da Agenda {agenda.TX_DESCRICAO} já está cadastrada.");
        //        }
        //    }
        //    else
        //    {
        //        if (FindAll(p => p.TX_DESCRICAO == agenda.TX_DESCRICAO).Any())
        //        {
        //            agenda.AddNotification("TX_DESCRICAO", $"A Descrição da Agenda {agenda.TX_DESCRICAO} já está cadastrada.");
        //        }
        //    }

        //    return agenda.IsValid();
        //}


    }
}
