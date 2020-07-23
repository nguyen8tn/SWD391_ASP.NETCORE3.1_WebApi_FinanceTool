using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD391.Data;
using SWD391.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SWD391.Models.Calculation;
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

            public Task<Bank> GeBankByIDAsync(int id)
            {
                throw new NotImplementedException();
            }

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
                return _context.Banks.ToList();
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

            public async Task<bool> AddSavingAccount(Transaction.SavingAccount obj)
            {
                await _context.SavingAccounts.AddAsync(obj);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<IEnumerable<Transaction.SavingAccount>> GetSavingAccounts()
            {
                var list = await _context.SavingAccounts.ToListAsync();
                return list;
            }

            public async Task<Transaction.SavingAccount> GetSavingAccountByID(string id)
            {
                var user = await _context.SavingAccounts.FirstOrDefaultAsync(i => i.ID.Equals(id));
                return user;
            }

            public async Task<bool> UpdateSavingAccount(Transaction.SavingAccount obj)
            {
                _context.SavingAccounts.Update(obj);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<bool> DeleteSavingAccount(Transaction.SavingAccount obj)
            {
                _context.SavingAccounts.Remove(obj);
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

            public async Task<bool> AddOperandAsync(Operand operand)
            {
                _context.Operands.Add(operand);
                bool check = await _context.SaveChangesAsync() > 0;
                return check;
            }

            public async Task<bool> UpdateOperandAsync(Operand operand)
            {
                _context.Operands.Update(operand);
                return await _context.SaveChangesAsync() > 0;
            }

            public async Task<Operand> FindOperandAsync(int id)
            {
                var t = await _context.Operands.Where(x => x.ID == id).FirstOrDefaultAsync();
                return t;
            }

            public async Task<bool> DeleteOperandAsync(Operand operand)
            {
                 _context.Operands.Remove(operand);
                return await _context.SaveChangesAsync() > 0;
            }
        }
    }
}
