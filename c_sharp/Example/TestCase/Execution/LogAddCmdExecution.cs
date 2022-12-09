using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Models;
using AAH_AutoSim.Model.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAH_AutoSim.TestCase.Communication;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase.Execution
{
    public class LogAddCmdExecution : BaseCmdExecution
    {
        private UtilExecution _utilExecution; 
        
        public LogAddCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            _utilExecution = new UtilExecution(eventAggregator, messageDialogService);
        }

		override protected void ExecuteCommand()
		{
            LogAddCmdObj logAddCmdObj = new LogAddCmdObj(_messageDialogService, tcObj);
            // If the command syntax error, just update and return.
            if (tcObj.PassFail == "F")
			{
				tcObj.SetTimestamp();
                return;
            }
            // Tell Logging stop writing, but keep reading data from Controller
            TestLogConfigObj.IsLogAdding = true;
            
            // Load object Ids for Test Log if Log data collection is empty
            if (TestCaseDataShop.testLogObjectIds.Count == 0)
            {
                new ExcelReadServiceNPOI(_messageDialogService).LoadTestLogDataObjs();
            }
            // Find object Name & Add the new object Id to the file and to the IdList
            if (!TestCaseDataShop.testLogObjectIds.Contains(logAddCmdObj.objectId))
            {
                string objectName = _utilExecution.getNameByObjectId(logAddCmdObj.objectId);
                new ExcelWriteServiceNPOI(_messageDialogService).AddPoint2Log(objectName, logAddCmdObj.objectId);
                TestCaseDataShop.testLogObjectIds.Add(logAddCmdObj.objectId);
            }

            TestLogConfigObj.IsLogAdding = false;
            tcObj.PassFail = "P";
			tcObj.SetTimestamp();
		}
    }
}
