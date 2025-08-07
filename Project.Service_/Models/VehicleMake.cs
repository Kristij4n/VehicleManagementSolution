using Project.Service_.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service_.Models
{
    public class VehicleMake
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }

        // Optional - navigation property for related VehicleModels
        public virtual ICollection<VehicleModel> VehicleModels { get; set; }
    }
}
