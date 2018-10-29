using DiamandCare.Core;
using DiamandCare.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DiamandCare.WebApi
{
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        private ReportsRepository _repoReports = null;

        public ReportsController(ReportsRepository repoReports)
        {
            _repoReports = repoReports;
        }

        [Authorize]
        [Route("GetReportTypes")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<ReportsTypesModel>>> GetReportTypes(string LoginType)
        {
            Tuple<bool, string, List<ReportsTypesModel>> result = null;
            try
            {
                result = await _repoReports.GetReportTypes(LoginType);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("DownloadReport")]
        [HttpPost]
        public async Task<IEnumerable<dynamic>> DownloadReport(ReportsModel reportsModel)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            Tuple<bool, string, List<RegisterKeyViewModel>> resultRegisterKey = null;
            Tuple<bool, string, List<WalletTransactionsViewModel>> resultWalletTransactions = null;

            try
            {
                if (reportsModel.ReportType == AppConstants.ALL_LOAN_PAYMENTS || reportsModel.ReportType == AppConstants.LOAN_PAYMENTS)
                {
                    result = await _repoReports.DownloadLoanPaymentsReport(reportsModel);
                    if (result.Item1)
                    {
                        return result.Item3.Select(x => new
                        {
                            x.LoanID,
                            x.UserName,
                            x.LoanAmount,
                            x.IssuedAmount,
                            x.AmountToPay,
                            x.AmountPaid,
                            x.AdminCharges,
                            x.PrePaidLoanCharges,
                            LoanType = x.LoanTypeDescription,
                            x.LoanApprovalStatus,
                            x.TransferStatus,
                            LoanStatus = x.LoanStatusName
                        });
                    }
                    else
                    {
                        if (!result.Item1 && result.Item2 == AppConstants.NO_RECORDS_FOUND)
                        {
                            return result.Item3.Select(x => new
                            {
                                x.LoanID
                            });
                        }
                    }
                }
                else if (reportsModel.ReportType == AppConstants.ALL_LOAN_DETAILS || reportsModel.ReportType == AppConstants.LOAN_DETAILS)
                {
                    result = await _repoReports.DownloadLoanDetailsReport(reportsModel);
                    if (result.Item1)
                    {
                        return result.Item3.Select(x => new
                        {
                            x.LoanID,
                            x.UserName,
                            x.LoanAmount,
                            x.IssuedAmount,
                            x.AmountToPay,
                            x.AmountPaid,
                            x.AdminCharges,
                            x.PrePaidLoanCharges,
                            LoanType = x.LoanTypeDescription,
                            x.LoanApprovalStatus,
                            x.TransferStatus,
                            LoanStatus = x.LoanStatusName
                        });
                    }
                    else
                    {
                        if (!result.Item1 && result.Item2 == AppConstants.NO_RECORDS_FOUND)
                        {
                            return result.Item3.Select(x => new
                            {
                                x.LoanID
                            });
                        }
                    }
                }
                else if (reportsModel.ReportType == AppConstants.ALL_USED_SECRET_KEYS || reportsModel.ReportType == AppConstants.USED_SECRET_KEYS)
                {
                    resultRegisterKey = await _repoReports.DownloadUsedSecretKeysReport(reportsModel);
                    if (resultRegisterKey.Item1)
                    {
                        return resultRegisterKey.Item3.Select(x => new
                        {
                            UserName = x.CreatedBy,
                            x.RegKey,
                            x.PhoneNumber,
                            x.RegKeyStatus,
                            x.CreateDate,
                            KeyType = x.KeyType == "P" ? "Paid" : "Free",
                            x.KeyCost,
                            x.UsedTo
                        });
                    }
                    else
                    {
                        if (!resultRegisterKey.Item1 && resultRegisterKey.Item2 == AppConstants.NO_RECORDS_FOUND)
                        {
                            return resultRegisterKey.Item3.Select(x => new
                            {
                                x.RegKey
                            });
                        }
                    }
                }
                else if (reportsModel.ReportType == AppConstants.ALL_ISSUED_SECRET_KEYS || reportsModel.ReportType == AppConstants.ISSUED_SECRET_KEYS)
                {
                    resultRegisterKey = await _repoReports.DownloadIssuedSecretKeysReport(reportsModel);
                    if (resultRegisterKey.Item1)
                    {
                        return resultRegisterKey.Item3.Select(x => new
                        {
                            UserName = x.CreatedBy,
                            x.RegKey,
                            x.PhoneNumber,
                            x.RegKeyStatus,
                            x.CreateDate,
                            KeyType = x.KeyType == "P" ? "Paid" : "Free",
                            x.KeyCost
                        });
                    }
                    else
                    {
                        if (!resultRegisterKey.Item1 && resultRegisterKey.Item2 == AppConstants.NO_RECORDS_FOUND)
                        {
                            return resultRegisterKey.Item3.Select(x => new
                            {
                                x.RegKey
                            });
                        }
                    }
                }
                else if (reportsModel.ReportType == AppConstants.ALL_WALLET_TRANSACTIONS || reportsModel.ReportType == AppConstants.WALLET_TRANSACTIONS)
                {
                    resultWalletTransactions = await _repoReports.DownloadWalletTransactionsReport(reportsModel);
                    if (resultWalletTransactions.Item1)
                    {
                        return resultWalletTransactions.Item3.Select(x => new
                        {
                            x.UserName,
                            x.Against,
                            x.AgainstType,
                            x.TransactionType,
                            x.TransactionAmount,
                            x.Purpose,
                            x.CreatedOn
                        });
                    }
                    else
                    {
                        if (!resultWalletTransactions.Item1 && resultWalletTransactions.Item2 == AppConstants.NO_RECORDS_FOUND)
                        {
                            return resultWalletTransactions.Item3.Select(x => new
                            {
                                x.Against
                            });
                        }
                    }
                }
                else if (reportsModel.ReportType == AppConstants.ALL_COMMISSIONS_LOG || reportsModel.ReportType == AppConstants.COMMISSIONS_LOG)
                {
                    resultWalletTransactions = await _repoReports.DownloadCommissionsLogReport(reportsModel);
                    if (resultWalletTransactions.Item1)
                    {
                        return resultWalletTransactions.Item3.Select(x => new
                        {
                            x.UserName,
                            x.Against,
                            x.AgainstType,
                            x.TransactionType,
                            x.TransactionAmount,
                            x.Purpose,
                            x.CreatedOn
                        });
                    }
                    else
                    {
                        if (!resultWalletTransactions.Item1 && resultWalletTransactions.Item2 == AppConstants.NO_RECORDS_FOUND)
                        {
                            return resultWalletTransactions.Item3.Select(x => new
                            {
                                x.Against
                            });
                        }
                    }
                }
                else if (reportsModel.ReportType == AppConstants.EXPENSES)
                {
                    resultWalletTransactions = await _repoReports.DownloadExpensesReport(reportsModel);
                    if (resultWalletTransactions.Item1)
                    {
                        return resultWalletTransactions.Item3.Select(x => new
                        {
                            x.UserName,
                            x.Against,
                            x.AgainstType,
                            x.TransactionType,
                            x.TransactionAmount,
                            x.Purpose,
                            x.CreatedOn
                        });
                    }
                    else
                    {
                        if (!resultWalletTransactions.Item1 && resultWalletTransactions.Item2 == AppConstants.NO_RECORDS_FOUND)
                        {
                            return resultWalletTransactions.Item3.Select(x => new
                            {
                                x.Against
                            });
                        }
                    }
                }
                else if (reportsModel.ReportType == AppConstants.TRANSFER_PAYMENTS || reportsModel.ReportType == AppConstants.TRANSFER_PAYMENTS)
                {

                }

            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return null;
        }

        [Authorize]
        [Route("DownloadUserReport")]
        [HttpPost]
        public async Task<IEnumerable<dynamic>> DownloadUserReport(ReportsModel reportsModel)
        {
            Tuple<bool, string, List<UserReportModel>> result = null;

            try
            {
                result = await _repoReports.DownloadUserReport(reportsModel);
                if (result.Item1)
                {
                    return result.Item3.Select(x => new
                    {
                        x.Id,
                        x.UserID,
                        x.UserName,
                        x.FirstName,
                        x.LastName,
                        x.Email,
                        x.PhoneNumber,
                        x.FatherName,
                        x.AadharNumber,
                        x.ParentID,
                        x.SponserID,
                        x.SponserName,
                        x.UnderID,
                        x.UnderName,
                        x.RegisterFrom,
                        x.SourceID,
                        x.SourceName,
                        x.CreatedBy,
                        x.CreatedDate,
                        x.DcID,
                        x.SecretKey,
                        x.UserStatusID,
                        x.UserStatus,
                        x.LoanWaiveOff,
                        x.IsSponserJoineesReq
                    });
                }
                else
                {
                    if (!result.Item1 && result.Item2 == AppConstants.NO_RECORDS_FOUND)
                    {
                        return result.Item3.Select(x => new
                        {
                            x.Id
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return null;
        }
        [Authorize]
        [Route("DownloadAllUserReport")]
        [HttpGet]
        public async Task<IEnumerable<dynamic>> DownloadAllUserReport()
        {
            Tuple<bool, string, List<UserReportModel>> result = null;

            try
            {
                result = await _repoReports.DownloadAllUserReport();
                if (result.Item1)
                {
                    return result.Item3.Select(x => new
                    {
                        x.Id,
                        x.UserID,
                        x.UserName,
                        x.FirstName,
                        x.LastName,
                        x.Email,
                        x.PhoneNumber,
                        x.FatherName,
                        x.AadharNumber,
                        x.ParentID,
                        x.SponserID,
                        x.SponserName,
                        x.UnderID,
                        x.UnderName,
                        x.RegisterFrom,
                        x.SourceID,
                        x.SourceName,
                        x.CreatedBy,
                        x.CreatedDate,
                        x.DcID,
                        x.SecretKey,
                        x.UserStatusID,
                        x.UserStatus,
                        x.LoanWaiveOff,
                        x.IsSponserJoineesReq
                    });
                }
                else
                {
                    if (!result.Item1 && result.Item2 == AppConstants.NO_RECORDS_FOUND)
                    {
                        return result.Item3.Select(x => new
                        {
                            x.Id
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return null;
        }
    }
}