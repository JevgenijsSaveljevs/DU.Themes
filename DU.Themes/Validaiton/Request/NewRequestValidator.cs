using DU.Themes.Entities;
using DU.Themes.Validation;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Linq;
using DU.Themes.Infrastructure;
using DU.Themes.Validaiton;
using DU.Themes.Views;

namespace DU.Themes.ValidaitonApiFilter
{
    public class NewRequestValidator : ValidatorBase<DU.Themes.Entities.Request>
    {
        public NewRequestValidator(DbContext ctx)
            : base(ctx)
        {
            RuleFor(x => x.Student).NotNull().WithName(ValidationErrors.Students).WithMessage(ValidationErrors.NotEmpty);
            RuleFor(x => x.Teacher).NotNull().WithName(ValidationErrors.Teachers).WithMessage(ValidationErrors.NotEmpty);
            RuleFor(x => x.Status).Equal(RequestStatus.New);
            RuleFor(x => x.SeenByStudent).Equal(false);
            RuleFor(x => x.SeenByTeacher).Equal(false);
            RuleFor(x => x.CreatedOn).NotEqual(DateTime.MinValue);
            //RuleFor(x => x.Student).Must( x => x.Id == Thread)
            Custom(x => OnlyOneActiveRequest(ctx, x));
            Custom(x => NoThemesForStudent(ctx, x));

            RuleFor(x => x.Start).NotNull().WithName(ValidationErrors.StartYear).WithMessage(ValidationErrors.NotEmpty);
            RuleFor(x => x.Start).MustBeExistingYear(ctx).When(x => x.Start != null);

            RuleFor(x => x.End).NotNull().WithName(ValidationErrors.EndYear).WithMessage(ValidationErrors.NotEmpty); ;
            RuleFor(x => x.End).MustBeExistingYear(ctx).When(x => x.End != null);

            //RuleFor(x => x.Start.End).LessThanOrEqualTo(x => x.End.Start);

        }

        private ValidationFailure NoThemesForStudent(DbContext ctx, Entities.Request request)
        {
            if(request.Student == null)
            {
                return null;
            }

            if (ctx.Themes.Any(x => x.StudentId == request.Student.Id && x.Active == true))
            {
                return new ValidationFailure("Status", ValidationErrors.AlreadyHasActive);
            }

            return null;
        }

        private ValidationFailure OnlyOneActiveRequest(DbContext ctx, DU.Themes.Entities.Request request)
        {
            if (ctx.Requests
                .Where(x => x.Student.Id == request.Student.Id && x.Status != RequestStatus.Cancelled && x.Status != RequestStatus.Accepted).Any())
            {
                return new ValidationFailure("Status", ValidationErrors.AlreadyHasActive);
            }

            else
            {
                return null;
            }
        }
    }
}