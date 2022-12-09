using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using System;
using System.Text.RegularExpressions;

namespace AAH_AutoSim.TestCase.Models
{
	public class ConfigCmdObj : BaseCmdObj
	{
		public string _configString = "";
		public const int ConfigStringLength = 30;

		private string CMDFormat = $" [CMD Format: Config {{configString}}]";
		
		public ConfigCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

		override protected void SetPattern()
		{
			_pattern = RegexConstants.ConfigCmdPattern;
		}

		/// <summary>
		/// Assign values to each field
		/// </summary>
		override protected void AssignValues()
		{
			Match match = GetMatch();

			if (match.Success)
			{
				_configString = match.Groups[1].ToString().Trim();
				if (_configString.Length != ConfigStringLength)
				{
                    tcObj.AppComments = tcObj.AppComments + $" Error in Config command: Invalid Config String length";
                    tcObj.PassFail = "F";
                }
			}
			else
			{
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of Config command{CMDFormat}";
                tcObj.PassFail = "F";
            }
		}
	}
}
