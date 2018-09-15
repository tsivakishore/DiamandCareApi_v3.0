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
                    result = Tuple.Create(false, "No records found", lstDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstDetails);
            }
            return result;
        }

    }
}