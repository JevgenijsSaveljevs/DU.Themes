using DU.Themes.Entities;
using DU.Themes.Validation;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace DU.Themes.Validaiton
{
    public class CreateStudyYearValidator : ValidatorBase<StudyYear>
    {
        public CreateStudyYearValidator(DbContext context)
            : base(context)
        {
            this.Custom(year => this.IsNewestYear(year));
            this.RuleFor(x => x.End).GreaterThan(x => x.Start).WithMessage(ValidationErrors.EndLesserThanStart);
        }

        private ValidationFailure IsNewestYear(StudyYear year)
        {
            if (this.Context.StudyYears.Any(x => x.Start.Year == year.Start.Year))
            {
                return new ValidationFailure(nameof(StudyYear.Start), ValidationErrors.YearAlreadyExists);
            }

            if (this.Context.StudyYears.Any(x => x.End.Year == year.End.Year))
            {
                return new ValidationFailure(nameof(StudyYear.End), ValidationErrors.YearAlreadyExists);
            }

            return null;
        }
    }
}