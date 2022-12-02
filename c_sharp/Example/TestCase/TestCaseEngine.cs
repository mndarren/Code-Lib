using AAH_AutoSim.Server;
using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.Server.SystemLog.Config;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Execution;
using AAH_AutoSim.TestCase.Models;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Documents;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase
{
    public class TestCaseResults
    {
        public int totalRunCmdCount = 0;
        public int failedCmdCount = 0;
        public int passedCmdCount = 0;
		public string executionTime = "";

		public void setExecutionTime(TimeSpan ts)
        {
            executionTime = String.Format("{0:00}:{1:00}:{2:00}.{3}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
        }

        public int getSkippedCmdCount()
        {
            return totalRunCmdCount - passedCmdCount - failedCmdCount;
        }

        public void updateTestCaseResults(TestCaseDataObj tcObj)
        {
			if (tcObj.PassFail == "F") failedCmdCount++;
			if (tcObj.PassFail == "P") passedCmdCount++;
			totalRunCmdCount++;
		}
	}


    public class TestCaseEngine
    {
        private IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;

        private CompareCmdExecution compareCmdExecution;
        private SetCmdExecution setCmdExecution;
        private WaitUntilCmdExecution waitUntilCmdExecution;
        private SaveCmdExecution saveCmdExecution;
        private SetOORCmdExecution setOORCmdExecution;
		private WaitCmdExecution waitCmdExecution;
        private ConfigCmdExecution configCmdExecution;
        private PauseCmdExecution pauseCmdExecution;
        private ExternalTestCmdExecution externalTestCmdExecution;
        private ReinitializeCmdExecution reinitializeCmdExecution;
        private IfElseCmdExecution ifElseCmdExecution;
        private StatusCmdExecution statusCmdExecution;
        private WebHMICmdExecution webHMICmdExecution;
        private SetMemberCmdExecution setMemberCmdExecution;
        private LogStartCmdExecution logStartCmdExecution;
        private LogOffCmdExecution logOffCmdExecution;
        private LogDeltaCmdExecution logDeltaCmdExecution;
        private LogAddCmdExecution logAddCmdExecution;

        // the following is for final report
        private TestCaseResults results;

        // Current Module name
        private string currModuleName = "";

        public TestCaseEngine(IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
            results = new TestCaseResults();
            
            compareCmdExecution = new CompareCmdExecution(_eventAggregator, _messageDialogService);
			setCmdExecution = new SetCmdExecution(_eventAggregator, _messageDialogService);
			waitUntilCmdExecution = new WaitUntilCmdExecution(_eventAggregator, _messageDialogService);
			saveCmdExecution = new SaveCmdExecution(_eventAggregator, _messageDialogService);
			setOORCmdExecution = new SetOORCmdExecution(_eventAggregator, _messageDialogService);
			waitCmdExecution = new WaitCmdExecution(_eventAggregator, _messageDialogService);
            logStartCmdExecution = new LogStartCmdExecution(_eventAggregator, _messageDialogService);
            logOffCmdExecution = new LogOffCmdExecution(_eventAggregator, _messageDialogService);
            logDeltaCmdExecution = new LogDeltaCmdExecution(_eventAggregator, _messageDialogService);
            logAddCmdExecution = new LogAddCmdExecution(_eventAggregator, _messageDialogService);
            configCmdExecution = new ConfigCmdExecution(_eventAggregator, _messageDialogService);
            pauseCmdExecution = new PauseCmdExecution(_eventAggregator, _messageDialogService);
            externalTestCmdExecution = new ExternalTestCmdExecution(_eventAggregator, _messageDialogService);
            reinitializeCmdExecution = new ReinitializeCmdExecution(_eventAggregator, _messageDialogService);
            ifElseCmdExecution = new IfElseCmdExecution(_eventAggregator, _messageDialogService);
            statusCmdExecution = new StatusCmdExecution(_eventAggregator, _messageDialogService);
            setMemberCmdExecution = new SetMemberCmdExecution(_eventAggregator, _messageDialogService);
            webHMICmdExecution = new WebHMICmdExecution(_eventAggregator, _messageDialogService);
		}

        /// <summary>
        /// Run Test Cases first, and then write out the test result to Excel file and show it on UI.
        /// </summary>
        /// <returns></returns>
        public void run()
        {
            RunTestCases();

            //new GenerateTestResultFileServiceNPOI(_messageDialogService).CreateTestResultFile();

        }

        private void AddTestCmdStr(TestCaseDataObj tcObj)
        {
			_eventAggregator.GetEvent<TestCmdStrEvent>().Publish($"{tcObj.Step}: {tcObj.Task} | {tcObj.AutoSimFunction}");
		}

        /// <summary>
        /// Run One Test Case
        /// </summary>
        public async void RunTestCase(TestCaseDataObj tcObj)
		{
			// Make sure it's not empty
			if (!string.IsNullOrEmpty(tcObj.AutoSimFunction))
			{
				AddTestCmdStr(tcObj);
				string cmdLo = tcObj.AutoSimFunction.ToLower();
				if (cmdLo.StartsWith(TestCaseTypeKeyword.RunModule))
				{
					await System.Threading.Tasks.Task.Run(() => RunTestModule(tcObj));

				}
				else
				{
					await System.Threading.Tasks.Task.Run(() => RunTestCasesNoModule(tcObj));
				}
			}
			_eventAggregator.GetEvent<TestResultEvent>().Publish(tcObj);
		}

		/// <summary>
		/// Run All Test Cases
		/// </summary>
		public async void RunTestCases()
        {
            bool isIfElseBlock = false;
            bool isConditionalIfCommand = false;
            bool isConditionalElseCommand = false;
            bool ifElseResult = false;
            TestCaseDataShop.RegresTestFailedStepIds = new List<string>();
            // Monitor execution time
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Run Auto Sim Functions from Test Case Excel file
            foreach (TestCaseDataObj tcObj in TestCaseDataShop.testCaseDataObjs)
            {
                // Make sure it's not empty
                if (!string.IsNullOrEmpty(tcObj.AutoSimFunction))
                {					
                    string cmdLo = tcObj.AutoSimFunction.ToLower().Trim();
                    // Run If Else block, special block
                    if (RunIfElseBlock(tcObj, ref isIfElseBlock, ref isConditionalIfCommand, ref isConditionalElseCommand, ref ifElseResult)) continue;

					AddTestCmdStr(tcObj);

					if (cmdLo.StartsWith(TestCaseTypeKeyword.RunModule))
                    {
                        await System.Threading.Tasks.Task.Run(() => RunTestModule(tcObj));

                        results.updateTestCaseResults(tcObj);

                        AutoSimMain.SendToAutoSimMainEvent(LogFilterType.SystemEvent, string.Format("TestCaseEngine.cs - Test Case Step- {0}", tcObj.Step));
                    }
                    else
                    {
                        await System.Threading.Tasks.Task.Run(() => RunTestCasesNoModule(tcObj));
                    }

                    FilterRegresTestId(tcObj);
                    _eventAggregator.GetEvent<TestResultEvent>().Publish(tcObj);
				}
 
                // Stop running test case commands if On Failure Stop is checked
                if (TestCaseDataShop.IsOnFailureStop && tcObj.PassFail == "F") break;
                // If Run Test Case window closed, we stop test case running
                if (TestCaseDataShop.IsRunTestCasesWindowClosed) break;
            }

			_eventAggregator.GetEvent<RunTestCasesCompleteEvent>().Publish();

            // Quit Chrome Driver
            if (TestCaseDataShop.webHMISelenium != null)
            {
                TestCaseDataShop.webHMISelenium.QuitDriver();
                TestCaseDataShop.webHMISelenium = null;
            }
            // Final Statistic Report
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            results.setExecutionTime(ts);
            string finalMsg;

			// If Regression Test
			if (TestCaseDataShop.IsRegressionTest)
            {
                if (TestCaseDataShop.RegresTestFailedStepIds.Count > 0)
                {
                     finalMsg = $"Failed Commands: {string.Join(",", TestCaseDataShop.RegresTestFailedStepIds.ToArray())}";
                    TestCaseDataShop.TestStatus = "F";
                }
                else
                {
                    finalMsg = "ALL PASSED!";
                    TestCaseDataShop.TestStatus = "P";
                }

                TestCaseDataShop.IsRegressionTest = false;
            }
            else if (results.failedCmdCount > 0)
            {
                finalMsg = $"Passed Commands: {results.passedCmdCount},{Environment.NewLine}Failed Commands: {results.failedCmdCount}," +
                    $"{Environment.NewLine}Skipped Commands: {results.getSkippedCmdCount()}.";
                TestCaseDataShop.TestStatus = "F";
            }
            else
            {
                finalMsg = "ALL PASSED!";
                TestCaseDataShop.TestStatus = "P";
            }

			_eventAggregator.GetEvent<TestCmdStrEvent>().Publish($"Total Test Commands: {results.totalRunCmdCount} [Execution Time: {results.executionTime}]{Environment.NewLine}{finalMsg}");

            new ExcelWriteServiceNPOI(_messageDialogService).CreateTestResultFile(results);
            // Just wait for finishing dumping log data from log collection to Log file
            while (TestCaseDataShop.LogWriterTask != null && !TestCaseDataShop.LogWriterTask.IsCompleted)
            {
                Thread.Sleep(1000);
            }
 
            AutoSimMain.SendToAutoSimMainEvent(LogFilterType.SystemEvent, string.Format("TestCaseEngine.cs - Test Case Completed - {0}", finalMsg));

            //Flush The System Event Log After Test Case is Completed
            AutoSimMain.Eventlogger.Flush();
        }
        /// <summary>
        /// Run test cases, not including run test modules
        /// </summary>
        /// <param name="tcObj">Test Case Data Object</param>
        public void RunTestCasesNoModule(TestCaseDataObj tcObj, bool isModule=false)
        {
            // Make sure it's not empty
            if (!string.IsNullOrEmpty(tcObj.AutoSimFunction))
            {
                // If Module CMD, update step string, and publish CMD
                if (isModule)
                {
                    UpdateModuleStepId(tcObj, isModule);
					AddTestCmdStr(tcObj);
				}
                string cmdLo = tcObj.AutoSimFunction.ToLower().Trim();
                switch(cmdLo)
                {
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.Compare): compareCmdExecution.RunCommand(tcObj); break;
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.WaitUtil): waitUntilCmdExecution.RunCommand(tcObj); break;
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.Set): setCmdExecution.RunCommand(tcObj); break;
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.Save): saveCmdExecution.RunCommand(tcObj); break;
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.SetOOR): setOORCmdExecution.RunCommand(tcObj); break;
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.Wait): waitCmdExecution.RunCommand(tcObj); break;
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.LogStart): logStartCmdExecution.RunCommand(tcObj); break;
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.LogOff): logOffCmdExecution.RunCommand(tcObj); break;
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.LogDelta): logDeltaCmdExecution.RunCommand(tcObj); break;
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.LogAdd): logAddCmdExecution.RunCommand(tcObj); break;
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.Pause): pauseCmdExecution.RunCommand(tcObj); break;
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.ExternalTest): externalTestCmdExecution.RunCommand(tcObj); break;
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.Status): statusCmdExecution.RunCommand(tcObj); break;
                    case { } when cmdLo.StartsWith(TestCaseTypeKeyword.Reinitialize): reinitializeCmdExecution.RunCommand(tcObj); break;
					case { } when cmdLo.StartsWith(TestCaseTypeKeyword.Config): configCmdExecution.RunCommand(tcObj); break;
					case { } when cmdLo.StartsWith(TestCaseTypeKeyword.If): ifElseCmdExecution.RunCommand(tcObj); break;
					case { } when cmdLo.StartsWith(TestCaseTypeKeyword.SetMember): setMemberCmdExecution.RunCommand(tcObj); break;
					case { } when cmdLo.StartsWith(TestCaseTypeKeyword.WebHMI): webHMICmdExecution.RunCommand(tcObj); break;
					case { } when cmdLo.StartsWith(TestCaseTypeKeyword.Ramp): new RampCmdExecution(_eventAggregator, _messageDialogService).RunCommand(tcObj); break;
                    default:  // Don't know the CMD keyword
                        tcObj.PassFail = "F";
                        tcObj.AppComments = "Invalid CMD!";
                        break;
                }

                results.updateTestCaseResults(tcObj);
				AutoSimMain.SendToAutoSimMainEvent(LogFilterType.SystemEvent, string.Format("TestCaseEngine.cs - Test Case Step- {0}", tcObj.Step));
            }
            if (isModule)
            {
                if (!string.IsNullOrEmpty(tcObj.AutoSimFunction))  // Do not publish empty command line
                {
                    _eventAggregator.GetEvent<TestResultEvent>().Publish(tcObj);
                    FilterRegresTestId(tcObj);
                }
            }
        }

        /// <summary>
        /// Run Test Module if Auto Sim Function starts with "Run"
        /// </summary>
        /// <param name="tcObj">Test Case Object</param>
        private void RunTestModule(TestCaseDataObj tcObj)
        {
            bool isPassedModule = true;
            bool isIfElseBlock = false;
            bool isConditionalIfCommand = false;
            bool isConditionalElseCommand = false;
            bool ifElseResult = false;

            string command = tcObj.AutoSimFunction;
            currModuleName = command.Substring(command.IndexOf(" ") + 1).Trim();
            string errorMsg = "";
            if (!TestCaseDataShop.testModuleDataObjs.ContainsKey(currModuleName))
            {
                _messageDialogService.ShowAlertDialog($"The Test Module {currModuleName} cannot be found", "Run Test Module", MessageIcon.Error);
                return;
            }
            ObservableCollection<TestCaseDataObj> testModuleDataObjs = TestCaseDataShop.testModuleDataObjs[currModuleName];

            foreach (TestCaseDataObj testModuleDataObj in testModuleDataObjs)
            {
                // Don't need null check here, since the following function will do null check
                if (RunIfElseBlock(testModuleDataObj, ref isIfElseBlock, ref isConditionalIfCommand, ref isConditionalElseCommand, ref ifElseResult, true)) continue;
                RunTestCasesNoModule(testModuleDataObj, true);
                // Only add first Failed comment to the Module CMD line Comment
                if (isPassedModule && testModuleDataObj.PassFail != null && testModuleDataObj.PassFail == "F" &&
                    testModuleDataObj.AutoSimFunction != null && !testModuleDataObj.AutoSimFunction.ToLower().StartsWith(TestCaseTypeKeyword.If))
                {
                    isPassedModule = false;
                    tcObj.AppComments = testModuleDataObj.AppComments;
                }
            }
            tcObj.PassFail = isPassedModule ? "P" : "F";
            if (errorMsg != "") tcObj.AppComments = errorMsg;
			tcObj.SetTimestamp();
		}
        /// <summary>
        /// The IfElse block task will be run in test module and test case.
        /// </summary>
        /// <param name="tcObj"></param>
        /// <param name="isIfElseBlock"></param>
        /// <param name="isConditionalIfCommand"></param>
        /// <param name="isConditionalElseCommand"></param>
        /// <param name="ifElseResult"></param>
        /// <param name="isModule"></param>
        /// <returns></returns>
        private bool RunIfElseBlock(TestCaseDataObj tcObj, ref bool isIfElseBlock, ref bool isConditionalIfCommand,
             ref bool isConditionalElseCommand, ref bool ifElseResult, bool isModule=false)
        {
            string cmdLo = tcObj.AutoSimFunction.ToLower();
            bool retureV = false;
            // Special code for If_Else command
            if (cmdLo.StartsWith(TestCaseTypeKeyword.If))
            {
                isIfElseBlock = true;
                isConditionalIfCommand = true;
                isConditionalElseCommand = false;
				// Run "if" command
				AddTestCmdStr(tcObj);
				RunTestCasesNoModule(tcObj);
                ifElseResult = ifElseCmdExecution.getCompareResult();
                IfElseActions(tcObj, isModule);
                retureV = true;
            }
            else if (cmdLo.StartsWith(TestCaseTypeKeyword.Else) && isIfElseBlock)
            {
                isConditionalIfCommand = false;
                isConditionalElseCommand = true;
                tcObj.PassFail = "NA";
                IfElseActions(tcObj, isModule);
                retureV = true;
            }
            else if (cmdLo.StartsWith(TestCaseTypeKeyword.Endif) && isIfElseBlock)
            {
                // End of if-else block, reset all values and continue to the next command
                isIfElseBlock = false;
                isConditionalIfCommand = false;
                isConditionalElseCommand = false;
                ifElseResult = false;
                tcObj.PassFail = "NA";
                IfElseActions(tcObj, isModule);
                retureV = true;
            }
            else if (isIfElseBlock && isConditionalIfCommand && (ifElseResult == false))
            {
                // If ifElseResult == false skip every command in "if" block
                tcObj.PassFail = "NA";
                tcObj.AppComments = "Skipped";
                IfElseActions(tcObj, isModule);
                retureV = true;
            }
            else if (isIfElseBlock && isConditionalElseCommand && (ifElseResult == true))
            {
                // If ifElseResult == true skip every command in "else" block
                tcObj.PassFail = "NA";
                tcObj.AppComments = "Skipped";
                IfElseActions(tcObj, isModule);
                retureV = true;
            }
            return retureV;
        }
        /// <summary>
        /// Actions for If Else block
        ///   1. update Module Step Id with M
        ///   2. Add failed step Ids
        ///   3. Set Timestamp
        ///   4. Publish Test Case Obj
        /// </summary>
        /// <param name="tcObj"></param>
        /// <param name="isModule"></param>
        private void IfElseActions(TestCaseDataObj tcObj, bool isModule)
        {
            UpdateModuleStepId(tcObj, isModule);
            FilterRegresTestId(tcObj);
            tcObj.SetTimestamp();
            _eventAggregator.GetEvent<TestResultEvent>().Publish(tcObj);
        }
        /// <summary>
        /// Filter Regression Test Ids to add the failed Ids to the collection
        /// </summary>
        /// <param name="tcObj"></param>
        private void FilterRegresTestId(TestCaseDataObj tcObj)
        {
            // Collect Regression Test Failed Commands, Not collect Ramp CMD at the beginning
            if (TestCaseDataShop.IsRegressionTest && ((tcObj.ExpectedResult == "P" && tcObj.PassFail != "P") || 
                (tcObj.ExpectedResult == "F" && tcObj.PassFail != "F") || (tcObj.ExpectedResult == "NA" && tcObj.PassFail != "NA")) &&
                !tcObj.AutoSimFunction.ToLower().StartsWith(TestCaseTypeKeyword.Ramp))
            {
                TestCaseDataShop.RegresTestFailedStepIds.Add(tcObj.Step);
            }
        }
        /// <summary>
        /// Update Module Step Id with mark "M"
        /// </summary>
        /// <param name="tcObj"></param>
        /// <param name="isModule"></param>
        private void UpdateModuleStepId(TestCaseDataObj tcObj, bool isModule)
        {
            if (isModule)
            {
                if (tcObj.Step == null)
                {
                    tcObj.Step = ModuleMark;
                }
                else if (!tcObj.Step.EndsWith(ModuleMark))
                {
                    tcObj.Step = tcObj.Step + ModuleMark;
                }
            }
        }
    }
}
