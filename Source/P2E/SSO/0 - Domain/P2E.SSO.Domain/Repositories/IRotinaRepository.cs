using System.Collections.Generic;
using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;

namespace P2E.SSO.Domain.Repositories
{
    public interface IRotinaRepository : IDapperRepository<Rotina>
    {
        DataPage<Rotina> GetByPage(DataPage<Rotina> page, string descricao, string nome);

        int GetTotalRows(PredicateGroup predicateGroup);

        bool ValidarDuplicidades(Rotina rotina);
    }
}
