using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamandCare.WebApi.ViewModels
{
    public class MenuViewModel
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuTitle { get; set; }
        public string Icon_Thumbnail { get; set; }
        public string Path { get; set; }
        public int? ParentMenuID { get; set; }
        public int MenuTypeID { get; set; }
        public string CssClass { get; set; }
    }
}
