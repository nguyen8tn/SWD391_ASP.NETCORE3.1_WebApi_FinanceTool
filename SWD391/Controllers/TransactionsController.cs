using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        [Route("add-account")]
        public async Task<ActionResult<Account>> AddAccount([FromBody] Account userSaving)
        {
            try
            {
                if (await _transactionService.AddAccount(userSaving))
                {
                    return StatusCode(StatusCodes.Status201Created, userSaving);
                }
                else {
                    return BadRequest();
                };
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }


        [HttpGet]
        [Route("get-account-by-id/{id}")]
        public async Task<ActionResult<Account>> GetAccountsByID(int id)
        {
            try
            {
                var list = await _transactionService.GetAccountByID(id);
            if (list != null)
            {
                return Ok(list);
            }
            }
            catch (Exception e)
            {

                return BadRequest(new { Message = e.Message });
            }
            return NotFound("Not Found This Account");
        }
        [HttpGet]
        [Route("get-accounts-by-uid-type/{uid}")]
        public async Task<ActionResult<Account>> GetAccountsByUID(string uid, [FromQuery] int type)
        {
            try
            {
                var list = await _transactionService.GetAccountsByUIDAndType(uid, type);
                if (list != null)
                {
                    return Ok(list);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return NotFound("Somthing Error");
        }
        [HttpPut]


        [HttpPut]
        [Route("update-account/{id}")]
        public async Task<ActionResult<IEnumerable<Account>>> UpdateAccounts(int id, [FromBody] Account account)
        {
            try
            {
                var baseAccount= await _transactionService.GetAccountByID(id);
                if (baseAccount != null)
                {   
                    bool t = await _transactionService.UpdateAccount(account);
                    if (t)
                    {
                       return StatusCode(StatusCodes.Status200OK,account);
                    } else
                    {
                       return BadRequest("Cannot Update");
                    }

                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return NotFound("Somthing Error");
        }

        [HttpDelete]
        [Route("delete-account/{id}")]
        public async Task<ActionResult<IEnumerable<Account>>> DeleteAccounts(int id) 
        {
            try
            {
                var baseAccount = await _transactionService.GetAccountByID(id);
                if (baseAccount != null)
                {
                    bool t = await _transactionService.DeleteAccount(baseAccount);
                    if (t)
                    {
                        return Ok(baseAccount);
                    }
                    else
                    {
                        return BadRequest("Cannot Delete");
                    }

                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return NotFound("Somthing Error");
        }
    }
}