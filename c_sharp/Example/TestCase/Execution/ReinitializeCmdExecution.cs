using AAH_AutoSim.Server;
using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Models;
using AAH_AutoSim.TestCase.Communication;
using Prism.Events;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;
using System.Diagnostics;

namespace AAH_AutoSim.TestCase.Execution
{
    public class ReinitializeCmdExecution : BaseCmdExecution
    {
        private UtilExecution _utilExecution;

        public ReinitializeCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            _utilExecution = new UtilExecution(eventAggregator, messageDialogService);
        }

		/// <summary>
		/// Run Reinitialize Test Case
		/// </summary>
		override protected void ExecuteCommand()
		{
			new ReinitializeCmdObj(_messageDialogService, tcObj);
            // If the command syntax error, just update and return.
            if (tcObj.PassFail == "F")
			{
				tcObj.SetTimestamp();
                return;
            }

            bool isPass = _utilExecution.resetController(tcObj);

            tcObj.PassFail = isPass ? "P" : "F";
			tcObj.SetTimestamp();
        }
    }
}
