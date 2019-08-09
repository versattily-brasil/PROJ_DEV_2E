using DapperExtensions.Mapper;

namespace P2E.SSO.Domain.Entities.Map
{
    public class RotinaGrupoOperacaoMap : ClassMapper<RotinaGrupoOperacao>
    {
        public RotinaGrupoOperacaoMap()
        {
            Table("TB_ROT_GRP");

            Map(p => p.CD_ROT_GRP).Column("CD_ROT_GRP").Key(KeyType.Identity);
            Map(p => p.CD_ROT).Column("CD_ROT");
            Map(p => p.CD_GRP).Column("CD_GRP");
            Map(p => p.CD_OPR).Column("CD_OPR");
        }
    }
}
