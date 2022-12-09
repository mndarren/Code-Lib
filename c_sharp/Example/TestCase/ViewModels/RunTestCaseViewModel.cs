using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace AAH_AutoSim.TestCase.ViewModels
{
    public class RunTestCaseViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        public DelegateCommand RunWindowLoaded { get; set; }
        //public DelegateCommand RunWindowClosed { get; set; }

        private ObservableCollection<TestCaseDataObj> _dataGridTestCase;
        public ObservableCollection<TestCaseDataObj> DataGridTestCase
        {
            get { return _dataGridTestCase; }
            set
            {
                SetProperty(ref _dataGridTestCase, value);
            }
        }

        private string? _currTestCaseCmd;
        public string? CurrTestCaseCmd
        {
            get { return _currTestCaseCmd; }
            set
            {
                SetProperty(ref _currTestCaseCmd, value);
            }
        }

        private string? _statusCmdMessage;
        public string? StatusCmdMessage
        {
            get { return _statusCmdMessage; }
            set
            {
                SetProperty(ref _statusCmdMessage, value);
            }
        }

        public RunTestCaseViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<TestCmdStrEvent>().Subscribe(UpdateTestCommadStr);
            _eventAggregator.GetEvent<TestStatusMsgEvent>().Subscribe(UpdateStatusMessage);
            _eventAggregator.GetEvent<TestResultEvent>().Subscribe(UpdateTestResult, ThreadOption.UIThread);
            RunWindowLoaded = new DelegateCommand(RunTestCases);
            CurrTestCaseCmd = "Test Command Data";
            TestCaseDataShop.IsRunningTestCases = true;
            GetTestCaseData();
        }

        private void UpdateTestCommadStr(string CommandStr)
        {
            CurrTestCaseCmd = CommandStr;
        }

        private void UpdateStatusMessage(string msg)
        {
            StatusCmdMessage = msg;
        }

        private void UpdateTestResult(TestCaseDataObj result)
        {
            DataGridTestCase.Add(result);
        }
        private void RunTestCases()
        {
            // Make sure the load test cases step works well
            if (TestCaseDataShop.testCaseDataObjs.Count > 0 && TestCaseDataShop.testModuleDataObjs.Count > 0)
            {
                // Pass test cases list and module dictionary to test engine
                TestCaseEngine testCaseEngine = new TestCaseEngine(_eventAggregator, _messageDialogService);
                // Test engine run test cases one by one in steps order
                // Update the test result in the run test case result window
                testCaseEngine.run();
            }

        }

        /// <summary>
        /// Get Test Case Record From Excel file
        /// </summary>
        private void GetTestCaseData()
        {
            try
            {
                DataGridTestCase = TestCaseDataShop.testResultDataObjs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
