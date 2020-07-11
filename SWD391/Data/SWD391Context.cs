using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWD391.Models;
using static SWD391.Models.Transaction;

namespace SWD391.Data
{
    public class SWD391Context : DbContext
    {
        public SWD391Context (DbContextOptions<SWD391Context> options)
            : base(options)
        {
        }

        public DbSet<Bank> Bank { get; set; }

        public DbSet<User> User { get; set; }
        public DbSet<Calculation.Operand> Operands { get; set; }
        public DbSet<Calculation.Formula> Formulas { get; set; }
        public DbSet<Calculation.Explanation> Explanations { get; set; }
        public DbSet<Calculation.BaseFormula> BaseFormulas { get; set; }

        public DbSet<UserSaving> UserSavings { get; set; }
    }
}
