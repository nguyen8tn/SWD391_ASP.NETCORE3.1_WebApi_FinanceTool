using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWD391.Data;
using SWD391.Models;

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
        public async Task<ActionResult<User>> Login()
        {
            var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var user = JsonConvert.DeserializeObject<User>(body);

            if (user.Uid == null)
            {
                return NotFound();
            }
            User baseUser = await _context.User.FindAsync(user.Uid);

            if (baseUser == null)
            {
                DateTime currentDay = DateTime.Now;
                //user date send from app is null -> add date
                user.CreatedDate = currentDay;
                await _context.User.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            return Ok(user);
        }
    }
}
