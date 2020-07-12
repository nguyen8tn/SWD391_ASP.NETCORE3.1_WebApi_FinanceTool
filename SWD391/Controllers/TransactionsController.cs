using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SWD391.Data;
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

        [HttpPost]
        [Route("get-loan-accounts/{uid}")]
        public void GetLoanAccounts()
        {

        }

        [HttpGet]
        [Route("get-saving-accounts/{uid}")]
        public async Task<ActionResult<IEnumerable<UserSaving>>> GetSavingAccounts(string uid)
        {
            try
            {
                var list = await _transactionService.GetSavingAccounts(uid);
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
        [Route("update-loan-accounts/{uid}")]
        public void UpdateLoanAccounts()
        {

        }

        [HttpPut]
        [Route("update-saving-accounts/{uid}")]
        public async Task<ActionResult<IEnumerable<UserSaving>>> UpdateSavingAccounts(string uid, [FromBody] UserSaving user)
        {
            try
            {
                var baseUser = await _transactionService.GetUserByID(uid);
                if (baseUser != null)
                {
                    bool t = await _transactionService.UpdateSavingAccount(user);
                    if (t)
                    {
                       return Ok(user);
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
        [Route("delete-saving-accounts/{uid}")]
        public async Task<ActionResult<IEnumerable<UserSaving>>> DeleteSavingAccounts(string uid, [FromBody] UserSaving user)
        {
            try
            {
                var baseUser = await _transactionService.GetUserByID(uid);
                if (baseUser != null)
                {
                    bool t = await _transactionService.DeleteSavingAccount(user);
                    if (t)
                    {
                        return Ok(user);
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