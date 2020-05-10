using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DukeInventorySysem.Models.Entity
{
    public class User
    {
        [Key]
        public string UserID { get; set; }
        
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}