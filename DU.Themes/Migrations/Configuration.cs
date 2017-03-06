namespace DU.Themes.Migrations
{
    using Entities;
    using Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Configuration;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );    <add key="SysAdmLogin" value="Boss" />


            this.CreateRoles(context);
            var manager = new ApplicationUserManager(new UserStore<Person, Role, long, UserLogin, UserRole, UserClaim>(context));

            if (manager.FindByEmail(ConfigurationManager.AppSettings.Get("SysAdmEmail")) == null)
            {
                var result = manager.Create(new Person
                {
                    Email = ConfigurationManager.AppSettings.Get("SysAdmEmail"),
                    UserName = ConfigurationManager.AppSettings.Get("SysAdmLogin"),
                    FirstName = "Admin",
                    LastName = "Admin"
                }, ConfigurationManager.AppSettings.Get("SysAdmPW"));

                if (!result.Succeeded)
                {
                    throw new System.Exception(string.Join(Environment.NewLine, result.Errors));
                }

                context.SaveChanges();

                var adm = manager.FindByEmail(ConfigurationManager.AppSettings.Get("SysAdmEmail"));

                manager.AddToRole(adm.Id, Roles.SystemAdministrator);

                context.SaveChanges();
            };
        }

        private void CreateRoles(DbContext context)
        {
            if (!context.Roles.Any(x => x.Name.Equals(Roles.SystemAdministrator)))
            {
                context.Roles.Add(new Role { Name = Roles.SystemAdministrator });
            }

            if (!context.Roles.Any(x => x.Name.Equals(Roles.Student)))
            {
                context.Roles.Add(new Role { Name = Roles.Student });
            }

            if (!context.Roles.Any(x => x.Name.Equals(Roles.Teacher)))
            {
                context.Roles.Add(new Role { Name = Roles.Teacher });
            }


            context.SaveChanges();
        }
    }
}
