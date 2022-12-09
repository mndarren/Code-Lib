using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Models;
using Prism.Events;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase.Execution
{
    public class PauseCmdExecution : BaseCmdExecution
    {
        public PauseCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService) { }

		/// <summary>
		/// Example: Pause Pause Test 1
		/// </summary>
		override protected void ExecuteCommand()
		{
            PauseCmdObj cmdObj = new PauseCmdObj(_messageDialogService, tcObj);
            // If the command syntax error, just update and return.
            if (tcObj.PassFail == "F")
			{
				tcObj.SetTimestamp();
                return;
            }

            _messageDialogService.ShowAlertDialog(cmdObj._message, "Click OK When Ready", MessageIcon.Question);
            tcObj.PassFail = "P";
			tcObj.SetTimestamp();

		}
    }
}
