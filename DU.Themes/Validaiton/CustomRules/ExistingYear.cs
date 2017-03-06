using DU.Themes.Entities;
using FluentValidation.Validators;
using System.Linq;

namespace DU.Themes
{
    public class ExistingYear<T> : PropertyValidator
        where T : StudyYear
    {
        public ExistingYear(DbContext context)
            : base("Year Should be registred in system")
        {
            this.Context = context;
        }

        public DbContext Context { get; private set; }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var studyYear = context.PropertyValue as StudyYear;
            var code = studyYear?.GetCode();

            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            if (!this.Context.StudyYears.Any(x => x.Code == code))
            {
                return false;
            }


            return true;
        }
    }
}