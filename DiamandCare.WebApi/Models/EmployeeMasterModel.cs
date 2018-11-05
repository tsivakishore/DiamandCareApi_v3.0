using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi.Models
{
    public class EmployeeMasterModel
    {
        public int ID { get; set; }
        public string Designation { get; set; }
        public string DesignationCode { get; set; }
        public decimal RegIncentive { get; set; }
        public decimal LoanRePayIncentive { get; set; }
        public int RecruitmentsReq { get; set; }
        public int TargetJoineesPerMonth { get; set; }
        public decimal Salary { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}