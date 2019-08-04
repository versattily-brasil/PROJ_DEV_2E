using MicroOrm.Dapper.Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_PAR_SRV_MOD")]
    public class ParceiroNegocioServicoModulo
    {
        [Key]
        [Identity]
        public int CD_PAR_SRV_MOD { get; set; }
        public int CD_PAR { get; set; }
        public int CD_SRV { get; set; }
        public int CD_MOD { get; set; }
    }
}

