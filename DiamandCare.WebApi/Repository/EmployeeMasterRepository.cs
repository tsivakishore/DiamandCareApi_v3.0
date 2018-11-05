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

namespace DiamandCare.WebApi.Repository
{
    public class EmployeeMasterRepository
    {
        private string _dcDb = Settings.Default.DiamandCareConnection;
        int userID;

        public EmployeeMasterRepository()
        {
            userID = Helper.FindUserByID().UserID;
        }

        public async Task<Tuple<bool, string, List<EmployeeMasterModel>>> GetEmployeeMasterDetails()
        {
            Tuple<bool, string, List<EmployeeMasterModel>> result = null;
            List<EmployeeMasterModel> lstDetails = new List<EmployeeMasterModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                   
                    var list = await con.QueryAsync<EmployeeMasterModel>("[dbo].[Select_EmployeeMaster]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstDetails = list as List<EmployeeMasterModel>;
                   
                }

                if (lstDetails != null && lstDetails.Count() > 0)
                {
                    result = Tuple.Create(true, "", lstDetails);
                }
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string>> UpdateEmployee(EmployeeMasterModel obj)
        {
            Tuple<bool, string> result = null;
            int status = -1;

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@ID", obj.ID, DbType.Int32);
                    parameters.Add("@RegIncentive", obj.RegIncentive, DbType.Decimal);
                    parameters.Add("@LoanRePayIncentive", obj.LoanRePayIncentive, DbType.Decimal);
                    parameters.Add("@RecruitementsReq", obj.RecruitmentsReq, DbType.Int32);
                    parameters.Add("@TargetJoineesPerMonth", obj.TargetJoineesPerMonth, DbType.Int32);
                    parameters.Add("@Salary", obj.Salary, DbType.Decimal);
                    parameters.Add("@Description", obj.Description, DbType.String);
                    parameters.Add("@CreatedBy", userID, DbType.Int32);

                    status = await cxn.ExecuteScalarAsync<int>("[dbo].[Update_EmployeeMaster]", parameters, commandType: CommandType.StoredProcedure);

                    if (status == 0)
                        result = Tuple.Create(true, "Employee details updated successfully.");
                    else
                        result = Tuple.Create(false, "There has been an error while updating Employee details.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Employee details update failed.Please try again.");
            }

            return result;
        }
    }
}