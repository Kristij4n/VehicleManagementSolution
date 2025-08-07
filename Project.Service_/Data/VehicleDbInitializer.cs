using System.Data.Entity;
using Project.Service_.Models;

namespace Project.Service_.Data
{
    public class VehicleDbInitializer : DropCreateDatabaseIfModelChanges<VehicleDbContext>
    {
        // delete old database and run project again to create new database with new data
        protected override void Seed(VehicleDbContext context)
        {
            var bmw = context.VehicleMakes.Add(new VehicleMake { Name = "BMW", Abrv = "BMW" });
            var ford = context.VehicleMakes.Add(new VehicleMake { Name = "Ford", Abrv = "FRD" });
            var vw = context.VehicleMakes.Add(new VehicleMake { Name = "Volkswagen", Abrv = "VW" });
            context.SaveChanges();

            context.VehicleModels.Add(new VehicleModel { Name = "128", Abrv = "128", MakeId = bmw.Id });
            context.VehicleModels.Add(new VehicleModel { Name = "325", Abrv = "325", MakeId = bmw.Id });
            context.VehicleModels.Add(new VehicleModel { Name = "X5", Abrv = "X5", MakeId = bmw.Id });
            context.VehicleModels.Add(new VehicleModel { Name = "Fiesta", Abrv = "FST", MakeId = ford.Id });
            context.SaveChanges();
        }
    }
}