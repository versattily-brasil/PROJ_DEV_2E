using DapperExtensions.Mapper;

namespace P2E.Automacao.Shared.Log.Entidades.Map
{
    public class AgendaMap : ClassMapper<Agenda>
    {
        public AgendaMap()
        {
            Table("TB_AGENDA");

            Map(p => p.CD_AGENDA).Column("CD_AGENDA").Key(KeyType.Identity);
            Map(p => p.DT_DATA_EXEC_PROG).Column("DT_DATA_EXEC_PROG");
            Map(p => p.DT_DATA_FIM_ULTIMA_EXEC).Column("DT_DATA_FIM_ULTIMA_EXEC");
            Map(p => p.DT_DATA_INICIO_ULTIMA_EXEC).Column("DT_DATA_INICIO_ULTIMA_EXEC");
            Map(p => p.HR_HORA_EXEC_PROG).Column("HR_HORA_EXEC_PROG");
            Map(p => p.OP_ATIVO).Column("OP_ATIVO");
            Map(p => p.OP_FORMA_EXEC).Column("OP_FORMA_EXEC");
            Map(p => p.OP_REPETE).Column("OP_REPETE");
            Map(p => p.OP_TIPO_REP).Column("OP_TIPO_REP");
            Map(p => p.OP_STATUS).Column("OP_STATUS");
            Map(p => p.TX_DESCRICAO).Column("TX_DESCRICAO");
            Map(p => p.CD_ULTIMA_EXEC).Column("CD_ULTIMA_EXEC");
        }
    }
}
