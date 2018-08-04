using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VeloceCars.Models.UserViewModels
{
    public class ApplicationUserListViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Contact No. (Home)")]
        public string ContactHome { get; set; }

        [Display(Name = "Contact No. (Office)")]
        public string ContactOffice { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Role Type")]
        public string RoleType { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public string Fullname
        {
            get { return Firstname + ' ' + Lastname; }
        }
    }

}
