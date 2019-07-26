using MicroOrm.Dapper.Repositories;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.SSO.Domain.Repositories
{
    public interface IParceiroNegocioRepository : IDapperRepository<ParceiroNegocio>
    {
        DataPage<ParceiroNegocio> GetByPage(DataPage<ParceiroNegocio> page, string razaoSocial, string cnpj);
    }
}
