using DU.Themes.Entities;
using System.Data.Entity.ModelConfiguration;

namespace DU.Themes.Data.TypeConfigurations
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            this.ToTable("Roles");
        }
    }
}