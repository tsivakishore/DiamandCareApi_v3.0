using DiamandCare.WebApi.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using DiamandCare.Core;

namespace DiamandCare.WebApi.Controllers
{

    [RoutePrefix("api/accounts")]
    public class AccountsController : ApiController
    {
        private AuthRepository _repo = null;

        public AccountsController(AuthRepository autRepisitory)
        {
            _repo = autRepisitory;
        }
       
        [Route("registeruser")]
        [HttpPost]
        public async Task<Tuple<bool, string>> RegisterUser(User model)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.RegisterUser(model);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
    }
}
