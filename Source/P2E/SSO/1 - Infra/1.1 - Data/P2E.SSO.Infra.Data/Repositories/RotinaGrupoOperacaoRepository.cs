using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class RotinaGrupoOperacaoRepository : DapperRepository<RotinaGrupoOperacao>, IRotinaGrupoOperacaoRepository
    {
        private readonly SSOContext _context;

        public RotinaGrupoOperacaoRepository(SSOContext context) : base(context.Connection)
        {
            _context = context;
        }        

        public bool ExcluirRotinaGrupoOperacao(int rotinaGrupoOperacaoId)
        {
            bool result = this.Delete(o => o.CD_ROT_GRP == rotinaGrupoOperacaoId);

            return result;
        }        
    }
}
