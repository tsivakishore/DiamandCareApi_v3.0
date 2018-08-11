﻿using DiamandCare.WebApi.Properties;
using Dapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DiamandCare.Core;
using System.Web;
using System.Configuration;
using System.Transactions;
using System.Net;
using System.IO;
using DiamandCare.WebApi.ViewModels;

namespace DiamandCare.WebApi.Repository
{
    public class AuthRepository : IDisposable
    {
        private AuthContext _ctx;

        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        BaseController _baseControler = null;

        private string _dcDb = Settings.Default.DiamandCareConnection;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            _roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(_ctx));

            //_userManager.FindAsync("", "");
        }

        public async Task<Tuple<bool, string>> RegisterUser(User userModel)
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
                    Email = userModel.Email.Trim(),
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
                    RoleID = userModel.RoleID,
                    EthAddress = userModel.EthAddress
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
                        parameters.Add("@CreatedBy", appUser.UserID, DbType.Int32);
                        parameters.Add("@CreatedDate", DateTime.Now, DbType.DateTime);

                        isUserInserted = await cxn.ExecuteScalarAsync<int>("dbo.InsertOrUpdate_UserDetails", parameters, commandType: CommandType.StoredProcedure);
                        cxn.Close();

                        string smsBody = $"Welcome to DIAMAND CARE  " +
                                            $"Your USERNAME :- {userModel.UserName}  " +
                                            $"ID number :- {userModel.DcID}  " +
                                            $"Your PASSWORD :- {userModel.Password}  " +
                                            $"Login at www.diamandcare.com";

                        await SendSMSDCID(userModel.PhoneNumber, smsBody);

                        if (isUserInserted != 0)
                        {
                            await _userManager.DeleteAsync(appUser);
                            return identityUserResult = Tuple.Create(false, "Oops! User details not created.Please try again.");
                        }
                    }
                    identityUserResult = Tuple.Create(true, "User Registered successfully!");
                }
                else
                    identityUserResult = Tuple.Create(identityUser.Succeeded, string.Join(",", identityUser.Errors));
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                await _userManager.DeleteAsync(appUser);
                identityUserResult = Tuple.Create(false, "Oops! User Registeration failed with error " + ex.Message + ".Please try again.");
            }

            return identityUserResult;
        }
        public async Task<Tuple<bool, string, List<RegistrationViewModel>>> GetAllUsers()
        {
            Tuple<bool, string, List<RegistrationViewModel>> result = null;
            List<RegistrationViewModel> allUsers = new List<RegistrationViewModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add(Helper.PARAM_MODE, Helper.MODE_ONE);

                    using (var multi = await cxn.QueryMultipleAsync("[dbo].[Select_Users]", parameters, commandType: CommandType.StoredProcedure))
                    {
                        allUsers = multi.Read<RegistrationViewModel>().ToList();
                    }
                    cxn.Close();
                }

                if (allUsers != null && allUsers.Count > 0)
                    result = Tuple.Create(true, "", allUsers);
                else
                    result = Tuple.Create(false, "", allUsers);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", allUsers);
            }

            return result;
        }

        public async Task<Tuple<bool, string>> SendSMSDCID(string PhoneNumber, string DcID)
        {
            Tuple<bool, string> result = null;

            string res = string.Empty;
            try
            {
                string url = "http://bulksms.mysmsmantra.com:8080/WebSMS/SMSAPI.jsp?username=sivakishore&password=1174306098&sendername=SFEOrg&mobileno=" + PhoneNumber + "&message=" + DcID;
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
        public async Task<Tuple<bool, string>> UpdateUserDetails(RegistrationViewModel updateUserModel)
        {
            int updatedStatus = -1;
            Tuple<bool, string> updateUser = null;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    int isInserted = -1;

                    var parameters = new DynamicParameters();
                    var spParams = new DynamicParameters();
                    parameters.Add("@Id", updateUserModel.Id);
                    parameters.Add("@UserID", updateUserModel.UserID);
                    parameters.Add("@FirstName", updateUserModel.FirstName, DbType.String);
                    parameters.Add("@LastName", updateUserModel.LastName, DbType.String);
                    parameters.Add("@Address1", updateUserModel.Address1, DbType.String);
                    parameters.Add("@Address2", updateUserModel.Address2, DbType.String);
                    parameters.Add("@City", updateUserModel.City, DbType.String);
                    parameters.Add("@State", updateUserModel.State, DbType.String);
                    parameters.Add("@Zipcode", updateUserModel.Zipcode, DbType.String);
                    parameters.Add("@Country", updateUserModel.Country, DbType.String);
                    parameters.Add("@IsActive", updateUserModel.IsActive, DbType.Boolean);
                    parameters.Add("@RoleId", updateUserModel.RoleID, DbType.String);
                    parameters.Add("@RoleName", updateUserModel.RoleName, DbType.String);
                    parameters.Add("@Email", updateUserModel.Email, DbType.String);
                    parameters.Add("@PhoneNumber", updateUserModel.PhoneNumber, DbType.String);
                    parameters.Add("@UserName", updateUserModel.UserName, DbType.String);
                    parameters.Add("@CreatedBy", Helper.FindUserByID().UserID, DbType.String);
                    parameters.Add("@Mode", Helper.MODE_ONE, DbType.Int32);

                    updatedStatus = await cxn.ExecuteScalarAsync<int>("dbo.InsertOrUpdate_Users", parameters, commandType: CommandType.StoredProcedure);

                    if (updatedStatus == 0)
                    {

                        //updateUserModel.UserAccounts.ForEach(x => x.UserID = updateUserModel.UserID);

                        //spParams.Add("@UserID", updateUserModel.UserID, DbType.Int32);
                        //spParams.AddDynamicParams(new DynamicParameters(new { UserAccounts = CollectionHelper.ConvertToDataTable(updateUserModel.UserAccounts) }));
                        //spParams.Add("@UserAccounts", ToDataTableAccountIDs(updateUserModel.UserAccounts).AsTableValuedParameter());
                        //isInserted = await cxn.ExecuteScalarAsync<int>("dbo.InsertOrUpdate_UserAccounts", spParams, commandType: CommandType.StoredProcedure);
                        //cxn.Close();

                        updateUser = Tuple.Create(true, "User details updated successfully.");
                    }
                    else
                        updateUser = Tuple.Create(false, "Oops! User details updatation failed.Please try again.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                updateUser = Tuple.Create(false, "Oops! User details updatation failed.Please try again.");
            }

            return updateUser;
        }
        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = null;
            try
            {
                user = await _userManager.FindAsync(userName, password);
            }
            catch (Exception ex)
            {
                Helper.SaveErrorToDataBase("AuthRepository", "FindUser", string.Empty, ex);
            }
            return user;
        }
        public async Task<Tuple<bool, string, IdentityResult>> RegisterRole(RoleViewModel roleViewModel)
        {

            Tuple<bool, string, IdentityResult> identityRoleResult = null;
            IdentityResult identityRole = null;

            try
            {
                ApplicationRole role = new ApplicationRole
                {
                    Name = roleViewModel.Name,
                    IsActive = roleViewModel.IsActive,
                    CreatedOn = DateTime.Now,
                    CreatedBy = Helper.FindUserByID().Id,
                    UpdatedOn = DateTime.Now,
                };

                identityRole = await _roleManager.CreateAsync(role);

                if (identityRole.Succeeded)
                    identityRoleResult = Tuple.Create(true, "Role Registered successfully!", identityRole);
                else
                {
                    string errors = identityRole.Errors.FirstOrDefault().ToString();
                    identityRoleResult = Tuple.Create(false, "Oops! " + errors + "Please try again.", identityRole);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                identityRoleResult = Tuple.Create(false, "Oops! Role Registeration failed.Please try again.", identityRole);
            }

            return identityRoleResult;
        }
        public async Task<Tuple<bool, string, List<Role>>> GetAllRoles()
        {
            Tuple<bool, string, List<Role>> allRoles = null;
            List<Role> rolesList = new List<Role>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    parameters.Add(Helper.PARAM_MODE, Helper.MODE_ONE);
                    var objectUserList = await cxn.QueryAsync<Role>("dbo.Select_Roles",
                        parameters, commandType: CommandType.StoredProcedure);

                    rolesList = (List<Role>)objectUserList.Select(x => new Role
                    {
                        RoleID = x.RoleID,
                        Name = x.Name,
                        IsActive = x.IsActive
                    }).ToList();
                }

                if (rolesList != null && rolesList.Count > 0)
                    allRoles = Tuple.Create(true, "Role details found.", rolesList);
                else
                    allRoles = Tuple.Create(false, "No Role details forund.", rolesList);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                allRoles = Tuple.Create(false, "No Role details forund.", rolesList);
            }

            return allRoles;
        }
        public async Task<Tuple<bool, string>> UpdateRole(RoleViewModel roleViewModel)
        {
            Tuple<bool, string> updateRoles = null;
            int updatedStatus = -1;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", roleViewModel.RoleID, DbType.String);
                    //parameters.Add("@Name", roleViewModel.Name);
                    parameters.Add("@IsActive", roleViewModel.IsActive);
                    parameters.Add("@CreatedBy", Helper.FindUserByID().UserID);

                    updatedStatus = await cxn.ExecuteScalarAsync<int>("dbo.Update_Role", parameters, commandType: CommandType.StoredProcedure);
                    cxn.Close();

                    if (updatedStatus == 0)
                        updateRoles = Tuple.Create(true, "Role updated successfully.");
                    else
                        updateRoles = Tuple.Create(false, "Oops! Role details updatation failed.Please try again.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                updateRoles = Tuple.Create(false, "Oops! Role details updatation failed.Please try again.");
            }
            return updateRoles;
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
        public async Task<IdentityRole> GetRole(string roleId)
        {
            IdentityRole role = null;
            try
            {
                role = await _roleManager.FindByIdAsync(roleId);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return role;
        }

        public async Task<ApiClient> FindClient(string clientId)
        {
            ApiClient client = null;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    // create sp parameters
                    DynamicParameters spParams = new DynamicParameters();
                    spParams.Add("@ClientID", clientId);

                    // execute sp 
                    var accts = await cxn.QueryFirstAsync<ApiClient>("Select_ApiClient", spParams, commandType: CommandType.StoredProcedure);
                    client = accts as ApiClient;

                    cxn.Close();
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
            }

            return client;
        }

        public async Task<Tuple<bool, string>> ForgetePassword(ForgetPasswordModel forgetPasswordModel)
        {
            Tuple<bool, string> resultForgetPassword = null;
            ApplicationUser createdUser = null;
            string passwordHash = string.Empty;
            int updatedStatus = -1;
            try
            {
                string password = Helper.GenerateForgetPassword(6);

                string smsBody = $"Welcome to DIAMAND CARE  " +
                        $"Your password has been reset.  " +
                        $"Your new password :- {password}  " +
                        $"Login at www.diamandcare.com";

                passwordHash = _userManager.PasswordHasher.HashPassword(password);
                createdUser = await _userManager.FindByNameAsync(forgetPasswordModel.UserName);
                if (createdUser != null)
                {
                    using (SqlConnection cxn = new SqlConnection(_dcDb))
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", createdUser.Id, DbType.String);
                        parameters.Add("@PasswordHash", passwordHash, DbType.String);
                        updatedStatus = await cxn.ExecuteScalarAsync<int>("dbo.Update_ForgetPassword", parameters, commandType: CommandType.StoredProcedure);
                    }

                    if (updatedStatus == 0)
                    {
                        resultForgetPassword = Tuple.Create(true, "Password has been sent to your register mobile number.");
                        await SendSMSDCID(createdUser.PhoneNumber, smsBody);
                    }
                    else
                        resultForgetPassword = Tuple.Create(false, "Oops! There has been an error from server. Please try again.");

                }
                else
                    resultForgetPassword = Tuple.Create(false, "Oops! There has been an error from server. Please try again.");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                resultForgetPassword = Tuple.Create(false, "Oops! There has been an error from server. Please try again..");
            }
            return resultForgetPassword;
        }

        public async Task<Tuple<bool, string>> ChangePassword(ForgetPasswordModel forgetPasswordModel)
        {
            Tuple<bool, string> changePasswordResult = null;
            string passwordHash = string.Empty;
            int updatedStatus = -1;

            try
            {
                passwordHash = _userManager.PasswordHasher.HashPassword(forgetPasswordModel.Password);
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Helper.FindUserByID().Id, DbType.String);
                    parameters.Add("@PasswordHash", passwordHash, DbType.String);
                    updatedStatus = await cxn.ExecuteScalarAsync<int>("dbo.Update_ForgetPassword", parameters, commandType: CommandType.StoredProcedure);
                }

                if (updatedStatus == 0)
                    changePasswordResult = Tuple.Create(true, "Password has been changed successfully.");
                else
                    changePasswordResult = Tuple.Create(false, "Oops! Change Password failed. Please try again.");

            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                changePasswordResult = Tuple.Create(false, "Oops! Change Password failed.Please try again.");
            }
            return changePasswordResult;
        }
        public async Task<Tuple<bool, string>> CheckPassword(ChangePassword updateModel)
        {
            Tuple<bool, string> checkPassword = null;
            try
            {
                var userDetails = _userManager.FindByIdAsync(Helper.FindUserByID().Id).Result;
                bool IsCorrectPassword = await _userManager.CheckPasswordAsync(userDetails, updateModel.Password);
                if (!IsCorrectPassword)
                    checkPassword = Tuple.Create(false, "");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                checkPassword = Tuple.Create(false, "Error while checking password.");
            }
            return checkPassword;
        }
        public async Task<Tuple<bool, string, int>> GetUserRole(User updateModel)
        {
            Tuple<bool, string, int> result = null;
            int roleID = 0;
            try
            {
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserName", updateModel.UserName, DbType.String);

                    roleID = await cxn.ExecuteScalarAsync<int>("dbo.Select_UserRole", parameters, commandType: CommandType.StoredProcedure);
                    cxn.Close();
                    result = Tuple.Create(true, "", roleID);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Role details updatation failed.Please try again.", roleID);
            }
            return result;
        }
        public Tuple<bool, string, User> GetUserDetailsByID()
        {
            Tuple<bool, string, User> result = null;
            User userDetails = new User();

            try
            {
                userDetails = FindUserByID();
                if (userDetails != null)
                    result = Tuple.Create(true, "", userDetails);
                else
                    result = Tuple.Create(false, "", userDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! User Details failed.Please try again.", userDetails);
            }
            return result;
        }

        public Tuple<bool, string, User> GetUserRoleByID()
        {
            Tuple<bool, string, User> result = null;
            User userDetails = new User();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@Id", Helper.FindUserByID().Id);
                    userDetails = con.QuerySingle<User>("dbo.Select_UserRoleByUserID", parameters, commandType: CommandType.StoredProcedure);
                }

                if (userDetails != null)
                    result = Tuple.Create(true, "", userDetails);
                else
                    result = Tuple.Create(false, "", userDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Error in get user role by Id.Please try again.", userDetails);
            }
            return result;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            try
            {
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    // create sp parameters
                    DynamicParameters spParams = new DynamicParameters();

                    spParams.Add("@Id", refreshTokenId, DbType.String);

                    await cxn.ExecuteScalarAsync<int>("[dbo].[Delete_RefreshToken]", spParams, commandType: CommandType.StoredProcedure);
                    cxn.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                return false;
            }
            return true;
        }
        private DataTable ToDataTableAccountIDs(string accountIDs)
        {
            DataTable dtAccount = new DataTable();
            dtAccount.Columns.Add("AccountID", typeof(String));
            if (!string.IsNullOrEmpty(accountIDs))
            {
                var accounts = accountIDs.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string url in accounts)
                    dtAccount.Rows.Add(new object[] { url.Trim() });
            }
            return dtAccount;
        }
        public User FindUserByID()
        {
            User userDetails = new User();
            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@Id", Helper.FindUserByID().Id);
                    parameters.Add(Helper.PARAM_MODE, Helper.MODE_TWO);
                    userDetails = con.QuerySingle<User>("dbo.Select_Users", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return userDetails;
        }

        public Tuple<bool, string, UserViewModel> GetUsersDetailsByLoginID()
        {
            Tuple<bool, string, UserViewModel> result = null;
            UserViewModel userDetails = null;
            BaseController _baseControler = new BaseController();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@Id", _baseControler.UserID);
                    userDetails = con.QuerySingle<UserViewModel>("dbo.Select_UsersByID", parameters, commandType: CommandType.StoredProcedure);
                }

                if (userDetails != null)
                    result = Tuple.Create(true, "", userDetails);
                else
                    result = Tuple.Create(false, "No details found.", userDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "No details found.", userDetails);
            }
            return result;
        }

        public Tuple<bool, string, UserProfileViewModel> GetUsersProfile()
        {
            Tuple<bool, string, UserProfileViewModel> result = null;
            UserProfileViewModel userProfile = null;
            BaseController _baseControler = new BaseController();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@Id", _baseControler.UserID);
                    userProfile = con.QuerySingle<UserProfileViewModel>("dbo.Select_UserProfile", parameters, commandType: CommandType.StoredProcedure);
                }

                if (userProfile != null)
                    result = Tuple.Create(true, "", userProfile);
                else
                    result = Tuple.Create(false, "No details found.", userProfile);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "No details found.", userProfile);
            }
            return result;
        }

        public Tuple<bool, string, AddressViewModel> GetUsersAddress()
        {
            Tuple<bool, string, AddressViewModel> result = null;
            AddressViewModel userAddress = null;
            BaseController _baseControler = new BaseController();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@Id", _baseControler.UserID);
                    userAddress = con.QuerySingle<AddressViewModel>("dbo.Select_UserAddress", parameters, commandType: CommandType.StoredProcedure);
                }

                if (userAddress != null)
                    result = Tuple.Create(true, "", userAddress);
                else
                    result = Tuple.Create(false, "No details found.", userAddress);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "No details found.", userAddress);
            }
            return result;
        }

        public Tuple<bool, string, UserNomineeDetailsViewModel> GetUsersNomineeDetails(int UserID)
        {
            Tuple<bool, string, UserNomineeDetailsViewModel> result = null;
            UserNomineeDetailsViewModel userNomineeDetails = null;

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@UserID", UserID);
                    userNomineeDetails = con.QuerySingle<UserNomineeDetailsViewModel>("dbo.Select_UserNomineeDetails", parameters, commandType: CommandType.StoredProcedure);
                }

                if (userNomineeDetails != null && userNomineeDetails.UserID > 0)
                    result = Tuple.Create(true, "", userNomineeDetails);
                else
                    result = Tuple.Create(false, "No details found.", userNomineeDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "No details found.", userNomineeDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<MenuViewModel>>> GetMenu(UserViewModel userViewModel)
        {
            Tuple<bool, string, List<MenuViewModel>> result = null;
            List<MenuViewModel> lstMenu = new List<MenuViewModel>();
            string UserName = string.Empty;

            ApplicationUser createdUser = await _userManager.FindByIdAsync(userViewModel.Id);
            UserName = createdUser.UserName;
            try
            {
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", userViewModel.Id, DbType.String);

                    var data = await cxn.QueryAsync<MenuViewModel>("[dbo].[Select_RoleMenusByUserID]", parameters, commandType: CommandType.StoredProcedure);
                    lstMenu = data as List<MenuViewModel>;

                    cxn.Close();
                    result = Tuple.Create(true, UserName, lstMenu);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! No role details found.Please try again.", lstMenu);
            }
            return result;
        }

        public async Task<Tuple<bool, string, List<UsersRoleViewModel>>> GetUserAndRoles()
        {
            Tuple<bool, string, List<UsersRoleViewModel>> result = null;
            List<UsersRoleViewModel> listUserRole = null;
            try
            {
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    var list = await cxn.QueryAsync<UsersRoleViewModel>("dbo.Select_UserAndRoles", commandType: CommandType.StoredProcedure);
                    listUserRole = list.Select(x => new UsersRoleViewModel
                    {
                        UserID = x.UserID,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        RoleID = x.RoleID,
                        RoleName = x.RoleName

                    }).ToList();

                }

                if (listUserRole != null && listUserRole.Count > 0)
                    result = Tuple.Create(true, "", listUserRole);
                else
                    result = Tuple.Create(false, "", listUserRole);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "Oops! Get user role details failed.Please try again.", listUserRole);
            }
            return result;
        }

        public async Task<Tuple<bool, string>> UpdateUserRole(RegistrationViewModel obj)
        {
            int updatedStatus = -1;
            Tuple<bool, string> updateUser = null;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@UserID", obj.Id);
                    parameters.Add("@RoleID", obj.RoleID, DbType.String);

                    updatedStatus = await cxn.ExecuteScalarAsync<int>("dbo.Update_UserRole", parameters, commandType: CommandType.StoredProcedure);

                    if (updatedStatus == 0)
                    {
                        updateUser = Tuple.Create(true, "User role updated successfully.");
                    }
                    else
                        updateUser = Tuple.Create(false, "Oops! User role updatation failed.Please try again.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                updateUser = Tuple.Create(false, "Oops! User role updatation failed.Please try again.");
            }

            return updateUser;
        }

        public async Task<Tuple<bool, string, UserProfileViewModel>> UpdateUserProfile(UserProfileViewModel userProfile)
        {
            int updatedStatus = -1;
            Tuple<bool, string, UserProfileViewModel> updateUserProfile = null;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", userProfile.Id);
                    parameters.Add("@UserID", userProfile.UserID);
                    parameters.Add("@FirstName", userProfile.FirstName);
                    parameters.Add("@LastName", userProfile.LastName);
                    parameters.Add("@PhoneNumber", userProfile.PhoneNumber);
                    parameters.Add("@Email", userProfile.Email);
                    parameters.Add("@FatherName", userProfile.FatherName);
                    parameters.Add("@AadharNumber", userProfile.AadharNumber);

                    updatedStatus = await cxn.ExecuteScalarAsync<int>("dbo.Update_UserProfile", parameters, commandType: CommandType.StoredProcedure);

                    if (updatedStatus == 0)
                        updateUserProfile = Tuple.Create(true, "User profile updated successfully.", userProfile);
                    else
                        updateUserProfile = Tuple.Create(false, "Oops! User profile updatation failed.Please try again.", new UserProfileViewModel());
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                updateUserProfile = Tuple.Create(false, "Oops! User profile updatation failed.Please try again.", new UserProfileViewModel());
            }

            return updateUserProfile;
        }

        public async Task<Tuple<bool, string, AddressViewModel>> UpdateUserAddress(AddressViewModel userAddress)
        {
            int updatedStatus = -1;
            Tuple<bool, string, AddressViewModel> updateUserAddress = null;

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", userAddress.Id);
                    parameters.Add("@Address1", userAddress.Address1);
                    parameters.Add("@Address2", userAddress.Address2);
                    parameters.Add("@City", userAddress.City);
                    parameters.Add("@State", userAddress.State);
                    parameters.Add("@Zipcode", userAddress.Zipcode);
                    parameters.Add("@Country", userAddress.Country);

                    updatedStatus = await cxn.ExecuteScalarAsync<int>("dbo.Update_UserAddress", parameters, commandType: CommandType.StoredProcedure);

                    if (updatedStatus == 0)
                        updateUserAddress = Tuple.Create(true, "User address updated successfully.", userAddress);
                    else
                        updateUserAddress = Tuple.Create(false, "Oops! User address updatation failed.Please try again.", new AddressViewModel());
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                updateUserAddress = Tuple.Create(false, "Oops! User address updatation failed.Please try again.", new AddressViewModel());
            }

            return updateUserAddress;
        }

        public async Task<Tuple<bool, string, UserNomineeDetailsViewModel>> AddOrModifyNomineeDetails(UserNomineeDetailsViewModel userNomineeDatails)
        {
            Tuple<bool, string, UserNomineeDetailsViewModel> updateUserNominee = null;
            UserNomineeDetailsViewModel objUserNomineeDetailsViewModel = new UserNomineeDetailsViewModel();

            try
            {
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@NomineeID", userNomineeDatails.NomineeID);
                    parameters.Add("@UserID", userNomineeDatails.UserID);
                    parameters.Add("@NomineeName", userNomineeDatails.NomineeName);
                    parameters.Add("@NomineeRelationshipID", userNomineeDatails.NomineeRelationshipID);
                    parameters.Add("@NomineeAddress", userNomineeDatails.NomineeAddress);
                    parameters.Add("@PhoneNumber", userNomineeDatails.PhoneNumber);
                    parameters.Add("@CreatedBy", userNomineeDatails.UserID);
                    parameters.Add("@NomineeRelations", userNomineeDatails.OtherRelationship);

                    var userNomineeDetailsViewModel = await cxn.QueryAsync<UserNomineeDetailsViewModel>("dbo.InsertorUpdate_UserNomineeDetails", parameters, commandType: CommandType.StoredProcedure);
                    objUserNomineeDetailsViewModel = userNomineeDetailsViewModel.FirstOrDefault();

                    if (objUserNomineeDetailsViewModel != null)
                        updateUserNominee = Tuple.Create(true, "User nominee details added/updated successfully.", objUserNomineeDetailsViewModel);
                    else
                        updateUserNominee = Tuple.Create(false, "Oops! User nominee details added/updatation failed.Please try again.", new UserNomineeDetailsViewModel());
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                updateUserNominee = Tuple.Create(false, "Oops! User nominee details added/updatation failed.Please try again.", new UserNomineeDetailsViewModel());
            }

            return updateUserNominee;
        }
        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            try
            {
                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    // create sp parameters
                    DynamicParameters spParams = new DynamicParameters();

                    spParams.Add("@Id", token.Id, DbType.String);
                    spParams.Add("@Subject", token.Subject, DbType.String);
                    spParams.Add("@ClientId", token.ClientId, DbType.String);
                    spParams.Add("@IssuedUtc", token.IssuedUtc, DbType.Date);
                    spParams.Add("@ExpiresUtc", token.ExpiresUtc, DbType.Date);
                    spParams.Add("@ProtectedTicket", token.ProtectedTicket, DbType.String);

                    await cxn.ExecuteScalarAsync<int>("[dbo].[Insert_RefreshToken]", spParams, commandType: CommandType.StoredProcedure);
                    cxn.Close();
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
                return false;
            }
            return true;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            RefreshToken refreshTokens = null;

            using (SqlConnection cxn = new SqlConnection(_dcDb))
            {
                DynamicParameters spParams = new DynamicParameters();
                spParams.Add("@Id", refreshTokenId, DbType.String);
                // execute sp                    
                using (var multi = cxn.QueryMultiple("[dbo].[Select_RefreshToken]", spParams, commandType: CommandType.StoredProcedure))
                {
                    refreshTokens = multi.Read<RefreshToken>().SingleOrDefault();
                }
                cxn.Close();
            }

            return refreshTokens;
        }

        public void Dispose()
        {
            _roleManager.Dispose();
            _userManager.Dispose();
        }
    }
}