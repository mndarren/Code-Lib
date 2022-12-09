using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using System;
using System.Text.RegularExpressions;

namespace AAH_AutoSim.TestCase.Models
{
    public class WaitCmdObj : BaseCmdObj
    {
        public int xSec = 0;
        public string message = "";
        private string CMDFormat = $" [CMD Format: Wait {{time}} sec {{message}}]";

        public WaitCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        override protected void SetPattern()
        {
            _pattern = RegexConstants.WaitCmdPattern;
        }

        /// <summary>
        /// Assign values to each field
        /// Example: 
        /// Wait 90 Sec for MT3 to restart
        /// </summary>
        /// <param name="_command">Wait Command</param>
        override protected void AssignValues()
        {
            Match match = GetMatch();

            if (match.Success)
            {
                bool isNumeric = int.TryParse(match.Groups[1].ToString().Trim(), out xSec);
                if (!isNumeric || xSec < 0)
                {
                    tcObj.AppComments = tcObj.AppComments + $" Not integer OR Negative integer seconds in Wait command";
                    tcObj.PassFail = "F";
                }

                message = match.Groups[2].ToString().Trim();
                if (message == null) message = "";
            }
            else
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of Wait command.{CMDFormat}";
                tcObj.PassFail = "F";
            }
        }
    }
}
