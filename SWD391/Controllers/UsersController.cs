﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SWD391.Data;
using SWD391.Models;

namespace SWD391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SWD391Context _context;

        public UsersController(SWD391Context context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        [AllowAnonymous]
        [EnableQuery(PageSize = 2)]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            //bool t = HttpContext.Users.Identity.IsAuthenticated;
            return await _context.Users.ToListAsync();
        }

        [HttpPost]
        [Route("details/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> GetUserDetails(string uid)
        {
            var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var user = JsonConvert.DeserializeObject<User>(body);
            if (user.Uid == null)
            {
                return NotFound();
            }
            var baseUser = await _context.Users.FindAsync(user.Uid);

            if (baseUser == null)
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, [FromBody] User user)
        {
            if (id != user.Uid)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Uid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.Uid }, user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Uid == id);
        }
    }
}
