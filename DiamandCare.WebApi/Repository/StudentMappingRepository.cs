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
            _imageUrl = System.Web.Hosting.HostingEnvironment.MapPath("~/Images");
        }


        public async Task<Tuple<bool, string, List<StudentMappingViewModel>>> GetStudentDetails(int UserID)
        {
            Tuple<bool, string, List<StudentMappingViewModel>> result = null;
            List<StudentMappingViewModel> lstStudentDetails = new List<StudentMappingViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@UserID", UserID, DbType.Int32);
                    con.Open();
                    var list = await con.QueryAsync<StudentMappingViewModel>("[dbo].[Select_FeePerticularsDetails]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    lstStudentDetails = list as List<StudentMappingViewModel>;
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
                        //await SendSMS(oTPViewModel.PhoneNumber, smsBody);
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