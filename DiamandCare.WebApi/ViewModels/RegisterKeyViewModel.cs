using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class RegisterKeyViewModel
    {
        public string RegKey { get; set; }
        public string PhoneNumber { get; set; }
        public string RegKeyStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string KeyType { get; set; }
        public int ToUserID { get; set; }
        public string SharedTo { get; set; }
        public decimal KeyCost { get; set; }
        public DateTime SharedOn { get; set; }
    }
}