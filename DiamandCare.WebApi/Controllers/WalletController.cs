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
        public async Task<Tuple<bool, string, List<WalletTransactions>>> GetWalletRecentExpenses()
        {
            Tuple<bool, string, List<WalletTransactions>> result = null;
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
    }
}
