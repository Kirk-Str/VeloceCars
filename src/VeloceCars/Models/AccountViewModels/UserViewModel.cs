using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VeloceCars.Models.RoleViewModels;

namespace VeloceCars.Models.AccountViewModels
{
    public class UserViewModel
    {

        public int Id { get; set; }
        [Required]
        public ApplicationUser ApplicationUser { get; set; }

        //[Required]
        //[Display(Name = "User Role")]
        //public RolesViewModel Role { get; set; }

        [Required]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Contact No. (Home)")]
        public string ContactHome { get; set; }

        [Required]
        [Display(Name = "Contact No. (Office)")]
        public string ContactOffice { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //public UserViewModel(ApplicationUser applicationUser)
        //{
        //    ApplicationUser = applicationUser;

        //    //Role = new RolesViewModel();
        //}
    }
}
