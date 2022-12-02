using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using System;
using System.Text.RegularExpressions;

namespace AAH_AutoSim.TestCase.Models
{
    public class LogDeltaCmdObj : BaseCmdObj
    {
        public int xSec = 0;
        private string CMDFormat = $" [CMD Format: Log Delta {{time}} sec]";

        public LogDeltaCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        override protected void SetPattern()
        {
            _pattern = RegexConstants.LogDeltaCmdPattern;
        }

        /// <summary>
        /// Assign values to each field
        /// Exmaple:
        /// Log Delta 5 sec
        /// </summary>
        /// <param name="cmd">Log Delta Command</param>
        override protected void AssignValues()
        {
            Match match = GetMatch();

            if (match.Success)
            {
                bool isNumeric = int.TryParse(match.Groups[1].ToString().Trim(), out xSec);
                if (!isNumeric)
                {
                    tcObj.AppComments = tcObj.AppComments + $" Not integer seconds in Log Delta command";
                    tcObj.PassFail = "F";
                }
            }
            else
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of Log Delta command{CMDFormat}";
                tcObj.PassFail = "F";
            }
        }
    }
}
