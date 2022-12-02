using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using System;
using System.Text.RegularExpressions;

namespace AAH_AutoSim.TestCase.Models
{
    public class WaitUntilCmdObj : BaseCmdObj
    {
        public string cmpCmd = "";
        public int xSec = 0;
        private string CMDFormat = $" [CMD Format: WaitUnit {{type}} {{value}} {{compare}} {{type}} {{value}} Wait {{time}} sec]";

        public WaitUntilCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        override protected void SetPattern()
        {
            _pattern = RegexConstants.WaitUntilCmdPattern;
        }

        /// <summary>
        /// Exmaple:
        /// WaitUnit objectId 0x2203 0x6EA8B852 = Value 90 Wait 120 Sec
        /// </summary>
        /// <param name="cmd"></param>
        override protected void AssignValues()
        {
            Match match = GetMatch();

            if (match.Success)
            {
                cmpCmd = match.Groups[1].ToString().Trim();
                bool isNumeric = int.TryParse(match.Groups[2].ToString().Trim(), out xSec);
                if (!isNumeric)
                {
                    tcObj.AppComments = tcObj.AppComments + $" Not integer seconds WaitUntil command";
                    tcObj.PassFail = "F";
                }
            }
            else
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of WaitUntil command.{CMDFormat}";
                tcObj.PassFail = "F";
            }
        }
    }
}
