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
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        private ReportsRepository _repoReports = null;

        public ReportsController(ReportsRepository repoReports)
        {
            _repoReports = repoReports;
        }

        [Authorize]
        [Route("GetReportTypes")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<ReportsTypesModel>>> GetReportTypes(string LoginType)
        {
            Tuple<bool, string, List<ReportsTypesModel>> result = null;
            try
            {
                result = await _repoReports.GetReportTypes(LoginType);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

    }
}