using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DiamandCare.WebApi.Models;
using DiamandCare.WebApi.Repository;
using DiamandCare.WebApi.ViewModels;
using DiamandCare.Core;

namespace DiamandCare.WebApi.Controllers
{
    //[Authorize(Policy = "ApiUser")]
    [RoutePrefix("api/Shared")]
    public class SharedController : ApiController
    {
        private SharedRepository _srepo = null;

        public SharedController(SharedRepository srepo)
        {
            _srepo = srepo;
        }

        [Route("GetState")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<State>>> GetState()
        {
            Tuple<bool, string, List<State>> result = null;

            try
            {
                result = await _srepo.GetState();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Route("GetSchool")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<SchoolModel>>> GetSchool()
        {
            Tuple<bool, string, List<SchoolModel>> result = null;

            try
            {
                result = await _srepo.GetSchool();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Route("sourceofuser")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<SourceOfUserModel>>> GetSourceOfUser()
        {
            Tuple<bool, string, List<SourceOfUserModel>> result = null;

            try
            {
                result = await _srepo.GetSourceOfUser();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Route("accounttypes")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<AccountTypesViewModel>>> GetAccountTypes()
        {
            Tuple<bool, string, List<AccountTypesViewModel>> result = null;

            try
            {
                result = await _srepo.GetAccountTypes();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("nomineerelations")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<NomineeRelationshipViewModel>>> GetNomineeRelations()
        {
            Tuple<bool, string, List<NomineeRelationshipViewModel>> result = null;

            try
            {
                result = await _srepo.GetNomineeRelations();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("banks")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<BanksModel>>> GetBanks()
        {
            Tuple<bool, string, List<BanksModel>> result = null;

            try
            {
                result = await _srepo.GetBanks();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Route("ResendSMS")]
        [HttpPost]
        public async Task<Tuple<bool, string>> ReSendSMS(RegisterKey resKey)
        {
            Tuple<bool, string> result = null;

            try
            {
                result = await _srepo.SendSMS(resKey.PhoneNumber, resKey.RegKey);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Route("VerifyKey")]
        [HttpPost]
        public async Task<Tuple<bool, string, string>> VerifySecretKey(RegisterKey resKey)
        {
            Tuple<bool, string, string> result = null;

            try
            {
                result = await _srepo.VerifySecretKey(resKey);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Route("GetSponserDetails")]
        [HttpPost]
        public async Task<Tuple<bool, string, SponserViewModel>> GetSponserDetails(CommonModel objCommon)
        {
            Tuple<bool, string, SponserViewModel> result = null;

            try
            {
                result = await _srepo.GetSponserDetails(objCommon);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GetModeofTransfer")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<ModeofTransfer>>> GetModeofTransfer()
        {
            Tuple<bool, string, List<ModeofTransfer>> result = null;
            try
            {
                result = await _srepo.GetModeofTransfer();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }
    }
}