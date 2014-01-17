using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using TradesWebApplication.Models;
using System.Web.Security;

namespace TradesWebApplication.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult LogOn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            //clean-up Username if it's email or contains GLOBAL domain
            var username = model.UserName;
            //global.root\
            if (username.Contains(@"\"))
            {
                var domain = username.Substring(0, username.IndexOf(@"\"));   

                if (domain.ToLower() != "global" && domain.ToLower() != "global.root")
                {
                    ModelState.AddModelError("", "Invalid domain.");
                }

                username = username.Substring(username.LastIndexOf(@"\") + 1);
            }
            //email entered
            if (username.Contains(@"@"))
            {
                var domain = username.Substring(username.IndexOf(@"@") + 1);
                
                if (domain.ToLower() != "bcaresearch.com" && domain.ToLower() != "euromoneyplc.com")
                {
                    ModelState.AddModelError("", "Unrecognized email domain.");
                }

                username = username.Substring(0, username.IndexOf(@"@"));
    
            }

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(username, model.Password))
                {
                  
                    FormsAuthentication.SetAuthCookie(username, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}