using MicroOrm.Dapper.Repositories;
using P2E.Importacao.Domain.Entities;
using P2E.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Importacao.Domain.Repositories
{
    public interface IHistoricoRepository : IDapperRepository<Historico>
    {
        DataPage<Historico> GetByPage(DataPage<Historico> page, string tx_status);
    }
}
