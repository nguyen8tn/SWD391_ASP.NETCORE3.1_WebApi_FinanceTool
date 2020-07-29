using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SWD391.Data;
using SWD391.Models;
using static SWD391.Service.IAppServices;
using SWD391.Service;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using SWD391.Utils;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace SWD391.Controllers
{
    [Route("api/[controller]")]
    //[ApiExplorerSettings(IgnoreApi = false)]
    public class BanksController : ControllerBase
    {
        private readonly SWD391Context _context;
        private IBankService _bankService;

        public BanksController(SWD391Context context, IBankService service)
        {
            _context = context;
            _bankService = service;
        }
        [HttpGet("crawl-list-bank")]
        public async Task<ActionResult<List<HtmlNode>>> CrawlBankPage()
        {
            WebScrapingService scrapingService = new WebScrapingService();
            var value = await scrapingService.CrawlListBankInMainAsync("https://thebank.vn/danh-ba-ngan-hang.html");
            return Ok();
        }

        // GET: api/Banks
        [HttpGet]
        [EnableQuery]
        [Route("get-bank")]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Bank>>> GetBank()
        {
            try
            {
                //var accessToken = Request.Headers[HeaderNames.Authorization];
                //if (!await SWDUtils.VerifyTokenAsync(accessToken, "admin"))
                //{
                //    return Unauthorized();
                //}
                var list = await _bankService.GetBanks();
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = e.Message});
            }
        }

        // GET: api/Banks/5
        [HttpGet("get-details/{id}")]
        public async Task<ActionResult<Bank>> GetBank(int id)
        {
            try
            {
                var bank = await _context.Banks.FindAsync(id);
                if (bank == null)
                {
                    return NotFound(new { message = "Not Found!" });
                }
                return Ok(bank);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT: api/Banks/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Bank>> PutBank(int id, [FromBody] Bank bank)
        {
            try
            {
                //string authHeader = Request.Headers["Authorization"];
                //if (!Utils.SWDUtils.isAdmin(authHeader))
                //{
                //    return Unauthorized(new { Message = "Access Denied!" });
                //}
                if (!BankExists(id))
                {
                    return NotFound(new { Message = "Not Found!" });
                }
                else
                {
                    if (await _bankService.UpdateBankAsync(bank))
                    {
                        return Ok(bank);
                    }
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = e.Message });
            }
        }


        [HttpPost("create")]
        public async Task<ActionResult<Bank>> PostBank([FromBody] Bank bank)
        {
            try
            {
                //string authHeader = Request.Headers["Authorization"];
                //if (!Utils.SWDUtils.isAdmin(authHeader))
                //{
                //    return Unauthorized(new { Message = "Access Denied!" });
                //}                
                if (await _bankService.AddBankAsync(bank))
                {
                    return StatusCode(StatusCodes.Status201Created, bank);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = e.Message });
            }
        }

        // DELETE: api/Banks/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Bank>> DeleteBank(int id)
        {
            try
            {
                //string authHeader = Request.Headers["Authorization"];
                //if (!Utils.SWDUtils.isAdmin(authHeader))
                //{
                //    return Unauthorized(new { Message = "Access Denied!" });
                //} 
                var bank = await _bankService.GeBankByIDAsync(id);
                if (bank == null)
                {
                    return NotFound();
                }
                else
                {
                    if (await _bankService.DeleteBankAsync(bank))
                    {
                        return NoContent();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = e.Message });
            }
        }
        private bool BankExists(int id)
        {
            return _context.Banks.Any(e => e.Id == id);
        }
    }
}
