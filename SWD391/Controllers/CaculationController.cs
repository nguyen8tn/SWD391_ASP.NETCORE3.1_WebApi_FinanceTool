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
using static SWD391.Models.EnumUtils;
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
            IEnumerable<Operand> list = await _context.Operands.Where(x => x.BaseFormulaID == bfID).Where(x => x.Type == (int)OperandTypeValue.INPUT).ToListAsync();
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
            //get user input
            List<Operand> request = operands;
            BaseFormula baseFormula = await _context.BaseFormulas.Where(x => x.ID == id).FirstOrDefaultAsync();
            List<Operand> operandT = await _context.Operands
                .Where(x => x.BaseFormulaID == id
                && x.Type != (int)OperandTypeValue.INPUT
                && x.OperandID == x.ID).ToListAsync();
            //parse inputed value
            VariableSet vSetBaseFormula = new VariableSet();
            var ep = new ExpressionParser();
            foreach (var item in request)
            {
                //keyValuesOperand.Add(item.Name, item.Value);
                vSetBaseFormula.RegisterVariable(OperandType.Double, item.Name, item.Value);
            }

            //-----------------------------
            foreach (var item in operandT)
            {
                if (item.Childs != null)
                {
                    List<Operand> childs = item.Childs.ToList();
                    if (childs.Count > 0)
                    {
                        Console.WriteLine(item.Name + "x " + item.Value);
                        vSetBaseFormula = await GetOperandChildsAsync(item, vSetBaseFormula);
                    }
                    else
                    {
                        var compiledExpression = ep.Parse(item.Expression);
                        var resultStack = compiledExpression.Evaluate(vSetBaseFormula);
                        var tmp = Convert.ToDouble(resultStack.Pop().GetValue());
                        if (vSetBaseFormula.Where(x => x.VariableName.Equals(item.Name)).FirstOrDefault() == null)
                        {
                            vSetBaseFormula.RegisterVariable(OperandType.Double, item.Name, tmp);
                        }
                    }
                }
            }
            var ce = ep.Parse(baseFormula.Expression);
            foreach (var item in vSetBaseFormula)
            {
                Console.WriteLine(item.VariableName + " " + item.Value);
            }
            var resul = ce.Evaluate(vSetBaseFormula);
            double value = Convert.ToDouble(resul.Pop().GetValue());
            Console.WriteLine(value);
            Console.WriteLine("asdsad");
            return 0;
        }

        [HttpPost]
        [Route("add-base-formula")]
        public async Task<ActionResult<BaseFormula>> AddBaseFormula([FromBody] FormulaRequest request)
        {
            try
            {
                request.Operands.Where(x => x.Type == (int)OperandTypeValue.INPUT).ToList();

                bool check = await _context.SaveChangesAsync() > 0;
                if (check)
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {

            }
            return BadRequest();
        }

        private async Task<VariableSet> GetOperandChildsAsync(Operand parent, VariableSet variableSet)
        {
            KeyValuePair<string, double> result;
            await _context.Entry(parent).Collection(x => x.Childs).Query().LoadAsync();
            var childs = parent.Childs;
            if (childs.Count > 1)
            {
                //caculate child
                for (int i = 1; i < childs.Count; i++)
                {

                    Console.WriteLine("i: " + i);
                    await GetOperandChildsAsync(childs.ElementAt(i), variableSet);
                }
                //caculate parent
                result = await CaculateOperandAsync(parent, variableSet);
                Console.WriteLine(result.Key + " " + result.Value);
                if (variableSet.Where(x => x.VariableName.Equals(result.Key)).FirstOrDefault() == null)
                {
                    variableSet.RegisterVariable(OperandType.Double, result.Key, result.Value);
                }
                return variableSet;
            }
            else
            {
                result = await CaculateOperandAsync(parent, variableSet);
                Console.WriteLine(result.Key + " " + result.Value);
                if (variableSet.Where(x => x.VariableName.Equals(result.Key)).FirstOrDefault() == null)
                {
                    variableSet.RegisterVariable(OperandType.Double, result.Key, result.Value);
                }
            }
            return variableSet;
        }

        private async Task<KeyValuePair<string, double>> CaculateOperandAsync(Operand operand, VariableSet variableSet)
        {
            Console.WriteLine("Break at: " + operand.Name + operand.Expression);
            foreach (var item in variableSet)
            {
                Console.WriteLine(item.VariableName + item.Value);
            }
            KeyValuePair<string, double> result = new KeyValuePair<string, double>();
            if (operand.Type == (int)OperandTypeValue.EXPRESSION || operand.Type == (int)OperandTypeValue.GROUP_VALUE)
            {
                var ep = new ExpressionParser();
                var compiledExpression = ep.Parse(operand.Expression);
                var resultStack = compiledExpression.Evaluate(variableSet);
                double value = Convert.ToDouble(resultStack.Pop().GetValue());
                if (operand.Type == (int)OperandTypeValue.EXPRESSION)
                {
                    operand.Value = value;
                    result = new KeyValuePair<string, double>(operand.Name, operand.Value);
                }
                else if (operand.Type == (int)OperandTypeValue.GROUP_VALUE)
                {
                    await _context.Entry(operand).Collection(x => x.GroupValues).LoadAsync();
                    var gropValues = operand.GroupValues.Where(x => x.Max > value).OrderBy(c => c.Max).ToList();
                    if (gropValues.Count() != 0)
                    {
                        compiledExpression = ep.Parse(gropValues[0].Value);
                        resultStack = compiledExpression.Evaluate(variableSet);
                        value = Convert.ToDouble(resultStack.Pop().GetValue());
                        Console.WriteLine("Add: " + operand.Name);
                        result = new KeyValuePair<string, double>(operand.Name, value);
                    }
                }
            }
            else if (operand.Type == (int)OperandTypeValue.STATIC)
            {

                result = new KeyValuePair<string, double>(operand.Name, operand.Value);
            }
            return result;
        }
    }
}