using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class OTPViewModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int OneTimePassword { get; set; }
        public string Email { get; set; }
        public int LoanOTP { get; set; }
        public bool HaveOTP { get; set; }
    }
}