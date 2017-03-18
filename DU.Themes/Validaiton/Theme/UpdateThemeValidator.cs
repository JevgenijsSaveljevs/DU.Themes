using DU.Themes.Entities;
using FluentValidation.Results;
using System.Linq;

namespace DU.Themes.Validaiton
{
    public class UpdateThemeValidator : ThemeValidatorBase
    {
        public UpdateThemeValidator(DbContext context)
            : base(context)
        {

        }

        public override ValidationFailure OnlyOneActive(Theme theme)
        {
            if (this.Context.Themes.Any(x => x.StudentId == theme.Student.Id && x.Active == true && x.Id != theme.Id))
            {
                return new ValidationFailure("Id", ValidationErrors.OnlyOneActiveTheme);
            }

            return null;
        }
    }
}