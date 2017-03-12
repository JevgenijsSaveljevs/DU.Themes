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
            this.RuleFor(x => x.Teacher).NotNull();
            this.RuleFor(x => x.Student).NotNull();

            this.RuleFor(x => x.ThemeENG).NotEmpty();
            this.RuleFor(x => x.ThemeLV).NotEmpty();

            this.RuleFor(x => x.Start).MustBeExistingYear(context);
            this.RuleFor(x => x.End).MustBeExistingYear(context);

            this.Custom(x => this.OnlyOneActive(x));
        }

        private ValidationFailure OnlyOneActive(Theme theme)
        {
            if(this.Context.Themes.Any(x => x.StudentId == theme.Student.Id && x.Active == true))
            {
                return new ValidationFailure("Id", ValidationErrors.OnlyOneActiveTheme);
            }

            return null;
        }
    }
}