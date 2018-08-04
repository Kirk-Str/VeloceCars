using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VeloceCars.Models.ReservationModels
{
    public class ReservationListViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Client")]
        public string Client { get; set; }

        [Display(Name = "Package")]
        public string Package { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime StartDateTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime EndDateTime { get; set; }

        public string Request { get; set; }

        public string Reply { get; set; }

        public int? Status { get; set; } //0 = Rejected, 1 = Accepted

        public string Feedback { get; set; }

        [Display(Name = "Created By")]
        public int? CreatedBy { get; set; } // Null = By Client, 1 = By Administrator

    }
}
