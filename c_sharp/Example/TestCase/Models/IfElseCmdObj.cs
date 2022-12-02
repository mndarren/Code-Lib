using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using NPOI.OpenXmlFormats.Spreadsheet;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace AAH_AutoSim.TestCase.Models
{
    public class IfElseCmdObj : BaseCmdObj
    {
		public string cmpCmd = "";

		// private string CMDFormat = " [CMD Format: If {Compare Command} Then {True sequence commands} Else {False sequence commands} Endif]";
		private string CMDFormat = $" [CMD Format: If {{type}} {{value}} {{compare}} {{type}} {{value}}]";

		public IfElseCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        override protected void SetPattern()
        {
            _pattern = RegexConstants.IfElseCmdPattern;
        }

        /// <summary>
        /// Assign values to each field
        /// </summary>
        override protected void AssignValues()
        {
            Match match = GetMatch();

            if (match.Success)
            {
				cmpCmd = match.Groups[1].ToString().Trim();
			}
            else
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of If-Else command{CMDFormat}";
                tcObj.PassFail = "F";
            }
        }
    }
}
