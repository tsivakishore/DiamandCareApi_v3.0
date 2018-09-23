using Dapper;
using DiamandCare.Core;
using DiamandCare.WebApi.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DiamandCare.WebApi
{
    public class FeeMasterRepository
    {
        private string _dcDb = Settings.Default.DiamandCareConnection;
        int UserID;

        public FeeMasterRepository()
        {
            UserID = Helper.FindUserByID().UserID;
        }

        public async Task<Tuple<bool, string, List<FeeMasterViewModel>>> GetFeeMasterDetails(int UserID)
        {
            Tuple<bool, string, List<FeeMasterViewModel>> result = null;
            List<FeeMasterViewModel> lstFeeMasterDetails = new List<FeeMasterViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    con.Open();
                    var list = await con.QueryAsync<FeeMasterViewModel>("[dbo].[Select_FeePerticularsDetails]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstFeeMasterDetails = list as List<FeeMasterViewModel>;
                    con.Close();
                }

                if (lstFeeMasterDetails != null && lstFeeMasterDetails.Count() > 0)
                    result = Tuple.Create(true, "", lstFeeMasterDetails);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstFeeMasterDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstFeeMasterDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string>> CreateFeeMaster(FeeMasterModel feeMasterModel)
        {
            Tuple<bool, string> result = null;
            int insertStatus = -1;
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    if (feeMasterModel.FeeMasterID > 0)
                        parameters.Add("@FeeMasterID", feeMasterModel.FeeMasterID, DbType.Int32);

                    parameters.Add("@UserID", feeMasterModel.UserID, DbType.Int32);
                    parameters.Add("@CourseID", feeMasterModel.CourseID, DbType.Int32);
                    parameters.Add("@CourseFee", feeMasterModel.CourseFee, DbType.Decimal);
                    parameters.Add("@CreatedBy", UserID, DbType.Int32);
                    cxn.Open();
                    insertStatus = await cxn.ExecuteScalarAsync<int>("dbo.InsertOrUpdate_FeePerticulars", parameters, commandType: CommandType.StoredProcedure);
                    cxn.Close();
                }

                if (insertStatus == 0)
                {
                    if (feeMasterModel.FeeMasterID == 0)
                        result = Tuple.Create(true, "Fee master details created successfully");
                    else if (feeMasterModel.FeeMasterID > 0)
                        result = Tuple.Create(true, "Fee master details updated successfully");
                }
                else
                    result = Tuple.Create(false, "Fee master details created failed");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message);
            }

            return result;
        }
    }
}