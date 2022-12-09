using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Models;
using AAH_AutoSim.Model.Models;
using Prism.Events;
using AAH_AutoSim.TestCase.Communication;

namespace AAH_AutoSim.TestCase.Execution
{
    public class LogOffCmdExecution : BaseCmdExecution
    {
        public LogOffCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
        }

		override protected void ExecuteCommand()
		{
            // Just check the Log Off command format
            new LogOffCmdObj(_messageDialogService, tcObj);
            // If the command syntax error, just update and return.
            if (tcObj.PassFail == "F")
            {
				tcObj.SetTimestamp();
                return;
            }

            TestLogConfigObj.SetLogOff();

            tcObj.PassFail = "P";
			tcObj.SetTimestamp();
		}
    }
}
