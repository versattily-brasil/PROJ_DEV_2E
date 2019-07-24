using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace P2E.SSO.Infra.Data.DataContext
{
    public class SSOContext : IDisposable
    {
        public SqlConnection Connection { get; set; }

        public SSOContext()
        {
            Connection = new SqlConnection(P2E.Shared.Configuration.ConnectionString);
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Close();
            }
        }
    }
}
