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
    [RoutePrefix("api/franchisedetails")]
    public class FranchiseController : ApiController
    {
        private FranchiseRepository _repo = null;
        public FranchiseController(FranchiseRepository repository)
        {
            _repo = repository;
        }

        [Authorize]
        [Route("GetFranchiseMasterDetails")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<FranchiseMaster>>> GetFranchiseMasterDetails()
        {
            Tuple<bool, string, List<FranchiseMaster>> result = null;
            try
            {
                result = await _repo.GetFranchiseMasterDetails();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("UpdateFranchise")]
        [HttpPost]
        public async Task<Tuple<bool, string, FranchiseMaster>> UpdateFranchise(FranchiseMaster obj)
        {
            Tuple<bool, string, FranchiseMaster> result = null;
            try
            {
                result = await _repo.UpdateFranchise(obj);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GetUpgradeTo")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<UpgradeTo>>> GetUpgradeTo()
        {
            Tuple<bool, string, List<UpgradeTo>> result = null;
            try
            {
                result = await _repo.GetUpgradeTo();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GetUsernameByDCIDorName")]
        [HttpGet]
        public async Task<Tuple<bool, string, UserIDNameModel>> GetUsernameByDCIDorName(string DcIDorName)
        {
            Tuple<bool, string, UserIDNameModel> result = null;
            try
            {
                result = await _repo.GetUsernameByDCIDorName(DcIDorName);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GetUnderFranchiseDetails")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<UserIDNameModel>,FranchiseMaster>> GetUnderFranchiseDetails(int FranchiseTypeID)
        {
            Tuple<bool, string, List<UserIDNameModel>, FranchiseMaster> result = null;
            try
            {
                result = await _repo.GetUnderFranchiseDetails(FranchiseTypeID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("InsertorUpdateFranchiseDetails")]
        [HttpPost]
        public async Task<Tuple<bool, string, Franchise>> InsertorUpdateFranchiseDetails(Franchise obj)
        {
            Tuple<bool, string, Franchise> result = null;
            try
            {
                result = await _repo.InsertorUpdateFranchiseDetails(obj);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GetFranchiseTypes")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<FranchiseTypes>>> GetFranchiseTypes()
        {
            Tuple<bool, string, List<FranchiseTypes>> result = null;
            try
            {
                result = await _repo.GetFranchiseTypes();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
        [Authorize]
        [Route("GetFranchiseUsernameWalletByIDorName")]
        [HttpGet]
        public async Task<Tuple<bool, string, Franchises, Wallet>> GetFranchiseUsernameWalletByIDorName(string DcIDorName)
        {
            Tuple<bool, string, Franchises, Wallet> result = null;
            try
            {
                result = await _repo.GetFranchiseUsernameWalletByIDorName(DcIDorName);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
    }
}
