using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VeloceCars.Models
{
    public class Schedule
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ReservationId { get; set; }

        [Required]
        public int DriverId { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDateTime { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDateTime { get; set; }

        [Display(Name = "Total Fare")]
        public double? NetTotal { get; set; }

        [Display(Name = "Journey Status")]
        public int? Status { get; set; } // Null = Not Started, 0 = Started, 1 = Ended


        public virtual Driver Driver { get; set; }
        //public virtual Reservation Rervertion { get; set; }

    }
}
