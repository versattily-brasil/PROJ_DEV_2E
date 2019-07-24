using P2E.Shared.ValuesObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_PAR_NEGO")]
    public class PAR_NEGO
    {
        public long CD_PAR { get; set; }
        public string TXT_RZSOC { get; set; }
        public Document CNPJ { get; set; }
    }
}
