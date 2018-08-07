using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DiamandCare.WebApi.Repository;
using DiamandCare.WebApi;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using DiamandCare.Core;

namespace DiamandCare.WebApi.Controllers
{
    
    [RoutePrefix("api/role")]
    public class RoleController : ApiController
    {
        private AuthRepository _repo = null;

        public RoleController(AuthRepository autRepisitory)
        {
            _repo = autRepisitory;
        }

        [Authorize]
        [Route("newrole")]
        [HttpPost]
        public async Task<Tuple<bool, string, IdentityResult>> RegisterRole(RoleViewModel roleViewModel)
        {
            Tuple<bool, string, IdentityResult> result = null;
            try
            {
                result = await _repo.RegisterRole(roleViewModel);
            }
            catch (Exception ex)
            {
               ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("updaterole")] //UpdateRole
        [HttpPost]
        public async Task<Tuple<bool, string>> UpdateRole(RoleViewModel updateModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.UpdateRole(updateModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("getuserrolebyid")]//GetUsersRoleByID
        [HttpGet]
        public Tuple<bool, string, User> GetUserRoleByID()
        {
            Tuple<bool, string, User> result = null;
            try
            {
                result = _repo.GetUserRoleByID();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }
    }
}