using P2E.Automacao.Orquestrador.DataContext;
using P2E.Automacao.Orquestrador.Lib.Util.Enum;
using P2E.Automacao.Orquestrador.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P2E.Automacao.Orquestrador.Lib.Util.Extensions;

namespace P2E.Automacao.Orquestrador.Lib.Util.Extensions
{
    public static class LogController
    {
        public static void RegistrarLog(string msg, eTipoLog tipo = eTipoLog.INFO)
        {
            EscreverLog(msg, tipo.GetDescription());
        }

        public static void RegistrarLog(string msg, eTipoLog tipo, int parentId, string origem, string adicional = "")
        {
            EscreverLog(msg, tipo.GetDescription());

            using (var context = new OrquestradorContext())
            {
                if (origem == "agenda")
                {
                    RegistrarLogAgenda(tipo, parentId, msg, adicional, context);
                }

                if (origem == "bot")
                {
                    var agendaExecRep = new BotExecLogRepository(context);
                    agendaExecRep.Insert(new Entidades.BotExecLog()
                    {
                        CD_BOT_EXEC = parentId,
                        DT_DATAHORA_REG = DateTime.Now,
                        OP_TIPO_LOG = tipo.GetDescription(),
                        TX_MENSAGEM = msg,
                        TX_INF_ADICIONAL = adicional
                    });
                }
            }
        }

        private static void RegistrarLogAgenda(eTipoLog tipo, int parentId, string msg, string adicional, OrquestradorContext context)
        {
            var agendaExecRep = new AgendaExecLogRepository(context);
            agendaExecRep.Insert(new Entidades.AgendaExecLog()
            {
                CD_AGENDA_EXEC = parentId,
                DT_DATAHORA_REG = DateTime.Now,
                OP_TIPO_LOG = tipo.GetDescription(),
                TX_MENSAGEM = msg,
                TX_INF_ADICIONAL = adicional
            });
        }

        private static void EscreverLog(string msg, string tipo)
        { 
            Console.WriteLine($"{tipo} [ {DateTime.Now} ]: {msg}");
        }
    }
}