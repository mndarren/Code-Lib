using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Models;
using AAH_AutoSim.Model.Models;
using AAH_AutoSim.TestCase.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AAH_AutoSim.Server;
using System.IO;
using System.Windows.Diagnostics;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.Model.Constants;
using System.Windows.Shapes;
using Path = System.IO.Path;
using AAH_AutoSim.Server.Config;

namespace AAH_AutoSim.TestCase.ViewModels
{
    public class TestCaseViewModel : BindableBase
    {
        private static bool _isTestCasesFileLoaded = false;
        private static bool _isModuleFileLoaded = false;
        private static bool _isLoadingTestCases = false;
        private static bool _isEditingTestCases = false;

        private readonly IMessageDialogService _messageDialogService;
        private readonly IRegionManager _regionManager;

        private ExcelEditorView excelEditor;
        private static RunTestCaseView runTestCaseView;

        #region Public Attributes
        public bool IsTestCasesFileLoaded
        {
            get { return _isTestCasesFileLoaded; }
            set { 
                SetProperty(ref _isTestCasesFileLoaded, value);
                ((DelegateCommand)EditTestCasesCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)RunTestCasesCommand).RaiseCanExecuteChanged();
            }
        }
        public bool IsModuleFileLoaded
        {
            get { return _isModuleFileLoaded; }
            set { 
                SetProperty(ref _isModuleFileLoaded, value);
                ((DelegateCommand)EditTestCasesCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)RunTestCasesCommand).RaiseCanExecuteChanged();
            }
        }
        public bool IsLoadingTestCases
        {
            get { return _isLoadingTestCases; }
            set { 
                SetProperty(ref _isLoadingTestCases, value);
                ((DelegateCommand)EditTestCasesCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)RunTestCasesCommand).RaiseCanExecuteChanged();
            }
        }
        public bool IsEditingTestCases
        {
            get { return _isEditingTestCases; }
            set { 
                SetProperty(ref _isEditingTestCases, value);
                ((DelegateCommand)EditTestCasesCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)RunTestCasesCommand).RaiseCanExecuteChanged();
            }
        }
        public bool IsRunningTestCases
        {
            get { return TestCaseDataShop.IsRunningTestCases; }
            set { 
                SetProperty(ref TestCaseDataShop.IsRunningTestCases, value);
                ((DelegateCommand)EditTestCasesCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)RunTestCasesCommand).RaiseCanExecuteChanged();
            }
        }
        
        public string FileName
        {
            get { return TestCaseDataShop.FileName; }
            set { 
                SetProperty(ref TestCaseDataShop.FileName, value);
                ((DelegateCommand)EditTestCasesCommand).RaiseCanExecuteChanged();
            }
        }
        public IEventAggregator _eventAggregator;
        public List<string> modules = new List<string>();  // List module names
        public List<string>? commands;                     // List commands for one module

        public DelegateCommand BrowseCommand { get; private set; }
        public DelegateCommand EditTestCasesCommand { get; private set; }
        public DelegateCommand RunTestCasesCommand { get; private set; }
        public DelegateCommand RegressionTestCommand { get; private set; }
        #endregion

        //Constructor
        public TestCaseViewModel(IMessageDialogService messageDialogService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _messageDialogService = messageDialogService;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            BrowseCommand = new DelegateCommand(ExecuteBrowseCmd);
            EditTestCasesCommand = new DelegateCommand(ExecuteEditTestCasesAsync, CanExecuteEditTestCases);
            RunTestCasesCommand = new DelegateCommand(ExecuteRunTestCasesAsync, CanExecuteRunTestCases);
            RegressionTestCommand = new DelegateCommand(ExecuteRegressionTestAsync);
            _eventAggregator.GetEvent<RunTestCasesCompleteEvent>().Subscribe(SetRunningStopped, true);
        }

        private void SetRunningStopped()
        {
            IsRunningTestCases = false;
        }
        
        private bool CanExecuteEditTestCases()
        {
            return FileName != "" && !IsRunningTestCases && !IsLoadingTestCases;
        }
        private bool CanExecuteRunTestCases()
        {
            return IsModuleFileLoaded && IsTestCasesFileLoaded && !IsEditingTestCases && !IsLoadingTestCases && !IsRunningTestCases;
        }
        private void ExecuteRunTestCasesAsync()
        {
            IsRunningTestCases = true;

            // Stop editing and running window before a new run
            if (excelEditor != null) excelEditor.Close();
            if (runTestCaseView != null) runTestCaseView.Close();
            TestCaseDataShop.Initial();

            TestCaseDataShop.IsRunTestCasesWindowClosed = false;
            runTestCaseView = new RunTestCaseView();
            runTestCaseView.Show();

            _regionManager.RequestNavigate(RegionNames.ContentRegion, "Content");
        }

        private async void ExecuteRegressionTestAsync()
        {
            IsRunningTestCases = true;
            TestCaseDataShop.IsRegressionTest = true;

            // Stop editing and running window before a new run
            if (excelEditor != null) excelEditor.Close();
            if (runTestCaseView != null) runTestCaseView.Close();
            TestCaseDataShop.Clear();
            TestCaseDataShop.Initial();

            // Load Regression Test files
            FileName = Path.Combine(ConfigPath.GetConfigPath(), TestCaseConstants.RegressionTestFilename);
            TestLogConfigObj.LogPath = Path.Combine(ConfigPath.GetConfigPath(), TestLogConfigObj.LogFilename);
            await LoadTestCases();

            TestCaseDataShop.IsRunTestCasesWindowClosed = false;
            runTestCaseView = new RunTestCaseView();
            runTestCaseView.Show();

            _regionManager.RequestNavigate(RegionNames.ContentRegion, "Content");
        }

        private void ExecuteEditTestCasesAsync()
        {
            IsEditingTestCases = true;

            // Keep only one Excel Editor window opening
            if (excelEditor != null) excelEditor.Close();
            
            excelEditor = new ExcelEditorView();
            excelEditor.Show();

            IsEditingTestCases = false;
        }

        private async Task LoadTestCases()
        {
            int idx;
            string? _filename;  // only filename
            string? _pathDir;   // only path

            try
            {
                // Reset Test Case Data Shop
                TestCaseDataShop.LoadMsg = "";  // refresh Load Message for each Load action.
                TestCaseDataShop.Clear();

                idx = FileName.LastIndexOf((char)92);           // Find the last / character in the string
                _filename = FileName.Substring(idx + 1);        // Grab the file name without the path
                _pathDir = FileName.Substring(0, idx);          // Get the path without file name

                ExcelReadServiceNPOI loadFileWorker = new ExcelReadServiceNPOI(_messageDialogService);
                loadFileWorker.LoadTestCaseDataObjs();

                // Tell user Done to load test cases file
                if (!IsTestCasesFileLoaded) IsTestCasesFileLoaded = true;
                TestCaseDataShop.LoadMsg += $"{FileName} Loaded.{Environment.NewLine}";

                // Start load module file
                if (TestModuleConfigObj.IsTestCaseLocation) // Test Module location radioButton selected)
                {
                    TestModuleConfigObj.ModulePath = _pathDir + "\\" + TestModuleConfigObj.ModuleFilename;
                }
                loadFileWorker.LoadTestModuleDataObjs(TestModuleConfigObj.ModulePath);

                // Updagte Log Path
                if (TestLogConfigObj.IsTestCaseLocation) // Test Log location radioButton selected)
                {
                    TestLogConfigObj.LogPath = _pathDir + "\\" + TestLogConfigObj.LogFilename;
                }

                if (!IsModuleFileLoaded) IsModuleFileLoaded = true;
                TestCaseDataShop.LoadMsg += $"{TestModuleConfigObj.ModulePath} Loaded.{Environment.NewLine}";
                await Task.FromResult(IsModuleFileLoaded);
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowAlertDialog(ex.Message, "Load Test Cases", MessageIcon.Error);
            }
        }

        private async void ExecuteBrowseCmd()
        {
            string filename = _messageDialogService.ShowOpenFile("File to Load", "excel files (*.xlsm)|*.xlsm");
            if (filename != "")
            {
                FileName = filename;

                // Stop editing and running window before a new loading
                if (excelEditor != null) excelEditor.Close();
                if (runTestCaseView != null) runTestCaseView.Close();

                IsLoadingTestCases = true;
                IsModuleFileLoaded = false;
                IsModuleFileLoaded = false;
                await LoadTestCases();
                IsLoadingTestCases = false;
                
            }
        }

    }
}
