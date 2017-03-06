using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DU.Themes.Controllers
{
    [Authorize]
    public class PeopleController : BaseController
    {
        // GET: People
        public ActionResult Teachers()
        {
            return View();
        }
    }
}