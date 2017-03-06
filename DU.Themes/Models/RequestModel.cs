using System;
using DU.Themes.Infrastructure;
using DU.Themes.Entities;

namespace DU.Themes.Models
{
    public class RequestModel : ModelBase
    {
        public RequestModel()
        {
            this.Student = new PersonModel();
            this.Teacher = new PersonModel();
            this.Reviewer = new PersonModel();
            this.Status = RequestStatus.New;
        }

        public PersonModel Student { get; set; }
        public PersonModel Teacher { get; set; }
        public PersonModel Reviewer { get; set; }
        public string ThemeLV { get; set; }
        public string ThemeENG { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime RespondedOn { get; set; }
        public bool SeenByTeacher { get; set; }
        //public bool SeenByReviewer { get; set; }
        public bool SeenByStudent { get; set; }
        public bool ThemeAccepted { get; set; }
        public bool SupervisorAccpeted { get; set; }
        public string StatusRepresentation
        {
            get
            {
                return this.Status.FromResource();
            }
        }

        public StudyYearModel Start { get; set; }
        public StudyYearModel End { get; set; }
    }
}