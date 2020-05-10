using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DukeInventorySysem.Models.Entity
{
    public class EquipmentCondition
    {
        [Key]
        public string ID { get; set; }

        public string Condition { get; set; }
    }
}