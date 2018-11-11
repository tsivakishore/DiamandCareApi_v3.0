using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using DiamandCare.Core;
using DiamandCare.WebApi.Models;
using DiamandCare.WebApi.Properties;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace DiamandCare.WebApi.Repository
{
    public class UpgradeToEmployeeRepository
    {
        private string _dvDb = Settings.Default.DiamandCareConnection;
        public static int UserID;

        public UpgradeToEmployeeRepository()
        {
            UserID = Helper.FindUserByID().UserID;
        }

        public async Task<Tuple<bool, string, List<UpgradeEmployeeModel>>> GetUnderEmployees(int designationID)
        {
            Tuple<bool, string, List<UpgradeEmployeeModel>> result = null;
            List<UpgradeEmployeeModel> lstUnderEmployees = new List<UpgradeEmployeeModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    parameters.Add("@DesignationID", designationID, DbType.String);
                    con.Open();
                    var list = await con.QueryAsync<UpgradeEmployeeModel>("[dbo].[Select_UnderEmployeesByDesignation]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstUnderEmployees = list as List<UpgradeEmployeeModel>;
                    con.Close();
                }

                if (lstUnderEmployees != null && lstUnderEmployees.Count() > 0)
                    result = Tuple.Create(true, "", lstUnderEmployees);
                else
                    result = Tuple.Create(false, AppConstants.NO_UNDER_EMPLOYEE, lstUnderEmployees);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstUnderEmployees);
            }
            return result;
        }

        public async Task<Tuple<bool, string>> InsertOrUpdateUpgradeEmployee(UpgradeEmployeeModel upgradeEmployeeModel)
        {
            Tuple<bool, string> result = null;
            int status = -1;
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    parameters.Add("@UserID", upgradeEmployeeModel.UserID);
                    parameters.Add("@DesignationID", upgradeEmployeeModel.DesignationID);
                    if (upgradeEmployeeModel.UnderEmployeeID > 0)
                        parameters.Add("@UnderEmployeeID", upgradeEmployeeModel.UnderEmployeeID);
                    parameters.Add("@RegIncentive", upgradeEmployeeModel.RegIncentive);
                    parameters.Add("@LoanPayIncentive", upgradeEmployeeModel.LoanPayIncentive);
                    parameters.Add("@RecruitmentReq", upgradeEmployeeModel.RecruitmentReq);
                    parameters.Add("@ConditionsApplySelf", upgradeEmployeeModel.ConditionsApplySelf);
                    parameters.Add("@ConditionsApplyGroup", upgradeEmployeeModel.ConditionsApplyGroup);
                    parameters.Add("@TargetJoineesPerMonth", upgradeEmployeeModel.TargetJoineesPerMonth);
                    parameters.Add("@Salary", upgradeEmployeeModel.Salary);
                    parameters.Add("@Description", upgradeEmployeeModel.Description);
                    parameters.Add("@CreatedBy", UserID);
                    con.Open();
                    status = await con.ExecuteScalarAsync<int>("[dbo].[InsertOrUpdate_UpgradeUnderEmployee]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    con.Close();
                }

                if (status >= 0)
                    result = Tuple.Create(true, "Upgrade Employee inserted successfully");
                else
                    result = Tuple.Create(false, "Oops! Upgrade Employee inserted/updated failed.Please try again.");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Upgrade Employee inserted/updated failed.Please try again.");
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<UpgradeEmployeeModel>>> GetUpgradeEmployees()
        {
            Tuple<bool, string, List<UpgradeEmployeeModel>> result = null;
            List<UpgradeEmployeeModel> lstUpgradeEmployees = new List<UpgradeEmployeeModel>();
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<UpgradeEmployeeModel>("[dbo].[Select_UpgradeToEmployees]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstUpgradeEmployees = list as List<UpgradeEmployeeModel>;
                    con.Close();
                }

                if (lstUpgradeEmployees != null && lstUpgradeEmployees.Count() > 0)
                    result = Tuple.Create(true, "", lstUpgradeEmployees);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstUpgradeEmployees);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstUpgradeEmployees);
            }
            return result;
        }
    }
}