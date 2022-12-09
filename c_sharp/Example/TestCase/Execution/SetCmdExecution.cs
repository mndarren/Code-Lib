using AAH_AutoSim.Modbus;
using AAH_AutoSim.Model.Constants;
using AAH_AutoSim.Model.Models;
using AAH_AutoSim.Model.Models.RainbowModels;
using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using static AAH_AutoSim.Model.Constants.ObjectType4IdName;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;
using static AAH_AutoSim.Model.Communication.ObjectTypeChecker;
using AAH_AutoSim.TestCase.Util;

namespace AAH_AutoSim.TestCase.Execution
{
    public class SetCmdExecution : BaseCmdExecution
    {
        private UtilExecution _utilExecution;

        public SetCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            _utilExecution = new UtilExecution(eventAggregator, messageDialogService);
        }

		/// <summary>
		/// Run Set Test Case
		/// </summary>
		override protected void ExecuteCommand()
		{
            Dictionary<ObjType, bool> isType = new Dictionary<ObjType, bool>() {
                { ObjType.FLOAT, false },
                { ObjType.STRING, false },
                { ObjType.BOOL, false },
                { ObjType.ULONG, false },
            };
            bool isPass = false;
            string addedComments = "";

            try
            {
                SetCmdObj setCmdObj = new SetCmdObj(_messageDialogService, tcObj);
                // If the command syntax error, just update and return.
                if (tcObj.PassFail == "F")
				{
					tcObj.SetTimestamp();
                    return;
                }

                // Check if both sides types are the same
                if (!AreBothSidesSameType(ref isType, setCmdObj))
                {
                    addedComments = "Both sides are not the same type.";
                    isPass = false;
                }
                else if (isType[ObjType.FLOAT])
                {
                    float writeValue = ErrorValueFloat;
                    if (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.Value))
                    {
                        writeValue = TCParser.ToFloat(setCmdObj.SetValueFrom);
                    }
                    else
                    {
                        writeValue = TestCaseDataShop.memoryNamesFloat[setCmdObj.SetMemNameFrom];
                    }
                    isPass = getSetCmdResult(setCmdObj, writeValue, tcObj);
                    addedComments = " " + $"Set Float Value {writeValue}";
                }
                else if (isType[ObjType.STRING])
                {

                    if (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.StringValue))
                    {
                        string writeValue = setCmdObj.SetValueFrom;
                        isPass = getSetCmdResult(setCmdObj, writeValue, tcObj);
                        addedComments = " " + $"Set String Value {writeValue}";
                    }
                    else if (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.Value))  // WORD type Enum value
                    {
                        ulong writeValue = TCParser.ToUlong(setCmdObj.SetValueFrom);
                        isPass = getSetCmdResult(setCmdObj, writeValue, tcObj);
                        addedComments = " " + $"Set WORD Value {writeValue}";
                    }
                    else  // MemoryName type
                    {
                        if (TestCaseDataShop.memoryNamesStr.ContainsKey(setCmdObj.SetMemNameFrom))
                        {
                            string writeValue = TestCaseDataShop.memoryNamesStr[setCmdObj.SetMemNameFrom];
                            isPass = getSetCmdResult(setCmdObj, writeValue, tcObj);
                            addedComments = " " + $"Set String Value {writeValue}";
                        }
                        else if (TestCaseDataShop.memoryNamesULong.ContainsKey(setCmdObj.SetMemNameFrom))  // WORD type Enum value
                        {
                            ulong writeValue = TestCaseDataShop.memoryNamesULong[setCmdObj.SetMemNameFrom];
                            isPass = getSetCmdResult(setCmdObj, writeValue, tcObj);
                            addedComments = " " + $"Set WORD Value {writeValue}";
                        }
                    }
                }
                else if (isType[ObjType.BOOL])
                {
                    bool writeValue = false;
                    bool badCMD = false;
                    if (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.Value))
                    {
                        if (setCmdObj.SetValueFrom.Equals("0")) writeValue = false;
                        else if (setCmdObj.SetValueFrom.Equals("1")) writeValue = true;
                        else
                        {
                            addedComments = $" Invalid Bool Value {setCmdObj.SetValueFrom}";
                            isPass = false;
                            badCMD = true;
                        }
                    }
                    else // MemoryName type
                    {
                        writeValue = TestCaseDataShop.memoryNamesBool[setCmdObj.SetMemNameFrom];
                    }
                    if (!badCMD)
                    {
                        isPass = getSetCmdResult(setCmdObj, writeValue, tcObj);
                        addedComments = $" Set Bool Value {writeValue}";
                    }
                }
                else if (isType[ObjType.ULONG])
                {
                    ulong writeValue = ErrorValueULong;
                    if (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.Value))
                    {
                        writeValue = TCParser.ToUlong(setCmdObj.SetValueFrom);
                    }
                    else
                    {
                        writeValue = TestCaseDataShop.memoryNamesULong[setCmdObj.SetMemNameFrom];
                    }
                    isPass = getSetCmdResult(setCmdObj, writeValue, tcObj);
                    addedComments = $" Set ULong Value {writeValue}";
                }
                else
                {
                    addedComments = "Unexpected object type";
                    isPass = false;
                }

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

        /// <summary>
        /// Check if both sides are the same type
        /// </summary>
        /// <param name="isType"></param>
        /// <param name="setCmdObj"></param>
        /// <returns></returns>
        private bool AreBothSidesSameType(ref Dictionary<ObjType, bool> isType, SetCmdObj setCmdObj)
        {
            if (_utilExecution.IsFloatType(setCmdObj.SetTypeTo.ToLower(), setCmdObj.SetValueTo, setCmdObj.ObjTypeIdA) &&
                (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.Value) || (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.MemoryName) 
                    && TestCaseDataShop.memoryNamesFloat.ContainsKey(setCmdObj.SetMemNameFrom))))
            {
                isType[ObjType.FLOAT] = true;
                return true;
            }
            // For Set CMD, we keep String type and WORD type together
            else if (_utilExecution.IsStringOrWordType(setCmdObj.SetTypeTo.ToLower(), setCmdObj.SetValueTo, setCmdObj.ObjTypeIdA))
            {
                if (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.StringValue) ||
                    (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.Value) && IsWord(setCmdObj.ObjTypeIdA)))
                {
                    isType[ObjType.STRING] = true;
                    return true;
                }
                else if (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.MemoryName) &&
                    (TestCaseDataShop.memoryNamesStr.ContainsKey(setCmdObj.SetMemNameFrom) || 
                    (TestCaseDataShop.memoryNamesULong.ContainsKey(setCmdObj.SetMemNameFrom) && IsWord(setCmdObj.ObjTypeIdA))))
                {
                    isType[ObjType.STRING] = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (_utilExecution.IsBoolType(setCmdObj.SetTypeTo.ToLower(), setCmdObj.SetValueTo, setCmdObj.ObjTypeIdA) &&
                (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.Value) || (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.MemoryName)
                    && TestCaseDataShop.memoryNamesBool.ContainsKey(setCmdObj.SetMemNameFrom))))
            {
                isType[ObjType.BOOL] = true;
                return true;
            }
            else if (_utilExecution.IsULongType(setCmdObj.SetTypeTo.ToLower(), setCmdObj.SetValueTo, setCmdObj.ObjTypeIdA) && 
                (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.Value) || (setCmdObj.SetTypeFrom.ToLower().Equals(AutoSimFuncObjTypes.MemoryName)
                    && TestCaseDataShop.memoryNamesULong.ContainsKey(setCmdObj.SetMemNameFrom))))
            {
                isType[ObjType.ULONG] = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set value to the object
        /// </summary>
        /// <param name="setCmdObj"></param>
        /// <param name="writeValue"></param>
        /// <param name="tcObj"></param>
        /// <returns>True if set value successfully, otherwise False</returns>
        private bool getSetCmdResult(SetCmdObj setCmdObj, float writeValue, TestCaseDataObj tcObj)
        {
            bool isPassed = false;
            switch (setCmdObj.SetTypeTo)
            {
                case AutoSimFuncObjTypes.objectName:
                    string objectId = NameIdData.RainbowNameIds[setCmdObj.SetValueTo].ObjectId;
                    isPassed = _utilExecution.setValueByObjectId(objectId, writeValue, tcObj, AutoSimFuncObjTypes.objectName);
                    break;
                case AutoSimFuncObjTypes.objectId:
                    isPassed = _utilExecution.setValueByObjectId(setCmdObj.SetValueTo, writeValue, tcObj, AutoSimFuncObjTypes.objectId);
                    break;
                case AutoSimFuncObjTypes.modbusData:
                    isPassed = _utilExecution.setMbValueByAddressName(setCmdObj.ModbusAddrA, setCmdObj.ModbusNameA, writeValue, tcObj);
                    break;
                default:
                    tcObj.AppComments = tcObj.AppComments + $"{setCmdObj.SetTypeTo} is not allowed Function Type for Set command";
                    break;
            }

            return isPassed;
        }

        /// <summary>
        /// Get Set Command Result by ULONG type parameter
        /// </summary>
        /// <param name="setCmdObj"></param>
        /// <param name="writeValue"></param>
        /// <param name="tcObj"></param>
        /// <returns></returns>
        private bool getSetCmdResult(SetCmdObj setCmdObj, ulong writeValue, TestCaseDataObj tcObj)
        {
            bool isPassed = false;
            switch (setCmdObj.SetTypeTo)
            {
                case AutoSimFuncObjTypes.objectName:
                    string objectId = NameIdData.RainbowNameIds[setCmdObj.SetValueTo].ObjectId;
                    isPassed = _utilExecution.setValueByObjectId(objectId, writeValue, tcObj, AutoSimFuncObjTypes.objectName);
                    break;
                case AutoSimFuncObjTypes.objectId:
                    isPassed = _utilExecution.setValueByObjectId(setCmdObj.SetValueTo, writeValue, tcObj, AutoSimFuncObjTypes.objectId);
                    break;
                case AutoSimFuncObjTypes.modbusData:
                    isPassed = _utilExecution.setMbValueByAddressName(setCmdObj.ModbusAddrA, setCmdObj.ModbusNameA, writeValue, tcObj);
                    break;
                default:
                    tcObj.AppComments = tcObj.AppComments + $"{setCmdObj.SetTypeTo} is not allowed Function Type for Set command";
                    break;
            }

            return isPassed;
        }

        /// <summary>
        /// Override Get Set Command Result with STRING type parameter
        /// </summary>
        /// <param name="setCmdObj"></param>
        /// <param name="writeValue"></param>
        /// <param name="tcObj"></param>
        /// <returns></returns>
        private bool getSetCmdResult(SetCmdObj setCmdObj, string writeValue, TestCaseDataObj tcObj)
        {
            bool isPassed = false;
            switch (setCmdObj.SetTypeTo)
            {
                case AutoSimFuncObjTypes.objectName:
                    string objectId = NameIdData.RainbowNameIds[setCmdObj.SetValueTo].ObjectId;
                    isPassed = _utilExecution.setValueByObjectId(objectId, writeValue, tcObj, AutoSimFuncObjTypes.objectName);
                    break;
                case AutoSimFuncObjTypes.objectId:
                    isPassed = _utilExecution.setValueByObjectId(setCmdObj.SetValueTo, writeValue, tcObj, AutoSimFuncObjTypes.objectId);
                    break;
                case AutoSimFuncObjTypes.modbusData:
                    isPassed = _utilExecution.setMbValueByAddressName(setCmdObj.ModbusAddrA, setCmdObj.ModbusNameA, writeValue, tcObj);
                    break;
                default:
                    tcObj.AppComments = tcObj.AppComments + $"{setCmdObj.SetTypeTo} is not allowed Function Type for Set command";
                    break;
            }

            return isPassed;
        }

        /// <summary>
        /// Override Get Set Command Result with BOOL type parameter
        /// </summary>
        /// <param name="setCmdObj"></param>
        /// <param name="writeValue"></param>
        /// <param name="tcObj"></param>
        /// <returns></returns>
        private bool getSetCmdResult(SetCmdObj setCmdObj, bool writeValue, TestCaseDataObj tcObj)
        {
            bool isPassed = false;
            switch (setCmdObj.SetTypeTo)
            {
                case AutoSimFuncObjTypes.objectName:
                    string objectId = NameIdData.RainbowNameIds[setCmdObj.SetValueTo].ObjectId;
                    isPassed = _utilExecution.setValueByObjectId(objectId, writeValue, tcObj, AutoSimFuncObjTypes.objectName);
                    break;
                case AutoSimFuncObjTypes.objectId:
                    isPassed = _utilExecution.setValueByObjectId(setCmdObj.SetValueTo, writeValue, tcObj, AutoSimFuncObjTypes.objectId);
                    break;
                case AutoSimFuncObjTypes.modbusData:
                    //isPassed = _utilExecution.setMbValueByAddressName(setCmdObj.ModbusAddrA, setCmdObj.ModbusNameA, writeValue, tcObj);
                    // Todo Modbus for bool value set
                    isPassed = false;
                    break;
                default:
                    tcObj.AppComments = tcObj.AppComments + $"{setCmdObj.SetTypeTo} is not allowed Function Type for Set command";
                    break;
            }

            return isPassed;
        }

    }
}
