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
    [RoutePrefix("api/treedata")]
    public class TreeDataController : ApiController
    {
        private TreeDataRepository _repo = null;
        public TreeDataController(TreeDataRepository treeDataRepository)
        {
            _repo = treeDataRepository;
        }

        [Authorize]
        [Route("GetTreeData")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<TreeData>>> GetTreeData(int ID)
        {
            Tuple<bool, string, List<TreeData>> result = null;
            try
            {
                result = await _repo.GetTreeData(ID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

    }
}
