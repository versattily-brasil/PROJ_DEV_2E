using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.SSO.Domain.Entities.Map
{
    public class ParceiroNegocioModuloMap : ClassMapper<ParceiroNegocioModulo>
    {
        public ParceiroNegocioModuloMap()
        {
            Table("TB_PAR_SRV_MOD");

            Map(p => p.CD_PAR_SRV_MOD).Column("CD_PAR_SRV_MOD").Key(KeyType.Identity);
            Map(p => p.CD_PAR).Column("CD_PAR");
            Map(p => p.CD_SRV).Column("CD_SRV");
            Map(p => p.CD_MOD).Column("CD_MOD");
        }
    }
}
