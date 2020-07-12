using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            public async Task<IEnumerable<Bank>> GetBanks()
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

        public class TransactionService : ITransactionService
        {
            private readonly SWD391Context _context;
            public TransactionService(SWD391Context context)
            {
                _context = context;
            }

            public async Task<bool> AddSavingAccount(Transaction.UserSaving obj)
            {
                await _context.UserSavings.AddAsync(obj);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<IEnumerable<Transaction.UserSaving>> GetSavingAccounts(string uid)
            {
                var list = await _context.UserSavings.Where(x => x.UID.Equals(uid)).ToListAsync();
                return list;
            }

            public async Task<Transaction.UserSaving> GetUserByID(string id)
            {
                var user = await _context.UserSavings.FirstOrDefaultAsync(i => i.ID.Equals(id));
                return user;
            }

            public async Task<bool> UpdateSavingAccount(Transaction.UserSaving obj)
            {
                _context.UserSavings.Update(obj);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<bool> DeleteSavingAccount(Transaction.UserSaving obj)
            {
                _context.UserSavings.Remove(obj);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }
        }
    }
}
