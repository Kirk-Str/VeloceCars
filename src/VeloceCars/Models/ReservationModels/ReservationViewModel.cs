using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VeloceCars.Models.ReservationModels
{
    public class ReservationViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Display(Name = "User")]
        public string ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Package")]
        public int PackageId { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime StartDateTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "Additional Requests")]
        public string Request { get; set; } // Additional Request from Client

        [Display(Name = "Tour Cost")]
        //[DisplayFormat(DataFormatString = "{0:0.00 €}", ApplyFormatInEditMode = true)]
        public string NetTotal { get; set; }
        //public string Reply { get; set; } // Reply from HQ
        //public int Status { get; set; } // 0 = Rejected, 1 = Accepted

        //public SelectList ApplicationUserList { get; set; }

        public SelectList PackageList { get; set; }


    }
}
