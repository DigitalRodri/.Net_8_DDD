//using Domain.Resources;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Web.Configuration;
//using System.Web.Http;
//using System.Web.Http.Controllers;

//namespace Application.Tags
//{
//    public class RequiresAuthorization : AuthorizeAttribute
//    {
//        protected override bool IsAuthorized(HttpActionContext actionContext)
//        {
//            // Validate the Authorization header Bearer jwt token using the Key and Issuer values
//            if (actionContext.Request.Headers.Authorization == null)
//                throw new ArgumentException(Resources.NoAuthorization, "Request.Headers.Authorization");

//            string jwtToken = actionContext.Request.Headers.Authorization.Parameter;
//            var tokenHandler = new JwtSecurityTokenHandler();

//            try
//            {
//                tokenHandler.ValidateToken(jwtToken, GetTokenValidationParameters(), out SecurityToken validatedToken);
//            }
//            catch (SecurityTokenValidationException)
//            {
//                return false;
//            }

//            return true;
//        }

//        protected private TokenValidationParameters GetTokenValidationParameters()
//        {
//            string issuerSigningKey = WebConfigurationManager.AppSettings["IssuerSigningKey"];
//            string validIssuer = WebConfigurationManager.AppSettings["ValidIssuer"];

//            return new TokenValidationParameters
//            {
//                ValidateIssuerSigningKey = true,
//                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(issuerSigningKey)),
//                ValidateIssuer = true,
//                ValidIssuer = validIssuer,
//                ValidateAudience = false,
//                ValidateLifetime = true,
//                ClockSkew = TimeSpan.Zero
//            };
//        }
//    }
//}