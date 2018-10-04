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

        [Authorize]
        [Route("CreateScreenMaster")]
        [HttpPost]
        public async Task<Tuple<bool, string>> CreateScreenMaster(MenuModel obj)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.CreateScreenMaster(obj);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("CreateRoleMenu")]
        [HttpPost]
        public async Task<Tuple<bool, string>> CreateRoleMenu(RoleMenuModel obj)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.CreateRoleMenu(obj);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("DeleteRoleMenu")]
        [HttpGet]
        public async Task<Tuple<bool, string>> DeleteRoleMenu(int ID)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.DeleteRoleMenu(ID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GetRoleMenuDetailsByScreenID")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<RoleMenuModel>>> GetRoleMenuDetailsByScreenID(int screenID)
        {
            Tuple<bool, string, List<RoleMenuModel>> result = null;
            try
            {
                result = await _repo.GetRoleMenuDetailsByScreenID(screenID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
    }
}
