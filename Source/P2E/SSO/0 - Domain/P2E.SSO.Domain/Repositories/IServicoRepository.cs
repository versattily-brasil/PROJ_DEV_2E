using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.SSO.Domain.Repositories
{
    public interface IServicoRepository : IDapperRepository<Servico>
    {
        DataPage<Servico> GetByPage(DataPage<Servico> page, string txt_dec);
        int GetTotalRows(PredicateGroup predicateGroup);
        bool ValidarDuplicidades(Servico servico);
    }
}
