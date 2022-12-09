using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Models;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Prism.Events;
using System;
using System.Threading;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase.Execution
{
    public class RampCmdExecution : BaseCmdExecution
    {
		public RampCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService) { }

		override protected void ExecuteCommand()
		{
			StartRamp();
		}

		private async void StartRamp()
		{
			// Run Ramp in a separate thread
			await System.Threading.Tasks.Task.Run(() => Ramp());
		}

		private void Ramp()
		{
			bool isPass = false;

			RampCmdObj cmdObj = new RampCmdObj(_messageDialogService, tcObj);
			// If the command has a syntax error, just update status and return.
			if (tcObj.PassFail == "F")
			{
				tcObj.SetTimestamp();
				return;
			}

			DateTime StartTime = DateTime.Now;

			try
			{
				float setValue = cmdObj.startValue;
				float deltaValue = (cmdObj.endValue - cmdObj.startValue) / cmdObj.xSec;
				for (int i = 0; i <= cmdObj.xSec; i++)
				{
					isPass = getCmdResult(cmdObj, setValue);
					if (TestCaseDataShop.IsRunningTestCases == false)
					{
						isPass = false;
						break;
					}
					// tcObj.AppComments alredy set by getCmdResult() so just exit the loop
					if (isPass == false) break;
					setValue += deltaValue;
					Thread.Sleep(1000);
				}
			}
			catch (Exception ex)
			{
				tcObj.AppComments += "Exception: " + ex.Message;
			}

			if (isPass)
			{
				TimeSpan ts = DateTime.Now.Subtract(StartTime);
				string elapsedTime = String.Format("{0:00}.{1:00}", ts.TotalSeconds, ts.TotalMilliseconds / 10);
				tcObj.AppComments += "Command Completed in " + elapsedTime + " seconds. ";
			}

			tcObj.SetTimestamp();
			tcObj.PassFail = isPass ? "P" : "F";

            // Finally filter Regression Test Ids
            if (TestCaseDataShop.IsRegressionTest && ((tcObj.ExpectedResult == "P" && tcObj.PassFail != "P") ||
                (tcObj.ExpectedResult == "F" && tcObj.PassFail != "F") || (tcObj.ExpectedResult == "NA" && tcObj.PassFail != "NA")))
            {
                TestCaseDataShop.RegresTestFailedStepIds.Add(tcObj.Step);
            }
        }

		/// <summary>
		/// Set value to the object
		/// </summary>
		/// <param name="cmdObj"></param>
		/// <param name="writeValue"></param>
		/// <returns>True if set value successfully, otherwise False</returns>
		private bool getCmdResult(RampCmdObj cmdObj, float writeValue)
		{
			bool isPassed = false;
			TestCaseDataObj cmdTestCaseObj = new TestCaseDataObj();

			// If Test case is finished, terminate Ramp loop
			if (TestCaseDataShop.IsRunningTestCases == false) 
			{
				tcObj.AppComments = "Terminated";
				tcObj.PassFail = "F";
				return isPassed;
			}

			try
			{
				switch (cmdObj.type)
				{
					case AutoSimFuncObjTypes.objectName:
					case AutoSimFuncObjTypes.objectId:
					case AutoSimFuncObjTypes.modbusData:
						cmdTestCaseObj.AutoSimFunction = TestCaseTypeKeyword.Set + " " + cmdObj.type + " " + cmdObj.value + " to Value " + writeValue;
						new SetCmdExecution(_eventAggregator, _messageDialogService).RunCommand(cmdTestCaseObj);
						if (cmdTestCaseObj.PassFail == "F") tcObj.AppComments += " " + cmdTestCaseObj.AppComments;
						break;
					case AutoSimFuncObjTypes.MemoryName:
						cmdTestCaseObj.AutoSimFunction = TestCaseTypeKeyword.Save + " Value " + writeValue + " to " + cmdObj.value;
						new SaveCmdExecution(_eventAggregator, _messageDialogService).RunCommand(cmdTestCaseObj);
						if (cmdTestCaseObj.PassFail == "F") tcObj.AppComments += " " + cmdTestCaseObj.AppComments;
						break;
					default:
						tcObj.AppComments += $"{cmdObj.origType} is not a valid Type for Ramp command";
						tcObj.PassFail = "F";
						break;
				}
			}
			catch (Exception ex)
			{
				tcObj.AppComments += " " + ex.Message;
				tcObj.PassFail = "F";
			}

			if (cmdTestCaseObj.PassFail == "P")
			{
				isPassed = true;
			}

			// If Test case is finished, terminate Ramp loop
			if (TestCaseDataShop.IsRunningTestCases == false)
			{
				tcObj.AppComments = "Terminated";
				tcObj.PassFail = "F";
				isPassed = false;
			}

			return isPassed;
		}


	}
}
