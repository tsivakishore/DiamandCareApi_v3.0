using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DiamandCare.WebApi.Models;
using DiamandCare.WebApi.Repository;

namespace DiamandCare.WebApi.Controllers
{
    [RoutePrefix("api/LoanEarns")]
    public class LoanEarnsController : ApiController
    {
        private LoanEarnsRepository _repo = null;

        public LoanEarnsController(LoanEarnsRepository repisitory)
        {
            _repo = repisitory;
        }

        [Route("GetLoanEarns")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoanEarns>>> GetLoanEarns()
        {
            Tuple<bool, string, List<LoanEarns>> result = result = await _repo.GetLoanEarns();
            return result;
        }
    }
}