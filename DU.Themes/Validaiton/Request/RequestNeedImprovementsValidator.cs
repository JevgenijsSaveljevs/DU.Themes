using DU.Themes.Validation;
using FluentValidation;

namespace DU.Themes.Validaiton.Request
{
    public class RequestNeedImprovementsValidator : ValidatorBase<DU.Themes.Entities.Request>
    {
        public RequestNeedImprovementsValidator(DbContext context)
            : base(context)
        {
            this.RuleFor(x => x.Status).NotEqual(Entities.RequestStatus.Cancelled).WithMessage("Nav iespējams atcelt jau atceltu pieteikumu");
            this.RuleFor(x => x.Status).NotEqual(Entities.RequestStatus.Accepted);
        }
    }
}