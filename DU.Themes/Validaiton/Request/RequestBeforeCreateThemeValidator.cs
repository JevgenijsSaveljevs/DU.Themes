using DU.Themes.Validation;
using FluentValidation;

namespace DU.Themes.Validaiton.Request
{
    public class RequestBeforeCreateThemeValidator : ValidatorBase<DU.Themes.Entities.Request>
    {
        public RequestBeforeCreateThemeValidator(DbContext context)
            : base(context)
        {
            this.RuleFor(x => x.Status).NotEqual(Entities.RequestStatus.Accepted);
            this.RuleFor(x => x.Status).NotEqual(Entities.RequestStatus.Cancelled);
        }
    }
}