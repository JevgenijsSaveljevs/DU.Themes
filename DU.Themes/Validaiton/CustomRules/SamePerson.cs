using DU.Themes.Models;
using FluentValidation.Validators;

namespace DU.Themes
{
    public class SamePerson<T> : PropertyValidator
       where T : DU.Themes.Entities.Person
    {
        public SamePerson(PersonModel model)
            : base("Nedrīkst mainīt pasniedzēju")
        {
            this.ToCompare = model;
        }

        public PersonModel ToCompare { get; private set; }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var prop = context.PropertyValue as T;

            return this.ToCompare?.Id == prop?.Id;
        }
    }
}