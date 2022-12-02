using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAH_AutoSim.TestCase
{
    public class TestCaseEvents
    {
    }

    public class TestResultEvent : PubSubEvent<TestCaseDataObj> { }
    public class TestCmdStrEvent : PubSubEvent<string> { }
    public class TestStatusMsgEvent : PubSubEvent<string> { }
    public class LogDeltaChangeEvent : PubSubEvent<int> { }
    public class RunTestCasesCompleteEvent : PubSubEvent { }
}
