using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiamandCare.WebApi.Repository;
using DiamandCare.WebApi.ViewModels;
using DiamandCare.WebApi;
using DiamandCare.WebApi.Models;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using DiamandCare.Core;

namespace DiamandCare.WebApi.Controllers
{

    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private AuthRepository _repo = null;

        public UserController(AuthRepository autRepisitory)
        {
            _repo = autRepisitory;
        }

        [Authorize]
        [Route("registeruser")]
        [HttpPost]
        public async Task<Tuple<bool, string>> RegisterUser(User model)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.RegisterUser(model);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("getallusers")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<RegistrationViewModel>>> GetAllUsers()
        {
            //List<RegistrationViewModel> allUsers = null;
            Tuple<bool, string, List<RegistrationViewModel>> result = null;
            try
            {
                result = await _repo.GetAllUsers();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("updateuser")] //UpdateUserDetails
        [HttpPost]
        public async Task<Tuple<bool, string>> UpdateUserDetails(RegistrationViewModel updateUser)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.UpdateUserDetails(updateUser);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("getuserrole")] //GetUserRole
        [HttpPost]
        public async Task<Tuple<bool, string, int>> GetUserRole(User updateUser)
        {
            Tuple<bool, string, int> result = null;
            try
            {
                result = await _repo.GetUserRole(updateUser);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("getuserbyid")]//GetUsersDetailsByID
        [HttpGet]
        public Tuple<bool, string, User> GetUserDetailsByID()
        {
            Tuple<bool, string, User> result = null;
            try
            {
                result = _repo.GetUserDetailsByID();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Route("getuserbyloginid")]//GetUsersDetailsByLoginID
        [HttpGet]
        public Tuple<bool, string, UserViewModel> GetUsersDetailsByLoginID()
        {
            Tuple<bool, string, UserViewModel> result = null;
            try
            {
                result = _repo.GetUsersDetailsByLoginID();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("usersprofile")]
        [HttpGet]
        public Tuple<bool, string, UserProfileViewModel> GetUsersProfile()
        {
            Tuple<bool, string, UserProfileViewModel> result = null;
            try
            {
                result = _repo.GetUsersProfile();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("updateuserprofile")] //UpdateUserProfile
        [HttpPost]
        public async Task<Tuple<bool, string, UserProfileViewModel>> UpdateUserProfile(UserProfileViewModel userProfile)
        {
            Tuple<bool, string, UserProfileViewModel> result = null;
            try
            {
                result = await _repo.UpdateUserProfile(userProfile);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("useraddress")]
        [HttpGet]
        public Tuple<bool, string, AddressViewModel> GetUsersAddress()
        {
            Tuple<bool, string, AddressViewModel> result = null;
            try
            {
                result = _repo.GetUsersAddress();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("updateuseraddress")] //UpdateUserAddress
        [HttpPost]
        public async Task<Tuple<bool, string, AddressViewModel>> UpdateUserAddress(AddressViewModel userAddress)
        {
            Tuple<bool, string, AddressViewModel> result = null;
            try
            {
                result = await _repo.UpdateUserAddress(userAddress);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("getnomineedetails")]
        [HttpGet]
        public Tuple<bool, string, UserNomineeDetailsViewModel> GetUsersNomineeDetails(int UserID)
        {
            Tuple<bool, string, UserNomineeDetailsViewModel> result = null;
            try
            {
                result = _repo.GetUsersNomineeDetails(UserID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("addupdatenomineedetails")] //Add Or Modify Nominee Details
        [HttpPost]
        public async Task<Tuple<bool, string, UserNomineeDetailsViewModel>> AddOrModifyNomineeDetails(UserNomineeDetailsViewModel userNomineeDatails)
        {
            Tuple<bool, string, UserNomineeDetailsViewModel> result = null;
            try
            {
                result = await _repo.AddOrModifyNomineeDetails(userNomineeDatails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("changepassword")] //UpdateUserAddress
        [HttpPost]
        public async Task<Tuple<bool, string>> ChangePassword(ForgetPasswordModel forgetPasswordModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.ChangePassword(forgetPasswordModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Route("getmenu")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<MenuViewModel1>>> GetMenu(string userID)
        {
            Tuple<bool, string, List<MenuViewModel1>> result = null;
            try
            {
                result = await _repo.GetMenu(userID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Route("forgetpassword")]
        [HttpPost]
        public async Task<Tuple<bool, string>> ForgetePassword(ForgetPasswordModel forgetPasswordModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.ForgetePassword(forgetPasswordModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Route("getusersandrole")] //GetUsers and Role
        [HttpGet]
        public async Task<Tuple<bool, string, List<UserRolesViewModel>>> GetUserAndRoles()
        {
            Tuple<bool, string, List<UserRolesViewModel>> result = null;
            try
            {
                result = await _repo.GetUserAndRoles();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Route("updateuserrole")] //UpdateUserRole
        [HttpPost]
        public async Task<Tuple<bool, string>> UpdateUserRole(UserRolesViewModel obj)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.UpdateUserRole(obj);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Route("getroles")] //GetAllRoles
        [HttpGet]
        public async Task<Tuple<bool, string, List<Role>>> GetAllRoles()
        {
            Tuple<bool, string, List<Role>> result = null;
            try
            {
                result = await _repo.GetAllRoles();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }
        [Authorize]
        [Route("updateloanwaiveoff")]
        [HttpGet]
        public async Task<Tuple<bool, string>> UpdateLoanWaiveoff(int UserID, bool LoanWaiveoff)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.UpdateLoanWaiveoff(UserID, LoanWaiveoff);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("getfreetopaiduserdetails")]
        [HttpGet]
        public async Task<Tuple<bool, string, UserIDNameModel, MasterCharges>> getfreetopaiduserdetails(string DcIDorName)
        {
            Tuple<bool, string, UserIDNameModel, MasterCharges> result = null;
            try
            {
                result = await _repo.getfreetopaiduserdetails(DcIDorName);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("getUserSponserJoineeRequired")]
        [HttpGet]
        public async Task<Tuple<bool, string, UserSponserJoineeModel>> GetUserSponserJoineeRequired(string DcIDorName)
        {
            Tuple<bool, string, UserSponserJoineeModel> result = null;
            try
            {
                result = await _repo.GetUserSponserJoineeRequired(DcIDorName);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("updatefreetopaidkeydetails")]
        [HttpGet]
        public async Task<Tuple<bool, string>> updatefreetopaidkeydetails(int UserID, decimal KeyCost)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.updatefreetopaidkeydetails(UserID, KeyCost);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("UpdateUserSponserJoineeRequired")]
        [HttpGet]
        public async Task<Tuple<bool, string>> UpdateUserSponserJoineeRequired(int UserID, bool isSponserJoineesReq)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.UpdateUserSponserJoineeRequired(UserID, isSponserJoineesReq);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        //Get user details by DCIC or Username
        [Authorize]
        [Route("UserDetailsByDCIDOrUserName")]
        [HttpGet]
        public async Task<Tuple<bool, string, UserProfileViewModel>> GetUserDetailsByDCIDOrUserName(string DCIDorName)
        {
            Tuple<bool, string, UserProfileViewModel> result = null;
            try
            {
                result = await _repo.GetUserDetailsByDCIDOrUserName(DCIDorName);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("useraddressbyId")]
        [HttpGet]
        public Tuple<bool, string, AddressViewModel> GetUsersAddressById(string Id)
        {
            Tuple<bool, string, AddressViewModel> result = null;
            try
            {
                result = _repo.GetUsersAddressById(Id);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }

        [Authorize]
        [Route("changepasswordbyId")] //UpdateUserAddress
        [HttpPost]
        public async Task<Tuple<bool, string>> ChangePasswordById(ForgetPasswordModel forgetPasswordModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.ChangePasswordById(forgetPasswordModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return result;
        }
    }
}