using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamandCare.WebApi
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }        
    }
    public class UserIDNameModel
    {        
        public int UserID { get; set; }
        public string UserName { get; set; }       
    }
}
