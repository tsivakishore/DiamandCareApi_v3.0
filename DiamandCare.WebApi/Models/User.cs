using DiamandCare.WebApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class User
    {
        public string Id { get; set; }
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; }
        public string RoleID { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PhoneNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string EthAddress { get; set; }
        public string FatherName { get; set; }
        public string AadharNumber { get; set; }
        public int ParentID { get; set; }
        public int SponserID { get; set; }
        public int SourceID { get; set; }
        public string SecretCode { get; set; }
        public int UnderID { get; set; }
        public string Position { get; set; }
        public string PositionsCompleted { get; set; }
        public string DcID { get; set; }
        public int UserStatusID { get; set; }
    }
}