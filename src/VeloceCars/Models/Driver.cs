using System.ComponentModel.DataAnnotations;

namespace VeloceCars.Models
{
    public class Driver
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Driver")]
        public string ApplicationUserId { get; set; }

        [Required]
        public int VehicleId { get; set; }


        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Vehicle Vehicle { get; set; }


    }
}
