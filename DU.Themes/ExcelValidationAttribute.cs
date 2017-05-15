using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DU.Themes
{
    public class ExcelValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            
            return base.IsValid(value);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }

        public override bool Match(object obj)
        {
            return base.Match(obj);
        }
    }
}