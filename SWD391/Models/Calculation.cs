using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD391.Models
{
    public class Calculation
    {
        public class Operand
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public double Value { get; set; }
            public bool Static { get; set; }
            public string Desc { get; set; }
            public int FormulaID { get; set; }
        }

        public class Formula
        {
            public int ID { get; set; }
            public string Fromula { get; set; }
            public int BaseID { get; set; }
            public int Sequence { get; set; }

        }
        public class BaseFormula
        {
            public int ID { get; set; }
            public string Fromula { get; set; }

        }

        public class Explanation
        {
            public int ID { get; set; }
            public int BaseID { get; set; }

        }
    }
}
