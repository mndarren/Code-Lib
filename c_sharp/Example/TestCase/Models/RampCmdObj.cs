using System;
using AAH_AutoSim.Server.Dialogs;
using System.Text.RegularExpressions;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.Model.Models.RainbowModels;
namespace AAH_AutoSim.TestCase.Models
{
    public class RampCmdObj : BaseCmdObj
    {
		public string origType = "";
		public string type = "";
        public string value = "";

		public float startValue = 0;
        public float endValue = 0;
        public int xSec = 0;

        private string CMDFormat = " [CMD Format: Ramp {Type} {value} from {startValue} to {endValue} in {time} sec]";

		public RampCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        override protected void SetPattern()
        {
            _pattern = RegexConstants.RampCmdPattern;
		}

		/// <summary>
		/// Assign values to each field and validate them
		/// </summary>
		override protected void AssignValues()
		{
			Match match = GetMatch();

			if (match.Success)
			{
				origType = match.Groups[1].ToString();
				type = origType.ToLower();
				value = match.Groups[2].ToString().Trim();
				startValue = Convert.ToSingle(match.Groups[3].ToString());
				endValue = Convert.ToSingle(match.Groups[4].ToString());
				bool isNumeric = int.TryParse(match.Groups[5].ToString().Trim(), out xSec);
				if (!isNumeric || xSec <= 0)
				{
					tcObj.AppComments = tcObj.AppComments + $"Seconds parameter value in Ramp command must be a positive integer";
					tcObj.PassFail = "F";
					return;
				}
			}
			else
			{
				tcObj.AppComments = tcObj.AppComments + $"Invalid format of Ramp command";
				tcObj.PassFail = "F";
				return;
			}

			if (validateType() == false)
			{
				tcObj.AppComments = $"[{origType}]: Invalid type in Ramp command";
				tcObj.PassFail = "F";
				return;
			}

			if (type == AutoSimFuncObjTypes.objectId || type == AutoSimFuncObjTypes.modbusData)
			{
				string[] parts = value.Split(" ");
				if (parts.Length != 2)
				{
					tcObj.AppComments = $" [{value}]: Invalid {origType} value in Ramp command";
					tcObj.PassFail = "F";
					return;
				}
			}
		}

		private bool validateType()
		{
			if (type == null || type == "")
			{
				return false;
			}

			if (type == AutoSimFuncObjTypes.objectId || type == AutoSimFuncObjTypes.objectName ||
				type == AutoSimFuncObjTypes.modbusData || type == AutoSimFuncObjTypes.MemoryName)
			{
				return true;
			}

			return false;
		}

	}
}
