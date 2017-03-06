using System;

namespace DU.Themes.Entities
{
    public class Request : EntityBase
    {
        public virtual Person Student { get; set; }
        public virtual Person Teacher { get; set; }
        public virtual Person Reviewer { get; set; }
        public long StudentId { get; set; }
        public long TeacherId { get; set; }
        public long? ReviewerId { get; set; }
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
        public virtual StudyYear Start { get; set; }
        public virtual StudyYear End { get; set; }
        public long StartId { get; set; }
        public long EndId { get; set; }
    }
}
