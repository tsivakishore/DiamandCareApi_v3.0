using DiamandCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DiamandCare.WebApi
{
    [RoutePrefix("api/feemaster")]
    public class FeeMasterController : ApiController
    {
        private FeeMasterRepository _repo = null;
        public FeeMasterController(FeeMasterRepository repository)
        {
            _repo = repository;
        }

        [Authorize]
        [Route("GetFeeMasterDetails")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<FeeMasterViewModel>>> GetFeeMasterDetails(int UserID)
        {
            Tuple<bool, string, List<FeeMasterViewModel>> result = null;
            try
            {
                result = await _repo.GetFeeMasterDetails(UserID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("CreateFeeMaster")]
        [HttpPost]
        public async Task<Tuple<bool, string>> CreateFeeMaster(FeeMasterModel feeMasterModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.CreateFeeMaster(feeMasterModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
    }
}
