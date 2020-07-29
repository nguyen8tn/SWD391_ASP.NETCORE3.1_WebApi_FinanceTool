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

        public static async Task<string> SetRoleAsync(string role, string uid)
        {
            var claims = new Dictionary<string, object>
            {
                { "role", role }
            };
            await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, claims);
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

        public static void WriteLog(string stringToken)
        {
            FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(stringToken);
        }

        public static async Task<bool> VerifyTokenAsync(string stringToken, string baseRole)
        {
            UserRecord user = await FirebaseAuth.DefaultInstance.GetUserAsync("Z1x73oyUWnZvlrPRPOHtKZ5ZK2K3");
            Console.WriteLine(user.CustomClaims["role"]);
            stringToken = stringToken.Replace("Bearer ", "");
            FirebaseToken decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(stringToken);
            object role;
            if (decoded.Claims.TryGetValue("role", out role))
            {
                if (role.ToString().Equals(baseRole))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
