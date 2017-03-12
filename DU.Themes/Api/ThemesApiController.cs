using DU.Themes.Entities;
using DU.Themes.Infrastructure;
using DU.Themes.Models;
using DU.Themes.Validaiton;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DU.Themes.Api
{
    public class ThemesApiController : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("datatables/student-themes", Name = RouteName.DataTablesStudentThemes)]
        public object StudentRequests(DataTablesRequest request)
        {
            using (var ctx = new DbContext())
            {
                var themes = ctx.Themes.AsEagerThemes();

                var userQuery = this.LimitToSelfIfStudent(themes);

                var sortBy = request.OrderBy.Contains("FullName") ? request.OrderBy.Replace("FullName", "FirstName") : request.OrderBy;

                var filteredData = userQuery.Sort(sortBy, request.OrderAscending)
                    .Search(request.Search.Value, x =>
                       x.Student.FirstName.StartsWith(request.Search.Value)
                       || x.Student.LastName.StartsWith(request.Search.Value)
                       || x.Teacher.FirstName.StartsWith(request.Search.Value)
                       || x.Teacher.LastName.StartsWith(request.Search.Value)
                       || x.End.Name.StartsWith(request.Search.Value)
                       || x.Start.Name.StartsWith(request.Search.Value)
                       );

                var dataPage = filteredData.Skip(request.Start).Take(request.Length);

                var data = dataPage.ToList().Select(x => x.CastTo<Theme, ThemeModel>());

                return new
                {
                    data = data,
                    draw = request.Draw,
                    recordsTotal = userQuery.Count(),
                    recordsFiltered = filteredData.Count()
                };
            }
        }

        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [Route("datatables/teacher-themes", Name = RouteName.DataTablesTeachersThemes)]
        public object TeacherRequests(DataTablesRequest request)
        {
            using (var ctx = new DbContext())
            {
                var id = this.User.Identity.GetUserId<long>();
                var requests = ctx.Themes
                    .AsEagerThemes()
                    .Where(x => x.TeacherId == id);

                var sortBy = request.OrderBy.Contains("FullName") ? request.OrderBy.Replace("FullName", "FirstName") : request.OrderBy;

                var filteredData = requests.Sort(sortBy, request.OrderAscending)
                    .Search(request.Search.Value, x =>
                       x.Student.FirstName.StartsWith(request.Search.Value)
                       || x.Student.LastName.StartsWith(request.Search.Value)
                       || x.Teacher.FirstName.StartsWith(request.Search.Value)
                       || x.Teacher.LastName.StartsWith(request.Search.Value)
                       || x.End.Name.StartsWith(request.Search.Value)
                       || x.Start.Name.StartsWith(request.Search.Value)
                       );

                var dataPage = filteredData.Skip(request.Start).Take(request.Length);

                var data = dataPage.ToList().Select(x => x.CastTo<Theme, ThemeModel>());

                return new
                {
                    data = data,
                    draw = request.Draw,
                    recordsTotal = requests.Count(),
                    recordsFiltered = filteredData.Count()
                };
            }
        }

        [HttpPost]
        [Route("api/teacher/themes/update", Name = RouteName.UpdateThemeByTeacher)]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IHttpActionResult> UpdateByTeacher(ThemeModel request)
        {
            var userId = this.User.Identity.GetUserId<long>();

            using (var ctx = new DbContext())
            {
                using (var tran = ctx.BeginTran())
                {
                    var theme = ctx.Themes.AsEagerThemes().ById(request.Id);

                    if(theme.TeacherId != userId)
                    {
                        return this.BadRequest();
                    }

                    theme.Validate(new SamePersonValidator(ctx, request));
                    this.MapRequest(theme, request, ctx);
                    theme.Touch();
                    theme.Validate(new ThemeValidatorBase(ctx));

                    await ctx.SaveChangesAsync();
                    tran.Commit();

                    return this.Ok();
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        [Route("teacher/theme", Name = RouteName.GetTeacherTheme)]
        public IHttpActionResult GetTeacherTheme(long Id)
        {
            using (var ctx = new DbContext())
            {
                var entity = ctx.Themes.AsEagerThemes().ById(Id);

                if (entity == null)
                {
                    return this.NotFound();
                }

                if (entity.TeacherId != this.User.Identity.GetUserId<long>())
                {
                    return this.BadRequest();
                }

                var model = entity.CastTo<Theme, ThemeModel>();

                return this.Ok(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = Roles.Student)]
        [Route("student/theme", Name = RouteName.GetStudentTheme)]
        public IHttpActionResult GetStudentTheme(long Id)
        {
            using (var ctx = new DbContext())
            {
                var entity = ctx.Themes.AsEagerThemes().ById(Id);

                if (entity == null)
                {
                    return this.NotFound();
                }

                if (entity.StudentId != this.User.Identity.GetUserId<long>())
                {
                    return this.BadRequest();
                }

                var model = entity.CastTo<Theme, ThemeModel>();

                return this.Ok(model);
            }
        }

        private void MapRequest(Theme requestDB, ThemeModel request, DbContext ctx)
        {
            requestDB.Teacher = request.Teacher.GetUserByModelId<Person, PersonModel>(ctx);
            requestDB.Student = request.Student.GetUserByModelId<Person, PersonModel>(ctx);
            requestDB.Reviewer = request.Reviewer.GetUserByModelId<Person, PersonModel>(ctx);
            requestDB.Start = request.Start.GetByModelId<StudyYear, StudyYearModel>(ctx);
            requestDB.End = request.End.GetByModelId<StudyYear, StudyYearModel>(ctx);
            requestDB.ThemeENG = request.ThemeENG;
            requestDB.ThemeLV = request.ThemeLV;
        }

        private IQueryable<Theme> LimitToSelfIfStudent(IQueryable<Theme> theme)
        {
            if (this.User.IsInRole(Roles.Student))
            {
                var id = this.User.Identity.GetUserId<long>();
                return theme.Where(x => x.Student.Id == id);
            }

            return theme;
        }
    }
}