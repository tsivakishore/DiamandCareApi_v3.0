using DiamandCare.WebApi.Repository;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace DiamandCare.WebApi
{
    public class TokenAuthProvider : OAuthAuthorizationServerProvider
    {
        private AuthRepository _repo = new AuthRepository();

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var clientId = string.Empty;
            var clientSecret = string.Empty;
            ApiClient client = null;

            // TODO: Kris commented this. Uncomment once the all the code is ready.
            // get the client credentials
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            // decline unauthorized client
            if (context.ClientId == null)
            {
                context.Rejected();
                context.SetError("invalid_client", "Client credentials could not be retrieved through the Authorization header.");
                return;
            }

            // get the client details from the database
            client = await _repo.FindClient(context.ClientId);
            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return;
            }
            if (!client.IsActive)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return;
            }

            // validate client only if required
            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return;
                }
                else
                {
                    string hashedClientSecret = Helper.GetHash(clientSecret);
                    if (client.Secret != hashedClientSecret)
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        return;
                    }
                }
            }
            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());
            context.OwinContext.Set<ApiClient>("oauth:client", client);
            context.Validated(clientId);

            //context.Validated();
            return;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            User user = null;
            string roleId = string.Empty;
            string roleName = string.Empty;
            //List<UserRolesModel> roles = null;
            //List<UserPermissionNamesModel> perms = null;

            using (AuthRepository _repo = new AuthRepository())
            {
                IdentityUser iuser = await _repo.FindUser(context.UserName, context.Password);

                if (iuser == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                foreach (IdentityUserRole role in iuser.Roles)
                {
                    roleId = role.RoleId;
                }

                if (roleId == "928f6866-a684-412f-a68c-30fdba25885b")
                    roleName = "Admin";
                else if (roleId == "a4d94ea6-d20a-4b17-b8c2-cf299edb254a")
                    roleName = "User";
                else if (roleId == "9cc2f65f-7cc5-4ddb-b162-760775879796")
                    roleName = "Franchise";

                user = new User
                {
                    Id = iuser.Id,
                    UserName = iuser.UserName,
                    RoleID = roleId,
                    RoleName = roleName
                };
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, roleId));

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                {"as:client_id", context.ClientId ?? string.Empty},
                {"userName", context.UserName},
                {"userId", user.Id},
                {"roleId", roleId},
                {"roleName", user.RoleName}
            }
       );


            var ticket = new AuthenticationTicket(identity, properties);
            context.Validated(ticket);

            context.Validated(identity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }

    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (AuthRepository _repo = new AuthRepository())
            {
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                var token = new RefreshToken()
                {
                    Id = Helper.GetHash(refreshTokenId),
                    ClientId = clientid,
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                token.ProtectedTicket = context.SerializeTicket();

                var result = await _repo.AddRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }

            }
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = Helper.GetHash(context.Token);

            using (AuthRepository _repo = new AuthRepository())
            {
                var refreshToken = await _repo.FindRefreshToken(hashedTokenId);

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    var result = await _repo.RemoveRefreshToken(hashedTokenId);
                }
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}