using DiamandCare.WebApi.Properties;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace DiamandCare.WebApi
{
    public class BaseController : ApiController
    {
        private string _diamandCareDB = Settings.Default.DiamandCareConnection;

        public ClaimsPrincipal ClaimsPrincipal
        {
            get
            {
                return RequestContext.Principal as ClaimsPrincipal;
            }
        }

        public string UserID
        {
            get
            {
                if (ClaimsPrincipal == null)
                    return null;
                return ClaimsPrincipal.Identity.GetUserId();
            }
        }

        public UserPrincipal UserPrincipal
        {
            get
            {
                return User as UserPrincipal;
            }
        }
        //User identity
        public UserIdentity Identity
        {
            get
            {
                if (UserPrincipal == null)
                    return null;
                return UserPrincipal.Identity as UserIdentity;
            }

        }
    }
}
