using System;
using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using System.Text.RegularExpressions;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.Model.Models.RainbowModels;
using AAH_AutoSim.TestCase.Util;

namespace AAH_AutoSim.TestCase.Models
{
    public class SetCmdObj : BaseCmdObj
    {
        public string SetTypeTo = "";
        public string SetValueTo = "";
        public string SetTypeFrom = "";
        public string SetValueFrom = "";    // If MemoryName type, update this var name to the value
        public string SetMemNameFrom = "";  // If MemoryName type, store the var name
        // the following fields could be null
        public string ObjTypeIdA; // assign value if objectId type
        public int ModbusAddrA;   // assign Modbus address
        public string ModbusNameA;// assign Modbus Name

        private string CMDFormat = $" [CMD Format: Set {{type}} {{value}} to {{setvalue}} {{value}}]";

        public SetCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        override protected void SetPattern()
        {
            _pattern = RegexConstants.SetCmdPattern;
        }

        /// <summary>
        /// Assign values to class fields
        ///
        /// Example:
        /// BOOL point:  Set objectId 0x2204 0x6EA828C6 to Value 1
        /// FLOAT point: Set objectId 0x2203 0x6EA8B852 to Value 90
        /// WORD point: set objectId 0x2302 0x6EA81C74 to StringValue ManClr
        /// WORD point: set objectId 0x2302 0x6EA81C74 to Value 1 
        /// STR40 point: Set objectId 0x2304 0x6EA8ACD7 to StringValue %s%N%e %a 
        /// ULONG point: Set ObjectId 0x2303 0x00002021 to Value 4 
        /// Modbus point: Set MbData 11 SF_EBM_SpdAO to Value 52 
        /// Set objectId 0x2302 0x6EA81C74 to MemoryName Save3 
        /// 
        /// </summary>
        /// <param name="setCmd">Set Function Command</param>
        override protected void AssignValues()
        {
            Match match = GetMatch();

            if (match.Success)
            {
                SetTypeTo = match.Groups[1].ToString().ToLower();
                SetValueTo = match.Groups[2].ToString().Trim();
                SetTypeFrom = match.Groups[3].ToString();
                SetValueFrom = match.Groups[4].ToString().Trim();

                if (SetTypeFrom.ToLower() == AutoSimFuncObjTypes.MemoryName)
                {
                    SetMemNameFrom = SetValueFrom;
                    // Update SetValueFrom to the real value by the MemoryName
                    if (TestCaseDataShop.memoryNamesFloat.ContainsKey(SetValueFrom))
                    {
                        SetValueFrom = TestCaseDataShop.memoryNamesFloat[SetValueFrom].ToString();
                    }
                    else if (TestCaseDataShop.memoryNamesStr.ContainsKey(SetValueFrom))
                    {
                        SetValueFrom = TestCaseDataShop.memoryNamesStr[SetValueFrom];
                    }
                    else if (TestCaseDataShop.memoryNamesULong.ContainsKey(SetValueFrom))
                    {
                        SetValueFrom = TestCaseDataShop.memoryNamesULong[SetValueFrom].ToString();
                    }
                    else if (TestCaseDataShop.memoryNamesBool.ContainsKey(SetValueFrom))
                    {
                        SetValueFrom = TestCaseDataShop.memoryNamesBool[SetValueFrom].ToString();
                    }
                    else
                    {
                        tcObj.AppComments = tcObj.AppComments + $" Invalid Memory Name in Set command";
                        tcObj.PassFail = "F";
                    }
                }
            }
            else
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of Set command{CMDFormat}";
                tcObj.PassFail = "F";
            }

            assignObjTypeId(SetTypeTo, SetValueTo, ref ObjTypeIdA);
            assignMbAddrName(SetTypeTo, SetValueTo, ref ModbusAddrA, ref ModbusNameA);

        }
        /// <summary>
        /// Assign Modbus Address and Name value
        /// </summary>
        /// <param name="setTypeX"></param>
        /// <param name="setValueX"></param>
        private void assignMbAddrName(string setTypeX, string setValueX, ref int ModbusAddrX, ref string ModbusNameX)
        {
            // Assign Modbus address and Name
            if (setTypeX != null && setTypeX == AutoSimFuncObjTypes.modbusData)
            {
                string[] parts = setValueX.Split(" ");
                if (parts.Length == 2)
                {
                    ModbusAddrX = TCParser.ToInt(parts[0]);
                    ModbusNameX = parts[1].Trim();
                }
                else
                {
                    tcObj.AppComments = tcObj.AppComments + $" Invalid Modbus Value in Set command";
                    tcObj.PassFail = "F";
                }
            }
        }
        /// <summary>
        /// Assign Object Type Id
        /// </summary>
        /// <param name="setTypeX"></param>
        /// <param name="setValueX"></param>
        private void assignObjTypeId(string setTypeX, string setValueX, ref string ObjTypeIdX)
        {
            // Assign Object Type Id by right side value
            if (setTypeX != null && (setTypeX == AutoSimFuncObjTypes.objectId || setTypeX == AutoSimFuncObjTypes.objectName))
            {
                string val = setValueX;
                if (setTypeX == AutoSimFuncObjTypes.objectName)
                {
                    if (!NameIdData.RainbowNameIds.ContainsKey(setValueX))
                    {
                        tcObj.AppComments = $" [{setValueX}]: Invalid ObjectName value in Ramp command";
                        tcObj.PassFail = "F";
                        return;
                    }
                    val = NameIdData.RainbowNameIds[setValueX].ObjectId;
                }
                string[] parts = val.Split(" ");
                if (parts.Length == 2)
                {
                    ObjTypeIdX = parts[0];
                }
                else
                {
                    tcObj.AppComments = tcObj.AppComments + $" Invalid ObjectId Value in Set command";
                    tcObj.PassFail = "F";
                }
            }
        }
    }
}
