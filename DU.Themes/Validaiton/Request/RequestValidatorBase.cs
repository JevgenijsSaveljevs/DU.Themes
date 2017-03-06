using DU.Themes.Infrastructure;
using DU.Themes.Validation;
using FluentValidation;

namespace DU.Themes.Validaiton.Request
{
    public class RequestValidatorBase : ValidatorBase<DU.Themes.Entities.Request>
    {
        public RequestValidatorBase(DbContext context)
            : base(context)
        {
            RuleFor(x => x.Start).NotNull();
            RuleFor(x => x.Start).MustBeExistingYear(context).When(x => x.Start != null);

            RuleFor(x => x.End).NotNull();
            RuleFor(x => x.End).MustBeExistingYear(context).When(x => x.End != null);

            RuleFor(x => x.Teacher).NotNull();
            RuleFor(x => x.Student).NotNull();
        }
    }
}