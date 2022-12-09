using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using System;
using System.Text.RegularExpressions;

namespace AAH_AutoSim.TestCase.Models
{
    public class ExternalTestCmdObj : BaseCmdObj
    {
        public string _message = "";
        private string CMDFormat = $" [CMD Format: External Test {{Message}}]";

        public ExternalTestCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        override protected void SetPattern()
        {
            _pattern = RegexConstants.ExternalCmdPattern;
        }

        /// <summary>
        /// Assign values to each field
        /// </summary>
        override protected void AssignValues()
        {
            Match match = GetMatch();

            if (match.Success)
            {
                _message = match.Groups[1].ToString().Trim();
                if (_message == null || _message.Length == 0)
                {
                    tcObj.AppComments = tcObj.AppComments + $" Missing parameter in External Test command";
                    tcObj.PassFail = "F";
                }
            }
            else
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of External Test command{CMDFormat}";
                tcObj.PassFail = "F";
            }
        }
    }
}
