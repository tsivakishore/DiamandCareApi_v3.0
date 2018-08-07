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


        public async Task<Tuple<bool, string, List<TreeData>>> GetTreeData(int ID)
        {
            Tuple<bool, string, List<TreeData>> result = null;
            List<TreeData> lstTreeData = new List<TreeData>();       
            IEnumerable<TreeData> lstNewParentTreeData;
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
                    lstNewParentTreeData = lstTreeData.Where(data => data.UserID == ID);

                    foreach (var treeItem in lstNewParentTreeData)
                    {
                        buildTreeviewMenu(treeItem, lstTreeData);
                    }

                    result = Tuple.Create(true, "", lstNewParentTreeData.ToList());
                }                   
                else
                    result = Tuple.Create(false, "No records found", lstTreeData);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstTreeData);
            }
            return result;
        }

        private void buildTreeviewMenu(TreeData treeItem, IEnumerable<TreeData> lstTreeData)
        {
            IEnumerable<TreeData> _treeItems;

            _treeItems = lstTreeData.Where(item => item.UnderID == treeItem.UserID);

            if (_treeItems != null && _treeItems.Count() > 0)
            {
                List<TreeData> items = new List<TreeData>(); 
                foreach (var item in _treeItems)
                {
                    treeItem.TreeItems.Add(item);
                    buildTreeviewMenu(item, lstTreeData);
                }
            }
        }
    }
}