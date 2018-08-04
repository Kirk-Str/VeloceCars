using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace VeloceCars.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        public string Address { get; set; }

        [Phone]
        public string ContactHome { get; set; }

        [Phone]
        public string ContactOffice { get; set; }

        [Required]
        public int RoleId { get; set; }

        public int CreatedBy { get; set; } // 0 = By Client, 1 = By Administrator

        public string Fullname {
            get { return Firstname + ' ' + Lastname; }
        }

    }
}
