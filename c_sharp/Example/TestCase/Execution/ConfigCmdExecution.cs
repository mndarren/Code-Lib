using AAH_AutoSim.Model.Communication;
using AAH_AutoSim.Model.Models.ConfigModels;
using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Models;
using Prism.Events;
using System;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase.Execution
{
	public class ConfigCmdExecution : BaseCmdExecution
    {
		private UtilExecution _utilExecution;

		public ConfigCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
		{
			_utilExecution = new UtilExecution(eventAggregator, messageDialogService);
		}

		private int[] ConfigurationValues = new int[AutoSimIOConfigDPSAData.ConfigCodeBkdwNum];

		override protected void ExecuteCommand()
		{
			ConfigCmdObj cmdObj = new ConfigCmdObj(_messageDialogService, tcObj);
            // If the command syntax error, just update and return.
            if (tcObj.PassFail == "F")
			{
				tcObj.SetTimestamp();
				return;
            }

            // First decode Configuration String and check if it is valid
            if (decodeConfigurationString(cmdObj._configString) == false)
			{
				updateTestStatusFail(ref tcObj);
				return;
			}

			if (setConfigurationStringValues(tcObj) == false)
			{
				updateTestStatusFail(ref tcObj);
				return;
			}

			// Check if configCode in the controller matches the config string from the command
			if (checkDeviceConfigurationString(cmdObj._configString) == false)
			{
				// If configCode in the controller was not set properly try to set it one more time
				if (setConfigurationStringValues(tcObj) == false)
				{
					updateTestStatusFail(ref tcObj);
					return;
				}
				else
				{
					// Check again if configCode in the controller matches the config string from the command
					if (checkDeviceConfigurationString(cmdObj._configString) == false)
					{
						updateTestStatusFail(ref tcObj);
						return;
					}
				}
			}

			// If we got here it means config string was set properly and the test passed 
			tcObj.PassFail = "P";
			tcObj.SetTimestamp();

			// TODO: we might need to publish this event when config command completed successfully,
			// but for now nobody subscribes to this event
			// _eventAggregator.GetEvent<ConfigUpdateEvent>().Publish();
		}

		// Converts configuration string into array of indexes
		private bool decodeConfigurationString(string configString)
		{
			char[] charArr = configString.ToCharArray();
			char ch;

			if (charArr.Length != AutoSimIOConfigDPSAData.ConfigCodeBkdwNum)
			{
                tcObj.AppComments = tcObj.AppComments + "Configuration String length error";
				return false;
			}

			for (int i = 0; i < AutoSimIOConfigDPSAData.ConfigCodeBkdwNum; i++)
			{
				ch = charArr[i];

				// Check ascii value of each character
				if (ch >= '0' && ch <= '9')
				{
					ConfigurationValues[i] = (int)ch - '0';
				}
				else if (ch >= 'A' && ch <= 'Z')
				{
					ConfigurationValues[i] = (int)ch - 'A' + 10;
				}
				else
				{
                    tcObj.AppComments = tcObj.AppComments + "Unknow charachter in Configuration String: " + ch;
					return false;
				}
				// Check if out of range
				if (ConfigurationValues[i] > ConfigData.IOData.IOConfigRange[i])
				{
                    tcObj.AppComments = tcObj.AppComments + $" The {i+1}th digit is Out Of Range [0, {ConfigData.IOData.IOConfigRange[i]}]";
                    return false;
                }
			}
			return true;
		}

		// Sets configuration string values into corresponding object ID's
		private bool setConfigurationStringValues(TestCaseDataObj tcObj)
		{
			bool retVal = true;
			int setValue = 0;
			int count = AutoSimIOConfigDPSAData.ConfigCodeBkdwNum;
			if (!string.IsNullOrEmpty(ControllerDataShop.ControllerAppVersion) && SetConstants.OldVersions.Contains(ControllerDataShop.ControllerAppVersion))
			{
				// If the version is older than 114, skip the 30th digit
				count -= 1;
			}
			for (int i = 0; i < count; i++)
			{
				ConfigCodeBkdwObject obj = ConfigData.IOData.ConfigCodeBkdw[i];

				// Check if the value for this index is not out of Values array capacity 
				if (ConfigurationValues[i] < obj.Values.Capacity)
				{
					// DPSA configuration defines DerivedConfigCode for MaxHeatRise and UnitSize.
					// The config value for those objects needs to be combined into a single value.
					// Example:
					// "Name": "MaxHeatRise", "Value": "(MaxHeatRise_x00 * 100) + (MaxHeatRise_0x0 * 10) + (MaxHeatRise_00x)", "Type": "int"
					// "Name": "UnitSize", "Value": "(UnitSize_x00 * 100) + (UnitSize_0x0 * 10) + (UnitSize_00x)", "Type": "int"
					if ((obj.Type == "int") && (ConfigurationValues[i+2] < obj.Values.Capacity) && 
						(obj.ObjectId == ConfigData.IOData.ConfigCodeBkdw[i+1].ObjectId))
					{
						// calculate combined value
						setValue = (ConfigurationValues[i] * 100) + (ConfigurationValues[i+1] * 10) + ConfigurationValues[i+2];
						// skip the next two configuration string positions because we already processed them
						i += 2;
					}
					else
					{
						setValue = ConfigurationValues[i];
					}
					retVal = _utilExecution.setValueByObjectId(obj.ObjectId, setValue, tcObj, AutoSimFuncObjTypes.objectId);
					if (retVal == false)
					{
                        tcObj.AppComments = tcObj.AppComments + $" Failed to set value for ObjectId {obj.ObjectId}";
					}
				}
				else
				{
                    tcObj.AppComments = tcObj.AppComments + $"Configuration String Value for {obj.Name} is our of range";
					retVal = false;
				}

				// If one of the object ID's can't be set don't continue, just fail the test. 
				if (retVal == false)
				{
					break;
				}
			}

			// Reset controller to update Config Code in the controller (no matter if all values were set properly or nor)
			retVal = _utilExecution.resetController(tcObj);
			return retVal;
		}

		/// <summary>
		/// Checks if the Configuration String returned by controller is the same as the string from the command
		/// </summary>
		/// <param name="configString"></param>
		/// <returns>True if the value is the same; otherwise False</returns>
		private bool checkDeviceConfigurationString(string configString)
		{
			string deviceConfigString = _utilExecution.getDeviceConfigurationString(tcObj);

			if (deviceConfigString.Length == 0)
			{
				return false;
			}
			return String.Equals(configString, deviceConfigString);
		}

		private void updateTestStatusFail(ref TestCaseDataObj tcObj)
		{
			tcObj.PassFail = "F";
			tcObj.SetTimestamp();
		}
	}
}
