using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;

namespace P2E.SSO.Domain.Repositories
{
    public interface IRotinaServicoRepository : IDapperRepository<RotinaServico>
    {
        bool ExcluirRotinaServico(int rotinaServicoId);
    }
}
