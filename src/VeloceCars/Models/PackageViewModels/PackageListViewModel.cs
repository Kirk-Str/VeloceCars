
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNet.Http;

namespace VeloceCars.Models.PackageViewModels
{
    public class PackageListViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Package")]
        public string PackageName { get; set; }

        [Display(Name = "Scheme")]
        public string Route { get; set; }

        [Display(Name = "Rate per Day")]
        [DisplayFormat(DataFormatString = "{0:0.00 €}")]
        public double Price { get; set; }

        [Display(Name = "Image")]
        public byte[] Image { get; set; }
        //[Display(Name = "Image")]
        //[DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "jpg,png,bmp")]
        //public IFormFile Image { get; set; }
    }
}
