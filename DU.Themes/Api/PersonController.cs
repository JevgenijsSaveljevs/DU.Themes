using DU.Themes.Entities;
using DU.Themes.Infrastructure;
using DU.Themes.Infrastructure.Comparers;
using DU.Themes.Models;
using DU.Themes.Models.Filter;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace DU.Themes.Api
{
    [Authorize]
    public class PersonController : ApiController
    {
        private ApplicationUserManager _userManager;

        public PersonController()
        {

        }

        public PersonController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? this.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Gets Supervisor list
        /// </summary>
        /// <param name="search">First Name or Last Name</param>
        /// <returns>first 10 of list ordered by last Name</returns>
        [HttpGet]
        public IEnumerable<PersonModel> Supervisors(string search = null)
        {
            if (search != null)
            {
                search = Encoding.UTF8
                    .GetString(Encoding.GetEncoding("ISO-8859-8").GetBytes(search))
                    .TrimEnd()
                    .TrimStart();
            }

            //Thread.Sleep(TimeSpan.FromSeconds(3));
            var result = new List<PersonModel>();

            using (var ctx = new DbContext())
            {

                var role = ctx.Roles.First(x => x.Name == Roles.Teacher);
                //var role = ctx.CustomUserRole.First(x => x.RoleId == supervisorRole.Id);

                if (string.IsNullOrEmpty(search))
                {
                    var founded = ctx.Set<Person>().Include("Roles").Where(x => x.Roles.Where(r => r.RoleId == role.Id).Any())
                       .OrderBy(x => x.LastName)
                       .Take(10) // TODO -> 10 from cfg
                       .ToList()
                       .Select(x => x.CastTo<Person, PersonModel>());

                    result.AddRange(founded);
                }
                else
                {
                    var enterances = search.Split(' ');

                    // one query would be too complex and execution time will be enourmously big
                    foreach (var enterance in enterances)
                    {
                        var byFName1q = ctx.Set<Person>().Include("Roles").ToList();
                        var byFName11 = ctx.Set<Person>().Include("Roles")
                            .Where(x => x.Roles.Any(r => r.RoleId == role.Id));

                        // by FirstName
                        var byFName = ctx.Set<Person>().Include("Roles")
                            .Where(x => x.Roles.Where(r => r.RoleId == role.Id).Any())
                            .Where(x => x.FirstName.StartsWith(enterance))
                            .Take(10)
                            .ToList()
                            .Select(x => x.CastTo<Person, PersonModel>());

                        result.AddRange(byFName);

                        // By LastName
                        var byLName = ctx.Set<Person>().Include("Roles")
                            .Where(x => x.Roles.Where(r => r.RoleId == role.Id).Any())
                            .Where(x => x.LastName.StartsWith(enterance))
                            .Take(10)
                            .ToList()
                            .Select(x => x.CastTo<Person, PersonModel>());

                        result.AddRange(byLName);
                    }
                }

                return result
                    .Distinct(Comparers<PersonModel>.ModelIdComparer)
                    .OrderBy(x => x.LastName)
                    .Take(10);
            }
        }

        /// <summary>
        /// Gets Supervisor list
        /// </summary>
        /// <param name="search">First Name or Last Name</param>
        /// <returns>first 10 of list ordered by last Name</returns>
        [HttpGet]
        public IEnumerable<PersonModel> Students(string search = null)
        {

            if (search != null)
            {
                search = Encoding.UTF8
                    .GetString(Encoding.GetEncoding("ISO-8859-8").GetBytes(search))
                    .TrimEnd()
                    .TrimStart();
            }

            //Thread.Sleep(TimeSpan.FromSeconds(3));
            var result = new List<PersonModel>();

            using (var ctx = new DbContext())
            {

                var role = ctx.Roles.First(x => x.Name == Roles.Student);

                if (string.IsNullOrEmpty(search))
                {
                    var founded = ctx.Users.Where(x => x.Roles.Where(r => r.RoleId == role.Id).Any())
                       .OrderBy(x => x.LastName)
                       .Take(10) // TODO -> 10 from cfg
                       .ToList()
                       .Select(x => x.CastTo<Person, PersonModel>());

                    result.AddRange(founded);
                }
                else
                {
                    var enterances = search.Split(' ');

                    // one query would be too complex and execution time will be enourmously big
                    foreach (var enterance in enterances)
                    {
                        var name = enterance;

                        //var s = new string(name., 0, name.Length, System.Text.Encoding.Unicode);

                        // by FirstName
                        var byFName = ctx.Users
                            .Where(x => x.Roles.Where(r => r.RoleId == role.Id).Any())
                            .Where(x => x.FirstName.StartsWith(name))
                            .Take(10)
                            .ToList()
                            .Select(x => x.CastTo<Person, PersonModel>());

                        result.AddRange(byFName);

                        // By LastName
                        var byLName = ctx.Users
                            .Where(x => x.Roles.Where(r => r.RoleId == role.Id).Any())
                            .Where(x => x.LastName.StartsWith(enterance))
                            .Take(10)
                            .ToList()
                            .Select(x => x.CastTo<Person, PersonModel>());

                        result.AddRange(byLName);
                    }
                }

                return result
                    .Distinct(Comparers<PersonModel>.ModelIdComparer)
                    .OrderBy(x => x.LastName)
                    .Take(10);
            }
        }

        [HttpGet]
        [Route("api/teachers-lookup", Name = RouteName.TeachersLookup)]
        [Authorize]
        public PageableResult<PersonModel> Teachers(string q, int page = 1, int size = 15)
        {
            using (var ctx = new DbContext())
            {
                var teacherRole = ctx.Roles.FirstOrDefault(x => x.Name == Roles.Teacher);

                var query = ctx.Users
                    .Where(x => x.Roles.Any(r => r.RoleId == teacherRole.Id))
                    .Search(q, x => x.FirstName.StartsWith(q) || x.LastName.StartsWith(q))
                    .OrderBy(x => x.Id)
                    .Skip((page - 1) * size)
                    .Take(size);

                var data = query
                    .ToList()
                    .Select(x => x.CastTo<Person, PersonModel>());

                var count = query.Count();

                return new PageableResult<PersonModel>(count, data, page);
            }
        }

        /// <summary>
        /// Gets Person list
        /// </summary>
        /// <param name="search">First Name or Last Name</param>
        /// <returns>first 10 of list ordered by last Name</returns>
        [HttpPost]
        public DataResponse<PersonModel> All(FilterBase filter)
        {

            if (filter?.Search != null)
            {
                filter.Search = Encoding.UTF8
                    .GetString(Encoding.GetEncoding("ISO-8859-8").GetBytes(filter.Search))
                    .TrimEnd()
                    .TrimStart();
            }

            IQueryable<Person> query = null;

            using (var ctx = new DbContext())
            {
                if (string.IsNullOrEmpty(filter?.Search))
                {
                    query = ctx.Users
                       .Sort(filter);
                }
                else
                {
                    var enterances = filter?.Search?.Split(' ');

                    query = ctx.Users
                        .Sort(filter)
                        .Where(x =>
                            enterances.Any(e => x.FirstName.StartsWith(e))
                            || enterances.Any(e => x.LastName.StartsWith(e))
                            || enterances.Any(e => x.FirstName.StartsWith(e))
                            || enterances.Any(e => x.Email.StartsWith(e))
                            || enterances.Any(e => x.Year.StartsWith(e))
                            || enterances.Any(e => x.ProgramName.StartsWith(e))
                        );
                }

                var total = query.Count();
                var data = query
                    .Skip(filter.Skip)
                    .Take(filter.Take)
                    .ToList()
                    .Select(x => x.CastTo<Person, PersonModel>());

                return new DataResponse<PersonModel>(data, filter.Take, filter.Skip, total);
            }
        }

        [HttpPost]
        [Route("datatables/persons", Name = "GetPersons")]
        public object Persons(DataTablesRequest request)
        {
            using (var ctx = new DbContext())
            {
                var persons = ctx.Set<Person>();

                var filteredData = persons.Sort(request.OrderBy, request.OrderAscending)
                    .Search(request.Search.Value, x =>
                       x.FirstName.StartsWith(request.Search.Value)
                       || x.LastName.StartsWith(request.Search.Value)
                       || x.ProgramLevel.Contains(request.Search.Value)
                       || x.ProgramName.Contains(request.Search.Value)
                       );

                var dataPage = filteredData.Skip(request.Start).Take(request.Length);

                var data = dataPage.ToList().Select(x => x.CastTo<Person, PersonModel>());

                return new
                {
                    data = data,
                    draw = request.Draw,
                    recordsTotal = persons.Count(),
                    recordsFiltered = filteredData.Count()
                };
            }
        }

        private object GetPropertyExpression(DataTablesRequest request)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet]
        public PersonModel Person(string userId)
        {
            using (var ctx = new DbContext())
            {
                var userDb = ctx.Users
                    .FirstOrDefault(x => x.UserName == userId);

                var user = userDb?.CastTo<Person, PersonModel>();

                if (userDb != null)
                {
                    user.IsAdmin = ctx.Roles
                        .Any(x => x.Users.Any(r => r.UserId == userDb.Id) && x.Name == Roles.SystemAdministrator);

                    user.IsStudent = ctx.Roles
                        .Any(x => x.Users.Any(r => r.UserId == userDb.Id) && x.Name == Roles.Student);

                    user.IsTeacher = ctx.Roles
                        .Any(x => x.Users.Any(r => r.UserId == userDb.Id) && x.Name == Roles.Teacher);
                }

                return user;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task Update(PersonModel person)
        {
            using (var ctx = new DbContext())
            {
                using (var tran = ctx.BeginTran())
                {
                    var personDb = ctx.Users.FirstOrDefault(x => x.Id == person.Id);

                    personDb.UpdateFrom(person);


                    if (User.IsInRole(Roles.SystemAdministrator))
                    {
                        var roles = await UserManager.GetRolesAsync(personDb.Id);

                        if (person.IsAdmin == false && roles.Contains(Roles.SystemAdministrator))
                        {
                            await UserManager.RemoveFromRoleAsync(personDb.Id, Roles.SystemAdministrator);
                        }
                        else if (person.IsAdmin == true && !roles.Contains(Roles.SystemAdministrator))
                        {
                            await UserManager.AddToRoleAsync(personDb.Id, Roles.SystemAdministrator);
                        }


                        if (person.IsStudent == false && roles.Contains(Roles.Student))
                        {
                            await UserManager.RemoveFromRoleAsync(personDb.Id, Roles.Student);
                        }
                        else if (person.IsStudent == true && !roles.Contains(Roles.Student))
                        {
                            await UserManager.AddToRoleAsync(personDb.Id, Roles.Teacher);
                        }


                        if (person.IsTeacher == false && roles.Contains(Roles.Teacher))
                        {
                            await UserManager.RemoveFromRoleAsync(personDb.Id, Roles.Teacher);
                        }
                        else if (person.IsTeacher == true && !roles.Contains(Roles.Teacher))
                        {
                            await UserManager.AddToRoleAsync(personDb.Id, Roles.Teacher);
                        }
                    }

                    // TODO: Validation
                    //  personDb
                    ctx.SaveChanges();
                    tran.Commit();
                }
            }
        }
    }
}
