using System;
using System.ComponentModel.DataAnnotations;

namespace VeloceCars.Models
{
    public class Reservation
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }

        [Required]
        [Display(Name = "Package")]
        public int PackageId { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode  = true)]
        public DateTime StartDateTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "Request")]
        public string Request { get; set; } // Additional Request from Client

        [Display(Name = "Reply")]
        public string Reply { get; set; } // Reply from HQ

        [Display(Name = "Feedback")]
        public string Feedback { get; set; } // Feedback from Client

        [Display(Name = "Status")]
        public int? Status { get; set; } // 1 = Accepted, 0 = Rejected

        [Display(Name = "Request Type")]
        public int? CreatedBy { get; set; } // 0 = By Client, 1 = By Administrator

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Package Package { get; set; }
        
    }
}
