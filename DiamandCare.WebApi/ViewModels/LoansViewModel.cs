using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class LoansViewModel
    {
        public int LoanID { get; set; }
        public int UserID { get; set; }
        public int GroupsID { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal IssuedAmount { get; set; }
        public decimal AmountToPay { get; set; }
        public decimal AdminCharges { get; set; }
        public int ModeofTransfer { get; set; }
        public int LoanStatusID { get; set; }
        public string LoanTypeCode { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool PrePaidLoan { get; set; }
        public int IsApproved { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal TDS { get; set; }
    }
}