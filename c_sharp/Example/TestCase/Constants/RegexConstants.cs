namespace AAH_AutoSim.TestCase.Constants
{
    public class RegexConstants
    {
        public const string CmpCmdPattern = @"^(?i)\s*Compare\s+([a-zA-Z_0-9]*)\s+([a-zA-Z_\s0-9]*)\s+(|=|<>|<=|<|>|>=|\+-\s*[0-9]+|)\s+(\(*.+\)*)\s*$";
        // For common Compare right value
        public const string Cmp1stCmdPattern = @"^(?i)\s*([a-zA-Z_0-9]+)\s+(.+)$";
        // For special Compare right value, (Value 5 || Value 3) or (Value > 3 &&  Value < 5)
        public const string Cmp2ndCmdPattern = @"^(?i)\s*\(\s*Value\s*[<>]*\s*([0-9]+)\s*(|\|\||&&|)\s*Value\s*[<>]*\s*([0-9]+)\s*\)\s*$";
        // For special case: Compare objectId 0x2204 0x6EA8D135 OR objectId 0x2204 0x6EA862C5 = Value 1
        public const string Cmp2IdToValuePattern = @"^(?i)\s*([a-zA-Z\s_0-9]*)\s+OR\s+([a-zA-Z_0-9]*)\s+([a-zA-Z_\s0-9]*)\s*$";
        public const string ConfigCmdPattern = @"^(?i)\s*Config\s+(.+)\s*$";
        public const string ExternalCmdPattern = @"^(?i)\s*External\s+Test\s+(.+)\s*$";
		public const string IfElseCmdPattern = @"^(?i)\s*If\s+(.+)\s*$";
		public const string PauseCmdPattern = @"^(?i)\s*Pause\s+(.+)\s*$";
        public const string RampCmdPattern = @"^(?i)\s*Ramp\s+([a-zA-Z_0-9]*)\s+(.+)\s+from\s+(.+)\s+to\s+(.+)\s+in\s+(.+)\s+Sec\s*$";
        public const string ReinitializeCmdPattern = @"^(?i)\s*Reinitialize\s*$";
        public const string SaveCmdPattern = @"^(?i)\s*Save\s+([a-zA-Z_0-9]+)\s+(.+)\s+to\s+(.+)$";
        public const string SetCmdPattern = @"^(?i)\s*Set\s+([a-zA-Z_0-9]+)\s+(.+)\s+to\s+(|Value|StringValue|MemoryName|)\s+(.+)$";
        public const string SetMemberCmdPattern = @"^(?i)\s*SetMember\s+(0x[a-zA-Z0-9]+)\s+(0x[a-zA-Z0-9]+)\s+(0x[a-zA-Z0-9]+)\s+to\s+Value\s+([0-9]+)\s*$";
        public const string SetOORCmdPattern = @"^(?i)\s*SetOOR\s+(.+)\s+to\s+(.+)\s*$";
        public const string StatusCmdPattern = @"^(?i)\s*Status\s+(.+)\s*$";
        public const string WaitCmdPattern = @"^(?i)\s*Wait\s+(.+)\s+Sec\s*(.*)$";
        public const string WaitUntilCmdPattern = @"^(?i)\s*WaitUntil\s+(.+)\s+Wait\s+(.+)\s+Sec\s*$";
        public const string WebHMICmdPattern = @"^(?i)\s*WebHMI\s+([a-zA-Z]+)\s+(.+)\s+(|=|<>|<=|<|>|>=|\+-\s*[0-9]+|)\s+(.+)\s*$";

        // Log Regex
        public const string LogStartCmdPattern = @"^(?i)\s*Log\s+Start\s*$";
        public const string LogOffCmdPattern = @"^(?i)\s*Log\s+Off\s*$";
        public const string LogDeltaCmdPattern = @"^(?i)\s*Log\s+Delta\s+(.+)\s+sec\s*$";
        public const string LogAddCmdPattern = @"^(?i)\s*Log\s+Add\s+(.+)\s*$";
    }
}
