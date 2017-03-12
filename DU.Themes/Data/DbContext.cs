using DU.Themes.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DU.Themes
{
    public partial class DbContext : IdentityDbContext<Person, Role, long, UserLogin, UserRole, UserClaim>
    {
        public DbContext()
            : base("TemasDB")
        {
            base.Configuration.LazyLoadingEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            
        }

        public static DbContext Create()
        {
            return new DbContext();
        }
    }
}