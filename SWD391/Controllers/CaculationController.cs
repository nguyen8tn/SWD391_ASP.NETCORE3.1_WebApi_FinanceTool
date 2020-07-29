using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Variables;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD391.Data;
using SWD391.Models;
using static SWD391.Models.Calculation;
using static SWD391.Models.EnumUtils;
using static SWD391.Service.IAppServices;
using Operand = SWD391.Models.Calculation.Operand;

namespace SWD391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaculationController : ControllerBase
    {
        private readonly SWD391Context _context;
        private ICalculationService _calculationService;
        public CaculationController(SWD391Context context, ICalculationService calculationService)
        {
            _context = context;
            _calculationService = calculationService;
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
        [Route("get-all-base-formula-by-admin")]
        [EnableQuery(PageSize = 10)]
        public async Task<ActionResult<IEnumerable<BaseFormula>>> GetAllBaseFormula()
        {
            try
            {
                IEnumerable<BaseFormula> list = await _calculationService.GetAllBaseFormulaByAdminAsync();
                return Ok(list);
            }
            catch (Exception e)
            {
                var response = new { Message = e.Message };
                return StatusCode(500, response);
            }
        }

        [HttpGet]
        [Route("get-all-base-formula-by-user")]
        public async Task<ActionResult<IEnumerable<BaseFormula>>> GetAllBaseFormulaUser()
        {
            try
            {
                IEnumerable<BaseFormula> list = await _calculationService.GetAllBaseFormulaByUserAsync();
                return Ok(list);
            }
            catch (Exception e)
            {
                var response = new { Message = e.Message };
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [Route("create-operand")]
        public async Task<ActionResult<Operand>> AddOperand([FromBody] Operand operand)
        {
            try
            {
                var t = await _calculationService.AddOperandAsync(operand);
                if (t)
                {
                    return CreatedAtAction("created operand", operand);
                }
                else
                {
                    return Conflict();
                }
            }
            catch (Exception e)
            {
                var response = new { Message = e.Message };
                return StatusCode(500, response);
            }
        }
        [HttpPut]
        [Route("update-operand")]
        public async Task<ActionResult<Operand>> UpdateOperand([FromBody] Operand operand)
        {
            try
            {
                if (_calculationService.GetOperandAsync(operand.ID) != null)
                {
                    if (await _calculationService.AddOperandAsync(operand))
                    {
                        return NoContent();
                    } else
                    {
                        return Conflict();
                    }
                } else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                var response = new { Message = e.Message };
                return StatusCode(500, response);
            }
        }

        [HttpDelete]
        [Route("delete-operand")]
        public async Task<ActionResult<Operand>> DeteleteOperand([FromBody] Operand operand)
        {
            try
            {
                var t = await _calculationService.GetOperandAsync(operand.ID);
                if (t != null)
                {
                    if (await _calculationService.DeleteOperandAsync(t))
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Conflict();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                var response = new { Message = e.Message };
                return StatusCode(500, response);
            }
        }
        [HttpDelete]
        [Route("delete-group-value")]
        public async Task<ActionResult<Operand>> DeteleteGroupValue([FromBody] GroupValue groupValue)
        {
            try
            {
                var t = await _calculationService.GetGroupValueAsync(groupValue.ID);
                return null;
            }
            catch (Exception e)
            {
                var response = new { Message = e.Message };
                return StatusCode(500, response);
            }
        }
        [HttpGet]
        [Route("push-user-input-operand-by-base-formula/{bfID}")]
        public async Task<ActionResult<IEnumerable<Operand>>> PushOperand(int bfID)
        {
            try
            {
                IEnumerable<Operand> list = await _context.Operands.Where(x => x.BaseFormulaID == bfID).Where(x => x.Type == (int)OperandTypeValue.INPUT).ToListAsync();
                if (list == null)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                var response = new { Message = e.Message };
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [Route("calculate-formula/{id}")]
        public async Task<ActionResult<CalculatonResponse>> Caculate([FromBody] List<Operand> operands, int id)
        {
            //get user input
            try
            {
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
                if (operandT != null)
                {
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
                }  
                
                var ce = ep.Parse(baseFormula.Expression);
                foreach (var item in vSetBaseFormula)
                {
                    Console.WriteLine(item.VariableName + " " + item.Value);
                }
                var resul = ce.Evaluate(vSetBaseFormula);
                double value = Convert.ToDouble(resul.Pop().GetValue());
                operandT.AddRange(request);
                List<Operand> resultOp = operandT.Where(x => x.OperandID == x.ID).ToList();
                CalculatonResponse calculatonResponse = new CalculatonResponse();
                calculatonResponse.Operands = resultOp;
                calculatonResponse.Result = value;
                return Ok(calculatonResponse);
            }
            catch (Exception e)
            {
                var response = new { Message = e.Message };
                //LogException(e);
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [Route("add-formula")]
        public ActionResult<Dictionary<int, double>> AddFormula([FromBody] FormulaRequest request)
        {
            try
            {
                var inputList = request.Operands.Where(x => x.Type == (int)OperandTypeValue.INPUT).ToList();
                List<Operand> list = request.Operands.Where(x => x.Type != (int)OperandTypeValue.INPUT && x.OperandID == x.ID)
                    .OrderBy(x => x.Sequence).ToList();
                var tmp = request.GroupValues.ToList();
                foreach (var item in tmp)
                {
                    var t = list.Where(x => x.Type == (int)OperandTypeValue.GROUP_VALUE && x.OperandID == item.OperandID).FirstOrDefault();
                    if (t != null)
                    {
                        list.Remove(t);
                        t.GroupValues.Add(item);
                        list.Add(t);
                    }
                }
                //parse inputed value
                VariableSet vSetBaseFormula = new VariableSet();
                var ep = new ExpressionParser();
                foreach (var item in inputList)
                {
                    //keyValuesOperand.Add(item.Name, item.Value);
                    vSetBaseFormula.RegisterVariable(OperandType.Double, item.Name, item.Value);
                }

                //-----------------------------
                Dictionary<int, double> listResult = new Dictionary<int, double>();
                Parallel.ForEach(request.testNumber, async (item) =>
                {
                    foreach (var operand in list)
                    {
                        if (operand.Childs != null)
                        {
                            List<Operand> childs = operand.Childs.ToList();
                            if (childs.Count > 0)
                            {
                                Console.WriteLine(operand.Name + "x " + operand.Value);
                                vSetBaseFormula = await GetOperandChildsAsync(operand, vSetBaseFormula);
                            }
                            else
                            {
                                var compiledExpression = ep.Parse(operand.Expression);
                                var resultStack = compiledExpression.Evaluate(vSetBaseFormula);
                                var tmp = Convert.ToDouble(resultStack.Pop().GetValue());
                                if (vSetBaseFormula.Where(x => x.VariableName.Equals(operand.Name)).FirstOrDefault() == null)
                                {
                                    vSetBaseFormula.RegisterVariable(OperandType.Double, operand.Name, tmp);
                                }
                            }
                        }
                    }
                    var ce = ep.Parse(request.baseFormula.Expression);
                    foreach (var param in vSetBaseFormula)
                    {
                        Console.WriteLine(param.VariableName + " " + param.Value);
                    }
                    var resul = ce.Evaluate(vSetBaseFormula);
                    double value = Convert.ToDouble(resul.Pop().GetValue());
                    listResult.Add(item, value);
                }
            );
                _context.BaseFormulas.Add(request.baseFormula);
                _context.Operands.AddRange(request.Operands);
                var check = _context.SaveChanges() > 0;
                if (check)
                {
                    return Ok(listResult);
                }
                else
                {
                    return BadRequest("Cannot save to db");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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