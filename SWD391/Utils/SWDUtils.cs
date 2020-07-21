using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace SWD391.Utils
{
    public class SWDUtils
    {
        public static void ValidateHttpHeader()
        {

        }
        public static string setRole(string role, string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            //get headder
            //remove Bearer prefix
            authHeader = authHeader.Replace("Bearer ", "");
            //cast to jwtlet
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            //add new custome claims
            tokenS.Payload["role"] = role;
            //var id = tokenS.Claims.First(claim => claim.Type == "nameid").Value;
            var jwt = handler.WriteToken(tokenS);
            return jwt;
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
