﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWD391.Models;
using static SWD391.Models.Calculation;
using static SWD391.Models.Transaction;
namespace SWD391.Data
{
    public class SWD391Context : DbContext
    {
        public SWD391Context(DbContextOptions<SWD391Context> options)
            : base(options)
        {

        }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Operand> Operands { get; set; }
        public DbSet<Explanation> Explanations { get; set; }
        public DbSet<BaseFormula> BaseFormulas { get; set; }
        public DbSet<GroupValue> GroupValues { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Rate> Rates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //----------------------------------------------
            modelBuilder.Entity<Bank>().Property<int>(nameof(Bank.Id)).UseIdentityColumn();
            modelBuilder.Entity<BaseFormula>().Property<int>(nameof(BaseFormula.ID)).UseIdentityColumn();
            modelBuilder.Entity<Operand>().Property<int>(nameof(Operand.ID)).UseIdentityColumn();
            modelBuilder.Entity<GroupValue>().Property<int>(nameof(GroupValue.ID)).UseIdentityColumn();
            modelBuilder.Entity<Account>().Property<int>(nameof(Account.ID)).UseIdentityColumn();
            modelBuilder.Entity<Rate>().Property<int>(nameof(Rate.ID)).UseIdentityColumn();
            //------------------------------------------
            //modelBuilder.Entity<Account>().Has(x => new { x.UID, x.BankID });
            modelBuilder.Entity<Account>().HasOne(x => x.User).WithMany(c => c.Accounts).HasForeignKey(n => n.UID);
            modelBuilder.Entity<BaseFormula>().HasMany(x => x.Operands);
            modelBuilder.Entity<Operand>().HasMany(x => x.Childs).WithOne(e => e.Parent)
                .HasForeignKey(e => e.OperandID);
            modelBuilder.Entity<Operand>().HasOne(x => x.BaseFormula);
            modelBuilder.Entity<Bank>().HasMany(x => x.Rates);
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
