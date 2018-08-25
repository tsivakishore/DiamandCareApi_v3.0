using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi.Models
{
    public class Wallet
    {
        public int UserID { get; set; }
        public decimal Balance { get; set; }
        public decimal HoldAmount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
    public class UpdateWallet
    {
        public int UserID { get; set; }
        public decimal AddBalance { get; set; }       
        public int CreatedBy { get; set; }       
    }
    public class WalletTransactions
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int AgainstTypeID { get; set; }
        public string TransactionType { get; set; }
        public int AgainstID { get; set; }        
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public decimal TransactionAmount { get; set; }
        public string Purpose { get; set; }
    }
}