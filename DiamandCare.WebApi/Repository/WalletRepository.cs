using Dapper;
using DiamandCare.Core;
using DiamandCare.WebApi.Core;
using DiamandCare.WebApi.Models;
using DiamandCare.WebApi.Properties;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DiamandCare.WebApi.Repository
{
    public class WalletRepository
    {
        private string _dcDb = Settings.Default.DiamandCareConnection;
        public static int UserID;

        public WalletRepository()
        {
            UserID = Helper.FindUserByID().UserID;
        }

        public async Task<Tuple<bool, string, Wallet>> GetWallet()
        {
            Tuple<bool, string, Wallet> result = null;
            Wallet data = new Wallet();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    var listData = await con.QueryAsync<Wallet>("[dbo].[Select_Wallet]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    data = listData.Single() as Wallet;
                    con.Close();
                }

                if (data != null)
                {
                    result = Tuple.Create(true, "", data);
                }
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, data);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", data);
            }
            return result;
        }
        public async Task<Tuple<bool, string>> InsertWalletExpenses(WalletTransactions obj)
        {
            int addExpensesStatus = -1;
            Tuple<bool, string> addExpensesResult = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    if (obj.ID > 0)
                        parameters.Add("@ID", obj.ID, DbType.Int32);
                    parameters.Add("@userID", UserID, DbType.Int32);
                    parameters.Add("@TransactionAmount", obj.TransactionAmount, DbType.Decimal);
                    parameters.Add("@Purpose", obj.Purpose, DbType.String);
                    addExpensesStatus = await con.ExecuteScalarAsync<int>("dbo.Insert_WalletExpenses", parameters, commandType: CommandType.StoredProcedure);
                    if (addExpensesStatus == 0)
                        addExpensesResult = Tuple.Create(true, "You have Inserted Expenses Details successfully.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                addExpensesResult = Tuple.Create(false, "Oops! Expenses Details failed.");
            }
            return addExpensesResult;
        }
        public async Task<Tuple<bool, string, List<WalletTransactionsViewModel>>> GetWalletRecentExpenses()
        {
            Tuple<bool, string, List<WalletTransactionsViewModel>> result = null;
            List<WalletTransactionsViewModel> lstKeys = new List<WalletTransactionsViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@userID", UserID, DbType.Int32);
                    con.Open();
                    var list = await con.QueryAsync<WalletTransactionsViewModel>("[dbo].[Select_WalletRecentExpenses]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstKeys = list as List<WalletTransactionsViewModel>;
                    con.Close();
                }
                if (lstKeys != null && lstKeys.Count > 0)
                    result = Tuple.Create(true, "", lstKeys);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstKeys);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstKeys);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<WalletTransactionsViewModel>>> GetWalletTransactions()
        {
            Tuple<bool, string, List<WalletTransactionsViewModel>> result = null;
            List<WalletTransactionsViewModel> lstKeys = new List<WalletTransactionsViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@userID", UserID, DbType.Int32);
                    con.Open();
                    var list = await con.QueryAsync<WalletTransactionsViewModel>("[dbo].[Select_WalletTransactions]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstKeys = list as List<WalletTransactionsViewModel>;
                    con.Close();
                }
                if (lstKeys != null && lstKeys.Count > 0)
                    result = Tuple.Create(true, "", lstKeys);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstKeys);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstKeys);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<WithdrawFundsViewModel>>> GetWithdrawalTransactions()
        {
            Tuple<bool, string, List<WithdrawFundsViewModel>> result = null;
            List<WithdrawFundsViewModel> lstKeys = new List<WithdrawFundsViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<WithdrawFundsViewModel>("[dbo].[Select_WithdrawalTransactions]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstKeys = list as List<WithdrawFundsViewModel>;
                    con.Close();
                }
                if (lstKeys != null && lstKeys.Count > 0)
                    result = Tuple.Create(true, "", lstKeys);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstKeys);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstKeys);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<FundRequestViewModel>>> GetFundRequest()
        {
            Tuple<bool, string, List<FundRequestViewModel>> result = null;
            List<FundRequestViewModel> lstKeys = new List<FundRequestViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@userID", UserID, DbType.Int32);
                    con.Open();
                    var list = await con.QueryAsync<FundRequestViewModel>("[dbo].[Select_FundRequest]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstKeys = list as List<FundRequestViewModel>;
                    con.Close();
                }
                if (lstKeys != null && lstKeys.Count > 0)
                    result = Tuple.Create(true, "", lstKeys);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstKeys);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstKeys);
            }
            return result;
        }
        public async Task<Tuple<bool, string, List<FundRequestStatus>>> GetFundRequestStatus()
        {
            Tuple<bool, string, List<FundRequestStatus>> result = null;
            List<FundRequestStatus> lstFundsRequestStatus = new List<FundRequestStatus>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var list = await con.QueryAsync<FundRequestStatus>("[dbo].[Select_FundRequestStatus]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstFundsRequestStatus = list as List<FundRequestStatus>;
                }
                if (lstFundsRequestStatus != null && lstFundsRequestStatus.Count > 0)
                    result = Tuple.Create(true, "", lstFundsRequestStatus);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstFundsRequestStatus);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstFundsRequestStatus);
            }
            return result;
        }

        public async Task<Tuple<bool, string>> RequestFunds(FundRequest fundRequestModel)
        {
            int requestStatus = -1;
            Tuple<bool, string> requestFundsResult = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@UserID", UserID, DbType.Int32);
                    parameters.Add("@RequestedAmount", fundRequestModel.RequestedAmount, DbType.Decimal);
                    parameters.Add("@RequestToUserID", fundRequestModel.RequestToUserID, DbType.Int32);
                    parameters.Add("@RequestStatusID", fundRequestModel.RequestStatusID, DbType.Int32);
                    parameters.Add("@CreatedBy", UserID, DbType.Int32);
                    requestStatus = await con.ExecuteScalarAsync<int>("dbo.Insert_FundRequest", parameters, commandType: CommandType.StoredProcedure);
                    if (requestStatus == 0)
                        requestFundsResult = Tuple.Create(true, "Your funds request successfully.");
                    else
                        requestFundsResult = Tuple.Create(false, "Your funds request failed.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                requestFundsResult = Tuple.Create(false, "Oops! Your funds request failed.");
            }
            return requestFundsResult;
        }

        public async Task<Tuple<bool, string, List<FundRequestViewModel>>> GetUserFundRequestDetails()
        {
            Tuple<bool, string, List<FundRequestViewModel>> result = null;
            List<FundRequestViewModel> lstUserFundsRequestDetails = new List<FundRequestViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    con.Open();
                    var list = await con.QueryAsync<FundRequestViewModel>("[dbo].[Select_UserFundRequestDetails]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstUserFundsRequestDetails = list as List<FundRequestViewModel>;
                    con.Close();
                }
                if (lstUserFundsRequestDetails != null && lstUserFundsRequestDetails.Count > 0)
                    result = Tuple.Create(true, "", lstUserFundsRequestDetails);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstUserFundsRequestDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstUserFundsRequestDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string>> ApproveFundsRequest(FundRequest fundRequestModel)
        {
            int requestStatus = -1;
            Tuple<bool, string> requestFundsResult = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@FundsRequestID", fundRequestModel.ID, DbType.Int32);
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    parameters.Add("@RequestedAmount", fundRequestModel.RequestedAmount, DbType.Decimal);
                    parameters.Add("@ApprovedAmount", fundRequestModel.ApprovedAmount, DbType.Decimal);
                    parameters.Add("@RequestToUserID", fundRequestModel.RequestToUserID, DbType.Int32);
                    parameters.Add("@RequestStatusID", fundRequestModel.RequestStatusID, DbType.Int32);
                    parameters.Add("@CreatedBy", UserID, DbType.Int32);
                    requestStatus = await con.ExecuteScalarAsync<int>("dbo.Update_FundRequest", parameters, commandType: CommandType.StoredProcedure);
                    if (requestStatus == 0)
                        requestFundsResult = Tuple.Create(true, "Funds request updated successfully.");
                    else
                        requestFundsResult = Tuple.Create(false, "Funds request updation failed.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                requestFundsResult = Tuple.Create(false, "Oops! Funds request updation failed.");
            }
            return requestFundsResult;
        }

        public async Task<Tuple<bool, string>> UpdateFundsTransfer(FundRequest fundRequestModel)
        {
            int requestStatus = -1;
            Tuple<bool, string> updateFundsTransferResult = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@FromUserID", UserID, DbType.Int32);
                    parameters.Add("@Amount", fundRequestModel.ApprovedAmount, DbType.Decimal);
                    parameters.Add("@ToUserID", fundRequestModel.RequestToUserID, DbType.Int32);
                    parameters.Add("@CreatedBy", UserID, DbType.Int32);
                    requestStatus = await con.ExecuteScalarAsync<int>("dbo.Update_FundTransfer", parameters, commandType: CommandType.StoredProcedure);
                    if (requestStatus == 0)
                        updateFundsTransferResult = Tuple.Create(true, "Transfer funds successfully.");
                    else
                        updateFundsTransferResult = Tuple.Create(false, "Transfer funds failed.Please try again.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                updateFundsTransferResult = Tuple.Create(false, "Oops! Transfer funds failed.Please try again.");
            }
            return updateFundsTransferResult;
        }

        public async Task<Tuple<bool, string>> InsertWithdrawals(WithdrawFunds withdrawModel)
        {
            int requestStatus = -1;
            Tuple<bool, string> InsertWithdrawalsResult = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@userID", UserID, DbType.Int32);
                    parameters.Add("@ID", withdrawModel.UserID, DbType.Int32);
                    parameters.Add("@WithdrawAmount", withdrawModel.WithdrawAmount, DbType.Decimal);
                    parameters.Add("@Purpose", withdrawModel.Purpose, DbType.String);
                    requestStatus = await con.ExecuteScalarAsync<int>("dbo.InsertWithdrawals", parameters, commandType: CommandType.StoredProcedure);
                    if (requestStatus == 0)
                        InsertWithdrawalsResult = Tuple.Create(true, "Withrawal of Rs." + withdrawModel.WithdrawAmount + " successfully.");
                    else
                        InsertWithdrawalsResult = Tuple.Create(false, "Withdrawal funds failed.Please try again.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                InsertWithdrawalsResult = Tuple.Create(false, "Oops! Withdrawal funds failed.Please try again.");
            }
            return InsertWithdrawalsResult;
        }

        public async Task<Tuple<bool, string>> DeleteWalletExpenses(WalletTransactions walletTransactions)
        {
            int deleteExpensesStatus = -1;
            Tuple<bool, string> deleteExpensesResult = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@ID", walletTransactions.ID, DbType.Int32);
                    parameters.Add("@userID", UserID, DbType.Int32);
                    parameters.Add("@TransactionAmount", walletTransactions.TransactionAmount, DbType.Decimal);

                    deleteExpensesStatus = await con.ExecuteScalarAsync<int>("dbo.Update_WalletExpenses", parameters, commandType: CommandType.StoredProcedure);
                    if (deleteExpensesStatus == 0)
                        deleteExpensesResult = Tuple.Create(true, "Expenses details deleted successfully.");
                    else
                        deleteExpensesResult = Tuple.Create(false, "Expenses details deleted failed.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                deleteExpensesResult = Tuple.Create(false, "Oops! Expenses details deleted failed.Please try again.");
            }
            return deleteExpensesResult;
        }
    }
}