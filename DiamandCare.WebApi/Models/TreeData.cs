using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi.Models
{
    public class TreeData
    {
        public int UserID { get; set; }
        public int UnderID { get; set; }
        public string Parents { get; set; }
        public int Level { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string DcID { get; set; }
        public List<TreeData> TreeItems { get; set; }

        public TreeData()
        {
            TreeItems = new List<TreeData>();
        }

    }
    //public class TreeItem
    //{
    //    public int ChildID { get; set; }
    //    public int UnderID { get; set; }
    //    public string Parents { get; set; }
    //    public int Level { get; set; }
    //    public TreeItem TreeItems { get; set; }
    //}
    public class OrgTreeData
    {
        public int UserID { get; set; }
       
        public string name { get; set; }
       
        public string styleClass { get; set; }
        public List<OrgTreeData> children { get; set; }
        public int Level { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string DcID { get; set; }
        public OrgTreeData()
        {
            children = new List<OrgTreeData>();
        }
    }
}