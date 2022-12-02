using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Models;
using Prism.Events;
using System.Collections.ObjectModel;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase.Execution
{
	public class IfElseCmdExecution : BaseCmdExecution
	{
		private bool cmpResult = false;
		public ObservableCollection<TestCaseDataObj> testIfDataObjs = new();
		public ObservableCollection<TestCaseDataObj> testElseDataObjs = new();

		public IfElseCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService) { }

		public bool getCompareResult() { return cmpResult; }

		override protected void ExecuteCommand()
		{
			IfElseCmdObj cmdObj = new IfElseCmdObj(_messageDialogService, tcObj);
			// If the command syntax error, just update and return.
			if (tcObj.PassFail == "F")
			{
				tcObj.SetTimestamp();
				return;
			}

			// Check If {Compare Command}
			TestCaseDataObj cmpTestCaseObj = new TestCaseDataObj();
			cmpTestCaseObj.AutoSimFunction = TestCaseTypeKeyword.Compare + " " + cmdObj.cmpCmd;

			new CompareCmdExecution(_eventAggregator, _messageDialogService).RunCommand(cmpTestCaseObj);

			if (cmpTestCaseObj.PassFail == "P")
			{
				cmpResult = true;
				tcObj.PassFail = "P";
			}
			else
			{
				cmpResult = false;
				tcObj.PassFail = "F";
			}

			tcObj.SetTimestamp();

		}
	}
}