using DapperExtensions.Mapper;

namespace P2E.SSO.Domain.Entities.Map
{
    public class ServicoMap : ClassMapper<Servico>
    {
        public ServicoMap()
        {
            Table("TB_SRV");

            Map(p => p.CD_SRV).Column("CD_SRV").Key(KeyType.Identity);
            Map(p => p.TXT_DEC).Column("TXT_DEC");
        }
    }
}
