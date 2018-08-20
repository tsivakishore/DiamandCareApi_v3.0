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

namespace DiamandCare.WebApi
{
    public class LoansRepository
    {
        private string _dvDb = Settings.Default.DiamandCareConnection;
        DataTable _dtName = null;
        public static int UserID;
        public LoansRepository()
        {
            UserID = Helper.FindUserByID().UserID;
        }

        public async Task<Tuple<bool, string>> ApplyPersonalLoan(LoansModel applyPLLoansModel)
        {
            int applyLoanStatus = -1;
            Tuple<bool, string> applyLoanResult = null;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dvDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@LoanID", applyPLLoansModel.LoanID, DbType.Int32);
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    parameters.Add("@GroupID", applyPLLoansModel.GroupID, DbType.Int32);
                    parameters.Add("@LoanAmount", applyPLLoansModel.LoanAmount, DbType.Double);
                    parameters.Add("@IssuedAmount", applyPLLoansModel.IssuedAmount, DbType.Double);
                    parameters.Add("@AmountToPay", applyPLLoansModel.AmountToPay, DbType.Double);
                    parameters.Add("@AdminCharges", applyPLLoansModel.AdminCharges, DbType.Double);
                    parameters.Add("@ModeofTransfer", applyPLLoansModel.ModeofTransfer, DbType.Int32);
                    parameters.Add("@LoanStatusID", applyPLLoansModel.LoanStatusID, DbType.Int32);
                    parameters.Add("@LoanTypeCode", applyPLLoansModel.LoanTypeCode, DbType.String);
                    parameters.Add("@PrePaidLoan", applyPLLoansModel.PrePaidLoan, DbType.Boolean);

                    applyLoanStatus = await cxn.ExecuteScalarAsync<int>("dbo.Insert_Loan", parameters, commandType: CommandType.StoredProcedure);

                    if (applyLoanStatus > 0)
                        applyLoanResult = Tuple.Create(true, "You have been applied personal loan successfully.");
                    else if (applyLoanStatus == -2)
                        applyLoanResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_2);
                    else if (applyLoanStatus == -3)
                        applyLoanResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_3);
                    else if (applyLoanStatus == -4)
                        applyLoanResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_4);
                    else if (applyLoanStatus == -5)
                        applyLoanResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_5);
                    else if (applyLoanStatus == -6)
                        applyLoanResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_6);
                    else if (applyLoanStatus == -7)
                        applyLoanResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_7);
                    else if (applyLoanStatus == -8)
                        applyLoanResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_8);
                    else if (applyLoanStatus == -9)
                        applyLoanResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_9);
                    else if (applyLoanStatus == -10)
                        applyLoanResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_10);
                    else if (applyLoanStatus == -11)
                        applyLoanResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_11);
                    else if (applyLoanStatus == -12)
                        applyLoanResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_12);
                    else if (applyLoanStatus == -13)
                        applyLoanResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_13);
                    else if (applyLoanStatus == -14)
                        applyLoanResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_14);
                    else
                        applyLoanResult = Tuple.Create(false, "Oops! Personal loan applied failed.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                applyLoanResult = Tuple.Create(false, "Oops! Personal loan applied failed.");
            }

            return applyLoanResult;
        }
        public async Task<Tuple<bool, string>> ApplyHomeLoan(LoansModel applyHLLoansModel)
        {
            int applyLoanStatus = -1;
            Tuple<bool, string> applyHomeResult = null;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dvDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@LoanID", applyHLLoansModel.LoanID, DbType.Int32);
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    parameters.Add("@GroupID", applyHLLoansModel.GroupID, DbType.Int32);
                    parameters.Add("@LoanAmount", applyHLLoansModel.LoanAmount, DbType.Double);
                    parameters.Add("@IssuedAmount", applyHLLoansModel.IssuedAmount, DbType.Double);
                    parameters.Add("@AmountToPay", applyHLLoansModel.AmountToPay, DbType.Double);
                    parameters.Add("@AdminCharges", applyHLLoansModel.AdminCharges, DbType.Double);
                    parameters.Add("@ModeofTransfer", applyHLLoansModel.ModeofTransfer, DbType.Int32);
                    parameters.Add("@LoanStatusID", applyHLLoansModel.LoanStatusID, DbType.Int32);
                    parameters.Add("@LoanTypeCode", applyHLLoansModel.LoanTypeCode, DbType.String);

                    applyLoanStatus = await cxn.ExecuteScalarAsync<int>("dbo.Insert_Loan", parameters, commandType: CommandType.StoredProcedure);

                    if (applyLoanStatus > 0)
                        applyHomeResult = Tuple.Create(true, "You have been applied home loan successfully.");
                    else if (applyLoanStatus == -2)
                        applyHomeResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_2);
                    else if (applyLoanStatus == -3)
                        applyHomeResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_3);
                    else if (applyLoanStatus == -4)
                        applyHomeResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_4);
                    else if (applyLoanStatus == -5)
                        applyHomeResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_5);
                    else if (applyLoanStatus == -6)
                        applyHomeResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_6);
                    else if (applyLoanStatus == -7)
                        applyHomeResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_7);
                    else if (applyLoanStatus == -8)
                        applyHomeResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_8);
                    else if (applyLoanStatus == -9)
                        applyHomeResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_9);
                    else if (applyLoanStatus == -10)
                        applyHomeResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_10);
                    else if (applyLoanStatus == -11)
                        applyHomeResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_11);
                    else if (applyLoanStatus == -12)
                        applyHomeResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_12);
                    else if (applyLoanStatus == -13)
                        applyHomeResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_13);
                    else if (applyLoanStatus == -14)
                        applyHomeResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_14);
                    else
                        applyHomeResult = Tuple.Create(false, "Oops! Home loan applied failed.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                applyHomeResult = Tuple.Create(false, "Oops! Home loan applied failed.");
            }

            return applyHomeResult;
        }
        public async Task<Tuple<bool, string>> ApplyFeeReimbursement(FeeReimbursementModel applyFeeReimbursementModel, List<MultipartFileData> multipartFileData, string fileUploadPath)
        {
            int applyLoanStatus = -1;
            Tuple<bool, string> applyFeeReimbursementResult = null;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dvDb))
                {
                    applyFeeReimbursementModel.UserID = UserID;
                    var parameters = new DynamicParameters();
                    parameters.Add("@LoanID", applyFeeReimbursementModel.LoanID, DbType.Int32);
                    parameters.Add("@UserID", applyFeeReimbursementModel.UserID, DbType.Int32);
                    parameters.Add("@GroupID", applyFeeReimbursementModel.GroupID, DbType.Int32);
                    parameters.Add("@LoanAmount", applyFeeReimbursementModel.LoanAmount, DbType.Double);
                    parameters.Add("@IssuedAmount", applyFeeReimbursementModel.IssuedAmount, DbType.Double);
                    parameters.Add("@AmountToPay", applyFeeReimbursementModel.AmountToPay, DbType.Double);
                    parameters.Add("@AdminCharges", applyFeeReimbursementModel.AdminCharges, DbType.Double);
                    parameters.Add("@ModeofTransfer", applyFeeReimbursementModel.ModeofTransfer, DbType.Int32);
                    parameters.Add("@LoanStatusID", applyFeeReimbursementModel.LoanStatusID, DbType.Int32);
                    parameters.Add("@LoanTypeCode", applyFeeReimbursementModel.LoanTypeCode, DbType.String);
                    parameters.Add("@LoanDocumentTable", CreateTableFR(applyFeeReimbursementModel, multipartFileData, fileUploadPath).AsTableValuedParameter());

                    applyLoanStatus = await cxn.ExecuteScalarAsync<int>("dbo.Insert_Loan", parameters, commandType: CommandType.StoredProcedure);

                    if (applyLoanStatus > 0)
                        applyFeeReimbursementResult = Tuple.Create(true, "You have been applied for fee reimbursement successfully.");
                    else if (applyLoanStatus == -2)
                        applyFeeReimbursementResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_2);
                    else if (applyLoanStatus == -3)
                        applyFeeReimbursementResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_3);
                    else if (applyLoanStatus == -4)
                        applyFeeReimbursementResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_4);
                    else if (applyLoanStatus == -5)
                        applyFeeReimbursementResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_5);
                    else if (applyLoanStatus == -6)
                        applyFeeReimbursementResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_6);
                    else if (applyLoanStatus == -7)
                        applyFeeReimbursementResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_7);
                    else if (applyLoanStatus == -8)
                        applyFeeReimbursementResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_8);
                    else if (applyLoanStatus == -9)
                        applyFeeReimbursementResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_9);
                    else if (applyLoanStatus == -10)
                        applyFeeReimbursementResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_10);
                    else if (applyLoanStatus == -11)
                        applyFeeReimbursementResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_11);
                    else if (applyLoanStatus == -12)
                        applyFeeReimbursementResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_12);
                    else if (applyLoanStatus == -13)
                        applyFeeReimbursementResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_13);
                    else if (applyLoanStatus == -14)
                        applyFeeReimbursementResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_14);
                    else
                        applyFeeReimbursementResult = Tuple.Create(false, "Oops! Fee reimbursement applied failed.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                applyFeeReimbursementResult = Tuple.Create(false, "Oops! Fee reimbursement applied failed.");
            }

            return applyFeeReimbursementResult;
        }

        public async Task<Tuple<bool, string>> ApplyHealthBenefits(HealthBenefitModel applyHealthBenefitModel, List<MultipartFileData> multipartFileData, string fileUploadPath)
        {
            int applyLoanStatus = -1;
            Tuple<bool, string> applyHealthBenefitResult = null;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dvDb))
                {
                    applyHealthBenefitModel.UserID = UserID;
                    var parameters = new DynamicParameters();
                    parameters.Add("@LoanID", applyHealthBenefitModel.LoanID, DbType.Int32);
                    parameters.Add("@UserID", applyHealthBenefitModel.UserID, DbType.Int32);
                    parameters.Add("@GroupID", applyHealthBenefitModel.GroupID, DbType.Int32);
                    parameters.Add("@LoanAmount", applyHealthBenefitModel.LoanAmount, DbType.Double);
                    parameters.Add("@IssuedAmount", applyHealthBenefitModel.IssuedAmount, DbType.Double);
                    parameters.Add("@AmountToPay", applyHealthBenefitModel.AmountToPay, DbType.Double);
                    parameters.Add("@AdminCharges", applyHealthBenefitModel.AdminCharges, DbType.Double);
                    parameters.Add("@ModeofTransfer", applyHealthBenefitModel.ModeofTransfer, DbType.Int32);
                    parameters.Add("@LoanStatusID", applyHealthBenefitModel.LoanStatusID, DbType.Int32);
                    parameters.Add("@LoanTypeCode", applyHealthBenefitModel.LoanTypeCode, DbType.String);
                    parameters.Add("@LoanDocumentTable", CreateTableHB(applyHealthBenefitModel, multipartFileData, fileUploadPath).AsTableValuedParameter());

                    applyLoanStatus = await cxn.ExecuteScalarAsync<int>("dbo.Insert_Loan", parameters, commandType: CommandType.StoredProcedure);

                    if (applyLoanStatus > 0)
                        applyHealthBenefitResult = Tuple.Create(true, "You have been applied for health benefit successfully.");
                    else if (applyLoanStatus == -2)
                        applyHealthBenefitResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_2);
                    else if (applyLoanStatus == -3)
                        applyHealthBenefitResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_3);
                    else if (applyLoanStatus == -4)
                        applyHealthBenefitResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_4);
                    else if (applyLoanStatus == -5)
                        applyHealthBenefitResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_5);
                    else if (applyLoanStatus == -6)
                        applyHealthBenefitResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_6);
                    else if (applyLoanStatus == -7)
                        applyHealthBenefitResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_7);
                    else if (applyLoanStatus == -8)
                        applyHealthBenefitResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_8);
                    else if (applyLoanStatus == -9)
                        applyHealthBenefitResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_9);
                    else if (applyLoanStatus == -10)
                        applyHealthBenefitResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_10);
                    else if (applyLoanStatus == -11)
                        applyHealthBenefitResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_11);
                    else if (applyLoanStatus == -12)
                        applyHealthBenefitResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_12);
                    else if (applyLoanStatus == -13)
                        applyHealthBenefitResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_13);
                    else if (applyLoanStatus == -14)
                        applyHealthBenefitResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_14);
                    else
                        applyHealthBenefitResult = Tuple.Create(false, "Oops! Health benefit applied failed.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                applyHealthBenefitResult = Tuple.Create(false, "Oops! Health benefit applied failed.");
            }

            return applyHealthBenefitResult;
        }

        public async Task<Tuple<bool, string>> ApplyRiskBenefits(RiskBenefitModel applyRiskBenefitModel, List<MultipartFileData> multipartFileData, string fileUploadPath)
        {
            int applyLoanStatus = -1;
            Tuple<bool, string> applyRiskBenefitsResult = null;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dvDb))
                {
                    applyRiskBenefitModel.UserID = UserID;
                    var parameters = new DynamicParameters();
                    parameters.Add("@LoanID", applyRiskBenefitModel.LoanID, DbType.Int32);
                    parameters.Add("@UserID", applyRiskBenefitModel.UserID, DbType.Int32);
                    parameters.Add("@GroupID", applyRiskBenefitModel.GroupID, DbType.Int32);
                    parameters.Add("@LoanAmount", applyRiskBenefitModel.LoanAmount, DbType.Double);
                    parameters.Add("@IssuedAmount", applyRiskBenefitModel.IssuedAmount, DbType.Double);
                    parameters.Add("@AmountToPay", applyRiskBenefitModel.AmountToPay, DbType.Double);
                    parameters.Add("@AdminCharges", applyRiskBenefitModel.AdminCharges, DbType.Double);
                    parameters.Add("@ModeofTransfer", applyRiskBenefitModel.ModeofTransfer, DbType.Int32);
                    parameters.Add("@LoanStatusID", applyRiskBenefitModel.LoanStatusID, DbType.Int32);
                    parameters.Add("@LoanTypeCode", applyRiskBenefitModel.LoanTypeCode, DbType.String);
                    parameters.Add("@LoanDocumentTable", CreateTableRB(applyRiskBenefitModel, multipartFileData, fileUploadPath).AsTableValuedParameter());

                    applyLoanStatus = await cxn.ExecuteScalarAsync<int>("dbo.Insert_Loan", parameters, commandType: CommandType.StoredProcedure);

                    if (applyLoanStatus > 0)
                        applyRiskBenefitsResult = Tuple.Create(true, "You have been applied for risk benefit successfully.");
                    else if (applyLoanStatus == -2)
                        applyRiskBenefitsResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_2);
                    else if (applyLoanStatus == -3)
                        applyRiskBenefitsResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_3);
                    else if (applyLoanStatus == -4)
                        applyRiskBenefitsResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_4);
                    else if (applyLoanStatus == -5)
                        applyRiskBenefitsResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_5);
                    else if (applyLoanStatus == -6)
                        applyRiskBenefitsResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_6);
                    else if (applyLoanStatus == -7)
                        applyRiskBenefitsResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_7);
                    else if (applyLoanStatus == -8)
                        applyRiskBenefitsResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_8);
                    else if (applyLoanStatus == -9)
                        applyRiskBenefitsResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_9);
                    else if (applyLoanStatus == -10)
                        applyRiskBenefitsResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_10);
                    else if (applyLoanStatus == -11)
                        applyRiskBenefitsResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_11);
                    else if (applyLoanStatus == -12)
                        applyRiskBenefitsResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_12);
                    else if (applyLoanStatus == -13)
                        applyRiskBenefitsResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_13);
                    else if (applyLoanStatus == -14)
                        applyRiskBenefitsResult = Tuple.Create(false, AppConstants.DISPLAY_MESSAGE_14);
                    else
                        applyRiskBenefitsResult = Tuple.Create(false, "Oops! Risk benefit applied failed.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                applyRiskBenefitsResult = Tuple.Create(false, "Oops! Risk benefit applied failed.");
            }

            return applyRiskBenefitsResult;
        }

        public async Task<Tuple<bool, string>> LoanApprovedOrRejected(LoansModel approvedLoansStatusModel)
        {
            int approvedLoanStatus = -1;
            Tuple<bool, string> approvedLoanResult = null;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dvDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@LoanID", approvedLoansStatusModel.LoanID, DbType.Int32);
                    parameters.Add("@UserID", approvedLoansStatusModel.UserID, DbType.Int32);
                    parameters.Add("@GroupID", approvedLoansStatusModel.GroupID, DbType.Int32);
                    parameters.Add("@IsApproved", approvedLoansStatusModel.IsApproved, DbType.Int32);
                    parameters.Add("@ApproveOrRejectedBy", UserID, DbType.Int32);

                    approvedLoanStatus = await cxn.ExecuteScalarAsync<int>("dbo.Loan_Approved_Rejected", parameters, commandType: CommandType.StoredProcedure);

                    if (approvedLoanStatus == 0)
                        approvedLoanResult = Tuple.Create(true, "You have been successfully approved/rejected loan.");
                    else
                        approvedLoanResult = Tuple.Create(false, "Oops! There has been an error while approve/reject loan.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                approvedLoanResult = Tuple.Create(false, "Oops! There has been an error while approve/reject loan.");
            }

            return approvedLoanResult;
        }

        public async Task<Tuple<bool, string>> LoanTransferApprovedOrRejected(LoansModel approvedLoansStatusModel)
        {
            int approvedLoanStatus = -1;
            Tuple<bool, string> approvedLoanResult = null;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dvDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@LoanID", approvedLoansStatusModel.LoanID, DbType.Int32);
                    parameters.Add("@UserID", approvedLoansStatusModel.UserID, DbType.Int32);
                    parameters.Add("@TransferStatusID", approvedLoansStatusModel.TransferStatusID, DbType.Int32);
                    parameters.Add("@TransferBy", UserID, DbType.Int32);

                    approvedLoanStatus = await cxn.ExecuteScalarAsync<int>("dbo.Update_LoanTransferStatus", parameters, commandType: CommandType.StoredProcedure);

                    if (approvedLoanStatus == 0)
                        approvedLoanResult = Tuple.Create(true, "You have changed  transfer status approved/rejected successfully.");
                    else
                        approvedLoanResult = Tuple.Create(false, "Oops! There has been an error while transfer status approve/reject loan.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                approvedLoanResult = Tuple.Create(false, "Oops! There has been an error while transfer status approve/reject loan.");
            }

            return approvedLoanResult;
        }

        public async Task<Tuple<bool, string, List<LoanDetailsViewModel>>> GetLoanDetails(int LoanID)
        {
            Tuple<bool, string, List<LoanDetailsViewModel>> result = null;
            List<LoanDetailsViewModel> lstLoanDetails = new List<LoanDetailsViewModel>();
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@LoanID", LoanID, DbType.Int32);
                    var list = await con.QueryAsync<LoanDetailsViewModel>("[dbo].[Select_LoanDetails]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstLoanDetails = list as List<LoanDetailsViewModel>;
                    con.Close();
                }
                if (lstLoanDetails != null)
                    result = Tuple.Create(true, "", lstLoanDetails);
                else
                    result = Tuple.Create(false, "No loan details found for this user.", lstLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstLoanDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string>> UpdateUserLoanPayment(int userID, int LoanID, decimal AmountToPay)
        {
            Tuple<bool, string> result = null;
            int updatedStatus = -1;
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@LoanID", LoanID, DbType.Int32);
                    parameters.Add("@UserID", userID, DbType.Int32);
                    parameters.Add("@AmountToPay", AmountToPay, DbType.Decimal);
                    parameters.Add("@CreatedBy", UserID, DbType.Int32);
                    updatedStatus = await con.ExecuteScalarAsync<int>("[dbo].[Update_Loan_PayAmount]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    con.Close();
                }
                if (updatedStatus == 0)
                    result = Tuple.Create(true, "Loan payment is successfull.");
                else
                    result = Tuple.Create(false, "Oops! Update loan payment failed. Please try again.");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Update loan payment failed. Please try again.");
            }
            return result;
        }
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetApproveLoanDetailsByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstApprovedLoanDetails = new List<LoansViewModel>();
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_Approved_by_UserID]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstApprovedLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstApprovedLoanDetails != null && lstApprovedLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstApprovedLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstApprovedLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstApprovedLoanDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetRejectedLoanDetailsByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstApprovedLoanDetails = new List<LoansViewModel>();
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_Rejected_By_UserID]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstApprovedLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstApprovedLoanDetails != null && lstApprovedLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstApprovedLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstApprovedLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstApprovedLoanDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetPendingLoanDetailsByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstNotApprovedLoanDetails = new List<LoansViewModel>();
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_NotApproved_By_UserID]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstNotApprovedLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstNotApprovedLoanDetails != null && lstNotApprovedLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstNotApprovedLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstNotApprovedLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstNotApprovedLoanDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetRejectedLoanDetailsByDCIDorUserName(string DCIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstApprovedLoanDetails = new List<LoansViewModel>();
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@DcIDorName", DCIDorName, DbType.String);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_Rejected_BY_DCIDorUserName]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstApprovedLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstApprovedLoanDetails != null && lstApprovedLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstApprovedLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstApprovedLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstApprovedLoanDetails);
            }
            return result;
        }
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetPaidLoanDetailsByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstApprovedLoanDetails = new List<LoansViewModel>();
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_Paid]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstApprovedLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstApprovedLoanDetails != null && lstApprovedLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstApprovedLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstApprovedLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstApprovedLoanDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetPaidLoanDetails()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstUserPaidLoanDetails = new List<LoansViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Users_Loans_Paid]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstUserPaidLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstUserPaidLoanDetails != null && lstUserPaidLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstUserPaidLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstUserPaidLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstUserPaidLoanDetails);
            }
            return result;
        }
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetPaidLoanDetailsByUserNameorDCID(string DcIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstUserPaidLoanDetails = new List<LoansViewModel>();
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@DcIDorName", DcIDorName, DbType.String);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_Paid_by_UserNameorDCID]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstUserPaidLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstUserPaidLoanDetails != null && lstUserPaidLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstUserPaidLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstUserPaidLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstUserPaidLoanDetails);
            }
            return result;
        }
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetActiveLoanDetailsByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstApprovedLoanDetails = new List<LoansViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_Active]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstApprovedLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstApprovedLoanDetails != null && lstApprovedLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstApprovedLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstApprovedLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstApprovedLoanDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetActiveLoanDetailsByUserNameorDCID(string DcIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstUserActiveLoans = new List<LoansViewModel>();
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@DcIDorName", DcIDorName, DbType.String);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_Active_By_DCIDorUserName]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstUserActiveLoans = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstUserActiveLoans != null && lstUserActiveLoans.Count > 0)
                    result = Tuple.Create(true, "", lstUserActiveLoans);
                else
                    result = Tuple.Create(false, "No records found", lstUserActiveLoans);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstUserActiveLoans);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> getapprovedloandetailsByDCIDorName(string DCIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstApprovedLoanDetails = new List<LoansViewModel>();
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@DCIDorName", DCIDorName, DbType.String);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_Approved_ByDCIDorName]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstApprovedLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstApprovedLoanDetails != null && lstApprovedLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstApprovedLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstApprovedLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstApprovedLoanDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> getpendingloandetailsByDCIDorName(string DCIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstApprovedLoanDetails = new List<LoansViewModel>();
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@DCIDorName", DCIDorName, DbType.String);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_NotApproved_ByDCIDorName]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstApprovedLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstApprovedLoanDetails != null && lstApprovedLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstApprovedLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstApprovedLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstApprovedLoanDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> getRejectedLoanDetailsDCIDorName(string DCIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstApprovedLoanDetails = new List<LoansViewModel>();
            var parameters = new DynamicParameters();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    parameters.Add("@DCIDorName", DCIDorName, DbType.String);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_Rejected_BY_DCIDorUserName]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstApprovedLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstApprovedLoanDetails != null && lstApprovedLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstApprovedLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstApprovedLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstApprovedLoanDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetRejectedLoanDetails() //UI Page appliedloandetails
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstApprovedLoanDetails = new List<LoansViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_Rejected]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstApprovedLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstApprovedLoanDetails != null && lstApprovedLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstApprovedLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstApprovedLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstApprovedLoanDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetNotApproveLoanDetails() //UI Page appliedloandetails
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstNotApprovedLoanDetails = new List<LoansViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_NotApproved]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstNotApprovedLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstNotApprovedLoanDetails != null && lstNotApprovedLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstNotApprovedLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstNotApprovedLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstNotApprovedLoanDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetApproveLoanDetails() //UI Page appliedloandetails
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstApprovedLoanDetails = new List<LoansViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_Approved]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstApprovedLoanDetails = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstApprovedLoanDetails != null && lstApprovedLoanDetails.Count > 0)
                    result = Tuple.Create(true, "", lstApprovedLoanDetails);
                else
                    result = Tuple.Create(false, "No records found", lstApprovedLoanDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstApprovedLoanDetails);
            }
            return result;
        }
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferPending()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstLoans = new List<LoansViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_AmountTransferPending]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstLoans = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstLoans != null && lstLoans.Count > 0)
                    result = Tuple.Create(true, "", lstLoans);
                else
                    result = Tuple.Create(false, "No records found", lstLoans);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstLoans);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferPendingByDCIDorName(string DCIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstLoans = new List<LoansViewModel>();
            try
            {
                var spParams = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    spParams.Add("@DCIDorName", DCIDorName, DbType.String);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_AmountTransferPending_ByDCIDorName]", spParams, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstLoans = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstLoans != null && lstLoans.Count > 0)
                    result = Tuple.Create(true, "", lstLoans);
                else
                    result = Tuple.Create(false, "No records found", lstLoans);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstLoans);
            }
            return result;
        }
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferedByDCIDorName(string DCIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstLoans = new List<LoansViewModel>();
            try
            {
                var spParams = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    spParams.Add("@DCIDorName", DCIDorName, DbType.String);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_AmountTransfered_ByDCIDorName]", spParams, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstLoans = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstLoans != null && lstLoans.Count > 0)
                    result = Tuple.Create(true, "", lstLoans);
                else
                    result = Tuple.Create(false, "No records found", lstLoans);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstLoans);
            }
            return result;
        }
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferRejectedByDCIDorName(string DCIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstLoans = new List<LoansViewModel>();
            try
            {
                var spParams = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    spParams.Add("@DCIDorName", DCIDorName, DbType.String);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_AmountTransferRejected_ByDCIDorName]", spParams, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstLoans = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstLoans != null && lstLoans.Count > 0)
                    result = Tuple.Create(true, "", lstLoans);
                else
                    result = Tuple.Create(false, "No records found", lstLoans);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstLoans);
            }
            return result;
        }
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransfered()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstLoans = new List<LoansViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_AmountTransfered]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstLoans = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstLoans != null && lstLoans.Count > 0)
                    result = Tuple.Create(true, "", lstLoans);
                else
                    result = Tuple.Create(false, "No records found", lstLoans);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstLoans);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferRejected()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstLoans = new List<LoansViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_AmountTransferRejected]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstLoans = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstLoans != null && lstLoans.Count > 0)
                    result = Tuple.Create(true, "", lstLoans);
                else
                    result = Tuple.Create(false, "No records found", lstLoans);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstLoans);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferPendingByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstLoans = new List<LoansViewModel>();
            try
            {
                var spParams = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    spParams.Add("@UserID", UserID, DbType.Int32);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_AmountTransferPending]", spParams, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstLoans = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstLoans != null && lstLoans.Count > 0)
                    result = Tuple.Create(true, "", lstLoans);
                else
                    result = Tuple.Create(false, "No records found", lstLoans);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstLoans);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferedByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstLoans = new List<LoansViewModel>();
            try
            {
                var spParams = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    spParams.Add("@UserID", UserID, DbType.Int32);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_AmountTransfered]", spParams, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstLoans = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstLoans != null && lstLoans.Count > 0)
                    result = Tuple.Create(true, "", lstLoans);
                else
                    result = Tuple.Create(false, "No records found", lstLoans);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstLoans);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferRejectedByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            List<LoansViewModel> lstLoans = new List<LoansViewModel>();
            try
            {
                var spParams = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    spParams.Add("@UserID", UserID, DbType.Int32);
                    var list = await con.QueryAsync<LoansViewModel>("[dbo].[Select_Loans_AmountTransferRejected]", spParams, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstLoans = list as List<LoansViewModel>;
                    con.Close();
                }
                if (lstLoans != null && lstLoans.Count > 0)
                    result = Tuple.Create(true, "", lstLoans);
                else
                    result = Tuple.Create(false, "No records found", lstLoans);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstLoans);
            }
            return result;
        }

        public async Task<Tuple<bool, string, LoanDetailsViewModel>> DownloadDocument(LoanDetailsViewModel loanDetailsViewModel)
        {
            Tuple<bool, string, LoanDetailsViewModel> result;

            LoanDetailsViewModel dataResult = null;
            try
            {
                using (SqlConnection cxn = new SqlConnection(_dvDb))
                {
                    // create sp parameters
                    DynamicParameters spParams = new DynamicParameters();
                    spParams.Add("@LoanID", loanDetailsViewModel.LoanID, DbType.Int32);
                    spParams.Add("@DocumentID", loanDetailsViewModel.DocumentID, DbType.Int32);
                    spParams.Add("@LoanType", loanDetailsViewModel.LoanType, DbType.String);
                    var res = await cxn.QueryAsync<LoanDetailsViewModel>("dbo.Download_Document", spParams, commandType: CommandType.StoredProcedure);
                    dataResult = res.FirstOrDefault();
                    //dataResult.FileContent = Encoding.UTF8.GetBytes(CtZip.Unzip(dataResult.FileContent));
                    cxn.Close();
                }

                if (dataResult != null)
                    result = Tuple.Create(true, "", dataResult);
                else
                    result = Tuple.Create(false, "No data found. Please try again.", dataResult);
            }
            catch (Exception ex)
            {
                //TODO: need to log the exception
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Download failed, Please try again.", dataResult);
            }
            return result;
        }
        private DataTable CreateTableFR(FeeReimbursementModel feeReimbursementModel, List<MultipartFileData> MultipartFileData, string fileUploadPath)
        {
            _dtName = new DataTable();
            _dtName.Columns.Add("UserID");
            _dtName.Columns.Add("FileName");
            _dtName.Columns.Add("FileContent", typeof(byte[]));
            _dtName.Columns.Add("DocumentType");
            _dtName.Columns.Add("LoanType");
            DataRow newDataRow = null;

            foreach (MultipartFileData fileContent in MultipartFileData)
            {
                ContentDispositionHeaderValue contentDispositionValue = fileContent.Headers.ContentDisposition;
                string Name = UnquoteToken(contentDispositionValue.Name) ?? String.Empty;
                string FileName = UnquoteToken(contentDispositionValue.FileName) ?? String.Empty;
                fileUploadPath = fileContent.LocalFileName;
                newDataRow = _dtName.NewRow();

                if (Name == "KYCDocumentContent")
                {
                    feeReimbursementModel.KYCDocumentContent = File.ReadAllBytes(fileUploadPath);
                    newDataRow = ReturnDataRow(feeReimbursementModel.UserID, feeReimbursementModel.KYCDocumentName, feeReimbursementModel.KYCDocumentContent, "KYC for Fee Reimbursement document", "FR");
                    _dtName.Rows.Add(newDataRow);
                }
                else if (Name == "BonafideContent")
                {
                    feeReimbursementModel.BonafideContent = File.ReadAllBytes(fileUploadPath);
                    newDataRow = ReturnDataRow(feeReimbursementModel.UserID, feeReimbursementModel.BonafideFileName, feeReimbursementModel.BonafideContent, "Bonafide cerificate document", "FR");
                    _dtName.Rows.Add(newDataRow);
                }
                else if (Name == "FeeReceiptContent")
                {
                    feeReimbursementModel.FeeReceiptContent = File.ReadAllBytes(fileUploadPath);
                    newDataRow = ReturnDataRow(feeReimbursementModel.UserID, feeReimbursementModel.FeeReceiptFileName, feeReimbursementModel.FeeReceiptContent, "Fee Receipt document", "FR");
                    _dtName.Rows.Add(newDataRow);
                }
                else if (Name == "FeeReimbursementOtherContent")
                {
                    feeReimbursementModel.FeeReimbursementOtherContent = File.ReadAllBytes(fileUploadPath);
                    newDataRow = ReturnDataRow(feeReimbursementModel.UserID, feeReimbursementModel.FeeReimbursementOtherFile, feeReimbursementModel.FeeReimbursementOtherContent, "Fee Reimbursement Other document", "FR");
                    _dtName.Rows.Add(newDataRow);
                }

            }

            return _dtName;
        }

        private DataTable CreateTableHB(HealthBenefitModel healthBenefitModel, List<MultipartFileData> MultipartFileData, string fileUploadPath)
        {
            _dtName = new DataTable();
            _dtName.Columns.Add("UserID");
            _dtName.Columns.Add("FileName");
            _dtName.Columns.Add("FileContent", typeof(byte[]));
            _dtName.Columns.Add("DocumentType");
            _dtName.Columns.Add("LoanType");
            DataRow newDataRow = null;

            foreach (MultipartFileData fileContent in MultipartFileData)
            {
                ContentDispositionHeaderValue contentDispositionValue = fileContent.Headers.ContentDisposition;
                string Name = UnquoteToken(contentDispositionValue.Name) ?? String.Empty;
                string FileName = UnquoteToken(contentDispositionValue.FileName) ?? String.Empty;
                fileUploadPath = fileContent.LocalFileName;
                newDataRow = _dtName.NewRow();

                if (Name == "KYCDocumentContent")
                {
                    healthBenefitModel.KYCDocumentContent = File.ReadAllBytes(fileUploadPath);
                    newDataRow = ReturnDataRow(healthBenefitModel.UserID, healthBenefitModel.KYCDocumentName, healthBenefitModel.KYCDocumentContent, "KYC for Health Benefit document", "HB");
                    _dtName.Rows.Add(newDataRow);
                }
                else if (Name == "HospitalAdmissionFormContent")
                {
                    healthBenefitModel.HospitalAdmissionFormContent = File.ReadAllBytes(fileUploadPath);
                    newDataRow = ReturnDataRow(healthBenefitModel.UserID, healthBenefitModel.HospitalAdmissionFormName, healthBenefitModel.HospitalAdmissionFormContent, "Hospital Admission document", "HB");
                    _dtName.Rows.Add(newDataRow);
                }
                else if (Name == "EstimatedHospitalChargesDocContent")
                {
                    healthBenefitModel.EstimatedHospitalChargesDocContent = File.ReadAllBytes(fileUploadPath);
                    newDataRow = ReturnDataRow(healthBenefitModel.UserID, healthBenefitModel.EstimatedHospitalChargesDocName, healthBenefitModel.EstimatedHospitalChargesDocContent, "Estimated Hospital Charges Document", "HB");
                    _dtName.Rows.Add(newDataRow);
                }
                else if (Name == "EstimatedHospitalOtherContent")
                {
                    healthBenefitModel.EstimatedHospitalOtherContent = File.ReadAllBytes(fileUploadPath);
                    newDataRow = ReturnDataRow(healthBenefitModel.UserID, healthBenefitModel.EstimatedHospitalOtherFile, healthBenefitModel.EstimatedHospitalOtherContent, "Estimated Hospital Other document", "HB");
                    _dtName.Rows.Add(newDataRow);
                }

            }

            return _dtName;
        }

        private DataTable CreateTableRB(RiskBenefitModel riskBenefitModel, List<MultipartFileData> MultipartFileData, string fileUploadPath)
        {
            _dtName = new DataTable();
            _dtName.Columns.Add("UserID");
            _dtName.Columns.Add("FileName");
            _dtName.Columns.Add("FileContent", typeof(byte[]));
            _dtName.Columns.Add("DocumentType");
            _dtName.Columns.Add("LoanType");
            DataRow newDataRow = null;

            foreach (MultipartFileData fileContent in MultipartFileData)
            {
                ContentDispositionHeaderValue contentDispositionValue = fileContent.Headers.ContentDisposition;
                string Name = UnquoteToken(contentDispositionValue.Name) ?? String.Empty;
                string FileName = UnquoteToken(contentDispositionValue.FileName) ?? String.Empty;
                fileUploadPath = fileContent.LocalFileName;
                newDataRow = _dtName.NewRow();

                if (Name == "KYCDocumentContent")
                {
                    riskBenefitModel.KYCDocumentContent = File.ReadAllBytes(fileUploadPath);
                    newDataRow = ReturnDataRow(riskBenefitModel.UserID, riskBenefitModel.KYCDocumentName, riskBenefitModel.KYCDocumentContent, "KYC for Risk Benefit document", "RB");
                    _dtName.Rows.Add(newDataRow);
                }
                else if (Name == "DeathCertificateContent")
                {
                    riskBenefitModel.DeathCertificateContent = File.ReadAllBytes(fileUploadPath);
                    newDataRow = ReturnDataRow(riskBenefitModel.UserID, riskBenefitModel.DeathCertificateFileName, riskBenefitModel.DeathCertificateContent, "Death Certificate", "RB");
                    _dtName.Rows.Add(newDataRow);
                }
                else if (Name == "RiskBenefitOtherContent")
                {
                    riskBenefitModel.RiskBenefitOtherContent = File.ReadAllBytes(fileUploadPath);
                    newDataRow = ReturnDataRow(riskBenefitModel.UserID, riskBenefitModel.RiskBenefitOtherFile, riskBenefitModel.RiskBenefitOtherContent, "Risk Benefi tOther document", "RB");
                    _dtName.Rows.Add(newDataRow);
                }
            }

            return _dtName;
        }
        private DataRow ReturnDataRow(int UserID, string FileName, byte[] FileContent, string DocumentType, string LoanType)
        {
            DataRow row = null;
            row = _dtName.NewRow();
            row["UserID"] = UserID;
            row["FileName"] = FileName;
            row["FileContent"] = FileContent;
            row["DocumentType"] = DocumentType;
            row["LoanType"] = LoanType;
            return row;
        }
        private static string UnquoteToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }
    }
}