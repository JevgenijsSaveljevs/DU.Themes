using DU.Themes.Models;
using FluentValidation;

namespace DU.Themes.Validation
{
    public class ValidatorBase<T> : AbstractValidator<T>
        where T : class
    {
        internal DbContext Context { get; set; }

        public ValidatorBase(DbContext context)
        {
            this.Context = context;
        }
    }
}