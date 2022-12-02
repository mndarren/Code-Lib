using AAH_AutoSim.Model.Constants;
using AAH_AutoSim.Model.Models.RainbowModels;
using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Models;
using Prism.Events;
using System;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;
using static AAH_AutoSim.Model.Communication.ObjectTypeChecker;
using AAH_AutoSim.TestCase.Util;

namespace AAH_AutoSim.TestCase.Execution
{
    public class SaveCmdExecution : BaseCmdExecution
    {
        private UtilExecution _utilExecution;

        public SaveCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            _utilExecution = new UtilExecution(eventAggregator, messageDialogService);
        }

		/// <summary>
		/// Run Save Test Case
		/// </summary>
		override protected void ExecuteCommand()
		{
            bool isPassed;

            try
            {
                SaveCmdObj saveCmdObj = new SaveCmdObj(_messageDialogService, tcObj);
                // If the command syntax error, just update and return.
                if (tcObj.PassFail == "F")
				{
					tcObj.SetTimestamp();
                    return;
                }

                isPassed = getSaveCmdResult(saveCmdObj, tcObj);

                // Add App Comments
                if (!isPassed)
                {
                    tcObj.AppComments = tcObj.AppComments + " " + $"Save Value to {saveCmdObj.MemoryName}";
                }
            }
            catch (Exception ex)
            {
                isPassed = false;
				tcObj.AppComments = tcObj.AppComments + " " + ex.Message;
            }

			tcObj.PassFail = isPassed ? "P" : "F";
			tcObj.SetTimestamp();
		}

        /// <summary>
        /// Get Save Command Result.
        /// </summary>
        /// <param name="saveCmdObj">Save Command object</param>
        /// <param name="tcObj">Test Case Object</param>
        /// <returns>True if saved successfully, ohterwise False</returns>
        private bool getSaveCmdResult(SaveCmdObj saveCmdObj, TestCaseDataObj tcObj)
        {
            bool isPassed = false;
            try
            {
                switch (saveCmdObj.SaveType)
                {
                    case AutoSimFuncObjTypes.objectName:
                        string objectId = NameIdData.RainbowNameIds[saveCmdObj.SaveValue].ObjectId;
                        isPassed = saveValueByObjectId(objectId, saveCmdObj.MemoryName, tcObj, AutoSimFuncObjTypes.objectName);
                        break;
                    case AutoSimFuncObjTypes.objectId:
                        isPassed = saveValueByObjectId(saveCmdObj.SaveValue, saveCmdObj.MemoryName, tcObj, AutoSimFuncObjTypes.objectId);
                        break;
                    case AutoSimFuncObjTypes.modbusData:
                        isPassed = saveMbValueByAddressName(saveCmdObj.ModbusAddr, saveCmdObj.ModbusName, saveCmdObj.MemoryName, tcObj);
                        break;
                    case AutoSimFuncObjTypes.MemoryName:
                        // Here, the saveCmdObj.SaveValue is the memory var name
                        if (TestCaseDataShop.memoryNamesFloat.ContainsKey(saveCmdObj.SaveValue))
                        {
                            TestCaseDataShop.memoryNamesFloat[saveCmdObj.MemoryName] = TestCaseDataShop.memoryNamesFloat[saveCmdObj.SaveValue];
                            isPassed = true;
                        }
                        else if (TestCaseDataShop.memoryNamesStr.ContainsKey(saveCmdObj.SaveValue))
                        {
                            TestCaseDataShop.memoryNamesStr[saveCmdObj.MemoryName] = TestCaseDataShop.memoryNamesStr[saveCmdObj.SaveValue];
                            isPassed = true;
                        }
                        else if (TestCaseDataShop.memoryNamesBool.ContainsKey(saveCmdObj.SaveValue))
                        {
                            TestCaseDataShop.memoryNamesBool[saveCmdObj.MemoryName] = TestCaseDataShop.memoryNamesBool[saveCmdObj.SaveValue];
                            isPassed = true;
                        }
                        else if (TestCaseDataShop.memoryNamesULong.ContainsKey(saveCmdObj.SaveValue))
                        {
                            TestCaseDataShop.memoryNamesULong[saveCmdObj.MemoryName] = TestCaseDataShop.memoryNamesULong[saveCmdObj.SaveValue];
                            isPassed = true;
                        }
                        else
                        {
                            isPassed = false;
                        }
                        break;
                    case AutoSimFuncObjTypes.Value:
                        TestCaseDataShop.memoryNamesFloat[saveCmdObj.MemoryName] = TCParser.ToFloat(saveCmdObj.SaveValue);
                        if (!saveCmdObj.SaveValue.Contains(".")) // not float value
                        {
                            TestCaseDataShop.memoryNamesULong[saveCmdObj.MemoryName] = TCParser.ToUlong(saveCmdObj.SaveValue);
                        }
                        isPassed = true;
                        break;
                    case AutoSimFuncObjTypes.StringValue:
                        TestCaseDataShop.memoryNamesStr[saveCmdObj.MemoryName] = saveCmdObj.SaveValue;
                        isPassed = true;
                        break;
                    default:
                        tcObj.AppComments = tcObj.AppComments + $"{saveCmdObj.SaveType} is not allowed Function Type for Save command";
                        break;
                }
            }
            catch (Exception ex)
            {
                tcObj.AppComments = tcObj.AppComments + ex.Message;
            }
            return isPassed;
        }

        /// <summary>
        /// Get Save Modbus value to Memory Variable. Todo
        /// </summary>
        /// <param name="modbusAddr"></param>
        /// <param name="modbusName"></param>
        /// <param name="tcObj"></param>
        /// <returns></returns>
        private bool saveMbValueByAddressName(int modbusAddr, string modbusName, string memoryName, TestCaseDataObj tcObj)
        {
            bool isPassed = false;
            try
            {
                // ToDo: Add String and Bool type for Modbus protocol
                float readValue = _utilExecution.getFloatMbValueByAddressName($"{modbusAddr} {modbusName}", tcObj);
                TestCaseDataShop.memoryNamesFloat[memoryName] = readValue;
                isPassed = Math.Abs(TestCaseDataShop.memoryNamesFloat[memoryName] - readValue) < Epsilon;
                tcObj.AppComments = tcObj.AppComments + $"[{memoryName}={readValue}]";
                return isPassed;
            }
            catch (Exception ex)
            {
                tcObj.AppComments = tcObj.AppComments + " " + ex.Message;
            }
            return isPassed;
        }
        /// <summary>
        /// Save Object Value to Memory Variable
        /// </summary>
        /// <param name="objectId">Object ID</param>
        /// <param name="memoryName">Memory Name</param>
        /// <param name="tcObj">Test Case object</param>
        /// <param name="objType">Object Type</param>
        /// <returns></returns>
        private bool saveValueByObjectId(string objectId, string memoryName, TestCaseDataObj tcObj, string objType)
        {
            bool isPassed = false;
            try
            {
                string objTypeId = objectId.Split(" ")[0];
                if (IsFloat(objTypeId))
                {
                    float readValue = _utilExecution.getFloatValueByObjectId(objectId, tcObj, objType);
                    TestCaseDataShop.memoryNamesFloat[memoryName] = readValue;
                    isPassed = Math.Abs(TestCaseDataShop.memoryNamesFloat[memoryName] - readValue) < Epsilon;
                    tcObj.AppComments = tcObj.AppComments + $" [{memoryName}={readValue}]";
                }
                else if (IsStringOrWord(objTypeId))
                {
                    string readValue = _utilExecution.getStringValueByObjectId(objectId, tcObj, objType);
                    TestCaseDataShop.memoryNamesStr[memoryName] = readValue;
                    isPassed = TestCaseDataShop.memoryNamesStr[memoryName].Equals(readValue);
                    tcObj.AppComments = tcObj.AppComments + $" [{memoryName}={readValue}]";
                }
                else if (IsBool(objTypeId))
                {
                    bool readValue = _utilExecution.getBoolValueByObjectId(objectId, tcObj, objType);
                    TestCaseDataShop.memoryNamesBool[memoryName] = readValue;
                    isPassed = TestCaseDataShop.memoryNamesBool[memoryName].Equals(readValue);
                    tcObj.AppComments = tcObj.AppComments + $" [{memoryName}={readValue}]";
                }
                else if (IsULong(objTypeId))
                {
                    ulong readValue = _utilExecution.getULongValueByObjectId(objectId, tcObj, objType);
                    TestCaseDataShop.memoryNamesULong[memoryName] = readValue;
                    isPassed = TestCaseDataShop.memoryNamesULong[memoryName].Equals(readValue);
                    tcObj.AppComments = tcObj.AppComments + $" [{memoryName}={readValue}]";
                }
                else
                {
                    isPassed = false;
                }
            }
            catch (Exception ex)
            {
                tcObj.AppComments = tcObj.AppComments + " " + ex.Message;
            }
            return isPassed;
        }
    }
}
