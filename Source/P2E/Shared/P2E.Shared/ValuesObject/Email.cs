using FluentValidator;
using FluentValidator.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Shared.ValuesObject
{
    public class Email : Notifiable
    {
        public Email(string address)
        {
            this.Address = address;
            AddNotifications(
                new ValidationContract()
                .Requires()
                .IsEmail(address, "Email", "O email é inválido")
            );
        }
        public string Address { get; private set; }

        public override string ToString() => Address;
    }
}
