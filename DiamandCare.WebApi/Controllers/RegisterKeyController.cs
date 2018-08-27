using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DiamandCare.WebApi.Models;
using DiamandCare.WebApi.Repository;
using DiamandCare.Core;

namespace DiamandCare.WebApi.Controllers
{
    
    [RoutePrefix("api/RegisterKey")]
    public class RegisterKeyController : ApiController
    {
        private RegisterKeyRepository _repo = null;
        private SharedRepository _srepo = null;

        public RegisterKeyController(RegisterKeyRepository repo, SharedRepository srepo)
        {
            _repo = repo;
            _srepo = srepo;
        }

        [Authorize]
        [Route("RegisterKeyGenearation")]
        [HttpPost]
        public async Task<Tuple<bool, string, RegisterKey>> RegisterKeyGenearation(RegisterKey obj)
        {
            Tuple<bool, string, RegisterKey> result = null;
            try
            {
                result = await _repo.RegisterKeyGenearation(obj);
                if (result.Item1)
                    await _srepo.SendSMS(result.Item3.PhoneNumber, result.Item3.RegKey);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GenerateMultipleRegisterKeys")]
        [HttpPost]
        public async Task<Tuple<bool, string>> GenerateMultipleRegisterKeys(MultipleRegisterKey obj)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.GenerateMultipleRegisterKeys(obj);
              
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GetIssuedRegisterKeys")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<RegisterKey>>> GetIssuedRegisterKeys()
        {
            Tuple<bool, string, List<RegisterKey>> result = null;
            try
            {
                result = await _repo.GetIssuedRegisterKeys();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetIssuedRegisterKeysByUserID")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<RegisterKey>>> GetIssuedRegisterKeysByUserID()
        {
            Tuple<bool, string, List<RegisterKey>> result = null;
            try
            {
                result = await _repo.GetIssuedRegisterKeysByUserID();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetUsedRegisterKeys")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<RegisterKey>>> GetUsedRegisterKeys()
        {
            Tuple<bool, string, List<RegisterKey>> result = null;
            try
            {
                result = await _repo.GetUsedRegisterKeys();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetUsernameWalletMasterChargesByDCIDorName")]
        [HttpGet]
        public async Task<Tuple<bool, string, MultipleSecreateKeys>> GetUsernameWalletMasterCharges(string DcIDorName)
        {
            Tuple<bool, string, MultipleSecreateKeys> result = null;
            try
            {
                result = await _repo.GetUsernameWalletMasterCharges(DcIDorName);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }
    }
}