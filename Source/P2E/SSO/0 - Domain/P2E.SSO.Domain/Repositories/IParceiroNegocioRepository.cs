﻿using MicroOrm.Dapper.Repositories;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.SSO.Domain.Repositories
{
    public interface IParceiroNegocioRepository : IDapperRepository<ParceiroNegocio>
    {
        IEnumerable<ParceiroNegocio> GetByPage(int currenPage = 1, int pageSize = 10);
    }
}