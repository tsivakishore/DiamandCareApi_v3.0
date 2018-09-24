using DiamandCare.WebApi.Properties;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using DiamandCare.Core;

namespace DiamandCare.WebApi
{
    public class Helper
    {
        private static string _DiamandCareDb = Settings.Default.DiamandCareConnection;
        private static string _sMTPServer;
        private static string _sMTPUser;
        private static string _sMTPPassword;
        private static string _sMTPPort;
        private static string _bCC;

        public Helper()
        {
            _sMTPServer = Settings.Default.SMTPServer;
            _sMTPUser = Settings.Default.SMTPUser;
            _sMTPPassword = Settings.Default.SMTPPassword;
            _sMTPPort = Settings.Default.SMTPPort;
            _bCC = Settings.Default.BBC;
        }
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        /// <summary>
        /// eg. EncodeToBase64(Postman:5E0920FA-9CD2-47F8-90CD-5129B0F58D9F)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncodeToBase64(string value)
        {
            var toEncodeAsBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(toEncodeAsBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static void SendEmail(string strTo, string strToName, string strSubject, string strMailMessage, string CC = "", string BCC = "")
        {
            try
            {
                MailMessage objMailMessage = new MailMessage();
                objMailMessage.IsBodyHtml = true;
                objMailMessage.Subject = strSubject;
                objMailMessage.Body = strMailMessage;
                objMailMessage.From = new MailAddress((string)ConfigurationManager.AppSettings["SMTPUser"], "Diamand Care");

                objMailMessage.To.Add(strTo);
                if (!string.IsNullOrEmpty(CC))
                    objMailMessage.CC.Add(CC);
                if (!string.IsNullOrEmpty(BCC))
                    objMailMessage.Bcc.Add(BCC);

                SmtpClient objSmtpClient = new SmtpClient();
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Host = (string)ConfigurationManager.AppSettings["SMTPServer"];
                objSmtpClient.Port = Convert.ToInt32((string)ConfigurationManager.AppSettings["SMTPPort"]);
                objSmtpClient.Credentials = new System.Net.NetworkCredential((string)ConfigurationManager.AppSettings["SMTPUser"], (string)ConfigurationManager.AppSettings["SMTPPassword"]);
                objSmtpClient.EnableSsl = true;

                objSmtpClient.Send(objMailMessage);
                objMailMessage.To.Clear();
            }
            catch (Exception ex)
            {
                SaveErrorToDataBase("DiamandCare.WebApi.Helper", "SendEmail", string.Empty, ex);
            }
        }

        public static void SendEmailWithSignature(string strTo, string strToName, string strSubject, string strMailMessage, Helper obj, AlternateView AV, string CC = "", string BCC = "")
        {
            try
            {
                MailMessage objMailMessage = new MailMessage();
                objMailMessage.AlternateViews.Add(AV);
                objMailMessage.IsBodyHtml = true;
                objMailMessage.Subject = strSubject;
                objMailMessage.From = new MailAddress(_sMTPUser, "DIAMAND");

                objMailMessage.To.Add(strTo);
                if (!string.IsNullOrEmpty(CC))
                    objMailMessage.CC.Add(CC);
                if (!string.IsNullOrEmpty(_bCC))
                    objMailMessage.Bcc.Add(_bCC);

                SmtpClient objSmtpClient = new SmtpClient();
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Host = _sMTPServer;
                objSmtpClient.Port = Convert.ToInt32(_sMTPPort);
                objSmtpClient.Credentials = new System.Net.NetworkCredential(_sMTPUser, _sMTPPassword);
                objSmtpClient.EnableSsl = true;

                objSmtpClient.Send(objMailMessage);
                objMailMessage.To.Clear();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
        }
        public static void SaveErrorToDataBase(string strScreenName, string strFunctionName, string strParameterValues, Exception exception)
        {
            try
            {
                BaseController baseController = new BaseController();

                using (SqlConnection cxn = new SqlConnection(_DiamandCareDb))
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add(AppConstants.CONST_SCREEN_NAME, strScreenName);
                    parameters.Add(AppConstants.CONST_FUNCTION_NAME, strFunctionName);
                    parameters.Add(AppConstants.CONST_PARAMETER_VALUES, strParameterValues);
                    parameters.Add(AppConstants.CONST_ERROR_MESSAGE, exception.Message);
                    parameters.Add(AppConstants.CONST_CREATED_BY, baseController.Identity);

                    cxn.QueryFirst<int>("dbo.InsertSystemErrorLog", commandType: CommandType.StoredProcedure);
                    cxn.Close();
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message);
            }
        }
        public static void LogMessage(string strErrorMessage)
        {
            try
            {
                string _strPath = DateTime.Now.Date.ToString("yyyyMMdd");
                string _strDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExecutionLog");
                if (!Directory.Exists(Path.Combine(_strDirectoryPath, _strPath)))
                {
                    Directory.CreateDirectory(Path.Combine(_strDirectoryPath, _strPath));
                }
                _strDirectoryPath = System.IO.Path.Combine(_strDirectoryPath, _strPath + ".txt");
                StreamWriter objStreamWriter = new StreamWriter(_strDirectoryPath, true);
                objStreamWriter.WriteLine(DateTime.Now.ToString() + " : " + strErrorMessage);
                objStreamWriter.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GeneratePassword(int length = 10)
        {
            char[] chars = new char[length];
            try
            {
                string _allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                Random randNum = new Random();
                int allowedCharCount = _allowedChars.Length;
                for (int i = 0; i < length; i++)
                {
                    chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
                }
            }
            catch (Exception ex)
            {
                SaveErrorToDataBase("DiamandCare.WebApi.Helper", "GeneratePassword", string.Empty, ex);
            }
            return new string(chars);
        }

        public static string GenerateSecretKey(int length = 10)
        {
            char[] chars = new char[length];
            try
            {
                string _allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random randNum = new Random();
                int allowedCharCount = _allowedChars.Length;
                for (int i = 0; i < length; i++)
                {
                    chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
                }
            }
            catch (Exception ex)
            {
                SaveErrorToDataBase("DiamandCare.WebApi.Helper", "GeneratePassword", string.Empty, ex);
            }
            return new string(chars);
        }

        public static string GenerateForgetPassword(int length = 6)
        {
            char[] chars = new char[length];
            try
            {
                string _allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random randNum = new Random();
                int allowedCharCount = _allowedChars.Length;
                for (int i = 0; i < length; i++)
                {
                    chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
                }
            }
            catch (Exception ex)
            {
                SaveErrorToDataBase("DiamandCare.WebApi.Helper", "GeneratePassword", string.Empty, ex);
            }
            return new string(chars);
        }

        public static string GenerateDCID(int length = 7)
        {
            char[] chars = new char[length];
            try
            {
                string _allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random randNum = new Random();
                int allowedCharCount = _allowedChars.Length;
                for (int i = 0; i < length; i++)
                {
                    chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
                }
            }
            catch (Exception ex)
            {
                SaveErrorToDataBase("DiamandCare.WebApi.Helper", "GenerateDCID", string.Empty, ex);
            }
            return new string(chars);
        }
        public static User FindUserByID()
        {
            User userDetails = new User();
            BaseController _baseControler = new BaseController();
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_DiamandCareDb))
                {
                    parameters.Add("@Id", _baseControler.UserID);
                    parameters.Add(PARAM_MODE, MODE_TWO);
                    userDetails = con.QuerySingle<User>("dbo.Select_Users", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Helper.SaveErrorToDataBase("Helper", "FindUserByID", string.Empty, ex);
            }
            return userDetails;
        }

        #region Modes
        public const string PARAM_MODE = "Mode";

        public const int MODE_ONE = 1;
        public const int MODE_TWO = 2;
        public const int MODE_THREE = 3;
        public const int MODE_FOUR = 4;
        public const int MODE_FIVE = 5;
        public const int MODE_SIX = 6;
        public const int MODE_SEVEN = 7;

        #endregion Modes
    }
    public static class AppConstants
    {
        public const string CONST_CREATED_BY = "CreatedBy";

        public const string CONST_SCREEN_NAME = "ScreenName";

        public const string CONST_FUNCTION_NAME = "FunctionName";

        public const string CONST_PARAMETER_VALUES = "ParameterValues";

        public const string CONST_ERROR_MESSAGE = "ErrorMessage";

        public const string CONST_FROM_DATE = "FromDate";

        public const string CONST_TO_DATE = "ToDate";

        public const string CONST_START_DATE = "StartDate";

        public const string CONST_END_DATE = "EndDate";

        public const string EMPTY_STRING = "";

        public const int NUMERIC_NO_VALUE = 0;

        public const int NUMERIC_ONE_VALUE = 1;

        public const int CURRENCY_DECIMALS = 2;

        //CSS Constants

        public const string CONST_ACTIVE_COLOR_RED = "red";

        public const string CONST_ACTIVE_COLOR_GREEN = "green";

        public const string CONST_ACTIVE_COLOR_ORANGE = "orange";

        public const string CONST_COLOR_RED = "red";

        public const string CONST_COLOR_GREEN = "green";

        public const string CONST_COLOR_ORANGE = "orange";

        //Loan eligibility fali conditions display messages

        public const string DISPLAY_MESSAGE_2 = "Payment not done for your previous loan";

        public const string NO_RECORDS_FOUND = "No records found";
        public const string DISPLAY_MESSAGE_3 = "Already applied for personal loan in this level";
        public const string DISPLAY_MESSAGE_4 = "Risk loan already applied";
        public const string DISPLAY_MESSAGE_5 = "Applied for Rinsk benefit, can't apply for any loan";
        public const string DISPLAY_MESSAGE_6 = "Payment not done for Emergency loan";
        public const string DISPLAY_MESSAGE_7 = "House loan already applied";
        public const string DISPLAY_MESSAGE_8 = "Previous Emergency loan payment not done";
        public const string DISPLAY_MESSAGE_9 = "Fee Reimbursement already applied";
        public const string DISPLAY_MESSAGE_10 = "Prepaid loan can't be applied";
        public const string DISPLAY_MESSAGE_11 = "Your ID was renewed, please apply loans with new user";
        public const string DISPLAY_MESSAGE_12 = "User is on hold, Please contact administrator";
        public const string DISPLAY_MESSAGE_13 = "Please fill the bank details";
        public const string DISPLAY_MESSAGE_14 = "Loans are not exist to Prepaid";
        public const string DISPLAY_MESSAGE_15 = "Can't prepaid the unpaid Loan";
        public const string DISPLAY_MESSAGE_16 = "Can't apply this loan as Prepaid second time";
        public const string DISPLAY_MESSAGE_17 = "Can't apply this loan second time as the next level is not enabled";
        public const string DISPLAY_MESSAGE_18 = "Fees Reimbursement can't applied as already loan was applied in this level";
        public const string DISPLAY_MESSAGE_19 = "Not eligible for this loan";

        public const string ALL_LOAN_PAYMENTS = "All Loan Payments";
        public const string LOAN_PAYMENTS = "Loan Payments";
        public const string ALL_LOAN_DETAILS = "All Loan Details";
        public const string LOAN_DETAILS = "Loan Details";
        public const string ALL_USED_SECRET_KEYS = "All Used Secret Keys";
        public const string USED_SECRET_KEYS = "Used Secret Keys";
        public const string ALL_ISSUED_SECRET_KEYS = "All Issued Secret Keys";
        public const string ISSUED_SECRET_KEYS = "Issued Secret Keys";
        public const string ALL_WALLET_TRANSACTIONS = "All Wallet Transactions";
        public const string WALLET_TRANSACTIONS = "Wallet Transactions";
        public const string TRANSFER_PAYMENTS = "Transfer Payments";
        public const string ALL_COMMISSIONS_LOG = "All Commissions Log";
        public const string COMMISSIONS_LOG = "Commissions Log";
    }
}