using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace DiamandCare.WebApi
{
    public interface ICustomPrincipal : System.Security.Principal.IPrincipal
    {
    }
    public class UserPrincipal : ICustomPrincipal
    {
        public IIdentity Identity { get; private set; }

        public UserPrincipal(UserIdentity customIdentity)
        {
            this.Identity = customIdentity;
        }

        public bool IsInRole(string role)
        {
            return true;
        }
    }

    public class UserIdentity : IIdentity
    {

        public int User_ID { get; set; }

        public string User_ID_EN { get; set; }

        //public string User_Name { get; set; }

        public string Name { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Email { get; set; }

        public int? Role_ID { get; set; }

        public string Profile_Image { get; set; }

        public bool? Menu_Visible_Flag { get; set; }


        public UserIdentity(string name)
        {
            this.Name = name;
        }

        public UserIdentity()
        {
            // TODO: Complete member initialization
        }


        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(this.Name); }
        }
    }
}