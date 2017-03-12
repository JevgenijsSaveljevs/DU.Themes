using AutoMapper;
using DU.Themes.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DU.Themes.Mappings.Resolvers
{
    public class PersonResolver : IValueResolver<Request, Theme, Person>
    {
        public PersonResolver(Expression<Func<Person, bool>> path)
        {
            this.Path = path;
        }

        public Expression<Func<Person, bool>> Path { get; private set; }

        public Person Resolve(Request source, Theme destination, Person destMember, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            object dbContextWrapped;

            if (context.Items.TryGetValue("Context", out dbContextWrapped))
            {
                var dbContext= dbContextWrapped as DbContext;
                var user = dbContext.Users.FirstOrDefault(Path);

                return user;
            }

            return null;
        }
    }
}