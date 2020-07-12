using Microsoft.AspNetCore.Mvc;
using SWD391.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SWD391.Models.Transaction;

namespace SWD391.Service
{
    public class IAppServices
    {
        public interface IBankService
        {
            Task<IEnumerable<Bank>> GetBanks();
        }
        public interface IUserService
        {
            Task<ActionResult<IEnumerable<User>>> GetUsers();
        }

        public interface ITransactionService
        {
            Task<IEnumerable<UserSaving>> GetSavingAccounts(string uid);
            Task<bool> AddSavingAccount(UserSaving obj);
            Task<bool> UpdateSavingAccount(UserSaving obj);
            Task<bool> DeleteSavingAccount(UserSaving obj);
            Task<UserSaving> GetUserByID(string id);

        }
    }
}
