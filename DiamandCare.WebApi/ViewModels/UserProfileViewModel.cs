using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string DcID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FatherName { get; set; }
        public string AadharNumber { get; set; }

    }
}