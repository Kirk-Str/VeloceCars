using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VeloceCars.Models.DriverViewModels
{
    public class DriverViewModel
    {
        [Required]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Driver")]
        public string ApplicationUserId { get; set; }

        [Required]
        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        public SelectList ApplicationUserList { get; set; }

        public SelectList VehicleList { get; set; }
       
    }

}
