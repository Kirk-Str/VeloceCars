using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VeloceCars.Models.VehicleViewModels
{
    public class VehicleViewModel
    {
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Make")]
        public string Make { get; set; }

        [Required]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Plate No.")]
        public string PlateNo { get; set; }

        [Required]
        [Display(Name = "Insurance No.")]
        public string InsuranceNo { get; set; }
    }
}
