using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DukeInventorySysem.Models.Entity
{
    public class EquipmentType
    {
        [Key]
        public string ID { get; set; }

        public string Type { get; set; }
    }
}