﻿using DU.Themes.Entities;
using DU.Themes.Infrastructure;
using DU.Themes.Models;
using DU.Themes.Validaiton.Request;
using DU.Themes.ValidaitonApiFilter;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

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
                var requests = ctx.Requests
                    .Include("End")
                    .Include("Start")
                    .Include("Teacher")
                    .Include("Student");

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
                    .Include("End")
                    .Include("Start")
                    .Include("Teacher")
                    .Include("Student")
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
                var entity = ctx.Requests
                    .Include("Student")
                    .Include("Teacher")
                    .Include("Start")
                    .Include("End")
                    .Include("Reviewer")
                    .ById(Id);

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

        private IQueryable<Request> LimitToSelfIfStudent(DbQuery<Request> requests)
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
        public async Task Create(RequestModel request)
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

            //var user = this.UserManager.FindById(Convert.ToInt64(this.User.Identity.GetUserId()));

            //var requestDB = new Request
            //{
            //    Student = user,
            //    CreatedOn = DateTime.UtcNow,
            //    Teacher = this._userManager.FindById(Convert.ToInt64(request?.Teacher?.Id)),
            //    ThemeLV = request.ThemeLV,
            //    ThemeENG = request.ThemeENG,
            //    Status = RequestStatus.New
            //};

            //var ctx = new ApplicationDbContext();
            //using (var tran = ctx.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            //{
            //    // TODO: Validation!
            //    request.Validate(new NewRequestValidator(ctx));
            //    ctx.Requests.Add(requestDB);

            //    await ctx.SaveChangesAsync().ContinueWith((Task<int> result) =>
            //    {
            //        tran.Commit();
            //    });

            //}
        }

        private void PreapareEntity(Request requestDB, RequestModel request, DbContext ctx)
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
                    var requestDB = ctx
                        .Requests
                        .Include("Start")
                        .Include("End")
                        .Include("Teacher")
                        .Include("Student")
                        .Include("Reviewer")
                        .ById(request.Id);

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

        private void MapRequest(Request requestDB, RequestModel request, DbContext ctx)
        {
            requestDB.Teacher = request.Teacher.GetUserByModelId<Person, PersonModel>(ctx);
            requestDB.Student = request.Student.GetUserByModelId<Person, PersonModel>(ctx);
            requestDB.Reviewer = request.Reviewer.GetUserByModelId<Person, PersonModel>(ctx);
            requestDB.Start = request.Start.GetByModelId<StudyYear, StudyYearModel>(ctx);
            requestDB.End = request.End.GetByModelId<StudyYear, StudyYearModel>(ctx);
        }

        [HttpPost]
        [Authorize]
        public IEnumerable<RequestModel> Requests()
        {
            IEnumerable<RequestModel> result = null;
            using (var ctx = new DbContext())
            {
                result = ctx.Requests
                     .Include(nameof(Entities.Request.Student))
                     .Include(nameof(Entities.Request.Teacher))
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
