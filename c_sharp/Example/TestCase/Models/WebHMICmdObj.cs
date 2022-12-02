using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AAH_AutoSim.TestCase.Models
{
    public class WebHMICmdObj : BaseCmdObj
    {
        public string WebControl = "";
        public string WebObjName = "";
        public string WebOperator = "";
        public string WebValue = "";
        // Object Name -> Object Id
        public string WebObjId = "";

        private string CMDFormat = $" [CMD Format: WebHMI {{Control}} {{ObjName}} {{Operator}} {{Value}}]";


        public WebHMICmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        protected override void SetPattern()
        {
            _pattern = RegexConstants.WebHMICmdPattern;
        }

        protected override void AssignValues()
        {
            Match match = GetMatch();
            string errorMsg = "";

            if (match.Success)
            {
                WebControl = match.Groups[1].ToString();
                WebObjName = match.Groups[2].ToString();
                WebOperator = match.Groups[3].ToString();
                WebValue = match.Groups[4].ToString();

                if (string.IsNullOrEmpty(WebControl)) errorMsg = "WebControl is invalid";
                if (string.IsNullOrEmpty(WebObjName)) errorMsg = "WebObjName is invalid";
                if (string.IsNullOrEmpty(WebOperator)) errorMsg = "WebOperator is invalid";
                if (string.IsNullOrEmpty(WebValue)) errorMsg = "WebValue is invalid";

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    tcObj.AppComments = tcObj.AppComments + $" {errorMsg}";
                    tcObj.PassFail = "F";
                    return;
                }

            }
            else
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of WebHMI command: {CMDFormat}";
                tcObj.PassFail = "F";
                return;
            }
        }

    }
}
