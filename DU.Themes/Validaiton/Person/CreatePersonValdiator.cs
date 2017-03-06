using FluentValidation;

namespace DU.Themes.Validaiton.Person
{
    public class CreatePersonValdiator : AbstractValidator<DU.Themes.Entities.Person>
    {
        public CreatePersonValdiator()
        {
            this.RuleFor(x => x.FirstName).NotEmpty();
            this.RuleFor(x => x.LastName).NotEmpty();
            this.RuleFor(x => x.StudentIdentifier).NotEmpty();
            this.RuleFor(x => x.Email).NotEmpty();
        }
    }
}