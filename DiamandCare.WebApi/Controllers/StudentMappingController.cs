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
    [RoutePrefix("api/studentmapping")]
    public class StudentMappingController : ApiController
    {
        private StudentMappingRepository _repo = null;
        public StudentMappingController(StudentMappingRepository repository)
        {
            _repo = repository;
        }

        [Authorize]
        [Route("UpdateUserOTP")]
        [HttpPost]
        public async Task<Tuple<bool, string, OTPViewModel>> UpdateUserOTP(OTPViewModel oTPViewModel)
        {
            Tuple<bool, string, OTPViewModel> result = null;
            try
            {
                result = await _repo.UpdateUserOTP(oTPViewModel);

            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("InsertStudentMapping")]
        [HttpPost]
        public async Task<Tuple<bool, string>> InsertStudentMapping(StudentMappingModel studentMappingModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.InsertStudentMapping(studentMappingModel);

            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Route("VerifyStudentOTP")]
        [HttpPost]
        public async Task<Tuple<bool, string, OTPViewModel>> VerifyStudentOTP(OTPViewModel oTPViewModel)
        {
            Tuple<bool, string, OTPViewModel> result = null;

            try
            {
                result = await _repo.VerifyStudentOTP(oTPViewModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GenerateLoanOTP")]
        [HttpPost]
        public async Task<Tuple<bool, string>> GenerateLoanOTP(OTPViewModel oTPViewModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.GenerateLoanOTP(oTPViewModel);

            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GetFeeMastersByUserID")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<FeeMastersModel>>> GetFeeMastersByUserID(int UserID)
        {
            Tuple<bool, string, List<FeeMastersModel>> result = null;
            try
            {
                result = await _repo.GetFeeMastersByUserID(UserID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GetStudentMappingDetails")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<StudentMappingViewModel>>> GetStudentDetails(int UserID)
        {
            Tuple<bool, string, List<StudentMappingViewModel>> result = null;
            try
            {
                result = await _repo.GetStudentDetails(UserID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
    }
}
