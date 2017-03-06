using DU.Themes.Entities;
using DU.Themes.Validation;
using FluentValidation.Results;
using System.Linq;

namespace DU.Themes.Validaiton
{
    public class DeleteStudyYearValidator : ValidatorBase<StudyYear>
    {
        public DeleteStudyYearValidator(DbContext context)
            : base(context)
        {
            this.Custom(year => this.NotAttachedToThemes(year));
        }

        private ValidationFailure NotAttachedToThemes(StudyYear year)
        {
            if (this.Context.Themes.Any(x => x.WorkStart.Id == year.Id))
            {
                return new ValidationFailure(nameof(StudyYear.Start), ValidationErrors.YearIsInUse);
            }

            return null;
        }
    }
}