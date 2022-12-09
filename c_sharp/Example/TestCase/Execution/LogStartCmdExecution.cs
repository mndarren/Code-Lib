using AAH_AutoSim.Model.Constants;
using static AAH_AutoSim.Model.Communication.ObjectTypeChecker;
using AAH_AutoSim.Model.Models.ConfigModels;
using AAH_AutoSim.Model.Models;
using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Models;
using Prism.Events;
using System.Collections.Generic;
using System.Threading;
using System;
using AAH_AutoSim.TestCase.Constants;

namespace AAH_AutoSim.TestCase.Execution
{
    public class LogStartCmdExecution : BaseCmdExecution
    {
        private UtilExecution _utilExecution;
        private ExcelWriteServiceNPOI writer;

        public LogStartCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            _utilExecution = new UtilExecution(eventAggregator, messageDialogService);
            writer = new ExcelWriteServiceNPOI(_messageDialogService);
        }

		override protected void ExecuteCommand()
		{
            // Just check the Log Start command format
            new LogStartCmdObj(_messageDialogService, tcObj);
            // If the command syntax error, just update and return.
            if (tcObj.PassFail == "F")
            {
				tcObj.SetTimestamp();
                return;
            }

            // Set Log start, Reset Log Data Colletction & Log Path
            TestLogConfigObj.SetLogStart();
            TestCaseDataShop.ClearLogIdList();

            // Load Test Log Data
            new ExcelReadServiceNPOI(_messageDialogService).LoadTestLogDataObjs();

            // Start logging
            StartLogging();
            
            tcObj.PassFail = "P";
			tcObj.SetTimestamp();
		}

        private async void StartLogging()
        {
            // Run log recording in a saperated thread
            await System.Threading.Tasks.Task.Run(() => RunLogRecording());
        }

        private void RunLogRecording()
        {

            while (true)
            {
                List<string> logData2Write = new List<string>();
                // Read object value from MT Controller
                ReadPresentValue(logData2Write);
                TestCaseDataShop.testLogData[DateTime.Now.ToString()] = logData2Write;

                // Write to Log file
                if (TestCaseDataShop.testLogData.Count >= LogConstants.NumOfRecordsToWrite && !TestLogConfigObj.IsLogAdding)
                {
                    TestCaseDataShop.LogWriterTask = System.Threading.Tasks.Task.Run(() => writer.WriteLogData(TestCaseDataShop.testLogData));
                }

                // Sleep for a Log Delta
                Thread.Sleep(TestLogConfigObj.LogDelta*1000);

                if (TestLogConfigObj.IsLogOff || !TestCaseDataShop.IsRunningTestCases)
                {
                    // Write out the current data before stop logging
                    if (TestCaseDataShop.testLogData.Count > 0)
                    {
                        TestCaseDataShop.LogWriterTask = System.Threading.Tasks.Task.Run(() => writer.WriteLogData(TestCaseDataShop.testLogData));
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Read Present Value from the MT Controller by object Ids in logData2Write
        /// </summary>
        /// <param name="logData2Write"></param>
        private void ReadPresentValue(List<string> logData2Write)
        {
            string value = "";
            TestCaseDataObj tcObj;

            foreach (string objId in TestCaseDataShop.testLogObjectIds)
            {
                tcObj = new TestCaseDataObj();
                string objTypeId = objId.Split(" ")[0];
                if (IsFloat(objTypeId))
                {
                    value = _utilExecution.getFloatValueByObjectId(objId, tcObj).ToString();
                }
                else if (IsStringOrWord(objTypeId))
                {
                    value = _utilExecution.getStringValueByObjectId(objId, tcObj);
                }
                else if (IsBool(objTypeId))
                {
                    value = _utilExecution.getBoolValueByObjectId(objId, tcObj).ToString();
                }
                else if (IsULong(objTypeId))
                {
                    value = _utilExecution.getULongValueByObjectId(objId, tcObj).ToString();
                }

                if (value != null)
                {
                    logData2Write.Add(value);
                }
                else if (tcObj.AppComments != null)
                {
                    logData2Write.Add(tcObj.AppComments);
                }
                else
                {
                    logData2Write.Add("");
                }
            }

        }
    }
}
