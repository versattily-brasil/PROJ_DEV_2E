using FluentValidator;
using MicroOrm.Dapper.Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Main.Domain.Entities
{
    public class Exemplo : Notifiable
    {
        public Exemplo()
        {

        }

        public Exemplo(string descricao, decimal valor)
        {
            Descricao = descricao;
            Valor = valor;
        }

        [Key]
        [Identity]
        public int ExemploId { get; private set; }
        public string Descricao { get; private set; }
        public decimal Valor { get; private set; }

        public bool IsValid()
        {
            if(string.IsNullOrEmpty(Descricao))
                AddNotification("Descricao", $"Descrição é um campo obrigatório.");

            if (Valor <= 0)
                AddNotification("Valor", $"Valor é um campo obrigatório.");

            return Valid;
        }

        public override string ToString() => $"{Descricao.ToString()}";
    }
}
