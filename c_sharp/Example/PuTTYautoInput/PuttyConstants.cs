using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PuTTYautoInput
{
    public class PuttyConstants
    {
        public const string WinAppDriverUrl = "http://127.0.0.1:4723";
        public const string WinAppDriverName = "WinAppDriver";
        public const string WinAppDriverLocation = @"C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe";
        // The following 3 lines are for minimizing window
        public const int SC_MINIMIZE = 6;
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public const string PuttyId = @"C:\Users\XieZ\OneDrive - Daikin Applied Americas\Documents\Team\git.key\pri_key.ppk";
        public const string PuttyTitle = "Pageant: Enter Passphrase";
        public const string InputClassName = "Edit";
        public const string InputStr = "JoseAF2!";
    }
}
