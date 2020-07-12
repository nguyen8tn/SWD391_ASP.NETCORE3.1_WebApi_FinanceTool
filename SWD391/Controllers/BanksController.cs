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
using static SWD391.Service.AppServices;
using static Microsoft.AspNet.OData.Query.AllowedQueryOptions;
using Microsoft.AspNet.OData.Query;
using static SWD391.Service.IAppServices;
using SWD391.Service;
using HtmlAgilityPack;

namespace SWD391.Controllers
{
    [Route("api/[controller]")]
    //[ApiExplorerSettings(IgnoreApi = false)]
    public class BanksController : Controller
    {
        private readonly SWD391Context _context;
        private IBankService _bankService;

        public BanksController(SWD391Context context, IBankService service)
        {
            _context = context;
            _bankService = service;
        }
        [HttpGet]
        [Route("/crawl-list-bank")]
        public async Task<ActionResult<List<HtmlNode>>> CrawlBankPage()
        {
            WebScrapingService scrapingService = new WebScrapingService();
            var value = await scrapingService.CrawlListBankInMainAsync("https://thebank.vn/danh-ba-ngan-hang.html");
            int id = 1;
            return Ok();
        }

        // GET: api/Banks
        [HttpGet]
        [EnableQuery(PageSize = 2)]
        [Route("get-bank")]
        //[Route("", Name = "GetBank")]
        //[EnableQuery(AllowedQueryOptions = Select | Top | Skip | Count | Filter)]
        public async Task<ActionResult<IEnumerable<Bank>>> GetBank()
        {
            //await _bankService.GetBanks();
            return await _context.Bank.ToListAsync();
        }

        // GET: api/Banks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bank>> GetBank(int id)
        {
            try
            {
                var bank = await _context.Bank.FindAsync(id);
                if (bank == null)
                {
                    return NotFound(new { message = "Not Found!" });
                }
                return Ok(bank);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        // PUT: api/Banks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutBank(int id)
        {
            try
            {
                if (!BankExists(id))
                {
                    return NotFound(new { Message = "Not Found!" });
                }
                else
                {
                    var reader = new StreamReader(Request.Body);
                    var body = reader.ReadToEnd();
                    var bank = JsonConvert.DeserializeObject<Bank>(body);
                    var baseBank = _context.Bank.FirstOrDefault(i => i.Id.Equals(id));
                    if (baseBank != null)
                    {
                        baseBank.Name = bank.Name;
                        baseBank.LoanRateSix = bank.LoanRateSix;
                        baseBank.LoanRateTwelve = bank.LoanRateTwelve;
                        baseBank.LoanRateTwentyFour = bank.LoanRateTwentyFour;
                        baseBank.SavingRateSix = bank.SavingRateSix;
                        baseBank.SavingRateTwelve = bank.SavingRateTwelve;
                        baseBank.SavingRateTwentyFour = bank.SavingRateTwentyFour;
                    }
                    _context.SaveChanges();
                    return Ok(baseBank);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        // POST: api/Banks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Bank> PostBank()
        {
            try
            {
                var reader = new StreamReader(Request.Body);
                var body = reader.ReadToEnd();
                var bank = JsonConvert.DeserializeObject<Bank>(body);
                _context.Bank.Add(bank);
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    if (BankExists(bank.Id))
                    {
                        return Conflict(e);
                    }
                }
                return CreatedAtAction("GetBank", new { id = bank.Id }, bank);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        // DELETE: api/Banks/5
        [HttpDelete("{id}")]
        public ActionResult<Bank> DeleteBank(int id)
        {
            try
            {
                var bank = _context.Bank.Find(id);
                if (bank == null)
                {
                    return NotFound();
                }
                _context.Bank.Remove(bank);
                _context.SaveChanges();
                return bank;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        
        private bool BankExists(int id)
        {
            return _context.Bank.Any(e => e.Id == id);
        }
    }
}
