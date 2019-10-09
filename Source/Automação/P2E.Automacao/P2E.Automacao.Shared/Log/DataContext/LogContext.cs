using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using P2E.Automacao.Shared.Log.Entidades.Map;
using Dapper;
using System.Configuration;
using DapperExtensions;

namespace P2E.Automacao.Shared.Log.DataContext
{
    public class LogContext : IDisposable
    {
        public SqlConnection Connection { get; set; }

        public LogContext()
        {
            string conStr = ConfigurationManager.ConnectionStrings["conexao"].ToString();
            Connection = new SqlConnection(conStr);

            InicializaMapperDapper();
            var dialect = new DapperExtensions.Sql.SqlServerDialect();
            var conf = new DapperExtensionsConfiguration(null,
                new[]
                {
                    typeof(AgendaExecLogMap).Assembly,
                    typeof(BotExecLogMap).Assembly
                }, dialect);
            DapperExtensions.DapperExtensions.Configure(conf);
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Close();
            }
        }

        public static void InicializaMapperDapper()
        {
            DapperExtensions.DapperExtensions.SetMappingAssemblies(new[]
            {
                typeof(AgendaExecLogMap).Assembly,
                typeof(BotExecLogMap).Assembly
            }
            );
        }
    }
}
