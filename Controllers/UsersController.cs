using DukeInventorySysem.Models;
using DukeInventorySysem.Models.Entity;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Misc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace DukeInventorySysem.Controllers
{
    public class UsersController : Controller
    {

        private const String Letters = "abcdefghijklmnopqrstuvwxyz";
        private readonly char[] Alphanumeric = (Letters + Letters.ToUpper() + "0123456789").ToCharArray();
        // GET: Users
        public ActionResult Users(string searchString)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (searchString == "" || searchString == null)
            {
                using (var context = new DatabaseContext())
                {
                    var user = context.Users.OrderByDescending(s => s.FirstName).ToList();
                    ViewBag.SearchString = "";
                    ViewBag.rowCount = user.Count();
                    return View(user);
                }
            }
            else
            {
                using (var context = new DatabaseContext())
                {
                    var user = new List<User>();
                    user = context.Users.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString) || s.UserName.Contains(searchString) || s.EmailAddress.Contains(searchString)).ToList();
                    Debugger.NotifyOfCrossThreadDependency();
                    ViewBag.SearchString = "Search result/s for '" + searchString + "'";
                    ViewBag.rowCount = user.Count();
                    return View(user);
                }
            }  
        }


        [HttpPost]
        public ActionResult SearchUser(string searchString)
        {
            if (searchString == "" || searchString == null)
            {
                using (var context = new DatabaseContext())
                {
                    var user = context.Users.OrderByDescending(s => s.FirstName).ToList();
                    ViewBag.SearchString = "";
                    ViewBag.rowCount = user.Count();
                    return RedirectToAction("Users", "Users", user);
                }
            }
            else
            {
                using (var context = new DatabaseContext())
                {
                    var user = new List<User>();
                    user = context.Users.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString) || s.UserName.Contains(searchString) || s.EmailAddress.Contains(searchString)).ToList();
                    Debugger.NotifyOfCrossThreadDependency();
                    ViewBag.SearchString = "Search result/s for '" + searchString + "'";
                    ViewBag.rowCount = user.Count();
                    return View("Users", user);
                }
            }
        }



        [HttpGet]
        public JsonResult GetUser(string Id)
        {
            using (var context = new DatabaseContext())
            {
                var user = context.Users.FirstOrDefault(s => s.UserID == Id);
                if (user != null)
                {
                    return Json(user, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("User not found", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult AddUser(User newUser, string retypePassword)
        {
            var id = GenerateID(15);
            string error = "";
            if (newUser.Password != retypePassword)
            {
                error = error + "• Password doesn't match <br>";
            }
            if (newUser.Password.Count() < 8)
            {
                error = error + "• Password should be atleast 8 characters <br>";
            }

            using (var context = new DatabaseContext())
            {
                var res = context.Users.FirstOrDefault(s => s.UserName == newUser.UserName);
                if (res != null)
                {
                    error = error + "• The Username '" + newUser.UserName + "' is already taken <br>";
                }
            }

            if (error == "")
            {
                using (var context = new DatabaseContext())
                {
                    var newUserToAdd = new User();
                    newUserToAdd.UserID = id;
                    newUserToAdd.UserName = newUser.UserName;
                    newUserToAdd.Password = newUser.Password;
                    newUserToAdd.EmailAddress = newUser.EmailAddress;
                    newUserToAdd.LastName = newUser.LastName;
                    newUserToAdd.FirstName = newUser.FirstName;
                    try
                    {
                        context.Users.Add(newUserToAdd);
                        context.SaveChanges();
                        return Json("ok", JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception ex)
                    {
                        error = error + ex.ToString();
                        return Json(error, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json(error, JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult VerifyUserName(string id)
        {
            using (var context = new DatabaseContext())
            {
                var res = context.Users.Where(s => s.UserName == id);
                if (res != null)
                {
                    var myReponse = new MyResponse();
                    myReponse.Message = "invalid";
                    var json = JsonConvert.SerializeObject(myReponse);
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var myReponse = new MyResponse();
                    myReponse.Message = "valid";
                    var json = JsonConvert.SerializeObject(myReponse);
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
            }

        }

        private String GenerateID(int length)
        {
            StringBuilder result = new StringBuilder();
            while (true)
            {
                Random rn = new Random();
                for (int i = 0; i < length; i++)
                {
                    result.Append(Alphanumeric[rn.Next(Alphanumeric.Length)]);
                }

                using (var context = new DatabaseContext())
                {
                    var test = result.ToString();
                    var res = context.Users.FirstOrDefault(s => s.UserID == test);
                    if (res == null)
                    {
                        break;
                    }
                }
            }


            return result.ToString();
        }

        [HttpDelete]
        public JsonResult DeleteUser(string Id)
        {
            string error = "";

            if (Session["UserID"].ToString() == Id)
            {
                return Json("This user is currently signed in",JsonRequestBehavior.AllowGet);
            }
            using (var context = new DatabaseContext())
            {
                var res = context.Users.FirstOrDefault(s => s.UserID == Id);
                if (res != null)
                {
                    try
                    {
                        context.Users.Remove(res);
                        context.SaveChanges();
                        return Json("ok", JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception ex)
                    {
                        return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    error = "Error: User not found";
                }
            }
            return Json(error, JsonRequestBehavior.AllowGet);
        }

        [HttpPatch]
        public JsonResult UpdateUser(User user)
        {
            string error = "";
            string oldUserName = user.UserName;

            using (var context = new DatabaseContext())
            {
                var res = context.Users.FirstOrDefault(s => s.UserID == user.UserID);
                if (res != null)
                {
                    if (res.UserName != user.UserName)
                    {
                        using (var Innercontext = new DatabaseContext())
                        {
                            var check = Innercontext.Users.FirstOrDefault(s => s.UserName == user.UserName);
                            if (check != null)
                            {
                                error = error + "• The Username '" + user.UserName + "' is already taken <br>";
                            }
                        }
                    }
                }
            }
            using (var context = new DatabaseContext())
            {
                var res = context.Users.FirstOrDefault(s => s.UserID == user.UserID);
                if (res == null)
                {
                    return Json("User not found", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (error == "")
                    {
                        res.FirstName = user.FirstName;
                        res.LastName = user.LastName;
                        res.EmailAddress = user.EmailAddress;
                        res.UserName = user.UserName;
                        context.SaveChanges();
                        return Json("ok", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(error, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        [HttpGet]
        public JsonResult GetUserId(string Id)
        {
            using (var context = new DatabaseContext())
            {
                var res = context.Users.FirstOrDefault(s => s.UserID == Id);
                if (res != null)
                {
                    return Json(res.UserID, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("User not found!", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult ChangePassword(string UserID, string Password, string NewPassword, string ConfirmPassword)
        {
            var error = "";

            if (NewPassword != ConfirmPassword)
            {
                error = error + "• Password doesn't match <br>";
            }
            if (NewPassword.Length < 8)
            {
                error = error + "• Password should be atleast 8 characters <br>";
            }
            using (var context = new DatabaseContext())
            {
                var res = context.Users.FirstOrDefault(s => s.UserID == UserID);
                if (res != null)
                {
                    if (res.Password == Password)
                    {
                        if (error == "")
                        {
                            res.Password = NewPassword;
                            context.SaveChanges();
                            return Json("ok", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(error, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        error = error + "• Wrong old password <br>";
                        return Json(error, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("User not found!", JsonRequestBehavior.AllowGet);
                }
            }

        }


    }
}