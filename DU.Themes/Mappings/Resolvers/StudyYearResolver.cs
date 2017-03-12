using AutoMapper;
using DU.Themes.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DU.Themes.Mappings.Resolvers
{
    public class StudyYearResolver : IValueResolver<Request, Theme, StudyYear>
    {
        public StudyYearResolver(Expression<Func<StudyYear, bool>> path)
        {
            this.Path = path;
        }

        public Expression<Func<StudyYear, bool>> Path { get; private set; }

        public StudyYear Resolve(Request source, Theme destination, StudyYear destMember, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            object dbContextWrapped;

            if (context.Items.TryGetValue("Context", out dbContextWrapped))
            {
                var dbContext = dbContextWrapped as DbContext;
                var year = dbContext.StudyYears.FirstOrDefault(this.Path);

                return year;
            }

            return null;
        }
    }
}