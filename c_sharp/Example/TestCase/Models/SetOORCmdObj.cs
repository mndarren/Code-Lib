using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Util;
using System;
using System.Text.RegularExpressions;


namespace AAH_AutoSim.TestCase.Models
{
    public class SetOORCmdObj : BaseCmdObj
    {
        public string ObjectId = "";
        public float SetOORValue;
        private string CMDFormat = $" [CMD Format: SetOOR {{object id}} to {{value}}]";

        public SetOORCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        override protected void SetPattern()
        {
            _pattern = RegexConstants.SetOORCmdPattern;
        }

        /// <summary>
        /// Example: SetOOR 0x2203 0x6EA8B852 to 90
        /// </summary>
        /// <param name="cmd"></param>
        override protected void AssignValues()
        {
            Match match = GetMatch();

            if (match.Success)
            {
                ObjectId = match.Groups[1].ToString().Trim();
                try
                {
                    SetOORValue = TCParser.ToFloat(match.Groups[2].ToString().Trim());
                }
                catch (Exception ex)
                {
                    tcObj.AppComments = tcObj.AppComments + $" Error to convert value to float! {ex.Message}";
                    tcObj.PassFail = "F";
                }
            }
            else
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of SetOOR command.{CMDFormat}";
                tcObj.PassFail = "F";
            }

        }
        
    }
}
