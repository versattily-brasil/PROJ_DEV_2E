using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class RotinaUsuarioOperacaoRepository : DapperRepository<RotinaUsuarioOperacao>, IRotinaUsuarioOperacaoRepository
    {
        private readonly SSOContext _context;

        public RotinaUsuarioOperacaoRepository(SSOContext context) : base(context.Connection)
        {
            _context = context;
        }

        public bool ExcluirRotinaUsuarioOperacao(int rotinaUsuarioOperacaoId)
        {
            bool result = this.Delete(o => o.CD_USR_ROT_OPR == rotinaUsuarioOperacaoId);

            return result;
        }
    }
}
