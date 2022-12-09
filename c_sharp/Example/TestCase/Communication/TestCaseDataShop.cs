using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Models;
using AAH_AutoSim.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using AAH_AutoSim.Server.SystemLog.Constants;
using System.Threading.Tasks;

namespace AAH_AutoSim.TestCase.Communication
{
    public class TestCaseDataShop
    {
        // testCaseDataObjs will be loaded all test case from the test case Excel file
        public static ObservableCollection<TestCaseDataObj> testCaseDataObjs = new();
        // testModuleDataObjs will be loaded all test module from the test module Excel file
        public static Dictionary<string, ObservableCollection<TestCaseDataObj>> testModuleDataObjs = new();
        // testResultDataObjs contains all test result including test case and test module
        public static ObservableCollection<TestCaseDataObj> testResultDataObjs = new();
        // Test log object Ids
        public static List<string> testLogObjectIds = new List<string>();
        // Test Log <timestamp, <obj_Id, obj_Value>>
        public static Dictionary<string, List<string>> testLogData = new Dictionary<string, List<string>>();
        // Log writer task
        public static Task LogWriterTask;
        // Collect Failed Regression Test Cases
        public static List<string> RegresTestFailedStepIds;
        // WebHMI Selenium Object
        public static WebHMISelenium webHMISelenium;

        // Test case excel file
        public static string FileName = "";  // Test case Excel file path and filename
        public static bool IsRunningTestCases;
        public static bool IsRunTestCasesWindowClosed;

        // Memory Name Dictionary: contains all Memory Name types with Key and Value
        public static Dictionary<string, string> memoryNamesStr = new();
        public static Dictionary<string, float> memoryNamesFloat = new();
        public static Dictionary<string, bool> memoryNamesBool = new();
        public static Dictionary<string, ulong> memoryNamesULong = new();

        public static bool IsRegressionTest = false;
        // Test Result Status
        public static string TestStatus = "P";

        public static void Clear()
        {
            // testCaseDataObjs.Clear() does not work
            testCaseDataObjs = new();
            testModuleDataObjs = new();
            testResultDataObjs = new();
        }

        public static void ClearLogIdList()
        {
            testLogObjectIds = new List<string>();
            ClearLogData();
        }

        public static void ClearLogData()
        {
            testLogData = new Dictionary<string, List<string>>();
        }

        public static void Initial()
        {
            TestStatus = "F";
            // For each Run all Test Cases, clear the Result Data (.Clear() not work)
            // For each Run all Test Cases, clear Memory Name libraries
            testResultDataObjs = new();
            memoryNamesStr = new();
            memoryNamesFloat = new();
            memoryNamesBool = new();
            memoryNamesULong = new();
            // Add default True False values to the memoryNamesBool
            memoryNamesBool["True"] = true;
            memoryNamesBool["False"] = false;
        }

        // Property: On Failure Stop CheckBox on Run Test Case Window
        private static bool _isOnFailureStop = false;
        [ExcludeFromCodeCoverage]
        public static bool IsOnFailureStop
        {
            get { return _isOnFailureStop; }
            set
            {
                _isOnFailureStop = value;
                NotifyStaticPropertyChanged();
            }
        }

        // Property: Is Developer Test for Regression Test
        [ExcludeFromCodeCoverage]
        public static object IsDeveloperTest
        {
            get { return !UserConstants.DeveloperTest ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible; }
        }

        // Property: LoadMsg TextBox on Run Test Case Window
        private static string _loadMsg = "";
        [ExcludeFromCodeCoverage]
        public static string LoadMsg 
        {
            get { return _loadMsg; }
            set
            {
                _loadMsg = value;
                NotifyStaticPropertyChanged();
            }
        }
        public static event PropertyChangedEventHandler StaticPropertyChanged;
        [ExcludeFromCodeCoverage]
        private static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// This method will be used to reload test case file after in-app editing
        /// </summary>
        /// <param name="_messageDialogService"></param>
        [ExcludeFromCodeCoverage]
        public static void ReloadTestCases(IMessageDialogService _messageDialogService)
        {
            int idx;
            string? _pathDir;   // only path

            try
            {
                // Reset LoadMsg and DataShop
                LoadMsg = "";  // refresh Load Message for each Load action.
                Clear();

                idx = FileName.LastIndexOf((char)92);           // Find the last / character in the string
                _pathDir = FileName.Substring(0, idx);          // Get the path without file name

                ExcelReadServiceNPOI loadFileWorker = new ExcelReadServiceNPOI(_messageDialogService);
                loadFileWorker.LoadTestCaseDataObjs();

                // Tell user Done to load test cases file
                LoadMsg += $"{FileName} Reloaded.{Environment.NewLine}";

                // Start load module file
                string modulePath;
                if (!TestModuleConfigObj.IsTestCaseLocation) // module location radioButton selected)
                {
                    modulePath = TestModuleConfigObj.ModulePath;
                }
                else
                {
                    modulePath = _pathDir + "\\" + TestModuleConfigObj.ModuleFilename;
                }
                loadFileWorker.LoadTestModuleDataObjs(modulePath);

                LoadMsg += $"{modulePath} Reloaded.{Environment.NewLine}";
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowAlertDialog(ex.Message, "Reload Test Cases", MessageIcon.Error);
            }
        }
    }
}
