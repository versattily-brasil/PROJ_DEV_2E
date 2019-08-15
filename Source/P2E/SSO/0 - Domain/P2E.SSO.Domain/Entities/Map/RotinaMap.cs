using System;
using System.Collections.Generic;
using System.Text;
using DapperExtensions.Mapper;

namespace P2E.SSO.Domain.Entities.Map
{
    public class RotinaMap : ClassMapper<Rotina>
    {
        public RotinaMap()
        {
            Table("TB_ROT");

            Map(p => p.CD_ROT).Column("CD_ROT").Key(KeyType.Identity);
            Map(p => p.TX_NOME).Column("TX_NOME");
            Map(p => p.TX_DSC).Column("TX_DSC");
            Map(p => p.OP_TIPO).Column("OP_TIPO");
            Map(p => p.CD_SRV).Column("CD_SRV");
            Map(p => p.TX_URL).Column("TX_URL");
        }
    }
}
