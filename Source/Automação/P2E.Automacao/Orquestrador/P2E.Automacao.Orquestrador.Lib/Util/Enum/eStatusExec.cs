using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Orquestrador.Lib.Util.Enum
{
    public enum eStatusExec
    {
        [Description("Não Programado")]
        Nao_Programado,
        [Description("Programado")]
        Programado,
        [Description("Aguardando Processamento")]
        Aguardando_Processamento,
        [Description("Executando")]
        Executando,
        [Description("Falha")]
        Falha,
        [Description("Concluído")]
        Conclúído
    }
}
