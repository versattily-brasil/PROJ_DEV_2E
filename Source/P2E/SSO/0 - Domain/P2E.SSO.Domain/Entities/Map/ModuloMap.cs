using DapperExtensions.Mapper;
namespace P2E.SSO.Domain.Entities.Map
{
    public class ModuloMap : ClassMapper<Modulo>
    {
        public ModuloMap()
        {
            Table("TB_MOD");

            Map(p => p.CD_MOD).Column("CD_MOD").Key(KeyType.Identity);
            Map(p => p.TX_DSC).Column("TX_DSC");
        }
    }
}
