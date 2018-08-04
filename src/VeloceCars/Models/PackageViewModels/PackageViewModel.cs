using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VeloceCars.Models.PackageViewModels
{
    public class PackageViewModel
    {
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Package")]
        public string PackageName { get; set; }

        [Required]
        [Display(Name = "Scheme")]
        public string Route { get; set; }

        [Required]
        [Display(Name = "Rate per Day")]
        [DisplayFormat(DataFormatString = "{0:0.00 €}", ApplyFormatInEditMode = true)]
        public string Price { get; set; }
    }
}
