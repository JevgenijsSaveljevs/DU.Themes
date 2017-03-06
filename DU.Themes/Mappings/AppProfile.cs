using AutoMapper;
using DU.Themes.Entities;
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
        }
    }
}