using FluentValidator;
using FluentValidator.Validation;

namespace P2E.Shared.ValuesObject
{
    public class Name : Notifiable
    {
        public Name(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;

            AddNotifications(
                new ValidationContract()
                .Requires()
                .HasMinLen(FirstName, 3, "FirstName", "O nome deve conter no mínimo 3 caracteres")
                .HasMaxLen(FirstName, 30, "FirstName", "O nome deve conter no mácimo 30 caracteres")
                .HasMinLen(LastName, 3, "LastName", "O sobrenome deve conter no mínimo 3 caracteres")
                .HasMaxLen(LastName, 30, "LastName", "O sobrenome deve conter no mácimo 30 caracteres")
            );
        }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public override string ToString() => $"{FirstName}{LastName}";
    }
}
