using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeloceCars.Models.RoleViewModels
{
    public class RolesViewModel
    {

        public int Key { get; set; }

        public static SelectList RoleTypes
        {
            get { return new SelectList(RoleList, "Value", "Key"); }
        }

        public static readonly IDictionary<string, string> RoleList = new Dictionary<string, string>
        {
            { "Select", "" },
            { "Client", "1" },
            { "Branch User", "2" },
            { "Driver", "3" },
            { "Administrator", "4"}
        };
    }
}
