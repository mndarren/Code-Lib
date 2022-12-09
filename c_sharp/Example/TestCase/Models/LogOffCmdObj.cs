using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using System;
using System.Text.RegularExpressions;

namespace AAH_AutoSim.TestCase.Models
{
    public class LogOffCmdObj : BaseCmdObj
    {
        private string CMDFormat = $" [CMD Format: Log Off]";

        public LogOffCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        /// <summary>
        /// Example:
        /// Log Off
        /// </summary>
        /// <param name="cmd"></param>
        protected override void AssignValues()
        {
            Match match = GetMatch();

            if (!match.Success)
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of Log Off command.{CMDFormat}";
                tcObj.PassFail = "F";
            }
        }

        protected override void SetPattern()
        {
            _pattern = RegexConstants.LogOffCmdPattern;
        }
    }
}
