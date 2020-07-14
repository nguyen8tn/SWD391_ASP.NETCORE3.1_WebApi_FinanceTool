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
using Microsoft.EntityFrameworkCore;
using SWD391.Data;
using SWD391.Models;
using static SWD391.Models.Calculation;
using Operand = SWD391.Models.Calculation.Operand;

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

        //[Route("parser")]
        //[HttpGet]
        //public string GetTax([FromQuery] SubFormula formula)
        //{
        //    VariableSet vSet = new VariableSet();
        //    vSet.RegisterVariable(OperandType.Double, "X", 6);
        //    vSet.RegisterVariable(OperandType.Long, "YX", 2);
        //    vSet.RegisterVariable(OperandType.Long, "bananas", 5);
        //    //----------------------------------------------------------
        //    var ep = new ExpressionParser();
        //    //var compiledExpression = ep.Parse("(6+2)*7");
        //    var compiledExpression = ep.Parse(formula.Formular);
        //    //var resultStack = compiledExpression.Evaluate(vSet);
        //    return compiledExpression.ToString() + " ---------------- ";
        //}

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Calculation.Operand>>> PushOperand()
        //{
        //    List<Calculation.Operand> operand = await _context.Operands.Where(x => x.BaseFormulaID == 1)
        //                                                        .Where(x => x.Static == false).ToListAsync();
        //    IDictionary<string, IEnumerable<Calculation.Operand>> respone = new Dictionary<string, IEnumerable<Calculation.Operand>>();
        //    respone.Add("values", operand);
        //    return Ok(respone);
        //}

        //[HttpGet]
        //[Route("caculate")]
        //public async Task<ActionResult<IDictionary<string, IEnumerable<Calculation.Operand>>>> Caculate([FromBody] List<Calculation.Operand> userOperand)
        //{
        //    int id = 1;
        //    List<Calculation.Operand> operand = await _context.Operands.Where(x => x.BaseFormulaID == id).Where(x => x.Static == false).ToListAsync();
        //    List<Calculation.SubFormula> formulas = await _context.Formulas.Where(x => x.Formular == id).ToListAsync();
        //    Calculation.BaseFormula baseFormula = await _context.BaseFormulas.Where(x => x.ID == id).FirstOrDefaultAsync();
        //    //-----------------------------------
        //    Caculate(operand, userOperand, formulas, baseFormula);
        //    for (int i = 0; i < formulas.Count; i++)
        //    {
        //        string praseFromula = formulas.ElementAt(i).Fromula;
        //    }
        //    IDictionary<string, IEnumerable<Calculation.Operand>> respone = new Dictionary<string, IEnumerable<Calculation.Operand>>();
        //    respone.Add("values", operand);
        //    return Ok(respone);
        //}

        //private string  Caculate(List<Calculation.Operand> operands, List<Calculation.Operand> userOperands, List<Calculation.SubFormula> formulas, Calculation.BaseFormula mainFormula)
        //{

        //    VariableSet vSet = new VariableSet();
        //    //register variable
        //    for (int i = 0; i < operands.Count; i++)
        //    {              
        //        //register variable for non-static operand
        //        if (operands.ElementAt(i).Static == false)
        //        {
        //            vSet.RegisterVariable(OperandType.Double, operands.ElementAt(i).Name, userOperands.Find(x => x.Name == operands.ElementAt(i).Name));
        //        } else
        //        {
        //            //register variable for static operand
        //            vSet.RegisterVariable(OperandType.Double, operands.ElementAt(i).Name, operands.ElementAt(i).Value);
        //        }
        //    }
        //    //---------------
        //    //caculate sub formula
        //    var ep = new ExpressionParser();
        //    List<double> resultList = new List<double>();
        //    TokenList compiledExpression;
        //    OperandStack result;
        //    for (int i = 0; i < formulas.Count; i++)
        //    {
        //        compiledExpression = ep.Parse(formulas.ElementAt(i).Fromula);
        //        result = compiledExpression.Evaluate(vSet);
        //        double value = (double) result.Pop().GetValue(); 
        //        resultList.Add(value);
        //    }
        //    // caculate main formula
        //    compiledExpression = ep.Parse(mainFormula.Fromula);
        //    Char[] operand = new Char[] { '+', '-', '*', ':', '^' };
        //    string[] x = mainFormula.Fromula.Split(operand);
        //    for (int i = 0; i < resultList.Count; i++)
        //    {
        //        vSet.RegisterVariable(OperandType.Double, x[i], resultList.ElementAt(i));
        //    }
        //    result = compiledExpression.Evaluate(vSet);
        //    double valuex = (double)result.Pop().GetValue();
        //    return "";
        //}
        ////private bool ValidateExpressionString(string expression)
        ////{

        ////}
        ////public async Task<IEnumerable<Calculation.Explanation>> CaculateDetails(List<Calculation.Operand> userOperands, int baseFormularID)
        ////{
        ////    IEnumerable<Calculation.Explanation> list = await _context.Explanations.Where(x => x.BaseID == baseFormularID).ToListAsync();
        ////    return list;
        ////}

        [HttpGet]
        [Route("get-all-base-formula")]
        public async Task<ActionResult<IEnumerable<BaseFormula>>> GetAllBaseFormula()
        {
            IEnumerable<BaseFormula> list = await _context.BaseFormulas.ToListAsync();
            return Ok(list);
        }

        [HttpGet]
        [Route("push-user-input-operand-by-base-formula/{bfID}")]
        public async Task<ActionResult<IEnumerable<Operand>>> PushOperand(int bfID)
        {
            IEnumerable<Operand> list = await _context.Operands.Where(x => x.BaseFormulaID == bfID).Where(x => x.Type == (int) OperandTypeValue.INPUT).ToListAsync();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpPost]
        [Route("calculate-formula/{id}")]
        public async Task<ActionResult<int>> Caculate([FromBody] List<Operand> operands, int id)
        {
            List<Operand> request = operands;
            BaseFormula baseFormula = await _context.BaseFormulas.Where(x => x.ID == id).FirstOrDefaultAsync();
            List<Operand> operandT = await _context.Operands.Where(x => x.BaseFormulaID == id)
                .Include(x => x.SubFormulas
                .Where(p => p.OperandID == x.ID))
                .OrderBy(x => x.Sequence).ToListAsync();
            Char[] operatorCus = new Char[] { '+', '-', '*', ':', '^', '(', ')' };
            //-----------------------------------------------------------------------------
            string[] operandArray = baseFormula.Expression.Split(operatorCus);
            Dictionary<string, double> keyValuesOperand = new Dictionary<string, double>();
            //----------------------
            VariableSet vSetBaseFormula = new VariableSet();
            foreach (var item in request)
            {
                keyValuesOperand.Add(item.Name, item.Value);
                vSetBaseFormula.RegisterVariable(OperandType.Double, item.Name, item.Value);
            }
            //----------------------
            
            for (int i = 0; i < operandT.Count; i++)
            {
                var ep = new ExpressionParser();
                if (operandT[i].Type == (int) OperandTypeValue.EXPRESSION)
                {
                    keyValuesOperand.Add(operandT[i].Name, 1);
                    await _context.Entry(operandT[i]).Collection(x => x.SubFormulas).LoadAsync();
                    var compiledExpression = ep.Parse(operandT[i].BaseFormula.Expression);

                }
                if (operandT[i].Type == (int)OperandTypeValue.GROUP_VALUE)
                {
                    //await _context.Entry(operandT[i]).Collection(x => x.).LoadAsync();
                }
            }
            //--------------------------------

            return 0;
        }

        [HttpPost]
        [Route("add-base-formula")]
        public async Task<ActionResult<BaseFormula>> AddBaseFormula([FromBody] BaseFormula baseFormula)
        {
            await _context.BaseFormulas.AddAsync(baseFormula);
            bool check = await _context.SaveChangesAsync() > 0;
            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}