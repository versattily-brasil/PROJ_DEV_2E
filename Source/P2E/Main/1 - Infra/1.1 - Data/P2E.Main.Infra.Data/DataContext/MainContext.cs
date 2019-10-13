using System;
using System.Data;
using System.Data.SqlClient;

namespace P2E.Main.Infra.Data.DataContext
{
    public class MainContext : IDisposable
    {
        public SqlConnection Connection { get; set; }

        public MainContext()
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
