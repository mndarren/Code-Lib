using NPOI.SS.Formula.Functions;
using System;

namespace AAH_AutoSim.TestCase.Models
{
    public class TestCaseDataObj
    {
		public TestCaseDataObj()
        {
            Step = "";
            Task = "";
			AppComments = "";
            ExpectedResult = "";
            PassFail = "";
            Note = "";
            AppComments = "";
            AutoSimFunction = "";
			Timestamp = "";
		}

        public void SetTimestamp()
        {
            Timestamp = DateTime.Now.ToString("HH:mm:ss");
        }

        // Test case fields
        public string? Step { get; set; }
        public string? Task { get; set; }
        public string? ExpectedResult { get; set; }
        public string? PassFail { get; set; }
        public string? Note { get; set; }
        public string? AppComments { get; set; }
        public string? AutoSimFunction { get; set; }
		public string? Timestamp { get; set; }
	}

}
