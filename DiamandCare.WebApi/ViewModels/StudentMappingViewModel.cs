using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class StudentMappingViewModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string StudentName { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public int CourseMasterID { get; set; }
        public int CourseID { get; set; }
        public int GroupID { get; set; }
        public decimal Fees { get; set; }
        public int ApprovalStatusID { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public int TransferStatusID { get; set; }
        public string TransferBy { get; set; }
        public string TransferOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
    }
}