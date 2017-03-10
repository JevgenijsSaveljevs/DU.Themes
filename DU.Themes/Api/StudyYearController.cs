using DU.Themes.Entities;
using DU.Themes.Infrastructure;
using DU.Themes.Models;
using DU.Themes.Validaiton;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DU.Themes.Api
{
    public class StudyYearController : ApiController
    {
        [Authorize(Roles = Roles.SystemAdministrator)]
        [Route("year/model", Name = "YearModel")]
        [HttpGet]
        public StudyYearModel GetModel()
        {
            // TODO: Get Last End Date From Date 
            var now = DateTime.UtcNow;
            return new StudyYearModel()
            {
                Start = now,
                End = now.AddYears(1)
            };
        }

        [Authorize(Roles = Roles.SystemAdministrator)]
        [Route("year/model/{id}", Name = "GetYearById")]
        [HttpGet]
        public IHttpActionResult GetModel(long id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            using (var ctx = new DbContext())
            {
                var entity = ctx.StudyYears
                    .FirstOrDefault(x => x.Id == id).CastTo<StudyYear, StudyYearModel>();

                return Ok(entity);
            }

        }

        [Authorize(Roles = Roles.SystemAdministrator)]
        [Route("year/model", Name = "CreateYearModel")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(StudyYearModel yearModel)
        {
            if (yearModel == null)
            {
                return this.BadRequest();
            }

            using (var ctx = new DbContext())
            {
                using (var tran = ctx.BeginTran())
                {
                    var year = yearModel.CastTo<StudyYearModel, StudyYear>();
                    this.CalculateNames(year);
                    year.Validate(new CreateStudyYearValidator(ctx));

                    ctx.StudyYears.Add(year);
                    await ctx.SaveChangesAsync();

                    tran.Commit();

                    return this.Ok(year);
                }
            }
        }

        [Authorize(Roles = Roles.SystemAdministrator)]
        [Route("year/delete/{id}", Name = "DeleteStudyYear")]
        [HttpPost]
        public async Task<IHttpActionResult> DeleteYear(long id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            using (var ctx = new DbContext())
            {
                var year = ctx.StudyYears.ById(id);

                if (year == null)
                {
                    return this.BadRequest();
                }

                using (var tran = ctx.BeginTran())
                {
                    year.Validate(new DeleteStudyYearValidator(ctx));

                    ctx.StudyYears.Remove(year);

                    ctx.SaveChanges();

                    tran.Commit();

                    return this.Ok(year);
                }
            }
        }

        [HttpGet]
        [Route("api/years", Name = RouteName.StudyYears)]
        [Authorize]
        public PageableResult<StudyYearModel> StudyYears(string q, int page = 1, int size = 15)
        {
            using (var ctx = new DbContext())
            {
                q = q.SafeTrim();

                var query = ctx.StudyYears
                    .Search(q, x => x.Name.StartsWith(q))               
                    .OrderBy(x => x.Id)
                    .Skip((page - 1) * size)
                    .Take(size);

                var data = query
                    .ToList()
                    .Select(x => x.CastTo<StudyYear, StudyYearModel>());

                var count = query.Count();

                return new PageableResult<StudyYearModel>(count, data, page);
            }
        }

        private void CalculateNames(StudyYear year)
        {
            var code = year.GetCode();
            year.Name = code;
            year.Code = code;
        }
    }
}
