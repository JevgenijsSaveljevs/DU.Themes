using DU.Themes.Entities;
using System.Data.Entity;

namespace DU.Themes
{
    public partial class DbContext
    {
        public DbSet<Request> Requests { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<StudyYear> StudyYears { get; set; }
    }
}