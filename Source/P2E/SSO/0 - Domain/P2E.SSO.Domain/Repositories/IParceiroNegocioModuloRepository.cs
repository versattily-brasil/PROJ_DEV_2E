using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.SSO.Domain.Repositories
{
    public interface IParceiroNegocioModuloRepository : IDapperRepository<ParceiroNegocioModulo>
    {
        List<ParceiroNegocioModulo> GetParceiroNegocioModulos(long cd_par, long cd_srv, long cd_mod);
    }
}
