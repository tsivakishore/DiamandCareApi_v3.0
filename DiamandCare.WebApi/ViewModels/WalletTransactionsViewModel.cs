using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class WalletTransactionsViewModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int AgainstTypeID { get; set; }
        public string AgainstType { get; set; }
        public string TransactionType { get; set; }
        public int AgainstID { get; set; }
        public string Against { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public decimal TransactionAmount { get; set; }
        public string Purpose { get; set; }
    }
}