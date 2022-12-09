using AAH_AutoSim.Model.Constants;
using AAH_AutoSim.Model.Models.RainbowModels;
using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Util;
using System;
using System.Text.RegularExpressions;
using static AAH_AutoSim.TestCase.Constants.CompareConstants;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase.Models
{
    /// <summary>
    /// Standard Compare function: Compare {type} {value} {compare} {type} {value} 
    /// {type} could be objectId, objectName, MbData, MemoryName, Value, StringValue
    /// {compare} could be =, >, >= ,<, <=, +-{range} 
    /// MbData {value} is {address} {function code} {name} 
    /// </summary>
    public class CompareCmdObj : BaseCmdObj
    {
        public string CmpTypeA = "";
        public string CmpValueA = "";
        public string CmpOperand = "";
        public string CmpTypeB = "";
        public string CmpValueB = "";
        public string CmpTypeZ = "";
        public string CmpValueZ = "";

        // Add new operators || and &&
        public string AndOrOperand1 = "";  // AndOrOperand1 <= AndOrOperand2
        public string AndOrOperator = "";
        public string AndOrOperand2 = "";
        public bool IsAndOrOperatorCMD = false;
        public bool Is2ObjectIdsCMD = false;

        // the following fields could be null
        public float? CmpRange;    // assign value if +-{range} operand
        public string? ObjTypeIdA; // assign value if objectId type
        public string? ObjTypeIdB; // assign value if objectId type
        public string? ObjTypeIdZ; // assign value if objectId type
        public int? ModbusAddrA;   // assign Modbus address
        public string? ModbusNameA;// assign Modbus Name
        public int? ModbusAddrB;   // assign Modbus address
        public string? ModbusNameB;// assign Modbus Name

        private string CMDFormat = $" [CMD Format: Compare {{type}} {{value}} {{operator}} {{type}} {{value}}]";


        public CompareCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        override protected void SetPattern()
        {
            _pattern = RegexConstants.CmpCmdPattern;
        }

        /// <summary>
        /// Assign values to each Compare Function items.
        /// Note: Keep all Types in lower case to compare.
        /// 
        /// Example:
        /// FLOAT point: Compare objectId 0x2206 0x6EA83B2F > Value 95
        /// BOOL point: Compare objectId 0x2204 0x6EA828C6 = Value 1
        /// WORD point: Compare objectId 0x2302 0x6EA81C74 = StringValue ManClr
        /// WORD point: Compare objectId 0x2302 0x6EA81C74 = Value 1
        /// STR40 point: Compare objectId 0x2304 0x6EA8ACD7 = StringValue %s%N%e %a
        /// Modbus point: Compare MbData 11 SF_EBM_SpdAI +-5 Value 66
        /// Compare objectId 0x230B 0x6EA89E60 = (Value 5 || Value 3)
        /// Compare objectId 0x230B 0x6EA89E60 = (Value > 3 &&  Value < 5)
        /// Compare objectId 0x2204 0x6EA8D135 OR objectId 0x2204 0x6EA862C5 = Value 1
        /// 
		/// </summary>
		/// <param name="cmpCmd">Compare Function command</param>
		override protected void AssignValues()
		{
            Match match = GetMatch();

            if (match.Success)
			{
				CmpTypeA = match.Groups[1].ToString().ToLower();
                CmpValueA = match.Groups[2].ToString().Trim();
                // Special cases with 2 object Ids to the same value:
                // Compare objectId 0x2204 0x6EA8D135 OR objectId 0x2204 0x6EA862C5 = Value 1
                if (CmpValueA.Split(" ").Length > 2 && CmpValueA.ToLower().Contains(" or "))
                {
                    Is2ObjectIdsCMD = true;
                    Regex regex = new Regex(RegexConstants.Cmp2IdToValuePattern, RegexOptions.IgnoreCase);
                    Match match2ndPart = regex.Match(CmpValueA);
                    if (match2ndPart.Success)
                    {
                        CmpValueA = match2ndPart.Groups[1].ToString().Trim();
                        CmpTypeZ = match2ndPart.Groups[2].ToString().Trim().ToLower();
                        CmpValueZ = match2ndPart.Groups[3].ToString().Trim();
                    }
                }
                CmpOperand = match.Groups[3].ToString();
                string rearParts = match.Groups[4].ToString().Trim();
                // Special cases with || and && operators:
                // Compare objectId 0x230B 0x6EA89E60 = (Value 5 || Value 3)
                // Compare objectId 0x230B 0x6EA89E60 = (Value > 3 &&  Value < 5)
                if (rearParts.StartsWith("("))
                {
                    IsAndOrOperatorCMD = true;
                    Regex regex = new Regex(RegexConstants.Cmp2ndCmdPattern, RegexOptions.IgnoreCase);
                    Match matchRearParts = regex.Match(rearParts);
                    
                    if (matchRearParts.Success)
                    {
                        AndOrOperand1 = matchRearParts.Groups[1].ToString();
                        AndOrOperator = matchRearParts.Groups[2].ToString();
                        AndOrOperand2 = matchRearParts.Groups[3].ToString();
                        // to make sure AndOrOperand1 < AndOrOperand2
                        if (AndOrOperator.Equals(CompareOperands.and) && String.Compare(AndOrOperand1, AndOrOperand2) > 0)
                        {
                            string temp = AndOrOperand1;
                            AndOrOperand1 = AndOrOperand2;
                            AndOrOperand2 = temp;
                        }
                    }
                    else
                    {
                        tcObj.AppComments = tcObj.AppComments + $" Invalid format of Compare command format with || &&";
                        tcObj.PassFail = "F";
                    }
                }
                else
                {
                    Regex regex = new Regex(RegexConstants.Cmp1stCmdPattern, RegexOptions.IgnoreCase);
                    Match matchRearParts = regex.Match(rearParts);

                    if (matchRearParts.Success)
                    {
                        CmpTypeB = matchRearParts.Groups[1].ToString().ToLower();
                        CmpValueB = matchRearParts.Groups[2].ToString().Trim();
                    }
                    else
                    {
                        tcObj.AppComments = tcObj.AppComments + $" Invalid format of Compare command{CMDFormat}";
                        tcObj.PassFail = "F";
                    }
                }
            }
            else
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of Compare command{CMDFormat} OR '||' '&&' command formats";
                tcObj.PassFail = "F";
            }
            // Get the range value from +-{range}
            if (!string.IsNullOrEmpty(CmpOperand) && CmpOperand.Contains(CompareOperands.range))
            {
                CmpRange = TCParser.ToFloat(CmpOperand.Substring(2));
            }
            assignObjTypeId(CmpTypeA, CmpValueA, ref ObjTypeIdA);
            assignObjTypeId(CmpTypeB, CmpValueB, ref ObjTypeIdB);
            assignObjTypeId(CmpTypeZ, CmpValueZ, ref ObjTypeIdZ);
            assignMbAddrName(CmpTypeA, CmpValueA, ref ModbusAddrA, ref ModbusNameA);
            assignMbAddrName(CmpTypeB, CmpValueB, ref ModbusAddrB, ref ModbusNameB);
            
        }
        /// <summary>
        /// Assign Modbus Address and Name value
        /// </summary>
        /// <param name="cmpTypeX"></param>
        /// <param name="cmpValueX"></param>
        private void assignMbAddrName(string? cmpTypeX, string? cmpValueX, ref int? ModbusAddrX, ref string? ModbusNameX)
        {
            // Assign Modbus address and Name
            if (!string.IsNullOrEmpty(cmpTypeX) && cmpTypeX == AutoSimFuncObjTypes.modbusData)
            {
                string[] parts = cmpValueX.Split(" ");
                if (parts.Length == 2)
                {
                    ModbusAddrX = TCParser.ToInt(parts[0]);
                    ModbusNameX = parts[1].Trim();
                }
                else
                {
                    tcObj.AppComments = tcObj.AppComments + $" Invalid Modbus Value in Compare command";
                    tcObj.PassFail = "F";
                }
            }
        }
        /// <summary>
        /// Assign Object Type Id
        /// </summary>
        /// <param name="cmpTypeX"></param>
        /// <param name="cmpValueX"></param>
        private void assignObjTypeId(string? cmpTypeX, string? cmpValueX, ref string? ObjTypeIdX)
        {
            string objId = "";
            // Assign Object Type Id by right side value
            if (!string.IsNullOrEmpty(cmpTypeX) && cmpTypeX == AutoSimFuncObjTypes.objectName)
            {
                objId = NameIdData.RainbowNameIds[cmpValueX].ObjectId;
            }
            else if (!string.IsNullOrEmpty(cmpTypeX) && cmpTypeX == AutoSimFuncObjTypes.objectId)
            {
                objId = cmpValueX;
            }
            if (objId != "")
            {
                string[] parts = objId.Split(" ");
                if (parts.Length == 2)
                {
                    ObjTypeIdX = parts[0];
                }
                else
                {
                    tcObj.AppComments = tcObj.AppComments + $" Invalid ObjectId in Compare command";
                    tcObj.PassFail = "F";
                }

            }
        }

        /// <summary>
        /// Check if the operand is a standard one.
        /// </summary>
        /// <returns>True if operand is not in the list, otherwise False.</returns>
        public bool notStandardOperand()
        {
            return !string.IsNullOrEmpty(CmpOperand) && !CmpOperand.Contains(CompareOperands.eq)
                && !CmpOperand.Contains(CompareOperands.lt) && !CmpOperand.Contains(CompareOperands.gt)
                && !CmpOperand.Contains(CompareOperands.lteq) && !CmpOperand.Contains(CompareOperands.gteq)
                && !CmpOperand.Contains(CompareOperands.range);
        }
    }
}
