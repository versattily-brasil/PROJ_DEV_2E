using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;

using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class UsuarioRepository : DapperRepository<Usuario>, IUsuarioRepository
    {
        private readonly SSOContext _ssoContext;

        public UsuarioRepository(SSOContext ssoContext) : base(ssoContext.Connection)
        {
            _ssoContext = ssoContext;
        }
    }
}
