using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Management;
using System.Collections.ObjectModel;

namespace PuTTYautoInput
{
    public class PuttySession
    {
        
        protected static WindowsDriver<WindowsElement> session;
        protected static Process process;

        /// <summary>
        /// Setup the WinAppDriver and Open AutoSim App
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ProductId"></param>
        /// <param name="windowTitle"></param>
        public static void ClassSetup(TestContext context, string ProductId, string windowTitle)
        {
            // First, check if there exists an instance, close existing instance
            session = SwitchWindowByTitle(windowTitle);
            // Second, Create a new session
            if (session == null)
            {
                Process.Start(ProductId);
                Thread.Sleep(TimeSpan.FromSeconds(15));
                session = SwitchWindowByTitle(windowTitle);

                // Please keep the following commented code because it's standard code to use win app driver
                // We don't use this for now because it doesn't work for pipeline for some reason
                // The current way works for pipeline and faster than standard code
                /*var appCapabilities = new AppiumOptions();
                appCapabilities.AddAdditionalCapability("app", AutoSimAppId);
                appCapabilities.AddAdditionalCapability("deviceName", "WindowsPC");
                appCapabilities.AddAdditionalCapability("platformName", "Windows");
                try
                {
                    session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities, TimeSpan.FromSeconds(15));
                }
                catch (Exception ex)
                {
                    // Third, find the instance main window if Test Runner cannot find the active window
                    // Must give some seconds to wait for app launching
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                    if (ex.Message.Contains("Cannot find active window"))
                    {
                        //session = GetSessionForCurrRunningApp("AAH_AutoSim");
                        session = SwitchWindowByTitle("AAH AutoSim");
                    }
                }*/
                
            }

            Assert.IsNotNull(session);
        }

        /// <summary>
        /// Switch to the window with the given window title
        /// </summary>
        /// <param name="winTitle"></param>
        /// <returns>App session by window title, otherwise null if cannot find it</returns>
        protected static WindowsDriver<WindowsElement> SwitchWindowByTitle(string winTitle)
        {
            SetupDriver();

            // Get the desktop session
            var desktopCapabilities = new AppiumOptions();
            desktopCapabilities.AddAdditionalCapability("app", "Root");
            var DesktopSession = new WindowsDriver<WindowsElement>(new Uri(PuttyConstants.WinAppDriverUrl), desktopCapabilities);

            // If cannot find the existing instance, return null
            IWebElement winWebElement;
            try
            {
                winWebElement = DesktopSession.FindElementByName(winTitle);
                String winHandleStr = winWebElement.GetAttribute("NativeWindowHandle");
                // Convert window handler to Hex
                int winHandleInt = int.Parse(winHandleStr);
                String winHandleHex = winHandleInt.ToString("x");
                var winCapabilities = new AppiumOptions();

                winCapabilities.AddAdditionalCapability("appTopLevelWindow", winHandleHex);
                var session = new WindowsDriver<WindowsElement>(new Uri(PuttyConstants.WinAppDriverUrl), winCapabilities);

                return session;
            }
            catch
            {
                return null;
            }

        }
        
        /// <summary>
        /// Setup WinAppDriver and then minimize it.
        /// </summary>
        private static void SetupDriver()
        {
            if (process == null)
            {
                // Run Windows Application Driver
                Process[] processes = Process.GetProcessesByName(PuttyConstants.WinAppDriverName);
                if (processes.Length > 0)
                {
                    process = processes[0];
                }
                else
                {
                    process = Process.Start(PuttyConstants.WinAppDriverLocation);
                }
                Thread.Sleep(TimeSpan.FromSeconds(1));
                // Minimize window
                PuttyConstants.ShowWindow(process.MainWindowHandle, PuttyConstants.SC_MINIMIZE);
            }
        }
        
        /// <summary>
        /// Close the application and WinAppDriver process
        /// </summary>
        protected static void ClassTearDown()
        {
            // Close the application first, because closing WinAppDriver first will auto close application sometimes
            if (session != null)
            {
                try
                {
                    session.Close();
                    session = null;
                }
                catch
                {
                    Console.WriteLine("Session has been closed already.");
                }
            }
            // Close the WinAppDriver second
            if (process != null)
            {
                try
                {
                    process.Kill();
                    process = null;
                }
                catch
                {
                    Console.WriteLine("Process has been killed already.");
                }
            }
        }

    }
}
