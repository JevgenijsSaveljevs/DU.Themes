using DU.Themes.Infrastructure;
using DU.Themes.Models;
using DU.Themes.Validation;

namespace DU.Themes.ValidaitonApiFilter
{
    public class RequestPersonsNotChangedValidator : ValidatorBase<DU.Themes.Entities.Request>
    {
        public RequestPersonsNotChangedValidator(DbContext context, RequestModel request)
            : base(context)
        {
            this.RuleFor(x => x.Teacher).MustBeSamePersonAs(request.Teacher);
            this.RuleFor(x => x.Student).MustBeSamePersonAs(request.Student);
            this.RuleFor(x => x.Reviewer).MustBeSamePersonAs(request.Reviewer);
        }
    }

}