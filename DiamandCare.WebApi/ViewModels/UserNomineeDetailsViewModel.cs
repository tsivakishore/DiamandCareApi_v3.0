using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class UserNomineeDetailsViewModel
    {
        public int NomineeID { get; set; }
        public int UserID { get; set; }
        public string NomineeName { get; set; }
        public int NomineeRelationshipID { get; set; }
        public string NomineeAddress { get; set; }
        public string OtherRelationship { get; set; }
        public string PhoneNumber { get; set; }
        public int CreatedBy { get; set; }
    }
}