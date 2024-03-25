using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Businesslayer;

namespace Login_Outsource_Webapplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserContainer userContainer;

        public AccountController()
        {
            userContainer = new UserContainer();
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // GET: /Account/Login
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await userContainer.GetUserByName(model.UserName);
            if (user == null || !PasswordHelper.ValidatePassword(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await userContainer.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe }, identity);

            return RedirectToLocal(returnUrl);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}
}
