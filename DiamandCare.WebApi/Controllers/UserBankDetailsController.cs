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
    [RoutePrefix("api/userbankdetails")]
    public class UserBankDetailsController : ApiController
    {
        private UserBankDetailsRepository _repo = null;
        public UserBankDetailsController(UserBankDetailsRepository userBankDetailsRepository)
        {
            _repo = userBankDetailsRepository;
        }

        [Authorize]
        [Route("GetUserBankDetails")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<UserBankDetails>>> GetUserBankDetails(int ID)
        {
            Tuple<bool, string, List<UserBankDetails>> result = null;
            try
            {
                result = await _repo.GetUserBankDetails(ID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("InsertorUpdateUserBankDetails")]
        [HttpPost]
        public async Task<Tuple<bool, string, UserBankDetails>> InsertorUpdateUserBankDetails(UserBankDetails obj)
        {
            Tuple<bool, string, UserBankDetails> result = null;
            try
            {
                result = await _repo.InsertorUpdateUserBankDetails(obj);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
    }
}
