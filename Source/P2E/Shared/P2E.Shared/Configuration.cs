using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Shared
{
    public static class Configuration
    {
        //public static string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\ProgramData\Lerron\TFS\Projeto2E\2E-dev\Source\P2E\Shared\P2E.Shared\DBLocal\DbLocal.mdf;Integrated Security=True;Connect Timeout=30";
        public static string ConnectionString = @"Data Source=LERRON-INSP\SQLEXPRESS;Initial Catalog = P2E; User ID = sa; Password=devtime;Integrated Security=True;Connect Timeout=30";
       // public static string ConnectionString = @"Server=localhost;Database=p2e;Trusted_Connection=True;";
        
    }
}
