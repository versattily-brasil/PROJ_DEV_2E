using System;
using DapperExtensions.Mapper;

namespace P2E.SSO.Domain.Entities.Map
{
    public class OperacaoMap : ClassMapper<Operacao>
    {
        public OperacaoMap()
        {
            Table("TB_OPR");

            Map(p => p.CD_OPR).Column("CD_OPR").Key(KeyType.Identity);            
            Map(p => p.TX_DSC).Column("TX_DSC");
        }
    }
}
