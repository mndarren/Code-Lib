using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Models;
using AAH_AutoSim.Model.Models;
using Prism.Events;
using AAH_AutoSim.TestCase.Communication;

namespace AAH_AutoSim.TestCase.Execution
{
    public class LogDeltaCmdExecution : BaseCmdExecution
    {
        public LogDeltaCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
        }

		/// <summary>
		/// Run Log Delta Cmd Test Case
		/// </summary>
		override protected void ExecuteCommand()
		{
            LogDeltaCmdObj logDeltaCmdObj = new LogDeltaCmdObj(_messageDialogService, tcObj);
            // If the command syntax error, just update and return.
            if (tcObj.PassFail == "F")
			{
				tcObj.SetTimestamp();
                return;
            }

            TestLogConfigObj.LogDelta = logDeltaCmdObj.xSec;

            _eventAggregator.GetEvent<LogDeltaChangeEvent>().Publish(logDeltaCmdObj.xSec);

            tcObj.PassFail = "P";
			tcObj.SetTimestamp();
		}
    }
}
