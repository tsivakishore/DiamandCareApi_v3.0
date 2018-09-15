using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamandCare.WebApi.Models
{
    public class SchoolModel
    {
        public int UserID { get; set; }
        public int UpgradeTo { get; set; }
        public string SchoolName { get; set; }
        public string BranchCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public int StateID { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
    public class SchoolViewModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int UpgradeTo { get; set; }
        public string SchoolName { get; set; }
        public string BranchCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public int StateID { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
