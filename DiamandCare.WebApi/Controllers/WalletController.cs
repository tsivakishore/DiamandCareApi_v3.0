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
    }
}
