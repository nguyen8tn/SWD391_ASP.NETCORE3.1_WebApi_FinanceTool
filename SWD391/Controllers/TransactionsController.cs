using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SWD391.Data;
using SWD391.Models;
using static SWD391.Models.Transaction;
using static SWD391.Service.IAppServices;


namespace SWD391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly SWD391Context _context;

        private readonly ITransactionService _transactionService;
        public TransactionsController(SWD391Context context, ITransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route("add-saving-account")]
        public async Task<ActionResult<SavingAccount>> AddSavingAccount([FromBody] SavingAccount userSaving)
        {
            try
            {
                var tmp = _context.SavingAccounts.FirstOrDefault(i => i.ID.Equals(userSaving.ID));
                if (tmp == null)
                {
                    SavingAccount t = userSaving;
                    await _context.SavingAccounts.AddAsync(t);
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

        [HttpPost]
        [Route("get-loan-accounts/{id}")]
        public void GetLoanAccounts()
        {

        }

        [HttpGet]
        [Route("get-saving-accounts/{id}")]
        public async Task<ActionResult<IEnumerable<SavingAccount>>> GetSavingAccounts(string id)
        {
            try
            {
                var list = await _transactionService.GetSavingAccountByID(id);
            if (list != null)
            {
                return Ok(list);
            }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            return NotFound("Somthing Error");
        }

        [HttpPut]
        [Route("update-loan-accounts/{id}")]
        public void UpdateLoanAccounts()
        {

        }

        [HttpPut]
        [Route("update-saving-accounts/{id}")]
        public async Task<ActionResult<IEnumerable<SavingAccount>>> UpdateSavingAccounts(string uid, [FromBody] SavingAccount account)
        {
            try
            {
                var baseAccount= await _transactionService.GetSavingAccountByID(uid);
                if (baseAccount != null)
                {
                    bool t = await _transactionService.UpdateSavingAccount(account);
                    if (t)
                    {
                        TransactionControllerResponse response = new TransactionControllerResponse();
                        string[] cars = new string[4] { "Volvo", "BMW", "Ford", "Mazda" };
                        response.value.Add("obj",cars);
                       return Ok(account);
                    } else
                    {
                       return BadRequest("Cannot Update");
                    }

                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return NotFound("Somthing Error");
        }

        [HttpPut]
        [Route("delete-saving-accounts/{id}")]
        public async Task<ActionResult<IEnumerable<SavingAccount>>> DeleteSavingAccounts(string uid, [FromBody] SavingAccount account)
        {
            try
            {
                var baseUser = await _transactionService.GetSavingAccountByID(uid);
                if (baseUser != null)
                {
                    bool t = await _transactionService.DeleteSavingAccount(account);
                    if (t)
                    {
                        return Ok(account);
                    }
                    else
                    {
                        return BadRequest("Cannot Update");
                    }

                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return NotFound("Somthing Error");
        }
    }
}