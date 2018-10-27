using DiamandCare.Core;
using DiamandCare.WebApi.Models;
using DiamandCare.WebApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DiamandCare.WebApi.Controllers
{
    [RoutePrefix("api/wallet")]
    public class WalletController : ApiController
    {
        private WalletRepository _repo = null;
        public WalletController(WalletRepository repo)
        {
            _repo = repo;
        }

        [Authorize]
        [Route("getwallet")]
        [HttpGet]
        public async Task<Tuple<bool, string, Wallet>> GetWallet()
        {
            Tuple<bool, string, Wallet> result = null;
            try
            {
                result = await _repo.GetWallet();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
        [Authorize]
        [Route("GetWalletRecentExpenses")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<WalletTransactionsViewModel>>> GetWalletRecentExpenses()
        {
            Tuple<bool, string, List<WalletTransactionsViewModel>> result = null;
            try
            {
                result = await _repo.GetWalletRecentExpenses();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("InsertWalletExpenses")]
        [HttpPost]
        public async Task<Tuple<bool, string>> InsertWalletExpenses(WalletTransactions obj)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.InsertWalletExpenses(obj);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetWalletTransactions")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<WalletTransactionsViewModel>>> GetWalletTransactions()
        {
            Tuple<bool, string, List<WalletTransactionsViewModel>> result = null;
            try
            {
                result = await _repo.GetWalletTransactions();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }
        [Authorize]
        [Route("UpdateWithdrawFunds")]
        [HttpPost]
        public async Task<Tuple<bool, string>> UpdateWithdrawFunds(WithdrawFundsViewModel withdrawFundsViewModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.UpdateWithdrawFunds(withdrawFundsViewModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetWithdrawalTransactions")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<WithdrawFundsViewModel>>> GetWithdrawalTransactions()
        {
            Tuple<bool, string, List<WithdrawFundsViewModel>> result = null;
            try
            {
                result = await _repo.GetWithdrawalTransactions();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetPendingWithdrawalTransactions")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<WithdrawFundsViewModel>>> GetPendingWithdrawalTransactions()
        {
            Tuple<bool, string, List<WithdrawFundsViewModel>> result = null;
            try
            {
                result = await _repo.GetPendingWithdrawalTransactions();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetRejectedWithdrawalTransactions")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<WithdrawFundsViewModel>>> GetRejectedWithdrawalTransactions()
        {
            Tuple<bool, string, List<WithdrawFundsViewModel>> result = null;
            try
            {
                result = await _repo.GetRejectedWithdrawalTransactions();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetApprovedWithdrawalTransactions")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<WithdrawFundsViewModel>>> GetApprovedWithdrawalTransactions()
        {
            Tuple<bool, string, List<WithdrawFundsViewModel>> result = null;
            try
            {
                result = await _repo.GetApprovedWithdrawalTransactions();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetFundRequest")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<FundRequestViewModel>>> GetFundRequest()
        {
            Tuple<bool, string, List<FundRequestViewModel>> result = null;
            try
            {
                result = await _repo.GetFundRequest();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }
        [Authorize]
        [Route("GetFundRequestStatus")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<FundRequestStatus>>> GetFundRequestStatus()
        {
            Tuple<bool, string, List<FundRequestStatus>> result = null;
            try
            {
                result = await _repo.GetFundRequestStatus();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("RequestFunds")]
        [HttpPost]
        public async Task<Tuple<bool, string>> RequestFunds(FundRequest fundRequestModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.RequestFunds(fundRequestModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetUserFundRequestDetails")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<FundRequestViewModel>>> GetUserFundRequestDetails()
        {
            Tuple<bool, string, List<FundRequestViewModel>> result = null;
            try
            {
                result = await _repo.GetUserFundRequestDetails();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("ApproveFundsRequest")]
        [HttpPost]
        public async Task<Tuple<bool, string>> ApproveFundsRequest(FundRequest fundRequestModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.ApproveFundsRequest(fundRequestModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("UpdateFundsTransfer")]
        [HttpPost]
        public async Task<Tuple<bool, string>> UpdateFundsTransfer(FundRequest fundRequestModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.UpdateFundsTransfer(fundRequestModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("WithdrawFunds")]
        [HttpPost]
        public async Task<Tuple<bool, string>> WithdrawFunds(WithdrawFunds withdrawModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.InsertWithdrawals(withdrawModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("DeleteWalletExpenses")]
        [HttpPost]
        public async Task<Tuple<bool, string>> DeleteWalletExpenses(WalletTransactions walletTransactions)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.DeleteWalletExpenses(walletTransactions);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }
    }
}
