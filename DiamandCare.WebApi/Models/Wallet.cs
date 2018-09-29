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

    public class WalletTransactionsViewModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int AgainstTypeID { get; set; }
        public string AgainstType { get; set; }
        public string TransactionType { get; set; }
        public int AgainstID { get; set; }
        public string Against { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public decimal TransactionAmount { get; set; }
        public string Purpose { get; set; }
    }

    public class FundRequest
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public int RequestToUserID { get; set; }
        public int RequestStatusID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public decimal TransferCharges { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
    public class FundRequestStatus
    {
        public int RequestStatusID { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }

    public class WithdrawFunds
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public decimal WithdrawAmount { get; set; }
        public int TransferBy { get; set; }
        public DateTime TransferOn { get; set; }
        public int TransferStatusID { get; set; }
        public string Purpose { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
    public class WithdrawFundsViewModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public decimal WithdrawAmount { get; set; }
        public int TransferBy { get; set; }
        public string TransferOn { get; set; }
        public int TransferStatusID { get; set; }
        public string Purpose { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
    }
}