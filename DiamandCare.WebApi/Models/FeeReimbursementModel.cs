using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class FeeReimbursementModel : LoansModel
    {
        public string KYCDocumentName { get; set; }
        public byte[] KYCDocumentContent { get; set; }
        public string BonafideFileName { get; set; }
        public byte[] BonafideContent { get; set; }
        public string FeeReceiptFileName { get; set; }
        public byte[] FeeReceiptContent { get; set; }
        public string FeeReimbursementOtherFile { get; set; }
        public byte[] FeeReimbursementOtherContent { get; set; }
    }
}