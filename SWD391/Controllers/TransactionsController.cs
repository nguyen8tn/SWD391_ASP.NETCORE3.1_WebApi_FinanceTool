using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD391.Data;

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
        public string AddSavingAccount()
        {
            string asf = "asdasd";
            return "";
        } 
    }
}