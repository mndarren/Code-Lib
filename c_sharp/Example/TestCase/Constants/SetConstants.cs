using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAH_AutoSim.TestCase.Constants
{
    public class SetConstants
    {
        internal class ErrorTypes
        {
            // Keep all being lower case
            public const string OutOfRange = "out of range";
            public const string NotExist = "doesn't exist";
        }
        // The old versions (110, 111, 112, 113) don't have IOConfig object. Only version 114 has.
        public static List<string> OldVersions = new List<string> { "2506036100", "2506036101", "2506036102", "2506036103",
            "2506036104", "2506036105", "2506036106", "2506036107", "2506036108", "2506036109", "2506036110", "2506036111", 
            "2506036112", "2506036113" };
    }
}
