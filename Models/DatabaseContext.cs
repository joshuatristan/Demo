using DukeInventorySysem.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DukeInventorySysem.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<EquipmentCondition> EquipmentConditions { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Location> Locations { get; set; }

        public DatabaseContext(): base(nameOrConnectionString: "Default")
        {
     
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }

    }
}