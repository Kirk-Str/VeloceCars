using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VeloceCars.Models.DriverViewModels
{
    public class DriverListViewModel
    {

        [Required]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Driver")]
        public string ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Vehicle")]
        public string Vehicle { get; set; }

        public int? Status { get; set; }
       
    }

}
