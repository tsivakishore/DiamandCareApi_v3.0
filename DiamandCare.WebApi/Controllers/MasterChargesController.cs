using DiamandCare.WebApi.Models;
using DiamandCare.WebApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DiamandCare.Core;

namespace DiamandCare.WebApi.Controllers
{
   
    [RoutePrefix("api/MasterCharges")]
    public class MasterChargesController : ApiController
    {
        private MasterChargesRepository _repo = null;

        public MasterChargesController(MasterChargesRepository repo)
        {
            _repo = repo;
        }

        [Authorize]
        [Route("AddMasterCharges")]
        [HttpPost]
        public async Task<Tuple<bool, string, MasterChargesModel>> AddMasterCharges(MasterChargesModel obj)
        {
            Tuple<bool, string, MasterChargesModel> result = null;
            try
            {
                result = await _repo.AddMasterCharges(obj);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GetMasterCharges")]
        [HttpGet]
        public async Task<Tuple<bool, string, MasterChargesModel>> GetMasterCharges()
        {
            Tuple<bool, string, MasterChargesModel> result = null;
            try
            {
                result = await _repo.GetMasterCharges();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

    }
}