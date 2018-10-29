using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiamandCare.WebApi.Properties;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using DiamandCare.Core;
using System.Globalization;
using DiamandCare.WebApi.Models;

namespace DiamandCare.WebApi
{
    public class ReportsRepository
    {
        string _dcDb = Settings.Default.DiamandCareConnection;
        int UserID = 0;

        public ReportsRepository()
        {
            UserID = Helper.FindUserByID().UserID;
        }

        public async Task<Tuple<bool, string, List<ReportsTypesModel>>> GetReportTypes(string LoginType)
        {
            Tuple<bool, string, List<ReportsTypesModel>> result = null;
            List<ReportsTypesModel> lstDetails = new List<ReportsTypesModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@LoginType", LoginType, DbType.String);
                    con.Open();
                    var list = await con.QueryAsync<ReportsTypesModel>("[dbo].[Select_ReportTypes]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstDetails = list as List<ReportsTypesModel>;
                    con.Close();
                }

                if (lstDetails != null && lstDetails.Count() > 0)
                    result = Tuple.Create(true, "", lstDetails);
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

        public async Task<Tuple<bool, string, List<LoansViewModel>>> DownloadLoanPaymentsReport(ReportsModel reportsModel)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstLoanpayments = new List<LoansViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@FromDate", DateTime.ParseExact(reportsModel.FromDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ToDate", DateTime.ParseExact(reportsModel.ToDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ReportType", reportsModel.ReportType, DbType.String);
                    parameters.Add("@UserID", UserID, DbType.Int32);

                    con.Open();

                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[rpt_LoanPayments]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstLoanpayments = list as List<LoansViewModel>;

                    con.Close();
                }

                if (lstLoanpayments != null && lstLoanpayments.Count() > 0)
                    result = Tuple.Create(true, "", lstLoanpayments);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstLoanpayments);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstLoanpayments);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<UserReportModel>>> DownloadUserReport(ReportsModel reportsModel)
        {
            Tuple<bool, string, List<UserReportModel>> result = null;
            List<UserReportModel> lst = new List<UserReportModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@FromDate", DateTime.ParseExact(reportsModel.FromDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ToDate", DateTime.ParseExact(reportsModel.ToDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);                   
                    parameters.Add("@UserStatusID", reportsModel.UserStatusID, DbType.Int32);
                    
                    var list = await con.QueryAsync<UserReportModel>("[dbo].[rpt_Users]", parameters, commandType: CommandType.StoredProcedure);
                    lst = list as List<UserReportModel>;
                }

                if (lst != null && lst.Count() > 0)
                    result = Tuple.Create(true, "", lst);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lst);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lst);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<UserReportModel>>> DownloadAllUserReport()
        {
            Tuple<bool, string, List<UserReportModel>> result = null;
            List<UserReportModel> lst = new List<UserReportModel>();

            try
            {               
                using (SqlConnection con = new SqlConnection(_dcDb))
                {                   
                    var list = await con.QueryAsync<UserReportModel>("[dbo].[rpt_AllUsers]", commandType: CommandType.StoredProcedure);
                    lst = list as List<UserReportModel>;
                }

                if (lst != null && lst.Count() > 0)
                    result = Tuple.Create(true, "", lst);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lst);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lst);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> DownloadLoanDetailsReport(ReportsModel reportsModel)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstLoanpayments = new List<LoansViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@FromDate", DateTime.ParseExact(reportsModel.FromDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ToDate", DateTime.ParseExact(reportsModel.ToDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ReportType", reportsModel.ReportType, DbType.String);
                    parameters.Add("@UserID", UserID, DbType.Int32);

                    con.Open();

                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[rpt_LoanDetails]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstLoanpayments = list as List<LoansViewModel>;

                    con.Close();
                }

                if (lstLoanpayments != null && lstLoanpayments.Count() > 0)
                    result = Tuple.Create(true, "", lstLoanpayments);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstLoanpayments);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstLoanpayments);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<RegisterKeyViewModel>>> DownloadUsedSecretKeysReport(ReportsModel reportsModel)
        {
            Tuple<bool, string, List<RegisterKeyViewModel>> result = null;
            List<RegisterKeyViewModel> lstUsedSecretKeys = new List<RegisterKeyViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@FromDate", DateTime.ParseExact(reportsModel.FromDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ToDate", DateTime.ParseExact(reportsModel.ToDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ReportType", reportsModel.ReportType, DbType.String);
                    parameters.Add("@UserID", UserID, DbType.Int32);

                    con.Open();

                    var list = await con.QueryAsync<RegisterKeyViewModel>("[dbo].[rpt_UsedSecretKeys]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstUsedSecretKeys = list as List<RegisterKeyViewModel>;

                    con.Close();
                }

                if (lstUsedSecretKeys != null && lstUsedSecretKeys.Count() > 0)
                    result = Tuple.Create(true, "", lstUsedSecretKeys);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstUsedSecretKeys);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstUsedSecretKeys);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<RegisterKeyViewModel>>> DownloadIssuedSecretKeysReport(ReportsModel reportsModel)
        {
            Tuple<bool, string, List<RegisterKeyViewModel>> result = null;
            List<RegisterKeyViewModel> lstIssuedSecretKeys = new List<RegisterKeyViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@FromDate", DateTime.ParseExact(reportsModel.FromDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ToDate", DateTime.ParseExact(reportsModel.ToDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ReportType", reportsModel.ReportType, DbType.String);
                    parameters.Add("@UserID", UserID, DbType.Int32);

                    con.Open();

                    var list = await con.QueryAsync<RegisterKeyViewModel>("[dbo].[rpt_IssuedSecretKeys]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstIssuedSecretKeys = list as List<RegisterKeyViewModel>;

                    con.Close();
                }

                if (lstIssuedSecretKeys != null && lstIssuedSecretKeys.Count() > 0)
                    result = Tuple.Create(true, "", lstIssuedSecretKeys);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstIssuedSecretKeys);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstIssuedSecretKeys);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<WalletTransactionsViewModel>>> DownloadWalletTransactionsReport(ReportsModel reportsModel)
        {
            Tuple<bool, string, List<WalletTransactionsViewModel>> result = null;
            List<WalletTransactionsViewModel> lstWalletTransactions = new List<WalletTransactionsViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@FromDate", DateTime.ParseExact(reportsModel.FromDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ToDate", DateTime.ParseExact(reportsModel.ToDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ReportType", reportsModel.ReportType, DbType.String);
                    parameters.Add("@UserID", UserID, DbType.Int32);

                    con.Open();

                    var list = await con.QueryAsync<WalletTransactionsViewModel>("[dbo].[rpt_WalletTransactions]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstWalletTransactions = list as List<WalletTransactionsViewModel>;

                    con.Close();
                }

                if (lstWalletTransactions != null && lstWalletTransactions.Count() > 0)
                    result = Tuple.Create(true, "", lstWalletTransactions);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstWalletTransactions);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstWalletTransactions);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<WalletTransactionsViewModel>>> DownloadCommissionsLogReport(ReportsModel reportsModel)
        {
            Tuple<bool, string, List<WalletTransactionsViewModel>> result = null;
            List<WalletTransactionsViewModel> lstCommissionsLog = new List<WalletTransactionsViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@FromDate", DateTime.ParseExact(reportsModel.FromDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ToDate", DateTime.ParseExact(reportsModel.ToDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ReportType", reportsModel.ReportType, DbType.String);
                    parameters.Add("@UserID", UserID, DbType.Int32);

                    con.Open();

                    var list = await con.QueryAsync<WalletTransactionsViewModel>("[dbo].[rpt_Commissions]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstCommissionsLog = list as List<WalletTransactionsViewModel>;

                    con.Close();
                }

                if (lstCommissionsLog != null && lstCommissionsLog.Count() > 0)
                    result = Tuple.Create(true, "", lstCommissionsLog);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstCommissionsLog);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstCommissionsLog);
            }
            return result;
        }
        public async Task<Tuple<bool, string, List<WalletTransactionsViewModel>>> DownloadExpensesReport(ReportsModel reportsModel)
        {
            Tuple<bool, string, List<WalletTransactionsViewModel>> result = null;
            List<WalletTransactionsViewModel> lstExpenses = new List<WalletTransactionsViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@FromDate", DateTime.ParseExact(reportsModel.FromDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ToDate", DateTime.ParseExact(reportsModel.ToDate, "d/M/yyyy", CultureInfo.InvariantCulture), DbType.DateTime);
                    parameters.Add("@ReportType", reportsModel.ReportType, DbType.String);
                    parameters.Add("@UserID", UserID, DbType.Int32);

                    con.Open();

                    var list = await con.QueryAsync<WalletTransactionsViewModel>("[dbo].[rpt_Expenses]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstExpenses = list as List<WalletTransactionsViewModel>;

                    con.Close();
                }

                if (lstExpenses != null && lstExpenses.Count() > 0)
                    result = Tuple.Create(true, "", lstExpenses);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstExpenses);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstExpenses);
            }
            return result;
        }
    }
}