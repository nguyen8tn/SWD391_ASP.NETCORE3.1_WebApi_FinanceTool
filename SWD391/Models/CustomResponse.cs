using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SWD391.Models.Calculation;

namespace SWD391.Models
{
    public class CustomResponse
    {
        public class UserResponse {
            public string JwtString { get; set; }
        }

        public class CaculattionResponse
        {
            public Explanation Explanation { get; set; }
            public double Result { get; set; }
        }
    }
}
