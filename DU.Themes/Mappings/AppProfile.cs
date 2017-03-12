using AutoMapper;
using DU.Themes.Entities;
using DU.Themes.Mappings.Resolvers;
using DU.Themes.Models;


namespace DU.Themes.Mappings
{
    /// <summary>
    /// AutoMapper Profile containing all neccessary mappings
    /// </summary>
    public class AppProfile : Profile
    {
        /// <summary>
        /// Constructor where happens mappings defintion
        /// </summary>
        public AppProfile()
        {
            this.CreateMap<Person, PersonModel>()
                      .IgnoreAllNonExisting();


            this.CreateMap<Request, RequestModel>();
            this.CreateMap<PersonModel, Person>()
                .ForMember(x => x.UserName, o => o.MapFrom(x => x.StudentIdentifier))
                .IgnoreAllNonExisting();

            this.CreateMap<StudyYear, StudyYearModel>();
            this.CreateMap<StudyYearModel, StudyYear>()
                .ForMember(x => x.TouchTime, o => o.Ignore());

            this.CreateMap<Theme, ThemeModel>()
                .ForMember(x => x.ThemeAccepted, o => o.Ignore())
                .ForMember(x => x.SupervisorAccpeted, o => o.Ignore());

            this.CreateMap<Request, Theme>()
                .ForMember(x => x.Active, o => o.Ignore())
                .ForMember(x => x.Start, o => o.Ignore())
                .ForMember(x => x.End, o => o.Ignore())
                .ForMember(x => x.Teacher, o => o.Ignore())
                .ForMember(x => x.Student, o => o.Ignore())
                .ForMember(x => x.Reviewer, o => o.Ignore());
                //.ForMember(x => x.Teacher, o => o.ResolveUsing(req => new PersonResolver(x => x.Id == req.TeacherId)))
                //.ForMember(x => x.Student, o => o.ResolveUsing(req => new PersonResolver(x => x.Id == req.StudentId)))
                //.ForMember(x => x.Reviewer, o => o.ResolveUsing(req => new PersonResolver(x => x.Id == req.ReviewerId)))
                //.ForMember(x => x.Start, o => o.ResolveUsing(req => new StudyYearResolver(x => x.Id == req.StartId)))
                //.ForMember(x => x.Start, o => o.ResolveUsing(req => new StudyYearResolver(x => x.Id == req.StartId)));



        }
    }
}