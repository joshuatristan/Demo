using DukeInventorySysem.Models;
using DukeInventorySysem.Models.Entity;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Security;

namespace DukeInventorySysem.Controllers
{

    public class HomeController : Controller
    { 
        public HomeController()
        {

        }

        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            { 
                return RedirectToAction("Login", "Login"); 
            }
            else
            {
                using (var context = new DatabaseContext())
                {
                    double workingCount = 0;
                    double notWorkingCount = 0;
                    double disposalCount = 0;
                    var assets = new List<Asset>();
                    assets = context.Assets.ToList();
                    Debugger.NotifyOfCrossThreadDependency();
                    if (assets.Count() > 0)
                    {
                        
                        foreach (var item in assets)
                        {
                            if (item.Condition == "Working")
                            {
                                workingCount = workingCount + item.Quantity;
                            }
                            if (item.Condition == "Not Working")
                            {
                                notWorkingCount = notWorkingCount + item.Quantity;
                            }
                            if (item.Condition == "For Disposal")
                            {
                                disposalCount = disposalCount + item.Quantity;
                            }
                        } 
                    }
                    ViewBag.Working = workingCount;
                    ViewBag.NotWorking = notWorkingCount;
                    ViewBag.Disposal = disposalCount;
                }



                return View();

            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Logout()
        {

            if (Session["UserName"] == null && Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                Session["UserName"] = null;
                Session["UserID"] = null;
                return RedirectToAction("Login", "Login");
            }
            
        }
    }
}