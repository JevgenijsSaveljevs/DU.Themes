using DU.Themes.Entities;
using FluentValidation.Validators;

namespace DU.Themes.Validaiton.CustomRules
{
    public class NewOrNeedImporvements : PropertyValidator
    {
        public NewOrNeedImporvements(string errorMsg = "TODO: NeedImporvements or New")
                        : base(errorMsg)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = (RequestStatus)context.PropertyValue;

            if(value == RequestStatus.New || value == RequestStatus.NeedImprovements)
            {
                return true;
            }

            return false;
        }
    }
}