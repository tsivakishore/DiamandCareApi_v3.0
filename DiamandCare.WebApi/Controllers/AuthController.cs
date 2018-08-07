using System.Security.Claims;
using System.Threading.Tasks;

using DiamandCare.WebApi.Models;

using DiamandCare.WebApi.Repository;

using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace DiamandCare.WebApi.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IJwtFactory _jwtFactory;
        //private readonly JwtIssuerOptions _jwtOptions;

        //public AuthController(UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        //{
        //    _userManager = userManager;
        //    _jwtFactory = jwtFactory;
        //    _jwtOptions = jwtOptions.Value;
        //}

        //[Route("login")]
        //[HttpPost("login")]
        ////public async Task<IActionResult> LogIn([FromBody]CredentialsViewModel credentials)
        ////{
        ////    if (!ModelState.IsValid)
        ////    {
        ////        return BadRequest(ModelState);
        ////    }

        ////    var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
        ////    if (identity == null)
        ////    {
        ////        return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
        ////    }

        ////    var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
        ////    return new OkObjectResult(jwt);
        ////}
        //public async Task<Tuple<bool, string, ApplicationUser, OkObjectResult>> LogIn(CredentialsViewModel credentials)
        //{
        //    Tuple<bool, string, ApplicationUser, OkObjectResult> result = null;
        //    ApplicationUser userDetails = null;
        //    OkObjectResult objOkObjectResult = null;
        //    userDetails = await _userManager.FindByNameAsync(credentials.UserName);        

        //    var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
        //    if (identity == null)
        //    {
        //        result = Tuple.Create(false, "Invalid username or password.", userDetails, objOkObjectResult);
        //    }
        //    else
        //    {
        //        try
        //        {
        //            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
        //            objOkObjectResult = new OkObjectResult(jwt);
        //            result = Tuple.Create(true, "", userDetails, objOkObjectResult);
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorLog.Write(ex);
        //            result = Tuple.Create(false, "Oops! Your login authintication failed.Please try again.", userDetails, objOkObjectResult);
        //        }
        //    }

        //    return result;
        //}

        //private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        //{
        //    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        //        return await Task.FromResult<ClaimsIdentity>(null);

        //    // get the user to verifty
        //    var userToVerify = await _userManager.FindByNameAsync(userName);

        //    if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

        //    // check the credentials
        //    if (await _userManager.CheckPasswordAsync(userToVerify, password))
        //    {
        //        return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
        //    }

        //    // Credentials are invalid, or account doesn't exist
        //    return await Task.FromResult<ClaimsIdentity>(null);
        //}
    }
}

