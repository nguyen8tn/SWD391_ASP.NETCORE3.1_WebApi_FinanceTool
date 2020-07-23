using Microsoft.AspNetCore.Mvc;
using SWD391.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SWD391.Models.Calculation;
using static SWD391.Models.Transaction;

namespace SWD391.Service
{
    public class IAppServices
    {
        public interface IBankService
        {
            Task<IEnumerable<Bank>> GetBanks();
            Task<Bank> GeBankByIDAsync(int id);
            Task<bool> UpdateBankAsync(Bank bank);
            Task<bool> DeleteBankAsync(Bank bank);
            Task<bool> AddBankAsync(Bank bank);
        }
        public interface IUserService
        {
            Task<ActionResult<IEnumerable<User>>> GetUsersAsync();
            Task<User> GetUserByIDAsync(string id);
            Task<bool> UpdateUserAsync(User user);
            Task<bool> DeleteUserAsync(User user);
            Task<bool> AddUserAsync(User user);
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
            Task<bool> AddOperandAsync(Operand operand);
            Task<bool> UpdateOperandAsync(Operand operand);
            Task<bool> DeleteOperandAsync(Operand operand);
            Task<Operand> FindOperandAsync(int id);
            Task<IEnumerable<BaseFormula>> GetAllBaseFormulaByUserAsync();
            Task<IEnumerable<BaseFormula>> GetAllBaseFormulaByAdminAsync();
            Task<bool> AddBaseFormulaAsync(BaseFormula baseFormula);
        }
    }
}
