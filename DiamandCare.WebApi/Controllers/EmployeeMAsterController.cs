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
    [RoutePrefix("api/Employeedetails")]
    public class EmployeeMasterController : ApiController
    {
        private EmployeeMasterRepository _repo = null;
        public EmployeeMasterController(EmployeeMasterRepository repository)
        {
            _repo = repository;
        }

        [Authorize]
        [Route("GetEmployeeMasterDetails")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<EmployeeMasterModel>>> GetEmployeeMasterDetails()
        {
            Tuple<bool, string, List<EmployeeMasterModel>> result = null;
            try
            {
                result = await _repo.GetEmployeeMasterDetails();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("UpdateEmployee")]
        [HttpPost]
        public async Task<Tuple<bool, string>> UpdateEmployee(EmployeeMasterModel obj)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.UpdateEmployee(obj);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

    }
}
