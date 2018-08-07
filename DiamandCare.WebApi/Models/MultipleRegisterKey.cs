using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class MultipleRegisterKey
    {
        public int UserID { get; set; }
        public int NoOfKeys { get; set; }
        public string KeyType { get; set; }
        public bool IsWallet { get; set; }
    }
}