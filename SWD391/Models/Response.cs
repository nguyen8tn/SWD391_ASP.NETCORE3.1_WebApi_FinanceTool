using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SWD391.Models.Calculation;

namespace SWD391.Models
{
    public abstract class Response
    {
        public abstract IDictionary<string, object> value { get; set; }
    }

    public class CalculatonResponse
    {
        public ICollection<Operand> Operands { get; set; }
        public double Result { get; set; }
    }

}
