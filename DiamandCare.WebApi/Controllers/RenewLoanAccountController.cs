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
    [RoutePrefix("api/renewloanaccount")]
    public class RenewLoanAccountController : ApiController
    {
        private RenewLoanAccountRepository _repo = null;

        public RenewLoanAccountController(RenewLoanAccountRepository repo)
        {
            _repo = repo;
        }

        [Authorize]
        [Route("RenewLoanAccount")]
        [HttpPost]
        public async Task<Tuple<bool, string>> RenewLoanAccountForUser(User model)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.RenewLoanAccountForUser(model);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
    }
}
