using Dapper;
using DiamandCare.Core;
using DiamandCare.WebApi.Properties;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace DiamandCare.WebApi
{
    public class RenewLoanAccountRepository
    {
        private AuthContext _ctx;

        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        BaseController _baseControler = null;
        int userID;

        private string _dcDb = Settings.Default.DiamandCareConnection;
        private string _url = Settings.Default.WebSiteURL;
        private string _smsUserName = Settings.Default.SMSUserName;
        private string _smsPwd = Settings.Default.SMSPwd;
        private string _smsSender = Settings.Default.SMSSender;
        public RenewLoanAccountRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            _roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(_ctx));

            userID = Helper.FindUserByID().UserID;
        }

        public async Task<Tuple<bool, string>> RenewLoanAccountForUser(User userModel)
        {
            Tuple<bool, string> identityUserResult = null;
            IdentityResult identityUser = null;
            ApplicationUser appUser = null;

            int isUserInserted = -1;
            _baseControler = new BaseController();
            try
            {
                userModel.DcID = "DCUSR" + Helper.GenerateDCID();

                appUser = new ApplicationUser
                {
                    EmailConfirmed = userModel.EmailConfirmed,
                    PasswordHash = userModel.PasswordHash,
                    PhoneNumber = userModel.PhoneNumber == "" ? "" : userModel.PhoneNumber.Trim(),
                    PhoneNumberConfirmed = userModel.PhoneNumberConfirmed,
                    TwoFactorEnabled = userModel.TwoFactorEnabled,
                    LockoutEnabled = userModel.LockoutEnabled,
                    AccessFailedCount = userModel.AccessFailedCount,
                    UserName = userModel.UserName.Trim(),
                    FirstName = userModel.FirstName.Trim(),
                    LastName = userModel.LastName == "" ? "" : userModel.LastName.Trim(),
                    IsActive = userModel.IsActive,
                    CreatedOn = DateTime.Now,
                    LastUpdatedOn = DateTime.Now,
                    RoleID = userModel.RoleID
                };

                identityUser = await _userManager.CreateAsync(appUser, userModel.Password);
                if (identityUser.Succeeded)
                {
                    var parameters = new DynamicParameters();
                    using (SqlConnection cxn = new SqlConnection(_dcDb))
                    {
                        parameters.Add("@UserID", appUser.UserID, DbType.Int32);
                        parameters.Add("@Id", appUser.Id, DbType.String);
                        parameters.Add("@RoleId", appUser.RoleID, DbType.String);
                        parameters.Add("@FatherName", userModel.FatherName, DbType.String);
                        parameters.Add("@AadharNumber", userModel.AadharNumber, DbType.String);
                        if (userModel.ParentID != 0)
                            parameters.Add("@ParentID", userModel.ParentID, DbType.Int32);
                        if (userModel.SponserID != 0)
                            parameters.Add("@SponserID", userModel.SponserID, DbType.Int32);
                        if (userModel.UnderID != 0)
                            parameters.Add("@UnderID", userModel.UnderID, DbType.Int32);
                        if (userModel.SourceID != 0)
                            parameters.Add("@SourceID", userModel.SourceID, DbType.Int32);
                        parameters.Add("@Position", userModel.Position, DbType.String);
                        parameters.Add("@DcID", userModel.DcID, DbType.String);
                        parameters.Add("@SecretCode", userModel.SecretCode, DbType.String);
                        parameters.Add("@UserStatusID", userModel.UserStatusID, DbType.Int32);
                        parameters.Add("@PositionsCompleted", userModel.PositionsCompleted, DbType.Int32);
                        parameters.Add("@CreatedBy", userID, DbType.Int32);
                        parameters.Add("@CreatedDate", DateTime.Now, DbType.DateTime);

                        isUserInserted = await cxn.ExecuteScalarAsync<int>("dbo.InsertOrUpdate_UserDetails_Renewal", parameters, commandType: CommandType.StoredProcedure);
                        cxn.Close();

                        string smsBody = $"Welcome to DIAMAND CARE  " +
                                            $"Your USERNAME :- {userModel.UserName}  " +
                                            $"DCID number :- {userModel.DcID}  " +
                                            $"Your PASSWORD :- {userModel.Password}  " +
                                            $"Login at " + _url;

                        await SendSMSDCID(userModel.PhoneNumber, smsBody);

                        if (isUserInserted != 0)
                        {
                            await _userManager.DeleteAsync(appUser);
                            return identityUserResult = Tuple.Create(false, "Renew loan account not created.Please try again.");
                        }
                    }
                    identityUserResult = Tuple.Create(true, "Renew loan account created successfully!");
                }
                else
                    identityUserResult = Tuple.Create(identityUser.Succeeded, string.Join(",", identityUser.Errors));
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                await _userManager.DeleteAsync(appUser);
                identityUserResult = Tuple.Create(false, "Oops! Renew loan account created failed with error " + ex.Message + ".Please try again.");
            }

            return identityUserResult;
        }

        public async Task<Tuple<bool, string>> SendSMSDCID(string PhoneNumber, string msgBody)
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
                    result = Tuple.Create(true, "Sent secret key failed.");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "");
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
    }
}