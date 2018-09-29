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
    public class MenuRepository
    {
        private string _dcDb = Settings.Default.DiamandCareConnection;
        int UserID;

        public MenuRepository()
        {
            UserID = Helper.FindUserByID().UserID;
        }

        public async Task<Tuple<bool, string, List<MenuModel>>> GetScreenMasterDetails()
        {
            Tuple<bool, string, List<MenuModel>> result = null;
            List<MenuModel> lstMenu = new List<MenuModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<MenuModel>("[dbo].[Select_MenuDetials]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstMenu = list as List<MenuModel>;
                    con.Close();
                }

                if (lstMenu != null && lstMenu.Count() > 0)
                {
                    result = Tuple.Create(true, "", lstMenu);
                }
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstMenu);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstMenu);
            }
            return result;
        }

        public async Task<Tuple<bool, string>> CreateScreenMaster(MenuModel obj)
        {
            Tuple<bool, string> result = null;
            int insertStatus = -1;
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    if (obj.MenuID > 0)
                        parameters.Add("@MenuID", obj.MenuID, DbType.Int32);

                    parameters.Add("@MenuName", obj.MenuName, DbType.String);
                    parameters.Add("@MenuDescription", obj.MenuDescription, DbType.String);                  
                    cxn.Open();
                    insertStatus = await cxn.ExecuteScalarAsync<int>("dbo.Insert_Menu", parameters, commandType: CommandType.StoredProcedure);
                    cxn.Close();
                }

                if (insertStatus == 0)
                {
                    if (obj.MenuID == 0)
                        result = Tuple.Create(true, "Screen details created successfully");
                    else if (obj.MenuID > 0)
                        result = Tuple.Create(true, "Screen details updated successfully");
                }
                else
                    result = Tuple.Create(false, "Screen details created failed");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message);
            }

            return result;
        }
    }
}