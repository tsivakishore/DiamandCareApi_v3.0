using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class FeeMasterModel
    {
        public int FeeMasterID { get; set; }
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public decimal CourseFee { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}