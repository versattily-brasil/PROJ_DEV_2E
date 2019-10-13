using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;

namespace P2E.SSO.Domain.Repositories
{
    public interface IParceiroNegocioRepository : IDapperRepository<ParceiroNegocio>
    {
        DataPage<ParceiroNegocio> GetByPage(DataPage<ParceiroNegocio> page, string razaoSocial, string cnpj);

        int GetTotalRows(PredicateGroup predicateGroup);

        bool ValidarDuplicidades(ParceiroNegocio parceiroNegocio);
    }
}
