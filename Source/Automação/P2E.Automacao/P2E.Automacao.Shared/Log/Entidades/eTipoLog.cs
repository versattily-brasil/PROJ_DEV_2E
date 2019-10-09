using System.ComponentModel;

namespace P2E.Automacao.Shared.Log.Enum
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
