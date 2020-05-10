using DukeInventorySysem.Models;
using DukeInventorySysem.Models.Entity;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DukeInventorySysem.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Home");
            } 
            return View();
        }



        public ActionResult Verify(User user)
        {
            using (var context = new DatabaseContext())
            {
                var result = context.Users.FirstOrDefault(s => s.UserName == user.UserName && s.Password == user.Password);
                if (result == null)
                {
                    ViewBag.Message = "Invalid Credentials!";
                    return View("Login");
                }
                else
                {
                    Session["UserName"] = result.UserName;
                    Session["UserID"] = result.UserID;
                    return RedirectToAction("Index","Home");
                }
            }
            
        }

 
    }
}