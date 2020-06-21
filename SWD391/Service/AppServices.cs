using Microsoft.AspNetCore.Mvc;
using SWD391.Data;
using SWD391.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SWD391.Service.IAppServices;

namespace SWD391.Service
{
    public class AppServices
    {
        public class BankService : IBankService
        {
            private readonly SWD391Context _context;

            public async Task<ActionResult<IEnumerable<Bank>>> GetBanks()
            {
                //string nextUrl = "", previousUrl = "";
                //var result = new PagedCollectionResponse<Bank>();
                //string filterBy  = filter.Term;
                //string[] value = filterBy.Split(";");
                //var listResult = _context.Bank.Where(n => n.Name.Contains(filter.Term))
                //    .Skip((filter.Page - 1) * filter.Limit).Take(filter.Limit);
                //if (listResult != null)
                //{
                //    result.Items = listResult;
                //}
                //throw new NotImplementedException();
                return _context.Bank.ToList();
            }
        }
        public class UserService : IUserService
        {
            public Task<ActionResult<IEnumerable<User>>> GetUsers()
            {
                throw new NotImplementedException();
            }
        }
    }
}
