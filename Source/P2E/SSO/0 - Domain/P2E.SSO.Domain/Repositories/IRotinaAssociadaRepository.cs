using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;

namespace P2E.SSO.Domain.Repositories
{
    public interface IRotinaAssociadaRepository : IDapperRepository<RotinaAssociada>
    {
        bool ExcluirRotinaAssociada(long cd_rot_principal);
    }
}
