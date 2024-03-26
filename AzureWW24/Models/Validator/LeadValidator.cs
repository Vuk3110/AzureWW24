using FluentValidation;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureWW24.Models.Validator
{
    public class LeadValidator : AbstractValidator<Lead>
    {
        public LeadValidator()
        {
            RuleFor(lead => lead.Source).NotEmpty();
            RuleFor(lead => lead.Date).Must(IsValidDate); 
            RuleFor(lead => lead.Size).GreaterThan(0);
            RuleFor(lead => lead.ServiceType).NotEmpty();

            RuleFor(lead => lead.Name).SetValidator(new NameValidator());
            RuleFor(lead => lead.Contact).SetValidator(new ContactValidator());
            RuleFor(lead => lead.From).SetValidator(new LocationValidator());
            RuleFor(lead => lead.To).SetValidator(new LocationValidator());
        }

        private bool IsValidDate(string date)
        {
            return !date.Equals(default(DateOnly));
        }
    }

    
}
