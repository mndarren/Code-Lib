using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PuTTYautoInput
{
    [TestClass]
    public class AutoInputPutty : PuttySession
    {
        [TestMethod]
        public void InputPassAuto()
        {
            session.FindElementByClassName(PuttyConstants.InputClassName).SendKeys(PuttyConstants.InputStr);
            session.FindElementByName("OK").Click();
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // Create session
            ClassSetup(context, $@"{PuttyConstants.PuttyId}", PuttyConstants.PuttyTitle);
           
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ClassTearDown();
        }
    }
}
