using System;

namespace DU.Themes.Entities
{
    public class StudyYear : EntityBase
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public string GetCode()
        {
            return string.Join("/", this.Start.Year, this.End.Year);
        }

        public bool IsCurrent { get; set; }
    }
}
