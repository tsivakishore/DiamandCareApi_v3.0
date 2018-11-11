using Dapper;
using DiamandCare.Core;
using DiamandCare.WebApi.Models;
using DiamandCare.WebApi.Properties;
using DiamandCare.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DiamandCare.WebApi.Repository
{
    public class SchoolRepository
    {
        private string _dcDb = Settings.Default.DiamandCareConnection;
        int userID;

        public SchoolRepository()
        {
            userID = Helper.FindUserByID().UserID;
        }

        public async Task<Tuple<bool, string, List<SchoolViewModel>>> GetSchoolDetails()
        {
            Tuple<bool, string, List<SchoolViewModel>> result = null;
            List<SchoolViewModel> lstDetails = new List<SchoolViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<SchoolViewModel>("[dbo].[Select_SchoolDetails]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstDetails = list as List<SchoolViewModel>;
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

        public async Task<Tuple<bool, string, SchoolModel>> InsertSchoolDetails(SchoolModel obj)
        {
            Tuple<bool, string, SchoolModel> objKey = null;
            SchoolModel franchiseData = new SchoolModel();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@UserID", obj.UserID, DbType.Int32);
                    parameters.Add("@SchoolName", obj.SchoolName, DbType.String);
                    parameters.Add("@BranchCode", obj.BranchCode, DbType.String);
                    parameters.Add("@Address1", obj.Address1, DbType.String);
                    parameters.Add("@Address2", obj.Address2, DbType.String);
                    parameters.Add("@City", obj.City, DbType.String);
                    parameters.Add("@District", obj.District, DbType.String);
                    parameters.Add("@StateID", obj.StateID, DbType.Int32);
                    parameters.Add("@Country", obj.Country, DbType.String);
                    parameters.Add("@Zipcode", obj.Zipcode, DbType.String);
                    parameters.Add("@CreatedBy", userID, DbType.Int32);

                    var resultObj = await cxn.QueryAsync<SchoolModel>("dbo.Insert_School", parameters, commandType: CommandType.StoredProcedure);

                    cxn.Close();
                }

                objKey = Tuple.Create(true, "School data inserted successfully.", franchiseData);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                objKey = Tuple.Create(false, "Oops! School data update failed.Please try again.", franchiseData);
            }

            return objKey;
        }

        public async Task<Tuple<bool, string, SchoolModel>> UpdateSchoolDetails(SchoolModel obj)
        {
            Tuple<bool, string, SchoolModel> objKey = null;
            SchoolModel franchiseData = new SchoolModel();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@UserID", obj.UserID, DbType.Int32);
                    parameters.Add("@SchoolName", obj.SchoolName, DbType.String);
                    parameters.Add("@BranchCode", obj.BranchCode, DbType.String);
                    parameters.Add("@Address1", obj.Address1, DbType.String);
                    parameters.Add("@Address2", obj.Address2, DbType.String);
                    parameters.Add("@City", obj.City, DbType.String);
                    parameters.Add("@District", obj.District, DbType.String);
                    parameters.Add("@StateID", obj.StateID, DbType.Int32);
                    parameters.Add("@Country", obj.Country, DbType.String);
                    parameters.Add("@Zipcode", obj.Zipcode, DbType.String);
                    parameters.Add("@CreatedBy", userID, DbType.Int32);

                    var resultObj = await cxn.QueryAsync<SchoolModel>("dbo.Update_School", parameters, commandType: CommandType.StoredProcedure);

                    cxn.Close();
                }

                objKey = Tuple.Create(true, "School data updated successfully.", franchiseData);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                objKey = Tuple.Create(false, "Oops! School data update failed.Please try again.", franchiseData);
            }

            return objKey;
        }

        public async Task<Tuple<bool, string, SchoolViewModel>> GetSchoolIDOrUserName(string DcIDorName)
        {
            Tuple<bool, string, SchoolViewModel> result = null;
            SchoolViewModel schoolDetails = new SchoolViewModel();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@DcIDorName", DcIDorName, DbType.String);
                    using (var multi = await con.QueryMultipleAsync("dbo.Select_SchoolIDandName", parameters, commandType: CommandType.StoredProcedure))
                    {
                        schoolDetails = multi.Read<SchoolViewModel>().Single();
                    }
                }

                if (schoolDetails != null)
                    result = Tuple.Create(true, "", schoolDetails);
                else
                    result = Tuple.Create(false, "No records found", schoolDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", schoolDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<EmployeeViewModel>>> GetEmployesImages()
        {
            Tuple<bool, string, List<EmployeeViewModel>> result = null;
            List<EmployeeViewModel> lstDetails = new List<EmployeeViewModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<EmployeeViewModel>("[dbo].[Select_EmployeesImages]", commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    lstDetails = list.Select(x => new EmployeeViewModel
                    {
                        EmployeeName = x.EmployeeName,
                        Designation = x.Designation,
                        ImageName = x.ImageName,
                        ImageContent = x.ImageContent
                    }).ToList();

                    con.Close();
                }

                if (lstDetails != null && lstDetails.Count() > 0)
                    result = Tuple.Create(true, "", lstDetails);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstDetails);
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