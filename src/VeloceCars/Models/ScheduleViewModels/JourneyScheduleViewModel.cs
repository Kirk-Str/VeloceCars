﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VeloceCars.Models.ScheduleViewModels
{
    public class JourneySchedueViewModel
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Reservation Id")]
        public int ReservationId { get; set; }

        [Display(Name = "Client")]
        public string Client { get; set; }

        [Display(Name = "Package")]
        public string Package { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "Journey Start At")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? JourneyStartDateTime { get; set; }

        [Display(Name = "Journey End At")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? JourneyEndDateTime { get; set; }

        [Display(Name = "Total Fare")]
        [DisplayFormat(DataFormatString = "{0:0.00 €}", ApplyFormatInEditMode = true)]
        public double? NetTotal { get; set;}

        [Display(Name = "Journey Status")]
        public int? Status { get; set; } // Null = Not Started, 0 = Started, 1 = Ended
    }
}
