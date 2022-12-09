using AAH_AutoSim.Modbus;
using AAH_AutoSim.Model.Constants;
using AAH_AutoSim.Model.Models;
using AAH_AutoSim.Model.Models.RainbowModels;
using AAH_AutoSim.Model.Models.ConfigModels;
using AAH_AutoSim.Rainbow;
using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.Server;
using AAH_AutoSim.Server.SystemLog.Config;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Models;
using Prism.Events;
using System;
using System.Linq;
using System.Threading;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;
using static AAH_AutoSim.Model.Communication.ObjectTypeChecker;
using System.Diagnostics;
using AAH_AutoSim.TestCase.Util;

namespace AAH_AutoSim.TestCase.Execution
{
    public class UtilExecution : BaseCmdExecution
    {
        public RainbowSingleton rainbow;

        public UtilExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            rainbow = RainbowSingleton.GetRainbow(_eventAggregator);
        }

        #region TypeCheck
        /// <summary>
        /// Check if the value is bool type.
        /// </summary>
        /// <param name="typeX"></param>
        /// <param name="valueX"></param>
        /// <param name="objTypeIdX"></param>
        /// <returns>True if it's bool type, otherwise false</returns>
        /// <exception cref="Exception"></exception>
        public bool IsBoolType(string typeX, string valueX, string objTypeIdX)
        {
            bool isBool;

            if (typeX.Equals(AutoSimFuncObjTypes.objectId) || typeX.Equals(AutoSimFuncObjTypes.objectName))
            {
                isBool = IsBool(objTypeIdX);
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.MemoryName))
            {
                if (TestCaseDataShop.memoryNamesBool.ContainsKey(valueX)) isBool = true;
                else isBool = false;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.modbusData))
            {
                // ToDo: to confirm if Modbus has bool type points
                return false;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.Value))
            {
                isBool = true;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.StringValue))
            {
                isBool = false;
            }
            else
            {
                throw new Exception("IsBoolType: Invalid Type " + typeX);
            }

            return isBool;
        }

        /// <summary>
        /// Check if the input value is ulong type
        /// </summary>
        /// <param name="typeX"></param>
        /// <param name="valueX"></param>
        /// <param name="objTypeIdX"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool IsULongType(string typeX, string valueX, string objTypeIdX)
        {
            bool isULong;

            if (typeX.Equals(AutoSimFuncObjTypes.objectId) || typeX.Equals(AutoSimFuncObjTypes.objectName))
            {
                isULong = IsULong(objTypeIdX);
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.MemoryName))
            {
                if (TestCaseDataShop.memoryNamesULong.ContainsKey(valueX)) isULong = true;
                else isULong = false;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.modbusData))
            {
                // ToDo: to confirm if Modbus has ULong type points
                return false;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.Value))
            {
                isULong = true;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.StringValue))
            {
                isULong = false;
            }
            else
            {
                throw new Exception("IsULongType: Invalid Type " + typeX);
			}

            return isULong;
        }

        /// <summary>
        /// Check if the value is string type
        /// </summary>
        /// <param name="typeX"></param>
        /// <param name="valueX"></param>
        /// <param name="objTypeIdX"></param>
        /// <returns>True if it's string, otherwise false</returns>
        /// <exception cref="Exception"></exception>
        public bool IsStringType(string typeX, string valueX, string objTypeIdX)
        {
            bool isString;

            if (typeX.Equals(AutoSimFuncObjTypes.objectId) || typeX.Equals(AutoSimFuncObjTypes.objectName))
            {
                isString = IsString(objTypeIdX);
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.MemoryName))
            {
                if (TestCaseDataShop.memoryNamesStr.ContainsKey(valueX)) isString = true;
                else isString = false;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.modbusData))
            {
                // ToDo: Modbus has string value as well, so need to update later
                return false;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.Value))
            {
                isString = false;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.StringValue))
            {
                isString = true;
            }
            else
            {
				throw new Exception("IsStringType: Invalid Type " + typeX);
			}

            return isString;
        }
        /// <summary>
        /// Check if the value is String or WORD (for Set Command)
        /// </summary>
        /// <param name="typeX"></param>
        /// <param name="valueX"></param>
        /// <param name="objTypeIdX"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool IsStringOrWordType(string typeX, string valueX, string objTypeIdX)
        {
            bool isString;

            if (typeX.Equals(AutoSimFuncObjTypes.objectId) || typeX.Equals(AutoSimFuncObjTypes.objectName))
            {
                isString = IsStringOrWord(objTypeIdX);
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.MemoryName))
            {
                if (TestCaseDataShop.memoryNamesStr.ContainsKey(valueX)) isString = true;
                else isString = false;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.modbusData))
            {
                // ToDo: Modbus has string value as well, so need to update later
                return false;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.Value))
            {
                isString = false;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.StringValue))
            {
                isString = true;
            }
            else
            {
				throw new Exception("IsStringOrWordType: Invalid Type " + typeX);
			}

            return isString;
        }

        /// <summary>
        /// Check if the value is WORD type
        /// </summary>
        /// <param name="typeX"></param>
        /// <param name="valueX"></param>
        /// <param name="objTypeIdX"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool IsWordType(string typeX, string valueX, string objTypeIdX)
        {
            bool isWord;

            if (typeX.Equals(AutoSimFuncObjTypes.objectId) || typeX.Equals(AutoSimFuncObjTypes.objectName))
            {
                isWord = IsWord(objTypeIdX);
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.MemoryName))
            {
                // Word type value could be Numeric or String
                if (TestCaseDataShop.memoryNamesULong.ContainsKey(valueX)) isWord = true;
                else if (TestCaseDataShop.memoryNamesStr.ContainsKey(valueX)) isWord = true;
                else isWord = false;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.modbusData))
            {
                // ToDo: Modbus has WORD value?
                return false;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.Value))
            {
                isWord = true;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.StringValue))
            {
                isWord = true;
            }
            else
            {
				throw new Exception("IsWordType: Invalid Type " + typeX);
			}

            return isWord;
        }

        /// <summary>
        /// Check if the pass in type value is float type
        /// </summary>
        /// <param name="typeX"></param>
        /// <param name="valueX"></param>
        /// <param name="objTypeIdX"></param>
        /// <returns></returns>
        public bool IsFloatType(string typeX, string valueX, string objTypeIdX)
        {
            bool isFloat;

            if (typeX.Equals(AutoSimFuncObjTypes.objectId) || typeX.Equals(AutoSimFuncObjTypes.objectName))
            {
                isFloat = IsFloat(objTypeIdX);
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.MemoryName))
            {
                if (TestCaseDataShop.memoryNamesFloat.ContainsKey(valueX)) isFloat = true;
                else isFloat = false;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.modbusData))
            {
                // ToDo: Modbus has string value as well, so need to update later
                return true;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.Value))
            {
                isFloat = true;
            }
            else if (typeX.Equals(AutoSimFuncObjTypes.StringValue))
            {
                isFloat = false;
            }
            else
            {
				throw new Exception("IsFloatType: Invalid Type " + typeX);
			}

            return isFloat;
        }
        #endregion TypeCheck

        #region GetValue
        /// <summary>
        /// Get the presentValue by object Id.
        /// Also, this funciton is used to get member value (default member is presentValue)
        /// SetMember CMD uses this function to get member value, incluing BOOL type.
        /// </summary>
        /// <param name="objId">Object Id</param>
        /// <param name="tcObj">Test Case Data Object</param>
        /// <returns>present Value</returns>
        public float getFloatValueByObjectId(string objId, TestCaseDataObj tcObj, string objType = AutoSimFuncObjTypes.objectId, Int32 memberId = ObjectMemberIds.PresentValueId)
        {
            RainbowInfo rainbowInfo = rainbow.ReadMember(objId, memberId);

            float value = ErrorValueFloat;
            if (rainbowInfo.Value != null)
            {
                if (rainbowInfo.Value.ToString() == "True")
                {
                    value = 1;
                }
                else if (rainbowInfo.Value.ToString() == "False")
                {
                    value = 0;
                }
                else 
                {
                    value = TCParser.ToFloat(rainbowInfo.Value.ToString());
                }
            }
                

            // Add Object Name in App Comments if object type is objectId # 53854
            if (objType.Equals(AutoSimFuncObjTypes.objectId))
            {
                tcObj.AppComments = tcObj.AppComments + " " + $"[{rainbowInfo.ObjectName}={value}]";
            }

            if (value.Equals(float.NaN))
            {
                tcObj.AppComments = tcObj.AppComments + " " + "Rainbow value = NULL";
            }
            if (rainbowInfo.ErrorText != null)
            {
                tcObj.AppComments = tcObj.AppComments + " " + rainbowInfo.ErrorText;
            }

            return value;
        }

        /// <summary>
        /// Get String value by Object Id
        /// </summary>
        /// <param name="objId">Object Id</param>
        /// <param name="tcObj">Test Case object</param>
        /// <param name="objType">Object Type</param>
        /// <returns></returns>
        public string getStringValueByObjectId(string objId, TestCaseDataObj tcObj, string objType = AutoSimFuncObjTypes.objectId)
        {
            string value = "";
            string[] statusValue;

            RainbowInfo rainbowInfo = rainbow.ReadMember(objId, ObjectMemberIds.PresentValueId);
            // The value could be "0", "1", ... if WORD type; or "real string" if STR40 type.
            value = rainbowInfo.Value?.ToString() ?? "";

            // If WORD type, we need to convert the number to string by Status Text
            if (IsWord(objId.Split(' ')[0]))
            {
                RainbowInfo rainbowInfo1 = rainbow.ReadMember(objId, ObjectMemberIds.StatusTextId);
                string valueStr = rainbowInfo1.Value?.ToString() ?? "";
                if (valueStr != "")
                {
                    statusValue = valueStr.Trim().Split("*");
                    value = statusValue[TCParser.ToInt(value)];
                }
            }

            if (objType.Equals(AutoSimFuncObjTypes.objectId))
            {
                tcObj.AppComments = tcObj.AppComments + " " + $"[{rainbowInfo.ObjectName}={value}]";
            }

            if (value == "")
            {
                tcObj.AppComments = tcObj.AppComments + " " + "Rainbow value = NULL";
            }
            if (rainbowInfo.ErrorText != null)
            {
                tcObj.AppComments = tcObj.AppComments + " " + rainbowInfo.ErrorText;
            }

            return value;
        }

        /// <summary>
        /// Get WORD value by Object Id
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="tcObj"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        public (int, string) getWordValueByObjectId(string objId, TestCaseDataObj tcObj, string objType = AutoSimFuncObjTypes.objectId)
        {
            string valueStr = ErrorValueStr;
            int valueInt = ErrorValueInt;
            string[] statusValue;

            RainbowInfo rainbowInfo = rainbow.ReadMember(objId, ObjectMemberIds.PresentValueId);
            valueStr = rainbowInfo.Value?.ToString() ?? "";

            // If WORD type, we need to convert the number to string by Status Text
            RainbowInfo rainbowInfo1 = rainbow.ReadMember(objId, ObjectMemberIds.StatusTextId);
            string value1 = rainbowInfo1.Value?.ToString() ?? "";
            if (value1 != "")
            {
                statusValue = value1.Trim().Split("*");
                valueInt = TCParser.ToInt(valueStr);
                valueStr = statusValue[valueInt];
            }

            if (objType.Equals(AutoSimFuncObjTypes.objectId))
            {
                tcObj.AppComments = tcObj.AppComments + $" [{rainbowInfo.ObjectName}={valueInt}]";
            }

            if (valueStr == "")
            {
                tcObj.AppComments = tcObj.AppComments + " " + "Rainbow value = NULL";
            }
            if (rainbowInfo.ErrorText != null)
            {
                tcObj.AppComments = tcObj.AppComments + " " + rainbowInfo.ErrorText;
            }

            return (valueInt, valueStr);
        }

        /// <summary>
        /// Get bool value via object Id
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="tcObj"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        public bool getBoolValueByObjectId(string objId, TestCaseDataObj tcObj, string objType = AutoSimFuncObjTypes.objectId)
        {
            RainbowInfo rainbowInfo = rainbow.ReadMember(objId, ObjectMemberIds.PresentValueId);
            bool value;
            
            if (rainbowInfo.Value != null && "NaN".Equals(rainbowInfo.Value.ToString()))
            {
                tcObj.AppComments = tcObj.AppComments + "NULL cannot be accepted as a BOOL value";
            }
            if (rainbowInfo.Value != null)
            {
                value = TCParser.ToBool(rainbowInfo.Value.ToString());
            }
            else
            {
                throw new Exception("Rainbow didn't get any value for BOOL type.");
            }

            if (objType.Equals(AutoSimFuncObjTypes.objectId))
            {
                tcObj.AppComments = tcObj.AppComments + " " + $"[{rainbowInfo.ObjectName}={value}]";
            }

            if (rainbowInfo.ErrorText != null)
            {
                tcObj.AppComments = tcObj.AppComments + " " + rainbowInfo.ErrorText;
            }
            return value;
        }

        /// <summary>
        /// Get ULONG value by Object Id
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="tcObj"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        public ulong getULongValueByObjectId(string objId, TestCaseDataObj tcObj, string objType = AutoSimFuncObjTypes.objectId)
        {
            RainbowInfo rainbowInfo = rainbow.ReadMember(objId, ObjectMemberIds.PresentValueId);

            // If NULL value, will pop up Error for BOOL type
            if (rainbowInfo.Value != null && "NaN".Equals(rainbowInfo.Value.ToString()))
            {
                tcObj.AppComments = tcObj.AppComments + "NULL cannot be accepted as a ULONG value";
            }
            ulong value = TCParser.ToUlong(rainbowInfo.Value != null ? rainbowInfo.Value.ToString() : ErrorValueULong.ToString());

            if (objType.Equals(AutoSimFuncObjTypes.objectId))
            {
                tcObj.AppComments = tcObj.AppComments + " " + $"[{rainbowInfo.ObjectName}={value}]";
            }

            if (rainbowInfo.ErrorText != null)
            {
                tcObj.AppComments = tcObj.AppComments + " " + rainbowInfo.ErrorText;
            }
            return value;
        }

        /// <summary>
        /// Get object name by object Id
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public string getNameByObjectId(string objId)
        {
            RainbowInfo rainbowInfo = rainbow.ReadMember(objId, ObjectMemberIds.PresentValueId);

            return rainbowInfo.ObjectName;
        }

        /// <summary>
        /// Get Modbus value by Address and Name
        /// </summary>
        /// <param name="mbValue">Modbus Value</param>
        /// <param name="tcObj"></param>
        /// <returns>Modbus return value</returns>
        public float getFloatMbValueByAddressName(string mbValue, TestCaseDataObj tcObj)
        {
            string[] parts = mbValue.Split(" ");

            if (parts.Length == 2)
            {
                try
                {
                    ModbusRead mbAddrName = new ModbusRead() { DeviceAddress = TCParser.ToInt(parts[0].Trim()), PointName = parts[1].Trim() };

                    ModbusSupervisor mbSupervisor = ModbusSupervisor.GetModbus(_eventAggregator, _messageDialogService);
                    ModbusInfo mbInfo = mbSupervisor.ModbusRead(mbAddrName);

                    if (mbInfo.ErrorText != null)
                    {
                        tcObj.AppComments = tcObj.AppComments + " " + mbInfo.ErrorText;
                        return ErrorValueFloat;
                    }
                    return TCParser.ToFloat(mbInfo.Value != null ? mbInfo.Value.ToString() : ErrorValueFloat.ToString());
                }
                catch (Exception ex)
                {
                    tcObj.AppComments = tcObj.AppComments + "Invalid Modbus Value: " + ex.Message;
                    return ErrorValueFloat;
                }
            }
            else
            {
                tcObj.AppComments = tcObj.AppComments + "Invalid Modbus Value";
                return ErrorValueFloat;
            }
        }

        /// <summary>
        /// Get string value by Modbus address and name
        /// </summary>
        /// <param name="mbValue"></param>
        /// <param name="tcObj"></param>
        /// <returns></returns>
        public string getStringMbValueByAddressName(string mbValue, TestCaseDataObj tcObj)
        {
            string[] parts = mbValue.Split(" ");

            if (parts.Length == 2)
            {
                try
                {
                    ModbusRead mbAddrName = new ModbusRead() { DeviceAddress = TCParser.ToInt(parts[0].Trim()), PointName = parts[1].Trim() };

                    ModbusSupervisor mbSupervisor = ModbusSupervisor.GetModbus(_eventAggregator, _messageDialogService);
                    ModbusInfo mbInfo = mbSupervisor.ModbusRead(mbAddrName);

                    if (mbInfo.ErrorText != null)
                    {
                        tcObj.AppComments = tcObj.AppComments + " " + mbInfo.ErrorText;
                        return ErrorValueStr;
                    }
                    return mbInfo.Value?.ToString() ?? "";
                }
                catch (Exception ex)
                {
                    tcObj.AppComments = tcObj.AppComments + "Invalid Modbus Value: " + ex.Message;
                    return ErrorValueStr;
                }
            }
            else
            {
                tcObj.AppComments = tcObj.AppComments + "Invalid Modbus Value";
                return ErrorValueStr;
            }
        }

        /// <summary>
        /// Get value from command type and command value by different types.
        /// </summary>
        /// <param name="cmdType">Compare Type</param>
        /// <param name="cmdValue">Compare Value</param>
        /// <param name="tcObj">Test Case Data object</param>
        /// <returns></returns>
        public float getFloatCmdValue(string cmdType, string cmdValue, TestCaseDataObj tcObj)
        {
            float value = ErrorValueFloat;

            switch (cmdType)
            {
                case AutoSimFuncObjTypes.objectName:
                    string objectId = NameIdData.RainbowNameIds[cmdValue].ObjectId;
                    value = getFloatValueByObjectId(objectId, tcObj, AutoSimFuncObjTypes.objectName);
                    break;
                case AutoSimFuncObjTypes.objectId:
                    value = getFloatValueByObjectId(cmdValue, tcObj, AutoSimFuncObjTypes.objectId);
                    break;
                case AutoSimFuncObjTypes.modbusData:
                    value = getFloatMbValueByAddressName(cmdValue, tcObj);
                    break;
                case AutoSimFuncObjTypes.MemoryName:
                    if (TestCaseDataShop.memoryNamesFloat.ContainsKey(cmdValue))
                    {
                        value = TestCaseDataShop.memoryNamesFloat[cmdValue];
                    }
                    else
                    {
                        tcObj.AppComments = tcObj.AppComments + $"{cmdValue} is not a valid FLOAT Memory Name for command {tcObj.AutoSimFunction}";
                    }
                    break;
                case AutoSimFuncObjTypes.Value:
                    value = TCParser.ToFloat(cmdValue);
                    break;
                default:
                    value = ErrorValueFloat;
                    break;
            }
            return value;

        }

        /// <summary>
        /// Get the string compare command value
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdValue"></param>
        /// <param name="tcObj"></param>
        /// <returns>string value</returns>
        public string getStringCmdValue(string cmdType, string cmdValue, TestCaseDataObj tcObj)
        {
            string value = ErrorValueStr;

            switch (cmdType)
            {
                case AutoSimFuncObjTypes.objectName:
                    string objectId = NameIdData.RainbowNameIds[cmdValue].ObjectId;
                    value = getStringValueByObjectId(objectId, tcObj, AutoSimFuncObjTypes.objectName);
                    break;
                case AutoSimFuncObjTypes.objectId:
                    value = getStringValueByObjectId(cmdValue, tcObj, AutoSimFuncObjTypes.objectId);
                    break;
                case AutoSimFuncObjTypes.modbusData:
                    value = getStringMbValueByAddressName(cmdValue, tcObj);
                    break;
                case AutoSimFuncObjTypes.MemoryName:
                    if (TestCaseDataShop.memoryNamesStr.ContainsKey(cmdValue))
                    {
                        value = TestCaseDataShop.memoryNamesStr[cmdValue];
                    }
                    else
                    {
                        tcObj.AppComments = tcObj.AppComments + $"{cmdValue} is not a valid STRING Memory Name for command {tcObj.AutoSimFunction}";
                    }
                    break;
                case AutoSimFuncObjTypes.StringValue:
                    value = cmdValue;
                    break;
                default:
                    value = ErrorValueStr;
                    break;
            }
            return value;

        }
        /// <summary>
        /// Get Value for WORD type
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdValue"></param>
        /// <param name="tcObj"></param>
        /// <returns></returns>
        public (int, string) getWordCmdValue(string cmdType, string cmdValue, TestCaseDataObj tcObj)
        {
            string valueStr = ErrorValueStr;
            int valueInt = ErrorValueInt;

            switch (cmdType)
            {
                case AutoSimFuncObjTypes.objectName:
                    string objectId = NameIdData.RainbowNameIds[cmdValue].ObjectId;
                    (valueInt, valueStr) = getWordValueByObjectId(objectId, tcObj, AutoSimFuncObjTypes.objectName);
                    break;
                case AutoSimFuncObjTypes.objectId:
                    (valueInt, valueStr) = getWordValueByObjectId(cmdValue, tcObj, AutoSimFuncObjTypes.objectId);
                    break;
                case AutoSimFuncObjTypes.modbusData:
                    valueStr = getStringMbValueByAddressName(cmdValue, tcObj);
                    break;
                case AutoSimFuncObjTypes.MemoryName:
                    if (TestCaseDataShop.memoryNamesStr.ContainsKey(cmdValue))
                    {
                        valueStr = TestCaseDataShop.memoryNamesStr[cmdValue];
                    }
                    else if (TestCaseDataShop.memoryNamesULong.ContainsKey(cmdValue))
                    {
                        valueInt = Convert.ToInt32(TestCaseDataShop.memoryNamesULong[cmdValue]);
                    }
                    else
                    {
                        tcObj.AppComments = tcObj.AppComments + $"{cmdValue} is not a valid STRING Memory Name for command {tcObj.AutoSimFunction}";
                    }
                    break;
                case AutoSimFuncObjTypes.StringValue:
                    valueStr = cmdValue;
                    break;
                case AutoSimFuncObjTypes.Value:
                    valueInt = TCParser.ToInt(cmdValue);
                    break;
                default:
                    valueStr = ErrorValueStr;
                    break;
            }
            return (valueInt, valueStr);

        }

        /// <summary>
        /// Get bool type compare value.
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdValue"></param>
        /// <param name="tcObj"></param>
        /// <returns>bool value</returns>
        /// <exception cref="Exception"></exception>
        public bool getBoolCmdValue(string cmdType, string cmdValue, TestCaseDataObj tcObj)
        {
            bool value = false;

            switch (cmdType)
            {
                case AutoSimFuncObjTypes.objectName:
                    string objectId = NameIdData.RainbowNameIds[cmdValue].ObjectId;
                    value = getBoolValueByObjectId(objectId, tcObj, AutoSimFuncObjTypes.objectName);
                    break;
                case AutoSimFuncObjTypes.objectId:
                    value = getBoolValueByObjectId(cmdValue, tcObj, AutoSimFuncObjTypes.objectId);
                    break;
                case AutoSimFuncObjTypes.modbusData:
                    //value = getStringMbValueByAddressName(cmdValue, tcObj);
                    // ToDo to comfirm if Modbus has bool value
                    throw new Exception("Invalid type: Modbus is not a boolean type");
                case AutoSimFuncObjTypes.MemoryName:
                    if (TestCaseDataShop.memoryNamesBool.ContainsKey(cmdValue))
                    {
                        value = TestCaseDataShop.memoryNamesBool[cmdValue];
                    }
                    else
                    {
                        tcObj.AppComments = tcObj.AppComments + $"{cmdValue} is not a valid BOOL Memory Name for command {tcObj.AutoSimFunction}";
                    }
                    break;
                case AutoSimFuncObjTypes.StringValue:
                    throw new Exception("Invalid type: StringValue is not a boolean type");
                case AutoSimFuncObjTypes.Value:
                    if ("0".Equals(cmdValue)) value = false;
                    else value = true;
                    break;
                default:
                    throw new Exception("Invalid type: {cmdType} is not boolean type");
            }
            return value;
        }

        /// <summary>
        /// Reads Configuration String from controller 
        /// </summary>
        /// <returns>Configuration String if succeeded to read it; otherwise returns empty string</returns>
        public string getDeviceConfigurationString(TestCaseDataObj tcObj)
        {
            if (!rainbow.MTConnected)
            {
                tcObj.AppComments = tcObj.AppComments + "Connection error reading Configuration String.";
                return "";
            }

            // Read Configuration String from the device using ConfigValueObjID defined in IO Config DPSA json file
            // Returns 15-charachters string
            RainbowInfo info1 = rainbow.ReadMember(ConfigData.IOData.ConfigValueObjID[0], ObjectMemberIds.PresentValueId);
            RainbowInfo info2 = rainbow.ReadMember(ConfigData.IOData.ConfigValueObjID[1], ObjectMemberIds.PresentValueId);

            if (info1 == null || info1.Error == true)
            {
                // Report error
                tcObj.AppComments = tcObj.AppComments + "Error reading Configuration String for " + ConfigData.IOData.ConfigValueObjID[0];
                return "";
            }

            if (info2 == null || info2.Error == true)
            {
                // Report error
                tcObj.AppComments = tcObj.AppComments + "Error reading Configuration String for " + ConfigData.IOData.ConfigValueObjID[1];
                return "";
            }

            return (string)info1.Value + (string)info2.Value;
        }

        /// <summary>
        /// Get ULong Command Value
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdValue"></param>
        /// <param name="tcObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ulong getULongCmdValue(string cmdType, string cmdValue, TestCaseDataObj tcObj)
        {
            ulong value = ErrorValueULong;

            switch (cmdType)
            {
                case AutoSimFuncObjTypes.objectName:
                    string objectId = NameIdData.RainbowNameIds[cmdValue].ObjectId;
                    value = getULongValueByObjectId(objectId, tcObj, AutoSimFuncObjTypes.objectName);
                    break;
                case AutoSimFuncObjTypes.objectId:
                    value = getULongValueByObjectId(cmdValue, tcObj, AutoSimFuncObjTypes.objectId);
                    break;
                case AutoSimFuncObjTypes.modbusData:
                    //value = getStringMbValueByAddressName(cmdValue, tcObj);
                    // ToDo to comfirm if Modbus has ULong value
                    throw new Exception("Modbus vs ULong type?");
                case AutoSimFuncObjTypes.MemoryName:
                    if (TestCaseDataShop.memoryNamesULong.ContainsKey(cmdValue))
                    {
                        value = TestCaseDataShop.memoryNamesULong[cmdValue];
                    }
                    else
                    {
                        tcObj.AppComments = tcObj.AppComments + $"{cmdValue} is not a valid ULONG Memory Name for command {tcObj.AutoSimFunction}";
                    }
                    break;
                case AutoSimFuncObjTypes.StringValue:
                    tcObj.AppComments = tcObj.AppComments + "The StringValue is not a ULONG type";
                    break;
                case AutoSimFuncObjTypes.Value:
                    value = TCParser.ToUlong(cmdValue);
                    break;
                default:
                    tcObj.AppComments = tcObj.AppComments + "Invalid type, not a ulong type";
                    break;
            }
            return value;

        }
        #endregion GetValue
        #region SetValue
        /// <summary>
        /// Set value to object by objectId with FLOAT type parameter
        /// Also, this funciton is used to set member value. (default member is presentValue)
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="rightValue"></param>
        /// <param name="tcObj"></param>
        /// <returns>True if set value successfully; otherwise False</returns>
        public bool setValueByObjectId(string objectId, float rightValue, TestCaseDataObj tcObj, string objType, Int32 memberId = ObjectMemberIds.PresentValueId)
        {
            bool isPassed = false;
            try
            {
                RainbowInfo rainbowInfo = rainbow.WriteMember(objectId, memberId, rightValue);
                if (rainbowInfo.ErrorText != null)
                {
                    tcObj.AppComments = tcObj.AppComments + " " + rainbowInfo.ErrorText;
                    if (rainbowInfo.ErrorText.ToLower().Contains(SetConstants.ErrorTypes.NotExist))
                    {
                        return false;
                    }
                    if (rainbowInfo.ErrorText.ToLower().Contains(SetConstants.ErrorTypes.OutOfRange))
                    {
                        var unitLowObj = rainbow.ReadMember(objectId, ObjectMemberIds.EngUnitLow).Value;
                        var unitHighObj = rainbow.ReadMember(objectId, ObjectMemberIds.EngUnitHigh).Value;

                        if (unitLowObj != null && unitHighObj != null)
                        {
                            tcObj.AppComments = tcObj.AppComments + $"[{unitLowObj.ToString()}, {unitHighObj.ToString()}]";
                        }
                        return false;
                    }
                }

                float readValue = getFloatValueByObjectId(objectId, tcObj, objType, memberId);
                isPassed = Math.Abs(readValue - rightValue) < Epsilon;

                return isPassed;
            }
            catch (Exception ex)
            {
                tcObj.AppComments = tcObj.AppComments + " setValueByObjectId Exception: " + ex.Message;
            }
            return isPassed;
        }

        /// <summary>
        /// Set value to object by objectId with ULONG type parameter
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="rightValue"></param>
        /// <param name="tcObj"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        public bool setValueByObjectId(string objectId, ulong rightValue, TestCaseDataObj tcObj, string objType)
        {
            bool isPassed = false;
            try
            {
                RainbowInfo rainbowInfo = rainbow.WriteMember(objectId, ObjectMemberIds.PresentValueId, rightValue);
                if (rainbowInfo.ErrorText != null)
                {
                    tcObj.AppComments = tcObj.AppComments + " " + rainbowInfo.ErrorText;
                    if (rainbowInfo.ErrorText.ToLower().Contains(SetConstants.ErrorTypes.NotExist))
                    {
                        return false;
                    }
                    if (rainbowInfo.ErrorText.ToLower().Contains(SetConstants.ErrorTypes.OutOfRange))
                    {
                        var unitHighObj = rainbow.ReadMember(objectId, ObjectMemberIds.NumberOfStates).Value;
                        if (unitHighObj != null)
                        {
                            tcObj.AppComments = tcObj.AppComments + $"[0, {unitHighObj.ToString()}]";
                        }
                        return false;
                    }
                }

                ulong readValue = getULongValueByObjectId(objectId, tcObj, objType);
                isPassed = readValue == rightValue;

                return isPassed;
            }
            catch (Exception ex)
            {
				tcObj.AppComments = tcObj.AppComments + " setValueByObjectId Exception: " + ex.Message;
			}
            return isPassed;
        }

        /// <summary>
        /// Override Set Value By Object Id with STRING type parameter
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="rightValue"></param>
        /// <param name="tcObj"></param>
        /// <param name="objType"></param>
        /// <returns>True if passed, otherwise false</returns>
        public bool setValueByObjectId(string objectId, string rightValue, TestCaseDataObj tcObj, string objType)
        {
            bool isPassed = false;
            RainbowInfo rainbowInfo = null;

            try
            {
                // If WORD type, we need to convert the number to string by Status Text
                if (IsWord(objectId.Split(' ')[0]))
                {
                    RainbowInfo rainbowInfo1 = rainbow.ReadMember(objectId, ObjectMemberIds.StatusTextId);
                    string value1 = rainbowInfo1.Value?.ToString() ?? "";
                    if (value1 != "")
                    {
                        string[] states = value1.Trim().Split("*");
                        // to lower case the whole array
                        states = states.Select(s => s.ToLowerInvariant()).ToArray();
                        int valueInt = Array.IndexOf(states, rightValue.ToLower());
                        rainbowInfo = rainbow.WriteMember(objectId, ObjectMemberIds.PresentValueId, valueInt);
                    }
                }
                else  // if STR40 type
                {
                    rainbowInfo = rainbow.WriteMember(objectId, ObjectMemberIds.PresentValueId, rightValue);
                }

                if (rainbowInfo.ErrorText != null)
                {
                    tcObj.AppComments = tcObj.AppComments + " " + rainbowInfo.ErrorText;
                    if (rainbowInfo.ErrorText.ToLower().Contains(SetConstants.ErrorTypes.NotExist))
                    {
                        return false;
                    }
                }

                string readValue = getStringValueByObjectId(objectId, tcObj, objType);
                isPassed = readValue.ToLower().Equals(rightValue.ToLower());

                return isPassed;
            }
            catch (Exception ex)
            {
				tcObj.AppComments = tcObj.AppComments + " setValueByObjectId Exception: " + ex.Message;
			}
            return isPassed;
        }

        /// <summary>
        /// Override Set Value by Object Id with BOOL type parameter
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="rightValue"></param>
        /// <param name="tcObj"></param>
        /// <param name="objType"></param>
        /// <returns>True if passed, otherwise false</returns>
        public bool setValueByObjectId(string objectId, bool rightValue, TestCaseDataObj tcObj, string objType)
        {
            bool isPassed = false;
            try
            {
                RainbowInfo rainbowInfo = rainbow.WriteMember(objectId, ObjectMemberIds.PresentValueId, rightValue);
                if (rainbowInfo.ErrorText != null)
                {
                    tcObj.AppComments = tcObj.AppComments + " " + rainbowInfo.ErrorText;
                    if (rainbowInfo.ErrorText.ToLower().Contains(SetConstants.ErrorTypes.NotExist))
                    {
                        return false;
                    }
                }

                bool readValue = getBoolValueByObjectId(objectId, tcObj, objType);
                isPassed = readValue.Equals(rightValue);

                return isPassed;
            }
            catch (Exception ex)
            {
				tcObj.AppComments = tcObj.AppComments + " setValueByObjectId Exception: " + ex.Message;
			}
            return isPassed;
        }

        /// <summary>
        /// Set Modbus Value by FLOAT type parameter
        /// </summary>
        /// <param name="modbusAddrA"></param>
        /// <param name="modbusNameA"></param>
        /// <param name="tcObj"></param>
        /// <returns>True if set successfully; otherwise False</returns>
        public bool setMbValueByAddressName(int modbusAddrA, string modbusNameA, float writeValue, TestCaseDataObj tcObj)
        {
            bool isPassed = false;
            try
            {
                // Todo ModbusSet() function needs to be fixed
                ModbusSet modbusSet = new ModbusSet() { DeviceAddress = modbusAddrA, PointName = modbusNameA, ValueToWrite = Convert.ToInt32(writeValue) };
                ModbusSupervisor modbusSupervisor = ModbusSupervisor.GetModbus(_eventAggregator, _messageDialogService);
                ModbusInfo modbusInfoSet = modbusSupervisor.ModbusSet(modbusSet);
                if (modbusInfoSet.ErrorText != null)
                {
                    tcObj.AppComments = tcObj.AppComments + " " + modbusInfoSet.ErrorText;
                    return false;
                }

                float mdValueRead = getFloatMbValueByAddressName($"{modbusAddrA} {modbusNameA}", tcObj);

                isPassed = Math.Abs(Convert.ToSingle(modbusInfoSet.Value) - mdValueRead) < Epsilon;

            }
            catch (Exception ex)
            {
				tcObj.AppComments = tcObj.AppComments + " setMbValueByAddressName Exception: " + ex.Message;
			}
            return isPassed;
        }

        /// <summary>
        /// Todo
        /// </summary>
        /// <param name="modbusAddrA"></param>
        /// <param name="modbusNameA"></param>
        /// <param name="writeValue"></param>
        /// <param name="tcObj"></param>
        /// <returns></returns>
        public bool setMbValueByAddressName(int modbusAddrA, string modbusNameA, string writeValue, TestCaseDataObj tcObj)
        {
            bool isPassed = false;
            try
            {
                // Todo ModbusSet() function needs to be fixed
                ModbusSet modbusSet = new ModbusSet() { DeviceAddress = modbusAddrA, PointName = modbusNameA, ValueToWrite = TCParser.ToInt(writeValue) };
                ModbusSupervisor modbusSupervisor = ModbusSupervisor.GetModbus(_eventAggregator, _messageDialogService);
                ModbusInfo modbusInfoSet = modbusSupervisor.ModbusSet(modbusSet);
                if (modbusInfoSet.ErrorText != null)
                {
                    tcObj.AppComments = tcObj.AppComments + " " + modbusInfoSet.ErrorText;
                    return false;
                }

                float mdValueRead = getFloatMbValueByAddressName($"{modbusAddrA} {modbusNameA}", tcObj);

                isPassed = Math.Abs(Convert.ToSingle(modbusInfoSet.Value) - mdValueRead) < Epsilon;

            }
            catch (Exception ex)
            {
				tcObj.AppComments = tcObj.AppComments + " setMbValueByAddressName Exception: " + ex.Message;
			}
            return isPassed;
        }

        /// <summary>
        /// Set Value by Object Id and Member Id
        /// </summary>
        /// <param name="objectId">Object Id</param>
        /// <param name="memberId">Member Id</param>
        /// <param name="rightValue">The value to set</param>
        /// <param name="tcObj">Used to update AppComments</param>
        /// <returns></returns>
        public bool setValueByObjMemberId(string objectId, Int32 memberId, bool rightValue, TestCaseDataObj tcObj)
        {
            bool isPassed = false;

            try
            {
                RainbowInfo rainbowInfo = rainbow.WriteMember(objectId, memberId, rightValue);
                if (rainbowInfo.ErrorText != null)
                {
                    tcObj.AppComments = tcObj.AppComments + " " + rainbowInfo.ErrorText;
                }
                else
                {
                    // Log this message for statistics
                    AutoSimMain.SendToAutoSimMainEvent(LogFilterType.SystemEvent, $"Set Value {rightValue} to {objectId}");
                    // Hard to check the member value's type, so not to read and compare the value here
                    isPassed = true;
                }

            }
            catch (Exception ex)
            {
				tcObj.AppComments = "setValueByObjMemberId Exception: " + ex.Message;
			}
            return isPassed;
        }

        public bool setValueByObjMemberIdNoCheck(string objectId, Int32 memberId, bool rightValue, TestCaseDataObj tcObj)
        {
            bool isPassed = false;

            try
            {
                RainbowInfo rainbowInfo = rainbow.WriteMemberNoChecks(objectId, memberId, rightValue);
                if (rainbowInfo.Error)
                {
                    tcObj.AppComments = /*tcObj.AppComments + " " +*/ rainbowInfo.ErrorText;
                }
                else
                {
                    // Log this message for statistics
                    AutoSimMain.SendToAutoSimMainEvent(LogFilterType.SystemEvent, $"Set Value {rightValue} to {objectId}");
                    // Hard to check the member value's type, so not to read and compare the value here
                    isPassed = true;
                }

            }
            catch (Exception ex)
            {
				tcObj.AppComments = "setValueByObjMemberIdNoCheck Exception: " + ex.Message;
			}
            return isPassed;
        }

        /// <summary>
        /// Set Reset value to restart controller
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="rightValue"></param>
        /// <param name="tcObj"></param>
        /// <param name="objType"></param>
        /// <returns>True if passed, otherwise false</returns>
        public bool resetController(TestCaseDataObj tcObj)
        {
            // Publish PreDeviceResetEvent 
            _eventAggregator.GetEvent<PreDeviceResetEvent>().Publish();

            // Set Reset value to the ObjectMemberIds.Reset to restart Controller
            bool isPassed = setValueByObjMemberIdNoCheck(ResetObjId, ObjectMemberIds.Reset, true, tcObj);

            if (isPassed)
            {
                // After Reset wait until controller gets reconnected
                try
                {
                    Thread.Sleep(30000);
                    rainbow.ConnectMT();

                    int retries = 0;
                    for (int i = 0; i < 60 && !rainbow.MTConnected; i++)
                    {
                        if (retries != 5)
                        {
                            i = 0;                            
                            retries++;
                            Thread.Sleep(10500);
                            rainbow.ConnectMT();
                        }
                        Thread.Sleep(500);
                    }
                    if (!rainbow.MTConnected)
                    {
                        Thread.Sleep(15000);
                        rainbow.ConnectMT();
                    }                    

                    isPassed = rainbow.MTConnected;

                    if (!isPassed)
                    {
                        AutoSimMain.SendToAutoSimMainEvent(LogFilterType.SystemEvent, string.Format("Failed to reconnect to Controller "));
                    }
                }
                catch (Exception ex)
                {
                    AutoSimMain.SendToAutoSimMainEvent(LogFilterType.SystemEvent, string.Format("Exception Error at Reinitialize reconnecting to Controller  {0}", ex.Message));
					tcObj.AppComments = "resetController Exception: " + ex.Message;
				}
            }

            // Publish event no matter if controller reset succeeded or failed
            _eventAggregator.GetEvent<DeviceResetEvent>().Publish();
            AutoSimMain.SendToAutoSimMainEvent(LogFilterType.SystemEvent, string.Format("Reinitializing Controller  {0}", isPassed.ToString()));
            // Added 30 more seconds for DVT team Config CMD losing connection
            Thread.Sleep(30000);
            return isPassed;
        }
        #endregion SetValue
    }
}
