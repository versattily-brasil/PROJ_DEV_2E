using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Automacao.Orquestrador.Lib.Entidades;
using P2E.Administrativo.Infra.Data.DataContext;

namespace P2E.Administrativo.Infra.Data.Repositories
{
    public class BotRepository : DapperRepository<Bot>
    {
        private readonly OrquestradorContext _ssoContext;

        public BotRepository(OrquestradorContext ssoContext) : base(ssoContext.Connection)
        {
            _ssoContext = ssoContext;
        }

      

        public int GetTotalRows(PredicateGroup predicateGroup)
        {
            return _ssoContext.Connection.GetList<Bot>(predicateGroup).Count();
        }

    }
}
