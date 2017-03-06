using Microsoft.AspNet.Identity.EntityFramework;

namespace DU.Themes.Entities
{
    public class UserRole : IdentityUserRole<long>
    {
        public long Id { get; set; }
    }
}