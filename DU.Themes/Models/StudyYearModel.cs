using System;

namespace DU.Themes.Models
{
    public class StudyYearModel : ModelBase
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}