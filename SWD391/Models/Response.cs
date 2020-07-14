using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD391.Models
{
    public abstract class Response
    {
        public abstract IDictionary<string, object> value { get; set; }
    }

    public class TransactionControllerResponse : Response
    {
        public override IDictionary<string, object> value { get; set; }
        public int Test { get; set; }
    }
}
