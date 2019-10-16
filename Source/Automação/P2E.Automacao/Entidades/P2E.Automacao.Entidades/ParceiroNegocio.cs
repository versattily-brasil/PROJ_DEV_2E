using MicroOrm.Dapper.Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Entidades
{
    public class ParceiroNegocio
    {  
        public long CD_PAR { get; set; }
        public string TXT_RZSOC { get; set; }
        public string CNPJ { get; set; }
        public string TX_EMAIL { get; set; }
    }
}
