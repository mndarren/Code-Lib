using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using System;
using System.Text.RegularExpressions;

namespace AAH_AutoSim.TestCase.Models
{
    public class LogAddCmdObj : BaseCmdObj
    {
        public string objectId = "";
        private string CMDFormat = $" [CMD Format: Log Add {{object id}}]";

        public LogAddCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj)
        {
        }

        /// <summary>
        /// Assign Values
        /// Example:
        /// Log Add 0x2203 0x6EA8538E
        /// </summary>
        /// <param name="cmd"></param>
        protected override void AssignValues()
        {
            Match match = GetMatch();

            if (match.Success)
            {
                objectId = match.Groups[1].ToString().Trim();
            }
            else
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of Log Add command.{CMDFormat}";
                tcObj.PassFail = "F";
            }
        }

        protected override void SetPattern()
        {
            _pattern = RegexConstants.LogAddCmdPattern;
        }
    }
}
