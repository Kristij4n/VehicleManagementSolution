using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Project.Service_.Models;

namespace Project.Service_.Data
{
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext() : base("DefaultConnection") { }

        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VehicleModel>()
                .HasRequired(vm => vm.Make)
                .WithMany(vm => vm.VehicleModels)
                .HasForeignKey(vm => vm.MakeId);
        }
    }
}