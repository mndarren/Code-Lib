using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Models;
using System;
using Xunit;
using Moq;
using AAH_AutoSim.Server.Dialogs;

namespace Tests.AAH_AutoSim.TestCase.Tests.CommunicationTests
{
    internal class xUnit_Tests : IDisposable
    {
        public xUnit_Tests()
        {
            Console.WriteLine("Inside SetUp Constructor");
        }

        public void Dispose()
        {
            Console.WriteLine("Inside CleanUp or Dispose method");
        }
    }

    
    public class TestCaseDataShopTest : IClassFixture<xUnit_Tests>
    {
        
        [Fact]
        public void ClearAndClearLogTest()
        {
            Assert.True(TestCaseDataShop.testResultDataObjs.Count == 0);
            Assert.True(TestCaseDataShop.testModuleDataObjs.Count == 0);
            Assert.True(TestCaseDataShop.testCaseDataObjs.Count == 0);
            TestCaseDataShop.testCaseDataObjs.Add(new TestCaseDataObj());
            TestCaseDataShop.testModuleDataObjs["Test1"] = TestCaseDataShop.testCaseDataObjs;
            TestCaseDataShop.testResultDataObjs.Add(new TestCaseDataObj());
            Assert.True(TestCaseDataShop.testResultDataObjs.Count == 1);
            Assert.True(TestCaseDataShop.testModuleDataObjs.Count == 1);
            Assert.True(TestCaseDataShop.testCaseDataObjs.Count == 1);
            TestCaseDataShop.Clear();
            Assert.True(TestCaseDataShop.testResultDataObjs.Count == 0);
            Assert.True(TestCaseDataShop.testModuleDataObjs.Count == 0);
            Assert.True(TestCaseDataShop.testCaseDataObjs.Count == 0);

            Assert.True(TestCaseDataShop.testLogObjectIds.Count == 0);
            TestCaseDataShop.testLogObjectIds.Add("ojbectId1");
            Assert.True(TestCaseDataShop.testLogObjectIds.Count == 1);
            TestCaseDataShop.ClearLogIdList();
            Assert.True(TestCaseDataShop.testLogObjectIds.Count == 0);
        }

        [Fact]
        public void InitialTest()
        {
            Assert.True(TestCaseDataShop.testResultDataObjs.Count == 0);
            TestCaseDataShop.testResultDataObjs.Add(new TestCaseDataObj());
            Assert.True(TestCaseDataShop.testResultDataObjs.Count == 1);
            TestCaseDataShop.Initial();
            Assert.True(TestCaseDataShop.testResultDataObjs.Count == 0);
            Assert.Equal("F", TestCaseDataShop.TestStatus);
        }

        //[Fact]
        //public void ReloadTestCasesTest()
        //{
        //    string testCaseFilePath = @"C:\path\to\Darren\TestCase.xlsx";
        //    string testModuleFilePath = @"C:\path\to\Darren\TestModule.xlsx";

        //    // Mock NPOI IO actions
        //    var mock = new Mock<ExcelReadServiceNPOI>();
        //    mock.CallBase = true;
        //    mock.Setup(x => x.LoadTestCaseDataObjs());
        //    mock.Setup(y => y.LoadTestModuleDataObjs(testModuleFilePath)).Verifiable();

        //    TestCaseDataShop.FileName = testCaseFilePath;
        //    TestCaseDataShop.ReloadTestCases(new MessageDialogService());
        //    Assert.Contains("Reload", TestCaseDataShop.LoadMsg);
        //}
    }
}
