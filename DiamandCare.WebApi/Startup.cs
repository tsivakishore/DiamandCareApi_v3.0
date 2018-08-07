using Castle.Windsor;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

[assembly: OwinStartup("DiamandCareAPI", typeof(DiamandCare.WebApi.Startup))]
namespace DiamandCare.WebApi
{
    public class Startup
    {
        private readonly IWindsorContainer _container;

        public Startup()
        {
            _container = new WindsorContainer().Install(new RepositoriesInstaller());
        }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            // dependency resolver
            config.DependencyResolver = new WindsorDependencyResolver(_container);

            // configure authentication
            ConfigureOAuth(app);
            var properties = new Microsoft.Owin.BuilderProperties.AppProperties(app.Properties);
            var token = properties.OnAppDisposing;
            if (token != System.Threading.CancellationToken.None)
            {
                token.Register(Close);
            }

            // Configure Web API for self-host. 
            config.EnableCors(new EnableCorsAttribute("*", "*", "GET, POST, OPTIONS, PUT, DELETE"));

            // register routing
            WebApiConfig.Register(config, _container);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(9),
                Provider = new TokenAuthProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };


            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            app.UseOAuthBearerTokens(OAuthServerOptions);
        }

        public void Close()
        {
            if (_container != null)
                _container.Dispose();
        }
    }
}