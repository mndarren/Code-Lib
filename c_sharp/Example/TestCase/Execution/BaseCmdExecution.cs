using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Models;
using Prism.Events;

namespace AAH_AutoSim.TestCase.Execution
{
    public abstract class BaseCmdExecution
    {
        protected readonly IMessageDialogService _messageDialogService;
        public IEventAggregator _eventAggregator;
        public TestCaseDataObj tcObj;

        public BaseCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
        }

		protected virtual void ExecuteCommand() {}

		public void RunCommand(TestCaseDataObj tcdObj)
        {
            tcObj = tcdObj;
            // Initialize the PassFail field to NA to avoid original value "F" issue
            tcObj.PassFail = "NA";
            tcObj.AppComments = "";

			ExecuteCommand();
        }
	}
}
