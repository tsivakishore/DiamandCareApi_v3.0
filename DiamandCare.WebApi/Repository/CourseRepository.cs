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
    public class CourseRepository
    {
        private string _dcDb = Settings.Default.DiamandCareConnection;
        int UserID;

        public CourseRepository()
        {
            UserID = Helper.FindUserByID().UserID;
        }

        public async Task<Tuple<bool, string, List<CourseMasterViewModel>>> GetCourseMasterDetails()
        {
            Tuple<bool, string, List<CourseMasterViewModel>> result = null;
            List<CourseMasterViewModel> lstCourseMasterDetails = new List<CourseMasterViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<CourseMasterViewModel>("[dbo].[Select_CourseMasterDetails]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstCourseMasterDetails = list as List<CourseMasterViewModel>;
                    con.Close();
                }

                if (lstCourseMasterDetails != null && lstCourseMasterDetails.Count() > 0)
                {
                    result = Tuple.Create(true, "", lstCourseMasterDetails);
                }
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstCourseMasterDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstCourseMasterDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string>> CreateCourseMaster(CourseMasterModel courseMasterModel)
        {
            Tuple<bool, string> result = null;
            int insertStatus = -1;
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    if (courseMasterModel.CourseMasterID > 0)
                        parameters.Add("@CourseMasterID", courseMasterModel.CourseMasterID, DbType.Int32);

                    parameters.Add("@CourseMasterName", courseMasterModel.CourseMasterName, DbType.String);
                    parameters.Add("@CourseDescription", courseMasterModel.CourseDescription, DbType.String);
                    parameters.Add("@CreatedBy", UserID, DbType.Int32);
                    cxn.Open();
                    insertStatus = await cxn.ExecuteScalarAsync<int>("dbo.Insert_CreateCourseMaster", parameters, commandType: CommandType.StoredProcedure);
                    cxn.Close();
                }

                if (insertStatus == 0)
                {
                    if (courseMasterModel.CourseMasterID == 0)
                        result = Tuple.Create(true, "Course master details created successfully");
                    else if (courseMasterModel.CourseMasterID > 0)
                        result = Tuple.Create(true, "Course master details updated successfully");
                }
                else
                    result = Tuple.Create(false, "Course master details created failed");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message);
            }

            return result;
        }

        public async Task<Tuple<bool, string, List<CourseViewModel>>> GetCourseDetailsByCourseMasterID(int CourseMasterID)
        {
            Tuple<bool, string, List<CourseViewModel>> result = null;
            List<CourseViewModel> lstCourseMasterDetails = new List<CourseViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@CourseMasterID", CourseMasterID, DbType.Int32);
                    con.Open();
                    var list = await con.QueryAsync<CourseViewModel>("[dbo].[Select_CourseMasterDetailsByID]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstCourseMasterDetails = list as List<CourseViewModel>;
                    con.Close();
                }

                if (lstCourseMasterDetails != null && lstCourseMasterDetails.Count() > 0)
                    result = Tuple.Create(true, "", lstCourseMasterDetails);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstCourseMasterDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstCourseMasterDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string>> CreateCourse(CourseModel courseModel)
        {
            Tuple<bool, string> result = null;
            int insertStatus = -1;
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    if (courseModel.CourseID > 0)
                        parameters.Add("@CourseID", courseModel.CourseID, DbType.Int32);

                    parameters.Add("@CourseMasterID", courseModel.CourseMasterID, DbType.Int32);
                    parameters.Add("@CourseName", courseModel.CourseName, DbType.String);
                    parameters.Add("@CourseDescription", courseModel.CourseDescription, DbType.String);
                    parameters.Add("@CreatedBy", UserID, DbType.Int32);
                    cxn.Open();
                    insertStatus = await cxn.ExecuteScalarAsync<int>("dbo.Insert_CreateCourse", parameters, commandType: CommandType.StoredProcedure);
                    cxn.Close();
                }

                if (insertStatus == 0)
                {
                    if (courseModel.CourseID == 0)
                        result = Tuple.Create(true, "Course details created successfully");
                    else if (courseModel.CourseID > 0)
                        result = Tuple.Create(true, "Course details updated successfully");
                }
                else
                    result = Tuple.Create(false, "Course details created failed");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message);
            }

            return result;
        }

        public async Task<Tuple<bool, string, List<CoursesModel>>> GetCourses()
        {
            Tuple<bool, string, List<CoursesModel>> result = null;
            List<CoursesModel> lstCourses = new List<CoursesModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    con.Open();
                    var list = await con.QueryAsync<CoursesModel>("[dbo].[Select_Courses]", commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstCourses = list as List<CoursesModel>;
                    con.Close();
                }

                if (lstCourses != null && lstCourses.Count() > 0)
                    result = Tuple.Create(true, "", lstCourses);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstCourses);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstCourses);
            }
            return result;
        }
    }
}