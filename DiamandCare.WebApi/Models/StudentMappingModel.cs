using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class StudentMappingModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string StudentName { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public int FeeMasterID { get; set; }
        public int GroupID { get; set; }
        public decimal CourseFee { get; set; }
        public int ApprovalStatusID { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public int TransferStatusID { get; set; }
        public int TransferBy { get; set; }
        public DateTime TransferOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}