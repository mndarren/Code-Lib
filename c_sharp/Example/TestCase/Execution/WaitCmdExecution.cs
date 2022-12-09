using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase.Execution
{
    public class WaitCmdExecution : BaseCmdExecution
    {
        public const int timerInterval = 1000;  // 1 second

        public WaitCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
        }

		/// <summary>
		/// Run Wait Test Case
		/// </summary>
		override protected void ExecuteCommand()
		{

            WaitCmdObj waitCmdObj = new WaitCmdObj(_messageDialogService, tcObj);
            // If the command syntax error, just update and return.
            if (tcObj.PassFail == "F")
			{
				tcObj.SetTimestamp();
                return;
            }

            OnTicks(tcObj, waitCmdObj);

			// Update Pass / Fail result once wait cmd is done
			tcObj.PassFail = "P";
			tcObj.SetTimestamp();
		}

        /// <summary>
        /// Run sleep for x Seconds.
        /// </summary>
        /// <param name="tcObj">Test Case Data Object</param>
        /// <param name="xSec">x Seconds to wait</param>
        private void OnTicks(TestCaseDataObj tcObj, WaitCmdObj waitCmdObj)
        {
            for (int i = waitCmdObj.xSec; i > 0; i--)
            {
                Thread.Sleep(timerInterval);
                string tickCmd = $"{tcObj.Step}: {tcObj.Task} | Wait {i - 1} sec {waitCmdObj.message}";
                _eventAggregator.GetEvent<TestCmdStrEvent>().Publish(tickCmd);
            }
		}
    }
}
