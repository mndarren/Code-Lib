using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AAH_AutoSim.Model.Models;
using AAH_AutoSim.Server.Config;
using AAH_AutoSim.TestCase.Constants;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using static System.Net.Mime.MediaTypeNames;

namespace AAH_AutoSim.TestCase.Communication
{
    public class WebHMISelenium
    {
        ChromeDriver chromeDriver;
        bool isDebug = Debugger.IsAttached;

        public WebHMISelenium()
        {
            string driverPath = Path.Combine(ConfigPath.GetConfigPath(), WebHMIConstants.WebDriverFilename);
            chromeDriver = new ChromeDriver(driverPath);
            SetUpPass();
        }
        /// <summary>
        /// Setup WebHMI Browser and Password
        /// </summary>
        private void SetUpPass()
        {
            // Open URL
            chromeDriver.Navigate().GoToUrl(WebHMIConstants.WebURL);

            // Show Web Title
            string title = chromeDriver.Title;
            if (isDebug) Debug.Write($"WebHMI Title: {title}\n");

            // Wait 7 seconds for loading web page
            Thread.Sleep(7000);

            // Click Refresh button
            IWebElement ahuElement = chromeDriver.FindElement(By.Id(WebHMIConstants.BtnIdRefresh));
            ahuElement.Click();
            if (isDebug) Debug.Write($"Click Refresh button\n");

            // Switch to iframe HMI
            chromeDriver.SwitchTo().Frame(WebHMIConstants.IFrameIdHMI);

            // Click Enter Password Button
            IList<IWebElement> buttonElements = chromeDriver.FindElements(By.XPath(WebHMIConstants.XPathEnterPWBtn));
            buttonElements[0].Click();
            if (isDebug) Debug.Write("Enter Password Button Click\n");

            Thread.Sleep(1000);

            // Click Enter PW image link
            IWebElement imgLink = chromeDriver.FindElement(By.XPath(WebHMIConstants.XPathEnterPWImgLink));
            imgLink.Click();
            if (isDebug) Debug.Write("Enter PW Image Link Click\n");

            Thread.Sleep(1000);

            // Switch to iframe Input. Before that, we must Switch to parent iframe of HMI first
            chromeDriver.SwitchTo().ParentFrame();
            chromeDriver.SwitchTo().Frame(WebHMIConstants.IFrameIdInput);
            if (isDebug) Debug.Write("Switch to input iFrame\n");

            //Input password
            IWebElement infoElement2 = chromeDriver.FindElement(By.XPath(WebHMIConstants.XPathInputPW));
            infoElement2.Clear();
            infoElement2.SendKeys(WebHMIConstants.PassKey);
            if (isDebug) Debug.Write("Input password\n");

            Thread.Sleep(1000);

            // Click Save Button
            chromeDriver.SwitchTo().ParentFrame();
            infoElement2 = chromeDriver.FindElement(By.Id(WebHMIConstants.BtnIdSave));
            infoElement2.Click();
            if (isDebug) Debug.Write("Click the Save button\n");

        }
        /// <summary>
        /// Get Object Value by Object Name in Web Page
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public string GetValueByName(string objName)
        {
            string value = TestCaseConstants.ErrorValueStr;

            // Switch to the main frame
            chromeDriver.SwitchTo().DefaultContent();
            chromeDriver.SwitchTo().Frame(WebHMIConstants.IFrameIdHMI);

            // Get the Object Name element Id
            IWebElement objNameElement = chromeDriver.FindElement(By.XPath($"//*[text()[contains(.,'{objName}')]]"));
            string elementId = objNameElement.GetAttribute("id");
            if (isDebug) Debug.WriteLine($"Object Name: {objNameElement.Text}, Element Id: {elementId}");
            
            // Calculate the Object Value element Id
            string numStr = (Convert.ToUInt16(elementId.Remove(0,1)) + 3).ToString();
            if (numStr.Length == 1) numStr = "00" + numStr;
            if (numStr.Length == 2) numStr = "0" + numStr;
            string valueId = "o" + numStr;

            // Get the Object Value
            IWebElement objValueElement = chromeDriver.FindElement(By.Id(valueId));
            value = objValueElement.Text;

            return value;
        }

        /// <summary>
        /// Quit Chrome Driver
        /// </summary>
        public void QuitDriver()
        {
            chromeDriver.Quit();
        }
    }
}
