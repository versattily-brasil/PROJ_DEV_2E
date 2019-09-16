using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Orquestrador.Lib.Util.Enum
{
    public enum eStatusExec
    {
        Nao_Programado,
        Programado,
        Aguardando_Processamento,
        Executando,
        Falha,
        Conclúído
    }
}
