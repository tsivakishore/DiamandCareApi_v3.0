using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class Role
    {
        public string RoleID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool DefaultFlag { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}