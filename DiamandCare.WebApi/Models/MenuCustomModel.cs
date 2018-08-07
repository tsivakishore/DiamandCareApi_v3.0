using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamandCare.WebApi.Models
{
    public class MenuCustomModel
    {
        public int? MenuID { get; set; }
        public int? RoleID { get; set; }
        public string Path { get; set; }
    }
}
