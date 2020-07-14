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
            Task<IEnumerable<SavingAccount>> GetSavingAccounts();
            Task<bool> AddSavingAccount(SavingAccount obj);
            Task<bool> UpdateSavingAccount(SavingAccount obj);
            Task<bool> DeleteSavingAccount(SavingAccount obj);
            Task<SavingAccount> GetSavingAccountByID(string id);

        }

        public interface ICalculationService
        {
            Task<string> PushOperant(int formulaID);

        }
    }
}
