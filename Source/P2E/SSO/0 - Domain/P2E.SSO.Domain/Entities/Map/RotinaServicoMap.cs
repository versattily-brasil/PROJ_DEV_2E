using DapperExtensions.Mapper;

namespace P2E.SSO.Domain.Entities.Map
{
    public class RotinaServicoMap : ClassMapper<RotinaServico>
    {
        public RotinaServicoMap()
        {
            Table("TB_ROT_SRV");

            Map(p => p.CD_ROT_SRV).Column("CD_ROT_SRV").Key(KeyType.Identity);
            Map(p => p.CD_ROT).Column("CD_ROT");
            Map(p => p.CD_SRV).Column("CD_SRV");
        }
    }
}
