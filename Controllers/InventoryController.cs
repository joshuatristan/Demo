using DukeInventorySysem.Models;
using DukeInventorySysem.Models.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

namespace DukeInventorySysem.Controllers
{
    public class InventoryController : Controller
    {
        private const String Letters = "abcdefghijklmnopqrstuvwxyz";
        private readonly char[] Alphanumeric = (Letters + Letters.ToUpper() + "0123456789").ToCharArray();
        // GET: Iventory
        public ActionResult Inventory(string searchString)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            using (var context = new DatabaseContext())
            {
                ViewData["itemTypes"] = context.EquipmentTypes.ToList();
                ViewData["locations"] = context.Locations.ToList();
                ViewData["conditions"] = context.EquipmentConditions.ToList();
            }

            if (searchString == "" || searchString == null)
            {
                using (var context = new DatabaseContext())
                {
                    var assets = new List<Asset>();
                    assets = context.Assets.OrderByDescending(s => s.ItemDescription).ToList();
                    ViewBag.SearchString = "";
                    return View("Inventory", assets);
                }
            }
            else
            {
                using (var context = new DatabaseContext())
                {
                    var assets = new List<Asset>();
                    assets = context.Assets.Where(s => s.AssetOrSerialNumber.Contains(searchString) || s.ItemDescription.Contains(searchString) || s.ItemType.Contains(searchString) || s.EquipmentLocation.Contains(searchString) || s.Condition.Contains(searchString) || s.AccountedTo.Contains(searchString) || s.HardwareSpecs.Contains(searchString) || s.Remarks.Contains(searchString)).ToList();
                    Debugger.NotifyOfCrossThreadDependency();
                    ViewBag.SearchString = "Search result/s for '" + searchString + "'";
                    return View("Inventory", assets);
                }
            }


        }

        [HttpPost]
        public JsonResult NewAsset(Asset newAsset)
        {
            string newId = GenerateID(15);
            string error = "";

            using (var context = new DatabaseContext())
            {
                var serial = context.Assets.FirstOrDefault(s => s.AssetOrSerialNumber == newAsset.AssetOrSerialNumber);
                if (serial != null)
                {
                    error = error + "• This serial number is already used <br>";
                }
            }

            if (error != "")
            {
                return Json(error, JsonRequestBehavior.AllowGet);
            }
            else
            {
                using (var context = new DatabaseContext())
                {
                    context.Assets.Add(newAsset);
                    context.SaveChanges();
                }
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GenerateAssetNumber()
        {
            var id = GenerateID(15);
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        private string GenerateID(int length)
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
                    var res = context.Assets.FirstOrDefault(s => s.AssetOrSerialNumber == test);
                    if (res == null)
                    {
                        break;
                    }
                }
            }
            return result.ToString();
        }

        [HttpDelete]
        public JsonResult DeleteAsset(string Id)
        {
            using (var context = new DatabaseContext())
            {
                var res = context.Assets.FirstOrDefault(s => s.AssetOrSerialNumber == Id);
                if (res == null)
                {
                    return Json("Asset not found!", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    context.Assets.Remove(res);
                    context.SaveChanges();
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public JsonResult GetAsset(string Id)
        {
            using (var context = new DatabaseContext())
            {
                var res = context.Assets.FirstOrDefault(s => s.AssetOrSerialNumber == Id);
                if (res != null)
                {
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("not found", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPatch]
        public JsonResult UpdateAsset(Asset asset)
        {
            
            using (var context = new DatabaseContext())
            {
                var res = context.Assets.FirstOrDefault(s => s.AssetOrSerialNumber == asset.AssetOrSerialNumber);
                if (res != null)
                {
                    res.ItemDescription = asset.ItemDescription;
                    res.ItemType = asset.ItemType;
                    res.EquipmentLocation = asset.EquipmentLocation;
                    res.HardwareSpecs = asset.HardwareSpecs;
                    res.Condition = asset.Condition;
                    res.Remarks = asset.Remarks;
                    res.Quantity = asset.Quantity;
                    res.AccountedTo = asset.AccountedTo;
                    res.DateOfPurchase = asset.DateOfPurchase;
                    context.SaveChanges();
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("record not found!", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult SearchAsset (string searchString)
        {
            using (var context = new DatabaseContext())
            {
                ViewData["itemTypes"] = context.EquipmentTypes.ToList();
                ViewData["locations"] = context.Locations.ToList();
                ViewData["conditions"] = context.EquipmentConditions.ToList();
            }
            if (searchString == "" || searchString == null)
            {
                using (var context = new DatabaseContext())
                {
                    var assets = new List<Asset>();
                    assets = context.Assets.OrderByDescending(s => s.ItemDescription).ToList();
                    ViewBag.SearchString = "";
                    return View("Inventory", assets);
                }
            }
            else
            {
                using (var context = new DatabaseContext())
                {
                    var assets = new List<Asset>();
                    assets = context.Assets.OrderByDescending(s => s.AssetOrSerialNumber.Contains(searchString) || s.ItemDescription.Contains(searchString) || s.ItemType.Contains(searchString) || s.EquipmentLocation.Contains(searchString) || s.HardwareSpecs.Contains(searchString) || s.Condition.Contains(searchString) || s.Remarks.Contains(searchString)).ToList();
                    ViewBag.SearchString = "Search result/s for '" + searchString + "'";
                    return View("Inventory", assets);
                }
            }
        }





    }
}
