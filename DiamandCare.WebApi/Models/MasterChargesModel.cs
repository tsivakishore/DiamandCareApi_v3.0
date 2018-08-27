using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamandCare.WebApi.Models
{
    public class MasterChargesModel
    {
        public int MasterID { get; set; }
        public decimal DocumentationAdminFee { get; set; }
        public decimal DocumentationAdminFee1 { get; set; }
        public decimal PrepaidLoanCharges { get; set; }
        public decimal RegistrationCharges { get; set; }
        public decimal AreaFee { get; set; }
        public decimal DistrictFee { get; set; }
        public decimal DistrictClusterFee { get; set; }
        public decimal StateFee { get; set; }
        public decimal StateClusterFee { get; set; }
        public decimal MotherFee { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal TDS { get; set; }
        public decimal PinCommission { get; set; }
        public decimal FundTransferCharges { get; set; }

    }
}
