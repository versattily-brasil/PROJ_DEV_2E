﻿using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.SSO.Domain.Repositories
{
    public interface IRotinaGrupoOperacaoRepository : IDapperRepository<RotinaGrupoOperacao>
    {
        bool ExcluirRotinaGrupoOperacao(int rotinaGrupoOperacaoId);
    }
}