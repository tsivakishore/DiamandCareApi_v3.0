using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class FeeMastersModel
    {
        public int FeeMasterID { get; set; }
        public string CourseName { get; set; }
        public decimal CourseFee { get; set; }
    }
}