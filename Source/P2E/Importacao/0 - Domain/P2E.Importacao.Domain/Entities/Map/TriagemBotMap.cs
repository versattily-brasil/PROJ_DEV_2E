using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Importacao.Domain.Entities.Map
{
    public class TriagemBotMap : ClassMapper<TriagemBot>
    {
        public TriagemBotMap()
        {
            Table("TB_TRIAGEM_BOT");

            Map(p => p.CD_TRIAGEM).Column("CD_TRIAGEM").Key(KeyType.Identity);
            Map(p => p.NR_DI).Column("NR_DI");            
            Map(p => p.DT_REGISTRO).Column("DT_REGISTRO");
            Map(p => p.CD_PAR_NEG).Column("CD_PAR_NEG");
            Map(p => p.OP_ACOMP_DESP_IMP).Column("OP_ACOMP_DESP_IMP");
            Map(p => p.OP_COMPROV_IMP).Column("OP_COMPROV_IMP");
            Map(p => p.OP_EXONERA_ICMS).Column("OP_EXONERA_ICMS");
            Map(p => p.OP_EXTRATO_PDF_XML).Column("OP_EXTRATO_PDF_XML");
            Map(p => p.OP_EXTRATO_RETIF).Column("OP_EXTRATO_RETIF");
            Map(p => p.OP_STATUS_DESEMB).Column("OP_STATUS_DESEMB");
            Map(p => p.OP_STATUS).Column("OP_STATUS");
            Map(p => p.OP_TELA_DEBITO).Column("OP_TELA_DEBITO");
        }
    }
}
