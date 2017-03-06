using DU.Themes.Entities;
using System.Data.Entity.ModelConfiguration;

namespace DU.Themes.Data.TypeConfigurations
{
    public class UserRoleConfiguration : EntityTypeConfiguration<UserRole>
    {
        public UserRoleConfiguration()
        {
            this.ToTable("UserRoles");
            this.HasKey(x => x.Id);
        }
    }
}