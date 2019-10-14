using DapperExtensions.Mapper;

namespace P2E.SSO.Domain.Entities.Map
{
    public class RotinaAssociadaMap : ClassMapper<RotinaAssociada>
    {
        public RotinaAssociadaMap()
        {
            Table("TB_ROT_ASS");

            Map(p => p.CD_ASS).Column("CD_ASS").Key(KeyType.Identity);
            Map(p => p.CD_ROT_PRINCIPAL).Column("CD_ROT_PRINCIPAL");
            Map(p => p.CD_ROT_ASS).Column("CD_ROT_ASS");
        }
    }
}
