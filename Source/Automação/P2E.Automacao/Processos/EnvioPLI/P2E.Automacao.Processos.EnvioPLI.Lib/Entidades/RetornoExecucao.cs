using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.EnviarPLI.Lib.Entidades
{
    
    public class RetornoExecucao
    {
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public eStatus Status { get; set; }
        public string[] Mensagens { get; set; }
    }

    public enum eStatus
    {
        CONCLUIDO_COM_ERRO,
        CONCLUIDO_SEM_ERRO
    }
}
