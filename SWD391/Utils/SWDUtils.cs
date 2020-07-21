using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWD391.Utils
{
    public class SWDUtils
    {
        public static void ValidateHttpHeader()
        {

        }
        public static async Task<string> setRoleAsync(string role, string uid)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("serviceAccountKey.json"),
            });
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Role, role }
            };
            var token = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid, claims);
            return token;
        }

        public static bool isAdmin(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            token = token.Replace("Bearer ", "");
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            var role = tokenS.Claims.First(claim => claim.Type == "role").Value;
            if (role == null || role.Equals("user"))
            {
                return false;
            }
            return true;
        }
    }
}
