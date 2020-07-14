using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWD391.Models;
using static SWD391.Models.Calculation;
using static SWD391.Models.Transaction;
using Microsoft.Extensions.Logging;
namespace SWD391.Data
{
    public class SWD391Context : DbContext
    {
        public SWD391Context(DbContextOptions<SWD391Context> options)
            : base(options)
        {

        }

        public DbSet<Bank> Bank { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Operand> Operands { get; set; }
        public DbSet<SubFormula> Formulas { get; set; }
        public DbSet<Explanation> Explanations { get; set; }
        public DbSet<BaseFormula> BaseFormulas { get; set; }
        public DbSet<SavingAccount> SavingAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<SavingAccount>().Has(x => new { x.UID, x.BankID });
            modelBuilder.Entity<SavingAccount>().HasOne(x => x.User).WithMany(c => c.SavingAccounts).HasForeignKey(n => n.UID);
            //modelBuilder.Entity<LoanAccount>().HasOne(x => x.User).WithMany(c => c.Accounts).HasForeignKey(n => n.UID);

            modelBuilder.Entity<User>().HasMany(x => x.LoanAccounts);
            modelBuilder.Entity<BaseFormula>().HasMany(x => x.Operands);
            modelBuilder.Entity<Operand>().HasMany(x => x.SubFormulas);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseLazyLoadingProxies(true);
        }

        public static readonly LoggerFactory _myLoggerFactory =
            new LoggerFactory(new[] {
                new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
            });

    }
}
