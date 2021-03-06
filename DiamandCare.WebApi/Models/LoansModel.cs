﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class LoansModel
    {
        public int LoanID { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public double LoanAmount { get; set; }
        public double IssuedAmount { get; set; }
        public double AmountToPay { get; set; }
        public double AdminCharges { get; set; }
        public int ModeofTransfer { get; set; }
        public int LoanStatusID { get; set; }
        public string LoanTypeCode { get; set; }
        public bool PrePaidLoan { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int IsApproved { get; set; }
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
    }

    
}