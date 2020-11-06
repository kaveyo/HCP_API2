using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace HCP_API.Models
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
         public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
         {
             context.Validated();
         }
         public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
         {
            // ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
           // context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (UserMasterRepository _repo = new UserMasterRepository())
             {
                 var user = _repo.ValidateUser(context.UserName, context.Password);
                 if (user == null)
                 {
                     context.SetError("invalid_grant", "Provided username and password is incorrect");
                     return;
                 }
                 if(user != null)
                 {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    if (context.UserName == "admin" && context.Password == "admin")
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, "SuperAdmin"));
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                        context.Validated(identity);
                    }
                    if (context.UserName == "zss" && context.Password == "Pq56@*!*89ste20189")
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, "SuperAdmin"));
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                        context.Validated(identity);
                    }
                    else if (context.UserName == "user" && context.Password == "user")
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                        context.Validated(identity);
                    }
                    else
                    {
                        context.SetError("invalid_grant", "Provided username and password is incorrect");
                        return;
                    }
                 /*   var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                      identity.AddClaim(new Claim(ClaimTypes.Role, "Pick"));
                      identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                      identity.AddClaim(new Claim("Email", user.UserEmailID));*/


                  
                 
                }
             }


         }

     
    }
}