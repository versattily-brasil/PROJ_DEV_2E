using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;

namespace P2E.SSO.Domain.Repositories
{
    public interface IRotinaUsuarioOperacaoRepository : IDapperRepository<RotinaUsuarioOperacao>
    {
        bool ExcluirRotinaUsuarioOperacao(int rotinaUsuarioOperacaoId);
    }
}
