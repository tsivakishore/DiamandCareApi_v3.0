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
        public string UserName { get; set; }
        public int GroupID { get; set; }
        public double LoanAmount { get; set; }
        public double IssuedAmount { get; set; }
        public double AmountToPay { get; set; }
        public double AdminCharges { get; set; }
        public int ModeofTransfer { get; set; }
        public string ModeofTransferType { get; set; }
        public int LoanStatusID { get; set; }
        public string LoanStatusName { get; set; }
        public string LoanTypeCode { get; set; }
        public string LoanTypeDescription { get; set; }
        public bool PrePaidLoan { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int IsApproved { get; set; }
        public string LoanApprovalStatus { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal TDS { get; set; }
        public decimal AmountPaid { get; set; }
        public int ApproveOrRejectedBy { get; set; }
        public DateTime ApproveOrRejectedOn { get; set; }
        public int TransferBy { get; set; }
        public DateTime TransferOn { get; set; }
        public int TransferStatusID { get; set; }
        public string TransferStatus { get; set; }
        public decimal PrePaidLoanCharges { get; set; }
    }

    public class LoanTransferPendingModel
    {
        public int LoanID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }       
        public double IssuedAmount { get; set; }        
        public string ModeofTransferType { get; set; }        
        public string LoanTypeDescription { get; set; }      
        public string LoanApprovalStatus { get; set; }       
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string BranchName { get; set; }
    }
}