using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi.Models
{
    public class RiskBenefitModel : LoansModel
    {
        public string KYCDocumentName { get; set; }
        public byte[] KYCDocumentContent { get; set; }
        public string DeathCertificateFileName { get; set; }
        public byte[] DeathCertificateContent { get; set; }
        public string RiskBenefitOtherFile { get; set; }
        public byte[] RiskBenefitOtherContent { get; set; }
    }
}