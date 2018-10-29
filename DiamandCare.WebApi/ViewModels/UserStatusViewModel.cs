using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi.ViewModels
{
    public class UserStatusViewModel
    {
        public int UserStatusID { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}