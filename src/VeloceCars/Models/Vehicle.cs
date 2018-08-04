using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VeloceCars.Models
{
    public class Vehicle
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string Make { get; set; }
        public string Model { get; set; }

        [Required]
        [Display(Name = "Plate No.")]
        public string PlateNo { get; set; }

        [Required]
        [Display(Name = "Insurance No.")]
        public string InsuranceNo { get; set; }

        public string VehicleName
        {
            get { return Make + ' ' + Model; }
        }
        
    }
}
