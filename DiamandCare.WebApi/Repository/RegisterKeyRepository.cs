using Dapper;
using DiamandCare.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DiamandCare.WebApi.Properties;
using System.Threading;
using DiamandCare.Core;

namespace DiamandCare.WebApi.Repository
{
    public class RegisterKeyRepository
    {
        string _dcDb = Settings.Default.DiamandCareConnection;
        string userID = string.Empty;
        int UserID = 0;

        public RegisterKeyRepository()
        {
            userID = Helper.FindUserByID().Id;
            UserID = Helper.FindUserByID().UserID;
        }
        public async Task<Tuple<bool, string, RegisterKey>> RegisterKeyGenearation(RegisterKey obj)
        {

            Tuple<bool, string, RegisterKey> objKey = null;
            RegisterKey regkey = new RegisterKey();

            try
            {
                var regKey = Helper.GenerateSecretKey();

                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@PhoneNumber", obj.PhoneNumber, DbType.String);
                    parameters.Add("@RegKey", regKey, DbType.String);
                    parameters.Add("@CreatedBy", userID, DbType.String);
                    parameters.Add("@CreatedDate", DateTime.Now, DbType.DateTime);
                    parameters.Add("@RegKeyStatus", "Issued", DbType.String);

                    var resultObj = await cxn.QueryAsync<RegisterKey>("dbo.Insert_RegisterKey", parameters, commandType: CommandType.StoredProcedure);
                    regkey = resultObj.Single() as RegisterKey;

                    cxn.Close();
                }
                objKey = Tuple.Create(true, "Register key generated successfully.", regkey);
            }
            catch (Exception ex)
            {
                // ErrorLog.Write(ex);
                objKey = Tuple.Create(false, "Oops! Register key generation failed.Please try again.", regkey);
            }

            return objKey;
        }

        public async Task<Tuple<bool, string>> GenerateMultipleRegisterKeys(MultipleRegisterKey multipleKeys)
        {
            Tuple<bool, string> resultKey = null;
            MultipleRegisterKey regkey = new MultipleRegisterKey();
            int insertStatus = -1;
            DataTable dtName = null;

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@UserID", multipleKeys.UserID, DbType.Int32);
                    parameters.Add("@KeyType", multipleKeys.KeyType, DbType.String);
                    parameters.Add("@NoOfKeys", multipleKeys.NoOfKeys, DbType.Int32);
                    parameters.Add("@IsWallet", multipleKeys.IsWallet, DbType.Boolean);
                    parameters.Add("@MultiKeysTable", CreateTableMultipleRegKeys(multipleKeys.NoOfKeys, multipleKeys.UserID, dtName).AsTableValuedParameter());

                    insertStatus = await cxn.ExecuteScalarAsync<int>("dbo.Insert_MultipleRegisterKey", parameters, commandType: CommandType.StoredProcedure);

                    cxn.Close();
                }

                if (insertStatus == 0)
                    resultKey = Tuple.Create(true, "Multiple register keys generated successfully.");
                else
                    Tuple.Create(false, "Oops! Multiple register keys generation failed.Please try again.");
            }
            catch (Exception ex)
            {
                // ErrorLog.Write(ex);
                resultKey = Tuple.Create(false, "Oops! " + ex.Message + ".Please try again.");
            }

            return resultKey;
        }

        private DataTable CreateTableMultipleRegKeys(int NoOfKeys, int ToUserID, DataTable dt)
        {
            dt = new DataTable();
            dt.Columns.Add("RegKey", typeof(string));
            dt.Columns.Add("RegKeyStatus", typeof(string));
            dt.Columns.Add("CreatedBy", typeof(string));
            dt.Columns.Add("ToUserID", typeof(int));

            for (int i = 0; i < NoOfKeys; i++)
            {
                DataRow newDataRow = null;
                Thread.Sleep(500);
                newDataRow = ReturnDataRow(Helper.GenerateSecretKey(), "Issued", userID, ToUserID, dt);
                dt.Rows.Add(newDataRow);
            }

            return dt;
        }

        private DataRow ReturnDataRow(string RegKey, string RegKeyStatus, string CreatedBy, int ToUserID, DataTable dtTable)
        {
            DataRow row = null;
            row = dtTable.NewRow();
            row["RegKey"] = RegKey;
            row["RegKeyStatus"] = RegKeyStatus;
            row["CreatedBy"] = CreatedBy;
            row["ToUserID"] = ToUserID;
            return row;
        }
        public async Task<Tuple<bool, string, List<RegisterKey>>> GetIssuedRegisterKeys()
        {
            Tuple<bool, string, List<RegisterKey>> result = null;
            List<RegisterKey> lstKeys = new List<RegisterKey>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<RegisterKey>("[dbo].[Select_IssuedRegisterKeys]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstKeys = list.Select(x => new RegisterKey
                    {
                        RegKey = x.RegKey,
                        PhoneNumber = x.PhoneNumber,
                        RegKeyStatus = x.RegKeyStatus,
                        CreatedBy = x.CreatedBy,
                        CreateDate = x.CreateDate,
                        KeyType = x.KeyType,
                        KeyCost = x.KeyCost
                    }).ToList();

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

        public async Task<Tuple<bool, string, List<RegisterKey>>> GetIssuedRegisterKeysByUserID()
        {
            Tuple<bool, string, List<RegisterKey>> result = null;
            List<RegisterKey> lstKeys = new List<RegisterKey>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@userID", UserID, DbType.Int32);
                    con.Open();
                    var list = await con.QueryAsync<RegisterKey>("[dbo].[Select_IssuedRegisterKeysByUserID]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstKeys = list.Select(x => new RegisterKey
                    {
                        ToUserID = x.ToUserID,
                        RegKey = x.RegKey,
                        PhoneNumber = x.PhoneNumber,
                        RegKeyStatus = x.RegKeyStatus,
                        CreatedBy = x.CreatedBy,
                        CreateDate = x.CreateDate,
                        KeyType = x.KeyType,
                        KeyCost = x.KeyCost
                    }).ToList();

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
        public async Task<Tuple<bool, string, List<RegisterKey>>> GetUsedRegisterKeys()
        {
            Tuple<bool, string, List<RegisterKey>> result = null;
            List<RegisterKey> lstKeys = new List<RegisterKey>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<RegisterKey>("[dbo].[Select_UsedRegisterKeys]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstKeys = list as List<RegisterKey>;
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

        public async Task<Tuple<bool, string, MultipleSecreateKeys>> GetUsernameWalletMasterCharges(string DcIDorName)
        {
            Tuple<bool, string, MultipleSecreateKeys> result = null;
            Franchises franchisesDetails = new Franchises();
            Wallet walletDetails = new Wallet();
            MasterCharges masterChargesDetails = new MasterCharges();
            MultipleSecreateKeys lstMultipleSecreateKeys = new MultipleSecreateKeys();

            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@DcIDorName", DcIDorName);
                    con.Open();

                    using (var multi = await con.QueryMultipleAsync("dbo.Select_FranchiseIDandName_RegKey", parameters, commandType: CommandType.StoredProcedure))
                    {
                        franchisesDetails = multi.Read<Franchises>().Single();
                        walletDetails = multi.Read<Wallet>().Single();
                        masterChargesDetails = multi.Read<MasterCharges>().Single();
                        lstMultipleSecreateKeys.Franchise = franchisesDetails;
                        lstMultipleSecreateKeys.Wallet = walletDetails;
                        lstMultipleSecreateKeys.MasterCharges = masterChargesDetails;
                    }
                    con.Close();
                }
                if (lstMultipleSecreateKeys != null)
                    result = Tuple.Create(true, "", lstMultipleSecreateKeys);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstMultipleSecreateKeys);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstMultipleSecreateKeys);
            }
            return result;
        }

        public async Task<Tuple<bool, string, MultipleSecreateKeys>> GetUserWalletMasterCharges(string DcIDorName)
        {
            Tuple<bool, string, MultipleSecreateKeys> result = null;
            Wallet walletDetails = new Wallet();
            MasterCharges masterChargesDetails = new MasterCharges();
            MultipleSecreateKeys lstMultipleSecreateKeys = new MultipleSecreateKeys();

            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@DcIDorName", DcIDorName);
                    con.Open();

                    using (var multi = await con.QueryMultipleAsync("dbo.Select_User_WalletMasterCharges_RegKey", parameters, commandType: CommandType.StoredProcedure))
                    {
                        walletDetails = multi.Read<Wallet>().Single();
                        masterChargesDetails = multi.Read<MasterCharges>().Single();
                        lstMultipleSecreateKeys.Wallet = walletDetails;
                        lstMultipleSecreateKeys.MasterCharges = masterChargesDetails;
                    }
                    con.Close();
                }
                if (lstMultipleSecreateKeys != null)
                    result = Tuple.Create(true, "", lstMultipleSecreateKeys);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstMultipleSecreateKeys);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstMultipleSecreateKeys);
            }
            return result;
        }

        public async Task<Tuple<bool, string>> ShareRegisterKey(RegisterKey registerKey)
        {
            Tuple<bool, string> resultShareKey = null;
            int insertStatus = -1;

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@RegKey", registerKey.RegKey, DbType.String);
                    parameters.Add("@ToUserID", registerKey.ToUserID, DbType.Int32);
                    parameters.Add("@SharedUserID", registerKey.SharedUserID, DbType.Int32);
                    //parameters.Add("@CreatedBy", userID, DbType.String);

                    insertStatus = await cxn.ExecuteScalarAsync<int>("dbo.Update_ShareRegKey", parameters, commandType: CommandType.StoredProcedure);

                    cxn.Close();
                }

                if (insertStatus == 0)
                    resultShareKey = Tuple.Create(true, "Register key shared successfully.");
                else
                    Tuple.Create(false, "Register key shared failed.Please try again.");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                resultShareKey = Tuple.Create(false, "Oops! Register key shared failed.Please try again.");
            }

            return resultShareKey;
        }

        public async Task<Tuple<bool, string, List<RegisterKeyViewModel>>> GetSharedRegisterKeysByUserID()
        {
            Tuple<bool, string, List<RegisterKeyViewModel>> result = null;
            List<RegisterKeyViewModel> lstSharedKeys = new List<RegisterKeyViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@userID", UserID, DbType.Int32);
                    con.Open();
                    var list = await con.QueryAsync<RegisterKeyViewModel>("[dbo].[Select_SharedRegKeyByUserID]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstSharedKeys = list.Select(x => new RegisterKeyViewModel
                    {
                        ToUserID = x.ToUserID,
                        SharedTo = x.SharedTo,
                        RegKey = x.RegKey,
                        PhoneNumber = x.PhoneNumber,
                        RegKeyStatus = x.RegKeyStatus,
                        SharedOn = x.SharedOn,
                        KeyType = x.KeyType,
                        KeyCost = x.KeyCost
                    }).ToList();

                    con.Close();
                }
                if (lstSharedKeys != null && lstSharedKeys.Count > 0)
                    result = Tuple.Create(true, "", lstSharedKeys);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstSharedKeys);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstSharedKeys);
            }
            return result;
        }
    }
}
