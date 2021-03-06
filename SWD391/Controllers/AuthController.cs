﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using SWD391.Utils;
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
        public async Task<ActionResult<UserResponse>> Login([FromBody] User user)
        {
            if (user.Uid == null)
            {
                return NotFound("This user is not valid");
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
            response.JwtString = await SWDUtils.SetRoleAsync("user", user.Uid);
            return Ok(response);
        }

        [HttpPost]
        [Route("login-admin")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> LoginAdmin([FromQuery] string uid)
        {
            UserResponse response = new UserResponse();
            response.JwtString = await SWDUtils.SetRoleAsync("admin", uid);
            return Ok(response);
        }

    }
}
