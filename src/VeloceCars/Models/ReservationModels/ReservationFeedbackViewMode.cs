using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VeloceCars.Models.ReservationModels
{
    public class ReservationFeedbackViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Feedback { get; set; }
    }
}
