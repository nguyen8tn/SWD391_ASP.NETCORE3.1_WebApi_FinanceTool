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

        // GET: api/Banks
        [HttpGet]
        [EnableQuery(PageSize = 2)]
        //[Route("", Name = "GetBank")]
        //[EnableQuery(AllowedQueryOptions = Select | Top | Skip | Count | Filter)]
        public async Task<ActionResult<IEnumerable<Bank>>> GetBank()
        {
            //await _bankService.GetBanks();
            return await _context.Bank.ToListAsync();
        }
        //public ActionResult<PagedCollectionResponse<Bank>> GetBank()
        //{
        //    return _context.Bank.ToList();
        //    //try
        //    //{
        //    //    //Filtering logic 
        //    //    Func<QueryParams, IEnumerable<Bank>> listBank = filterModel =>
        //    //    {
        //    //        return _context.Bank.Where(n => n.Name.Contains(filterModel.Term))
        //    //        .Skip((filterModel.Page - 1) * filter.Limit)
        //    //        .Take(filterModel.Limit);
        //    //    };
        //    //    var list = _context.Bank.ToList();
        //    //    var result = new PagedCollectionResponse<Bank>();
        //    //    result.Items = listBank(filter);
        //    //    //
        //    //    //int te;

        //    //    //te = listBank(filter).Count();
        //    //    string nextUrl = "", previousUrl = "";
        //    //    var jsonString = JsonConvert.SerializeObject(filter);

        //    //    //Get next page URL string  
        //    //    QueryParams nextFilter = JsonConvert.DeserializeObject<QueryParams>(jsonString);
        //    //    nextFilter.Page += 1;
        //    //    if (listBank(nextFilter).Count() > 0)
        //    //    {
        //    //        nextUrl = Url.Action("get", "banks", nextFilter, Request.Scheme);
        //    //    }
        //    //    //Get previous page
        //    //    QueryParams preFilter = JsonConvert.DeserializeObject<QueryParams>(jsonString);
        //    //    preFilter.Page -= 1;
        //    //    if (preFilter.Page > 0)
        //    //    {
        //    //        previousUrl = Url.Action("Get", "Banks", preFilter);
        //    //    }
        //    //    if (!nextUrl.Equals(""))
        //    //    {
        //    //        result.NextPage = new Uri(nextUrl);
        //    //    }
        //    //    if (!previousUrl.Equals(""))
        //    //    {
        //    //        result.PreviousPage = new Uri(previousUrl);
        //    //    }
        //    //    return Ok(result);
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    return StatusCode(StatusCodes.Status500InternalServerError, e);
        //    //}

        //}

        // GET: api/Banks/5
        [HttpGet("{id}")]
        public ActionResult<Bank> GetBank(string id)
        {
            try
            {
                var bank = _context.Bank.Find(id);
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
        public IActionResult PutBank(string id)
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
        public ActionResult<Bank> DeleteBank(string id)
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

        private bool BankExists(string id)
        {
            return _context.Bank.Any(e => e.Id == id);
        }
    }
}
