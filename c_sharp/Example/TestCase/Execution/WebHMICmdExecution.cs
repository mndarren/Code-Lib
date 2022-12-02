using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Models;
using AAH_AutoSim.TestCase.Util;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase.Execution
{
    public class WebHMICmdExecution : BaseCmdExecution
    {
        public WebHMICmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
        }

        /// <summary>
        /// Run WebHMI Test Case
        /// </summary>
        protected override void ExecuteCommand()
        {
            bool isPass = false;
            string addedComments = "";

            try
            {
                WebHMICmdObj webHMICmdObj = new WebHMICmdObj(_messageDialogService, tcObj);
                // If the command syntax error, just update and return.
                if (tcObj.PassFail == "F")
                {
                    tcObj.SetTimestamp();
                    return;
                }

                if (TestCaseDataShop.webHMISelenium == null)
                {
                    TestCaseDataShop.webHMISelenium = new WebHMISelenium();

                }
                string valueFromWeb = TestCaseDataShop.webHMISelenium.GetValueByName(webHMICmdObj.WebObjName);
                                
                isPass = webHMICmdObj.WebValue == valueFromWeb;
                addedComments = " ";

                tcObj.AppComments = tcObj.AppComments + addedComments;
            }
            catch (Exception ex)
            {
                tcObj.AppComments = tcObj.AppComments + " " + ex.Message;
                isPass = false;
            }

            tcObj.SetTimestamp();
            tcObj.PassFail = isPass ? "P" : "F";
        }
    }
}
