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
        public long CD_PAR_SRV_MOD { get; set; }
        public long CD_PAR { get; set; }
        public long CD_SRV { get; set; }
        public long CD_MOD { get; set; }

        public Modulo Modulo { get; set; }
        public Servico Servico { get; set; }
    }
}

