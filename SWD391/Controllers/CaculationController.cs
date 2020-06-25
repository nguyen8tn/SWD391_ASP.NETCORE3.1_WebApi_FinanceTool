using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Variables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD391.Data;

namespace SWD391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaculationController : ControllerBase
    {
        private readonly SWD391Context _context;
        public CaculationController(SWD391Context context)
        {
            _context = context;
        }
        [Route("parser")]
        [HttpGet]
        public string GetTax()
        {
            VariableSet vSet = new VariableSet();
            vSet.RegisterVariable(OperandType.Double, "cabbages", 6);
            vSet.RegisterVariable(OperandType.Long, "onions", 2);
            vSet.RegisterVariable(OperandType.Long, "bananas", 5);
            //----------------------------------------------------------
            var ep = new ExpressionParser();
            //var compiledExpression = ep.Parse("(6+2)*7");
            var compiledExpression = ep.Parse("(cabbages+onions)*bananas");
            var resultStack = compiledExpression.Evaluate(vSet);
            return compiledExpression.ToString() + " ---------------- " + resultStack.ToString() + " -------------- ";
        }

        //private bool ValidateExpressionString(string expression)
        //{

        //}
    }
}