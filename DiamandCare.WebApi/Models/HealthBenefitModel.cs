using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class HealthBenefitModel : LoansModel
    {
        public string KYCDocumentName { get; set; }
        public byte[] KYCDocumentContent { get; set; }
        public string HospitalAdmissionFormName { get; set; }
        public byte[] HospitalAdmissionFormContent { get; set; }
        public string EstimatedHospitalChargesDocName { get; set; }
        public byte[] EstimatedHospitalChargesDocContent { get; set; }
        public string EstimatedHospitalOtherFile { get; set; }
        public byte[] EstimatedHospitalOtherContent { get; set; }
    }
}