using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAH_AutoSim.TestCase.Constants
{
    public class WebHMIConstants
    {
        // Web Driver filename and URL
        public const string WebDriverFilename = "chromedriver.exe";
        public const string WebURL = @"http://WEB:SBTAdmin!@192.168.1.42";

        // Html items
        public const string BtnIdRefresh = "refresh";
        public const string BtnIdSave = "savebutton";
        public const string IFrameIdHMI = "HMI";
        public const string IFrameIdInput = "Input";
        public const string XPathEnterPWBtn = "//img[contains(@src,'HMIlink.gif')]";
        public const string XPathEnterPWImgLink = "//img[contains(@src,'HMIedit.gif')]";
        public const string XPathInputPW = "//input[contains(@type, 'password')]";
        public const string PassKey = "636363";

    }
}
