﻿using Dapper;
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

        public async Task<Tuple<bool, string, List<RoleMenuModel>>> GetRoleMenuDetailsByScreenID(int screenID)
        {
            Tuple<bool, string, List<RoleMenuModel>> result = null;
            List<RoleMenuModel> lstMenuRoles = new List<RoleMenuModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@ScreenID", screenID, DbType.Int32);
                    var list = await con.QueryAsync<RoleMenuModel>("[dbo].[Select_RoleMenuDetailsByScreenID]", parameters, commandType: CommandType.StoredProcedure);
                    lstMenuRoles = list as List<RoleMenuModel>;
                }

                if (lstMenuRoles != null && lstMenuRoles.Count() > 0)
                {
                    result = Tuple.Create(true, "", lstMenuRoles);
                }
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstMenuRoles);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstMenuRoles);
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
        public async Task<Tuple<bool, string>> CreateRoleMenu(RoleMenuModel obj)
        {
            Tuple<bool, string> result = null;
            int insertStatus = -1;
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@MenuID", obj.MenuID, DbType.Int32);
                    parameters.Add("@RoleID", obj.RoleID, DbType.String);

                    insertStatus = await cxn.ExecuteScalarAsync<int>("dbo.Insert_RoleMenu", parameters, commandType: CommandType.StoredProcedure);
                }

                if (insertStatus == 0)
                {
                    result = Tuple.Create(true, "Screen and Role mapped successfully");
                }
                else
                    result = Tuple.Create(false, "Screen and Role mapping failed");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message);
            }

            return result;
        }
        public async Task<Tuple<bool, string>> DeleteRoleMenu(int ID)
        {
            Tuple<bool, string> result = null;
            int insertStatus = -1;
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@ID", ID, DbType.Int32);                    

                    insertStatus = await cxn.ExecuteScalarAsync<int>("dbo.Delete_RoleMenu", parameters, commandType: CommandType.StoredProcedure);
                }

                if (insertStatus == 0)
                {
                    result = Tuple.Create(true, "Screen and Role map deleted successfully");
                }
                else
                    result = Tuple.Create(false, "Screen and Role map deletion failed");
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