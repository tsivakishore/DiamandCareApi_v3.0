using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamandCare.WebApi.Models
{
    public class RegisterKey
    {
        public string RegKey { get; set; }
        public string PhoneNumber { get; set; }
        public string RegKeyStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string KeyType { get; set; }
        public int ToUserID { get; set; }
    }
}
