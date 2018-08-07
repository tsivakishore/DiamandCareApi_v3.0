using DiamandCare.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamandCare.WebApi.ViewModels
{
    public class SponserViewModel
    {
        public SponserModel SponserDetails { get; set; }
        public UnderModel UnderDetails { get; set; }
        public List<SponserModel> lstSponserDetails { get; set; }
        public List<UnderModel> lstUnderDetails { get; set; }
    }
}
