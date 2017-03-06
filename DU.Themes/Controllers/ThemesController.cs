using System.Web.Mvc;

namespace DU.Themes.Controllers
{
    [Authorize]
    public class ThemesController : BaseController
    {
        // GET: Themes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}