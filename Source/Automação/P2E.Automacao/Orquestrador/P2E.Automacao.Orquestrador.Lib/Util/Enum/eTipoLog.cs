using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Orquestrador.Lib.Util.Enum
{
    public enum eTipoLog
    {
        [Description("Info")]
        INFO,
        [Description("Erro")]
        ERRO,
        [Description("Alerta")]
        ALERTA
    }
}
