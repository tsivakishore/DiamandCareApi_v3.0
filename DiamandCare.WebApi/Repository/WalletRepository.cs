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
        private string _dvDb = Settings.Default.DiamandCareConnection;
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
                using (SqlConnection con = new SqlConnection(_dvDb))
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
                    result = Tuple.Create(false, "No records found", data);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", data);
            }
            return result;
        }
    }
}