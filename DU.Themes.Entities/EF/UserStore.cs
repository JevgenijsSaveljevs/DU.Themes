using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DU.Themes.Entities.EF
{
    public class UserStore : UserStore<Person, Role, long,
    UserLogin, UserRole, UserClaim>
    {
        public UserStore(DbContext context) : base(context)
        {
        }
    }
}
