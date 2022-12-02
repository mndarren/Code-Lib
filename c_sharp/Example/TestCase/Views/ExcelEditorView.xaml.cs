using AAH_AutoSim.TestCase.Communication;
using System;
using System.Windows;

namespace AAH_AutoSim.TestCase.Views
{
    /// <summary>
    /// Interaction logic for ExcelEditorView.xaml
    /// </summary>
    public partial class ExcelEditorView : Window
    {

        public ExcelEditorView()
        {
            InitializeComponent();
            this.Closed += CloseOff;
        }
        private void CloseOff(Object Sender, EventArgs E)
        {
            // When closing the Editor window, reload the test case files
            TestCaseDataShop.ReloadTestCases(new Server.Dialogs.MessageDialogService());
        }

    }
}
