using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD391.Data;
using SWD391.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SWD391.Models.Calculation;
using static SWD391.Models.EnumUtils;
using static SWD391.Models.Transaction;
using static SWD391.Service.IAppServices;

namespace SWD391.Service
{
    public class AppServices
    {
        public class BankService : IBankService
        {
            private readonly SWD391Context _context;
            public BankService(SWD391Context context)
            {
                _context = context;
            }
            public async Task<bool> AddBankAsync(Bank bank)
            {
                await _context.Banks.AddAsync(bank);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<bool> DeleteBankAsync(Bank bank)
            {
                _context.Banks.Remove(bank);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<Bank> GeBankByIDAsync(int id)
            {
                return await _context.Banks.Where(x => x.Id == id).FirstOrDefaultAsync();
            }

            public async Task<IEnumerable<Bank>> GetBanks()
            {

                return  await _context.Banks.ToListAsync();
            }

            public async Task<bool> UpdateBankAsync(Bank bank)
            {
                _context.Banks.Update(bank);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }
        }

        public class UserService : IUserService
        {
            private readonly SWD391Context _context;
            public UserService(SWD391Context context)
            {
                _context = context;
            }
            public async Task<User> GetUserByIDAsync(string id)
            {
                var user = await _context.Users.FirstOrDefaultAsync(i => i.Uid.Equals(id));
                return user;
            }

            public Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
            {
                throw new NotImplementedException();
            }

            public async Task<bool> UpdateUserAsync(User user)
            {
                _context.Users.Update(user);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<bool> AddUserAsync(User user)
            {
                await _context.Users.AddAsync(user);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<bool> DeleteUserAsync(User user)
            {
                _context.Users.Remove(user);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }
        }

        public class TransactionService : ITransactionService
        {
            private readonly SWD391Context _context;
            public TransactionService(SWD391Context context)
            {
                _context = context;
            }

            public async Task<bool> AddAccount(Account obj)
            {
                await _context.Accounts.AddAsync(obj);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<IEnumerable<Account>> GetAccountsByUIDAndType(string uid, int type)
            {
                var list = await _context.Accounts.Where(x => x.UID.Equals(uid) && x.Type == type).ToListAsync();
                return list;
            }

            public async Task<Account> GetAccountByID(int id)
            {
                var user = await _context.Accounts.AsNoTracking().Where(i => i.ID == id).FirstOrDefaultAsync();
                return user;
            }

            public async Task<bool> UpdateAccount(Account obj)
            {
                _context.Accounts.Update(obj);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<bool> DeleteAccount(Transaction.Account obj)
            {
                _context.Accounts.Remove(obj);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }
        }

        public class CalculationService : ICalculationService
        {
            private readonly SWD391Context _context;
            public CalculationService(SWD391Context context)
            {
                _context = context;
            }
            public async Task<bool> AddBaseFormulaAsync(BaseFormula baseFormula)
            {
                _context.BaseFormulas.Add(baseFormula);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<IEnumerable<BaseFormula>> GetAllBaseFormulaByAdminAsync()
            {
                    return await _context.BaseFormulas.Include(x => x.Operands).ThenInclude(x => x.GroupValues).ToListAsync();
            }

            public async Task<IEnumerable<BaseFormula>> GetAllBaseFormulaByUserAsync()
            {
                return await _context.BaseFormulas.ToListAsync();
            }

            public async Task<bool> AddOperandsAsync(List<Operand> operand)
            {
                await _context.Operands.AddRangeAsync(operand);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<bool> UpdateOperandsAsync(List<Operand> operand)
            {
                _context.Operands.UpdateRange(operand);
                return await _context.SaveChangesAsync() > 0;
            }

            public async Task<Operand> GetOperandAsync(int id)
            {
                var t = await _context.Operands.Include(x => x.GroupValues).Where(x => x.ID == id).FirstOrDefaultAsync();
                return t;
            }

            public async Task<bool> DeleteOperandsAsync(List<Operand> operand)
            {
                 _context.Operands.RemoveRange(operand);
                return await _context.SaveChangesAsync() > 0;
            }

            public async Task<bool> DeleteBaseFormulaAsync(BaseFormula baseFormula)
            {
                _context.BaseFormulas.Remove(baseFormula);
                return await _context.SaveChangesAsync() > 0;
            }

            public async Task<bool> DeleteGroupValuesAsync(List<GroupValue> groupValue)
            {
                _context.GroupValues.RemoveRange(groupValue);
                return await _context.SaveChangesAsync() > 0;
            }

            public async Task<GroupValue> GetGroupValueAsync(int id)
            {
                return await _context.GroupValues.FindAsync(id);
            }

            public async Task<BaseFormula> GetBaseFormulaAsync(int id)
            {
                return await _context.BaseFormulas.Include(x => x.Operands)
                    .ThenInclude(x => x.GroupValues).Where(x => x.ID == id).SingleOrDefaultAsync();
            }

            public async Task<bool> AddGroupValuesAsync(List<GroupValue> groupValue)
            {
                await _context.GroupValues.AddRangeAsync(groupValue);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<bool> UpdateGroupValuesAsync(List<GroupValue> groupValue)
            {
                _context.GroupValues.UpdateRange(groupValue);
                return await _context.SaveChangesAsync() > 0;
            }
        }
    }
}
