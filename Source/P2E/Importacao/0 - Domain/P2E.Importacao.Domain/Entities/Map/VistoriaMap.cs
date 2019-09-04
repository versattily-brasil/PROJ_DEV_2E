using DapperExtensions.Mapper;

namespace P2E.Importacao.Domain.Entities.Map
{
    public class VistoriaMap : ClassMapper<Vistoria>
    {
        public VistoriaMap()
        {
            Table("TB_VIST");

            Map(p => p.CD_VIST).Column("CD_VIST").Key(KeyType.Identity);
            Map(p => p.CD_IMP).Column("CD_IMP");
            Map(p => p.TX_DESC).Column("TX_DESC");
        }
    }
}
