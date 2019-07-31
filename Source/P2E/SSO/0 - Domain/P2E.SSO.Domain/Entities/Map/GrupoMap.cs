using DapperExtensions.Mapper;

namespace P2E.SSO.Domain.Entities.Map
{
    public class GrupoMap : ClassMapper<Grupo>
    {
        public GrupoMap()
        {
            Table("TB_GRP");

            Map(p => p.CD_GRP).Column("CD_GRP").Key(KeyType.Identity);
            Map(p => p.TX_DSC).Column("TX_DSC");
        }
    }
}
