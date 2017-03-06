using DU.Themes.Entities;
using DU.Themes.Models;
using DU.Themes.Validation;
using FluentValidation;

namespace DU.Themes.ValidaitonApiFilter
{
    public class RequestNeedImprovementsStatusValidator : ValidatorBase<DU.Themes.Entities.Request>
    {
        public RequestNeedImprovementsStatusValidator(DbContext context)
            : base(context)
        {
            RuleFor(x => x.Status).Equal(RequestStatus.NeedImprovements);
        }
    }
}