using Microsoft.Owin.Security;
using MyBlog.Engine;
using MyBlog.Engine.Data.Models;
using MyBlog.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (UserService.Get() == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }

        [AllowAnonymous]
        public ActionResult Login(String returnUrl)
        {
            AccountProviders model = new AccountProviders
            {
                Providers = GetAccountProviders().ToArray()
            };

            // Test return url
            model.ReturnUrl = String.IsNullOrEmpty(returnUrl)
                ? Settings.Current.Url
                : returnUrl;

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult _Login(String url)
        {
            AccountProviders model = new AccountProviders
            {
                Providers = GetAccountProviders().ToArray()
            };

            model.ReturnUrl = url;
            return PartialView(model);
        }

        private static IEnumerable<AccountProvider> GetAccountProviders()
        {
            if (Settings.Current.TwitterAuthentication)
            {
                yield return new AccountProvider
                {
                    Style = "twitter",
                    Icon= "twitter",
                    Provider = "Twitter",
                    Name = "Twitter"
                };
            }
            if (Settings.Current.FacebookAuthentication)
            {
                yield return new AccountProvider
                {
                    Style = "facebook",
                    Icon = "facebook-f",
                    Provider = "Facebook",
                    Name = "Facebook"
                };
            }
            if (Settings.Current.GoogleAuthentication)
            {
                yield return new AccountProvider
                {
                    Style = "google",
                    Icon = "google-plus-g",
                    Provider = "Google",
                    Name = "Google"
                };
            }
            if (Settings.Current.MicrosoftAccountAuthentication)
            {
                yield return new AccountProvider
                {
                    Style = "microsoft",
                    Icon = "windows",
                    Provider = "Microsoft",
                    Name = "Microsoft"
                };
            }
        }

        /// <summary>
        /// Log out
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            var am = HttpContext.GetOwinContext().Authentication;
            // Get all authentication types
            var types = am.GetAuthenticationTypes()
                .Select(c => c.AuthenticationType).ToArray();
            // Sig out on all authentication types
            am.SignOut(types);

            // clear session
            UserService.Clear();

            // Redirect to success page
            return RedirectToAction("LogoutSuccess");
        }

        /// <summary>
        /// Log out successed
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ViewResult LogoutSuccess()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(String provider, String returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(
                provider,
                Settings.Current.Url + Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(String returnUrl)
        {
            // Get user from claims
            var user = UserService.GetFromClaims();
            if (user == null) return View();

            // Set user in session to use later
            UserService.Set(user);

            // Test new user, to edit profile
            if (user.Id == 0)
            {
                return RedirectToAction("Edit");
            }

            // Redirection
            return Redirect(returnUrl);
        }


        #region Edition

        public ActionResult Edit()
        {
            // Get current user
            UserProfile user = UserService.GetFreshUseerUpdatedFromDataBase();
            if (user == null) return HttpNotFound();

            // Set model
            EditUserProfile model = new EditUserProfile
            {
                User = user,
                Success = null // none action done at this time
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> Edit(EditUserProfile model)
        {
            // Test model
            if (ModelState.IsValid)
            {
                // Get current user from session
                UserProfile user = UserService.Get();

                // Update editable data
                user.Name = model.User.Name;
                user.Email = model.User.Email;

                // update database
                using (var db = new DataService())
                {
                    // Try to save changes
                    model.Success = await db.EditUser(user);
                    // Refresh
                    model.User = user;
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<PartialViewResult> SendValidationMail()
        {
            Boolean model;
            // Get current user
            UserProfile user = UserService.Get();
            if (user == null)
            {
                model = false;
            }
            else
            {
                using (var db = new DataService())
                {
                    model = await db.SendValidationMail(user.Id);
                }
            }
            return PartialView("_SendValidationMail", model);
        }

        /// <summary>
        /// ValidateMail (url send by mail to users)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ActionResult ValidateMail(Int32 id, Guid token)
        {
            Boolean model;
            using (var db = new DataService())
            {
                model = db.ValidateMail(id, token);
            }
            return View(model);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// ChallengeResult for OWIN
        /// </summary>
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                var owin = context.HttpContext.GetOwinContext();
                owin.Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion

    }
}