using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SWD391.Data;
using static SWD391.Models.Transaction;

namespace SWD391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly SWD391Context _context;

        public TransactionsController(SWD391Context context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("add-saving-account")]
        public async Task<ActionResult<UserSaving>> AddSavingAccount([FromBody] UserSaving userSaving)
        {
            try
            {
                var tmp = _context.UserSavings.FirstOrDefault(i => i.ID.Equals(userSaving.ID));
                if (tmp == null)
                {
                    UserSaving t = userSaving;
                    await _context.UserSavings.AddAsync(t);
                    await _context.SaveChangesAsync();
                    return Ok(userSaving);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("add-loan-account")]
        public string AddLoanAccount()
        {
            string asf = "loan";
            return asf;
        }

        [HttpGet]
        [Route("get-loan-accounts/{uid}")]
        public async Task<ActionResult<IEnumerable<UserSaving>>> GetLoanAccounts(string uid)
        {
            try
            {
                var list = await _context.UserSavings.Where(x => x.UID.Equals(uid)).ToListAsync();
            if (list != null)
            {
                return Ok(list);
            }
            }
            catch (Exception e)
            {

                throw;
            }
            return BadRequest();
        }
    }
}