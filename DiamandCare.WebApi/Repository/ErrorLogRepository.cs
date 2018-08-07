using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DiamandCare.WebApi.Models;
using Dapper;
using DiamandCare.WebApi.Properties;
using DiamandCare.Core;

namespace DiamandCare.WebApi.Repository
{
    public class ErrorLogRepository
    {
        private string _dvDb = Settings.Default.DiamandCareConnection;


        public async Task<Tuple<bool, string, List<ErrorLogViewModel>>> GetAllErrorLogs()
        {
            Tuple<bool, string, List<ErrorLogViewModel>> result = null;
            List<ErrorLogViewModel> lstErrorLogs = new List<ErrorLogViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<ErrorLogViewModel>("[dbo].[Select_ErrorLogsCount]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstErrorLogs = list as List<ErrorLogViewModel>;
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

        public async Task<Tuple<bool, string, List<ErrorLogViewModel>>> GetAllErrorLogsByAppName(string appName)
        {
            Tuple<bool, string, List<ErrorLogViewModel>> result = null;
            List<ErrorLogViewModel> lstErrorLogs = new List<ErrorLogViewModel>();
            try
            {
                DynamicParameters spParams = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dvDb))
                {
                    spParams.Add("@Application", appName);
                    var list = await con.QueryAsync<ErrorLogViewModel>("[dbo].[Select_ErrorLogs]", spParams, commandType: CommandType.StoredProcedure);
                    lstErrorLogs = list as List<ErrorLogViewModel>;
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
    }
}
