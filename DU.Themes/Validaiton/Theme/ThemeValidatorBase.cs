using System;
using DU.Themes.Entities;
using DU.Themes.Infrastructure;
using DU.Themes.Validation;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace DU.Themes.Validaiton
{
    public class ThemeValidatorBase : ValidatorBase<Theme>
    {
        public ThemeValidatorBase(DbContext context)
            : base(context)
        {
            this.RuleFor(x => x.Teacher).NotNull().WithName(ValidationErrors.Teachers).WithMessage(ValidationErrors.NotEmpty);
            this.RuleFor(x => x.Student).NotNull().WithName(ValidationErrors.Students).WithMessage(ValidationErrors.NotEmpty);

            this.RuleFor(x => x.ThemeENG).NotEmpty().WithName(ValidationErrors.ThemeEng).WithMessage(ValidationErrors.NotEmpty);
            this.RuleFor(x => x.ThemeLV).NotEmpty().WithName(ValidationErrors.ThemeLv).WithMessage(ValidationErrors.NotEmpty);

            this.RuleFor(x => x.Start).MustBeExistingYear(context);
            this.RuleFor(x => x.End).MustBeExistingYear(context);

            this.Custom(x => this.OnlyOneActive(x));
        }

        public virtual ValidationFailure OnlyOneActive(Theme theme)
        {
            if(this.Context.Themes.Any(x => x.StudentId == theme.Student.Id && x.Active == true))
            {
                return new ValidationFailure("Id", ValidationErrors.OnlyOneActiveTheme);
            }

            return null;
        }
    }
}