using System;
using System.Collections.Generic;
using System.Text;
using DapperExtensions.Mapper;

namespace P2E.SSO.Domain.Entities.Map
{
    public class ParceiroNegocioMap : ClassMapper<ParceiroNegocio>
    {
        public ParceiroNegocioMap()
        {
            Table("TB_PAR_NEG");

            Map(p => p.CD_PAR).Column("CD_PAR").Key(KeyType.Identity);
            Map(p => p.CNPJ).Column("CNPJ");
            Map(p => p.TXT_RZSOC).Column("TXT_RZSOC");
        }
    }
}
