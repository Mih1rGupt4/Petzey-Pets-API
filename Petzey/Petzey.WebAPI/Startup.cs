using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;

[assembly: OwinStartup(typeof(Petzey.WebAPI.Startup))]

namespace Petzey.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //var issuer = "https://localhost:44392/";
            //var audience = "https://localhost:44392/";
            //var secret = "your_secret_key"; 

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("X5eXk4xyojNFum1kl2Ytv8dlNP4-c57dO6QGTVBwaNk"))
            };

            var options = new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = tokenValidationParameters
            };

            app.UseJwtBearerAuthentication(options);
        }
    }
}
