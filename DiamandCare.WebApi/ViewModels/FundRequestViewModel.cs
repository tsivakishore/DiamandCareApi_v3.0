using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class FundRequestViewModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string RequestedBy { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public int RequestToUserID { get; set; }
        public string RequestedTo { get; set; }
        public int RequestStatusID { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}