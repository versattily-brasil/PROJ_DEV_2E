using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using P2E.Importacao.Domain.Entities.Map;
using DapperExtensions;
using Dapper;
using P2E.Shared.TypeHandler;

namespace P2E.SSO.Infra.Data.DataContext
{
    public class ImportacaoContext : IDisposable
    {
        public SqlConnection Connection { get; set; }

        public ImportacaoContext()
        {
            Connection = new SqlConnection(P2E.Shared.Configuration.ConnectionString);

            InicializaMapperDapper();
            var dialect = new DapperExtensions.Sql.SqlServerDialect();
            var conf = new DapperExtensionsConfiguration(null,
                new[]
                {
                    typeof(ImportacaoMap).Assembly
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
                typeof(ImportacaoMap).Assembly
            }
            );

            SqlMapper.AddTypeHandler(new DocumentTypeHandler());
        }
    }
}
