using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class FranchiseRequestResponse
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime RequestedMonth { get; set; }
        public int StatusID { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}