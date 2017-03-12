using System;

namespace DU.Themes.Models
{
    public class ThemeModel : ModelBase
    {
        public ThemeModel()
        {
            this.Student = new PersonModel();
            this.Teacher = new PersonModel();
            this.Reviewer = new PersonModel();
        }

        public PersonModel Student { get; set; }
        public PersonModel Teacher { get; set; }
        public PersonModel Reviewer { get; set; }
        public string ThemeLV { get; set; }
        public string ThemeENG { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime RespondedOn { get; set; }
        public bool SeenByTeacher { get; set; }
        public bool SeenByStudent { get; set; }
        public bool ThemeAccepted { get; set; }
        public bool SupervisorAccpeted { get; set; }
        public StudyYearModel Start { get; set; }
        public StudyYearModel End { get; set; }
        public bool Active { get; set; }
    }
}