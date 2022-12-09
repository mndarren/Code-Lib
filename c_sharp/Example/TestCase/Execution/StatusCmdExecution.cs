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
    public class StatusCmdExecution : BaseCmdExecution
    {
        public StatusCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService) { }

		override protected void ExecuteCommand()
		{
            StatusCmdObj cmdObj = new StatusCmdObj(_messageDialogService, tcObj);
            // If the command syntax error, just update and return.
            if (tcObj.PassFail == "F")
			{
				tcObj.SetTimestamp();
                return;
            }
            // Set RunTestCaseViewModel.StatusCmdMessage to cmdObj._message
            _eventAggregator.GetEvent<TestStatusMsgEvent>().Publish(cmdObj._message);

			tcObj.PassFail = "P";
			tcObj.SetTimestamp();
		}
    }
}
