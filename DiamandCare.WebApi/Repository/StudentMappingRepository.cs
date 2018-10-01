using Dapper;
using DiamandCare.Core;
using DiamandCare.WebApi.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;

namespace DiamandCare.WebApi
{
    public class StudentMappingRepository
    {
        private string _dcDb = Settings.Default.DiamandCareConnection;
        private string _smsUserName = Settings.Default.SMSUserName;
        private string _smsPwd = Settings.Default.SMSPwd;
        private string _smsSender = Settings.Default.SMSSender;
        private static string _imageUrl;
        int UserID;

        public StudentMappingRepository()
        {
            UserID = Helper.FindUserByID().UserID;
            _imageUrl = System.Web.Hosting.HostingEnvironment.MapPath("~/");
        }

        public async Task<Tuple<bool, string>> InsertStudentMapping(StudentMappingModel studentMappingModel)
        {
            Tuple<bool, string> result = null;
            int registerStatus = -1;
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@UserID", studentMappingModel.UserID, DbType.Int32);
                    parameters.Add("@StudentName", studentMappingModel.StudentName, DbType.String);
                    parameters.Add("@Gender", studentMappingModel.Gender, DbType.String);
                    parameters.Add("@Address1", studentMappingModel.Address1, DbType.String);
                    parameters.Add("@Address2", studentMappingModel.Address2, DbType.String);
                    parameters.Add("@City", studentMappingModel.City, DbType.String);
                    parameters.Add("@District", studentMappingModel.District, DbType.String);
                    parameters.Add("@State", studentMappingModel.State, DbType.String);
                    parameters.Add("@Country", studentMappingModel.Country, DbType.String);
                    parameters.Add("@Zipcode", studentMappingModel.Zipcode, DbType.String);
                    parameters.Add("@FeeMasterID", studentMappingModel.FeeMasterID, DbType.Int32);
                    parameters.Add("@GroupID", studentMappingModel.GroupID, DbType.Int32);
                    parameters.Add("@Fees", studentMappingModel.CourseFee, DbType.Decimal);
                    parameters.Add("@ApprovalStatusID", studentMappingModel.ApprovalStatusID, DbType.Int32);
                    parameters.Add("@TransferStatusID", studentMappingModel.TransferStatusID, DbType.Int32);
                    parameters.Add("@CreatedBy", UserID, DbType.Int32);
                    registerStatus = await cxn.ExecuteScalarAsync<int>("dbo.Insert_StudentMapping", parameters, commandType: CommandType.StoredProcedure);

                    if (registerStatus == 0)
                        result = Tuple.Create(true, "");
                    else
                        result = Tuple.Create(false, "Student mapping failed.Please try again.");

                    cxn.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Student mapping failed.Please try again.");
            }

            return result;
        }

        public async Task<Tuple<bool, string, List<StudentMappingViewModel>>> GetStudentDetails(int userID)
        {
            Tuple<bool, string, List<StudentMappingViewModel>> result = null;
            List<StudentMappingViewModel> lstStudentDetails = new List<StudentMappingViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@UserID", userID, DbType.Int32);
                    con.Open();
                    var list = await con.QueryAsync<StudentMappingViewModel>("[dbo].[Select_StudentMappingDetails]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    lstStudentDetails = (List<StudentMappingViewModel>)list.Select(x => new StudentMappingViewModel
                    {
                        UserID = x.UserID,
                        StudentName = x.StudentName,
                        Gender = x.Gender == "M" ? "Male" : "Female",
                        GroupID = x.GroupID,
                        CourseFee = x.CourseFee,
                        PhoneNumber = x.PhoneNumber,
                        ApprovalStatusID = x.ApprovalStatusID,
                        ApprovalStatus = x.ApprovalStatus,
                        TransferStatusID = x.TransferStatusID,
                        TransferStatus = x.TransferStatus,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedOn = x.UpdatedOn
                    }).ToList();

                    con.Close();
                }

                if (lstStudentDetails != null && lstStudentDetails.Count() > 0)
                    result = Tuple.Create(true, "", lstStudentDetails);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstStudentDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstStudentDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, OTPViewModel>> UpdateUserOTP(OTPViewModel oTPViewModel)
        {
            Tuple<bool, string, OTPViewModel> resultOTPUpdate = null;
            OTPViewModel otpResultObj = new OTPViewModel();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@UserID", oTPViewModel.UserID, DbType.Int32);
                    parameters.Add("@OneTimePassword", oTPViewModel.OneTimePassword, DbType.Int32);

                    var resultObj = await cxn.QueryAsync<OTPViewModel>("dbo.Update_UserOTP", parameters, commandType: CommandType.StoredProcedure);
                    otpResultObj = resultObj.Single() as OTPViewModel;

                    if (otpResultObj != null)
                    {
                        resultOTPUpdate = Tuple.Create(true, "", otpResultObj);
                        string smsBody = $"Welcome to DIAMAND CARE  " +
                                         $"Your OTP :- {oTPViewModel.OneTimePassword}";
                        await SendSMS(oTPViewModel.PhoneNumber, smsBody);
                        //if (oTPViewModel.Email != "")
                        //{
                        //    string mailBody = $"<table width='100%'><tr><td style='font-family:Times New Roman;font-size:15px !important;'> Dear {oTPViewModel.FirstName + " " + oTPViewModel.LastName},</td></tr><tr><td><table width='100%'><tr><td style='width:95%;font-family:Times New Roman;font-size:15px !important;'>&nbsp;&nbsp;&nbsp;&nbsp;" +
                        //        $"Your one time password is: {oTPViewModel.OneTimePassword}</td></tr></table></td></tr>" +
                        //        $"<tr><td><table width='100%'><tr><td style='font-family:Times New Roman;font-size:15px !important;'>Kind Regards, <br/>DIAMAND CARE</td></tr></table><table width='100%'><tr><img src=cid:MyImage id='img'/></tr><tr><td style='font-family:Times New Roman;font-size:9px !important;'>CONFIDENTIALITY NOTICE AND DISCLAIMER</td>" +
                        //        $"</tr><tr><td><table width='100%'><tr><td style='width:95%;font-family:Times New Roman;font-size:9px !important;'>" +
                        //        $"Any views or opinions expressed within this email are those of the author. If you are not the intended recipient of this email you must not use, copy, distribute or disclose the e - mail, its existence, or any part of its contents to any other party.If you have received this email in error, please notify us immediately and destroy the message and all copies in your possession. Although the author operates anti - virus programs they do not accept responsibility for any loss or damage howsoever caused by viruses being passed or arising from the use of this e - mail or its attachments.We recommend that you subject them to your own virus checking procedures prior to use.Before printing, please think about the environment." +
                        //        $"</td></tr></table></td></tr></table></td></tr></table>";

                        //    AlternateView av = MailBody(mailBody);
                        //    await Task.Run(async () => await SendEmailWithSignature(oTPViewModel, "OTP", mailBody, av));
                        //}
                    }
                    else
                        resultOTPUpdate = Tuple.Create(false, "One time password generation failed.Please try again.", otpResultObj);

                    cxn.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                resultOTPUpdate = Tuple.Create(false, "Oops! One time password generation failed.Please try again.", otpResultObj);
            }

            return resultOTPUpdate;
        }

        public async Task<Tuple<bool, string, OTPViewModel>> VerifyOTP(OTPViewModel oTPViewModel)
        {
            Tuple<bool, string, OTPViewModel> result = null;
            OTPViewModel otpResultObj = new OTPViewModel();
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@UserID", oTPViewModel.UserID, DbType.Int32);
                    parameters.Add("@OneTimePassword", oTPViewModel.OneTimePassword, DbType.Int32);
                    var resultObj = await cxn.QueryAsync<OTPViewModel>("dbo.Validate_OTP", parameters, commandType: CommandType.StoredProcedure);
                    otpResultObj = resultObj.Single() as OTPViewModel;

                    if (otpResultObj != null)
                        result = Tuple.Create(true, "", otpResultObj);
                    else
                        result = Tuple.Create(false, "Please enter valid OTP.", otpResultObj);

                    cxn.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Please enter valid OTP.", otpResultObj);
            }

            return result;
        }

        public async Task<Tuple<bool, string>> GenerateLoanOTP(OTPViewModel oTPViewModel)
        {
            Tuple<bool, string> result = null;
            int generateStatus = -1;
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add("@UserID", oTPViewModel.UserID, DbType.Int32);
                    parameters.Add("@PhoneNumber", oTPViewModel.PhoneNumber, DbType.String);
                    parameters.Add("@LoanOTP", oTPViewModel.LoanOTP, DbType.Int32);
                    generateStatus = await cxn.ExecuteScalarAsync<int>("dbo.Generate_Student_LoanOTP", parameters, commandType: CommandType.StoredProcedure);

                    if (generateStatus == 0)
                    {
                        string smsBody = $"Welcome to DIAMAND CARE  " +
                                         $"Your OTP for apply loan:- {oTPViewModel.LoanOTP}";
                        await SendSMS(oTPViewModel.PhoneNumber, smsBody);
                        result = Tuple.Create(true, "OTP sent to your phone number: " + oTPViewModel.PhoneNumber);
                    }
                    else if (generateStatus == -2)
                        result = Tuple.Create(false, "Your phone number is in correct.Please contact admin.");
                    else
                        result = Tuple.Create(false, "Your OTP generation failed.Please try again.");

                    cxn.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Your OTP generation failed.Please try again.");
            }

            return result;
        }
        public async Task<Tuple<bool, string, List<FeeMastersModel>>> GetFeeMastersByUserID(int userID)
        {
            Tuple<bool, string, List<FeeMastersModel>> result = null;
            List<FeeMastersModel> lstFeeMasters = new List<FeeMastersModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@UserID", userID, DbType.Int32);
                    con.Open();
                    var list = await con.QueryAsync<FeeMastersModel>("[dbo].[Select_FeeMasterByUserID]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstFeeMasters = list as List<FeeMastersModel>;
                    con.Close();
                }

                if (lstFeeMasters != null && lstFeeMasters.Count() > 0)
                    result = Tuple.Create(true, "", lstFeeMasters);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstFeeMasters);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, ex.Message, lstFeeMasters);
            }
            return result;
        }

        public async Task<Tuple<bool, string>> SendSMS(string PhoneNumber, string msgBody)
        {
            Tuple<bool, string> result = null;

            string res = string.Empty;
            try
            {
                string url = "http://bulksms.mysmsmantra.com:8080/WebSMS/SMSAPI.jsp?username=" + _smsUserName + "&password=" + _smsPwd + "&sendername=" + _smsSender + "&mobileno=" + PhoneNumber + "&message=" + msgBody;
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

        public async Task SendEmailWithSignature(OTPViewModel userDetails, string Subject, string strBody, AlternateView AV)
        {
            string siteUrl = string.Empty;
            try
            {
                Helper obj = new Helper();
                Helper.SendEmailWithSignature(userDetails.Email, userDetails.FirstName, Subject, strBody, obj, AV);
            }
            catch (Exception ex)
            {
                Helper.SaveErrorToDataBase("", "SendEmail", string.Empty, ex);
            }
        }

        private AlternateView MailBody(string mailBody)
        {
            LinkedResource Img = new LinkedResource(_imageUrl + "\\logo_email.jpg", MediaTypeNames.Image.Jpeg);
            Img.ContentId = "MyImage";
            string str = mailBody;
            AlternateView AV = AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
            AV.LinkedResources.Add(Img);
            return AV;
        }
    }
}