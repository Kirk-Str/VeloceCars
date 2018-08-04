using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VeloceCars.Models.ReservationModels
{
    public class ReservationReviewViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public int PackageId { get; set; }

        [Display(Name = "Client")]
        public string ApplicationUser { get; set; }


        [Display(Name = "Package")]
        public string Package { get; set; }

        [Required]
        [Display(Name = "Driver")]
        public int DriverId { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDateTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "Additional Requests")]
        public string Request { get; set; } // Additional Request from Client

        public string Reply { get; set; } // Reply from HQ

        public string Feedback { get; set; } // Client Feedback

        public int? Status { get; set; } // 0 = Rejected, 1 = Accepted

        [Display(Name = "Created By")]
        public int? CreatedBy { get; set; } // Null = By Client, 1 = By Administrator

        public SelectList DriverList { get; set; }

    }
}
