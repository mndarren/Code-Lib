using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using System;
using System.Text.RegularExpressions;

namespace AAH_AutoSim.TestCase.Models
{
    public class LogStartCmdObj : BaseCmdObj
    {
        private string CMDFormat = $" [CMD Format: Log Start]";

        public LogStartCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        /// <summary>
        /// Example: Log Start
        /// </summary>
        /// <param name="cmd"></param>
        protected override void AssignValues()
        {
            Match match = GetMatch();

            if (!match.Success)
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of Log Start command.{CMDFormat}";
                tcObj.PassFail = "F";
            }
        }

        protected override void SetPattern()
        {
            _pattern = RegexConstants.LogStartCmdPattern;
        }
    }
}
