using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Util;
using System.Text.RegularExpressions;
using System;
using System.Globalization;

namespace AAH_AutoSim.TestCase.Models
{
    public class SetMemberCmdObj : BaseCmdObj
    {
        public string SMObjId = "";
        public Int32 SMMemId = TestCaseConstants.ErrorValueInt;
        public string SMValue = "";
        public string SMObjTypeId = "";

        private string CMDFormat = $" [CMD Format: SetMember {{ObjectId}} {{MemberId}} to Value {{value}}]";

        public SetMemberCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj) : base(messageDialogService, tcObj) { }

        override protected void SetPattern()
        {
            _pattern = RegexConstants.SetMemberCmdPattern;
        }

        /// <summary>
        /// Assign values to class fields
        ///
        /// Example:
        /// BOOL point:  SetMember 0x2204 0x6EA82B2A 0x0106 to Value 1
        /// 
        /// </summary>
        override protected void AssignValues()
        {
            Match match = GetMatch();
            string errorMsg = "";

            if (match.Success)
            {
                SMObjTypeId = match.Groups[1].ToString();
                string ObjId2ndPart = match.Groups[2].ToString();
                SMObjId = SMObjTypeId + " " + ObjId2ndPart;
                string SMMemIdStr = match.Groups[3].ToString();
                SMValue = match.Groups[4].ToString();

                if (string.IsNullOrEmpty(SMObjTypeId) || string.IsNullOrEmpty(ObjId2ndPart)) errorMsg = "Object Id is invalid";
                if (string.IsNullOrEmpty(SMMemIdStr)) errorMsg = "Member Id is invalid";
                if (string.IsNullOrEmpty(SMValue)) errorMsg = "Set Value is invalid";

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    tcObj.AppComments = tcObj.AppComments + $" {errorMsg}";
                    tcObj.PassFail = "F";
                    return;
                }
                // Remove 0x from Member Id
                SMMemId = Int32.Parse(SMMemIdStr.Substring(2), NumberStyles.HexNumber);
            }
            else
            {
                tcObj.AppComments = tcObj.AppComments + $" Invalid format of Set command{CMDFormat}";
                tcObj.PassFail = "F";
                return;
            }

        }

    }
}
