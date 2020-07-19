using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD391.Models
{
    public class EnumUtils
    {
        public enum OperandTypeValue : int
        {
            INPUT = 1,
            EXPRESSION = 2,
            GROUP_VALUE = 3,
            STATIC = 4
        }

        public class DBConnection
        {
            public string ConStr { get; set; }
            private DBConnection(string conn) {
                ConStr = conn;
            }
            public static DBConnection NguyenNCLocal { get { return new DBConnection(@"Server=DESKTOP-BV4OVDM\SQLEXPRESS;Database=TMP_DB;Trusted_Connection=True;Connection Timeout=300;"); } }
            public static DBConnection Deploy { get { return new DBConnection("Server=tcp:financial-service-db.database.windows.net,1433;Initial Catalog=swd391_db;Persist Security Info=False;User ID=swd391;Password=Swd123456;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=300;"); } }
        }

    }
}
