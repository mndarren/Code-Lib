using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAH_AutoSim.TestCase.Constants
{
    public class TestCaseConstants
    {
        // Excel field names
        public const string Step = "Step";
        public const string Task = "Task";
        public const string ExpectedResult = "Expected Result";
        public const string PassFail = "P/F";
        public const string Note = "Notes";
        public const string AppComments = "App Comments";
        public const string AutoSimFunction = "Auto Sim Functions";
        public const string AutoSimCellId = "autosim";

        public class TestCaseTypeKeyword
        {
            // Keep the lower case for all keywords
            public const string RunModule = "run";
            public const string Compare = "compare";
            public const string Set = "set ";  // Keey a space at the ending please
            public const string SetOOR = "setoor";
            public const string WaitUtil = "waituntil";
            public const string Save = "save";
            public const string Wait = "wait ";
            public const string LogStart = "log start";
            public const string LogOff = "log off";
            public const string LogDelta = "log delta";
            public const string LogAdd = "log add";

            public const string Config = "config";
            public const string Pause = "pause";
            public const string ExternalTest = "external test";
            public const string Reinitialize = "reinitialize";
            public const string If = "if";
			public const string Else = "else";
			public const string Endif = "endif";
			public const string Status = "status";
            public const string Ramp = "ramp";
            public const string SetMember = "setmember";
            public const string WebHMI = "webhmi";
        }

        internal class AutoSimFuncObjTypes
        {
            // keep all being lower case by ToLower() to compare
            public const string objectId = "objectid";
            public const string objectName = "objectname";
            public const string modbusData = "mbdata";
            public const string MemoryName = "memoryname";
            public const string Value = "value";
            public const string StringValue = "stringvalue";

        }

        // Worksheet Names
        public const string ResultWorksheetName = "Test Result";
        public const string TestCaseWorksheetName = "Test Case";

        // Start Column #
        public const int StepColumnNum = 3;

        // Regression Test filename
        public const string RegressionTestFilename = "TC_MT4_RebApl_Regression_Test.xlsm";
        public const string RegressionTestDirName = "TestData";

        // Error Value
        public const float ErrorValueFloat = -9999;
        public const ulong ErrorValueULong = 9999;
        public const int ErrorValueInt = -7777;
        public const string ErrorValueStr = "ERROR_STRING_VALUE";
        
        // Compare Float Value, accept difference
        public const double Epsilon = 0.000001;
        // Run Module to separate Step # between Test Case and Test Module
        public const string ModuleMark = "M";
        // Reinitialize Command: Reset Controller Object aoDiagnostic ID
        public const string ResetObjId = "0x0020 0x00000001";
        // One Second for sleep
        public const int OneSecond = 1000;
    }
}
