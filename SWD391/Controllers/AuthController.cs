using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using SWD391.Data;
using SWD391.Models;
using static SWD391.Models.CustomResponse;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SWD391.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly SWD391Context _context;
        public AuthController(SWD391Context context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> Login()
        {
            var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var user = JsonConvert.DeserializeObject<User>(body);
            if (user.Uid == null)
            {
                return NotFound();
            }
            User baseUser = await _context.Users.FindAsync(user.Uid);

            if (baseUser == null)
            {
                DateTime currentDay = DateTime.Now;
                //user date send from app is null -> add date
                user.CreatedDate = currentDay;
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            UserResponse response = new UserResponse();
            response.JwtString = setRole("user");
            return Ok(response);
        }

        private string setRole(string role)
        {
            var handler = new JwtSecurityTokenHandler();
            //get headder
            string authHeader = Request.Headers["Authorization"];
            //remove Bearer prefix
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            //cast to jwtlet
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            //add new custome claims
            tokenS.Payload["role"] = role;
            //var id = tokenS.Claims.First(claim => claim.Type == "nameid").Value;
            var jwt = handler.WriteToken(tokenS);
            return jwt;
        }

        private bool valideRole()
        {
            return false;
        }
    }
}
