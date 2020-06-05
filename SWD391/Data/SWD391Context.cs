using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWD391.Models;

namespace SWD391.Data
{
    public class SWD391Context : DbContext
    {
        public SWD391Context (DbContextOptions<SWD391Context> options)
            : base(options)
        {
        }

        public DbSet<Bank> Bank { get; set; }

        public DbSet<SWD391.Models.User> User { get; set; }
    }
}
