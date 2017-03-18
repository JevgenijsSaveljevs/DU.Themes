using DU.Themes.Infrastructure;
using System.Web.Mvc;

namespace DU.Themes.Controllers
{
    [Authorize]
    public class ThemesController : BaseController
    {
        [Authorize(Roles = Roles.Teacher)]
        public ActionResult Teacher()
        {
            return View();
        }

        [Authorize(Roles = Roles.Teacher)]
        public ActionResult Edit(string Id)
        {
            ViewBag.Id = Id;

            return View();
        }

        [Authorize(Roles = Roles.Student)]
        public ActionResult Student(string Id)
        {
            return View();
        }
    }
}