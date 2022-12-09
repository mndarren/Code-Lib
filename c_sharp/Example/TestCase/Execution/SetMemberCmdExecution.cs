using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Models;
using AAH_AutoSim.TestCase.Util;
using Prism.Events;
using System;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase.Execution
{
    public class SetMemberCmdExecution : BaseCmdExecution
    {
        private UtilExecution _utilExecution;

        public SetMemberCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            _utilExecution = new UtilExecution(eventAggregator, messageDialogService);
        }

        /// <summary>
        /// Run SetMember Test Case
        /// </summary>
        override protected void ExecuteCommand()
        {
            bool isPass = false;
            string addedComments = "";

            try
            {
                SetMemberCmdObj setCmdObj = new SetMemberCmdObj(_messageDialogService, tcObj);
                // If the command syntax error, just update and return.
                if (tcObj.PassFail == "F")
                {
                    tcObj.SetTimestamp();
                    return;
                }

                float writeValue = TCParser.ToFloat(setCmdObj.SMValue);
                isPass = _utilExecution.setValueByObjectId(setCmdObj.SMObjId, writeValue, tcObj,
                    AutoSimFuncObjTypes.objectId, setCmdObj.SMMemId);
                addedComments = " " + $"Set Member Value {writeValue}";
                

                tcObj.AppComments = tcObj.AppComments + addedComments;
            }
            catch (Exception ex)
            {
                tcObj.AppComments = tcObj.AppComments + " " + ex.Message;
                isPass = false;
            }

            tcObj.SetTimestamp();
            tcObj.PassFail = isPass ? "P" : "F";
        }

    }
}
