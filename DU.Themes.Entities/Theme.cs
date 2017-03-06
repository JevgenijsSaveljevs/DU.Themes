using System;

namespace DU.Themes.Entities
{
    public class Theme : EntityBase
    {
        public Person Student { get; set; }
        public Person Teacher { get; set; }
        public Person Reviewer { get; set; }
        public long StudentId { get; set; }
        public long TeacherId { get; set; }
        public long ReviewerId { get; set; }
        public string ThemeLV { get; set; }
        public string ThemeRU { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime RespondedOn { get; set; }
        public bool SeenByTeacher { get; set; }
        public bool SeenByReviewer { get; set; }
        public bool SeenByStudent { get; set; }
        public StudyYear WorkStart { get; set; }
        public long WorkStartId { get; set; }
        public StudyYear WorkEnd { get; set; }
        public long WorkStartEndId { get; set; }
    }
}
