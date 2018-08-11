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
    public class FranchiseRepository
    {
        private string _dcDb = Settings.Default.DiamandCareConnection;
        int userID;

        public FranchiseRepository()
        {
            userID = Helper.FindUserByID().UserID;
        }

        public async Task<Tuple<bool, string, List<FranchiseMaster>>> GetFranchiseMasterDetails()
        {
            Tuple<bool, string, List<FranchiseMaster>> result = null;
            List<FranchiseMaster> lstDetails = new List<FranchiseMaster>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<FranchiseMaster>("[dbo].[Select_FranchiseMaster]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstDetails = list as List<FranchiseMaster>;
                    con.Close();
                }

                if (lstDetails != null && lstDetails.Count() > 0)
                {
                    result = Tuple.Create(true, "", lstDetails);
                }
                else
                    result = Tuple.Create(false, "No records found", lstDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, FranchiseMaster>> UpdateFranchise(FranchiseMaster obj)
        {
            Tuple<bool, string, FranchiseMaster> objKey = null;
            FranchiseMaster franchiseData = new FranchiseMaster();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@ID", obj.ID, DbType.Int32);
                    parameters.Add("@FranchiseType", obj.FranchiseType, DbType.String);
                    parameters.Add("@PaymentReceiptPercentage", obj.PaymentReceiptPercentage, DbType.Decimal);
                    parameters.Add("@TargetJoineesPerMonth", obj.TargetJoineesPerMonth, DbType.Int32);
                    parameters.Add("@MinimumJoineesAvg", obj.MinimumJoineesAvg, DbType.Int32);
                    parameters.Add("@CreatedBy", userID, DbType.Int32);

                    var resultObj = await cxn.QueryAsync<FranchiseMaster>("dbo.Update_FranchiseMaster", parameters, commandType: CommandType.StoredProcedure);
                    franchiseData = resultObj.Single() as FranchiseMaster;

                    cxn.Close();
                }
                if (obj.ID != 0)
                    objKey = Tuple.Create(true, "Franchise data updated successfully.", franchiseData);
                else
                    objKey = Tuple.Create(true, "Franchise data updated successfully.", franchiseData);
            }
            catch (Exception ex)
            {
                // ErrorLog.Write(ex);
                objKey = Tuple.Create(false, "Oops! Franchise data update failed.Please try again.", franchiseData);
            }

            return objKey;
        }

        public async Task<Tuple<bool, string, List<UpgradeTo>>> GetUpgradeTo()
        {
            Tuple<bool, string, List<UpgradeTo>> result = null;
            List<UpgradeTo> lstDetails = new List<UpgradeTo>();

            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<UpgradeTo>("[dbo].[Select_UpgradeTo]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstDetails = list as List<UpgradeTo>;
                    con.Close();
                }

                if (lstDetails != null && lstDetails.Count() > 0)
                {
                    result = Tuple.Create(true, "", lstDetails);
                }
                else
                    result = Tuple.Create(false, "No records found", lstDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, UserIDNameModel>> GetUsernameByDCIDorName(string DcIDorName)
        {
            Tuple<bool, string, UserIDNameModel> result = null;
            UserIDNameModel dataModel = new UserIDNameModel();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    parameters.Add("@DcIDorName", DcIDorName, DbType.String);
                    var list = await con.QueryAsync<UserIDNameModel>("[dbo].[Select_UsernameByDCIDorName]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    dataModel = list.Single() as UserIDNameModel;
                    con.Close();
                }

                if (dataModel.UserName != null)
                {
                    result = Tuple.Create(true, "", dataModel);
                }
                else
                    result = Tuple.Create(false, "No records found", dataModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "No records found", dataModel);
            }
            return result;
        }
        public async Task<Tuple<bool, string, List<UserIDNameModel>, FranchiseMaster>> GetUnderFranchiseDetails(int FranchiseTypeID)
        {
            Tuple<bool, string, List<UserIDNameModel>, FranchiseMaster> result = null;
            List<UserIDNameModel> dataModel = new List<UserIDNameModel>();
            FranchiseMaster fmaster = new FranchiseMaster();
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    //con.Open();
                    parameters.Add("@FranchiseTypeID", FranchiseTypeID, DbType.Int32);
                    //var list = await con.QueryAsync<UserIDNameModel>("[dbo].[Select_UnderFranchiseDetails]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    //dataModel = list as List<UserIDNameModel>;
                    //con.Close();
                    using (var multi = await con.QueryMultipleAsync("[dbo].[Select_UnderFranchiseDetails]", parameters, commandType: CommandType.StoredProcedure))
                    {
                        dataModel = multi.Read<UserIDNameModel>().ToList();
                        fmaster = multi.Read<FranchiseMaster>().SingleOrDefault();
                    }
                }

                if (dataModel != null && dataModel.Count > 0)
                    result = Tuple.Create(true, "", dataModel, fmaster);
                else
                    result = Tuple.Create(false, "No records found", dataModel, fmaster);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", dataModel, fmaster);
            }
            return result;
        }

        public async Task<Tuple<bool, string, Franchise>> InsertorUpdateFranchiseDetails(Franchise obj)
        {
            Tuple<bool, string, Franchise> objKey = null;
            Franchise franchiseData = new Franchise();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    if (obj.ID > 0)
                        parameters.Add("@ID", obj.ID, DbType.Int32);

                    parameters.Add("@UserID", obj.UserID, DbType.Int32);
                    parameters.Add("@FranchiseTypeID", obj.FranchiseTypeID, DbType.Int32);
                    parameters.Add("@ConditionsApplySelf", obj.ConditionsApplySelf, DbType.Boolean);
                    parameters.Add("@ConditionsApplyUnderJoinees", obj.ConditionsApplyUnderJoinees, DbType.Boolean);
                    parameters.Add("@FranchiseJoinees", obj.FranchiseJoinees, DbType.Int32);
                    parameters.Add("@FranchiseJoineesMinimum", obj.FranchiseJoineesMinimum, DbType.Int32);
                    parameters.Add("@UnderFranchiseID", obj.UnderFranchiseID, DbType.Int32);
                    parameters.Add("@CreatedBy", userID, DbType.Int32);

                    var resultObj = await cxn.QueryAsync<Franchise>("dbo.InsertorUpdate_Franchise", parameters, commandType: CommandType.StoredProcedure);
                    franchiseData = resultObj.Single() as Franchise;

                    cxn.Close();
                }
                if (obj.ID != 0)
                    objKey = Tuple.Create(true, "Franchise data updated successfully.", franchiseData);
                else
                    objKey = Tuple.Create(true, "Franchise data inserted successfully.", franchiseData);
            }
            catch (Exception ex)
            {
                // ErrorLog.Write(ex);
                objKey = Tuple.Create(false, "Oops! Franchise data update failed.Please try again.", franchiseData);
            }

            return objKey;
        }
        public async Task<Tuple<bool, string, List<FranchiseTypes>>> GetFranchiseTypes()
        {
            Tuple<bool, string, List<FranchiseTypes>> result = null;
            List<FranchiseTypes> lstDetails = new List<FranchiseTypes>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<FranchiseTypes>("[dbo].[Select_FranchiseType]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstDetails = list as List<FranchiseTypes>;
                    con.Close();
                }

                if (lstDetails != null && lstDetails.Count() > 0)
                {
                    result = Tuple.Create(true, "", lstDetails);
                }
                else
                    result = Tuple.Create(false, "No records found", lstDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, Franchises, Wallet>> GetFranchiseUsernameWalletByIDorName(string DcIDorName)
        {
            Tuple<bool, string, Franchises, Wallet> result = null;
            Franchises franchisesDetails = new Franchises();
            Wallet walletDetails = new Wallet();

            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@DcIDorName", DcIDorName);
                    con.Open();

                    using (var multi = await con.QueryMultipleAsync("dbo.Select_FranchiseIDandName_Wallet", parameters, commandType: CommandType.StoredProcedure))
                    {
                        franchisesDetails = multi.Read<Franchises>().Single();
                        walletDetails = multi.Read<Wallet>().Single();
                    }
                    con.Close();
                }
                if (franchisesDetails != null)
                    result = Tuple.Create(true, "", franchisesDetails, walletDetails);
                else
                    result = Tuple.Create(false, "No records found", franchisesDetails, walletDetails);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
                result = Tuple.Create(false, "No records found", franchisesDetails, walletDetails);
            }
            return result;
        }
    }
}