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
    public class UserBankDetailsRepository
    {
        private string _dvDb = Settings.Default.DiamandCareConnection;


        public async Task<Tuple<bool, string, List<UserBankDetails>>> GetUserBankDetails(int ID)
        {
            Tuple<bool, string, List<UserBankDetails>> result = null;
            List<UserBankDetails> lstBankDetails = new List<UserBankDetails>();
         
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", ID, DbType.Int32);
                    var list = await con.QueryAsync<UserBankDetails>("[dbo].[Select_UserBankDetails]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstBankDetails = list as List<UserBankDetails>;
                    con.Close();
                }

                if (lstBankDetails != null && lstBankDetails.Count() > 0)
                {
                    result = Tuple.Create(true, "", lstBankDetails);
                }
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstBankDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstBankDetails);
            }
            return result;
        }
        public async Task<Tuple<bool, string, UserBankDetails>> InsertorUpdateUserBankDetails(UserBankDetails obj)
        {
            Tuple<bool, string, UserBankDetails> objKey = null;
            UserBankDetails userBank = new UserBankDetails();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dvDb))
                {
                    if(obj.ID!=0)
                        parameters.Add("@ID", obj.ID, DbType.Int32);
                    parameters.Add("@UserID", obj.UserID, DbType.Int32);
                    parameters.Add("@BankID", obj.BankID, DbType.Int32);
                    parameters.Add("@AccountHolderName", obj.AccountHolderName, DbType.String);
                    parameters.Add("@AccountNumber", obj.AccountNumber, DbType.String);
                    parameters.Add("@IFSCCode", obj.IFSCCode, DbType.String);
                    parameters.Add("@BranchName", obj.BranchName, DbType.String);
                    parameters.Add("@BranchAddress", obj.BranchAddress, DbType.String);
                    parameters.Add("@CreatedBy", Helper.FindUserByID().UserID, DbType.Int32);

                    var resultObj = await cxn.QueryAsync<UserBankDetails>("dbo.InsertorUpdate_UserBankDetails", parameters, commandType: CommandType.StoredProcedure);
                    userBank = resultObj.Single() as UserBankDetails;

                    cxn.Close();
                }
                if(obj.ID!=0)
                objKey = Tuple.Create(true, "User Bank Details inserted successfully.", userBank);
                else
                    objKey = Tuple.Create(true, "User Bank Details updated successfully.", userBank);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                objKey = Tuple.Create(false, "Oops! User Bank Details inserted/updated failed.Please try again.", userBank);
            }

            return objKey;
        }
    }
}