using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Util;
using System;
using System.Text.RegularExpressions;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase.Models
{
    public class SaveCmdObj : BaseCmdObj
    {
        public string SaveType = "";
        public string SaveValue = "";
        public string MemoryName = "";
        // the following fields could be null
        public string ObjTypeId = ""; // assign value if objectId type
        public int ModbusAddr = 0;   // assign Modbus address
        public string ModbusName = "";// assign Modbus Name

        private string CMDFormat = $" [CMD Format: Save {{type}} {{value}} to {{MemoryName}}]";

        public SaveCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        override protected void SetPattern()
        {
            _pattern = RegexConstants.SaveCmdPattern;
        }

        /// <summary>
        /// Example:
        /// Save objectId 0x2203 0x6EA8B852 to Save1
        /// Save MemoryName Save1 to ClgCap
        /// Save MbData 11 SF_EBM_SpdAI to MbMem1
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        override protected void AssignValues()
        {
            Match match = GetMatch();

            if (match.Success)
            {
                SaveType = match.Groups[1].ToString().ToLower();
                SaveValue = match.Groups[2].ToString().Trim();
                MemoryName = match.Groups[3].ToString().Trim();
            }
            else
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of Save command{CMDFormat}";
                tcObj.PassFail = "F";
            }

            assignObjTypeId();
            assignMbAddrName();

        }

        /// <summary>
        /// Assign Modbus Address and Name value
        /// </summary>
        /// <param name="SaveType"></param>
        /// <param name="SaveValue"></param>
        private void assignMbAddrName()
        {
            // Assign Modbus address and Name
            if (SaveType != null && SaveType == AutoSimFuncObjTypes.modbusData)
            {
                string[] parts = SaveValue.Split(" ");
                if (parts.Length == 2)
                {
                    ModbusAddr = TCParser.ToInt(parts[0]);
                    ModbusName = parts[1].Trim();
                }
                else
                {
                    tcObj.AppComments = tcObj.AppComments + $" Invalid Modbus Value in Save command";
                    tcObj.PassFail = "F";
                }
            }
        }
        /// <summary>
        /// Assign Object Type Id
        /// </summary>
        private void assignObjTypeId()
        {
            // Assign Object Type Id by right side value
            if (SaveType != null && SaveType == AutoSimFuncObjTypes.objectId)
            {
                string[] parts = SaveValue.Split(" ");
                if (parts.Length == 2)
                {
                    ObjTypeId = parts[0];
                }
                else
                {
                    tcObj.AppComments = tcObj.AppComments + $" Invalid ObjectId in Save command";
                    tcObj.PassFail = "F";
                }
            }
        }
    }
}
