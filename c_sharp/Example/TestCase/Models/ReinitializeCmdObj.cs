using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using System;
using System.Text.RegularExpressions;

namespace AAH_AutoSim.TestCase.Models
{
    public class ReinitializeCmdObj : BaseCmdObj
    {
        private string CMDFormat = $" [CMD Format: Reinitialize]";

        public ReinitializeCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        override protected void SetPattern()
        {
            _pattern = RegexConstants.ReinitializeCmdPattern;
        }

        /// <summary>
        /// Assign values to each field
        /// </summary>
        override protected void AssignValues()
        {
            Match match = GetMatch();

            if (!match.Success)
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of Reinitialize command{CMDFormat}";
                tcObj.PassFail = "F";
            }
        }
    }
}
