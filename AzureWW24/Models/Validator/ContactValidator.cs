using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureWW24.Models.Validator
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(contact => contact.Email).NotEmpty().EmailAddress();
            RuleFor(contact => contact.Phone).Matches(@"^\+\d{10,15}$").When(contact => !string.IsNullOrEmpty(contact.Phone));
        }
    }
}
