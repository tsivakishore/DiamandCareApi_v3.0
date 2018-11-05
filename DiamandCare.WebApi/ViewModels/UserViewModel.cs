using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamandCare.WebApi
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }        
    }
    public class UserIDNameModel
    {        
        public int UserID { get; set; }
        public string UserName { get; set; }       
    }
    public class UserRolesViewModel
    {
        public string Id { get; set; }             
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public List<UserRoleMapping> Roles { get; set; }
    }

    public class UserRoleMapping
    {
        public string UserId { get; set; }      
        public string RoleId { get; set; }
        public string RoleName { get; set; } 
    }

    public class UserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }

    public class UserSponserJoineeModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public bool IsSponserJoineesReq { get; set; }
    }
    public class UserReportModel
    {
        public string Id { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FatherName { get; set; }
        public string AadharNumber { get; set; }
        public int ParentID { get; set; }
        public int SponserID { get; set; }
        public string SponserName { get; set; }
        public int UnderID { get; set; }
        public string UnderName { get; set; }
        public string RegisterFrom { get; set; }
        public int SourceID { get; set; }
        public string SourceName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DcID { get; set; }
        public string SecretKey { get; set; }
        public int UserStatusID { get; set; }
        public string UserStatus { get; set; }
        public bool LoanWaiveOff { get; set; }
        public bool IsSponserJoineesReq { get; set; }
    }

    public class UserImageModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string ImageName { get; set; }
        public byte[] ImageContent { get; set; }        
    }
}
