using DU.Themes.Entities;
using DU.Themes.Models;
using System.Data.Entity.ModelConfiguration;

namespace DU.Themes.Data.TypeConfigurations
{
    public class RequestConfiguration : EntityTypeConfiguration<Request>
    {
        public RequestConfiguration()
        {
            this.ToTable("Requests");
            this.HasRequired(x => x.Student);
            this.HasRequired(x => x.Teacher);
            this.HasOptional(x => x.Reviewer);
        }
    }
}