using DU.Themes.Entities;
using System.Data.Entity.ModelConfiguration;

namespace DU.Themes.Data.TypeConfigurations
{
    public class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration()
        {
            this.ToTable("Users");
            this.HasMany(x => x.Roles).WithRequired().HasForeignKey(p => p.UserId);
        }
    }
}