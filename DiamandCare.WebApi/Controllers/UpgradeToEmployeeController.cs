using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DiamandCare.Core;
using DiamandCare.WebApi.Models;
using DiamandCare.WebApi.Repository;
using System.Threading.Tasks;
using System.Configuration;

namespace DiamandCare.WebApi
{

    [RoutePrefix("api/UpgradeToEmployee")]
    public class UpgradeToEmployeeController : ApiController
    {
        private UpgradeToEmployeeRepository _repo = null;
        public UpgradeToEmployeeController(UpgradeToEmployeeRepository upgradeToEmployeeRepository)
        {
            _repo = upgradeToEmployeeRepository;
        }

        [Authorize]
        [Route("GetUnderEmployees")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<UpgradeEmployeeModel>>> GetUnderEmployees(int designationID)
        {
            Tuple<bool, string, List<UpgradeEmployeeModel>> result = null;
            try
            {
                result = await _repo.GetUnderEmployees(designationID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("InsertOrUpdateUpgradeEmployee")]
        [HttpPost]
        public async Task<Tuple<bool, string>> InsertOrUpdateUpgradeEmployee(UpgradeEmployeeModel upgradeEmployeeModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.InsertOrUpdateUpgradeEmployee(upgradeEmployeeModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GetUpgradeEmployees")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<UpgradeEmployeeModel>>> GetUpgradeEmployees()
        {
            Tuple<bool, string, List<UpgradeEmployeeModel>> result = null;
            try
            {
                result = await _repo.GetUpgradeEmployees();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
    }
}
