using DU.Themes.Entities;
using DU.Themes.Infrastructure;
using DU.Themes.Models;
using DU.Themes.Validaiton;
using DU.Themes.Validaiton.Request;
using DU.Themes.ValidaitonApiFilter;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using DU.Themes.Models.Request;

namespace DU.Themes.Api
{
    //[Authorize]
    public class RequestApiController : ApiController
    {
        private ApplicationUserManager _userManager;

        public RequestApiController()
        {

        }

        public RequestApiController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        [HttpPost]
        [Authorize(Roles = Roles.Student)]
        //[Authorize]
        //[AllowAnonymous]
        public async Task test()
        {
            var userId = Convert.ToInt64(this.User.Identity.GetUserId());

            using (var ctx = new DbContext())
            {
                using (var tran = ctx.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {

                    var requestDB = new Request
                    {
                        Student = ctx.Users.FirstOrDefault(x => x.Id == userId),
                        CreatedOn = DateTime.UtcNow,
                        Teacher = ctx.Users.FirstOrDefault(x => x.Id == 2),
                        ThemeLV = "super tema",
                        ThemeENG = "super theme",
                        Status = RequestStatus.New,
                        RespondedOn = DateTime.UtcNow,
                    };
                    requestDB.Touch();
                    // TODO: Validation!
                    requestDB.Validate(new NewRequestValidator(ctx));

                    ctx.Requests.Add(requestDB);

                    await ctx.SaveChangesAsync();
                    tran.Commit();
                }
            }
        }

        [HttpPost]
        [Authorize]
        [Route("datatables/requests", Name = RouteName.DataTablesRequests)]
        public object StudentRequests(DataTablesRequest request)
        {
            using (var ctx = new DbContext())
            {
                var requests = ctx.Requests.AsEagerRequests();

                var userQuery = this.LimitToSelfIfStudent(requests);

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

                var data = dataPage.ToList().Select(x => x.CastTo<Request, RequestModel>());

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
        [Route("datatables/teacher-requests", Name = RouteName.DataTablesTeacherRequests)]
        public object TeacherRequests(DataTablesRequest request)
        {
            using (var ctx = new DbContext())
            {
                var id = this.User.Identity.GetUserId<long>();
                var requests = ctx.Requests
                    .AsEagerRequests()
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

                var data = dataPage.ToList().Select(x => x.CastTo<Request, RequestModel>());

                return new
                {
                    data = data,
                    draw = request.Draw,
                    recordsTotal = requests.Count(),
                    recordsFiltered = filteredData.Count()
                };
            }
        }

        [HttpGet]
        [Authorize(Roles = Roles.Student)]
        [Route("student/request", Name = RouteName.GetStudentRequests)]
        public IHttpActionResult GetStudentRequest(long Id)
        {
            using (var ctx = new DbContext())
            {
                var entity = ctx.Requests.AsEagerRequests().ById(Id);

                if (entity == null)
                {
                    return this.NotFound();
                }

                if (entity.StudentId != this.User.Identity.GetUserId<long>())
                {
                    return this.BadRequest();
                }

                var model = entity.CastTo<Request, RequestModel>();

                return this.Ok(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        [Route("teacher/request", Name = RouteName.GetTeacherRequests)]
        public IHttpActionResult GetTeacherRequest(long Id)
        {
            using (var ctx = new DbContext())
            {
                var entity = ctx.Requests.AsEagerRequests().ById(Id);

                if (entity == null)
                {
                    return this.NotFound();
                }

                if (entity.TeacherId != this.User.Identity.GetUserId<long>())
                {
                    return this.BadRequest();
                }

                var model = entity.CastTo<Request, RequestModel>();

                return this.Ok(model);
            }
        }

        private IQueryable<Request> LimitToSelfIfStudent(IQueryable<Request> requests)
        {
            if (this.User.IsInRole(Roles.Student))
            {
                var id = this.User.Identity.GetUserId<long>();
                return requests.Where(x => x.Student.Id == id);
            }

            return requests;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Student)]
        public async Task Create(CreateRequestModel request)
        {
            var userId = Convert.ToInt64(this.User.Identity.GetUserId());

            using (var ctx = new DbContext())
            {
                using (var tran = ctx.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var requestDB = new Request
                    {
                        Student = ctx.Users.FirstOrDefault(x => x.Id == userId),
                        CreatedOn = DateTime.UtcNow,
                        Teacher = ctx.Users.FirstOrDefault(x => x.Id == request.Teacher.Id),
                        ThemeLV = request.ThemeLV,
                        ThemeENG = request.ThemeENG,
                        Status = RequestStatus.New,
                        RespondedOn = DateTime.UtcNow,
                    };

                    this.PreapareEntity(requestDB, request, ctx);

                    requestDB.Touch();
                    // TODO: Validation!
                    requestDB.Validate(new NewRequestValidator(ctx));

                    ctx.Requests.Add(requestDB);

                    await ctx.SaveChangesAsync();

                    tran.Commit();
                }
            }
        }

        [HttpGet]
        [Route("api/new-request-count", Name = RouteName.NewRequestCount)]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<object> NewRequestcount()
        {
            var userId = Convert.ToInt64(this.User.Identity.GetUserId());

            using (var ctx = new DbContext())
            {
                var newRequestsDb = await ctx.Requests
                    .AsEagerRequests()
                    .Where(x => x.TeacherId == userId)
                    .Where(x => x.Status == RequestStatus.New)
                    .OrderByDescending(x => x.CreatedOn)
                    .Take(5)
                    .ToListAsync();

                var newRequests = newRequestsDb
                    .Select(x => x.CastTo<Request, RequestModel>());                 
                    

                var newRequestCount = await ctx.Requests
                    .Where(x => x.TeacherId == userId)
                    .Where(x => x.Status == RequestStatus.New)
                    .CountAsync();

                return new
                {
                    items = newRequests,
                    count = newRequestCount
                };
            }
        }

        private void PreapareEntity(Request requestDB, CreateRequestModel request, DbContext ctx)
        {
            if (request.Start != null)
            {
                requestDB.Start = ctx.StudyYears.ById(request.Start.Id);
            }

            if (request.End != null)
            {
                requestDB.End = ctx.StudyYears.ById(request.End.Id);
            }
        }

        [HttpPost]
        [Route("api/student/request/update", Name = RouteName.UpdateRequestStudent)]
        [Authorize(Roles = Roles.Student)]
        public async Task<IHttpActionResult> UpdateRequestByStudent(RequestModel request)
        {
            var userId = this.User.Identity.GetUserId<long>();

            using (var ctx = new DbContext())
            {
                using (var tran = ctx.BeginTran())
                {
                    var requestDB = ctx.Requests.AsEagerRequests().ById(request.Id);

                    requestDB.Validate(new RequestPersonsNotChangedValidator(ctx, request));

                    this.MapRequest(requestDB, request, ctx);
                    requestDB.Touch();
                    requestDB.Validate(new RequestUpdateByStudentValidator(ctx));

                    await ctx.SaveChangesAsync();
                    tran.Commit();

                    return this.Ok();
                }
            }
        }

        [HttpPost]
        [Route("api/teacher/request/update", Name = RouteName.UpdateRequestTeacher)]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IHttpActionResult> UpdateRequestByTeacher(RequestModel request)
        {
            var userId = this.User.Identity.GetUserId<long>();

            using (var ctx = new DbContext())
            {
                using (var tran = ctx.BeginTran())
                {
                    var requestDB = ctx.Requests.AsEagerRequests().ById(request.Id);

                    requestDB.Validate(new RequestPersonsNotChangedValidator(ctx, request));

                    this.MapRequest(requestDB, request, ctx);
                    requestDB.Touch();
                    requestDB.Validate(new RequestUpdateByStudentValidator(ctx));

                    await ctx.SaveChangesAsync();
                    tran.Commit();

                    return this.Ok();
                }
            }
        }

        [HttpPost]
        [Route("api/teacher/themes/create", Name = RouteName.CreateTheme)]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IHttpActionResult> CreateTheme(RequestModel request)
        {
            var userId = this.User.Identity.GetUserId<long>();

            using (var ctx = new DbContext())
            {
                using (var tran = ctx.BeginTran())
                {
                    var requestDB = ctx.Requests.AsEagerRequests().ById(request.Id);

                    requestDB.Validate(new RequestBeforeCreateThemeValidator(ctx));                  

                    var theme = requestDB.CastTo<Request, Theme>(ctx);
                    theme.Active = true;
                    this.MapTheme(theme, requestDB, ctx);
                    theme.Validate(new ThemeValidatorBase(ctx));
                    ctx.Themes.Add(theme);
                    ctx.Requests.Remove(requestDB);

                    await ctx.SaveChangesAsync();
                    tran.Commit();

                    return this.Ok();
                }
            }
        }

        private void MapTheme(Theme theme, Request request, DbContext ctx)
        {
            theme.Teacher = request.Teacher.GetById<Person, Person>(ctx);
            theme.Student = request.Student.GetById<Person, Person>(ctx);
            theme.Reviewer = request.Reviewer.GetById<Person, Person>(ctx);
            theme.Start = request.Start.GetById<StudyYear, StudyYear>(ctx);
            theme.End = request.End.GetById<StudyYear, StudyYear>(ctx);
        }

        [HttpPost]
        [Route("api/teacher/request/need-improvements", Name = RouteName.RequestNeedImprovements)]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IHttpActionResult> MarkAsNeedImprovements(RequestModel request)
        {
            var userId = this.User.Identity.GetUserId<long>();

            using (var ctx = new DbContext())
            {
                using (var tran = ctx.BeginTran())
                {
                    var requestDB = ctx.Requests.AsEagerRequests().ById(request.Id);

                    requestDB.Validate(new RequestPersonsNotChangedValidator(ctx, request));
                    requestDB.Validate(new RequestNeedImprovementsValidator(ctx));

                    requestDB.Status = RequestStatus.NeedImprovements;
                    requestDB.Touch();
                    requestDB.Validate(new RequestUpdateByStudentValidator(ctx));

                    await ctx.SaveChangesAsync();
                    tran.Commit();

                    return this.Ok();
                }
            }
        }

        [HttpPost]
        [Route("api/teacher/request/reject", Name = RouteName.RejectByTeacher)]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IHttpActionResult> RejectByTeacher(RequestModel request)
        {
            var userId = this.User.Identity.GetUserId<long>();

            using (var ctx = new DbContext())
            {
                using (var tran = ctx.BeginTran())
                {
                    var requestDB = ctx.Requests.AsEagerRequests().ById(request.Id);

                    if (requestDB.TeacherId != userId)
                    {
                        return this.BadRequest();
                    }

                    requestDB.Validate(new RequestPersonsNotChangedValidator(ctx, request));
                    requestDB.Validate(new RequestNeedImprovementsValidator(ctx));

                    requestDB.Status = RequestStatus.Cancelled;
                    requestDB.Touch();
                    //requestDB.Validate(new RequestUpdateByStudentValidator(ctx));

                    await ctx.SaveChangesAsync();
                    tran.Commit();

                    return this.Ok();
                }
            }
        }

        [HttpPost]
        [Route("api/student/request/reject", Name = RouteName.RejectByStudent)]
        [Authorize(Roles = Roles.Student)]
        public async Task<IHttpActionResult> RejectByStudent(RequestModel request)
        {
            var userId = this.User.Identity.GetUserId<long>();

            using (var ctx = new DbContext())
            {
                using (var tran = ctx.BeginTran())
                {
                    var requestDB = ctx.Requests.AsEagerRequests().ById(request.Id);

                    if (requestDB.StudentId != userId)
                    {
                        return this.BadRequest();
                    }

                    requestDB.Validate(new RequestPersonsNotChangedValidator(ctx, request));
                    requestDB.Validate(new RequestNeedImprovementsValidator(ctx));

                    requestDB.Status = RequestStatus.Cancelled;
                    requestDB.Touch();
                  //  requestDB.Validate(new RequestUpdateByStudentValidator(ctx));

                    await ctx.SaveChangesAsync();
                    tran.Commit();

                    return this.Ok();
                }
            }
        }

        private void MapRequest(Request requestDB, RequestModel request, DbContext ctx)
        {
            requestDB.Teacher = request.Teacher.GetUserByModelId<Person, PersonModel>(ctx);
            requestDB.Student = request.Student.GetUserByModelId<Person, PersonModel>(ctx);
            requestDB.Reviewer = request.Reviewer.GetUserByModelId<Person, PersonModel>(ctx);
            requestDB.Start = request.Start.GetByModelId<StudyYear, StudyYearModel>(ctx);
            requestDB.End = request.End.GetByModelId<StudyYear, StudyYearModel>(ctx);
            requestDB.ThemeENG = request.ThemeENG;
            requestDB.ThemeLV = request.ThemeLV;
        }

        [HttpPost]
        [Authorize]
        public IEnumerable<RequestModel> Requests()
        {
            IEnumerable<RequestModel> result = null;
            using (var ctx = new DbContext())
            {
                result = ctx.Requests
                     .OrderBy(x => x.Id)
                     .Take(10)
                     .ToList()
                     .Select(x => x.CastTo<Request, RequestModel>());
            }

            return result;
        }

        [Authorize]
        [HttpGet]
        public RequestModel Empty()
        {
            var currentUser = new Person();
            using (var ctx = new DbContext())
            {
                if (this.User.IsInRole(Roles.Student))
                {
                    currentUser = ctx.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                }
            }

            return new RequestModel
            {
                Student = currentUser.CastTo<Person, PersonModel>(),
                Start = new StudyYearModel(),
                End = new StudyYearModel()
            };
        }
    }
}
