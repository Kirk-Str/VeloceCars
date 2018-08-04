using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VeloceCars.Models
{
    public class Package
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Package")]
        public string PackageName { get; set; }

        [Display(Name = "Scheme")]
        public string Route { get; set; }

        [Required]
        [Display(Name = "Rate per Day")]
        public double Price { get; set; }

        public byte[] Image { get; set; }
        //[Display(Name ="Image")]
        //[DataType(DataType.Upload)]
        //[FileExtensions(Extensions ="jpg,png,bmp")]
        //public IFormFile Image { get; set; }

    }
}
