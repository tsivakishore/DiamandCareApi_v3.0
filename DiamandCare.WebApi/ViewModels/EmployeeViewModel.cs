using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi.ViewModels
{
    public class EmployeeViewModel
    {
        public string EmployeeName { get; set; }
        public string ImageName { get; set; }
        public byte[] ImageContent { get; set; }
        public string Designation { get; set; }
    }
}