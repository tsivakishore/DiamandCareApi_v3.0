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
    [RoutePrefix("api/displayscreens")]
    public class MenuController : ApiController
    {
        private MenuRepository _repo = null;
        public MenuController(MenuRepository repository)
        {
            _repo = repository;
        }

        [Authorize]
        [Route("GetScreenMasterDetails")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<MenuModel>>> GetScreenMasterDetails()
        {
            Tuple<bool, string, List<MenuModel>> result = null;
            try
            {
                result = await _repo.GetScreenMasterDetails();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
    }
}
