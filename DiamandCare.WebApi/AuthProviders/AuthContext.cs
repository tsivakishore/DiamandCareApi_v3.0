using DiamandCare.WebApi.Properties;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{

    public class ApplicationUser : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public string RoleID { get; set; }
        public bool AllowLoginFlag { get; set; }
        public bool WelcomeEmailSentFlag { get; set; }
        public string EthAddress { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public bool IsActive { get; set; }
        public bool DefaultFlag { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        public AuthContext()
           : base(Settings.Default.DiamandCareConnection)
        {
        }

        //public DbSet<Client> Clients { get; set; }
        //public DbSet<RefreshToken> RefreshTokens { get; set; }
    }

}