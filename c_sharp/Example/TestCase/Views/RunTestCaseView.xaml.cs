using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AAH_AutoSim.Model.Models;
using AAH_AutoSim.TestCase.Communication;

namespace AAH_AutoSim.TestCase.Views
{
    /// <summary>
    /// Interaction logic for RunTestCaseView.xaml
    /// </summary>
    public partial class RunTestCaseView : Window
    {
        public RunTestCaseView()
        {
            InitializeComponent();
            this.Closed += CloseOff;
        }

        private void CloseOff(Object Sender, EventArgs E)
        {
            // When Run Test Case window closed, we need the test engine stop running test case
            TestCaseDataShop.IsRunTestCasesWindowClosed = true;
        }
    }
}
