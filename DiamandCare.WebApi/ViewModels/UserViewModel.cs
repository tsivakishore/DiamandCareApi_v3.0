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
}
