using MicroOrm.Dapper.Repositories;
using P2E.Main.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Main.Domain.Repositories
{
    public interface IServicoRepository : IDapperRepository<Servico>
    {
        List<Servico> BuscarPorDescricao(string descricao);
    }
}
