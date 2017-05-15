using Du.Themes.Excel;
using DU.Themes.Entities;
using DU.Themes.Infrastructure;
using DU.Themes.Infrastructure.Excel;
using DU.Themes.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DU.Themes.Controllers
{
    public class MaintenanceController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public MaintenanceController()
        {
        }

        public MaintenanceController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        ////[ExcelValidation]
        // GET: Maintenance
        [Authorize(Roles = Roles.SystemAdministrator)]
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        ////[ExcelValidation]
        [Authorize(Roles = Roles.SystemAdministrator)]
        [HttpPost]
        public async Task<ActionResult> Upload()
        {
            var files = Request.Files;

            HttpPostedFileBase file = Request.Files[0];

            IEnumerable<PersonModel> users = new List<PersonModel>();

            if ((file != null) && (file.ContentLength != 0) && !string.IsNullOrEmpty(file.FileName))
            {
                string fileName = file.FileName;
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                var excelParser = new ExcelSomething<PersonModel>(
                    file.InputStream,
                    new EntityDescription(ExcelHelper.DefinitionFromConfig()));

                users = excelParser.ReadFromExcel();
                ViewBag.fileCount = users.Count();
            }

            var dbContext = HttpContext.GetOwinContext().Get<DbContext>();
            var studentRole = dbContext.Roles.FirstOrDefault(x => x.Name == Roles.Student);

            List<string> exceptions = new List<string>();

            foreach (var usr in users)
            {
                usr.TrimStrings();
                using (var tran = dbContext.BeginTran())
                {
                    try
                    {
                        var person = usr.CastTo<PersonModel, Person>();

                        await this.UserManager.CreateAsync(person)
                            .ContinueWith((Task<IdentityResult> res) =>
                            {
                                if (res.Result.Succeeded)
                                {
                                    UserManager.AddToRole(person.Id, Roles.Student);
                                    usr.SuccessfulyImported = true;
                                }
                            });

                        dbContext.SaveChanges();

                        tran.Commit();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        break;
                    }
                }
            }

            // TODO: Further Improvement. insert batch by 50;

            //// batch by 50;
            //using (var tran = dbContext.BeginTran())
            //{

            //    var userBatches = users.Chunk(50);
            //    Parallel.ForEach(userBatches, async (IEnumerable<PersonModel> batch) =>
            //    {
            //        foreach (var usr in batch)
            //        {
            //            usr.TrimStrings();

            //            try
            //            {
            //                var person = usr.CastTo<PersonModel, Person>();

            //                await this.UserManager.CreateAsync(person)
            //                .ContinueWith((Task<IdentityResult> res) =>
            //                {
            //                    res.Wait();

            //                    if (res.Result.Succeeded)
            //                    {
            //                        UserManager.AddToRole(person.Id, Roles.Student);
            //                        usr.SuccessfulyImported = true;
            //                    }
            //                });

            //                dbContext.SaveChanges();



            //            }
            //            catch (Exception ex)
            //            {
            //                //   tran.Rollback();
            //                break;
            //            }
            //        }

            //    });

            //    tran.Commit();
            //}

            return View(users);
        }

        [Authorize(Roles = Roles.SystemAdministrator)]
        public ActionResult Persons()
        {
            return View("PersonsDataTables");
        }

        [Authorize(Roles = Roles.SystemAdministrator)]
        public ActionResult Edit(string Id)
        {
            ViewBag.UserId = Id;
            return View();
        }

        [Authorize(Roles = Roles.SystemAdministrator)]
        public ActionResult StudyYearsList(int page = 1, int take = 20, int total = 0)
        {
            using (var ctx = new DbContext())
            {
                var pageCount = ctx.StudyYears.Count();
                ViewBag.Total = pageCount;

                if (pageCount == 0)
                {
                    return View();
                }

                var years = ctx.StudyYears
                    .OrderBy(x => x.Start)
                    .Skip(page > 0 ? (page - 1) * take : 0)
                    .Take(take)
                     .ToList()
                    .Select(x => x.CastTo<StudyYear, StudyYearModel>());

                return View(years);
            }


        }

        [Authorize(Roles = Roles.SystemAdministrator)]
        public ActionResult CreateYear()
        {
            return View();
        }

        [Authorize(Roles = Roles.SystemAdministrator)]
        public ActionResult EditYear(long id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}