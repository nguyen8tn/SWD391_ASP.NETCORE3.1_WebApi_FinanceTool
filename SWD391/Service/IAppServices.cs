using Microsoft.AspNetCore.Mvc;
using SWD391.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD391.Service
{
    public class IAppServices
    {
        public interface IBankService
        {
            Task<ActionResult<IEnumerable<Bank>>> GetBanks();
        }
        public interface IUserService
        {
            Task<ActionResult<IEnumerable<User>>> GetUsers();
        }
    }
}
