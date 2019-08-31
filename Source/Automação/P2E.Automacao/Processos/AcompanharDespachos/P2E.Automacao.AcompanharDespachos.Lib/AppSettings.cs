using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.AcompanharDespachos.Lib
{
    public class AppSettings
    {
        public string ApiBaseURL { get; set; } = "http://localhost:7000/";

        public static implicit operator AppSettings(string v)
        {
            throw new NotImplementedException();
        }
    }
}
