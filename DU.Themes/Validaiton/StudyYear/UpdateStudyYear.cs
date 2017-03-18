using System;
using DU.Themes.Entities;
using DU.Themes.Validation;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace DU.Themes.Validaiton
{
    public class UpdateStudyYear : ValidatorBase<StudyYear>
    {
        public UpdateStudyYear(DbContext context)
            : base(context)
        {
            this.Custom(x => this.UniqueBegining(x));
        }

        private ValidationFailure UniqueBegining(StudyYear year)
        {
            ////if(this.Context.StudyYears.Any(x => x.End  year.Start ))
            return null;
        }
    }
}