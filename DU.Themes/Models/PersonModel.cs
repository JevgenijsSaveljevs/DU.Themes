using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DU.Themes.Models
{
    public class PersonModel : ModelBase
    {
        public string Year { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentIdentifier { get; set; }
        public string ProgramName { get; set; }
        public string ProgramLevel { get; set; }
        public string StudyForm { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsStudent { get; set; }
        public bool IsTeacher { get; set; }
        public string FullName
        {
            get
            {
                return string.Join(" ", this.LastName, this.FirstName);
            }
        }


        public bool SuccessfulyImported { get; set; }
    }
}