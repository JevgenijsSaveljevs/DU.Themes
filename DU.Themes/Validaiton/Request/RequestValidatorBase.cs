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
            RuleFor(x => x.Start).NotNull().WithName(ValidationErrors.StartYear).WithMessage(ValidationErrors.NotEmpty);
            RuleFor(x => x.Start).MustBeExistingYear(context).When(x => x.Start != null);

            RuleFor(x => x.End).NotNull().WithName(ValidationErrors.EndYear).WithMessage(ValidationErrors.NotEmpty); ;
            RuleFor(x => x.End).MustBeExistingYear(context).When(x => x.End != null);

            RuleFor(x => x.Teacher).NotNull().WithName(ValidationErrors.Teachers).WithMessage(ValidationErrors.NotEmpty); ;
            RuleFor(x => x.Student).NotNull().WithName(ValidationErrors.Students).WithMessage(ValidationErrors.NotEmpty); ;
        }
    }
}