using FluentValidation;

namespace DU.Themes.Validaiton
{
    public class CreateStudentValidator : AbstractValidator<DU.Themes.Entities.Person>
    {
        public CreateStudentValidator()
        {
            this.RuleFor(x => x.FirstName).NotEmpty();
            this.RuleFor(x => x.LastName).NotEmpty();
            this.RuleFor(x => x.StudentIdentifier).NotEmpty();
            this.RuleFor(x => x.Email).NotEmpty();
            this.RuleFor(x => x.ProgramLevel).NotEmpty();
            this.RuleFor(x => x.ProgramName).NotEmpty();
            this.RuleFor(x => x.Year).NotEmpty();
            this.RuleFor(x => x.StudyForm).NotEmpty();
        }
    }
}