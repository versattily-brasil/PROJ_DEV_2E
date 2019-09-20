using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Administrativo.Infra.Data.DataContext;
using P2E.Automacao.Orquestrador.Lib.Entidades;

namespace P2E.Administrativo.Infra.Data.Repositories
{
    public class AgendaBotRepository : DapperRepository<AgendaBot>
    {
        private readonly OrquestradorContext _admContext;

        public AgendaBotRepository(OrquestradorContext admContext) : base(admContext.Connection)
        {
            _admContext = admContext;
        }
    }
}
