using DU.Themes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace DU.Themes.Controllers
{
    //[MyClassAuthorization]
    public class BaseController : Controller
    {
        public BaseController()
        {

        }

        //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        //public class MyClassAuthorizationAttribute : Attribute, IAuthorizationFilter
        //{
        //    public void OnAuthorization(AuthorizationContext filterContext)
        //    {
        //        if (filterContext.HttpContext.User.Identity.IsAuthenticated)
        //        {
        //            using (var ctx = new ApplicationDbContext())
        //            {
        //                var userId = Convert.ToInt64(filterContext.HttpContext.User.Identity.GetUserId());
        //                var roleIds = ctx.CustomUserRole.Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
        //                var roles = ctx.Roles.Where(x => roleIds.Contains(x.Id));

        //                filterContext.Controller.ViewBag["Roles"] = roles;
        //            }

        //        }
        //    }
        //}
    }
}