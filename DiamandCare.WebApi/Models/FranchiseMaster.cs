using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi.Models
{
    public class FranchiseMaster
    {
        public int ID { get; set; }
        public string FranchiseType { get; set; }
        public string PaymentReceiptPercentage { get; set; }
        public int TargetJoineesPerMonth { get; set; }
        public int MinimumJoineesAvg { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class Franchise
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int UpgradeTo { get; set; }
        public int FranchiseTypeID { get; set; }
        public bool ConditionsApplySelf { get; set; }
        public bool ConditionsApplyUnderJoinees { get; set; }
        public int FranchiseJoinees { get; set; }
        public int FranchiseJoineesMinimum { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UnderFranchiseID { get; set; }
    }
    public class UpgradeTo
    {
        public int ID { get; set; }
        public string UpgradeType { get; set; }
        public string Description { get; set; }
    }
    public class FranchiseTypes
    {
        public int ID { get; set; }
        public string FranchiseType { get; set; }
    }
}