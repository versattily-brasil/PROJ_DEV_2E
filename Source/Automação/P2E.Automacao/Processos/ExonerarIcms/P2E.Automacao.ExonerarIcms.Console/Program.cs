using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace P2E.Automacao.ExonerarIcms.Console
{
    class Program
    {
        static async Task Main(string[] args)        {
            var lib = new P2E.Automacao.ExonerarIcms.Lib.Work();
            await lib.ExecutarAsync();
        }
    }
}
