using Dapper;
using DiamandCare.Core;
using DiamandCare.WebApi.Models;
using DiamandCare.WebApi.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DiamandCare.WebApi.Repository
{
    public class TreeDataRepository
    {
        private string _dvDb = Settings.Default.DiamandCareConnection;


        public async Task<Tuple<bool, string, List<OrgTreeData>>> GetTreeData(int ID)
        {
            Tuple<bool, string, List<OrgTreeData>> result = null;
            List<TreeData> lstTreeData = new List<TreeData>();
            List<OrgTreeData> lstOrgTreeData = new List<OrgTreeData>();
            IEnumerable<OrgTreeData> lstNewParentTreeData;           
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@ID", ID, DbType.String);
                    var list = await con.QueryAsync<TreeData>("[dbo].[Select_treedata]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstTreeData = list as List<TreeData>;
                    con.Close();
                }

                if (lstTreeData != null && lstTreeData.Count() > 1)
                {
                    lstNewParentTreeData = lstTreeData.Where(data => data.UserID == ID).Select(x =>
                    new OrgTreeData
                    {
                        name = x.UserName,
                        UserID = x.UserID,
                        //UnderID = x.UnderID,
                        //type = "person",
                        styleClass = setColor(x.Level),
                        //expanded = true,
                        //data = new OrgTreeDetailedData
                        //{
                        //    Parents = x.Parents,
                        //    Level = x.Level,
                        //    FirstName = x.FirstName,
                        //    LastName = x.LastName,
                        //    UserName = x.UserName,
                        //    PhoneNumber = x.PhoneNumber,
                        //    DcID = x.DcID
                        //}
                    });

                    foreach (var treeItem in lstNewParentTreeData)
                    {
                        buildTreeviewMenu(treeItem, lstTreeData);
                        lstOrgTreeData.Add(treeItem);
                    }
                    result = Tuple.Create(true, "", lstOrgTreeData);
                }
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstOrgTreeData);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstOrgTreeData);
            }
            return result;
        }

        private void buildTreeviewMenu(OrgTreeData treeItem, IEnumerable<TreeData> lstTreeData)
        {
            IEnumerable<OrgTreeData> _treeItems;

            _treeItems = lstTreeData.Where(item => item.UnderID == treeItem.UserID).Select(x =>
                    new OrgTreeData
                    {
                        name = x.UserName,
                        UserID = x.UserID,
                        //UnderID = x.UnderID,
                       // type = "person",
                       styleClass = setColor(x.Level),
                       // expanded = x.Level < 4 ? true : false,
                        //data = new OrgTreeDetailedData
                        //{
                        //    Parents = x.Parents,
                        //    Level = x.Level,
                        //    FirstName = x.FirstName,
                        //    LastName = x.LastName,
                        //    UserName = x.UserName,
                        //    PhoneNumber = x.PhoneNumber,
                        //    DcID = x.DcID
                        //}
                    });

            if (_treeItems != null && _treeItems.Count() > 0)
            {
                List<OrgTreeData> items = new List<OrgTreeData>();
                foreach (var item in _treeItems)
                {
                    treeItem.children.Add(item);
                    buildTreeviewMenu(item, lstTreeData);
                }
            }
        }

        private string setColor(int num)
        {
            string strColor = string.Empty;

            if (num == 0)
                strColor = "pink";
            else if (num == 1)
                strColor = "#C45F4A";
            else if (num == 2)
                strColor = "orange";
            else if (num == 3)
                strColor = "#ff3399";
            else if (num == 4)
                strColor = "#C45F4A";
            else
                strColor = "purple";

            return strColor;
        }
    }
}