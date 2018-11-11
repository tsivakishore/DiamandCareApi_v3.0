using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi.Models
{
    public class UpgradeEmployeeModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int DesignationID { get; set; }
        public string Designation { get; set; }
        public int UnderEmployeeID { get; set; }
        public decimal RegIncentive { get; set; }
        public decimal LoanPayIncentive { get; set; }
        public bool RecruitmentReq { get; set; }
        public bool ConditionsApplySelf { get; set; }
        public bool ConditionsApplyGroup { get; set; }
        public int TargetJoineesPerMonth { get; set; }
        public decimal Salary { get; set; }
        public string Description { get; set; }
        public string UnderEmployeeName { get; set; }
    }
}