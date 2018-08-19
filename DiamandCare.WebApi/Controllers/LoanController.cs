using DiamandCare.Core;
using DiamandCare.WebApi.Core;
using DiamandCare.WebApi.Models;
using DiamandCare.WebApi.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace DiamandCare.WebApi.Controllers
{
    [RoutePrefix("api/loan")]
    public class LoanController : BaseController
    {
        private LoanEarnsRepository _repo = null;
        private LoansRepository _repoLoans = null;

        public LoanController(LoanEarnsRepository repisitory, LoansRepository repoLoans)
        {
            _repo = repisitory;
            _repoLoans = repoLoans;
        }

        [Authorize]
        [Route("getloans")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoanEarnsModel>>> GetLoanEarns()
        {
            Tuple<bool, string, List<LoanEarnsModel>> result = null;
            try
            {
                result = await _repo.GetLoans();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("getloandetailsbyloanid")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoanDetailsViewModel>>> GetLoanDetails(int LoanID)
        {
            Tuple<bool, string, List<LoanDetailsViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetLoanDetails(LoanID);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("UpdateUserLoanPayment")]
        [HttpGet]
        public async Task<Tuple<bool, string>> UpdateUserLoanPayment(int UserID, int LoanID, decimal AmountToPay)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repoLoans.UpdateUserLoanPayment(UserID, LoanID, AmountToPay);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("getapprovedloandetailsuserid")] //By User id
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetApproveLoanDetailsByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetApproveLoanDetailsByUserID();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("getpendingloandetailsuserid")]  //By User id
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetNotApproveLoanDetailsByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetNotApproveLoanDetailsByUserID();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetRejectedLoanDetailsByUserID")] //By User id
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetRejectedLoanDetailsByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetRejectedLoanDetailsByUserID();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }
        [Authorize]
        [Route("GetRejectedLoanDetailsByDCIDorUserName")] //By User id
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetRejectedLoanDetailsByDCIDorUserName(string DCIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {

                result = await _repoLoans.GetRejectedLoanDetailsByDCIDorUserName(DCIDorName);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }
        [Authorize]
        [Route("GetActiveLoanDetailsByUserID")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetActiveLoanDetailsByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetActiveLoanDetailsByUserID();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetActiveLoanDetailsByUserNameorDCID")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetActiveLoanDetailsByUserNameorDCID(string DcIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetActiveLoanDetailsByUserNameorDCID(DcIDorName);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetPaidLoanDetailsByUserID")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetPaidLoanDetailsByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetPaidLoanDetailsByUserID();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetPaidLoanDetails")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetPaidLoanDetails()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetPaidLoanDetails();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetPaidLoanDetailsByUserNameorDCID")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetPaidLoanDetailsByUserNameorDCID(string DcIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetPaidLoanDetailsByUserNameorDCID(DcIDorName);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("getapprovedloandetails")] //All user loan details
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetApproveLoanDetails()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetApproveLoanDetails();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("getapprovedloandetailsByDCIDorName")] //All user loan details
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> getapprovedloandetailsByDCIDorName(string DCIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.getapprovedloandetailsByDCIDorName(DCIDorName);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("getpendingloandetailsByDCIDorName")] //All user loan details
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> getpendingloandetailsByDCIDorName(string DCIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.getpendingloandetailsByDCIDorName(DCIDorName);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("getRejectedLoanDetailsDCIDorName")] //All user loan details
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> getRejectedLoanDetailsDCIDorName(string DCIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.getRejectedLoanDetailsDCIDorName(DCIDorName);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetRejectedLoanDetails")]//All user loan details
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetRejectedLoanDetails()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetRejectedLoanDetails();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("getpendingloandetails")]//All user loan details
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetNotApproveLoanDetails()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetNotApproveLoanDetails();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetLoansAmountTransferPendingByDCIDorName")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferPendingByDCIDorName(string DCIDorName)
        {
          Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetLoansAmountTransferPendingByDCIDorName(DCIDorName);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }
        [Authorize]
        [Route("GetLoansAmountTransferedByDCIDorName")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferedByDCIDorName(string DCIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetLoansAmountTransferedByDCIDorName(DCIDorName);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }
        [Authorize]
        [Route("GetLoansAmountTransferRejectedByDCIDorName")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferRejectedByDCIDorName(string DCIDorName)
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetLoansAmountTransferRejectedByDCIDorName(DCIDorName);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }
        [Authorize]
        [Route("GetLoansAmountTransferPending")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferPending()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetLoansAmountTransferPending();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }
        [Authorize]
        [Route("GetLoansAmountTransfered")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransfered()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetLoansAmountTransfered();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetLoansAmountTransferRejected")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferRejected()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetLoansAmountTransferRejected();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetLoansAmountTransferPendingByUserID")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferPendingByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetLoansAmountTransferPendingByUserID();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetLoansAmountTransferedByUserID")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferedByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetLoansAmountTransferedByUserID();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("GetLoansAmountTransferRejectedByUserID")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<LoansViewModel>>> GetLoansAmountTransferRejectedByUserID()
        {
            Tuple<bool, string, List<LoansViewModel>> result = null;
            try
            {
                result = await _repoLoans.GetLoansAmountTransferRejectedByUserID();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("personalloan")]
        [HttpPost]
        public async Task<Tuple<bool, string>> ApplyPersonalLoan(LoansModel applyPLLoansModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repoLoans.ApplyPersonalLoan(applyPLLoansModel);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("approvedorrejectloan")]
        [HttpPost]
        public async Task<Tuple<bool, string>> LoanApprovedOrRejected(LoansModel approvedLoansStatusModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repoLoans.LoanApprovedOrRejected(approvedLoansStatusModel);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("LoanTransferApprovedOrRejected")]
        [HttpPost]
        public async Task<Tuple<bool, string>> LoanTransferApprovedOrRejected(LoansModel obj)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repoLoans.LoanTransferApprovedOrRejected(obj);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("feereimbursement")]
        [HttpPost]
        public async Task<Tuple<bool, string>> ApplyFeeReimbursement()
        {
            FeeReimbursementModel applyFeeReimbursementModel = new FeeReimbursementModel();
            string fileUploadPath = ConfigurationManager.AppSettings["FileUploadPath"].ToString();
            Tuple<bool, string> resTuple = null;
            string filePath = string.Empty;
            string[] headerColumns = new string[0];
            Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            try
            {
                if (!Directory.Exists(fileUploadPath))
                    Directory.CreateDirectory(fileUploadPath);
                else
                {
                    Array.ForEach(Directory.GetFiles(fileUploadPath),
                    delegate (string path)
                    {
                        File.Delete(path);
                    });
                }

                var provider = new MultipartFormDataStreamProvider(fileUploadPath);
                var result = await Request.Content.ReadAsMultipartAsync(provider);

                if (result.FormData == null)
                    return Tuple.Create(false, "BadRequest");

                var formData = result.FormData;
                foreach (var prop in typeof(FeeReimbursementModel).GetProperties())
                {
                    var curVal = formData[prop.Name];
                    if (curVal != null && !string.IsNullOrEmpty(curVal))
                    {
                        prop.SetValue(applyFeeReimbursementModel, To(curVal, prop.PropertyType), null);
                    }
                }

                if (UserID == null)
                    return Tuple.Create(false, "User not Authenticated");

                if (result.FileData.Count > 0)
                    resTuple = await _repoLoans.ApplyFeeReimbursement(applyFeeReimbursementModel, result.FileData.ToList(), fileUploadPath);
                else
                    resTuple = Tuple.Create(false, "There has been an error while applying fee reimbursement.");
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return resTuple;
        }

        [Authorize]
        [Route("healthbenefits")]
        [HttpPost]
        public async Task<Tuple<bool, string>> ApplyHealthBenefits()
        {
            HealthBenefitModel applyHealthBenefitModel = new HealthBenefitModel();
            string fileUploadPath = ConfigurationManager.AppSettings["FileUploadPath"].ToString();
            Tuple<bool, string> resTuple = null;
            string filePath = string.Empty;
            string[] headerColumns = new string[0];
            Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            try
            {
                if (!Directory.Exists(fileUploadPath))
                    Directory.CreateDirectory(fileUploadPath);
                else
                {
                    Array.ForEach(Directory.GetFiles(fileUploadPath),
                    delegate (string path)
                    {
                        File.Delete(path);
                    });
                }

                var provider = new MultipartFormDataStreamProvider(fileUploadPath);
                var result = await Request.Content.ReadAsMultipartAsync(provider);

                if (result.FormData == null)
                    return Tuple.Create(false, "BadRequest");

                var formData = result.FormData;
                foreach (var prop in typeof(HealthBenefitModel).GetProperties())
                {
                    var curVal = formData[prop.Name];
                    if (curVal != null && !string.IsNullOrEmpty(curVal))
                    {
                        prop.SetValue(applyHealthBenefitModel, To(curVal, prop.PropertyType), null);
                    }
                }

                if (UserID == null)
                    return Tuple.Create(false, "User not Authenticated");

                if (result.FileData.Count > 0)
                    resTuple = await _repoLoans.ApplyHealthBenefits(applyHealthBenefitModel, result.FileData.ToList(), fileUploadPath);
                else
                    resTuple = Tuple.Create(false, "There has been an error while applying fee reimbursement.");
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return resTuple;
        }

        [Authorize]
        [Route("riskbenefits")]
        [HttpPost]
        public async Task<Tuple<bool, string>> ApplyRiskBenefits()
        {
            RiskBenefitModel applyRiskBenefitModel = new RiskBenefitModel();
            string fileUploadPath = ConfigurationManager.AppSettings["FileUploadPath"].ToString();
            Tuple<bool, string> resTuple = null;
            string filePath = string.Empty;
            string[] headerColumns = new string[0];
            Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            try
            {
                if (!Directory.Exists(fileUploadPath))
                    Directory.CreateDirectory(fileUploadPath);
                else
                {
                    Array.ForEach(Directory.GetFiles(fileUploadPath),
                    delegate (string path)
                    {
                        File.Delete(path);
                    });
                }

                var provider = new MultipartFormDataStreamProvider(fileUploadPath);
                var result = await Request.Content.ReadAsMultipartAsync(provider);

                if (result.FormData == null)
                    return Tuple.Create(false, "BadRequest");

                var formData = result.FormData;
                foreach (var prop in typeof(RiskBenefitModel).GetProperties())
                {
                    var curVal = formData[prop.Name];
                    if (curVal != null && !string.IsNullOrEmpty(curVal))
                    {
                        prop.SetValue(applyRiskBenefitModel, To(curVal, prop.PropertyType), null);
                    }
                }

                if (UserID == null)
                    return Tuple.Create(false, "User not Authenticated");

                if (result.FileData.Count > 0)
                    resTuple = await _repoLoans.ApplyRiskBenefits(applyRiskBenefitModel, result.FileData.ToList(), fileUploadPath);
                else
                    resTuple = Tuple.Create(false, "There has been an error while applying fee reimbursement.");
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return resTuple;
        }

        [Authorize]
        [Route("homeloan")]
        [HttpPost]
        public async Task<Tuple<bool, string>> ApplyHomeLoan(LoansModel applyHLLoansModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repoLoans.ApplyHomeLoan(applyHLLoansModel);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("downloadfile")]
        [HttpPost]
        public async Task<Tuple<bool, string, LoanDetailsViewModel>> DownloadDocument(LoanDetailsViewModel loanDetailsViewModel)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            Tuple<bool, string, LoanDetailsViewModel> resultView = null;
            try
            {
                resultView = await _repoLoans.DownloadDocument(loanDetailsViewModel);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }

            return resultView;
        }

        [Authorize]
        [Route("checkpersonalloan")]
        [HttpGet]
        public async Task<Tuple<bool, string>> CheckPersonalLoan()
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.CheckPersonalLoan();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("checkfeereimbursement")]
        [HttpGet]
        public async Task<Tuple<bool, string>> CheckFeeReimbursement()
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.CheckFeeReimbursement();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("checkhealthbenefit")]
        [HttpGet]
        public async Task<Tuple<bool, string>> CheckHealthLoan()
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.CheckHealthLoan();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("riskbenefit")]
        [HttpGet]
        public async Task<Tuple<bool, string>> CheckRiskBenefit()
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.CheckRiskBenefit();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("checkhomeloan")]
        [HttpGet]
        public async Task<Tuple<bool, string>> CheckHomeLoan()
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.CheckHomeLoan();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("CheckRenewalStatus")]
        [HttpGet]
        public async Task<Tuple<bool>> CheckRenewalStatus()
        {
            Tuple<bool> result = null;
            try
            {
                result = await _repo.CheckRenewalStatus();
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }
            return result;
        }

        private object To(IConvertible obj, Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);

            if (u != null)
            {
                return (obj == null) ? GetDefaultValue(t) : Convert.ChangeType(obj, u);
            }
            else
            {
                return Convert.ChangeType(obj, t);
            }
        }

        private object GetDefaultValue(Type t)
        {
            if (t.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(t);
            }
            return null;
        }

    }
}
