using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Importacao.Domain.Entities;
using P2E.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Importacao.Domain.Repositories
{
    public interface ICEMercanteItensRepository : IDapperRepository<CEMercanteItens>
    {
        DataPage<CEMercanteItens> GetByPage( DataPage<CEMercanteItens> page, string tx_descricao );

        int GetTotalRows( PredicateGroup predicateGroup );

        void DeleteAll();
    }
}
