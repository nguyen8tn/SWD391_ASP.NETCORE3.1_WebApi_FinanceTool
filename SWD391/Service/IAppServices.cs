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
            Task<bool> AddAccount(Account obj);
            Task<bool> UpdateAccount(Account obj);
            Task<bool> DeleteAccount(Account obj);
            Task<Account> GetAccountByID(int id);
            Task<IEnumerable<Account>> GetAccountsByUIDAndType(string uid, int type);
        }

        public interface ICalculationService
        {
            Task<bool> AddOperandsAsync(List<Operand> operand);
            Task<bool> UpdateOperandsAsync(List<Operand> operand);
            Task<bool> DeleteBaseFormulaAsync(BaseFormula baseFormula);
            Task<bool> DeleteOperandsAsync(List<Operand> operand);
            Task<bool> AddGroupValuesAsync(List<GroupValue> groupValue);
            Task<bool> UpdateGroupValuesAsync(List<GroupValue> groupValue);
            Task<bool> DeleteGroupValuesAsync(List<GroupValue> groupValue);
            Task<Operand> GetOperandAsync(int id);
            Task<GroupValue> GetGroupValueAsync(int id);
            Task<BaseFormula> GetBaseFormulaAsync(int id);
            Task<IEnumerable<BaseFormula>> GetAllBaseFormulaByUserAsync();
            Task<IEnumerable<BaseFormula>> GetAllBaseFormulaByAdminAsync();
            Task<bool> AddBaseFormulaAsync(BaseFormula baseFormula);
        }
    }
}
