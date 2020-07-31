using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD391.Data;
using SWD391.Models;

namespace SWD391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly SWD391Context _context;
        public RateController(SWD391Context context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get-rates-of-bank")]
        public async Task<ActionResult<Bank>> GetBanksSavingPackageByBankID(int id)
        {
            try
            {
                var parent = await _context.Banks
                .Include(p => p.Rates)
                .SingleOrDefaultAsync(p => p.Id == id);
                if (parent != null)
                {
                    parent.Rates = parent.Rates.OrderByDescending(x => x.Name).ToList();
                    return Ok(parent);
                } else
                {
                    return NotFound(new { Message = "Not Found this Bank" });
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = e.Message });
            }
        }

        [HttpPost]
        [Route("add-rate")]
        public async Task<ActionResult<Rate>> AddRate([FromBody] List<Rate> rates)
        {
            try
            {
                await _context.Rates.AddRangeAsync(rates);
                bool check = await _context.SaveChangesAsync() > 0;
                if (check)
                {
                    return StatusCode(StatusCodes.Status201Created, rates);
                }
                return Conflict();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = e.Message });
            }
        }
    }
}