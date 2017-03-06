using DU.Themes.Entities;
using DU.Themes.Models;
using DU.Themes.Validaiton;
using FluentValidation;

namespace DU.Themes.Infrastructure
{
    public partial class Extensions
    {
        public static IRuleBuilderOptions<TClass, TProp> MustBeExistingYear<TClass, TProp>(
            this IRuleBuilder<TClass, TProp> ruleBuilder,
            DbContext ctx)
            where TClass : class
            where TProp : StudyYear
        {
            return ruleBuilder.SetValidator(new ExistingYear<TProp>(ctx));
        }

        public static IRuleBuilderOptions<TClass, TProp> MustBeSamePersonAs<TClass, TProp>(
            this IRuleBuilder<TClass, TProp> ruleBuilder,
            PersonModel model)
            where TClass : class
            where TProp : Person
        {
            return ruleBuilder.SetValidator(new SamePerson<TProp>(model));
        }

        //public static IRuleBuilderOptions<TClass, TProp> NewOrNeedImporvements<TClass, TProp>(
        //    this IRuleBuilder<TClass, TProp> ruleBuilder,
        //    PersonModel model)
        //    where TClass : class
        //    where TProp : RequestStatus
        //{
        //    return ruleBuilder.SetValidator(new SamePerson<TProp>(model));
        //}

        //NewOrNeedImporvements
    }
}