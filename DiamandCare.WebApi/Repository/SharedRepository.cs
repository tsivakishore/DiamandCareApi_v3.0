using Dapper;
using DiamandCare.WebApi.Models;
using DiamandCare.WebApi.Properties;
using DiamandCare.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DiamandCare.Core;

namespace DiamandCare.WebApi.Repository
{
    public class SharedRepository
    {
        private string _dvDb = Settings.Default.DiamandCareConnection;


        public async Task<Tuple<bool, string, List<State>>> GetState()
        {
            Tuple<bool, string, List<State>> result = null;
            List<State> lstErrorLogs = new List<State>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<State>("[dbo].[Select_State]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstErrorLogs = list as List<State>;
                    con.Close();
                }
                if (lstErrorLogs != null && lstErrorLogs.Count > 0)
                    result = Tuple.Create(true, "", lstErrorLogs);
                else
                    result = Tuple.Create(false, "No records found", lstErrorLogs);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstErrorLogs);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<SchoolModel>>> GetSchool()
        {
            Tuple<bool, string, List<SchoolModel>> result = null;
            List<SchoolModel> lstSchoolDetails = new List<SchoolModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<SchoolModel>("[dbo].[Select_School]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstSchoolDetails = list as List<SchoolModel>;

                    lstSchoolDetails = list.Select(x => new SchoolModel
                    {
                        SchoolID = x.SchoolID,
                        SchoolName = x.SchoolName + " (" + x.BranchCode + ")"
                    }).ToList();


                    con.Close();
                }
                if (lstSchoolDetails != null && lstSchoolDetails.Count > 0)
                    result = Tuple.Create(true, "", lstSchoolDetails);
                else
                    result = Tuple.Create(false, "No records found", lstSchoolDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstSchoolDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<SourceOfUserModel>>> GetSourceOfUser()
        {
            Tuple<bool, string, List<SourceOfUserModel>> result = null;
            List<SourceOfUserModel> lstSourceOfUser = new List<SourceOfUserModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<SourceOfUserModel>("[dbo].[Select_SourceofUser]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstSourceOfUser = list as List<SourceOfUserModel>;

                    lstSourceOfUser = list.Select(x => new SourceOfUserModel
                    {
                        SourceID = x.SourceID,
                        SourceName = x.SourceName
                    }).ToList();


                    con.Close();
                }
                if (lstSourceOfUser != null && lstSourceOfUser.Count > 0)
                    result = Tuple.Create(true, "", lstSourceOfUser);
                else
                    result = Tuple.Create(false, "No records found", lstSourceOfUser);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstSourceOfUser);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<AccountTypesViewModel>>> GetAccountTypes()
        {
            Tuple<bool, string, List<AccountTypesViewModel>> result = null;
            List<AccountTypesViewModel> lstAccountTypes = new List<AccountTypesViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<AccountTypesViewModel>("[dbo].[Select_SourceofUser]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstAccountTypes = list as List<AccountTypesViewModel>;

                    lstAccountTypes = list.Select(x => new AccountTypesViewModel
                    {
                        AccountTypeID = x.AccountTypeID,
                        AccountTypeName = x.AccountTypeName
                    }).ToList();


                    con.Close();
                }
                if (lstAccountTypes != null && lstAccountTypes.Count > 0)
                    result = Tuple.Create(true, "", lstAccountTypes);
                else
                    result = Tuple.Create(false, "No records found", lstAccountTypes);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstAccountTypes);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<LoanTransferStatus>>> GetLoanTransferStatus()
        {
            Tuple<bool, string, List<LoanTransferStatus>> result = null;
            List<LoanTransferStatus> lstLoanTransferStatus = new List<LoanTransferStatus>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<LoanTransferStatus>("[dbo].[Select_TransferStatusTypes]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstLoanTransferStatus = list as List<LoanTransferStatus>;

                    lstLoanTransferStatus = list.Select(x => new LoanTransferStatus
                    {
                        ID = x.ID,
                        Status = x.Status
                    }).ToList();

                    con.Close();
                }
                if (lstLoanTransferStatus != null && lstLoanTransferStatus.Count > 0)
                    result = Tuple.Create(true, "", lstLoanTransferStatus);
                else
                    result = Tuple.Create(false, "No records found", lstLoanTransferStatus);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstLoanTransferStatus);
            }
            return result;
        }
        public async Task<Tuple<bool, string, List<NomineeRelationshipViewModel>>> GetNomineeRelations()
        {
            Tuple<bool, string, List<NomineeRelationshipViewModel>> result = null;
            List<NomineeRelationshipViewModel> lstNomineeRelations = new List<NomineeRelationshipViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<NomineeRelationshipViewModel>("[dbo].[Select_NomineeRelations]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstNomineeRelations = list as List<NomineeRelationshipViewModel>;

                    lstNomineeRelations = list.Select(x => new NomineeRelationshipViewModel
                    {
                        NomineeRelationshipID = x.NomineeRelationshipID,
                        NomineeRelations = x.NomineeRelations
                    }).ToList();


                    con.Close();
                }
                if (lstNomineeRelations != null && lstNomineeRelations.Count > 0)
                    result = Tuple.Create(true, "", lstNomineeRelations);
                else
                    result = Tuple.Create(false, "No records found", lstNomineeRelations);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstNomineeRelations);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<BanksModel>>> GetBanks()
        {
            Tuple<bool, string, List<BanksModel>> result = null;
            List<BanksModel> lstBanks = new List<BanksModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<BanksModel>("[dbo].[Select_Banks]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstBanks = list as List<BanksModel>;

                    lstBanks = list.Select(x => new BanksModel
                    {
                        BankID = x.BankID,
                        BankName = x.BankName
                    }).ToList();


                    con.Close();
                }
                if (lstBanks != null && lstBanks.Count > 0)
                    result = Tuple.Create(true, "", lstBanks);
                else
                    result = Tuple.Create(false, "No records found", lstBanks);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "No records found", lstBanks);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<ModeofTransfer>>> GetModeofTransfer()
        {
            Tuple<bool, string, List<ModeofTransfer>> result = null;
            List<ModeofTransfer> lstModeofTransfer = new List<ModeofTransfer>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<ModeofTransfer>("[dbo].[Select_ModeofTransfer]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstModeofTransfer = list as List<ModeofTransfer>;

                    lstModeofTransfer = list.Select(x => new ModeofTransfer
                    {
                        ID = x.ID,
                        Mode = x.Mode
                    }).ToList();

                    con.Close();
                }
                if (lstModeofTransfer != null && lstModeofTransfer.Count > 0)
                    result = Tuple.Create(true, "", lstModeofTransfer);
                else
                    result = Tuple.Create(false, "No records found", lstModeofTransfer);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "No records found", lstModeofTransfer);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<FranchiseRequestStaus>>> GetFranchiseRequestStaus()
        {
            Tuple<bool, string, List<FranchiseRequestStaus>> result = null;
            List<FranchiseRequestStaus> lstFranchiseRequestStaus = new List<FranchiseRequestStaus>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<FranchiseRequestStaus>("[dbo].[Select_FranchiseRequestStaus]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstFranchiseRequestStaus = list as List<FranchiseRequestStaus>;

                    lstFranchiseRequestStaus = list.Select(x => new FranchiseRequestStaus
                    {
                        StatusID = x.StatusID,
                        Status = x.Status
                    }).ToList();

                    con.Close();
                }
                if (lstFranchiseRequestStaus != null && lstFranchiseRequestStaus.Count > 0)
                    result = Tuple.Create(true, "", lstFranchiseRequestStaus);
                else
                    result = Tuple.Create(false, "No records found", lstFranchiseRequestStaus);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "No records found", lstFranchiseRequestStaus);
            }
            return result;
        }
        public async Task<Tuple<bool, string>> SendSMS(string PhoneNumber, string RegKey)
        {
            Tuple<bool, string> result = null;
            string msgBody = "Your one time registration key: " + RegKey;

            string res = string.Empty;
            try
            {
                string url = "http://bulksms.mysmsmantra.com:8080/WebSMS/SMSAPI.jsp?username=sivakishore&password=1174306098&sendername=SFEOrg&mobileno=" + PhoneNumber + "&message=" + msgBody;
                res = getHTTP(url.Trim());
                if (res.Contains("Your message is successfully sent"))
                {
                    result = Tuple.Create(true, "Sent secret key successfully.");
                }
                else
                    result = Tuple.Create(false, "Sent secret key failed.");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Sent secret key failed.");
            }
            return result;
        }

        public async Task<Tuple<bool, string>> UpdatePhonenumber(string PhoneNumber, string RegKey)
        {
            int updatedStatus = -1;
            Tuple<bool, string> updatePhonenumber = null;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dvDb))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@RegKey", RegKey);
                    parameters.Add("@PhoneNumber", PhoneNumber, DbType.String);

                    updatedStatus = await cxn.ExecuteScalarAsync<int>("dbo.Update_RegKeyPhonenumber", parameters, commandType: CommandType.StoredProcedure);

                    if (updatedStatus == 0)
                        updatePhonenumber = Tuple.Create(true, "");
                    else
                        updatePhonenumber = Tuple.Create(false, "");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                updatePhonenumber = Tuple.Create(false, "");
            }

            return updatePhonenumber;
        }
        public async Task<Tuple<bool, string, string>> VerifySecretKey(RegisterKey obj)
        {
            Tuple<bool, string, string> objKey = null;
            RegisterKey regkey = new RegisterKey();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dvDb))
                {
                    parameters.Add("@PhoneNumber", obj.PhoneNumber, DbType.String);
                    parameters.Add("@RegKey", obj.RegKey, DbType.String);
                    var resultObj = await cxn.QueryAsync<RegisterKey>("dbo.Validate_RegisterKey", parameters, commandType: CommandType.StoredProcedure);
                    regkey = resultObj.Single() as RegisterKey;

                    if (!string.IsNullOrEmpty(regkey.RegKey))
                    {
                        objKey = Tuple.Create(true, "", regkey.KeyType);
                    }
                    else
                        objKey = Tuple.Create(false, "Oops! Please enter valid key and mobile number.", "");

                    cxn.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                objKey = Tuple.Create(false, "Oops! Please enter valid key and mobile number.", "");
            }

            return objKey;
        }

        public async Task<Tuple<bool, string, SponserViewModel>> GetSponserDetails(CommonModel objCommon)
        {
            Tuple<bool, string, SponserViewModel> resultSponserDetails = null;

            SponserViewModel lstSponserDetails = new SponserViewModel();
            SponserModel sponserDetails = new SponserModel();
            List<UnderModel> lstUnderModel = new List<UnderModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dvDb))
                {
                    parameters.Add("@DcID", objCommon.DcID, DbType.String);

                    using (var multi = await cxn.QueryMultipleAsync("dbo.Select_SponserDetails", parameters, commandType: CommandType.StoredProcedure))
                    {
                        sponserDetails = multi.Read<SponserModel>().Single();
                        lstUnderModel = multi.Read<UnderModel>().ToList();
                        lstSponserDetails.SponserDetails = sponserDetails;
                        lstSponserDetails.lstUnderDetails = lstUnderModel;
                    }

                    if (lstSponserDetails.SponserDetails != null && lstSponserDetails.lstUnderDetails.Count > 0)
                        resultSponserDetails = Tuple.Create(true, "", lstSponserDetails);
                    else
                        resultSponserDetails = Tuple.Create(false, "Please provide correct sponser id.", lstSponserDetails);

                    cxn.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                resultSponserDetails = Tuple.Create(false, "Oops! No record found.", lstSponserDetails);
            }

            return resultSponserDetails;
        }

        public string getHTTP(string szURL)
        {
            HttpWebRequest HttpRequest;
            HttpWebResponse httpResponse;
            string BodtText = null;
            Int32 Bytes;
            Stream ResponseStream;
            byte[] RecvByte = new byte[byte.MaxValue + 1];

            HttpRequest = (HttpWebRequest)WebRequest.Create(szURL);

            httpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            ResponseStream = httpResponse.GetResponseStream();

            while ((true))
            {
                Bytes = ResponseStream.Read(RecvByte, 0, RecvByte.Length);
                if (Bytes <= 0)
                    break;
                BodtText += System.Text.Encoding.UTF8.GetString
                (RecvByte, 0, Bytes);
            }
            return BodtText;
        }

    }
}
