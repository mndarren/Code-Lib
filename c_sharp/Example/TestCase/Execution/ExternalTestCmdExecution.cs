using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Models;
using Prism.Events;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase.Execution
{
    public class ExternalTestCmdExecution : BaseCmdExecution
    {
        public ExternalTestCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService) { }

		/// <summary>
		/// Example:
		/// External Test message 1
		/// </summary>
		override protected void ExecuteCommand()
		{
            ExternalTestCmdObj cmdObj = new ExternalTestCmdObj(_messageDialogService, tcObj);
            // If the command syntax error, just update and return.
            if (tcObj.PassFail == "F")
            {
				tcObj.SetTimestamp();
                return;
            }
            MessageDialogResult result = _messageDialogService.ShowYesNoDialog(cmdObj._message, "Click Yes button if the test passed and No if it failed", MessageIcon.Question);
 
            tcObj.PassFail = (result == MessageDialogResult.Yes) ? "P": "F";
   
			tcObj.SetTimestamp();
        }
    }
}
