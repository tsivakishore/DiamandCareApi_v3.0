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
    [RoutePrefix("api/schooldetails")]
    public class SchoolController : ApiController
    {
        private SchoolRepository _repo = null;
        public SchoolController(SchoolRepository repository)
        {
            _repo = repository;
        }

        [Authorize]
        [Route("GetSchoolDetails")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<SchoolViewModel>>> GetSchoolDetails()
        {
            Tuple<bool, string, List<SchoolViewModel>> result = null;
            try
            {
                result = await _repo.GetSchoolDetails();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("InsertSchoolDetails")]
        [HttpPost]
        public async Task<Tuple<bool, string, SchoolModel>> InsertSchoolDetails(SchoolModel obj)
        {
            Tuple<bool, string, SchoolModel> result = null;
            try
            {
                result = await _repo.InsertSchoolDetails(obj);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
        [Authorize]
        [Route("UpdateSchoolDetails")]
        [HttpPost]
        public async Task<Tuple<bool, string, SchoolModel>> UpdateSchoolDetails(SchoolModel obj)
        {
            Tuple<bool, string, SchoolModel> result = null;
            try
            {
                result = await _repo.UpdateSchoolDetails(obj);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
    }
}
