using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DU.Themes.Entities
{
    public class Person : IdentityUser<long, UserLogin, UserRole, UserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Person, long> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string Year { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentIdentifier { get; set; }
        public string ProgramName { get; set; }
        public string ProgramLevel { get; set; }
        public string StudyForm { get; set; }
    }
}
