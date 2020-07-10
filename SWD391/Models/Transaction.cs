using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD391.Models
{
    public class Transaction
    {
        public class UserSaving
        {
            public int UID { get; set; }
            public int BankID { get; set; }
            public DateTime FromDate { get; set; }
        }
    }
}
