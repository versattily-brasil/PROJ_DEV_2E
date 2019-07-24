using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.SSO.Domain.Repositories
{
    public interface IPAR_NEGORepository : IDapperRepository<PAR_NEGO>
    {
        IEnumerable<PAR_NEGO> GetByPage(int page = 1, int pageSize = 10);
    }
}
