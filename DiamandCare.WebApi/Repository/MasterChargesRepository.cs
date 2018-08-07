using Dapper;
using DiamandCare.WebApi.Models;
using DiamandCare.WebApi.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DiamandCare.Core;

namespace DiamandCare.WebApi.Repository
{

    public class MasterChargesRepository
    {
        private string _dcDb = Settings.Default.DiamandCareConnection;

        public async Task<Tuple<bool, string, MasterChargesModel>> AddMasterCharges(MasterChargesModel obj)
        {
            int status = -1;

            Tuple<bool, string, MasterChargesModel> objMasterCharges = null;
            MasterChargesModel masterCharges = new MasterChargesModel();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@DocumentationAdminFee", obj.DocumentationAdminFee, DbType.Decimal);
                    parameters.Add("@DocumentationAdminFee1", obj.DocumentationAdminFee1, DbType.Decimal);
                    parameters.Add("@PrepaidLoanCharges", obj.PrepaidLoanCharges, DbType.Decimal);
                    parameters.Add("@RegistrationCharges", obj.RegistrationCharges, DbType.Decimal);
                    parameters.Add("@AreaFee", obj.AreaFee, DbType.Decimal);
                    parameters.Add("@DistrictFee", obj.DistrictFee, DbType.Decimal);
                    parameters.Add("@DistrictClusterFee", obj.DistrictClusterFee, DbType.Decimal);
                    parameters.Add("@StateFee", obj.StateFee, DbType.Decimal);
                    parameters.Add("@StateClusterFee", obj.StateClusterFee, DbType.Decimal);
                    parameters.Add("@MotherFee", obj.MotherFee, DbType.Decimal);
                    parameters.Add("@SGST", obj.SGST, DbType.Decimal);
                    parameters.Add("@CGST", obj.CGST, DbType.Decimal);
                    parameters.Add("@IGST", obj.IGST, DbType.Decimal);
                    parameters.Add("@TDS", obj.TDS, DbType.Decimal);
                    parameters.Add("@CreatedBy", Helper.FindUserByID().Id, DbType.String);
                    parameters.Add("@CreatedOn", DateTime.Now, DbType.DateTime);

                    masterCharges = await cxn.QuerySingleAsync<MasterChargesModel>("dbo.Insert_MasterCharges", parameters, commandType: CommandType.StoredProcedure);
                    if (masterCharges != null)
                        objMasterCharges = Tuple.Create(true, "Master charges added successfully.", masterCharges);
                    else
                        objMasterCharges = Tuple.Create(false, "Oops! Master charges added failed.Please try again.", masterCharges);

                    cxn.Close();
                }

            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                objMasterCharges = Tuple.Create(false, "Oops! Master charges added failed.Please try again.", masterCharges);
            }

            return objMasterCharges;
        }

        public async Task<Tuple<bool, string, MasterChargesModel>> GetMasterCharges()
        {
            Tuple<bool, string, MasterChargesModel> result = null;
            MasterChargesModel masterCharges = new MasterChargesModel();
            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    var mCharges = await con.QueryAsync<MasterChargesModel>("[dbo].[Select_MasterCharges]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    masterCharges = mCharges.FirstOrDefault();
                    con.Close();
                }
                if (masterCharges != null)
                    result = Tuple.Create(true, "", masterCharges);
                else
                    result = Tuple.Create(false, "No records found", masterCharges);
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
                result = Tuple.Create(false, "", masterCharges);
            }
            return result;
        }
    }
}
