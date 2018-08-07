using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class MultipleSecreateKeys
    {
        public Franchises Franchise { get; set; }
        public Wallet Wallet { get; set; }
        public MasterCharges MasterCharges { get; set; }
    }

    public class Franchises
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }

    public class Wallet
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public decimal Balance { get; set; }
        public decimal HoldAmount { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class MasterCharges
    {
        public decimal DocumentationAdminFee { get; set; }
        public decimal DocumentationAdminFee1 { get; set; }
        public decimal PrepaidLoanCharges { get; set; }
        public decimal RegistrationCharges { get; set; }
    }
}