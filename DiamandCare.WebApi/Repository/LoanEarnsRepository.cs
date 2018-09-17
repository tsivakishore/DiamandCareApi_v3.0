using Dapper;

using DiamandCare.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DiamandCare.Core;
using DiamandCare.WebApi.Properties;

namespace DiamandCare.WebApi.Repository
{
    public class LoanEarnsRepository
    {
        private string _dvDb = Settings.Default.DiamandCareConnection;


        public async Task<Tuple<bool, string, List<LoanEarns>>> GetLoanEarns()
        {
            Tuple<bool, string, List<LoanEarns>> result = null;
            List<LoanEarns> lstErrorLogs = new List<LoanEarns>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<LoanEarns>("[dbo].[Select_LoanEarns]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstErrorLogs = list as List<LoanEarns>;
                    con.Close();
                }
                if (lstErrorLogs != null && lstErrorLogs.Count > 0)
                    result = Tuple.Create(true, "", lstErrorLogs);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstErrorLogs);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstErrorLogs);
            }
            return result;
        }
        public async Task<Tuple<bool, string, List<LoanEarnsModel>>> GetLoans()
        {
            Tuple<bool, string, List<LoanEarnsModel>> result = null;
            List<LoanEarnsModel> lstErrorLogs = new List<LoanEarnsModel>();
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@ID", Helper.FindUserByID().UserID, DbType.Int32);
                    var list = await con.QueryAsync<LoanEarnsModel>("[dbo].[Check_LoanEligibleDetails]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstErrorLogs = list as List<LoanEarnsModel>;
                    con.Close();
                }
                if (lstErrorLogs != null && lstErrorLogs.Count > 0)
                    result = Tuple.Create(true, "", lstErrorLogs);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstErrorLogs);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstErrorLogs);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoanEarnsModel>, int>> GetLoanForUser(string DCIDorName)
        {
            Tuple<bool, string, List<LoanEarnsModel>, int> result = null;
            List<LoanEarnsModel> lstLoans = new List<LoanEarnsModel>();
            var parameters = new DynamicParameters();
            int UserID = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@DCIDorName", DCIDorName, DbType.String);

                    using (var multi = await con.QueryMultipleAsync("[dbo].[Check_LoanEligibleDetails_By_DCIDorName]", parameters, commandType: CommandType.StoredProcedure))
                    {
                        lstLoans = multi.Read<LoanEarnsModel>().ToList();
                        UserID = multi.Read<int>().Single();
                    }

                    con.Close();
                }
                if (lstLoans != null && lstLoans.Count > 0)
                    result = Tuple.Create(true, "", lstLoans, UserID);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstLoans, UserID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstLoans, UserID);
            }
            return result;
        }
        public async Task<Tuple<bool, string>> CheckPersonalLoan()
        {
            Tuple<bool, string> result = null;
            int loanStatu = -1;
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", Helper.FindUserByID().UserID, DbType.Int32);
                    loanStatu = await con.ExecuteScalarAsync<int>("[dbo].[Check_PersonalLoan]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    con.Close();
                }

                if (loanStatu == 1)
                    result = Tuple.Create(true, "");
                else if (loanStatu == 0)
                    result = Tuple.Create(false, "Payment not done for previous applied loan");
                else
                    result = Tuple.Create(false, "Oops! Error while checking personal loan");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Error while checking personal loan");
            }
            return result;
        }

        public async Task<Tuple<bool, string>> CheckFeeReimbursement()
        {
            Tuple<bool, string> result = null;
            int loanStatu = -1;
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", Helper.FindUserByID().UserID, DbType.Int32);
                    loanStatu = await con.ExecuteScalarAsync<int>("[dbo].[Check_FeesReimbursementLoan]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    con.Close();
                }
                if (loanStatu == -2)
                    result = Tuple.Create(false, "Payment not done for previous applied loan");
                else if (loanStatu == -9)
                    result = Tuple.Create(false, "Fees Reimbursement already applied");
                else
                    result = Tuple.Create(true, "");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Error while checking fee reimbursement");
            }
            return result;
        }

        public async Task<Tuple<bool, string>> CheckHealthLoan()
        {
            Tuple<bool, string> result = null;
            int loanStatu = -1;
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", Helper.FindUserByID().UserID, DbType.Int32);
                    loanStatu = await con.ExecuteScalarAsync<int>("[dbo].[Check_HealthLoan]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    con.Close();
                }

                if (loanStatu == 1)
                    result = Tuple.Create(true, "");
                else if (loanStatu == 0)
                    result = Tuple.Create(false, "Payment not done for previous applied loan");
                else
                    result = Tuple.Create(false, "Oops! Error while checking health loan");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Error while checking health loan");
            }
            return result;
        }

        public async Task<Tuple<bool, string>> CheckRiskBenefit()
        {
            Tuple<bool, string> result = null;
            int loanStatu = -1;
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", Helper.FindUserByID().UserID, DbType.Int32);
                    loanStatu = await con.ExecuteScalarAsync<int>("[dbo].[Check_RiskBenefit]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    con.Close();
                }

                if (loanStatu == 1)
                    result = Tuple.Create(true, "");
                else if (loanStatu == 0)
                    result = Tuple.Create(false, "Payment not done for previous applied loan");
                else
                    result = Tuple.Create(false, "Oops! Error while checking risk benefit");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Error while checking risk benefit");
            }
            return result;
        }

        public async Task<Tuple<bool, string>> CheckHomeLoan()
        {
            Tuple<bool, string> result = null;
            int loanStatu = -1;
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", Helper.FindUserByID().UserID, DbType.Int32);
                    loanStatu = await con.ExecuteScalarAsync<int>("[dbo].[Check_HomeLoan]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    con.Close();
                }

                if (loanStatu == 1)
                    result = Tuple.Create(true, "");
                else if (loanStatu == 0)
                    result = Tuple.Create(false, "Payment not done for previous applied loan");
                else
                    result = Tuple.Create(false, "Oops! Error while checking home loan");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Error while checking home loan");
            }
            return result;
        }

        public async Task<Tuple<bool>> CheckRenewalStatus()
        {
            Tuple<bool> result = null;
            int status = -1;
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", Helper.FindUserByID().UserID, DbType.Int32);
                    status = await con.ExecuteScalarAsync<int>("[dbo].[Check_RenewalStatus]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    con.Close();
                }

                if (status == 1)
                    result = Tuple.Create(true);
                else if (status == 0)
                    result = Tuple.Create(false);
                else
                    result = Tuple.Create(false);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false);
            }
            return result;
        }

        //Start Loan Apply for User through Admin or Franchise
        public async Task<Tuple<bool, string>> CheckPersonalLoanByUserID(int UserID)
        {
            Tuple<bool, string> result = null;
            int loanStatu = -1;
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    loanStatu = await con.ExecuteScalarAsync<int>("[dbo].[Check_PersonalLoan]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    con.Close();
                }

                if (loanStatu == 1)
                    result = Tuple.Create(true, "");
                else if (loanStatu == 0)
                    result = Tuple.Create(false, "Payment not done for previous applied loan");
                else
                    result = Tuple.Create(false, "Oops! Error while checking personal loan");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Error while checking personal loan");
            }
            return result;
        }

        public async Task<Tuple<bool, string>> CheckFeeReimbursementByUserID(int UserID)
        {
            Tuple<bool, string> result = null;
            int loanStatu = -1;
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    loanStatu = await con.ExecuteScalarAsync<int>("[dbo].[Check_FeesReimbursementLoan]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    con.Close();
                }
                if (loanStatu == -2)
                    result = Tuple.Create(false, "Payment not done for previous applied loan");
                else if (loanStatu == -9)
                    result = Tuple.Create(false, "Fees Reimbursement already applied");
                else
                    result = Tuple.Create(true, "");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Error while checking fee reimbursement");
            }
            return result;
        }

        public async Task<Tuple<bool, string>> CheckHealthLoanByUserID(int UserID)
        {
            Tuple<bool, string> result = null;
            int loanStatu = -1;
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    loanStatu = await con.ExecuteScalarAsync<int>("[dbo].[Check_HealthLoan]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    con.Close();
                }

                if (loanStatu == 1)
                    result = Tuple.Create(true, "");
                else if (loanStatu == 0)
                    result = Tuple.Create(false, "Payment not done for previous applied loan");
                else
                    result = Tuple.Create(false, "Oops! Error while checking health loan");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Error while checking health loan");
            }
            return result;
        }

        public async Task<Tuple<bool, string>> CheckRiskBenefitByUserID(int UserID)
        {
            Tuple<bool, string> result = null;
            int loanStatu = -1;
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    loanStatu = await con.ExecuteScalarAsync<int>("[dbo].[Check_RiskBenefit]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    con.Close();
                }

                if (loanStatu == 1)
                    result = Tuple.Create(true, "");
                else if (loanStatu == 0)
                    result = Tuple.Create(false, "Payment not done for previous applied loan");
                else
                    result = Tuple.Create(false, "Oops! Error while checking risk benefit");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Error while checking risk benefit");
            }
            return result;
        }

        public async Task<Tuple<bool, string>> CheckHomeLoanByUserID(int UserID)
        {
            Tuple<bool, string> result = null;
            int loanStatu = -1;
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    loanStatu = await con.ExecuteScalarAsync<int>("[dbo].[Check_HomeLoan]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    con.Close();
                }

                if (loanStatu == 1)
                    result = Tuple.Create(true, "");
                else if (loanStatu == 0)
                    result = Tuple.Create(false, "Payment not done for previous applied loan");
                else
                    result = Tuple.Create(false, "Oops! Error while checking home loan");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Error while checking home loan");
            }
            return result;
        }

        public async Task<Tuple<bool>> CheckRenewalStatusByUserID(int UserID)
        {
            Tuple<bool> result = null;
            int status = -1;
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    status = await con.ExecuteScalarAsync<int>("[dbo].[Check_RenewalStatus]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    con.Close();
                }

                if (status == 1)
                    result = Tuple.Create(true);
                else if (status == 0)
                    result = Tuple.Create(false);
                else
                    result = Tuple.Create(false);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false);
            }
            return result;
        }
        //End Loan Apply for User through Admin or Franchise
    }
}
