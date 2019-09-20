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
    public class AgendaExecRepository : DapperRepository<AgendaExec>
    {
        private readonly OrquestradorContext _admContext;

        public AgendaExecRepository(OrquestradorContext admContext) : base(admContext.Connection)
        {
            _admContext = admContext;
        }
    }
}
