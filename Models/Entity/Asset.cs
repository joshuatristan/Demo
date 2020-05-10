using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DukeInventorySysem.Models.Entity
{
    public class Asset
    {
        [Key]
        public string AssetOrSerialNumber { get; set; }

        public string ItemDescription { get; set; }
        public string ItemType { get; set; }
        public string EquipmentLocation { get; set; }
        public string HardwareSpecs { get; set; }
        public string Condition { get; set; }
        public string Remarks { get; set; }
        public double Quantity { get; set; }
        public string AccountedTo { get; set; }
        public DateTime DateOfPurchase { get; set; }

    }
}