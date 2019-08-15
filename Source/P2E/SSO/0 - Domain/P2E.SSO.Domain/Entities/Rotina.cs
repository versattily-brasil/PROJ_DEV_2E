using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Enum;
using P2E.Shared.Message;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_ROT")]
    public class Rotina : CustomNotifiable
    {
        public Rotina() {}

        public Rotina(string tx_nome, string tx_dsc, eTipoRotina op_tipo, int cd_srv)
        {
            TX_NOME = tx_nome?.Trim();
            TX_DSC = tx_dsc?.Trim();
            OP_TIPO = op_tipo;
            CD_SRV = cd_srv;

            IsValid();
        }

        [Key]
        [Identity]
        public int CD_ROT { get; set; }
        public string TX_NOME { get; set; }
        public string TX_DSC { get; set; }
        public eTipoRotina OP_TIPO { get; set; }
        public int CD_SRV { get; set; }

        public string TX_URL { get; set; }

        public List<RotinaServico> RotinaServico { get; set; } = new List<RotinaServico>();
        public List<Servico> Servicos { get; set; } = new List<Servico>();
        
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TX_NOME))
                AddNotification("TX_NOME", $"O Nome da Rotina é um campo obrigatório.");

            if (string.IsNullOrEmpty(TX_DSC))
                AddNotification("TX_DSC", $"A Descrição da Rotina é um campo obrigatório.");

            //if (OP_TIPO == 0)
            //    AddNotification("OP_TIPO", $"O Tipo da Rotina é um campo obrigatório.");

            return Valid;
        }

        public override string ToString() => $"{TX_NOME.ToString()}";

        public List<Operacao> Operacoes { get; set; }

        [NotMapped]
        public Servico Servico { get; set; }
    }
}
