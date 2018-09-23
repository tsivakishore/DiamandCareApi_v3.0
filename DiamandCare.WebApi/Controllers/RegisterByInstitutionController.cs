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
    [RoutePrefix("api/userbyinstitution")]
    public class RegisterByInstitutionController : ApiController
    {
        private RegisterByInstitutionRepository _repo = null;

        public RegisterByInstitutionController(RegisterByInstitutionRepository repo)
        {
            _repo = repo;
        }

        [Authorize]
        [Route("registeruserbyinstitution")]
        [HttpPost]
        public async Task<Tuple<bool, string>> RegisterUserByInstitution(User model)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.RegisterUserByInstitution(model);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("getusersbyuserid")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<UsersByInstitutionViewModel>>> GetUsersByInstitution(int UserID)
        {
            Tuple<bool, string, List<UsersByInstitutionViewModel>> result = null;
            try
            {
                result = await _repo.GetUsersByInstitution(UserID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }
    }
}
