using DU.Themes.Infrastructure.RemoteAuhtentication;
using DU.Themes.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DU.Themes.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
            this.AuthenticationService = new DUAuthenticationService(AppConfig.AuthenticationUrl);
            this.AuthenticationService.ConfigureCertificateValidation();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public IAuthenticationService AuthenticationService { get; private set; }

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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.LoginName.Equals(ConfigurationManager.AppSettings.Get("SysAdmEmail"), StringComparison.InvariantCultureIgnoreCase))
            {
                return await this.LoginAsAdmin(model, returnUrl);
            }

            var usr = UserManager.FindByName(model.LoginName);

            if (usr == null)
            {
                ModelState.AddModelError("", ModelResources.NoUser);
                return View(model);
            }

            return await this.LoginInternal(model, returnUrl, usr);
        }

        private async Task<ActionResult> LoginInternal(LoginViewModel model, string returnUrl, Entities.Person usr)
        {
            if (AppConfig.IsDevelopment)
            {
                await SignInManager.SignInAsync(usr, true, model.RememberMe);

                return RedirectToLocal(returnUrl);
            }
            try
            {
                var response = await this.AuthenticationService.PostCredentials(model.LoginName, model.Password);

                if (this.AuthenticationService.IsAuthenticated(response))
                {
                    await SignInManager.SignInAsync(usr, true, model.RememberMe);

                    return RedirectToLocal(returnUrl);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("", ModelResources.BadCredentials);
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("", string.Format(ModelResources.AuthenticationServerErrorPlaceholder, response.StatusCode));
                    return View(model);
                }
            }
            catch (HttpRequestException webEx)
            {
                var baseEx = webEx.GetBaseException();                
                ModelState.AddModelError("", string.Format(ModelResources.AuthenticationServerErrorPlaceholder, baseEx.Message));
                return View(model);
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        ////
        //// GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private async Task<ActionResult> LoginAsAdmin(LoginViewModel model, string returnUrl)
        {
            var admin = UserManager.FindByEmail(ConfigurationManager.AppSettings.Get("SysAdmEmail"));

            var result = await SignInManager.PasswordSignInAsync(admin.UserName, model.Password, true, false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Request");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}