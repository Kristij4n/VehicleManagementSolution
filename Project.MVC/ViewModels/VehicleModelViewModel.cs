using System.ComponentModel.DataAnnotations;

namespace Project.MVC_.ViewModels
{
    public class VehicleModelViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Abrv { get; set; }

        [Required]
        [Display(Name = "Make")]
        public int MakeId { get; set; }

        public string MakeName { get; set; } // for display only
    }
}