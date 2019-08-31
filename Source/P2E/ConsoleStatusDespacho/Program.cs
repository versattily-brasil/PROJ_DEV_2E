using StatusDespacho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleStatusDespacho
{
    class Program
    {
        static void Main(string[] args)
        {
            StatusDespachos prompt = new StatusDespachos( false);

            bool checkeValue = prompt.Executar();

            if (checkeValue == true)
                prompt.executaBat();
            else
                Console.WriteLine("Error ao gravar arquivo");

            //StatusAcompanharDespacho.Executar();
        }
    }
}
