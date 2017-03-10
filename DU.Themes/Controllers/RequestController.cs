using DU.Themes.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DU.Themes.Controllers
{
    [Authorize]
    public class RequestController : BaseController
    {
        // GET: Request
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TeacherRequests()
        {
            return View();
        }

        public ActionResult NewRequest()
        {
            return View();
        }

        [Authorize(Roles = Roles.Student)]
        public ActionResult Edit(string Id)
        {
            ViewBag.Id = Id;

            return View();
        }

        [Authorize(Roles = Roles.Teacher)]
        public ActionResult Respond(string Id)
        {
            ViewBag.Id = Id;

            return View();
        }
    }
}