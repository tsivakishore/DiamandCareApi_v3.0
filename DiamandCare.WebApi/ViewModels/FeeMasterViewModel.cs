using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class FeeMasterViewModel
    {
        public int FeeMasterID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public decimal CourseFee { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
    }
}