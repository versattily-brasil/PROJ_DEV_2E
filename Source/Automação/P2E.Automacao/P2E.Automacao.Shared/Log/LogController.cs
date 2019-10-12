using P2E.Automacao.Shared.Extensions;
using P2E.Automacao.Shared.Log.DataContext;
using P2E.Automacao.Shared.Log.Entidades;
using P2E.Automacao.Shared.Log.Enum;
using P2E.Automacao.Shared.Log.Repositories;
using System;
using System.Threading;

namespace P2E.Automacao.Shared.Log
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

            if (parentId > 0)
            {
                using (var context = new LogContext())
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

                RoboInterrompido(parentId);
            }
        }

        private static void RegistrarLogAgenda(eTipoLog tipo, int parentId, string msg, string adicional, LogContext context)
        {
            var agendaExecLogRep = new AgendaExecLogRepository(context);
            agendaExecLogRep.Insert(new Entidades.AgendaExecLog()
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

        private static void RoboInterrompido(int cdBotExec)
        {
            using (var context = new LogContext())
            {
                var botExecRep = new MicroOrm.Dapper.Repositories.DapperRepository<BotExec>(context.Connection);
                var execBot = botExecRep.Find(p => p.CD_BOT_EXEC == cdBotExec);

                if (execBot.OP_STATUS_BOT_EXEC == Shared.Enum.eStatusExec.Interrompido)
                {
                    Thread.CurrentThread.Abort(Shared.Enum.eStatusExec.Interrompido);
                }
            }
        }
    }
}