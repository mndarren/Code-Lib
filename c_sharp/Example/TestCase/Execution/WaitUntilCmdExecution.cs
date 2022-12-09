using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Models;
using Prism.Events;
using System.Threading;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase.Execution
{
    public class WaitUntilCmdExecution : BaseCmdExecution
    {

        public WaitUntilCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
        }

		/// <summary>
		/// Run WaitUntil Test Case
		/// </summary>
		override protected void ExecuteCommand()
		{
            WaitUntilCmdObj waitUntilCmdObj = new WaitUntilCmdObj(_messageDialogService, tcObj);
            // If the command syntax error, just update and return.
            if (tcObj.PassFail == "F")
			{
				tcObj.SetTimestamp();
                return;
            }

            TestCaseDataObj cmpTestCaseFromWaitUntil = new TestCaseDataObj();
            cmpTestCaseFromWaitUntil.AutoSimFunction = TestCaseTypeKeyword.Compare + " " + waitUntilCmdObj.cmpCmd;

            OnTicks(tcObj, cmpTestCaseFromWaitUntil, waitUntilCmdObj);

        }
        /// <summary>
        /// Run Compare Test Case per second for x Seconds.
        /// </summary>
        /// <param name="tcObj">Test Case Data Object</param>
        /// <param name="cmpTestCaseFromWaitUntil">Compare Test Case</param>
        /// <param name="xSec">x Seconds to wait</param>
        private void OnTicks(TestCaseDataObj tcObj, TestCaseDataObj cmpTestCaseFromWaitUntil, WaitUntilCmdObj waitUntilCmdObj)
        {
            for (int i = waitUntilCmdObj.xSec; i > 0; i--)
            {
                new CompareCmdExecution(_eventAggregator, _messageDialogService).RunCommand(cmpTestCaseFromWaitUntil);

                if (cmpTestCaseFromWaitUntil.PassFail == "P")
                {
                    tcObj.PassFail = "P";
                    tcObj.AppComments = cmpTestCaseFromWaitUntil.AppComments;
					tcObj.SetTimestamp();
					return;
                }
                Thread.Sleep(OneSecond);
                string tickCmd = $"{tcObj.Step}: {tcObj.Task} | WaitUntil {waitUntilCmdObj.cmpCmd} Wait {i - 1} sec";
                _eventAggregator.GetEvent<TestCmdStrEvent>().Publish(tickCmd);
            }
            if (cmpTestCaseFromWaitUntil.PassFail == "F")
            {
                tcObj.PassFail = "F";
                tcObj.AppComments = cmpTestCaseFromWaitUntil.AppComments;
				tcObj.SetTimestamp();
			}
        }
    }
}
