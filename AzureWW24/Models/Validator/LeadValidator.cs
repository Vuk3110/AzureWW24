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
            RuleFor(lead => lead.Source).NotEmpty().WithMessage("invalid source");
            RuleFor(lead => lead.Date).Must(IsValidDate); 
            RuleFor(lead => lead.Size).GreaterThan(0);
      

            RuleFor(lead => lead.Name).SetValidator(new NameValidator());
            RuleFor(lead => lead.Contact).SetValidator(new ContactValidator());
            RuleFor(lead => lead.From).SetValidator(new LocationValidator());
            RuleFor(lead => lead.To).SetValidator(new LocationValidator());

            RuleFor(lead => lead.From.State).Must(ValidateState).WithMessage("Invalid 'From' state acronym.");
            RuleFor(lead => lead.To.State).Must(ValidateState).WithMessage("Invalid 'To' state acronym.");

            RuleFor(lead => new { lead.From.Zip, lead.From.State })
                .Must(x => ValidateStateAndZip(x.Zip, x.State))
                
                .WithMessage("'From' ZIP code is invalid for the state.");

            RuleFor(lead => new { lead.To.Zip, lead.To.State })
                .Must(x => ValidateStateAndZip(x.Zip, x.State))
                
                .WithMessage("'To' ZIP code is invalid for the state.");
        }

        private bool IsValidDate(string date)
        {
            return !date.Equals(default(DateOnly));
        }

        private static bool ValidateState(string acronym) => stateModel.Any(state => state.StateAcronym == acronym);

        private static bool ValidateStateAndZip(int zip, string acronym)
        {
            return stateModel.Any(state => acronym == state.StateAcronym && zip >= state.FromZip && zip <= state.ToZip);
        }

        public static List<StateModel> stateModel = new List<StateModel>()
       {
           new(){ StateAcronym = "AL", FromZip = 35004, ToZip = 36925 },
           new(){ StateAcronym = "AK", FromZip = 99501, ToZip = 99950 },
           new(){ StateAcronym = "AZ", FromZip = 85001, ToZip = 86556 },
           new(){ StateAcronym = "AR", FromZip = 71601, ToZip = 72959 },
           new(){ StateAcronym = "CA", FromZip = 90001, ToZip = 96162 },
           new(){ StateAcronym = "CO", FromZip = 80001, ToZip = 81658 },
           new(){ StateAcronym = "CT", FromZip = 06001, ToZip = 06928 },
           new(){ StateAcronym = "CT", FromZip = 06001, ToZip = 06928 },
           new(){ StateAcronym = "DE", FromZip = 19701, ToZip = 19980 },
           new(){ StateAcronym = "FL", FromZip = 32003, ToZip = 34997 },
           new(){ StateAcronym = "GA", FromZip = 30002, ToZip = 39901 },

           new(){ StateAcronym = "HI", FromZip = 96701, ToZip = 96898 },
           new(){ StateAcronym = "ID", FromZip = 83201, ToZip = 83877 },
           new(){ StateAcronym = "IL", FromZip = 60001, ToZip = 62999 },
           new(){ StateAcronym = "IN", FromZip = 46001, ToZip = 47997 },
           new(){ StateAcronym = "IA", FromZip = 50001, ToZip = 52809 },
           new(){ StateAcronym = "KS", FromZip = 66002, ToZip = 67954 },
           new(){ StateAcronym = "KY", FromZip = 40003, ToZip = 42788 },
           new(){ StateAcronym = "LA", FromZip = 70001, ToZip = 71497 },
           new(){ StateAcronym = "ME", FromZip = 03901, ToZip = 04992 },
           new(){ StateAcronym = "MD", FromZip = 20588, ToZip = 21930 },

           new(){ StateAcronym = "MA", FromZip = 01001, ToZip = 05544 },
           new(){ StateAcronym = "MI", FromZip = 48001, ToZip = 49971 },
           new(){ StateAcronym = "MN", FromZip = 55001, ToZip = 56763 },
           new(){ StateAcronym = "MS", FromZip = 38601, ToZip = 39776 },
           new(){ StateAcronym = "MO", FromZip = 63001, ToZip = 65899 },
           new(){ StateAcronym = "MT", FromZip = 59001, ToZip = 59937 },
           new(){ StateAcronym = "NE", FromZip = 68001, ToZip = 69367 },
           new(){ StateAcronym = "NV", FromZip = 88901, ToZip = 89883 },
           new(){ StateAcronym = "NH", FromZip = 03031, ToZip = 03897 },
           new(){ StateAcronym = "NJ", FromZip = 07001, ToZip = 08989 },

           new(){ StateAcronym = "NM", FromZip = 87001, ToZip = 88439 },
           new(){ StateAcronym = "NY", FromZip = 00501, ToZip = 14925 },
           new(){ StateAcronym = "NC", FromZip = 27006, ToZip = 28909 },
           new(){ StateAcronym = "ND", FromZip = 58001, ToZip = 58856 },
           new(){ StateAcronym = "OH", FromZip = 43001, ToZip = 45999 },
           new(){ StateAcronym = "OK", FromZip = 73001, ToZip = 74966 },
           new(){ StateAcronym = "OR", FromZip = 97001, ToZip = 97920 },
           new(){ StateAcronym = "PA", FromZip = 15001, ToZip = 19640 },
           new(){ StateAcronym = "RI", FromZip = 02801, ToZip = 02940 },
           new(){ StateAcronym = "SC", FromZip = 29001, ToZip = 29945 },

           new(){ StateAcronym = "SD", FromZip = 57001, ToZip = 57799 },
           new(){ StateAcronym = "TN", FromZip = 37010, ToZip = 38589 },
           new(){ StateAcronym = "TX", FromZip = 73301, ToZip = 88595 },
           new(){ StateAcronym = "UT", FromZip = 84001, ToZip = 84791 },
           new(){ StateAcronym = "VT", FromZip = 05001, ToZip = 05907 },
           new(){ StateAcronym = "VA", FromZip = 20101, ToZip = 24658 },
           new(){ StateAcronym = "WA", FromZip = 98001, ToZip = 99403 },
           new(){ StateAcronym = "WV", FromZip = 24701, ToZip = 26886 },
           new(){ StateAcronym = "WI", FromZip = 53001, ToZip = 54990 },
           new(){ StateAcronym = "WY", FromZip = 82001, ToZip = 83414 }
       };


    }


}
