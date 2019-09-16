using MicroOrm.Dapper.Repositories;
using P2E.Administrativo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Administrativo.Domain.Repositories
{
    public interface IAgendaRepository : IDapperRepository<Agenda>
    {
        bool ValidarDuplicidades(Agenda agenda);
    }
}
