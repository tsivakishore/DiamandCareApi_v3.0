using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi.Models
{
    public class MenuModel
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuDescription { get; set; }
    }
    public class RoleMenuModel
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string RoleID { get; set; }
        public string RoleName { get; set; }
    }
}