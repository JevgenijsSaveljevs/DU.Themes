using DU.Themes.Infrastructure;
using System.Web.Mvc;

namespace DU.Themes.Controllers
{
    [Authorize]
    public class ThemesController : BaseController
    {
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
    }
}