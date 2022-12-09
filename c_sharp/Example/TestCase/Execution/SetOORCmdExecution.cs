using AAH_AutoSim.Model.Constants;
using AAH_AutoSim.Model.Models;
using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Models;
using Prism.Events;
using System;
using static AAH_AutoSim.Model.Communication.ObjectTypeChecker;

namespace AAH_AutoSim.TestCase.Execution
{
    public class SetOORCmdExecution : BaseCmdExecution
    {
        private UtilExecution _utilExecution;

        public SetOORCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            _utilExecution = new UtilExecution(eventAggregator, messageDialogService);
        }

		/// <summary>
		/// Run Set Test Case
		/// </summary>
		override protected void ExecuteCommand()
		{
            float rightValue;
            bool isPassed = false;

            try
            {
                SetOORCmdObj setOORCmdObj = new SetOORCmdObj(_messageDialogService, tcObj);
                // If the command syntax error, just update and return.
                if (tcObj.PassFail == "F")
				{
					tcObj.SetTimestamp();
                    return;
                }

                rightValue = setOORCmdObj.SetOORValue;
                isPassed = getSetOORCmdResult(setOORCmdObj, rightValue, tcObj);

                tcObj.PassFail = isPassed ? "P" : "F";
            }
            catch (Exception ex)
            {
                tcObj.AppComments = tcObj.AppComments + " " + ex.Message;
                isPassed = false;
			}

			tcObj.SetTimestamp();
			tcObj.PassFail = isPassed ? "P" : "F";
		}
        /// <summary>
        /// Get result for SetOOR command
        /// </summary>
        /// <param name="setOORCmdObj"></param>
        /// <param name="rightValue"></param>
        /// <param name="tcObj"></param>
        /// <returns></returns>
        private bool getSetOORCmdResult(SetOORCmdObj setOORCmdObj, float rightValue, TestCaseDataObj tcObj)
        {
            bool isPassed = false;
            string objectId = setOORCmdObj.ObjectId;
            string addedComment;

            try
            {
                string objTypeId = objectId.Split(" ")[0];
                if (!IsFloat(objTypeId))
                {
                    tcObj.AppComments = tcObj.AppComments + "Invalid type value! The SetOOR command only accept FLOAT type value";
                }
                RainbowInfo rainbowInfo = _utilExecution.rainbow.WriteMember(objectId, ObjectMemberIds.PresentValueId, rightValue);
                var unitLow = _utilExecution.rainbow.ReadMember(objectId, ObjectMemberIds.EngUnitLow).Value;
                var unitHigh = _utilExecution.rainbow.ReadMember(objectId, ObjectMemberIds.EngUnitHigh).Value;
                string unitLowStr = unitLow != null ? unitLow.ToString() : "Null";
                string unitHighStr = unitHigh != null ? unitHigh.ToString() : "Null";

                if (rainbowInfo.ErrorText != null && rainbowInfo.ErrorText.ToLower().Contains(SetConstants.ErrorTypes.OutOfRange))
                {
                    addedComment = "not in range";
                    isPassed = true;
                }
                else
                {
                    addedComment = "in range";
                }
                tcObj.AppComments = tcObj.AppComments + " " + $"{rightValue} {addedComment} [{unitLowStr}, {unitHighStr}]";

            }
            catch (Exception ex)
            {
                tcObj.AppComments = tcObj.AppComments + " " + ex.Message;
            }
            return isPassed;
        }
    }
}
