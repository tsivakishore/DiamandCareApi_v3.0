﻿using DiamandCare.Core;
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
        public async Task<Tuple<bool, string>> UpdateUserOTP(OTPViewModel oTPViewModel)
        {
            Tuple<bool, string> result = null;
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
    }
}
