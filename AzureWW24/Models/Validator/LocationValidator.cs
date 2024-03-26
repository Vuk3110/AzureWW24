using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureWW24.Models.Validator
{
    public class LocationValidator : AbstractValidator<Location>
    {
        public LocationValidator()
        {
            RuleFor(location => location.State).Length(2);
            RuleFor(location => location.City).NotEmpty();
            RuleFor(location => location.Zip).InclusiveBetween(10000, 99999);
        }

     

        

    }
}
