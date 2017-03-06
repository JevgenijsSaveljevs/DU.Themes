using System;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DU.Themes.Data.Conventions
{
    public class DateTimeConvetion : Convention
    {
        public DateTimeConvetion()
        {
            this.Properties<DateTime>().Configure(p => p.HasColumnType("datetime2"));
        }
    }
}