using Dapper;
using DapperExtensions;
using P2E.Shared.TypeHandler;
using P2E.SSO.Domain.Entities.Map;
using System;
using System.Data;
using System.Data.SqlClient;

namespace P2E.SSO.Infra.Data.DataContext
{
    public class SSOContext : IDisposable
    {
        public SqlConnection Connection { get; set; }

        public SSOContext()
        {
            Connection = new SqlConnection(P2E.Shared.Configuration.ConnectionString);

            InicializaMapperDapper();
            var dialect = new DapperExtensions.Sql.SqlServerDialect();
            var conf = new DapperExtensionsConfiguration(null,
                new[]
                {
                    typeof(ParceiroNegocioMap).Assembly
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
                typeof(ParceiroNegocioMap).Assembly
            }
            );

            SqlMapper.AddTypeHandler(new DocumentTypeHandler());
        }
    }
}
