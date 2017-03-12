using DU.Themes.Entities;
using DU.Themes.Infrastructure;
using DU.Themes.Models;
using DU.Themes.Validation;

namespace DU.Themes.Validaiton
{
    public class SamePersonValidator : ValidatorBase<Theme>// ThemeValidatorBase
    {
        public SamePersonValidator(DbContext context, ThemeModel theme)
            : base(context)
        {
            this.RuleFor(x => x.Teacher).MustBeSamePersonAs(theme.Teacher);
            this.RuleFor(x => x.Student).MustBeSamePersonAs(theme.Student);
        }
    }
}